using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using KCV;
using KCV.Strategy;

using Server_Common;
using Server_Models;
using Server_Controllers;
using Common.Enum;

using local.models;
using local.models.battle;
using local.utils;
using local.managers;

namespace Runner
{
    class Program
    {
        /// <summary>
        /// 装備IDから名称を取得する
        /// </summary>
        /// <param name="itemId">装備ID</param>
        /// <returns>装備名称（未登録の場合はID表示）</returns>
        private static string GetSlotItemName(int itemId)
        {
            if (Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(itemId, out Mst_slotitem slotitem))
            {
                return slotitem.Name;
            }
            return $"装備ID:{itemId}";
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== プログラム開始 ===");

            // システムの初期化
            Console.WriteLine("システムの初期化を開始します...");
            InitializeSystem();
            Console.WriteLine("システムの初期化が完了しました。");

            // デバッグ用の初期データ設定
            Console.WriteLine("\nデバッグ用の初期データ設定を開始します...");
            SetupDebugData();
            Console.WriteLine("デバッグデータの設定が完了しました。");

            // 戦闘のシミュレーション実行
            Console.WriteLine("\n戦闘のシミュレーション実行を開始します...");
            RunBattleSimulation();
            Console.WriteLine("戦闘シミュレーションが完了しました。");

            Console.WriteLine("\n=== プログラムが正常に終了しました ===");
        }

        /// <summary>
        /// システムの初期化を行う
        /// </summary>
        private static void InitializeSystem()
        {
            Console.WriteLine("  AppInitializeManager.Awake() を実行中...");
            AppInitializeManager.Awake();

            Console.WriteLine("  App.InitSystems() を実行中...");
            App.InitSystems();

            Console.WriteLine("  ManagerBase.initialize() を実行中...");
            ManagerBase.initialize();
        }

        /// <summary>
        /// デバッグ用のデータを設定する
        /// </summary>
        private static void SetupDebugData()
        {
            Debug_Mod debug_Mod = new Debug_Mod();

            // 艦娘の追加
            Console.WriteLine("  艦娘の追加を開始します...");
            SetupShips(debug_Mod);

            // 編成の設定
            Console.WriteLine("  編成の設定を開始します...");
            SetupOrganization(debug_Mod);

            // 消費アイテムの設定
            Console.WriteLine("  消費アイテムの設定を開始します...");
            SetupUseItems(debug_Mod);

            // 資源の設定
            Console.WriteLine("  資源の設定を開始します...");
            SetupMaterials(debug_Mod);

            // 装備の設定
            Console.WriteLine("  装備の設定を開始します...");
            SetupSlotItems(debug_Mod);

            DebugUtils.SLog("DEBUG_MOD OK");
        }

        /// <summary>
        /// 艦娘を追加する
        /// </summary>
        /// <param name="debugMod">デバッグ用のModオブジェクト</param>
        private static void SetupShips(Debug_Mod debugMod)
        {
            List<int> shipList = new List<int>();

            Debug.Log("ADD SHIP");
            debugMod.Add_Ship(shipList);

            // 特定の艦娘を追加
            shipList.Add(330); // 秋月改
            shipList.Add(24);  // 大井
            shipList.Add(175); // Z3
            shipList.Add(117); // 瑞鳳改
            shipList.Add(75);  // 飛鷹
            shipList.Add(321); // 大淀改
            shipList.Add(182); // 明石

            Console.WriteLine($"    特定の艦娘を {shipList.Count} 隻追加しました");

            // 連番の艦娘を追加
            int initialCount = shipList.Count;
            for (int i = 100; i < 110; i++)
            {
                shipList.Add(i);
            }

            Console.WriteLine($"    連番の艦娘を {shipList.Count - initialCount} 隻追加しました");

            debugMod.Add_Ship(shipList);
            Console.WriteLine($"    合計 {shipList.Count} 隻の艦娘を追加完了");
        }

        /// <summary>
        /// 編成を設定する
        /// </summary>
        /// <param name="debugMod">デバッグ用のModオブジェクト</param>
        private static void SetupOrganization(Debug_Mod debugMod)
        {
            var userData = Comm_UserDatas.Instance;
            OrganizeManager organizeManager = new OrganizeManager(1);

            // 艦隊を追加（2～6番艦隊）
            for (int deckNumber = 2; deckNumber <= 6; deckNumber++)
            {
                debugMod.Add_Deck(deckNumber);
                Console.WriteLine($"    {deckNumber}番艦隊を追加しました");
            }

            // 艦隊編成の変更
            organizeManager.ChangeOrganize(1, 2, 2);
            Console.WriteLine("    1番艦隊の2番位置に艦娘ID:2を配置しました");

            organizeManager.ChangeOrganize(1, 3, 3);
            Console.WriteLine("    1番艦隊の3番位置に艦娘ID:3を配置しました");
        }

