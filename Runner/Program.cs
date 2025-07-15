using System;
using Common.Enum;
using KCV;
using Server_Models;
using Server_Controllers;
using local.managers;
using System.Collections.Generic;
using System.Linq;

namespace Runner
{
    /// <summary>
    /// ゲーム実行を管理するメインクラス
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new GameRunner().RunGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nエラーが発生しました: {ex.Message}");
                Console.WriteLine($"スタックトレース: {ex.StackTrace}");
            }
        }
    }

    /// <summary>
    /// ゲーム実行の全体的な流れを管理するクラス
    /// </summary>
    public class GameRunner
    {
        private readonly Debug_Mod _debugMod = new Debug_Mod();
        // 艦隊編成用のOrganizeManager
        private OrganizeManager _organizeManager;
        private readonly BattleHelper _battleHelper = new BattleHelper();

        /// <summary>
        /// システム/マスタデータを初期化
        /// </summary>
        private void InitializeSystem()
        {
            Console.WriteLine("  システム初期化を開始...");
            try
            {
                Console.WriteLine("  マスタデータ初期化を開始...");
                if (!AppInitializeManager.IsInitialize)
                {
                    Console.WriteLine("  Mst_DataManagerでマスタデータを読み込み中...");
                    InitializeMasterDataManager();
                    Console.WriteLine("  Appクラスでセーブデータを初期化中...");
                    App.Initialize();
                    AppInitializeManager.IsInitialize = true;
                    Console.WriteLine("  AppInitializeManagerの初期化完了");
                }
                Console.WriteLine("  Appクラスでシステムを初期化...");
                App.InitSystems();
                Console.WriteLine("  ManagerBaseで基本マネージャーを初期化...");
                ManagerBase.initialize();
                Console.WriteLine("  システム初期化完了");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  システム初期化中にエラーが発生しました: {ex.Message}");
                Console.WriteLine($"  スタックトレース: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// マスタデータを初期化
        /// </summary>
        private void InitializeMasterDataManager()
        {
            Console.WriteLine("  マスタデータマネージャーを初期化中...");
            Mst_DataManager.Instance.LoadStartMaster(() =>
            {
                Console.WriteLine("  マスタデータ読み込み完了");
                Mst_DataManager.Instance.SetStartMasterData();
                App.isMasterInit = true;
            });
            // TrophyManagerの初期化をスキップし、やったことにする
            App.isTrophyInit = true;
            if (!App.isMasterInit)
            {
                throw new Exception("マスタデータの初期化がタイムアウトしました");
            }
            Console.WriteLine("  マスタデータマネージャーの初期化完了");
        }

        /// <summary>
        /// 提督情報をまとめて初期化・セットアップ
        /// </summary>
        private void SetupAdmiralInfo()
        {
            Console.WriteLine("  提督情報の初期化を開始...");
            Console.WriteLine($"  難易度: {DifficultKind.OTU}");
            Console.WriteLine($"  提督名: 新米提督");
            Console.WriteLine($"  初期艦娘: {BattlePrinter.GetShipName(9)} (ID: 9)");
            bool success = App.CreateSaveDataNInitialize(
                "新米提督",
                9,
                DifficultKind.OTU,
                false
            );
            Console.WriteLine(success ? "  セーブデータを正常に作成しました" : "  セーブデータの作成に失敗しました");
        }

        /// <summary>
        /// ゲーム全体を実行
        /// </summary>
        public void RunGame()
        {
            Console.WriteLine("\nシステム/マスタデータ初期化");
            InitializeSystem();
            Console.WriteLine("\n提督（プレイヤー）情報の設定");
            SetupAdmiralInfo();
            Debug_Mod.OpenMapAreaAll();
            Console.WriteLine("\n基本的なゲームリソースの設定");
            SetupBasicResources();
            AddShips(GetDefaultAdditionalShips());
            AddBasicEquipments();

            Console.WriteLine("\n艦隊編成の準備");
            // 艦隊ID: 1, 艦数: 6
            int fleetId = 1;
            int shipCount = 6;
            _organizeManager = new OrganizeManager(fleetId);
            const int MaxFleetSize = 6;
            const int StartPosition = 2; // 1番位置は初期艦娘のため2番から開始
            try
            {
                // 艦隊に艦娘を配置（2番位置から開始、1番位置は初期艦娘）
                for (int position = StartPosition; position <= Math.Min(shipCount, MaxFleetSize); position++)
                {
                    _organizeManager.ChangeOrganize(fleetId, position, position);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"艦隊編成の準備に失敗しました: {ex.Message}", ex);
            }

            Console.WriteLine("\nエリア/マップ/セルの一覧");
            var maparea = Mst_DataManager.Instance.Mst_maparea.FirstOrDefault(x => x.Value.Id == 1);
            var mapinfos = Mst_DataManager.Instance.Mst_mapinfo;
            var mapcellsAll = new Dictionary<int, Mst_mapcell2>();
            Console.WriteLine($"■ エリアID: {maparea.Value.Id}, 名称: {maparea.Value.Name}");
            var infos = mapinfos.Values.Where(x => x.Maparea_id == maparea.Value.Id).OrderBy(x => x.No).ToList();
            if (infos.Count == 0)
            {
                Console.WriteLine("  紐づくマップなし");
            }
            else
            {
                foreach (var info in infos)
                {
                    Mst_DataManager.Instance.Make_MapCell(maparea.Value.Id, info.No);
                    var relatedCells = Mst_DataManager.Instance.Mst_mapcell.Values
                        .Where(cell => cell.Maparea_id == maparea.Value.Id && cell.Mapinfo_no == info.No)
                        .OrderBy(cell => cell.No)
                        .ToList();
                    Console.WriteLine($"  マップNo: {info.No}, 名称: {info.Name} (紐づくセル数: {relatedCells.Count})");
                    foreach (var cell in relatedCells)
                    {
                        string startMark = cell.Event_1 == enumMapEventType.None ? " [START]" : "";
                        var nextCells = new List<string>();
                        var nextNos = new[] { cell.Next_no_1, cell.Next_no_2, cell.Next_no_3, cell.Next_no_4 };
                        var nextRates = (cell.Next_rate ?? "").Split(',');
                        for (int i = 0; i < nextNos.Length; i++)
                        {
                            if (nextNos[i] > 0)
                            {
                                string nextCellShort = $"{nextNos[i]}";
                                string rate = (i < nextRates.Length) ? nextRates[i] : "";
                                if (!string.IsNullOrWhiteSpace(rate)) nextCellShort += $"({rate}%)";
                                nextCells.Add(nextCellShort);
                            }
                        }
                        string nextCellInfo = nextCells.Count > 0 ? " | 次: " + string.Join(" / ", nextCells) : "";
                        Console.WriteLine($"      - セルNo: {cell.No}, イベントタイプ: {cell.Event_1}, 戦闘タイプ: {cell.Event_2}{startMark}{nextCellInfo}");
                    }
                }
            }

            var sorties = new[] { "5-1", "5-2" };
            for (int i = 0; i < sorties.Length; i++)
            {
                var mapArea = 5;
                var mapNo = i + 1;
                Console.WriteLine("\n出撃準備とマップ選択");
                Console.WriteLine($"  エリア{mapArea}のマップ{mapArea}-{mapNo}を選択");
                Console.WriteLine($"\n戦闘実行: {sorties[i]}");

                // 戦闘進行の表示と制御
                ExecuteBattleWithDisplay(mapArea, mapNo, sorties[i]);

                if (i == 0)
                {
                    var deck1 = local.managers.ManagerBase.PublicUserInfo.GetDeck(1); // 第1艦隊
                    var flagship1 = deck1?.GetFlagShip();
                    if (flagship1 != null && flagship1.IsTaiha())
                    {
                        Console.WriteLine("\n旗艦が大破したため、艦隊は帰投します。5-2戦闘はスキップされました。");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 基本的なゲームリソースを設定
        /// </summary>
        private void SetupBasicResources()
        {
            Console.WriteLine("  基本的なゲームリソースを設定中...");
            var resourceAmount = 1000;
            var repairKits = 10;
            var devKits = 10;
            var initialCoins = 5000;
            _debugMod.Add_Materials(enumMaterialCategory.Fuel, resourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Bull, resourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Steel, resourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Bauxite, resourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Repair_Kit, repairKits);
            _debugMod.Add_Materials(enumMaterialCategory.Dev_Kit, devKits);
            Console.WriteLine($"  基本資源: 燃料{resourceAmount}, 弾薬{resourceAmount}, 鋼材{resourceAmount}, ボーキサイト{resourceAmount}");
            Console.WriteLine($"  高速修復材: {repairKits}個, 開発資材: {devKits}個");
            _debugMod.Add_Coin(initialCoins);
            Console.WriteLine($"  初期家具コイン: {initialCoins:N0}");
            Console.WriteLine("  基本リソース設定完了");
        }

        /// <summary>
        /// 追加の艦娘を配備
        /// </summary>
        /// <param name="shipIds">配備する艦娘のIDリスト</param>
        private void AddShips(List<int> shipIds)
        {
            Console.WriteLine("  追加の艦娘を配備中...");
            _debugMod.Add_Ship(shipIds);
            Console.WriteLine($"  追加艦娘: {shipIds.Count}隻");
            shipIds.ForEach(shipId => Console.WriteLine($"    - {BattlePrinter.GetShipName(shipId)} (ID: {shipId})"));
        }

        /// <summary>
        /// 基本装備を配備
        /// </summary>
        private void AddBasicEquipments()
        {
            Console.WriteLine("  基本装備を配備中...");
            var equipmentCount = 20;
            var basicEquipments = new List<int>();
            for (int i = 0; i < equipmentCount; i++)
            {
                basicEquipments.Add(1);
                basicEquipments.Add(14);
            }
            _debugMod.Add_SlotItem(basicEquipments);
            Console.WriteLine($"  基本装備: {basicEquipments.Count}個");
        }

        /// <summary>
        /// デフォルトの追加艦娘を取得
        /// </summary>
        /// <returns>デフォルトの追加艦娘IDリスト</returns>
        private List<int> GetDefaultAdditionalShips() => new List<int> { 24, 175, 117, 75, 321 };

        /// <summary>
        /// 戦闘実行と進行表示を統合したメソッド
        /// </summary>
        /// <param name="mapArea">エリアID</param>
        /// <param name="mapNo">マップ番号</param>
        /// <param name="sortieId">出撃ID</param>
        private void ExecuteBattleWithDisplay(int mapArea, int mapNo, string sortieId)
        {
            // 戦闘実行開始の表示
            BattlePrinter.PrintBattleExecutionStart();
            Console.WriteLine($"戦闘開始: {sortieId} (エリア{mapArea}-{mapNo})");

            // 戦闘実行
            var battleResult = _battleHelper.ExecuteBattle(mapArea, mapNo, BattleFormationKinds1.TanJuu, true, true);

            if (battleResult.Success)
            {
                // 戦闘成功時の表示
                BattlePrinter.PrintDetailedBattleResult(battleResult.BattleResultModel);
                Console.WriteLine($"  {sortieId}戦闘完了！");
            }
            else
            {
                // 戦闘失敗時の表示
                Console.WriteLine($"  {sortieId}戦闘に失敗しました: {battleResult.ErrorMessage}");
            }
        }
    }
}
