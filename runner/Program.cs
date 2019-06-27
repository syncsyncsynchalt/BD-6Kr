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
        static void Main(string[] args)
        {
            AppInitializeManager.Awake();
            App.InitSystems(); // 26 fuso

            Debug_Mod debug_Mod = new Debug_Mod();
            List<int> list = new List<int>();
            Debug.Log("ADD SHIP");
            debug_Mod.Add_Ship(list);
            list.Add(330); // Akizuki Kai
            list.Add(24); // Ooi
            list.Add(175); // Z3
            list.Add(117); // Zuihou Kai
            list.Add(75); // Hiyou
            list.Add(321); // Ooyodo Kai
            list.Add(182); // Akashi
            for (int i = 100; i < 110; i++)
            {
                // 100 Tama
                // 101 Kiso
                // 102 Chitose
                // 103 Chiyoda
                // 104 Chitose Kai
                // 105 Chiyoda Kai
                // 106 Chitose A
                // 107 Chiyoda A
                // 108 Chitose-Kou
                // 109 Chiyoda-Kou
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
            organizeManager.ChangeOrganize(1, 3, 3);

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


            // StrategyTopTaskManager top = new StrategyTopTaskManager();
            // top.Awake2();

            StrategyMapManager strategyMapManager = new StrategyMapManager();


            //SortieManager sortieManager = strategyMapManager.SelectArea(1);
            //SortieMapManager _clsSortieMapManager = sortieManager.GoSortie(1, 11);
            //SortieMapManager _clsSortieMapManager = sortieManager.GoSortie(1, 14);
            //SortieBattleManager _clsSortieBattleManager = _clsSortieMapManager.BattleStart(BattleFormationKinds1.TanJuu);
            // var a = KCV.BattleCut.BattleCutPhase.Command;
            // CommandPhaseModel _clscommandPhaseModel = _clsSortieBattleManager.GetCommandPhaseModel();
            // var a = Server_Controllers.BattleLogic.ExecBattleKinds.DayToNight;
            // _clsSortieBattleManager.StartDayToNightBattle();
            //BattleResultModel res = _clsSortieBattleManager.GetBattleResult();

            // var bgm = Mst_DataManager.Instance.GetMstBgm();
            // var cabinet = Mst_DataManager.Instance.GetMstCabinet();
            // var payitem = Mst_DataManager.Instance.GetPayitem();
            // var furnitureText = Mst_DataManager.Instance.GetFurnitureText();

            Console.WriteLine("hajimarimasu");
        }
    }
}
