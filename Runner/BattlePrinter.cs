using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enum;
using local.models.battle;
using local.models;
using local.managers;
using Server_Common.Formats.Battle;
using Server_Models;

namespace Runner
{
    /// <summary>
    /// 標準出力に戦闘情報を表示するユーティリティクラス
    /// </summary>
    public static class BattlePrinter
    {
        /// <summary>
        /// 戦闘指揮情報を表示（純粋な表示機能のみ）
        /// </summary>
        public static void PrintCommandPhaseInfo(SortieBattleManager battleManager)
        {
            CommandPhaseModel commandPhaseModel = battleManager.GetCommandPhaseModel();
            if (commandPhaseModel == null)
            {
                Console.WriteLine("  コマンドフェーズモデルが存在しません");
                return;
            }

            // 戦闘指揮を取得
            List<BattleCommand> presetCommands = commandPhaseModel.GetPresetCommand();
            List<BattleCommand> selectableCommands = commandPhaseModel.GetSelectableCommands().ToList();

            PrintCommandInfo(presetCommands, selectableCommands);
        }
        /// <summary>
        /// 戦闘開始情報を表示
        /// </summary>
        public static void PrintBattleStart(int areaId, int mapNo, BattleFormationKinds1 formation)
        {
            Console.WriteLine($"  戦闘開始！（エリア{areaId}、マップ{mapNo}、陣形: {GetFormationName(formation)}）");
        }

        /// <summary>
        /// 戦闘指揮情報を表示
        /// </summary>
        public static void PrintCommandInfo(List<BattleCommand> presetCommands, List<BattleCommand> selectableCommands)
        {
            Console.WriteLine($"  戦闘指揮数: {presetCommands.Count}");
            Console.WriteLine($"  戦闘指揮の内容: {string.Join(", ", presetCommands.Select((cmd, i) => $"{cmd}({BattleCommandHelper.GetDescription(cmd)})"))}");
            Console.WriteLine($"  選択可能な戦闘指揮: {string.Join(", ", selectableCommands.Select(cmd => $"{cmd}({BattleCommandHelper.GetDescription(cmd)})"))}");
        }

