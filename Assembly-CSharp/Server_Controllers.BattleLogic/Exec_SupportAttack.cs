using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_SupportAttack : BattleLogicBase<SupportAtack>
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		private Mem_deck supportDeck;

		private List<Mem_ship> supportShips;

		private MissionType supportType;

		private BattleSearchValues[] serchValues;

		private ILookup<int, int> mst_support_data;

		public override BattleBaseData F_Data => _f_Data;

		public override BattleBaseData E_Data => _e_Data;

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo => _f_SubInfo;

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo => _e_SubInfo;

		public Exec_SupportAttack(BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, SearchInfo[] serch, ILookup<int, int> mst_support_type)
		{
			_e_Data = enemyData;
			_e_SubInfo = enemySubInfo;
			practiceFlag = false;
			mst_support_data = mst_support_type;
			serchValues = new BattleSearchValues[2]
			{
				serch[0].SearchValue,
				serch[1].SearchValue
			};
		}

		public void SelectSupportDeck(List<Mem_deck> targetDeck)
		{
			MissionType ms_type = MissionType.None;
			if (Mst_DataManager.Instance.Mst_mapenemy[_e_Data.Enemy_id].Boss != 0)
			{
				ms_type = MissionType.SupportBoss;
			}
			else
			{
				ms_type = MissionType.SupportForward;
			}
			supportType = ms_type;
			if (targetDeck != null && targetDeck.Count != 0)
			{
				supportDeck = (from deck in targetDeck
					let mst_misson = Mst_DataManager.Instance.Mst_mission[deck.Mission_id]
					where mst_misson.Mission_type == ms_type
					select deck).FirstOrDefault();
			}
		}

		public void SelectSupportDeck(List<Mem_deck> targetDeck, bool isForwardDeck)
		{
			MissionType missionType = supportType = ((Mst_DataManager.Instance.Mst_mapenemy[_e_Data.Enemy_id].Boss == 0) ? MissionType.SupportForward : MissionType.SupportBoss);
			if (targetDeck != null && targetDeck.Count != 0)
			{
				IOrderedEnumerable<Mem_deck> source = from x in targetDeck
					orderby x.Mission_id descending
					select x;
				MissionType searchKey = missionType;
				supportDeck = (from deck in source
					let mst_misson = Mst_DataManager.Instance.Mst_mission[deck.Mission_id]
					where mst_misson.Mission_type == searchKey
					select deck).FirstOrDefault();
			}
		}

		public override SupportAtack GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			SupportAtack supportAtack = new SupportAtack();
			BattleSupportKinds battleSupportKinds = Init(ref supportAtack.Undressing_Flag);
			if (battleSupportKinds == BattleSupportKinds.None)
			{
				return null;
			}
			supportAtack.Deck_Id = supportDeck.Rid;
			supportAtack.SupportType = battleSupportKinds;
			switch (battleSupportKinds)
			{
			case BattleSupportKinds.AirAtack:
				using (Exec_SupportAirBattle exec_SupportAirBattle = new Exec_SupportAirBattle(F_Data, F_SubInfo, E_Data, E_SubInfo, practiceFlag))
				{
					supportAtack.AirBattle = exec_SupportAirBattle.GetResultData(formation, cParam);
				}
				if (supportAtack.AirBattle == null)
				{
					return null;
				}
				break;
			case BattleSupportKinds.Hougeki:
				using (Exec_SupportHougeki exec_SupportHougeki = new Exec_SupportHougeki(F_Data, F_SubInfo, E_Data, E_SubInfo, practiceFlag))
				{
					supportAtack.Hourai = exec_SupportHougeki.GetResultData<Support_HouRai>(formation);
				}
				if (supportAtack.Hourai == null)
				{
					return null;
				}
				break;
			case BattleSupportKinds.Raigeki:
				using (Exec_SupportRaigeki exec_SupportRaigeki = new Exec_SupportRaigeki(F_Data, F_SubInfo, E_Data, E_SubInfo, practiceFlag))
				{
					supportAtack.Hourai = exec_SupportRaigeki.GetResultData<Support_HouRai>(formation);
				}
				if (supportAtack.Hourai == null)
				{
					return null;
				}
				break;
			}
			supportDeck.ChangeSupported();
			return supportAtack;
		}

		public override void Dispose()
		{
			if (F_Data != null)
			{
				F_Data.Dispose();
			}
			randInstance = null;
		}

		private BattleSupportKinds Init(ref bool[] undressing)
		{
			if (mst_support_data == null)
			{
				return BattleSupportKinds.None;
			}
			if (E_Data.ShipData.FirstOrDefault((Mem_ship x) => x.Nowhp > 0) == null)
			{
				return BattleSupportKinds.None;
			}
			if (supportDeck == null)
			{
				return BattleSupportKinds.None;
			}
			supportShips = supportDeck.Ship.getMemShip();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (IGrouping<int, int> mst_support_datum in mst_support_data)
			{
				int key = mst_support_datum.Key;
				dictionary.Add(key, 0);
				foreach (int item in mst_support_datum)
				{
					dictionary2.Add(item, key);
				}
			}
			int num = 0;
			int num2 = 0;
			List<int> list = new List<int>();
			List<List<Mst_slotitem>> list2 = new List<List<Mst_slotitem>>();
			foreach (var item2 in supportShips.Select((Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}))
			{
				int num3 = dictionary2[item2.obj.Stype];
				list.Add(item2.obj.Stype);
				Dictionary<int, int> dictionary3;
				Dictionary<int, int> dictionary4 = dictionary3 = dictionary;
				int key2;
				int key3 = key2 = num3;
				key2 = dictionary3[key2];
				dictionary4[key3] = key2 + 1;
				if (item2.obj.Get_FatigueState() == FatigueState.Exaltation)
				{
					int num4 = (item2.ship_idx != 0) ? 5 : 15;
					num2 += num4;
				}
				if (item2.obj.Get_DamageState() >= DamageState.Tyuuha)
				{
					undressing[item2.ship_idx] = true;
				}
				List<Mst_slotitem> list3 = new List<Mst_slotitem>();
				foreach (var item3 in item2.obj.Slot.Select((int rid, int idx) => new
				{
					rid,
					idx
				}))
				{
					if (item3.rid <= 0)
					{
						break;
					}
					Mst_slotitem value = null;
					if (!Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(Comm_UserDatas.Instance.User_slot[item3.rid].Slotitem_id, out value))
					{
						break;
					}
					list3.Add(value);
					if (item2.obj.Onslot[item3.idx] > 0)
					{
						FighterInfo.FighterKinds kind = FighterInfo.GetKind(value);
						if (kind == FighterInfo.FighterKinds.BAKU || kind == FighterInfo.FighterKinds.RAIG)
						{
							num++;
						}
					}
				}
				list2.Add(list3);
			}
			int num5 = (supportType != MissionType.SupportForward) ? (num2 + 85) : (50 + num2);
			if (num5 < randInstance.Next(100))
			{
				return BattleSupportKinds.None;
			}
			_f_Data = new BattleBaseData(supportDeck, supportShips, list, list2);
			_f_Data.Formation = BattleFormationKinds1.TanJuu;
			_f_Data.BattleFormation = E_Data.BattleFormation;
			List<Mem_ship> memShip = _f_Data.Deck.Ship.getMemShip();
			_f_SubInfo = new Dictionary<int, BattleShipSubInfo>();
			for (int i = 0; i < memShip.Count; i++)
			{
				BattleShipSubInfo value2 = new BattleShipSubInfo(i, memShip[i]);
				_f_SubInfo.Add(memShip[i].Rid, value2);
			}
			int num6 = dictionary[1];
			int num7 = dictionary[2];
			int num8 = dictionary[3];
			int num9 = dictionary[4];
			int num10 = dictionary[5];
			int num11 = dictionary[6];
			if (num6 >= 3 && num > 0)
			{
				return BattleSupportKinds.AirAtack;
			}
			if (num7 >= 2)
			{
				return BattleSupportKinds.Raigeki;
			}
			if (num8 + num9 >= 4)
			{
				return BattleSupportKinds.Hougeki;
			}
			if (num10 + num11 >= 4)
			{
				return BattleSupportKinds.Raigeki;
			}
			return BattleSupportKinds.Hougeki;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
