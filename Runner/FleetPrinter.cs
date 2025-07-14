using System;
using System.Collections.Generic;
using Server_Models;
using local.managers;

namespace Runner
{
    /// <summary>
    /// 標準出力に艦隊情報を表示するユーティリティクラス
    /// </summary>
    public static class FleetPrinter
    {
        /// <summary>
        /// 艦娘IDから艦娘の名前を取得
        /// </summary>
        /// <param name="shipId">艦娘ID</param>
        /// <returns>艦娘の名前</returns>
        public static string ShipName(int shipId)
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
        public static string HpStatusText(int currentHp, int maxHp)
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
        /// 艦隊状態を表示
        /// </summary>
        /// <param name="sortieMapManager">出撃マップマネージャー</param>
        public static void FleetStatus(SortieMapManager sortieMapManager)
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
                ShipDetails(ships);
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
        public static void EnemyFleetInfo(SortieMapManager sortieMapManager)
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

                MapInfo(sortieMapManager.Map);
                EnemyShips(sortieMapManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  敵艦隊情報の表示中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 艦娘の詳細情報を表示
        /// </summary>
        private static void ShipDetails(object[] ships)
        {
            for (int i = 0; i < ships.Length; i++)
            {
                var ship = ships[i];
                if (ship == null) continue;

                var shipInfo = ShipInfo(ship, i + 1);
                Console.WriteLine(shipInfo);
            }
        }

        /// <summary>
        /// 艦娘の表示用情報を取得
        /// </summary>
        private static string ShipInfo(dynamic ship, int position)
        {
            try
            {
                var statusText = HpStatusText(ship.NowHp, ship.MaxHp);

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
        private static void MapInfo(dynamic mapInfo)
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
        private static void EnemyShips(SortieMapManager sortieMapManager)
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

                    var enemyInfo = EnemyInfo(enemy, i + 1);
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
        private static string EnemyInfo(dynamic enemy, int position)
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
    }
}
