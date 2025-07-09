using System;
using System.Collections.Generic;
using System.Linq;
using Server_Models;
using KCV.Strategy;
using Common.Enum;
using local.managers;
using local.models.battle;
using local.models;

namespace Runner
{
    /// <summary>
    /// 戦闘システムの管理を担当するクラス
    /// </summary>
    public class BattleManager
    {
        /// <summary>
        /// 戦闘を実行
        /// </summary>
        /// <param name="areaId">エリアID</param>
        /// <param name="mapNo">マップ番号</param>
        /// <param name="formation">戦闘陣形</param>
        /// <param name="executeNightBattle">夜戦を実行するかどうか</param>
        /// <returns>戦闘結果</returns>
        public BattleResult ExecuteBattle(int areaId = 2, int mapNo = 24, BattleFormationKinds1 formation = BattleFormationKinds1.TanJuu, bool executeNightBattle = false)
        {
            Console.WriteLine("  戦闘を実行中...");

            try
            {
                // マップ管理の初期化
                Console.WriteLine("  StrategyMapManagerを初期化中...");
                var strategyMapManager = new StrategyMapManager();

                Console.WriteLine($"  エリア{areaId}を選択中...");
                var sortieManager = strategyMapManager.SelectArea(areaId);
                if (sortieManager == null)
                {
                    Console.WriteLine($"  エラー: SortieManagerがnullです（エリア{areaId}）");
                    return new BattleResult { Success = false, ErrorMessage = "SortieManagerの取得に失敗" };
                }

                Console.WriteLine($"  マップ{areaId}-{mapNo}に出撃中...");

                // マップIDを計算（エリア4のマップ4は44）
                int mapId = areaId * 10 + mapNo;
                Console.WriteLine($"  実際のマップID: {mapId}");

                var sortieMapManager = sortieManager.GoSortie(1, mapId);
                if (sortieMapManager == null)
                {
                    Console.WriteLine($"  エラー: SortieMapManagerがnullです（マップ{mapNo}）");
                    return new BattleResult { Success = false, ErrorMessage = "SortieMapManagerの取得に失敗" };
                }

                Console.WriteLine($"  出撃成功！（エリア{areaId}、マップ{mapNo}）");

                // 艦隊と敵艦隊の情報を表示
                FleetManager.DisplayFleetStatus(sortieMapManager);
                FleetManager.DisplayEnemyFleetInfo(sortieMapManager);

                // 戦闘開始
                Console.WriteLine($"  戦闘開始！（陣形: {GetFormationName(formation)}）");
                var sortieBattleManager = sortieMapManager.BattleStart_Write(formation);

                // 戦闘指揮の処理
                var battleResult = ProcessBattleCommands(sortieBattleManager, executeNightBattle);

                return battleResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  戦闘実行中にエラーが発生しました: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 戦闘指揮を処理
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        /// <param name="executeNightBattle">夜戦を実行するかどうか</param>
        /// <returns>戦闘結果</returns>
        private BattleResult ProcessBattleCommands(SortieBattleManager battleManager, bool executeNightBattle = false)
        {
            Console.WriteLine("  戦闘指揮を準備中...");
            var commandPhaseModel = battleManager.GetCommandPhaseModel();

            if (commandPhaseModel == null)
            {
                Console.WriteLine("  コマンドフェーズモデルの取得に失敗しました");
                return new BattleResult { Success = false, ErrorMessage = "コマンドフェーズモデルの取得に失敗" };
            }

            Console.WriteLine("  コマンドフェーズモデルを取得しました");

            // 戦闘指揮を取得
            var presetCommands = commandPhaseModel.GetPresetCommand();
            var selectableCommands = commandPhaseModel.GetSelectableCommands().ToList();

            DisplayCommandInfo(presetCommands, selectableCommands);

            if (presetCommands.Count == 0)
            {
                Console.WriteLine("  戦闘指揮がありません");
                return new BattleResult { Success = false, ErrorMessage = "戦闘指揮がありません" };
            }

            // 戦闘指揮を実行
            bool commandResult = commandPhaseModel.SetCommand(presetCommands);
            Console.WriteLine($"  戦闘指揮の実行結果: {commandResult}: {commandPhaseModel}");

            if (!commandResult)
            {
                Console.WriteLine("  戦闘指揮の実行に失敗しました");
                return new BattleResult { Success = false, ErrorMessage = "戦闘指揮の実行に失敗" };
            }

            Console.WriteLine("  戦闘指揮の実行完了");

            // 昼戦結果の取得
            Console.WriteLine("  昼戦結果を計算中...");

            // XXX: 夜戦実行前にGetBattleResultを呼んではいけない。BattleManagerの_battleDataがnullになってエラー
            // var dayBattleResult = battleManager.GetBattleResult();

            Console.WriteLine("  昼戦完了！");

            // 夜戦の実行判定
            if (executeNightBattle && battleManager.HasNightBattle())
            {
                Console.WriteLine("  夜戦突入可能です");
                Console.WriteLine("  夜戦を開始します...");

                // 夜戦の前処理を実行
                var nightBattleResult = ProcessNightBattle(battleManager);

                Console.WriteLine("  夜戦完了！");

                var result = new BattleResult
                {
                    Success = true,
                    BattleResultModel = nightBattleResult,
                    PresetCommands = presetCommands,
                    SelectableCommands = selectableCommands
                };

                return result;
            }
            else
            {
                if (!executeNightBattle)
                {
                    Console.WriteLine("  夜戦フラグがfalseのため夜戦をスキップします");
                }
                else
                {
                    Console.WriteLine("  夜戦は発生しません");
                }

                var result = new BattleResult
                {
                    Success = true,
                    // BattleResultModel = dayBattleResult,
                    PresetCommands = presetCommands,
                    SelectableCommands = selectableCommands
                };

                return result;
            }
        }

        /// <summary>
        /// 戦闘指揮情報を表示
        /// </summary>
        /// <param name="presetCommands">戦闘指揮</param>
        /// <param name="selectableCommands">選択可能な戦闘指揮</param>
        private void DisplayCommandInfo(List<BattleCommand> presetCommands, List<BattleCommand> selectableCommands)
        {
            Console.WriteLine($"  戦闘指揮数: {presetCommands.Count}");
            Console.WriteLine($"  戦闘指揮の内容: {string.Join(", ", presetCommands.Select((cmd, i) => $"{cmd}({BattleCommandHelper.GetDescription(cmd)})"))}");
            Console.WriteLine($"  選択可能な戦闘指揮: {string.Join(", ", selectableCommands.Select(cmd => $"{cmd}({BattleCommandHelper.GetDescription(cmd)})"))}");
        }

        /// <summary>
        /// 陣形名を取得
        /// </summary>
        /// <param name="formation">陣形</param>
        /// <returns>陣形名</returns>
        private string GetFormationName(BattleFormationKinds1 formation)
        {
            switch (formation)
            {
                case BattleFormationKinds1.TanJuu:
                    return "単縦陣";
                case BattleFormationKinds1.FukuJuu:
                    return "複縦陣";
                case BattleFormationKinds1.Rinkei:
                    return "輪形陣";
                case BattleFormationKinds1.Teikei:
                    return "梯形陣";
                case BattleFormationKinds1.TanOu:
                    return "単横陣";
                default:
                    return "不明";
            }
        }

        /// <summary>
        /// 夜戦を処理
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        /// <returns>夜戦結果</returns>
        private BattleResultModel ProcessNightBattle(SortieBattleManager battleManager)
        {
            Console.WriteLine("  夜戦の前処理を実行中...");

            try
            {
                // 1. 夜戦データの取得と戦闘フェーズの変更
                Console.WriteLine("    夜戦データを取得中...");
                battleManager.StartDayToNightBattle();

                // 2. 夜戦開始時の艦娘状態の記録
                Console.WriteLine("    艦娘の夜戦開始状態を記録中...");
                RecordNightBattleStartState(battleManager);

                // 3. 夜戦用キャッシュデータの更新
                Console.WriteLine("    夜戦用キャッシュデータを更新中...");
                UpdateNightBattleCacheData(battleManager);

                Console.WriteLine("  夜戦の前処理完了");
                Console.WriteLine("  夜戦中...");

                // 4. 夜戦結果の取得
                var nightBattleResult = battleManager.GetBattleResult();
                Console.WriteLine("  夜戦結果を取得しました");

                return nightBattleResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  夜戦処理中にエラーが発生しました: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 夜戦開始時の艦娘状態を記録
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        private void RecordNightBattleStartState(SortieBattleManager battleManager)
        {
            try
            {
                Console.WriteLine("    自軍艦娘の状態を記録中...");
                var friendlyShips = battleManager.Ships_f;
                int friendlyCount = 0;
                foreach (var ship in friendlyShips)
                {
                    if (ship != null && ship.TmpId > 0)
                    {
                        friendlyCount++;
                        var damageState = ship.DmgStateEnd;
                        Console.WriteLine($"      {ship.Name}: HP {ship.HpEnd}/{ship.MaxHp} ({damageState})");
                    }
                }
                Console.WriteLine($"    自軍艦娘 {friendlyCount}隻の状態を記録しました");

                Console.WriteLine("    敵軍艦娘の状態を記録中...");
                var enemyShips = battleManager.Ships_e;
                int enemyCount = 0;
                foreach (var ship in enemyShips)
                {
                    if (ship != null && ship.TmpId > 0)
                    {
                        enemyCount++;
                        var damageState = ship.DmgStateEnd;
                        Console.WriteLine($"      {ship.Name}: HP {ship.HpEnd}/{ship.MaxHp} ({damageState})");
                    }
                }
                Console.WriteLine($"    敵軍艦娘 {enemyCount}隻の状態を記録しました");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    艦娘状態の記録中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 夜戦用キャッシュデータを更新
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        private void UpdateNightBattleCacheData(SortieBattleManager battleManager)
        {
            try
            {
                Console.WriteLine("    夜戦用データを準備中...");

                // 夜戦モデルの取得
                var nightCombatModel = battleManager.GetNightCombatData();
                if (nightCombatModel != null)
                {
                    Console.WriteLine("    夜戦データを取得しました");

                    // 探照灯の情報を確認
                    CheckSearchLightInfo(nightCombatModel);

                    // 照明弾の情報を確認
                    CheckFlareInfo(nightCombatModel);

                    // 夜戦攻撃データの準備
                    var hougekiList = battleManager.GetHougekiList_Night();
                    if (hougekiList != null)
                    {
                        Console.WriteLine($"    夜戦攻撃データを準備しました（攻撃数: {hougekiList.Count}）");
                    }
                    else
                    {
                        Console.WriteLine("    夜戦攻撃データが見つかりません");
                    }
                }
                else
                {
                    Console.WriteLine("    夜戦データの取得に失敗しました");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    夜戦用キャッシュデータの更新中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 探照灯の情報を確認
        /// </summary>
        /// <param name="nightCombatModel">夜戦モデル</param>
        private void CheckSearchLightInfo(NightCombatModel nightCombatModel)
        {
            try
            {
                var friendlySearchLight = nightCombatModel.GetSearchLightShip(true);
                if (friendlySearchLight != null)
                {
                    Console.WriteLine($"    自軍探照灯艦: {friendlySearchLight.Name}");
                }

                var enemySearchLight = nightCombatModel.GetSearchLightShip(false);
                if (enemySearchLight != null)
                {
                    Console.WriteLine($"    敵軍探照灯艦: {enemySearchLight.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    探照灯情報の確認中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 照明弾の情報を確認
        /// </summary>
        /// <param name="nightCombatModel">夜戦モデル</param>
        private void CheckFlareInfo(NightCombatModel nightCombatModel)
        {
            try
            {
                var friendlyFlare = nightCombatModel.GetFlareShip(true);
                if (friendlyFlare != null)
                {
                    Console.WriteLine($"    自軍照明弾艦: {friendlyFlare.Name}");
                }

                var enemyFlare = nightCombatModel.GetFlareShip(false);
                if (enemyFlare != null)
                {
                    Console.WriteLine($"    敵軍照明弾艦: {enemyFlare.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    照明弾情報の確認中にエラーが発生しました: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 戦闘指揮のヘルパークラス
    /// </summary>
    public static class BattleCommandHelper
    {
        /// <summary>
        /// 戦闘指揮の説明を取得
        /// </summary>
        /// <param name="command">戦闘指揮</param>
        /// <returns>コマンドの説明</returns>
        public static string GetDescription(BattleCommand command)
        {
            switch (command)
            {
                case BattleCommand.None:
                    return "なし";
                case BattleCommand.Sekkin:
                    return "接近戦";
                case BattleCommand.Hougeki:
                    return "砲撃戦";
                case BattleCommand.Raigeki:
                    return "雷撃戦";
                case BattleCommand.Ridatu:
                    return "離脱";
                case BattleCommand.Taisen:
                    return "対潜戦";
                case BattleCommand.Kaihi:
                    return "回避";
                case BattleCommand.Kouku:
                    return "航空戦";
                case BattleCommand.Totugeki:
                    return "突撃";
                case BattleCommand.Tousha:
                    return "投射";
                default:
                    return "不明";
            }
        }
    }

    /// <summary>
    /// 戦闘結果の表示を担当するクラス
    /// </summary>
    public static class BattleResultDisplayer
    {
        /// <summary>
        /// 戦闘結果を表示（簡素化版）
        /// </summary>
        /// <param name="battleResult">戦闘結果</param>
        public static void DisplayBattleResult(BattleResultModel battleResult)
        {
            Console.WriteLine("\n=== 戦闘結果 ===");

            try
            {
                if (battleResult == null)
                {
                    Console.WriteLine("  戦闘結果が取得できませんでした");
                    return;
                }

                Console.WriteLine($"  勝利判定: {battleResult.WinRank} / MVP: {battleResult.MvpShip}番艦");

                // 敵艦の状態を表示
                DisplayEnemyStatus(battleResult.Ships_e);

                // 味方艦隊の状態を表示
                DisplayFriendlyStatus(battleResult.Ships_f);

                Console.WriteLine("  戦闘終了");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  戦闘結果の表示中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 敵艦の状態を表示（簡素化版）
        /// </summary>
        /// <param name="enemyShips">敵艦配列</param>
        private static void DisplayEnemyStatus(ShipModel_BattleResult[] enemyShips)
        {
            int destroyedEnemies = 0;
            int totalEnemies = enemyShips.Length;

            Console.WriteLine("  敵艦:");
            for (int i = 0; i < enemyShips.Length; i++)
            {
                var enemy = enemyShips[i];
                if (enemy == null) continue;

                string damageState = GetDamageStateDescription(enemy.DmgStateEnd);
                Console.WriteLine($"    {i + 1}. {enemy.Name}: {enemy.HpStart} → {enemy.HpEnd} [{damageState}]");

                // 撃沈判定
                if (enemy.DmgStateEnd == DamageState_Battle.Gekichin || enemy.HpEnd <= 0)
                {
                    destroyedEnemies++;
                }
            }

            Console.WriteLine($"  撃沈数: {destroyedEnemies}/{totalEnemies}隻");
        }

        /// <summary>
        /// 味方艦の状態を表示（簡素化版）
        /// </summary>
        /// <param name="friendlyShips">味方艦配列</param>
        private static void DisplayFriendlyStatus(ShipModel_BattleResult[] friendlyShips)
        {
            Console.WriteLine("  味方艦隊:");
            for (int i = 0; i < friendlyShips.Length; i++)
            {
                var ship = friendlyShips[i];
                if (ship == null) continue;

                string damageState = GetDamageStateDescription(ship.DmgStateEnd);
                Console.WriteLine($"    {i + 1}. {ship.Name}: {ship.HpStart} → {ship.HpEnd} [{damageState}]");
            }
        }

        /// <summary>
        /// 損傷状態の説明を取得
        /// </summary>
        /// <param name="damageState">損傷状態</param>
        /// <returns>損傷状態の説明</returns>
        private static string GetDamageStateDescription(DamageState_Battle damageState)
        {
            switch (damageState)
            {
                case DamageState_Battle.Normal:
                    return "無傷";
                case DamageState_Battle.Shouha:
                    return "小破";
                case DamageState_Battle.Tyuuha:
                    return "中破";
                case DamageState_Battle.Taiha:
                    return "大破";
                case DamageState_Battle.Gekichin:
                    return "撃沈";
                default:
                    return "不明";
            }
        }
    }

    /// <summary>
    /// 出撃準備のユーティリティクラス
    /// </summary>
    public static class SortieHelper
    {
        /// <summary>
        /// 出撃準備を行う
        /// </summary>
        /// <param name="areaId">エリアID</param>
        /// <param name="mapNo">マップ番号</param>
        public static void PrepareForSortie(int areaId = 4, int mapNo = 4)
        {
            Console.WriteLine("  出撃準備を開始中...");

            // 戦略画面への遷移
            Console.WriteLine("  戦略画面（母港）に遷移中...");
            var strategyTaskManager = new StrategyTopTaskManager();

            // マップ選択画面への遷移
            Console.WriteLine("  マップ選択画面に遷移中...");
            var strategyMapManager = new StrategyMapManager();

            Console.WriteLine("  出撃可能なマップを確認中...");

            // マスタデータからエリア情報を取得
            try
            {
                var areaModel = strategyMapManager.Area.GetValueOrDefault(areaId);
                if (areaModel != null)
                {
                    Console.WriteLine($"  {areaModel.Name}（エリア{areaModel.Id}）のマップ{areaModel.Id}-{mapNo}を選択");
                }
                else
                {
                    Console.WriteLine($"  エリア{areaId}のマップ{areaId}-{mapNo}を選択");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  エリア情報の取得でエラーが発生しました: {ex.Message}");
                Console.WriteLine($"  エリア{areaId}のマップ{areaId}-{mapNo}を選択");
            }

            Console.WriteLine("  出撃準備完了");
        }
    }

    /// <summary>
    /// 戦闘結果を表現するクラス
    /// </summary>
    public class BattleResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public BattleResultModel BattleResultModel { get; set; }
        public List<BattleCommand> PresetCommands { get; set; }
        public List<BattleCommand> SelectableCommands { get; set; }
    }
}
