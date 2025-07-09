using System;
using Common.Enum;
using UnityEngine;
using KCV;
using KCV.Strategy;
using KCV.Startup;
using Server_Common;
using Server_Models;
using Server_Controllers;
using local.models;
using local.models.battle;
using local.utils;
using local.managers;
using System.Collections.Generic;
using System.Collections;

namespace Runner
{
    /// <summary>
    /// 艦隊これくしょんのゲーム実行を管理するメインクラス
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 艦これ改 - ゲーム開始から最初の戦闘まで ===");
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
    /// ゲーム実行の設定を管理するクラス
    /// StartupDataの機能を拡張したゲーム設定クラス
    /// </summary>
    public class GameConfig
    {
        // StartupDataで管理される基本設定
        public string AdmiralName { get; set; } = "新米提督";
        public int InitialShipIndex { get; set; } = 9;
        public DifficultKind Difficulty { get; set; } = DifficultKind.OTU;
        public bool IsInherit { get; set; } = false;

        // 独自の拡張設定
        public int ResourceAmount { get; set; } = 1000;
        public int RepairKits { get; set; } = 10;
        public int DevKits { get; set; } = 10;
        public int InitialCoins { get; set; } = 5000;
        public int FleetSize { get; set; } = 6;
        public BattleFormationKinds1 Formation { get; set; } = BattleFormationKinds1.TanJuu;
        public int EquipmentCount { get; set; } = 20;

        /// <summary>
        /// GameConfigからStartupDataを作成
        /// </summary>
        /// <returns>StartupData</returns>
        public KCV.Startup.StartupData ToStartupData()
        {
            return new KCV.Startup.StartupData
            {
                AdmiralName = this.AdmiralName,
                PartnerShipID = this.InitialShipIndex,
                Difficlty = this.Difficulty,
                isInherit = this.IsInherit
            };
        }
    }

    /// <summary>
    /// ゲーム実行の全体的な流れを管理するクラス
    /// </summary>
    public class GameRunner
    {
        private readonly GameConfig _config;
        private readonly Debug_Mod _debugMod = new Debug_Mod();
        private FleetManager _fleetManager;
        private readonly BattleManager _battleManager = new BattleManager();

        public GameRunner(GameConfig config = null)
        {
            _config = config ?? new GameConfig();
        }

        /// <summary>
        /// システム/マスターデータを初期化
        /// </summary>
        private void InitializeSystem()
        {
            Console.WriteLine("  システム初期化を開始...");
            try
            {
                Console.WriteLine("  マスターデータ初期化を開始...");
                if (!AppInitializeManager.IsInitialize)
                {
                    Console.WriteLine("  Mst_DataManagerでマスターデータを読み込み中...");
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
        /// マスターデータを初期化（TrophyManager なしで実行）
        /// </summary>
        private void InitializeMasterDataManager()
        {
            Console.WriteLine("  マスターデータマネージャーを初期化中...");
            Mst_DataManager.Instance.LoadStartMaster(() =>
            {
                Console.WriteLine("  マスターデータ読み込み完了");
                Mst_DataManager.Instance.SetStartMasterData();
                App.isMasterInit = true;
            });
            // TrophyManagerの初期化をスキップし、やったことにする
            App.isTrophyInit = true;
            if (!App.isMasterInit)
            {
                throw new Exception("マスターデータの初期化がタイムアウトしました");
            }
            Console.WriteLine("  マスターデータマネージャーの初期化完了");
        }

        // 未使用の InitializeTrophyManager メソッドを削除

        /// <summary>
        /// 提督（プレイヤー）情報をまとめて初期化・セットアップ
        /// </summary>
        private void SetupAdmiralInfo()
        {
            Console.WriteLine("  提督情報の初期化を開始...");
            try
            {
                var startupData = _config.ToStartupData();
                Console.WriteLine($"  難易度: {startupData.Difficlty}");
                Console.WriteLine($"  提督名: {startupData.AdmiralName}");
                Console.WriteLine($"  初期艦娘: {FleetManager.GetShipName(startupData.PartnerShipID)} (ID: {startupData.PartnerShipID})");

                bool success = App.CreateSaveDataNInitialize(
                    startupData.AdmiralName,
                    startupData.PartnerShipID,
                    startupData.Difficlty,
                    startupData.isInherit
                );
                Console.WriteLine(success ? "  セーブデータを正常に作成しました" : "  セーブデータの作成に失敗しました");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  提督情報設定中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// ゲーム全体を実行
        /// </summary>
        public void RunGame()
        {
            Console.WriteLine("\n【ステップ1】システム/マスターデータ初期化");
            InitializeSystem();
            Console.WriteLine("\n【ステップ2】提督（プレイヤー）情報の設定");
            SetupAdmiralInfo();
            Debug_Mod.OpenMapAreaAll();
            Console.WriteLine("\n【ステップ3】基本的なゲームリソースの設定");
            SetupBasicResources();
            AddShips(GetDefaultAdditionalShips());
            AddBasicEquipments();
            Console.WriteLine("\n【ステップ4】艦隊管理システムの初期化");
            _fleetManager = new FleetManager();
            Console.WriteLine("\n【ステップ5】艦隊編成の準備");
            _fleetManager?.PrepareFleetOrganization(1, _config.FleetSize);
            Console.WriteLine("\n【ステップ6】出撃準備とマップ選択");
            SortieHelper.PrepareForSortie(4, 4);
            Console.WriteLine("\n【ステップ7】最初の戦闘実行");
            var battleResult = _battleManager.ExecuteBattle(4, 4, _config.Formation);
            if (battleResult.Success)
            {
                BattleResultDisplayer.DisplayBattleResult(battleResult.BattleResultModel);
                Console.WriteLine("  戦闘完了！");
            }
            else
            {
                Console.WriteLine($"  戦闘に失敗しました: {battleResult.ErrorMessage}");
            }
            Console.WriteLine("\n=== ゲーム開始から最初の戦闘まで完了 ===");
        }



        /// <summary>
        /// 基本的なゲームリソースを設定
        /// </summary>
        private void SetupBasicResources()
        {
            Console.WriteLine("  基本的なゲームリソースを設定中...");
            _debugMod.Add_Materials(enumMaterialCategory.Fuel, _config.ResourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Bull, _config.ResourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Steel, _config.ResourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Bauxite, _config.ResourceAmount);
            _debugMod.Add_Materials(enumMaterialCategory.Repair_Kit, _config.RepairKits);
            _debugMod.Add_Materials(enumMaterialCategory.Dev_Kit, _config.DevKits);
            Console.WriteLine($"  基本資源: 燃料{_config.ResourceAmount}, 弾薬{_config.ResourceAmount}, 鋼材{_config.ResourceAmount}, ボーキサイト{_config.ResourceAmount}");
            Console.WriteLine($"  高速修復材: {_config.RepairKits}個, 開発資材: {_config.DevKits}個");
            _debugMod.Add_Coin(_config.InitialCoins);
            Console.WriteLine($"  初期家具コイン: {_config.InitialCoins:N0}");
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
            foreach (int shipId in shipIds)
                Console.WriteLine($"    - {FleetManager.GetShipName(shipId)} (ID: {shipId})");
        }

        /// <summary>
        /// 基本装備を配備
        /// </summary>
        private void AddBasicEquipments()
        {
            Console.WriteLine("  基本装備を配備中...");
            var basicEquipments = new List<int>();
            for (int i = 0; i < _config.EquipmentCount; i++)
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
    }
}
