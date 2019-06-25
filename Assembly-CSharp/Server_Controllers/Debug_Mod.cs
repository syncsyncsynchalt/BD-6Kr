using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Debug_Mod : IRebellionPointOperator
	{
		public Debug_Mod(List<int> firstShips)
		{
		}

		public Debug_Mod()
		{
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point[area_id].AddPoint(this, addNum);
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point[area_id].SubPoint(this, subNum);
		}

		public void Add_EscortDeck(int area_id)
		{
			Comm_UserDatas.Instance.Add_EscortDeck(area_id, area_id);
		}

		public void Add_Deck(int rid)
		{
			Comm_UserDatas.Instance.Add_Deck(rid);
			LocalManager localManager = new LocalManager();
			if (localManager.UserInfo != null)
			{
				localManager.UserInfo.__UpdateDeck__();
			}
		}

		public List<int> Add_Ship(List<int> ship_ids)
		{
			List<int> result = Comm_UserDatas.Instance.Add_Ship(ship_ids);
			LocalManager localManager = new LocalManager();
			if (localManager.UserInfo != null)
			{
				localManager.UserInfo.__UpdateShips__();
			}
			return result;
		}

		public List<int> Add_SlotItem(List<int> slot_ids)
		{
			return Comm_UserDatas.Instance.Add_Slot(slot_ids);
		}

		public void Set_ShipChargeData(int rid, int fuel, int bull, List<int> onslot)
		{
			Mem_ship value = null;
			if (Comm_UserDatas.Instance.User_ship.TryGetValue(rid, out value))
			{
				value.Set_ChargeData(bull, fuel, onslot);
			}
		}

		public void Add_UseItem(Dictionary<int, int> add_items)
		{
			foreach (KeyValuePair<int, int> add_item in add_items)
			{
				Comm_UserDatas.Instance.Add_Useitem(add_item.Key, add_item.Value);
			}
		}

		public void Add_Port()
		{
			Comm_UserDatas.Instance.User_basic.PortExtend(1);
		}

		public void Add_Materials(enumMaterialCategory category, int count)
		{
			Comm_UserDatas.Instance.User_material[category].Add_Material(count);
		}

		public void Add_Spoint(int count)
		{
			if (count > 0)
			{
				Comm_UserDatas.Instance.User_basic.AddPoint(count);
			}
			else if (count < 0)
			{
				int subNum = Math.Abs(count);
				Comm_UserDatas.Instance.User_basic.SubPoint(subNum);
			}
		}

		public void Add_Coin(int count)
		{
			Comm_UserDatas.Instance.User_basic.AddCoin(count);
		}

		public void ShipAddExp(List<Mem_ship> ships, List<int> addExps)
		{
			int count = addExps.Count;
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
			for (int i = 0; i < count; i++)
			{
				Mem_shipBase mem_shipBase = new Mem_shipBase(ships[i]);
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id];
				List<int> lvupInfo = null;
				int addExp = addExps[i];
				int num = mem_shipBase.Level = ships[i].getLevelupInfo(dictionary, ships[i].Level, ships[i].Exp, ref addExp, out lvupInfo);
				mem_shipBase.Exp += addExp;
				int num2 = num - ships[i].Level;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = ships[i].Kyouka;
				for (int j = 0; j < num2; j++)
				{
					dictionary2 = ships[i].getLevelupKyoukaValue(ships[i].Ship_id, dictionary2);
				}
				mem_shipBase.SetKyoukaValue(dictionary2);
				int value = 0;
				int value2 = 0;
				dictionary.TryGetValue(mem_shipBase.Level - 1, out value);
				dictionary.TryGetValue(mem_shipBase.Level + 1, out value2);
				ships[i].SetRequireExp(mem_shipBase.Level, dictionary);
				ships[i].Set_ShipParam(mem_shipBase, mst_data, enemy_flag: false);
			}
		}

		public static void ShipAddExp_To_MariageLevel(int ship_rid)
		{
			Mem_ship value = null;
			if (Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value) && value.Level < 99)
			{
				Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
				if (value.Level < 99)
				{
					int item = dictionary[99] - value.Exp;
					Debug_Mod debug_Mod = new Debug_Mod();
					debug_Mod.ShipAddExp(new List<Mem_ship>
					{
						value
					}, new List<int>
					{
						item
					});
				}
			}
		}

		public void AddFurniture(List<int> mst_id)
		{
			mst_id.ForEach(delegate(int x)
			{
				if (!Comm_UserDatas.Instance.User_furniture.ContainsKey(x) && Mst_DataManager.Instance.Mst_furniture.ContainsKey(x))
				{
					Comm_UserDatas.Instance.User_furniture.Add(x, new Mem_furniture(x));
				}
			});
		}

		public static void SubHp(int rid, int subvalue)
		{
			Comm_UserDatas.Instance.User_ship[rid].SubHp(subvalue);
		}

		public static void DeckRefresh(int DeckID)
		{
			List<Mem_ship> memShip = Comm_UserDatas.Instance.User_deck[DeckID].Ship.getMemShip();
			foreach (Mem_ship item in memShip)
			{
				item.SubHp(-(item.Maxhp - item.Nowhp));
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[item.Ship_id];
				item.Set_ChargeData(mst_ship.Bull_max, mst_ship.Fuel_max, mst_ship.Maxeq.ToList());
			}
		}

		public static void OpenLargeDock()
		{
			Comm_UserDatas.Instance.User_basic.OpenLargeDock();
		}

		public static void OpenMapArea(int maparea_id, int mapinfo_no)
		{
			if (Comm_UserDatas.Instance.User_basic.Starttime == 0 || !Mst_DataManager.Instance.Mst_maparea.ContainsKey(maparea_id))
			{
				return;
			}
			int num = Mst_mapinfo.ConvertMapInfoId(maparea_id, mapinfo_no);
			if (!Mst_DataManager.Instance.Mst_mapinfo.ContainsKey(num))
			{
				return;
			}
			Mem_mapclear value = null;
			if (Comm_UserDatas.Instance.User_mapclear.TryGetValue(num, out value))
			{
				if (value.State == MapClearState.InvationClose)
				{
					return;
				}
				if (value.State != 0)
				{
					value.StateChange(MapClearState.Cleard);
				}
			}
			else
			{
				value = new Mem_mapclear(num, maparea_id, mapinfo_no, MapClearState.Cleard);
				value.Insert();
			}
			if (Utils.IsGameClear() && Comm_UserDatas.Instance.User_kdock.Count > 0)
			{
				Comm_UserDatas.Instance.User_record.AddClearDifficult(Comm_UserDatas.Instance.User_basic.Difficult);
			}
			if (maparea_id != 1 && mapinfo_no == 1 && Mem_history.IsFirstOpenArea(num))
			{
				Mem_history mem_history = new Mem_history();
				mem_history.SetAreaOpen(Comm_UserDatas.Instance.User_turn.Total_turn, num);
				Comm_UserDatas.Instance.Add_History(mem_history);
			}
			List<int> reOpenMap = new List<int>();
			new RebellionUtils().MapReOpen(value, out reOpenMap);
		}

		public static void OpenMapAreaAll()
		{
			foreach (Mst_mapinfo value in Mst_DataManager.Instance.Mst_mapinfo.Values)
			{
				if (!Comm_UserDatas.Instance.User_mapclear.ContainsKey(value.Id))
				{
					Mem_mapclear mem_mapclear = new Mem_mapclear(value.Id, value.Maparea_id, value.No, MapClearState.Cleard);
					mem_mapclear.Insert();
				}
				if (value.Maparea_id != 1 && value.No == 1 && Mem_history.IsFirstOpenArea(value.Id))
				{
					Mem_history mem_history = new Mem_history();
					mem_history.SetAreaOpen(Comm_UserDatas.Instance.User_turn.Total_turn, value.Id);
					Comm_UserDatas.Instance.Add_History(mem_history);
				}
			}
			if (Utils.IsGameClear() && Comm_UserDatas.Instance.User_kdock.Count > 0)
			{
				Comm_UserDatas.Instance.User_record.AddClearDifficult(Comm_UserDatas.Instance.User_basic.Difficult);
			}
			new Api_get_Member().StrategyInfo();
		}

		public static void Add_Tunker(int num)
		{
			Comm_UserDatas.Instance.Add_Tanker(num);
		}

		public static void Add_ShipAll()
		{
			List<int> mst_list = new List<int>();
			Mst_DataManager.Instance.Mst_ship.Values.ToList().ForEach(delegate(Mst_ship x)
			{
				if (x.Id < 500 && x.Sortno != 0)
				{
					mst_list.Add(x.Id);
				}
			});
			Comm_UserDatas.Instance.Add_Ship(mst_list);
			LocalManager localManager = new LocalManager();
			if (localManager.UserInfo != null)
			{
				localManager.UserInfo.__UpdateShips__();
			}
		}

		public static void Add_SlotItemAll()
		{
			List<int> mst_list = new List<int>();
			Mst_DataManager.Instance.Mst_Slotitem.Values.ToList().ForEach(delegate(Mst_slotitem x)
			{
				if (x.Id < 500)
				{
					mst_list.Add(x.Id);
				}
			});
			Comm_UserDatas.Instance.Add_Slot(mst_list);
		}

		public static void MissionOpenToMissionId(int missionId)
		{
			Mst_mission2 master = Mst_DataManager.Instance.Mst_mission[missionId];
			Mem_missioncomp mem_missioncomp = new Mem_missioncomp(master.Id, master.Maparea_id, MissionClearKinds.CLEARED);
			List<User_MissionFmt> activeMission = mem_missioncomp.GetActiveMission();
			if (activeMission.Any((User_MissionFmt x) => x.MissionId == master.Id))
			{
				Mem_missioncomp value = null;
				if (Comm_UserDatas.Instance.User_missioncomp.TryGetValue(missionId, out value))
				{
					mem_missioncomp.Update();
				}
				else
				{
					mem_missioncomp.Insert();
				}
				mem_missioncomp.GetActiveMission();
			}
		}

		public static void MissionOpenToArea(int areaId)
		{
			IEnumerable<Mst_mission2> enumerable = from x in Mst_DataManager.Instance.Mst_mission.Values
				where x.Maparea_id == areaId
				select x;
			foreach (Mst_mission2 item in enumerable)
			{
				Mem_missioncomp mem_missioncomp = new Mem_missioncomp(item.Id, areaId, MissionClearKinds.CLEARED);
				if (!Comm_UserDatas.Instance.User_missioncomp.ContainsKey(item.Id))
				{
					mem_missioncomp.Insert();
				}
				else
				{
					mem_missioncomp.Update();
				}
			}
		}

		public static void ChangeSlotLevel(int slot_rid, int level)
		{
			if (level > 10)
			{
				level = 10;
			}
			if (level < 0)
			{
				level = 0;
			}
			if (Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out Mem_slotitem mem_slot))
			{
				Dictionary<int, List<Mst_slotitem_remodel>> mst_slotitem_remodel = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
				if (mst_slotitem_remodel.Values.Any((List<Mst_slotitem_remodel> x) => x.Any((Mst_slotitem_remodel y) => y.Slotitem_id == mem_slot.Slotitem_id)))
				{
					mem_slot.SetLevel(level);
				}
			}
		}

		public static List<int> GetEnableSlotChangeItem()
		{
			if (Comm_UserDatas.Instance.User_slot.Count == 0)
			{
				return new List<int>();
			}
			Dictionary<int, List<Mst_slotitem_remodel>> dict = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
			IEnumerable<Mem_slotitem> source = from x in Comm_UserDatas.Instance.User_slot.Values
				where dict.Values.Any((List<Mst_slotitem_remodel> list) => list.Any((Mst_slotitem_remodel item) => item.Slotitem_id == x.Slotitem_id))
				select x;
			if (source.Count() == 0)
			{
				return new List<int>();
			}
			return (from ret_item in source
				select ret_item.Rid).ToList();
		}

		public static void DeckPracticeMenu_StateChange(DeckPracticeType type, bool state)
		{
			Comm_UserDatas.Instance.User_deckpractice.StateChange(type, state);
		}

		public static void SetRebellionPoint(int area_id, int num)
		{
			if (Comm_UserDatas.Instance.User_rebellion_point.Count == 0)
			{
				new Api_get_Member().StrategyInfo();
			}
			Mem_rebellion_point value = null;
			if (Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(area_id, out value))
			{
				if (value.Point < num)
				{
					int addNum = num - value.Point;
					((IRebellionPointOperator)new Debug_Mod()).AddRebellionPoint(area_id, addNum);
				}
				else
				{
					int subNum = (value.Point - num > 0) ? (value.Point - num) : value.Point;
					((IRebellionPointOperator)new Debug_Mod()).SubRebellionPoint(area_id, subNum);
				}
			}
		}

		public static void SetShipKyoukaValue(int ship_id, PowUpInfo powerUpValues)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_id];
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id];
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			mem_shipBase.C_houg = ((powerUpValues.Karyoku - mst_ship.Houg <= 0) ? mem_shipBase.C_houg : (powerUpValues.Karyoku - mst_ship.Houg));
			mem_shipBase.C_raig = ((powerUpValues.Raisou - mst_ship.Raig <= 0) ? mem_shipBase.C_raig : (powerUpValues.Raisou - mst_ship.Raig));
			mem_shipBase.C_kaihi = ((powerUpValues.Kaihi - mst_ship.Kaih <= 0) ? mem_shipBase.C_kaihi : (powerUpValues.Kaihi - mst_ship.Kaih));
			mem_shipBase.C_luck = ((powerUpValues.Lucky - mst_ship.Luck <= 0) ? mem_shipBase.C_luck : (powerUpValues.Lucky - mst_ship.Luck));
			mem_shipBase.C_souk = ((powerUpValues.Soukou - mst_ship.Souk <= 0) ? mem_shipBase.C_souk : (powerUpValues.Soukou - mst_ship.Souk));
			mem_shipBase.C_tyku = ((powerUpValues.Taiku - mst_ship.Tyku <= 0) ? mem_shipBase.C_tyku : (powerUpValues.Taiku - mst_ship.Tyku));
			mem_shipBase.C_taisen = ((powerUpValues.Taisen - mst_ship.Tais <= 0) ? mem_shipBase.C_taisen : (powerUpValues.Taisen - mst_ship.Tais));
			mem_ship.Set_ShipParam(mem_shipBase, mst_ship, enemy_flag: false);
		}

		public static void SetFleetLevel(int fleetLevel)
		{
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			if (user_record.Level == fleetLevel)
			{
				return;
			}
			Dictionary<int, int> mstLevelUser = ArrayMaster.GetMstLevelUser();
			int value = 0;
			if (mstLevelUser.TryGetValue(fleetLevel, out value))
			{
				uint exp = user_record.Exp;
				int num = (int)(value - exp);
				if (num < 0)
				{
					user_record.GetType().InvokeMember("_level", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, user_record, new object[1]
					{
						1
					});
					user_record.GetType().InvokeMember("_exp", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, user_record, new object[1]
					{
						0u
					});
					num = value;
				}
				user_record.UpdateExp(num, mstLevelUser);
			}
		}

		public static void SetRebellionPhase(int phase)
		{
			if (phase != 0 && phase <= 6)
			{
				int turn_from = Mst_DataManager.Instance.Mst_RebellionPoint[phase].Turn_from;
				Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
				user_turn.GetType().InvokeMember("_total_turn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, user_turn, new object[1]
				{
					turn_from - 1
				});
				user_turn.GetType().InvokeMember("_reqQuestReset", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, user_turn, new object[1]
				{
					true
				});
			}
		}

		public static void SetRadingPhase(int phase)
		{
			if (phase <= 4 && phase >= 1)
			{
				List<Mst_radingtype> list = Mst_DataManager.Instance.Mst_RadingType[(int)Comm_UserDatas.Instance.User_basic.Difficult];
				int turn_from;
				switch (phase)
				{
				case 3:
					turn_from = list[0].Turn_from;
					break;
				case 2:
					turn_from = list[1].Turn_from;
					break;
				default:
					turn_from = list[2].Turn_from;
					break;
				}
				Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
				user_turn.GetType().InvokeMember("_total_turn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, user_turn, new object[1]
				{
					turn_from - 1
				});
				user_turn.GetType().InvokeMember("_reqQuestReset", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, user_turn, new object[1]
				{
					true
				});
			}
		}

		public static List<int> Check_CreateItemData(int typeNo)
		{
			if (typeNo > 3)
			{
				return null;
			}
			Api_req_Kousyou api_req_Kousyou = new Api_req_Kousyou();
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_slotitemget2", "mst_slotitemget2", "Id");
			api_req_Kousyou.GetType().InvokeMember("createItemTable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, api_req_Kousyou, new object[1]
			{
				enumerable
			});
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 0);
			dictionary.Add(enumMaterialCategory.Bull, 0);
			dictionary.Add(enumMaterialCategory.Steel, 0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0);
			List<int> list = new List<int>();
			dictionary[enumMaterialCategory.Fuel] = 100;
			dictionary[enumMaterialCategory.Bull] = 0;
			dictionary[enumMaterialCategory.Steel] = 0;
			dictionary[enumMaterialCategory.Bauxite] = 0;
			int item = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, api_req_Kousyou, new object[2]
			{
				typeNo,
				dictionary
			});
			list.Add(item);
			dictionary[enumMaterialCategory.Fuel] = 0;
			dictionary[enumMaterialCategory.Bull] = 100;
			dictionary[enumMaterialCategory.Steel] = 0;
			dictionary[enumMaterialCategory.Bauxite] = 0;
			int item2 = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, api_req_Kousyou, new object[2]
			{
				typeNo,
				dictionary
			});
			list.Add(item2);
			dictionary[enumMaterialCategory.Fuel] = 0;
			dictionary[enumMaterialCategory.Bull] = 0;
			dictionary[enumMaterialCategory.Steel] = 100;
			dictionary[enumMaterialCategory.Bauxite] = 0;
			int item3 = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, api_req_Kousyou, new object[2]
			{
				typeNo,
				dictionary
			});
			list.Add(item3);
			dictionary[enumMaterialCategory.Fuel] = 0;
			dictionary[enumMaterialCategory.Bull] = 0;
			dictionary[enumMaterialCategory.Steel] = 0;
			dictionary[enumMaterialCategory.Bauxite] = 100;
			int item4 = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, api_req_Kousyou, new object[2]
			{
				typeNo,
				dictionary
			});
			list.Add(item4);
			return list;
		}
	}
}
