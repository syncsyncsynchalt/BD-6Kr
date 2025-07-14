using System;
using System.Linq;
using KCV.Strategy;
using local.managers;

namespace Runner
{
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

            Console.WriteLine("  出撃可能なマップを確認中...");                // マスタデータからエリア情報を取得
            try
            {
                if (strategyMapManager.Area.TryGetValue(areaId, out var areaModel))
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
}
