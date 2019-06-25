using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers.BattleLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_SortieBattle : BattleControllerBase
	{
		public Api_req_SortieBattle(Api_req_Map instance)
		{
			mapInstance = instance;
			init();
		}

		protected override void init()
		{
			practiceFlag = false;
			mapInstance.GetBattleShipData(out userData, out enemyData);
			setBattleSubInfo(userData, out userSubInfo);
			setBattleSubInfo(enemyData, out enemySubInfo);
			battleKinds = ExecBattleKinds.None;
			battleCommandParams = new BattleCommandParams(userData);
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override Api_Result<AllBattleFmt> GetDayPreBattleInfo(BattleFormationKinds1 formationKind)
		{
			Mst_mapcell2 nowCell = mapInstance.GetNowCell();
			if (nowCell.Event_2 == enumMapWarType.Midnight)
			{
				Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Api_Result<AllBattleFmt> dayPreBattleInfo = base.GetDayPreBattleInfo(formationKind);
			if (dayPreBattleInfo.state == Api_Result_State.Success)
			{
				Dictionary<int, List<Mst_slotitem>> useShipInfo = null;
				List<int> givenShips = null;
				if (getCombatRationResult(out useShipInfo, out givenShips))
				{
					dayPreBattleInfo.data.DayBattle.Header.UseRationShips = useShipInfo;
				}
			}
			return dayPreBattleInfo;
		}

		public override Api_Result<AllBattleFmt> DayBattle()
		{
			Mst_mapcell2 nowCell = mapInstance.GetNowCell();
			if (nowCell.Event_2 != enumMapWarType.Normal)
			{
				Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return base.DayBattle();
		}

		public Api_Result<AllBattleFmt> AirBattle()
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mst_mapcell2 nowCell = mapInstance.GetNowCell();
			if (nowCell.Event_2 != enumMapWarType.AirBattle && battleKinds != 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (base.allBattleFmt == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			AllBattleFmt allBattleFmt = base.allBattleFmt;
			allBattleFmt.DayBattle.OpeningProduction = battleCommandParams.GetProductionData(userData.GetDeckBattleCommand()[0]);
			if (battleCommandParams.IsEscape)
			{
				allBattleFmt.DayBattle.ValidMidnight = isGoMidnight();
				battleKinds = ExecBattleKinds.DayOnly;
				api_Result.data = allBattleFmt;
				return api_Result;
			}
			using (Exec_AirBattle exec_AirBattle = new Exec_AirBattle(userData, userSubInfo, enemyData, enemySubInfo, allBattleFmt.DayBattle.Search, practice: false))
			{
				allBattleFmt.DayBattle.AirBattle = exec_AirBattle.GetResultData(formationParams, battleCommandParams);
				seikuValue = exec_AirBattle.getSeikuValue();
			}
			allBattleFmt.BattleFormation = (userData.BattleFormation = (enemyData.BattleFormation = formationParams.AfterAirBattle_RewriteBattleFormation2(userData)));
			using (Exec_AirBattle exec_AirBattle2 = new Exec_AirBattle(userData, userSubInfo, enemyData, enemySubInfo, allBattleFmt.DayBattle.Search, practice: false))
			{
				allBattleFmt.DayBattle.AirBattle2 = exec_AirBattle2.GetResultData(formationParams, battleCommandParams);
			}
			allBattleFmt.DayBattle.ValidMidnight = isGoMidnight();
			battleKinds = ExecBattleKinds.DayOnly;
			api_Result.data = allBattleFmt;
			return api_Result;
		}

		public override Api_Result<AllBattleFmt> NightBattle()
		{
			Mst_mapcell2 nowCell = mapInstance.GetNowCell();
			List<enumMapWarType> list = new List<enumMapWarType>();
			list.Add(enumMapWarType.Normal);
			list.Add(enumMapWarType.AirBattle);
			List<enumMapWarType> list2 = list;
			if (!list2.Contains(nowCell.Event_2))
			{
				Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return base.NightBattle();
		}

		public Api_Result<AllBattleFmt> Night_Sp(BattleFormationKinds1 formationKind)
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mst_mapcell2 nowCell = mapInstance.GetNowCell();
			if (nowCell.Event_2 != enumMapWarType.Midnight || battleKinds != 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (userData.ShipData[0].Get_DamageState() == DamageState.Taiha)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			initFormation(formationKind);
			AllBattleFmt allBattleFmt = new AllBattleFmt(userData.Formation, enemyData.Formation, userData.BattleFormation);
			Dictionary<int, List<Mst_slotitem>> useShipInfo = null;
			List<int> givenShips = null;
			if (!getCombatRationResult(out useShipInfo, out givenShips))
			{
				useShipInfo = null;
			}
			using (Exec_Midnight exec_Midnight = new Exec_Midnight(2, seikuValue, userData, userSubInfo, enemyData, enemySubInfo, practice: false))
			{
				allBattleFmt.NightBattle = exec_Midnight.GetResultData(formationParams, battleCommandParams);
				allBattleFmt.NightBattle.Header.UseRationShips = useShipInfo;
			}
			battleKinds = ExecBattleKinds.NightOnly;
			api_Result.data = allBattleFmt;
			base.allBattleFmt = allBattleFmt;
			return api_Result;
		}

		public override Api_Result<BattleResultFmt> BattleResult()
		{
			Api_Result<BattleResultFmt> api_Result = base.BattleResult();
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			List<Mem_ship> activeShips = null;
			List<Mem_ship> inactiveShips = null;
			mapInstance.GetSortieShipDatas(out activeShips, out inactiveShips);
			EscapeInfo escapeInfo = new EscapeInfo(activeShips);
			api_Result.data.EscapeInfo = ((!escapeInfo.ValidEscape()) ? null : escapeInfo);
			if (api_Result.data.GetAirReconnaissanceItems != null)
			{
				mapInstance.updateMapitemGetData(api_Result.data.GetAirReconnaissanceItems);
			}
			battleKinds = ExecBattleKinds.None;
			Mst_mapcell2 nowCell = mapInstance.GetNowCell();
			bool boss = Mst_DataManager.Instance.Mst_mapenemy[enemyData.Enemy_id].Boss == 1;
			new QuestSortie(nowCell.Maparea_id, nowCell.Mapinfo_no, api_Result.data.WinRank, userData.Deck.Rid, userData.ShipData, enemyData.ShipData, boss).ExecuteCheck();
			mapInstance.SetSlotExpChangeValues(this, GetSlotExpBattleData());
			return api_Result;
		}

		public override void GetBattleResultBase(BattleResultBase out_data)
		{
			base.GetBattleResultBase(out_data);
			out_data.Cleard = mapInstance.GetMapClearState();
			out_data.NowCell = mapInstance.GetNowCell();
			out_data.RebellionBattle = mapInstance.IsRebbelion;
			out_data.GetAirCellItems = mapInstance.AirReconnaissanceItems;
		}

		public bool GoBackPort(int escapeRid, int towRid)
		{
			Mem_ship value = null;
			Mem_ship value2 = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(escapeRid, out value))
			{
				return false;
			}
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(towRid, out value2))
			{
				return false;
			}
			if (value.Escape_sts)
			{
				return false;
			}
			if (value2.Escape_sts)
			{
				return false;
			}
			value.ChangeEscapeState();
			value2.ChangeEscapeState();
			return true;
		}

		private bool getCombatRationResult(out Dictionary<int, List<Mst_slotitem>> useShipInfo, out List<int> givenShips)
		{
			int mapBattleCellPassCount = mapInstance.MapBattleCellPassCount;
			useShipInfo = null;
			givenShips = null;
			if (mapBattleCellPassCount < 2)
			{
				return false;
			}
			Dictionary<int, List<Mst_slotitem>> dictionary = new Dictionary<int, List<Mst_slotitem>>();
			List<int> list = new List<int>();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(145);
			hashSet.Add(150);
			HashSet<int> searchIds = hashSet;
			Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
			Dictionary<int, int> dictionary3 = userSubInfo.Keys.ToDictionary((int key) => key, (int value) => 0);
			for (int i = 0; i < userData.ShipData.Count; i++)
			{
				Mem_ship mem_ship = userData.ShipData[i];
				if (!mem_ship.IsFight())
				{
					continue;
				}
				Dictionary<int, List<int>> slotIndexFromId = mem_ship.GetSlotIndexFromId(searchIds);
				if ((slotIndexFromId[145].Count == 0 && slotIndexFromId[150].Count == 0) || !isRationLotteryWinning(mapBattleCellPassCount, mem_ship.Luck))
				{
					continue;
				}
				List<int> rationRecoveryShips = getRationRecoveryShips(i);
				if (rationRecoveryShips.Count != 0)
				{
					dictionary.Add(mem_ship.Rid, new List<Mst_slotitem>());
					int num = 0;
					List<int> list2 = new List<int>();
					if (slotIndexFromId[145].Count > 0)
					{
						num++;
						int index = slotIndexFromId[145].Count - 1;
						list2.Add(slotIndexFromId[145][index]);
						dictionary[mem_ship.Rid].Add(Mst_DataManager.Instance.Mst_Slotitem[145]);
					}
					if (slotIndexFromId[150].Count > 0)
					{
						num += 2;
						int index2 = slotIndexFromId[150].Count - 1;
						list2.Add(slotIndexFromId[150][index2]);
						dictionary[mem_ship.Rid].Add(Mst_DataManager.Instance.Mst_Slotitem[150]);
					}
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary3;
					int rid;
					int key2 = rid = mem_ship.Rid;
					rid = dictionary4[rid];
					dictionary5[key2] = rid + getCombatRationCondPlus(num, givenShip: false);
					dictionary2.Add(i, list2);
					rationRecoveryShips.Remove(mem_ship.Rid);
					list.AddRange(rationRecoveryShips);
					foreach (int item2 in rationRecoveryShips)
					{
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> dictionary7 = dictionary6 = dictionary3;
						int key3 = rid = item2;
						rid = dictionary6[rid];
						dictionary7[key3] = rid + getCombatRationCondPlus(num, givenShip: true);
					}
				}
			}
			if (dictionary2.Count == 0)
			{
				return false;
			}
			List<int> list3 = new List<int>();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			foreach (KeyValuePair<int, List<int>> item3 in dictionary2)
			{
				Mem_ship mem_ship2 = userData.ShipData[item3.Key];
				Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship2);
				List<Mst_slotitem> list4 = userData.SlotData[item3.Key];
				foreach (int item4 in item3.Value)
				{
					int item;
					if (item4 != int.MaxValue)
					{
						item = mem_ship2.Slot[item4];
						mem_shipBase.Slot[item4] = -1;
						list4[item4] = null;
					}
					else
					{
						item = mem_ship2.Exslot;
						mem_shipBase.Exslot = -1;
					}
					list3.Add(item);
				}
				mem_ship2.Set_ShipParam(mem_shipBase, mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
				mem_ship2.TrimSlot();
				list4.RemoveAll((Mst_slotitem x) => x == null);
			}
			foreach (KeyValuePair<int, int> item5 in dictionary3)
			{
				Mem_ship shipInstance = userSubInfo[item5.Key].ShipInstance;
				int value2 = item5.Value;
				shipInstance.AddRationItemCond(value2);
			}
			Comm_UserDatas.Instance.Remove_Slot(list3);
			useShipInfo = dictionary;
			givenShips = list.Distinct().ToList();
			return true;
		}

		private bool isRationLotteryWinning(int battleCount, int luck)
		{
			int num = 10;
			int num2 = (int)(Math.Sqrt(luck) * 2.0);
			int num3 = battleCount * 5;
			double num4 = num + num2 + num3;
			double num5 = (int)Utils.GetRandDouble(0.0, 100.0, 1.0, 1);
			return (num4 >= num5) ? true : false;
		}

		private List<int> getRationRecoveryShips(int useShipIndex)
		{
			List<int> list = new List<int>();
			list.Add(useShipIndex);
			list.Add(useShipIndex + 1);
			list.Add(useShipIndex - 1);
			List<int> list2 = list;
			List<Mem_ship> shipData = userData.ShipData;
			List<int> list3 = new List<int>();
			foreach (int item in list2)
			{
				if (item >= 0 && item <= shipData.Count - 1 && shipData[item].IsFight() && shipData[item].Cond < 100)
				{
					list3.Add(shipData[item].Rid);
				}
			}
			return list3;
		}

		private int getCombatRationCondPlus(int type, bool givenShip)
		{
			int result = 0;
			if (!givenShip)
			{
				switch (type)
				{
				case 1:
					result = 6 + (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1);
					break;
				case 2:
					result = 10 + (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1);
					break;
				case 3:
					result = 20 + (int)Utils.GetRandDouble(0.0, 8.0, 1.0, 1);
					break;
				}
			}
			else
			{
				switch (type)
				{
				case 1:
					result = 2 + (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1);
					break;
				case 2:
					result = 8 + (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1);
					break;
				case 3:
					result = 16 + (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1);
					break;
				}
			}
			return result;
		}
	}
}