        /// <summary>
        /// 消費アイテムを設定する
        /// </summary>
        /// <param name="debugMod">デバッグ用のModオブジェクト</param>
        private static void SetupUseItems(Debug_Mod debugMod)
        {
            Dictionary<int, int> useItemsDict = new Dictionary<int, int>();

            // 特定の範囲のアイテムを設定
            int itemCount = 0;
            for (int itemId = 0; itemId < 100; itemId++)
            {
                if ((1 <= itemId && itemId <= 3) ||
                    (10 <= itemId && itemId <= 12) ||
                    (49 <= itemId && itemId <= 59))
                {
                    useItemsDict[itemId] = 1;
                    itemCount++;
                }
            }

            // 特定アイテムの個数を設定
            useItemsDict[54] = 0;
            useItemsDict[59] = 10;

            debugMod.Add_UseItem(useItemsDict);
            Console.WriteLine($"    消費アイテムを {itemCount} 種類設定しました");
            Console.WriteLine("    特定アイテム（ID:54=0個, ID:59=10個）を設定しました");
        }

        /// <summary>
        /// 資源を設定する
        /// </summary>
        /// <param name="debugMod">デバッグ用のModオブジェクト</param>
        private static void SetupMaterials(Debug_Mod debugMod)
        {
            const int materialAmount = 2000;

            // 各種資源を追加
            debugMod.Add_Materials(enumMaterialCategory.Fuel, materialAmount);        // 燃料
            debugMod.Add_Materials(enumMaterialCategory.Bull, materialAmount);        // 弾薬
            debugMod.Add_Materials(enumMaterialCategory.Steel, materialAmount);       // 鋼材
            debugMod.Add_Materials(enumMaterialCategory.Bauxite, materialAmount);     // ボーキサイト
            debugMod.Add_Materials(enumMaterialCategory.Repair_Kit, materialAmount);  // 修理キット
            debugMod.Add_Materials(enumMaterialCategory.Dev_Kit, materialAmount);     // 開発キット
            debugMod.Add_Materials(enumMaterialCategory.Revamp_Kit, materialAmount);  // 改修キット
            debugMod.Add_Materials(enumMaterialCategory.Build_Kit, materialAmount);   // 建造キット

            Console.WriteLine($"    各種資源を {materialAmount} ずつ設定しました");
            Console.WriteLine("    (燃料、弾薬、鋼材、ボーキサイト、修理キット、開発キット、改修キット、建造キット)");

            // コインを追加
            debugMod.Add_Coin(80000);
            Console.WriteLine("    コインを 80,000 追加しました");
        }

        /// <summary>
        /// 装備アイテムを設定する
        /// </summary>
        /// <param name="debugMod">デバッグ用のModオブジェクト</param>
        private static void SetupSlotItems(Debug_Mod debugMod)
        {
            List<int> slotItemList = new List<int>();

            // 基本装備を6個追加
            for (int i = 0; i < 6; i++)
            {
                slotItemList.Add(1);
            }
            Console.WriteLine($"    {GetSlotItemName(1)}を 6個 追加しました");

            // 特定装備を30個追加
            for (int i = 0; i < 30; i++)
            {
                slotItemList.Add(14);
            }
            Console.WriteLine($"    {GetSlotItemName(14)}を 30個 追加しました");

            // 連番装備を追加
            int beforeCount = slotItemList.Count;
            for (int i = 1; i < 100; i++)
            {
                slotItemList.Add(i);
            }
            Console.WriteLine($"    連番装備(ID:1-99)を {slotItemList.Count - beforeCount}個 追加しました");

            // 特定装備を30個追加
            for (int i = 0; i < 30; i++)
            {
                slotItemList.Add(25);
            }
            Console.WriteLine($"    {GetSlotItemName(25)}を 30個 追加しました");

            // 特定装備を6個追加
            for (int i = 0; i < 6; i++)
            {
                slotItemList.Add(42);
            }
            Console.WriteLine($"    {GetSlotItemName(42)}を 6個 追加しました");

            // 装備のレベルを設定
            int levelUpCount = 0;
            for (int i = 1; i < 100; i++)
            {
                if (i < slotItemList.Count)
                {
                    Debug_Mod.ChangeSlotLevel(slotItemList[i], 9);
                    levelUpCount++;
                }
            }
            Console.WriteLine($"    装備のレベルを {levelUpCount}個 レベル9に設定しました");

            debugMod.Add_SlotItem(slotItemList);
            Console.WriteLine($"    合計 {slotItemList.Count}個 の装備を追加完了");
        }

        /// <summary>
        /// 戦闘のシミュレーションを実行する
        /// </summary>
        private static void RunBattleSimulation()
        {
            // 戦略画面のタスクマネージャーを初期化
            Console.WriteLine("  戦略画面のタスクマネージャーを初期化中...");
            StrategyTopTaskManager top = new StrategyTopTaskManager();

            // マップ管理とエリア選択
            Console.WriteLine("  マップ管理を初期化中...");
            StrategyMapManager strategyMapManager = new StrategyMapManager();

            Console.WriteLine("  エリア1を選択中...");
            SortieManager sortieManager = strategyMapManager.SelectArea(1);

            // 出撃開始
            Console.WriteLine("  出撃を開始中... (エリア1, マップ11)");
            SortieMapManager sortieMapManager = sortieManager.GoSortie(1, 11);

            // 戦闘開始と結果取得
            Console.WriteLine("  戦闘を開始中... (陣形: 単縦陣)");
            SortieBattleManager sortieBattleManager = sortieMapManager.BattleStart(BattleFormationKinds1.TanJuu);

            Console.WriteLine("  戦闘結果を取得中...");
            BattleResultModel battleResultModel = sortieBattleManager.GetBattleResult();

            Console.WriteLine("  戦闘結果を取得完了");
        }
    }
}
