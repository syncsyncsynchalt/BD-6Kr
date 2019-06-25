using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Ending
	{
		private delegate bool getRankDlgt(DifficultKind difficult, DateTime nowDt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive);

		private Mem_newgame_plus mem_newgame;

		public Api_req_Ending()
		{
			mem_newgame = Comm_UserDatas.Instance.User_plus;
		}

		public bool GetOverallRank(out OverallRank clearRank, out int decorationNum)
		{
			clearRank = OverallRank.F;
			decorationNum = 0;
			List<getRankDlgt> list = new List<getRankDlgt>();
			list.Add(this.setRankEx);
			list.Add(this.setRankS);
			list.Add(this.setRankA);
			list.Add(this.setRankB);
			list.Add(this.setRankC);
			list.Add(this.setRankD);
			list.Add(this.setRankD);
			list.Add(this.setRankE);
			list.Add(this.setRankF);
			List<getRankDlgt> list2 = list;
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			DateTime dateTime = Comm_UserDatas.Instance.User_turn.GetDateTime();
			int elapsedYear = Comm_UserDatas.Instance.User_turn.GetElapsedYear(dateTime);
			uint lostShipNum = Comm_UserDatas.Instance.User_record.LostShipNum;
			bool outPositive = true;
			foreach (getRankDlgt item in list2)
			{
				if (item(difficult, dateTime, elapsedYear, lostShipNum, ref clearRank, ref decorationNum, ref outPositive))
				{
					return outPositive;
				}
			}
			clearRank = OverallRank.F;
			return outPositive;
		}

		public int GetTakeOverShipCount()
		{
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.SHI))
			{
				return 88;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.KOU))
			{
				return 65;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.OTU))
			{
				return 50;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.HEI))
			{
				return 35;
			}
			return 20;
		}

		public int GetTakeOverSlotCount()
		{
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.SHI))
			{
				return 50;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.KOU))
			{
				return 40;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.OTU))
			{
				return 30;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.HEI))
			{
				return 20;
			}
			return 10;
		}

		public bool IsGoTrueEnd()
		{
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			return Comm_UserDatas.Instance.User_ship.Values.Any((Mem_ship x) => mst_ship[x.Ship_id].Yomi.Equals("アイオワ"));
		}

		public void PurgeNewGamePlus()
		{
			mem_newgame.PurgeData();
		}

		public void CreateNewGamePlusData(bool level)
		{
			int priority = (!level) ? 1 : 0;
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			List<Mem_ship> ships = Comm_UserDatas.Instance.User_ship.Values.ToList();
			Mem_ship value = null;
			Comm_UserDatas.Instance.User_ship.TryGetValue(mem_newgame.TempRewardShipRid, out value);
			List<Mem_ship> sortedShipList = getSortedShipList(ships, priority);
			if (value != null)
			{
				sortedShipList.Remove(value);
				sortedShipList.Insert(0, value);
			}
			IEnumerable<Mem_ship> enumerable = sortedShipList.Take(GetTakeOverShipCount());
			List<Mem_shipBase> list = new List<Mem_shipBase>();
			List<Mem_slotitem> takeOverSlotItems = new List<Mem_slotitem>();
			foreach (Mem_ship item2 in enumerable)
			{
				Mem_shipBase item = new Mem_shipBase(item2);
				list.Add(item);
				item2.Slot.ForEach(delegate(int slot_rid)
				{
					if (slot_rid > 0)
					{
						takeOverSlotItems.Add(Comm_UserDatas.Instance.User_slot[slot_rid]);
					}
				});
				if (item2.Exslot > 0)
				{
					takeOverSlotItems.Add(Comm_UserDatas.Instance.User_slot[item2.Exslot]);
				}
			}
			IEnumerable<Mem_slotitem> source = Comm_UserDatas.Instance.User_slot.Values.Except(takeOverSlotItems);
			IEnumerable<Mem_slotitem> source2 = from x in source
				where x.Equip_flag == Mem_slotitem.enumEquipSts.Unset
				select x;
			List<Mem_slotitem> sortedSlotList = getSortedSlotList(source2.ToList());
			IEnumerable<Mem_slotitem> collection = sortedSlotList.Take(GetTakeOverSlotCount());
			takeOverSlotItems.AddRange(collection);
			mem_newgame.SetData(list, takeOverSlotItems);
		}

		public List<Mem_shipBase> GetTakeOverShips()
		{
			return mem_newgame.Ship.ToList();
		}

		public List<Mem_slotitem> GetTakeOverSlotItems()
		{
			return mem_newgame.Slotitem.ToList();
		}

		private List<Mem_ship> getSortedShipList(List<Mem_ship> ships, int priority)
		{
			List<Mem_ship> range = ships.GetRange(0, ships.Count);
			range.Sort((Mem_ship a, Mem_ship b) => _compShip(a, b, priority));
			return range;
		}

		private List<Mem_slotitem> getSortedSlotList(List<Mem_slotitem> items)
		{
			List<Mem_slotitem> range = items.GetRange(0, items.Count);
			range.Sort((Mem_slotitem a, Mem_slotitem b) => _compSlot(a, b));
			return range;
		}

		private int _compShip(Mem_ship a, Mem_ship b, int priority)
		{
			int num = __compMarriage(a, b);
			if (num != 0)
			{
				return num;
			}
			switch (priority)
			{
			case 0:
				num = __compLevel(a, b);
				if (num != 0)
				{
					return num;
				}
				num = __compLock(a, b);
				if (num != 0)
				{
					return num;
				}
				break;
			case 1:
				num = __compLock(a, b);
				if (num != 0)
				{
					return num;
				}
				num = __compLevel(a, b);
				if (num != 0)
				{
					return num;
				}
				break;
			}
			num = __compLov(a, b);
			if (num != 0)
			{
				return num;
			}
			return __compGetNo(a, b);
		}

		private int _compSlot(Mem_slotitem a, Mem_slotitem b)
		{
			int num = __compSlotLock(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __compSlotRare(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __compSlotLevel(a, b);
			if (num != 0)
			{
				return num;
			}
			return __compSlotGetNo(a, b);
		}

		private int __compMarriage(Mem_ship a, Mem_ship b)
		{
			bool flag = (a.Level >= 100) ? true : false;
			bool flag2 = (b.Level >= 100) ? true : false;
			if (!flag && flag2)
			{
				return 1;
			}
			if (flag && !flag2)
			{
				return -1;
			}
			return 0;
		}

		private int __compLevel(Mem_ship a, Mem_ship b)
		{
			if (a.Level < b.Level)
			{
				return 1;
			}
			if (a.Level > b.Level)
			{
				return -1;
			}
			return 0;
		}

		private int __compSortNo(Mem_ship a, Mem_ship b)
		{
			if (a.Sortno > b.Sortno)
			{
				return 1;
			}
			if (a.Sortno < b.Sortno)
			{
				return -1;
			}
			return 0;
		}

		private int __compGetNo(Mem_ship a, Mem_ship b)
		{
			if (a.GetNo > b.GetNo)
			{
				return 1;
			}
			if (a.GetNo < b.GetNo)
			{
				return -1;
			}
			return 0;
		}

		private int __compLock(Mem_ship a, Mem_ship b)
		{
			if (a.Locked < b.Locked)
			{
				return 1;
			}
			if (a.Locked > b.Locked)
			{
				return -1;
			}
			return 0;
		}

		private int __compLov(Mem_ship a, Mem_ship b)
		{
			if (a.Lov < b.Lov)
			{
				return 1;
			}
			if (a.Lov > b.Lov)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotLock(Mem_slotitem a, Mem_slotitem b)
		{
			if (!a.Lock && b.Lock)
			{
				return 1;
			}
			if (a.Lock && !b.Lock)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotRare(Mem_slotitem a, Mem_slotitem b)
		{
			Dictionary<int, Mst_slotitem> mst_Slotitem = Mst_DataManager.Instance.Mst_Slotitem;
			if (mst_Slotitem[a.Slotitem_id].Rare < mst_Slotitem[b.Slotitem_id].Rare)
			{
				return 1;
			}
			if (mst_Slotitem[a.Slotitem_id].Rare > mst_Slotitem[b.Slotitem_id].Rare)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotLevel(Mem_slotitem a, Mem_slotitem b)
		{
			if (a.Level < b.Level)
			{
				return 1;
			}
			if (a.Level > b.Level)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotGetNo(Mem_slotitem a, Mem_slotitem b)
		{
			if (a.GetNo > b.GetNo)
			{
				return 1;
			}
			if (a.GetNo < b.GetNo)
			{
				return -1;
			}
			return 0;
		}

		private bool setRankEx(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			outPositive = true;
			outRank = OverallRank.EX;
			outDecNum = 0;
			if (difficult != DifficultKind.SHI)
			{
				return false;
			}
			if (elapsedYear <= 1 && lostNum == 0)
			{
				outDecNum = 4;
				return true;
			}
			if (elapsedYear <= 2 && lostNum == 0)
			{
				outDecNum = 3;
				return true;
			}
			if (elapsedYear <= 3 && lostNum == 0)
			{
				outDecNum = 2;
				DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
				if (dt.Date <= dateTime.Date)
				{
					return true;
				}
			}
			if (elapsedYear <= 2 && lostNum <= 3)
			{
				outDecNum = 1;
				return true;
			}
			if (elapsedYear <= 3 && lostNum <= 3)
			{
				outDecNum = 0;
				DateTime dateTime2 = user_turn.GetDateTime(3, 8, 15);
				if (dt.Date <= dateTime2.Date)
				{
					return true;
				}
			}
			return false;
		}

		private bool setRankS(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			if (difficult < DifficultKind.OTU)
			{
				return false;
			}
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			outPositive = true;
			outRank = OverallRank.S;
			outDecNum = 0;
			if (elapsedYear <= 1 && lostNum == 0)
			{
				outDecNum = 3;
				return true;
			}
			if (difficult >= DifficultKind.OTU)
			{
				outDecNum = 2;
				if (difficult == DifficultKind.SHI)
				{
					if (lostNum <= 6)
					{
						return true;
					}
				}
				else if (elapsedYear <= 2 && lostNum == 0)
				{
					return true;
				}
			}
			if (difficult >= DifficultKind.OTU)
			{
				outDecNum = 1;
				if (difficult == DifficultKind.SHI)
				{
					if (lostNum <= 9)
					{
						return true;
					}
				}
				else if (elapsedYear <= 3)
				{
					DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
					if (dt.Date <= dateTime.Date && lostNum == 0)
					{
						return true;
					}
				}
			}
			if (difficult >= DifficultKind.OTU)
			{
				outDecNum = 0;
				if (difficult == DifficultKind.SHI)
				{
					if (lostNum >= 10 && lostNum <= 19)
					{
						return true;
					}
				}
				else
				{
					DateTime dateTime2 = user_turn.GetDateTime(3, 8, 15);
					if (dt.Date <= dateTime2.Date && lostNum <= 3)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool setRankA(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			if (difficult > DifficultKind.KOU)
			{
				return false;
			}
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			outRank = OverallRank.A;
			outPositive = true;
			outDecNum = 0;
			if (difficult == DifficultKind.KOU && lostNum <= 6)
			{
				DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
				outDecNum = 3;
				if (dt.Date <= dateTime.Date)
				{
					return true;
				}
			}
			if (difficult <= DifficultKind.OTU)
			{
				outDecNum = 2;
				if (difficult != DifficultKind.OTU)
				{
					if (elapsedYear <= 1 && lostNum == 0)
					{
						return true;
					}
				}
				else
				{
					DateTime dateTime2 = user_turn.GetDateTime(3, 8, 15);
					if (lostNum <= 6 && dt.Date <= dateTime2.Date)
					{
						return true;
					}
				}
			}
			if (difficult <= DifficultKind.HEI)
			{
				outDecNum = 1;
				if (elapsedYear <= 2 && lostNum == 0)
				{
					return true;
				}
			}
			outDecNum = 0;
			if (difficult <= DifficultKind.HEI)
			{
				DateTime dateTime3 = user_turn.GetDateTime(3, 8, 15);
				if (dt.Date <= dateTime3.Date && lostNum == 0)
				{
					return true;
				}
			}
			return false;
		}

		private bool setRankB(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
			outDecNum = 0;
			outPositive = true;
			outRank = OverallRank.B;
			if (dt.Date <= dateTime.Date)
			{
				outDecNum = 2;
				if (difficult <= DifficultKind.HEI && lostNum <= 4)
				{
					return true;
				}
				if (difficult == DifficultKind.SHI && lostNum <= 19)
				{
					return true;
				}
				if ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && lostNum <= 9)
				{
					return true;
				}
			}
			outDecNum = 1;
			if (dt.Date <= dateTime.Date)
			{
				if (difficult <= DifficultKind.HEI && lostNum <= 9)
				{
					return true;
				}
				if ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && lostNum <= 14)
				{
					return true;
				}
			}
			if ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && lostNum <= 9)
			{
				return true;
			}
			outDecNum = 0;
			if (difficult <= DifficultKind.HEI && lostNum <= 9)
			{
				return true;
			}
			if ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && dt.Date <= dateTime.Date && lostNum <= 19)
			{
				return true;
			}
			if (difficult == DifficultKind.SHI && lostNum <= 19)
			{
				return true;
			}
			return false;
		}

		private bool setRankC(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outDecNum = 0;
			outPositive = true;
			outRank = OverallRank.C;
			if (difficult <= DifficultKind.KOU && lostNum >= 10 && lostNum <= 19)
			{
				return true;
			}
			outPositive = false;
			if (lostNum >= 20 && lostNum <= 24)
			{
				outDecNum = 1;
				return true;
			}
			if (lostNum >= 25 && lostNum <= 29)
			{
				outDecNum = 2;
				return true;
			}
			return false;
		}

		private bool setRankD(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outRank = OverallRank.D;
			outDecNum = 0;
			outPositive = true;
			if (lostNum >= 30 && lostNum <= 34)
			{
				return true;
			}
			return false;
		}

		private bool setRankE(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outRank = OverallRank.E;
			outDecNum = 0;
			outPositive = true;
			if (lostNum >= 35 && lostNum <= 39)
			{
				return true;
			}
			return false;
		}

		private bool setRankF(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outRank = OverallRank.F;
			outDecNum = 0;
			outPositive = true;
			if (lostNum >= 40)
			{
				return true;
			}
			return false;
		}
	}
}
