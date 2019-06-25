using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Kaisou
	{
		public enum SlotSetChkResult_Ship
		{
			Ok,
			Invalid,
			Mission,
			ActionEndDeck,
			Repair,
			BlingShip
		}

		public enum RemodelingChkResult
		{
			OK,
			Invalid,
			BlingShip,
			ActionEndDeck,
			Mission,
			Repair,
			Level,
			Drawing,
			Steel,
			Bull
		}

		public SlotSetChkResult_Ship IsValidSlotSet(int ship_rid)
		{
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				return SlotSetChkResult_Ship.Invalid;
			}
			if (value.IsBlingShip())
			{
				return SlotSetChkResult_Ship.BlingShip;
			}
			Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.Values.FirstOrDefault((Mem_deck x) => (x.Ship.Find(ship_rid) != -1) ? true : false);
			if (mem_deck != null)
			{
				if (mem_deck.MissionState != 0)
				{
					return SlotSetChkResult_Ship.Mission;
				}
				if (mem_deck.IsActionEnd)
				{
					return SlotSetChkResult_Ship.ActionEndDeck;
				}
			}
			if (Comm_UserDatas.Instance.User_ndock.Values.Any((Mem_ndock x) => x.Ship_id == ship_rid))
			{
				return SlotSetChkResult_Ship.Repair;
			}
			return SlotSetChkResult_Ship.Ok;
		}

		public SlotSetChkResult_Slot IsValidSlotSet(int ship_rid, int slot_rid, int equip_idx)
		{
			if (IsValidSlotSet(ship_rid) != 0)
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_rid];
			if (mem_ship.Slotnum < equip_idx + 1)
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			int num = mem_ship.Slot.FindIndex((int x) => x == -1);
			if (num != -1 && num < equip_idx)
			{
				equip_idx = num;
			}
			Mem_slotitem value = null;
			if (slot_rid != -1)
			{
				Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out value);
				if (value == null)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
				if (value.Equip_flag == Mem_slotitem.enumEquipSts.Equip)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
			}
			else if (slot_rid == -1)
			{
				return SlotSetChkResult_Slot.Ok;
			}
			int preCost = 0;
			int afterCost = 0;
			OnslotChangeType slotChangeCost = Mst_slotitem_cost.GetSlotChangeCost(mem_ship.Slot[equip_idx], slot_rid, out preCost, out afterCost);
			int onslotKeisu = mem_ship.Onslot[equip_idx];
			if (slotChangeCost == OnslotChangeType.OtherToPlane)
			{
				onslotKeisu = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id].Maxeq[equip_idx];
			}
			int slotChangeBauxiteNum = Mst_slotitem_cost.GetSlotChangeBauxiteNum(slotChangeCost, preCost, afterCost, onslotKeisu);
			if (slotChangeBauxiteNum > 0)
			{
				return SlotSetChkResult_Slot.Ok;
			}
			if (Math.Abs(slotChangeBauxiteNum) > Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bauxite].Value)
			{
				if (preCost == 0 && afterCost > 0)
				{
					return SlotSetChkResult_Slot.NgBauxiteShort;
				}
				if (preCost < afterCost)
				{
					return SlotSetChkResult_Slot.NgBausiteShortHighCost;
				}
			}
			return SlotSetChkResult_Slot.Ok;
		}

		public Api_Result<SlotSetChkResult_Slot> SlotSet(int ship_rid, int slot_rid, int equip_idx)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = new Api_Result<SlotSetChkResult_Slot>();
			SlotSetChkResult_Slot slotSetChkResult_Slot = IsValidSlotSet(ship_rid, slot_rid, equip_idx);
			if (slotSetChkResult_Slot != 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				api_Result.data = slotSetChkResult_Slot;
				return api_Result;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_rid];
			Mem_slotitem value = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out value);
			int num = mem_ship.Slot.FindIndex((int x) => x == -1);
			if (num != -1 && num < equip_idx)
			{
				equip_idx = num;
			}
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			Mem_slotitem value2 = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(mem_ship.Slot[equip_idx], out value2);
			int preCost = 0;
			int afterCost = 0;
			OnslotChangeType slotChangeCost = Mst_slotitem_cost.GetSlotChangeCost(mem_ship.Slot[equip_idx], slot_rid, out preCost, out afterCost);
			int num2 = mem_ship.Onslot[equip_idx];
			if (slotChangeCost == OnslotChangeType.OtherToPlane)
			{
				num2 = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id].Maxeq[equip_idx];
			}
			int slotChangeBauxiteNum = Mst_slotitem_cost.GetSlotChangeBauxiteNum(slotChangeCost, preCost, afterCost, num2);
			if (slotChangeBauxiteNum < 0)
			{
				switch (slotChangeCost)
				{
				case OnslotChangeType.PlaneToPlane:
					api_Result.data = SlotSetChkResult_Slot.OkBauxiteUseHighCost;
					break;
				case OnslotChangeType.OtherToPlane:
					api_Result.data = SlotSetChkResult_Slot.OkBauxiteUse;
					mem_shipBase.Onslot[equip_idx] = num2;
					break;
				}
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bauxite].Sub_Material(Math.Abs(slotChangeBauxiteNum));
			}
			else
			{
				if (slotChangeCost == OnslotChangeType.PlaneOther)
				{
					mem_shipBase.Onslot[equip_idx] = 0;
				}
				api_Result.data = SlotSetChkResult_Slot.Ok;
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bauxite].Add_Material(slotChangeBauxiteNum);
			}
			mem_shipBase.Slot[equip_idx] = slot_rid;
			value2?.UnEquip();
			if (slot_rid != -1)
			{
				value.Equip(ship_rid);
			}
			mem_ship.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
			if (slot_rid == -1)
			{
				mem_ship.TrimSlot();
			}
			return api_Result;
		}

		public Api_Result<Hashtable> Unslot_all(int ship_rid)
		{
			Api_Result<Hashtable> result = new Api_Result<Hashtable>();
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_rid];
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			for (int i = 0; i < mem_shipBase.Slot.Count(); i++)
			{
				Mem_slotitem value = null;
				int num = mem_shipBase.Slot[i];
				mem_shipBase.Slot[i] = -1;
				if (num > 0 && Comm_UserDatas.Instance.User_slot.TryGetValue(num, out value))
				{
					Mst_slotitem_cost value2 = null;
					if (Mst_DataManager.Instance.Mst_slotitem_cost.TryGetValue(value.Slotitem_id, out value2))
					{
						int addNum = value2.GetAddNum(mem_shipBase.Onslot[i]);
						Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bauxite].Add_Material(addNum);
					}
					value.UnEquip();
				}
			}
			mem_ship.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
			mem_ship.TrimSlot();
			return result;
		}

		public bool IsExpandSlotShip(int shipRid)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[shipRid];
			if (mem_ship.Level < 30 || mem_ship.IsOpenExSlot())
			{
				return false;
			}
			return true;
		}

		public Api_Result<Mem_ship> ExpandSlot(int shipRid)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[shipRid];
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id];
			mem_shipBase.Exslot = -1;
			mem_ship.Set_ShipParam(mem_shipBase, mst_data, enemy_flag: false);
			api_Result.data = mem_ship;
			Mem_useitem value = null;
			Comm_UserDatas.Instance.User_useItem.TryGetValue(64, out value);
			value.Sub_UseItem(1);
			return api_Result;
		}

		public SlotSetChkResult_Slot IsValidSlotSet(int ship_rid, int slot_rid)
		{
			if (IsValidSlotSet(ship_rid) != 0)
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_rid];
			if (!mem_ship.IsOpenExSlot())
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			Mem_slotitem value = null;
			if (slot_rid != -1)
			{
				Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out value);
				if (value == null)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
				if (value.Equip_flag == Mem_slotitem.enumEquipSts.Equip)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
			}
			else if (slot_rid == -1)
			{
				return SlotSetChkResult_Slot.Ok;
			}
			return SlotSetChkResult_Slot.Ok;
		}

		public Api_Result<SlotSetChkResult_Slot> SlotSet(int ship_rid, int slot_rid)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = new Api_Result<SlotSetChkResult_Slot>();
			SlotSetChkResult_Slot slotSetChkResult_Slot = IsValidSlotSet(ship_rid, slot_rid);
			if (slotSetChkResult_Slot != 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				api_Result.data = slotSetChkResult_Slot;
				return api_Result;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_rid];
			Mem_slotitem value = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out value);
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			Mem_slotitem value2 = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(mem_ship.Exslot, out value2);
			mem_shipBase.Exslot = slot_rid;
			value2?.UnEquip();
			if (slot_rid != -1)
			{
				value.Equip(ship_rid);
			}
			mem_ship.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship[mem_shipBase.Ship_id], enemy_flag: false);
			return api_Result;
		}

		public Api_Result<bool> SlotLockChange(int slot_rid)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Mem_slotitem value = null;
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			value.LockChange();
			api_Result.data = value.Lock;
			return api_Result;
		}

		public Api_Result<int> Powerup(int ship_rid, HashSet<int> rid_items)
		{
			Api_Result<int> api_Result = new Api_Result<int>();
			api_Result.data = 0;
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.IsBlingShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> list = new List<Mem_ship>();
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary.Add(Mem_ship.enumKyoukaIdx.Houg, 0);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Raig, 0);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Tyku, 0);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Souk, 0);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = dictionary;
			Dictionary<Mem_ship, int> dictionary3 = new Dictionary<Mem_ship, int>();
			Dictionary<int, Mem_deck> user_deck = Comm_UserDatas.Instance.User_deck;
			Dictionary<double, int> dictionary4 = new Dictionary<double, int>();
			foreach (int rid_item in rid_items)
			{
				Mem_ship value2 = null;
				if (!Comm_UserDatas.Instance.User_ship.TryGetValue(rid_item, out value2))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				if (value2.Locked == 1 || value2.IsBlingShip() || value2.GetLockSlotItems().Count > 0)
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				list.Add(value2);
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[value2.Ship_id];
				List<int> powup = mst_ship.Powup;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary5 = dictionary = dictionary2;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key2 = key = Mem_ship.enumKyoukaIdx.Houg;
				int num = dictionary[key];
				dictionary5[key2] = num + powup[0];
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary6;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary7 = dictionary6 = dictionary2;
				Mem_ship.enumKyoukaIdx key3 = key = Mem_ship.enumKyoukaIdx.Raig;
				num = dictionary6[key];
				dictionary7[key3] = num + powup[1];
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary8;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary9 = dictionary8 = dictionary2;
				Mem_ship.enumKyoukaIdx key4 = key = Mem_ship.enumKyoukaIdx.Tyku;
				num = dictionary8[key];
				dictionary9[key4] = num + powup[2];
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary10;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary11 = dictionary10 = dictionary2;
				Mem_ship.enumKyoukaIdx key5 = key = Mem_ship.enumKyoukaIdx.Souk;
				num = dictionary10[key];
				dictionary11[key5] = num + powup[3];
				double luckUpKeisu = mst_ship.GetLuckUpKeisu();
				if (luckUpKeisu != 0.0)
				{
					if (dictionary4.ContainsKey(luckUpKeisu))
					{
						Dictionary<double, int> dictionary12;
						Dictionary<double, int> dictionary13 = dictionary12 = dictionary4;
						double key6;
						double key7 = key6 = luckUpKeisu;
						num = dictionary12[key6];
						dictionary13[key7] = num + 1;
					}
					else
					{
						dictionary4.Add(luckUpKeisu, 1);
					}
				}
				int[] array = user_deck[1].Search_ShipIdx(value2.Rid);
				if (array[0] != -1 && array[0] == 1 && array[1] == 0)
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				dictionary3.Add(value2, array[0]);
			}
			Mst_ship mst_ship2 = Mst_DataManager.Instance.Mst_ship[value.Ship_id];
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary14 = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg_max);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig_max);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku_max);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk_max);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Luck, mst_ship2.Luck_max);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary15 = dictionary14;
			dictionary14 = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Luck, mst_ship2.Luck);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary16 = dictionary14;
			Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka = value.Kyouka;
			dictionary14 = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Houg, kyouka[Mem_ship.enumKyoukaIdx.Houg]);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Raig, kyouka[Mem_ship.enumKyoukaIdx.Raig]);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Tyku, kyouka[Mem_ship.enumKyoukaIdx.Tyku]);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Souk, kyouka[Mem_ship.enumKyoukaIdx.Souk]);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Kaihi, kyouka[Mem_ship.enumKyoukaIdx.Kaihi]);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Taisen, kyouka[Mem_ship.enumKyoukaIdx.Taisen]);
			dictionary14.Add(Mem_ship.enumKyoukaIdx.Luck, kyouka[Mem_ship.enumKyoukaIdx.Luck]);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary17 = dictionary14;
			Random random = new Random();
			foreach (KeyValuePair<Mem_ship.enumKyoukaIdx, int> item in dictionary2)
			{
				if (item.Value > 0)
				{
					int num2 = dictionary16[item.Key] + kyouka[item.Key];
					if (dictionary15[item.Key] > num2)
					{
						int num3 = random.Next(2);
						int num4 = (int)Math.Floor((float)item.Value * 0.6f + (float)item.Value * 0.6f * (float)num3 + 0.3f);
						if (num2 + num4 > dictionary15[item.Key])
						{
							dictionary17[item.Key] = dictionary15[item.Key] - dictionary16[item.Key];
						}
						else
						{
							Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary18 = dictionary14 = dictionary17;
							Mem_ship.enumKyoukaIdx key;
							Mem_ship.enumKyoukaIdx key8 = key = item.Key;
							int num = dictionary14[key];
							dictionary18[key8] = num + num4;
						}
					}
				}
			}
			int num5 = kyouka[Mem_ship.enumKyoukaIdx.Luck] + dictionary16[Mem_ship.enumKyoukaIdx.Luck];
			int num6 = dictionary15[Mem_ship.enumKyoukaIdx.Luck];
			if (dictionary4.Count > 0 && dictionary15[Mem_ship.enumKyoukaIdx.Luck] > num5)
			{
				double num7 = 0.0;
				foreach (KeyValuePair<double, int> item2 in dictionary4)
				{
					double num8 = item2.Key * (double)item2.Value;
					num7 += num8;
				}
				int num9 = (int)Math.Floor(num7 + Utils.GetRandDouble(0.0, 0.9, 0.1, 2));
				if (num6 < num9 + num5)
				{
					dictionary17[Mem_ship.enumKyoukaIdx.Luck] = num6 - dictionary16[Mem_ship.enumKyoukaIdx.Luck];
				}
				else
				{
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary19;
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary20 = dictionary19 = dictionary17;
					Mem_ship.enumKyoukaIdx key;
					Mem_ship.enumKyoukaIdx key9 = key = Mem_ship.enumKyoukaIdx.Luck;
					int num = dictionary19[key];
					dictionary20[key9] = num + num9;
				}
			}
			bool flag = false;
			foreach (KeyValuePair<Mem_ship.enumKyoukaIdx, int> item3 in dictionary17)
			{
				if (kyouka[item3.Key] != item3.Value)
				{
					flag = true;
				}
			}
			List<Mem_ship> list2 = dictionary3.Keys.ToList();
			int num10 = 0;
			int num11 = 0;
			int sameShipCount = getSameShipCount(value, list2);
			int num12 = selectSamePowerupType(sameShipCount);
			dictionary17.Add(Mem_ship.enumKyoukaIdx.Taik_Powerup, value.Kyouka[Mem_ship.enumKyoukaIdx.Taik_Powerup]);
			switch (num12)
			{
			case 1:
				num11 = GetSameShipPowerupLuck(value, sameShipCount, maxFlag: false);
				break;
			case 3:
				num11 = GetSameShipPowerupLuck(value, sameShipCount, maxFlag: true);
				break;
			case 2:
				num10 = GetSameShipPowerupTaikyu(value, sameShipCount, maxFlag: false);
				break;
			}
			int num13 = mst_ship2.Luck + dictionary17[Mem_ship.enumKyoukaIdx.Luck];
			int num14 = mst_ship2.Luck_max - num13;
			int num15 = mst_ship2.Taik + value.Kyouka[Mem_ship.enumKyoukaIdx.Taik] + value.Kyouka[Mem_ship.enumKyoukaIdx.Taik_Powerup];
			int num16 = mst_ship2.Taik_max - num15;
			if (num11 > num14)
			{
				num11 = num14;
			}
			if (num10 > num16)
			{
				num10 = num16;
			}
			bool flag2 = false;
			if (num11 > 0)
			{
				flag2 = true;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary21;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary22 = dictionary21 = dictionary17;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key10 = key = Mem_ship.enumKyoukaIdx.Luck;
				int num = dictionary21[key];
				dictionary22[key10] = num + num11;
				flag = true;
			}
			if (num10 > 0)
			{
				flag2 = true;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary23;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary24 = dictionary23 = dictionary17;
				Mem_ship.enumKyoukaIdx key;
				Mem_ship.enumKyoukaIdx key11 = key = Mem_ship.enumKyoukaIdx.Taik_Powerup;
				int num = dictionary23[key];
				dictionary24[key11] = num + num10;
				flag = true;
			}
			if (flag)
			{
				Mem_shipBase mem_shipBase = new Mem_shipBase(value);
				dictionary17.Add(Mem_ship.enumKyoukaIdx.Taik, value.Kyouka[Mem_ship.enumKyoukaIdx.Taik]);
				mem_shipBase.SetKyoukaValue(dictionary17);
				value.Set_ShipParam(mem_shipBase, mst_ship2, enemy_flag: false);
				api_Result.data = ((!flag2) ? 1 : 2);
			}
			value.SumLovToKaisouPowUp(rid_items.Count);
			foreach (KeyValuePair<Mem_ship, int> item4 in dictionary3)
			{
				if (item4.Value != -1)
				{
					Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck[item4.Value];
					mem_deck.Ship.RemoveShip(item4.Key.Rid);
				}
			}
			Comm_UserDatas.Instance.Remove_Ship(list2);
			QuestKaisou questKaisou = new QuestKaisou(flag);
			questKaisou.ExecuteCheck();
			return api_Result;
		}

		public int GetSameShipPowerupTaikyu(int ship_rid, HashSet<int> rid_items)
		{
			Mem_ship owner = Comm_UserDatas.Instance.User_ship[ship_rid];
			List<Mem_ship> list = new List<Mem_ship>();
			foreach (int rid_item in rid_items)
			{
				list.Add(Comm_UserDatas.Instance.User_ship[rid_item]);
			}
			int sameShipCount = getSameShipCount(owner, list);
			return GetSameShipPowerupTaikyu(owner, sameShipCount, maxFlag: true);
		}

		private int GetSameShipPowerupTaikyu(Mem_ship owner, int sameNum, bool maxFlag)
		{
			if (owner.Kyouka[Mem_ship.enumKyoukaIdx.Taik_Powerup] >= 3)
			{
				return 0;
			}
			if (sameNum >= 4)
			{
				return 1;
			}
			return 0;
		}

		public int GetSameShipPowerupLuck(int ship_rid, HashSet<int> rid_items)
		{
			Mem_ship owner = Comm_UserDatas.Instance.User_ship[ship_rid];
			List<Mem_ship> list = new List<Mem_ship>();
			foreach (int rid_item in rid_items)
			{
				list.Add(Comm_UserDatas.Instance.User_ship[rid_item]);
			}
			int sameShipCount = getSameShipCount(owner, list);
			return GetSameShipPowerupLuck(owner, sameShipCount, maxFlag: true);
		}

		private int GetSameShipPowerupLuck(Mem_ship owner, int sameNum, bool maxFlag)
		{
			if (sameNum == 0)
			{
				return 0;
			}
			if (sameNum <= 4)
			{
				return 1;
			}
			if (sameNum >= 5 && maxFlag)
			{
				return 2;
			}
			return 1;
		}

		private int getSameShipCount(Mem_ship owner, List<Mem_ship> feedShip)
		{
			string owner_yomi = Mst_DataManager.Instance.Mst_ship[owner.Ship_id].Yomi;
			return feedShip.Count(delegate(Mem_ship x)
			{
				string yomi = Mst_DataManager.Instance.Mst_ship[x.Ship_id].Yomi;
				return yomi.Equals(owner_yomi) ? true : false;
			});
		}

		public int selectSamePowerupType(int sameNum)
		{
			List<double> rateValues;
			List<int> list3;
			switch (sameNum)
			{
			case 1:
			{
				List<double> list = new List<double>();
				list.Add(3.0);
				list.Add(97.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(0);
				list3 = list2;
				break;
			}
			case 2:
			{
				List<double> list = new List<double>();
				list.Add(10.0);
				list.Add(90.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(0);
				list3 = list2;
				break;
			}
			case 3:
			{
				List<double> list = new List<double>();
				list.Add(20.0);
				list.Add(80.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(0);
				list3 = list2;
				break;
			}
			case 4:
			{
				List<double> list = new List<double>();
				list.Add(25.0);
				list.Add(10.0);
				list.Add(65.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(2);
				list2.Add(0);
				list3 = list2;
				break;
			}
			case 5:
			{
				List<double> list = new List<double>();
				list.Add(25.0);
				list.Add(15.0);
				list.Add(10.0);
				list.Add(50.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(2);
				list2.Add(3);
				list2.Add(0);
				list3 = list2;
				break;
			}
			default:
				return 0;
			}
			int randomRateIndex = Utils.GetRandomRateIndex(rateValues);
			return list3[randomRateIndex];
		}

		public RemodelingChkResult ValidRemodeling(int ship_rid, out int reqDrawingNum)
		{
			Mem_ship value = null;
			reqDrawingNum = 0;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				return RemodelingChkResult.Invalid;
			}
			Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.Values.FirstOrDefault((Mem_deck x) => (x.Ship.Find(ship_rid) != -1) ? true : false);
			if (mem_deck != null)
			{
				if (mem_deck.MissionState != 0)
				{
					return RemodelingChkResult.Mission;
				}
				if (mem_deck.IsActionEnd)
				{
					return RemodelingChkResult.ActionEndDeck;
				}
			}
			if (Comm_UserDatas.Instance.User_ndock.Values.Any((Mem_ndock x) => x.Ship_id == ship_rid))
			{
				return RemodelingChkResult.Repair;
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[value.Ship_id];
			if (mst_ship.Aftershipid <= 0)
			{
				return RemodelingChkResult.Invalid;
			}
			if (value.Level < mst_ship.Afterlv)
			{
				return RemodelingChkResult.Level;
			}
			if (value.IsBlingShip())
			{
				return RemodelingChkResult.BlingShip;
			}
			Mst_shipupgrade value2 = null;
			if (Mst_DataManager.Instance.Mst_upgrade.TryGetValue(mst_ship.Aftershipid, out value2))
			{
				reqDrawingNum = value2.Drawing_count;
			}
			if (value.Ship_id == 466 || value.Ship_id == 467)
			{
				reqDrawingNum = 0;
			}
			if (reqDrawingNum > 0)
			{
				Mem_useitem value3 = null;
				if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(58, out value3))
				{
					return RemodelingChkResult.Drawing;
				}
				if (value3.Value < reqDrawingNum)
				{
					return RemodelingChkResult.Drawing;
				}
			}
			if (Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel].Value < mst_ship.Afterfuel)
			{
				return RemodelingChkResult.Steel;
			}
			if (Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Value < mst_ship.Afterbull)
			{
				return RemodelingChkResult.Bull;
			}
			int remodelDevKitNum = mst_ship.GetRemodelDevKitNum();
			if (Comm_UserDatas.Instance.User_material[enumMaterialCategory.Dev_Kit].Value < remodelDevKitNum)
			{
				return RemodelingChkResult.Invalid;
			}
			return RemodelingChkResult.OK;
		}

		public Api_Result<Mem_ship> Remodeling(int ship_rid, int drawingNum)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.IsBlingShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[value.Ship_id];
			int aftershipid = Mst_DataManager.Instance.Mst_ship[value.Ship_id].Aftershipid;
			Mst_ship mst_ship2 = Mst_DataManager.Instance.Mst_ship[aftershipid];
			Mem_shipBase mem_shipBase = new Mem_shipBase(value);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = dictionary;
			dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg_max);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig_max);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk_max);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku_max);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary3 = dictionary;
			Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka = value.Kyouka;
			Random random = new Random();
			foreach (KeyValuePair<Mem_ship.enumKyoukaIdx, int> item in dictionary2)
			{
				int num = dictionary2[item.Key];
				int num2 = dictionary3[item.Key];
				int num3 = num2 - num;
				double a = (double)num3 * (0.4 + 0.4 * (double)random.Next(2)) * (double)value.Level / 99.0;
				int value2 = (int)Math.Ceiling(a);
				kyouka[item.Key] = value2;
				if (num2 < kyouka[item.Key] + num)
				{
					kyouka[item.Key] = num2 - num;
				}
			}
			if (kyouka[Mem_ship.enumKyoukaIdx.Luck] + mst_ship2.Luck > mst_ship2.Luck_max)
			{
				kyouka[Mem_ship.enumKyoukaIdx.Luck] = mst_ship2.Luck_max - mst_ship2.Luck;
			}
			kyouka[Mem_ship.enumKyoukaIdx.Taik_Powerup] = 0;
			mem_shipBase.C_taik_powerup = 0;
			if (mem_shipBase.Level >= 100)
			{
				int remodelingTaik = getRemodelingTaik(mst_ship2.Taik);
				int num4 = mst_ship2.Taik + remodelingTaik;
				if (num4 > mst_ship2.Taik_max)
				{
					num4 = mst_ship2.Taik_max;
				}
				kyouka[Mem_ship.enumKyoukaIdx.Taik] = num4 - mst_ship2.Taik;
			}
			List<int> list = Comm_UserDatas.Instance.Add_Slot(mst_ship2.Defeq);
			mem_shipBase.Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					Comm_UserDatas.Instance.User_slot[x].UnEquip();
				}
			});
			if (value.Exslot > 0)
			{
				Comm_UserDatas.Instance.User_slot[value.Exslot].UnEquip();
				mem_shipBase.Exslot = -1;
			}
			mem_shipBase.Slot.Clear();
			mem_shipBase.Onslot.Clear();
			for (int i = 0; i < mst_ship2.Slot_num; i++)
			{
				if (list.Count > i)
				{
					mem_shipBase.Slot.Add(list[i]);
					Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot[list[i]];
					mem_slotitem.Equip(mem_shipBase.Rid);
				}
				else
				{
					mem_shipBase.Slot.Add(mst_ship2.Defeq[i]);
				}
				mem_shipBase.Onslot.Add(mst_ship2.Maxeq[i]);
			}
			mem_shipBase.Nowhp = kyouka[Mem_ship.enumKyoukaIdx.Taik] + mst_ship2.Taik;
			mem_shipBase.Fuel = mst_ship2.Fuel_max;
			mem_shipBase.Bull = mst_ship2.Bull_max;
			mem_shipBase.Cond = 40;
			mem_shipBase.SetKyoukaValue(kyouka);
			value.Set_ShipParam(mem_shipBase, mst_ship2, enemy_flag: false);
			value.SumLovToRemodeling();
			if (drawingNum > 0)
			{
				Comm_UserDatas.Instance.User_useItem[58].Sub_UseItem(drawingNum);
			}
			int remodelDevKitNum = mst_ship2.GetRemodelDevKitNum();
			if (remodelDevKitNum > 0)
			{
				Comm_UserDatas.Instance.User_material[enumMaterialCategory.Dev_Kit].Sub_Material(remodelDevKitNum);
			}
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Steel].Sub_Material(mst_ship.Afterfuel);
			Comm_UserDatas.Instance.User_material[enumMaterialCategory.Bull].Sub_Material(mst_ship.Afterbull);
			Comm_UserDatas.Instance.Add_Book(1, value.Ship_id);
			api_Result.data = value;
			return api_Result;
		}

		private int getRemodelingTaik(int now_taik)
		{
			if (now_taik <= 29)
			{
				return 4;
			}
			if (now_taik <= 39)
			{
				return 5;
			}
			if (now_taik <= 49)
			{
				return 6;
			}
			if (now_taik <= 69)
			{
				return 7;
			}
			if (now_taik <= 89)
			{
				return 8;
			}
			return 9;
		}

		public bool ValidMarriage(int ship_rid)
		{
			Mem_ship value = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, out value))
			{
				return false;
			}
			if (value.Level != 99)
			{
				return false;
			}
			return true;
		}

		public Api_Result<Mem_ship> Marriage(int ship_rid)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			if (!ValidMarriage(ship_rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_useitem value = null;
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(55, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (value.Value == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship[ship_rid];
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[mem_ship.Ship_id];
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			int num = mem_ship.Maxhp - mem_shipBase.C_taik_powerup;
			int num2 = num + getMariageTaik(num);
			if (num2 > mst_ship.Taik_max)
			{
				num2 = mst_ship.Taik_max;
			}
			mem_shipBase.C_taik = num2 - mst_ship.Taik;
			mem_shipBase.C_taik_powerup = ((num2 + mem_shipBase.C_taik_powerup <= mst_ship.Taik_max) ? mem_shipBase.C_taik_powerup : (mst_ship.Taik_max - num2));
			num2 = (mem_shipBase.Nowhp = num2 + mem_shipBase.C_taik_powerup);
			int luck = mem_ship.Luck;
			int num3 = (int)Utils.GetRandDouble(3.0, 6.0, 1.0, 1);
			int num4 = luck + num3;
			if (num4 > mst_ship.Luck_max)
			{
				num4 = mst_ship.Luck_max;
			}
			mem_shipBase.C_luck = num4 - mst_ship.Luck;
			mem_shipBase.Level = 100;
			mem_ship.Set_ShipParam(mem_shipBase, mst_ship, enemy_flag: false);
			Dictionary<int, int> mst_level = Mst_DataManager.Instance.Get_MstLevel(shipTable: true);
			mem_ship.SetRequireExp(mem_ship.Level, mst_level);
			mem_ship.SumLovToMarriage();
			Comm_UserDatas.Instance.Ship_book[mem_ship.Ship_id].UpdateShipBook(damage: false, mariage: true);
			Comm_UserDatas.Instance.User_useItem[55].Sub_UseItem(1);
			api_Result.data = mem_ship;
			return api_Result;
		}

		private int getMariageTaik(int now_taik)
		{
			return getRemodelingTaik(now_taik);
		}
	}
}
