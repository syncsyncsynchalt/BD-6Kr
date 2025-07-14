using System;
using System.Collections.Generic;
using System.Linq;
using KCV.Strategy;
using Common.Enum;
using local.managers;
using local.models.battle;

namespace Runner
{
    /// <summary>
    /// 戦闘システムの管理を担当するクラス
    /// </summary>
    public class BattleHelper
    {
        /// <summary>
        /// 戦闘を実行
        /// </summary>
        /// <param name="areaId">エリアID</param>
        /// <param name="mapNo">マップ番号</param>
        /// <param name="formation">戦闘陣形</param>
        /// <param name="executeNightBattle">夜戦を実行するかどうか</param>
        /// <returns>戦闘結果</returns>
        public BattleResult ExecuteBattle(int areaId, int mapNo, BattleFormationKinds1 formation = BattleFormationKinds1.TanJuu, bool executeNightBattle = false)
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
                FleetPrinter.FleetStatus(sortieMapManager);
                FleetPrinter.EnemyFleetInfo(sortieMapManager);

                // 戦闘開始
                BattlePrinter.PrintBattleStart(areaId, mapNo, formation);
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

            BattlePrinter.PrintCommandInfo(presetCommands, selectableCommands);

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
                    PresetCommands = presetCommands,
                    SelectableCommands = selectableCommands
                };

                return result;
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
                BattlePrinter.RecordNightBattleStartState(battleManager);

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
                    BattlePrinter.CheckSearchLightInfo(nightCombatModel);

                    // 照明弾の情報を確認
                    BattlePrinter.CheckFlareInfo(nightCombatModel);

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
}