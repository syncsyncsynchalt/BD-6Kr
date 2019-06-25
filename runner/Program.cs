using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using KCV;
using Server_Common;
using Server_Models;
using Server_Controllers;
using local.models;
using local.managers;
using Common.Enum;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            AppInitializeManager.Awake();
            App.InitSystems();

            Debug_Mod debug_Mod = new Debug_Mod();
            List<int> list = new List<int>();
            Debug.Log("ADD SHIP");
            debug_Mod.Add_Ship(list);
            list.Add(330);
            list.Add(24);
            list.Add(175);
            list.Add(117);
            list.Add(75);
            list.Add(321);
            list.Add(182);
            for (int i = 100; i < 110; i++)
            {
                list.Add(i);
            }
            debug_Mod.Add_Ship(list);

            var userData = Comm_UserDatas.Instance;



            // var ui = (new LocalManager()).UserInfo;
            // LocalManager localManager = new LocalManager();

            OrganizeManager organizeManager = new OrganizeManager(1);
            debug_Mod.Add_Deck(2);
            debug_Mod.Add_Deck(3);
            debug_Mod.Add_Deck(4);
            debug_Mod.Add_Deck(5);
            debug_Mod.Add_Deck(6);
            ManagerBase.initialize();
            organizeManager.ChangeOrganize(1, 2, 2);

            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int j = 0; j < 100; j++)
            {
                if ((1 <= j && j <= 3) || (10 <= j && j <= 12) || (49 <= j && j <= 59))
                {
                    dictionary[j] = 1;
                }
                dictionary[54] = 0;
                dictionary[59] = 10;
            }
            debug_Mod.Add_UseItem(dictionary);
            debug_Mod.Add_Materials(enumMaterialCategory.Fuel, 2000);
            debug_Mod.Add_Materials(enumMaterialCategory.Bull, 2000);
            debug_Mod.Add_Materials(enumMaterialCategory.Steel, 2000);
            debug_Mod.Add_Materials(enumMaterialCategory.Bauxite, 2000);
            debug_Mod.Add_Materials(enumMaterialCategory.Repair_Kit, 2000);
            debug_Mod.Add_Materials(enumMaterialCategory.Dev_Kit, 2000);
            debug_Mod.Add_Materials(enumMaterialCategory.Revamp_Kit, 2000);
            debug_Mod.Add_Materials(enumMaterialCategory.Build_Kit, 2000);
            debug_Mod.Add_Coin(80000);
            List<int> list2 = new List<int>();
            list2.Add(1);
            list2.Add(1);
            list2.Add(1);
            list2.Add(1);
            list2.Add(1);
            list2.Add(1);
            for (int k = 0; k < 30; k++)
            {
                list2.Add(14);
            }
            for (int l = 1; l < 100; l++)
            {
                list2.Add(l);
            }
            for (int m = 0; m < 30; m++)
            {
                list2.Add(25);
            }
            for (int n = 0; n < 6; n++)
            {
                list2.Add(42);
            }
            for (int num = 1; num < 100; num++)
            {
                Debug_Mod.ChangeSlotLevel(list2[num], 9);
            }
            debug_Mod.Add_SlotItem(list2);
            DebugUtils.SLog("DEBUG_MOD OK");

            StrategyMapManager strategyMapManager = new StrategyMapManager();
            SortieManager sortieManager = strategyMapManager.SelectArea(1);
            var _clsSortieMapManager = sortieManager.GoSortie(1, 11);
            sortieManager.GoSortie(1, 11);
            var _clsSortieBattleManager = _clsSortieMapManager.BattleStart(BattleFormationKinds1.FukuJuu);

            var res = _clsSortieBattleManager.GetBattleResult();

            var bgm = Mst_DataManager.Instance.GetMstBgm();
            // var cabinet = Mst_DataManager.Instance.GetMstCabinet();
            // var payitem = Mst_DataManager.Instance.GetPayitem();
            // var furnitureText = Mst_DataManager.Instance.GetFurnitureText();

            Console.WriteLine("hajimarimasu");
        }
    }
}
