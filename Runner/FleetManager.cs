using System;
using System.Collections.Generic;
using Server_Models;
using KCV.Strategy;
using Common.Enum;
using local.managers;
using Server_Controllers;

namespace Runner
{
    /// <summary>
    /// 艦隊編成の管理を担当するクラス
    /// </summary>
    public class FleetManager
    {
        /// <summary>
        /// 艦娘IDから艦娘の名前を取得
        /// </summary>
        /// <param name="shipId">艦娘ID</param>
        /// <returns>艦娘の名前</returns>
        public static string GetShipName(int shipId)
        {
            try
            {
                return Mst_DataManager.Instance.Mst_ship[shipId].Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  艦娘名の取得に失敗しました (ID: {shipId}): {ex.Message}");
                return $"不明な艦娘 (ID: {shipId})";
            }
        }

        private readonly OrganizeManager _organizeManager;

        public FleetManager(int fleetId = 1)
        {
            _organizeManager = new OrganizeManager(fleetId);
        }

        /// <summary>
        /// 艦隊編成を準備
        /// </summary>
        /// <param name="fleetId">艦隊番号</param>
        /// <param name="shipCount">編成する艦数</param>
        public void PrepareFleetOrganization(int fleetId = 1, int shipCount = 6)
        {
            Console.WriteLine("  艦隊編成を準備中...");
            Console.WriteLine($"  {fleetId}番艦隊に艦娘を配備中...");

            // 艦隊に艦娘を配置（2番位置から開始、1番位置は初期艦娘）
            for (int position = 2; position <= Math.Min(shipCount, 6); position++)
            {
                _organizeManager.ChangeOrganize(fleetId, position, position);
            }

            Console.WriteLine($"  {fleetId}番艦隊に{shipCount}隻の艦娘を配備完了");
            Console.WriteLine("  艦隊編成準備完了");
        }

        /// <summary>
        /// 艦隊状態を表示
        /// </summary>
        /// <param name="sortieMapManager">出撃マップマネージャー</param>
        public static void DisplayFleetStatus(SortieMapManager sortieMapManager)
        {
            Console.WriteLine("\n=== 出撃艦隊の状態 ===");

            try
            {
                var fleet = sortieMapManager.Deck;
                var ships = fleet.GetShips();

                if (ships == null || ships.Length == 0)
                {
                    Console.WriteLine("  艦隊情報が取得できませんでした");
                    return;
                }

                Console.WriteLine($"  艦隊規模: {ships.Length}隻");
                Console.WriteLine("  ----------------------------------------");

                for (int i = 0; i < ships.Length; i++)
                {
                    var ship = ships[i];
                    if (ship == null) continue;

                    Console.WriteLine($"  {i + 1}番艦: {ship.Name} (Lv.{ship.Level})");
                    Console.WriteLine($"    艦種: {ship.ShipTypeName}");
                    Console.WriteLine($"    HP: {ship.NowHp}/{ship.MaxHp}");
                    Console.WriteLine($"    火力: {ship.Karyoku} 雷装: {ship.Raisou}");
                    Console.WriteLine("  ----------------------------------------");
                }
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
        public static void DisplayEnemyFleetInfo(SortieMapManager sortieMapManager)
        {
            Console.WriteLine("\n=== 敵艦隊の情報 ===");

            try
            {
                var mapInfo = sortieMapManager.Map;
                Console.WriteLine($"  マップ: {mapInfo.AreaId}-{mapInfo.No}");
                Console.WriteLine($"  マップ名: {mapInfo.Name}");

                var enemyShips = sortieMapManager.GetNextCellEnemys();

                if (enemyShips == null || enemyShips.Count == 0)
                {
                    Console.WriteLine("  敵艦隊の情報が取得できませんでした");
                    return;
                }

                Console.WriteLine($"  敵艦隊規模: {enemyShips.Count}隻");
                Console.WriteLine("  ----------------------------------------");

                for (int i = 0; i < enemyShips.Count; i++)
                {
                    var enemy = enemyShips[i];
                    if (enemy == null) continue;

                    Console.WriteLine($"  {i + 1}番艦: {enemy.Name}");
                    Console.WriteLine($"    艦種: {enemy.ShipTypeName}");
                    Console.WriteLine($"    HP: {enemy.Taikyu}");
                    Console.WriteLine($"    火力: {enemy.Karyoku} 雷装: {enemy.Raisou}");
                    Console.WriteLine("  ----------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  敵艦隊情報の表示中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}
