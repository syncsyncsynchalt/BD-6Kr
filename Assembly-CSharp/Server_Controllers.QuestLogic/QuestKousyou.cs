using Common.Enum;
using Server_Common;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server_Controllers.QuestLogic
{
	public class QuestKousyou : QuestLogicBase
	{
		private enum KousyouKind
		{
			CreateShip = 1,
			CreateSlot,
			DestroyShip,
			DestroyItem,
			RemodelSlot
		}

		private KousyouKind type;

		private int createShipId;

		private Mst_ship destroyShip;

		private List<Mst_slotitem> destroyItems;

		private Mst_slotitem_remodel_detail remodelDetail;

		private Mem_slotitem remodelAfterSlot;

		private Dictionary<enumMaterialCategory, int> useMaterial;

		private bool successFlag;

		private QuestKousyou()
		{
			checkData = getCheckDatas(6);
		}

		public QuestKousyou(Dictionary<enumMaterialCategory, int> useMat, int createShip)
			: this()
		{
			type = KousyouKind.CreateShip;
			useMaterial = useMat;
			createShipId = createShip;
		}

		public QuestKousyou(Dictionary<enumMaterialCategory, int> useMat, bool success)
			: this()
		{
			type = KousyouKind.CreateSlot;
			useMaterial = useMat;
			successFlag = success;
		}

		public QuestKousyou(Mst_ship destroyShip)
			: this()
		{
			type = KousyouKind.DestroyShip;
			this.destroyShip = destroyShip;
		}

		public QuestKousyou(List<Mst_slotitem> destroySlotItem)
			: this()
		{
			type = KousyouKind.DestroyItem;
			destroyItems = destroySlotItem;
		}

		public QuestKousyou(Mst_slotitem_remodel_detail menuData, Mem_slotitem afterSlotItem, bool success)
			: this()
		{
			type = KousyouKind.RemodelSlot;
			remodelDetail = menuData;
			remodelAfterSlot = afterSlotItem;
			successFlag = success;
		}

		public override List<int> ExecuteCheck()
		{
			List<int> list = new List<int>(checkData.Count);
			foreach (Mem_quest checkDatum in checkData)
			{
				string funcName = getFuncName(checkDatum);
				if ((bool)GetType().InvokeMember(funcName, BindingFlags.InvokeMethod, null, this, new object[1]
				{
					checkDatum
				}))
				{
					checkDatum.StateChange(this, QuestState.COMPLETE);
					list.Add(checkDatum.Rid);
				}
			}
			return list;
		}

		public bool Check_01(Mem_quest targetQuest)
		{
			if (type != KousyouKind.CreateShip)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (type != KousyouKind.CreateSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyShip)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			if (type != KousyouKind.CreateSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			if (type != KousyouKind.CreateShip)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (type != KousyouKind.CreateSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			if (type != KousyouKind.CreateShip)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyShip)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			int count = destroyItems.Count;
			for (int i = 0; i < count; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			int count = destroyItems.Count;
			for (int i = 0; i < count; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			int count = destroyItems.Count;
			for (int i = 0; i < count; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			int count = destroyItems.Count;
			for (int i = 0; i < count; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, int> mstSlotItemNum_OrderId = flagShip.GetMstSlotItemNum_OrderId(new HashSet<int>
			{
				93
			});
			if (mstSlotItemNum_OrderId[93] == 0)
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 17);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, int> mstSlotItemNum_OrderId = flagShip.GetMstSlotItemNum_OrderId(new HashSet<int>
			{
				99
			});
			if (mstSlotItemNum_OrderId[99] == 0)
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 24);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, int> mstSlotItemNum_OrderId = flagShip.GetMstSlotItemNum_OrderId(new HashSet<int>
			{
				109
			});
			if (mstSlotItemNum_OrderId[109] == 0)
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 22);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			int count = destroyItems.Count;
			for (int i = 0; i < count; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			if (type != KousyouKind.RemodelSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			if (type != KousyouKind.RemodelSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			addCounterIncrementAll(counter);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_20(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, List<int>> slotIndexFromId = flagShip.GetSlotIndexFromId(new HashSet<int>
			{
				16
			});
			if (slotIndexFromId[16].Count == 0)
			{
				return false;
			}
			bool flag = false;
			foreach (int item in slotIndexFromId[16])
			{
				if (Comm_UserDatas.Instance.User_slot[flagShip.Slot[item]].IsMaxSkillLevel())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 16);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_21(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, List<int>> slotIndexFromId = flagShip.GetSlotIndexFromId(new HashSet<int>
			{
				23
			});
			if (slotIndexFromId[23].Count == 0)
			{
				return false;
			}
			bool flag = false;
			foreach (int item in slotIndexFromId[23])
			{
				if (Comm_UserDatas.Instance.User_slot[flagShip.Slot[item]].IsMaxSkillLevel())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 23);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_22(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			string yomi = Mst_DataManager.Instance.Mst_ship[flagShip.Ship_id].Yomi;
			if (!yomi.Equals("しょうかく"))
			{
				return false;
			}
			Dictionary<int, int> mstSlotItemNum_OrderId = flagShip.GetMstSlotItemNum_OrderId(new HashSet<int>
			{
				143
			});
			if (mstSlotItemNum_OrderId[143] == 0)
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 17);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_23(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			string yomi = Mst_DataManager.Instance.Mst_ship[flagShip.Ship_id].Yomi;
			if (!yomi.Equals("しょうかく") && !yomi.Equals("あかぎ"))
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 16);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_24(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			int count = destroyItems.Count;
			for (int i = 0; i < count; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_25(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			string yomi = Mst_DataManager.Instance.Mst_ship[flagShip.Ship_id].Yomi;
			if (!yomi.Equals("ほうしょう"))
			{
				return false;
			}
			Dictionary<int, List<int>> slotIndexFromId = flagShip.GetSlotIndexFromId(new HashSet<int>
			{
				20
			});
			if (slotIndexFromId[20].Count == 0)
			{
				return false;
			}
			bool flag = false;
			foreach (int item in slotIndexFromId[20])
			{
				if (Comm_UserDatas.Instance.User_slot[flagShip.Slot[item]].IsMaxSkillLevel())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = 2;
			int num2 = 0;
			Mem_questcount value = new Mem_questcount();
			if (Comm_UserDatas.Instance.User_questcount.TryGetValue(6806, out value))
			{
				num2 = value.Value;
			}
			int num3 = 1;
			int num4 = 0;
			Mem_questcount value2 = new Mem_questcount();
			if (Comm_UserDatas.Instance.User_questcount.TryGetValue(6807, out value2))
			{
				num4 = value2.Value;
			}
			foreach (Mst_slotitem destroyItem in destroyItems)
			{
				if (destroyItem.Id == 20 && num2 < num)
				{
					num2++;
				}
				if (destroyItem.Id == 19 && num4 < num3)
				{
					num4++;
				}
			}
			if (dictionary.ContainsKey(6806))
			{
				dictionary[6806] = num2;
			}
			if (dictionary.ContainsKey(6807))
			{
				dictionary[6807] = num4;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_26(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, int> mstSlotItemNum_OrderId = flagShip.GetMstSlotItemNum_OrderId(new HashSet<int>
			{
				96
			});
			if (mstSlotItemNum_OrderId[96] == 0)
			{
				return false;
			}
			int num = destroyItems.Count((Mst_slotitem x) => x.Id == 21);
			for (int i = 0; i < num; i++)
			{
				addCounterIncrementAll(counter);
			}
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_27(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, List<int>> slotIndexFromId = flagShip.GetSlotIndexFromId(new HashSet<int>
			{
				22
			});
			if (slotIndexFromId[22].Count == 0)
			{
				return false;
			}
			bool flag = false;
			foreach (int item in slotIndexFromId[22])
			{
				if (Comm_UserDatas.Instance.User_slot[flagShip.Slot[item]].IsMaxSkillLevel())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			foreach (Mst_slotitem destroyItem in destroyItems)
			{
				if (destroyItem.Id == 22)
				{
					num++;
				}
				if (destroyItem.Id == 21)
				{
					num2++;
				}
			}
			if (dictionary.ContainsKey(6809) && num > 0)
			{
				dictionary[6809] = num;
			}
			if (dictionary.ContainsKey(6810) && num2 > 0)
			{
				dictionary[6810] = num2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_28(Mem_quest targetQuest)
		{
			if (type != KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> dictionary = isAddCounter(targetQuest.Rid, checkData);
			Mem_ship flagShip = getFlagShip(1);
			Dictionary<int, List<int>> slotIndexFromId = flagShip.GetSlotIndexFromId(new HashSet<int>
			{
				79
			});
			if (slotIndexFromId[79].Count == 0)
			{
				return false;
			}
			bool flag = false;
			foreach (int item in slotIndexFromId[79])
			{
				if (Comm_UserDatas.Instance.User_slot[flagShip.Slot[item]].IsMaxSkillLevel())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			foreach (Mst_slotitem destroyItem in destroyItems)
			{
				if (destroyItem.Id == 80)
				{
					num++;
				}
				if (destroyItem.Id == 26)
				{
					num2++;
				}
			}
			if (dictionary.ContainsKey(6811))
			{
				dictionary[6811] = num;
			}
			if (dictionary.ContainsKey(6812))
			{
				dictionary[6812] = num2;
			}
			addCounter(dictionary);
			return CheckClearCounter(targetQuest.Rid);
		}

		public static List<string> GetRequireShip(int quest_id)
		{
			List<string> list = new List<string>();
			if (quest_id == 622)
			{
				list.Add("しょうかく");
			}
			if (quest_id == 625)
			{
				list.Add("ほうしょう");
			}
			return list;
		}

		private Mem_ship getFlagShip(int deckRid)
		{
			int key = Comm_UserDatas.Instance.User_deck[deckRid].Ship[0];
			return Comm_UserDatas.Instance.User_ship[key];
		}
	}
}
