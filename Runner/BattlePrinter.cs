using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enum;
using local.models.battle;
using local.models;
using local.managers;

namespace Runner
{
    /// <summary>
    /// 標準出力に戦闘情報を表示するユーティリティクラス
    /// </summary>
    public static class BattlePrinter
    {
        /// <summary>
        /// 戦闘指揮情報を表示
        /// </summary>
        public static void PrintCommandPhaseInfo(SortieBattleManager battleManager)
        {
            Console.WriteLine("  戦闘指揮を準備中...");
            var commandPhaseModel = battleManager.GetCommandPhaseModel();

            if (commandPhaseModel == null)
            {
                Console.WriteLine("  コマンドフェーズモデルの取得に失敗しました");
                return;
            }

            Console.WriteLine("  コマンドフェーズモデルを取得しました");

            // 戦闘指揮を取得
            var presetCommands = commandPhaseModel.GetPresetCommand();
            var selectableCommands = commandPhaseModel.GetSelectableCommands().ToList();

            PrintCommandInfo(presetCommands, selectableCommands);

            if (presetCommands.Count == 0)
            {
                Console.WriteLine("  戦闘指揮がありません");
                return;
            }

            // 戦闘指揮を実行
            bool commandResult = commandPhaseModel.SetCommand(presetCommands);
            Console.WriteLine($"  戦闘指揮の実行結果: {commandResult}: {commandPhaseModel}");

            if (!commandResult)
            {
                Console.WriteLine("  戦闘指揮の実行に失敗しました");
                return;
            }

            Console.WriteLine("  戦闘指揮の実行完了");
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
        /// 損傷状態の説明を取得
        /// </summary>
        /// <param name="damageState">損傷状態</param>
        /// <param name="isFriendly">味方艦かどうか</param>
        /// <returns>損傷状態の説明</returns>
        private static string GetDamageStateDescription(DamageState_Battle damageState, bool isFriendly = false)
        {
            if (isFriendly && (damageState == DamageState_Battle.Gekichin))
            {
                return "轟沈";
            }
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
    }
}
