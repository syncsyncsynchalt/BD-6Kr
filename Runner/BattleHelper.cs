using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enum;
using local.managers;
using local.models;
using local.models.battle;
using Server_Controllers;

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
        /// <param name="enableDetailedAnalysis">詳細分析を有効にするかどうか</param>
        /// <returns>戦闘結果</returns>
        public BattleResult ExecuteBattle(int areaId, int mapNo, BattleFormationKinds1 formation = BattleFormationKinds1.TanJuu, bool executeNightBattle = false, bool enableDetailedAnalysis = false)
        {
            try
            {
                // マップ管理の初期化
                StrategyMapManager strategyMapManager = new StrategyMapManager();
                SortieManager sortieManager = strategyMapManager.SelectArea(areaId);
                if (sortieManager == null)
                {
                    return new BattleResult { Success = false, ErrorMessage = "SortieManagerの取得に失敗" };
                }

                // マップIDを計算（エリア4のマップ4は44）
                int mapId = areaId * 10 + mapNo;
                SortieMapManager sortieMapManager = sortieManager.GoSortie(1, mapId);
                if (sortieMapManager == null)
                {
                    return new BattleResult { Success = false, ErrorMessage = "SortieMapManagerの取得に失敗" };
                }

                // 戦闘開始
                SortieBattleManager sortieBattleManager = sortieMapManager.BattleStart_Write(formation);

                // === 戦闘指揮処理（インライン化） ===
                // 1. 戦闘データの基本設定は__Init__メソッド内で実行済み

                // 2. 戦闘指揮前のキャッシュデータ作成
                sortieBattleManager.__createCacheDataBeforeCommand__();

                CommandPhaseModel commandPhaseModel = sortieBattleManager.GetCommandPhaseModel();
                if (commandPhaseModel == null)
                {
                    return new BattleResult { Success = false, ErrorMessage = "コマンドフェーズモデルの取得に失敗" };
                }

                // 戦闘指揮を取得（SetCommand実行前）
                List<BattleCommand> presetCommands = commandPhaseModel.GetPresetCommand();
                List<BattleCommand> selectableCommands = commandPhaseModel.GetSelectableCommands().ToList();

                // 3. 戦闘指揮の実行
                bool commandSuccess = commandPhaseModel.SetCommand(presetCommands);
                if (!commandSuccess)
                {
                    return new BattleResult { Success = false, ErrorMessage = "戦闘指揮の実行に失敗" };
                }

                // === 昼戦後の処理 ===

                // 詳細分析を有効にした場合、昼戦の詳細分析を実行
                if (enableDetailedAnalysis)
                {
                    AnalyzeDayBattleDetails(sortieBattleManager);
                }

                BattleResult battleResult;

                // 夜戦の実行判定
                if (executeNightBattle && sortieBattleManager.HasNightBattle())
                {
                    // === 夜戦処理（インライン化） ===
                    try
                    {
                        // 1. 夜戦データの取得と戦闘フェーズの変更
                        sortieBattleManager.StartDayToNightBattle();

                        // 2. 夜戦用キャッシュデータの更新
                        try
                        {
                            // 夜戦モデルの取得
                            NightCombatModel nightCombatModel = sortieBattleManager.GetNightCombatData();
                            if (nightCombatModel != null)
                            {
                                // 夜戦攻撃データの準備
                                HougekiListModel hougekiList = sortieBattleManager.GetHougekiList_Night();
                            }
                        }
                        catch (Exception ex)
                        {
                            return new BattleResult { Success = false, ErrorMessage = $"夜戦用キャッシュデータの更新中にエラーが発生しました: {ex.Message}" };
                        }

                        // 詳細分析を有効にした場合、夜戦の詳細分析を実行
                        if (enableDetailedAnalysis)
                        {
                            AnalyzeNightBattleDetails(sortieBattleManager);
                        }

                        // 3. 夜戦結果の取得
                        BattleResultModel nightBattleResult = sortieBattleManager.GetBattleResult();

                        battleResult = new BattleResult
                        {
                            Success = true,
                            BattleResultModel = nightBattleResult,
                            PresetCommands = presetCommands,
                            SelectableCommands = selectableCommands
                        };
                    }
                    catch (Exception ex)
                    {
                        return new BattleResult { Success = false, ErrorMessage = $"夜戦処理中にエラーが発生しました: {ex.Message}" };
                    }
                }
                else
                {
                    // 昼戦結果の取得
                    BattleResultModel dayBattleResult = sortieBattleManager.GetBattleResult();

                    battleResult = new BattleResult
                    {
                        Success = true,
                        BattleResultModel = dayBattleResult,
                        PresetCommands = presetCommands,
                        SelectableCommands = selectableCommands
                    };
                }

                // 戦闘結果にマップ情報を追加
                if (battleResult.Success)
                {
                    battleResult.SortieMapManager = sortieMapManager;
                    battleResult.AreaId = areaId;
                    battleResult.MapNo = mapNo;
                    battleResult.Formation = formation;
                }

                return battleResult;
            }
            catch (Exception ex)
            {
                return new BattleResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 昼戦の詳細分析を実行
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        private void AnalyzeDayBattleDetails(SortieBattleManager battleManager)
        {
            try
            {
                // 昼戦詳細分析のヘッダーを表示
                BattlePrinter.PrintDetailedDayBattleAnalysisHeader();

                // 昼戦フェーズの基本情報を表示
                BattlePrinter.PrintDayBattlePhaseAnalysis(battleManager);

                // 昼戦後の状況分析を表示
                BattlePrinter.PrintDayBattleResultAnalysis(battleManager);
            }
            catch (Exception)
            {
                // 分析エラーは戦闘の継続に影響しないため、例外を再スローしない
            }
        }

        /// <summary>
        /// 夜戦の詳細分析を実行
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        private void AnalyzeNightBattleDetails(SortieBattleManager battleManager)
        {
            try
            {
                // 夜戦詳細分析のヘッダーを表示
                BattlePrinter.PrintDetailedNightBattleAnalysisHeader();

                // 夜戦参加艦の状況分析
                BattlePrinter.PrintNightBattleParticipantsAnalysis(battleManager);

                // 夜戦特殊装備の効果分析
                BattlePrinter.PrintNightBattleEquipmentAnalysis(battleManager);

                // 夜戦攻撃パターンの分析
                BattlePrinter.PrintNightBattleAttackPatternsAnalysis(battleManager);

                // 夜戦結果の詳細分析
                BattlePrinter.PrintNightBattleResultAnalysis(battleManager);
            }
            catch (Exception)
            {
                // 分析エラーは戦闘の継続に影響しないため、例外を再スローしない
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
            public SortieMapManager SortieMapManager { get; set; }
            public int AreaId { get; set; }
            public int MapNo { get; set; }
            public BattleFormationKinds1 Formation { get; set; }
        }
    }
}