        /// <summary>
        /// 戦闘結果を表示（簡素化版）
        /// </summary>
        /// <param name="battleResult">戦闘結果</param>
        public static void PrintBattleResult(BattleResultModel battleResult)
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
        /// 戦闘結果を詳細に表示
        /// </summary>
        /// <param name="battleResult">戦闘結果</param>
        public static void PrintDetailedBattleResult(BattleResultModel battleResult)
        {
            Console.WriteLine("\n=== 詳細戦闘結果 ===");

            try
            {
                if (battleResult == null)
                {
                    Console.WriteLine("  戦闘結果が取得できませんでした");
                    return;
                }

                // 基本的な戦闘結果
                Console.WriteLine($"  勝利判定: {battleResult.WinRank}");
                Console.WriteLine($"  MVP艦: {battleResult.MvpShip?.Name ?? "なし"}番艦");

                // 敵艦の詳細状態
                DisplayDetailedEnemyStatus(battleResult.Ships_e);

                // 味方艦隊の詳細状態
                DisplayDetailedFriendlyStatus(battleResult.Ships_f);

                Console.WriteLine("  戦闘終了");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  詳細戦闘結果の表示中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 昼戦詳細分析のヘッダーを表示
        /// </summary>
        public static void PrintDetailedDayBattleAnalysisHeader()
        {
            Console.WriteLine("\n=== 昼戦詳細分析 ===");
        }

        /// <summary>
        /// 夜戦詳細分析のヘッダーを表示
        /// </summary>
        public static void PrintDetailedNightBattleAnalysisHeader()
        {
            Console.WriteLine("\n=== 夜戦詳細分析 ===");
        }

        /// <summary>
        /// 昼戦フェーズの分析を表示
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        public static void PrintDayBattlePhaseAnalysis(SortieBattleManager battleManager)
        {
            Console.WriteLine("  [昼戦フェーズ分析]");

            try
            {
                // 航空戦フェーズの実戦分析
                AnalyzeAirCombatPhase(battleManager);

                // 先制対潜攻撃の実戦分析
                AnalyzeOpeningAntiSubPhase(battleManager);

                // 砲撃戦フェーズの実戦分析
                AnalyzeShellingPhase(battleManager);

                // 雷撃戦フェーズの実戦分析
                AnalyzeTorpedoPhase(battleManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    昼戦フェーズ分析中にエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 航空戦フェーズの実戦分析
        /// </summary>
        private static void AnalyzeAirCombatPhase(SortieBattleManager battleManager)
        {
            Console.WriteLine("    === 航空戦フェーズ ===");

            try
            {
                // 航空戦データの存在確認
                if (!battleManager.IsExistKoukuuData())
                {
                    Console.WriteLine("      航空戦は発生しませんでした");
                    return;
                }

                dynamic koukuData = battleManager.GetKoukuuData();
                if (koukuData == null)
                {
                    Console.WriteLine("      航空戦データが取得できませんでした");
                    return;
                }

                // 制空権状態の分析
                Common.Enum.BattleSeikuKinds seikuKind = koukuData.SeikuKind;
                string airSupremacyStatus = GetAirSuperiorityDescription(seikuKind);
                Console.WriteLine($"      制空権状況: {airSupremacyStatus}");

                // 艦載機損失の分析
                int friendlyStage1Lost = koukuData.Stage1_LostCount_f;
                int enemyStage1Lost = koukuData.Stage1_LostCount_e;
                int friendlyStage2Lost = koukuData.Stage2_LostCount_f;
                int enemyStage2Lost = koukuData.Stage2_LostCount_e;

                int friendlyTotalLoss = friendlyStage1Lost + friendlyStage2Lost;
                int enemyTotalLoss = enemyStage1Lost + enemyStage2Lost;

                Console.WriteLine($"      艦載機損失: 味方 {friendlyTotalLoss}機 (制空戦:{friendlyStage1Lost} 対空戦:{friendlyStage2Lost}) / 敵 {enemyTotalLoss}機 (制空戦:{enemyStage1Lost} 対空戦:{enemyStage2Lost})");

                // 航空戦優劣の評価
                if (seikuKind == Common.Enum.BattleSeikuKinds.Kakuho || seikuKind == Common.Enum.BattleSeikuKinds.Yuusei)
                {
                    Console.WriteLine("      航空戦優勢: 制空権を確保し有利な戦況");
                }
                else if (seikuKind == Common.Enum.BattleSeikuKinds.Ressei || seikuKind == Common.Enum.BattleSeikuKinds.Lost)
                {
                    Console.WriteLine("      航空戦劣勢: 制空権不利により攻撃効果減少");
                }
                else
                {
                    Console.WriteLine("      航空戦: 制空権拮抗状態");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      航空戦分析エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 先制対潜攻撃フェーズの実戦分析
        /// </summary>
        private static void AnalyzeOpeningAntiSubPhase(SortieBattleManager battleManager)
        {
            Console.WriteLine("    === 先制対潜攻撃 ===");

            try
            {
                // 先制雷撃データの存在確認
                if (!battleManager.IsExistKaimakuData())
                {
                    Console.WriteLine("      先制対潜攻撃は発生しませんでした");
                    return;
                }

                dynamic kaimakuData = battleManager.GetKaimakuData();
                if (kaimakuData == null)
                {
                    Console.WriteLine("      先制対潜攻撃データが取得できませんでした");
                    return;
                }

                // 味方・敵の攻撃数を取得
                int friendlyAttackCount = kaimakuData.Count_f;
                int enemyAttackCount = kaimakuData.Count_e;
                int totalAttackCount = friendlyAttackCount + enemyAttackCount;

                Console.WriteLine($"      対潜攻撃回数: 計{totalAttackCount}回 (味方:{friendlyAttackCount}回 敵:{enemyAttackCount}回)");

                if (totalAttackCount > 0)
                {
                    Console.WriteLine("      対潜攻撃を実施しました");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      対潜攻撃分析エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 砲撃戦フェーズの実戦分析
        /// </summary>
        private static void AnalyzeShellingPhase(SortieBattleManager battleManager)
        {
            Console.WriteLine("    === 砲撃戦フェーズ ===");

            try
            {
                // 砲撃戦データの存在確認
                if (!battleManager.IsExistHougekiPhase_Day())
                {
                    Console.WriteLine("      砲撃戦は発生しませんでした");
                    return;
                }

                dynamic hougekiPhases = battleManager.GetHougekiData_Day();
                if (hougekiPhases == null || hougekiPhases.Count == 0)
                {
                    Console.WriteLine("      砲撃戦データが取得できませんでした");
                    return;
                }

                int totalAttacks = 0;
                int totalDamage = 0;
                int hits = 0;
                Dictionary<string, int> attackTypes = new Dictionary<string, int>();

                // 各砲撃フェーズを分析
                for (int phaseIndex = 0; phaseIndex < hougekiPhases.Count; phaseIndex++)
                {
                    dynamic phase = hougekiPhases[phaseIndex];
                    if (phase == null) continue;

                    Console.WriteLine($"      --- 砲撃フェーズ {phaseIndex + 1} ---");

                    // 味方砲撃の分析
                    HougekiListModel friendlyHougeki = phase.GetHougeki_f();
                    if (friendlyHougeki != null)
                    {
                        Console.WriteLine("        味方砲撃:");
                        AnalyzeHougekiDataDetailed(friendlyHougeki, ref totalAttacks, ref totalDamage, ref hits, attackTypes, "味方");
                    }

                    // 敵砲撃の分析
                    HougekiListModel enemyHougeki = phase.GetHougeki_e();
                    if (enemyHougeki != null)
                    {
                        Console.WriteLine("        敵砲撃:");
                        AnalyzeHougekiDataDetailed(enemyHougeki, ref totalAttacks, ref totalDamage, ref hits, attackTypes, "敵");
                    }
                }

                Console.WriteLine($"      砲撃回数: {totalAttacks}回");
                Console.WriteLine($"      砲撃成果: 命中 {hits}回 / 総ダメージ {totalDamage}");

                if (totalAttacks > 0)
                {
                    Console.WriteLine($"      砲撃命中率: {(hits * 100.0 / totalAttacks):F1}%");
                }

                // 攻撃タイプ別集計
                if (attackTypes.Count > 0)
                {
                    Console.WriteLine("      攻撃タイプ別:");
                    foreach (KeyValuePair<string, int> kvp in attackTypes)
                    {
                        Console.WriteLine($"        {kvp.Key}: {kvp.Value}回");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      砲撃戦分析エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 砲撃データの詳細分析（攻撃者と被攻撃者の詳細表示）
        /// </summary>
        private static void AnalyzeHougekiDataDetailed(HougekiListModel hougekiList, ref int totalAttacks, ref int totalDamage, ref int hits, Dictionary<string, int> attackTypes, string side)
        {
            if (hougekiList == null) return;

            for (int i = 0; i < hougekiList.Count; i++)
            {
                HougekiModel attack = hougekiList.GetData(i);
                if (attack == null) continue;

                totalAttacks++;
                int damage = attack.GetDamage();
                totalDamage += damage;

                if (damage > 0)
                {
                    hits++;
                }

                // HougekiModelのToString()内容を表示
                Console.WriteLine(attack.ToString());
                Console.WriteLine("---");

                // 攻撃タイプの分類
                string attackType = ClassifyDayAttackType(attack);
                if (!attackTypes.ContainsKey(attackType))
                    attackTypes[attackType] = 0;
                attackTypes[attackType]++;
            }
        }

        /// <summary>
        /// 砲撃データの詳細分析
        /// </summary>
        private static void AnalyzeHougekiData(HougekiListModel hougekiList, ref int totalAttacks, ref int totalDamage, ref int hits, Dictionary<string, int> attackTypes, string side)
        {
            if (hougekiList == null) return;

            for (int i = 0; i < hougekiList.Count; i++)
            {
                HougekiModel attack = hougekiList.GetData(i);
                if (attack == null) continue;

                totalAttacks++;
                int damage = attack.GetDamage();
                totalDamage += damage;

                if (damage > 0)
                {
                    hits++;
                }

                // 攻撃タイプの分類
                string attackType = ClassifyDayAttackType(attack);
                if (!attackTypes.ContainsKey(attackType))
                    attackTypes[attackType] = 0;
                attackTypes[attackType]++;
            }
        }

        /// <summary>
        /// 雷撃戦フェーズの実戦分析
        /// </summary>
        private static void AnalyzeTorpedoPhase(SortieBattleManager battleManager)
        {
            Console.WriteLine("    === 雷撃戦フェーズ ===");

            try
            {
                // 雷撃戦データの存在確認
                if (!battleManager.IsExistRaigekiData())
                {
                    Console.WriteLine("      雷撃戦は発生しませんでした");
                    return;
                }

                dynamic raigekiData = battleManager.GetRaigekiData();
                if (raigekiData == null)
                {
                    Console.WriteLine("      雷撃戦データが取得できませんでした");
                    return;
                }

                // 味方・敵の攻撃数を取得
                int friendlyAttackCount = raigekiData.Count_f;
                int enemyAttackCount = raigekiData.Count_e;

                Console.WriteLine($"      雷撃攻撃: 味方 {friendlyAttackCount}回 / 敵 {enemyAttackCount}回");

                // 雷撃戦の優劣評価
                if (friendlyAttackCount > enemyAttackCount)
                {
                    Console.WriteLine("      雷撃戦優勢: 味方が有効な雷撃を実施");
                }
                else if (enemyAttackCount > friendlyAttackCount)
                {
                    Console.WriteLine("      雷撃戦劣勢: 敵の雷撃が効果的");
                }
                else if (friendlyAttackCount > 0 && enemyAttackCount > 0)
                {
                    Console.WriteLine("      雷撃戦互角: 双方同程度の雷撃実施");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      雷撃戦分析エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 制空権状態の説明を取得
        /// </summary>
        private static string GetAirSuperiorityDescription(Common.Enum.BattleSeikuKinds seikuKind)
        {
            switch (seikuKind)
            {
                case Common.Enum.BattleSeikuKinds.Kakuho:
                    return "制空権確保";
                case Common.Enum.BattleSeikuKinds.Yuusei:
                    return "航空優勢";
                case Common.Enum.BattleSeikuKinds.Ressei:
                    return "航空劣勢";
                case Common.Enum.BattleSeikuKinds.Lost:
                    return "制空権喪失";
                case Common.Enum.BattleSeikuKinds.None:
                default:
                    return "制空権争い無し";
            }
        }

        /// <summary>
        /// 航空攻撃結果の分析
        /// </summary>
        private static string AnalyzeAirAttackResults(dynamic koukuData)
        {
            // 実際の航空攻撃データに基づいて分析（簡易版）
            return "航空攻撃実施"; // 実装では詳細な分析が必要
        }

        /// <summary>
        /// 昼戦攻撃タイプの分類
        /// </summary>
        private static string ClassifyDayAttackType(dynamic attack)
        {
            try
            {
                // 実際の実装では attack.SpType や装備構成によって判定
                int damage = attack.GetDamage();

                if (damage > 200)
                    return "主砲連撃 (高威力)";
                else if (damage > 100)
                    return "主砲攻撃 (中威力)";
                else if (damage > 0)
                    return "通常攻撃";
                else
                    return "ミス/回避";
            }
            catch
            {
                return "不明";
            }
        }

        /// <summary>
        /// 昼戦結果の分析を表示
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        public static void PrintDayBattleResultAnalysis(SortieBattleManager battleManager)
        {
            Console.WriteLine("  [昼戦結果]");

            try
            {
                dynamic friendlyShips = battleManager.Ships_f;
                dynamic enemyShips = battleManager.Ships_e;

                // 昼戦での損害状況
                int friendlyDamaged = 0;
                int friendlyHeavyDamaged = 0;
                int enemyDestroyed = 0;
                int enemyDamaged = 0;

                foreach (dynamic ship in friendlyShips)
                {
                    if (ship != null && ship.TmpId > 0)
                    {
                        if (ship.DmgStateEnd >= DamageState_Battle.Shouha)
                            friendlyDamaged++;
                        if (ship.DmgStateEnd >= DamageState_Battle.Taiha)
                            friendlyHeavyDamaged++;
                    }
                }

                foreach (dynamic ship in enemyShips)
                {
                    if (ship != null && ship.TmpId > 0)
                    {
                        if (ship.DmgStateEnd == DamageState_Battle.Gekichin || ship.HpEnd <= 0)
                            enemyDestroyed++;
                        else if (ship.DmgStateEnd >= DamageState_Battle.Shouha)
                            enemyDamaged++;
                    }
                }

                Console.WriteLine($"    味方損害: 小破以上 {friendlyDamaged}隻 / 大破以上 {friendlyHeavyDamaged}隻");
                Console.WriteLine($"    敵艦撃沈: {enemyDestroyed}隻 / 損害 {enemyDamaged}隻");

                // 夜戦突入判定
                bool canEnterNightBattle = battleManager.HasNightBattle();
                Console.WriteLine($"    夜戦突入可能: {(canEnterNightBattle ? "可能" : "不可")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    昼戦結果分析中にエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 夜戦参加艦の状況分析を表示
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        public static void PrintNightBattleParticipantsAnalysis(SortieBattleManager battleManager)
        {
            Console.WriteLine("  [夜戦参加艦状況]");

            try
            {
                dynamic friendlyShips = battleManager.Ships_f;
                dynamic enemyShips = battleManager.Ships_e;

                int friendlyParticipants = 0;
                int enemyParticipants = 0;

                // 味方艦の夜戦参加可能判定
                foreach (dynamic ship in friendlyShips)
                {
                    if (ship != null && ship.TmpId > 0)
                    {
                        // 大破していない艦は夜戦参加可能
                        if (ship.DmgStateEnd != DamageState_Battle.Taiha &&
                            ship.DmgStateEnd != DamageState_Battle.Gekichin)
                        {
                            friendlyParticipants++;
                        }
                    }
                }

                // 敵艦の夜戦参加可能判定
                foreach (dynamic ship in enemyShips)
                {
                    if (ship != null && ship.TmpId > 0)
                    {
                        if (ship.DmgStateEnd != DamageState_Battle.Gekichin && ship.HpEnd > 0)
                        {
                            enemyParticipants++;
                        }
                    }
                }

                Console.WriteLine($"    夜戦参加可能艦数: 味方 {friendlyParticipants}隻 / 敵 {enemyParticipants}隻");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    夜戦参加艦分析中にエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 夜戦特殊装備の効果分析を表示
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        public static void PrintNightBattleEquipmentAnalysis(SortieBattleManager battleManager)
        {
            Console.WriteLine("  [夜戦特殊装備効果]");

            try
            {
                NightCombatModel nightCombatModel = battleManager.GetNightCombatData();
                if (nightCombatModel != null)
                {
                    // 探照灯の効果
                    dynamic friendlySearchLight = nightCombatModel.GetSearchLightShip(true);
                    dynamic enemySearchLight = nightCombatModel.GetSearchLightShip(false);

                    if (friendlySearchLight != null)
                    {
                        Console.WriteLine($"    味方探照灯: {friendlySearchLight.Name} (命中率+, 被発見率+)");
                    }
                    if (enemySearchLight != null)
                    {
                        Console.WriteLine($"    敵探照灯: {enemySearchLight.Name} (命中率+, 被発見率+)");
                    }

                    // 照明弾の効果
                    dynamic friendlyFlare = nightCombatModel.GetFlareShip(true);
                    dynamic enemyFlare = nightCombatModel.GetFlareShip(false);

                    if (friendlyFlare != null)
                    {
                        Console.WriteLine($"    味方照明弾: {friendlyFlare.Name} (全体命中率向上)");
                    }
                    if (enemyFlare != null)
                    {
                        Console.WriteLine($"    敵照明弾: {enemyFlare.Name} (全体命中率向上)");
                    }

                    // 夜間触接
                    dynamic friendlyTouchPlane = nightCombatModel.GetTouchPlane(true);
                    dynamic enemyTouchPlane = nightCombatModel.GetTouchPlane(false);

                    if (friendlyTouchPlane != null)
                    {
                        Console.WriteLine($"    味方夜間触接: {friendlyTouchPlane.Name} (攻撃力・命中率向上)");
                    }
                    if (enemyTouchPlane != null)
                    {
                        Console.WriteLine($"    敵夜間触接: {enemyTouchPlane.Name} (攻撃力・命中率向上)");
                    }

                    if (friendlySearchLight == null && enemySearchLight == null &&
                        friendlyFlare == null && enemyFlare == null &&
                        friendlyTouchPlane == null && enemyTouchPlane == null)
                    {
                        Console.WriteLine("    特殊装備による効果はありません");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    夜戦装備分析中にエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 夜戦攻撃パターンの分析を表示
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        public static void PrintNightBattleAttackPatternsAnalysis(SortieBattleManager battleManager)
        {
            Console.WriteLine("  [夜戦攻撃パターン]");

            try
            {
                HougekiListModel hougekiList = battleManager.GetHougekiList_Night();
                if (hougekiList != null && hougekiList.Count > 0)
                {
                    Console.WriteLine($"    夜戦攻撃回数: {hougekiList.Count}回");

                    Dictionary<string, int> attackTypeCount = new Dictionary<string, int>();
                    int totalNightDamage = 0;
                    int nightCriticals = 0;

                    for (int i = 0; i < hougekiList.Count; i++)
                    {
                        HougekiModel attack = hougekiList.GetData(i);
                        if (attack != null)
                        {
                            Console.WriteLine(attack.ToString());
                            Console.WriteLine("---");
                            // ダメージとクリティカルの集計
                            totalNightDamage += attack.GetDamage();
                        }
                    }

                    // 攻撃パターンの表示
                    foreach (KeyValuePair<string, int> kvp in attackTypeCount)
                    {
                        Console.WriteLine($"    {kvp.Key}: {kvp.Value}回");
                    }

                    Console.WriteLine($"    夜戦総ダメージ: {totalNightDamage}");
                    Console.WriteLine($"    夜戦クリティカル: {nightCriticals}回");
                }
                else
                {
                    Console.WriteLine("    夜戦攻撃は発生しませんでした");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    夜戦攻撃パターン分析中にエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// 夜戦結果の詳細分析を表示
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        public static void PrintNightBattleResultAnalysis(SortieBattleManager battleManager)
        {
            Console.WriteLine("  [夜戦結果]");

            try
            {
                BattleResultModel battleResult = battleManager.GetBattleResult();
                if (battleResult != null)
                {
                    // 最終的な戦果
                    Console.WriteLine($"    最終勝利判定: {battleResult.WinRank}");
                    Console.WriteLine($"    MVP艦: {battleResult.MvpShip?.Name ?? "なし"}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    夜戦結果分析中にエラー: {ex.Message}");
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

                string damageState = GetDamageStateDescription(ship.DmgStateEnd, true);
                Console.WriteLine($"    {i + 1}. {ship.Name}: {ship.HpStart} → {ship.HpEnd} [{damageState}]");
            }
        }

        /// <summary>
        /// 敵艦の詳細状態を表示
        /// </summary>
        /// <param name="enemyShips">敵艦配列</param>
        private static void DisplayDetailedEnemyStatus(ShipModel_BattleResult[] enemyShips)
        {
            int destroyedEnemies = 0;
            int heavilyDamagedEnemies = 0;
            int lightlyDamagedEnemies = 0;
            int totalEnemies = enemyShips.Length;

            Console.WriteLine("  敵艦詳細状況:");
            for (int i = 0; i < enemyShips.Length; i++)
            {
                var enemy = enemyShips[i];
                if (enemy == null) continue;

                string damageState = GetDamageStateDescription(enemy.DmgStateEnd);
                int damageDealt = enemy.HpStart - enemy.HpEnd;
                double damagePercent = (damageDealt * 100.0) / enemy.HpStart;

                Console.WriteLine($"    {i + 1}. {enemy.Name}: {enemy.HpStart} → {enemy.HpEnd} [{damageState}] (-{damageDealt}HP)");

                // 統計の更新
                if (enemy.DmgStateEnd == DamageState_Battle.Gekichin || enemy.HpEnd <= 0)
                {
                    destroyedEnemies++;
                }
                else if (enemy.DmgStateEnd >= DamageState_Battle.Taiha)
                {
                    heavilyDamagedEnemies++;
                }
                else if (enemy.DmgStateEnd >= DamageState_Battle.Shouha)
                {
                    lightlyDamagedEnemies++;
                }
            }

            Console.WriteLine($"  敵艦戦果: 撃沈 {destroyedEnemies}隻 / 大破 {heavilyDamagedEnemies}隻 / 小破 {lightlyDamagedEnemies}隻 / 無傷 {totalEnemies - destroyedEnemies - heavilyDamagedEnemies - lightlyDamagedEnemies}隻");
        }

        /// <summary>
        /// 味方艦の詳細状態を表示
        /// </summary>
        /// <param name="friendlyShips">味方艦配列</param>
        private static void DisplayDetailedFriendlyStatus(ShipModel_BattleResult[] friendlyShips)
        {
            int sunkShips = 0;
            int heavilyDamagedShips = 0;
            int lightlyDamagedShips = 0;
            int intactShips = 0;

            Console.WriteLine("  味方艦隊詳細状況:");
            for (int i = 0; i < friendlyShips.Length; i++)
            {
                var ship = friendlyShips[i];
                if (ship == null) continue;

                string damageState = GetDamageStateDescription(ship.DmgStateEnd, true);
                int damageReceived = ship.HpStart - ship.HpEnd;

                Console.WriteLine($"    {i + 1}. {ship.Name}: {ship.HpStart} → {ship.HpEnd} [{damageState}] (-{damageReceived}HP)");

                // 統計の更新
                if (ship.DmgStateEnd == DamageState_Battle.Gekichin)
                {
                    sunkShips++;
                }
                else if (ship.DmgStateEnd >= DamageState_Battle.Taiha)
                {
                    heavilyDamagedShips++;
                }
                else if (ship.DmgStateEnd >= DamageState_Battle.Shouha)
                {
                    lightlyDamagedShips++;
                }
                else
                {
                    intactShips++;
                }
            }

            Console.WriteLine($"  味方損害: 轟沈 {sunkShips}隻 / 大破 {heavilyDamagedShips}隻 / 小破 {lightlyDamagedShips}隻 / 無傷 {intactShips}隻");
        }

        /// <summary>
        /// 損傷状態の説明を取得
        /// </summary>
        /// <param name="damageState">損傷状態</param>
        /// <param name="isFriendly">味方艦かどうか</param>
        /// <returns>損傷状態の説明</returns>
        private static string GetDamageStateDescription(DamageState_Battle damageState, bool isFriendly = false)
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
                    return isFriendly ? "轟沈" : "撃沈";
                default:
                    return "不明";
            }
        }

        /// <summary>
        /// 夜戦開始時の艦娘状態を記録
        /// </summary>
        /// <param name="battleManager">戦闘マネージャー</param>
        public static void RecordNightBattleStartState(SortieBattleManager battleManager)
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
        /// 探照灯の情報を確認
        /// </summary>
        /// <param name="nightCombatModel">夜戦モデル</param>
        public static void CheckSearchLightInfo(NightCombatModel nightCombatModel)
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
        public static void CheckFlareInfo(NightCombatModel nightCombatModel)
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

        private static string GetFormationName(BattleFormationKinds1 formation)
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
        /// 艦隊状態を表示
        /// </summary>
        /// <param name="sortieMapManager">出撃マップマネージャー</param>
        public static void PrintFleetStatus(SortieMapManager sortieMapManager)
        {
            const string header = "\n=== 出撃艦隊の状態 ===";
            Console.WriteLine(header);

            try
            {
                var ships = sortieMapManager?.Deck?.GetShips();
                if (ships == null || ships.Length == 0)
                {
                    Console.WriteLine("  艦隊情報が取得できませんでした");
                    return;
                }

                Console.WriteLine($"  艦隊規模: {ships.Length}隻");
                PrintShipDetails(ships);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  艦隊状態の表示中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 敵艦隊の情報を表示
        /// </summary>
        /// <param name="sortieMapManager">出撃マップマネージャー</param>
        public static void PrintEnemyFleetInfo(SortieMapManager sortieMapManager)
        {
            const string header = "\n=== 敵艦隊の情報 ===";
            Console.WriteLine(header);

            try
            {
                if (sortieMapManager?.Map == null)
                {
                    Console.WriteLine("  マップ情報が取得できませんでした");
                    return;
                }

                PrintMapInfo(sortieMapManager.Map);
                PrintEnemyShips(sortieMapManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  敵艦隊情報の表示中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 艦娘IDから艦娘の名前を取得
        /// </summary>
        /// <param name="shipId">艦娘ID</param>
        /// <returns>艦娘の名前</returns>
        public static string GetShipName(int shipId)
        {
            if (shipId <= 0)
                return $"無効な艦娘ID (ID: {shipId})";

            try
            {
                var shipData = Mst_DataManager.Instance.Mst_ship[shipId];
                return shipData?.Name ?? $"不明な艦娘 (ID: {shipId})";
            }
            catch (KeyNotFoundException)
            {
                return $"存在しない艦娘 (ID: {shipId})";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"艦娘名の取得に失敗しました (ID: {shipId}): {ex.Message}");
                return $"不明な艦娘 (ID: {shipId})";
            }
        }

        /// <summary>
        /// 艦娘の状態を表示
        /// </summary>
        /// <param name="currentHp">現在HP</param>
        /// <param name="maxHp">最大HP</param>
        /// <returns>HP状態の日本語文字列</returns>
        public static string GetHpStatusText(int currentHp, int maxHp)
        {
            if (maxHp <= 0) return "不明";
            if (currentHp < 0) currentHp = 0;

            var hpRatio = (double)currentHp / maxHp;

            if (hpRatio >= 1.0) return "無傷";
            if (hpRatio > 0.5) return "小破";
            if (hpRatio > 0.25) return "中破";
            return "大破";
        }

        /// <summary>
        /// 艦娘の詳細情報を表示
        /// </summary>
        private static void PrintShipDetails(object[] ships)
        {
            for (int i = 0; i < ships.Length; i++)
            {
                var ship = ships[i];
                if (ship == null) continue;

                var shipInfo = GetShipInfo(ship, i + 1);
                Console.WriteLine(shipInfo);
            }
        }

        /// <summary>
        /// 艦娘の表示用情報を取得
        /// </summary>
        private static string GetShipInfo(dynamic ship, int position)
        {
            try
            {
                var statusText = GetHpStatusText(ship.NowHp, ship.MaxHp);

                return $"  {position}. {ship.Name} (Lv.{ship.Level}) " +
                       $"HP:{ship.NowHp}/{ship.MaxHp} [{statusText}]";
            }
            catch (Exception)
            {
                return $"  {position}. 情報取得エラー";
            }
        }

        /// <summary>
        /// マップ情報を表示
        /// </summary>
        private static void PrintMapInfo(dynamic mapInfo)
        {
            try
            {
                Console.WriteLine($"  マップ: {mapInfo.AreaId}-{mapInfo.No} ({mapInfo.Name})");
            }
            catch (Exception)
            {
                Console.WriteLine("  マップ情報の表示に失敗しました");
            }
        }

        /// <summary>
        /// 敵艦隊の詳細を表示
        /// </summary>
        private static void PrintEnemyShips(SortieMapManager sortieMapManager)
        {
            try
            {
                var enemyShips = sortieMapManager.GetNextCellEnemys();

                if (enemyShips == null || enemyShips.Count == 0)
                {
                    Console.WriteLine("  敵艦隊の情報が取得できませんでした");
                    return;
                }

                Console.WriteLine($"  敵艦隊規模: {enemyShips.Count}隻");

                for (int i = 0; i < enemyShips.Count; i++)
                {
                    var enemy = enemyShips[i];
                    if (enemy == null) continue;

                    var enemyInfo = GetEnemyInfo(enemy, i + 1);
                    Console.WriteLine(enemyInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  敵艦隊詳細の表示に失敗しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 敵艦の表示用情報を取得
        /// </summary>
        private static string GetEnemyInfo(dynamic enemy, int position)
        {
            try
            {
                return $"  {position}. {enemy.Name} HP:{enemy.Taikyu} " +
                       $"(火力:{enemy.Karyoku}/雷装:{enemy.Raisou})";
            }
            catch (Exception)
            {
                return $"  {position}. 敵艦情報取得エラー";
            }
        }

        /// <summary>
        /// 戦闘実行開始メッセージを表示
        /// </summary>
        public static void PrintBattleExecutionStart()
        {
            Console.WriteLine("  戦闘を実行中...");
        }

        /// <summary>
        /// StrategyMapManager初期化メッセージを表示
        /// </summary>
        public static void PrintStrategyMapManagerInitialization()
        {
            Console.WriteLine("  StrategyMapManagerを初期化中...");
        }

        /// <summary>
        /// エリア選択メッセージを表示
        /// </summary>
        /// <param name="areaId">エリアID</param>
        public static void PrintAreaSelection(int areaId)
        {
            Console.WriteLine($"  エリア{areaId}を選択中...");
        }

        /// <summary>
        /// マップ出撃メッセージを表示
        /// </summary>
        /// <param name="areaId">エリアID</param>
        /// <param name="mapNo">マップ番号</param>
        /// <param name="mapId">実際のマップID</param>
        public static void PrintMapSortie(int areaId, int mapNo, int mapId)
        {
            Console.WriteLine($"  マップ{areaId}-{mapNo}に出撃中...");
            Console.WriteLine($"  実際のマップID: {mapId}");
        }

        /// <summary>
        /// 出撃成功メッセージを表示
        /// </summary>
        /// <param name="areaId">エリアID</param>
        /// <param name="mapNo">マップ番号</param>
        public static void PrintSortieSuccess(int areaId, int mapNo)
        {
            Console.WriteLine($"  出撃成功！（エリア{areaId}、マップ{mapNo}）");
        }

        /// <summary>
        /// 戦闘指揮準備メッセージを表示
        /// </summary>
        public static void PrintCommandPreparation()
        {
            Console.WriteLine("  戦闘指揮を準備中...");
        }

        /// <summary>
        /// コマンドフェーズモデル取得成功メッセージを表示
        /// </summary>
        public static void PrintCommandPhaseModelSuccess()
        {
            Console.WriteLine("  コマンドフェーズモデルを取得しました");
        }

        /// <summary>
        /// 戦闘指揮実行結果を表示
        /// </summary>
        /// <param name="result">実行結果</param>
        /// <param name="commandPhaseModel">コマンドフェーズモデル</param>
        public static void PrintCommandExecutionResult(bool result, dynamic commandPhaseModel)
        {
            Console.WriteLine($"  戦闘指揮の実行結果: {result}: {commandPhaseModel}");
        }

        /// <summary>
        /// 戦闘指揮実行完了メッセージを表示
        /// </summary>
        public static void PrintCommandExecutionComplete()
        {
            Console.WriteLine("  戦闘指揮の実行完了");
        }

        /// <summary>
        /// 昼戦計算中メッセージを表示
        /// </summary>
        public static void PrintDayBattleCalculation()
        {
            Console.WriteLine("  昼戦結果を計算中...");
        }

        /// <summary>
        /// 昼戦完了メッセージを表示
        /// </summary>
        public static void PrintDayBattleComplete()
        {
            Console.WriteLine("  昼戦完了！");
        }

        /// <summary>
        /// 夜戦突入可能メッセージを表示
        /// </summary>
        public static void PrintNightBattleAvailable()
        {
            Console.WriteLine("  夜戦突入可能です");
        }

        /// <summary>
        /// 夜戦開始メッセージを表示
        /// </summary>
        public static void PrintNightBattleStart()
        {
            Console.WriteLine("  夜戦を開始します...");
        }

        /// <summary>
        /// 夜戦前処理メッセージを表示
        /// </summary>
        public static void PrintNightBattlePreProcessing()
        {
            Console.WriteLine("  夜戦の前処理を実行中...");
        }

        /// <summary>
        /// 夜戦データ取得メッセージを表示
        /// </summary>
        public static void PrintNightBattleDataRetrieval()
        {
            Console.WriteLine("    夜戦データを取得中...");
        }

        /// <summary>
        /// 夜戦データ準備メッセージを表示
        /// </summary>
        public static void PrintNightBattleDataPreparation()
        {
            Console.WriteLine("    夜戦用データを準備中...");
        }

        /// <summary>
        /// 夜戦データ取得成功メッセージを表示
        /// </summary>
        public static void PrintNightBattleDataSuccess()
        {
            Console.WriteLine("    夜戦データを取得しました");
        }

        /// <summary>
        /// 夜戦攻撃データ準備完了メッセージを表示
        /// </summary>
        /// <param name="attackCount">攻撃数</param>
        public static void PrintNightBattleAttackDataReady(int attackCount)
        {
            Console.WriteLine($"    夜戦攻撃データを準備しました（攻撃数: {attackCount}）");
        }

        /// <summary>
        /// 夜戦前処理完了メッセージを表示
        /// </summary>
        public static void PrintNightBattlePreProcessingComplete()
        {
            Console.WriteLine("  夜戦の前処理完了");
        }

        /// <summary>
        /// 夜戦中メッセージを表示
        /// </summary>
        public static void PrintNightBattleInProgress()
        {
            Console.WriteLine("  夜戦中...");
        }

        /// <summary>
        /// 夜戦結果取得メッセージを表示
        /// </summary>
        public static void PrintNightBattleResultRetrieved()
        {
            Console.WriteLine("  夜戦結果を取得しました");
        }

        /// <summary>
        /// 夜戦完了メッセージを表示
        /// </summary>
        public static void PrintNightBattleComplete()
        {
            Console.WriteLine("  夜戦完了！");
        }

        /// <summary>
        /// 夜戦スキップメッセージを表示
        /// </summary>
        /// <param name="reason">スキップ理由</param>
        public static void PrintNightBattleSkipped(string reason)
        {
            Console.WriteLine($"  {reason}");
        }

        /// <summary>
        /// エラーメッセージを表示
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public static void PrintError(string message)
        {
            Console.WriteLine($"  エラー: {message}");
        }

        /// <summary>
        /// 攻撃タイプの説明を取得
        /// </summary>
        private static string GetAttackTypeDescription(Common.Enum.BattleAttackKind attackType)
        {
            switch (attackType)
            {
                case Common.Enum.BattleAttackKind.Normal:
                    return "通常攻撃";
                case Common.Enum.BattleAttackKind.Bakurai:
                    return "爆雷攻撃";
                case Common.Enum.BattleAttackKind.Gyorai:
                    return "魚雷攻撃";
                case Common.Enum.BattleAttackKind.AirAttack:
                    return "航空攻撃";
                case Common.Enum.BattleAttackKind.Laser:
                    return "レーザー攻撃";
                case Common.Enum.BattleAttackKind.Renzoku:
                    return "連続攻撃";
                case Common.Enum.BattleAttackKind.Sp1:
                    return "特殊攻撃1";
                case Common.Enum.BattleAttackKind.Sp2:
                    return "特殊攻撃2";
                case Common.Enum.BattleAttackKind.Sp3:
                    return "特殊攻撃3";
                case Common.Enum.BattleAttackKind.Sp4:
                    return "特殊攻撃4";
                case Common.Enum.BattleAttackKind.Syu_Rai:
                    return "主砲+魚雷カットイン";
                case Common.Enum.BattleAttackKind.Rai_Rai:
                    return "魚雷×2カットイン";
                case Common.Enum.BattleAttackKind.Syu_Syu_Fuku:
                    return "主砲×2+副砲カットイン";
                case Common.Enum.BattleAttackKind.Syu_Syu_Syu:
                    return "主砲×3カットイン";
                default:
                    return "不明な攻撃";
            }
        }
    }
}
