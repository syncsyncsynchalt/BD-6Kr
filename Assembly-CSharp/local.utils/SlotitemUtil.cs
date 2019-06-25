using local.models;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.utils
{
	public static class SlotitemUtil
	{
		public enum SlotitemSortKey
		{
			Type3,
			LEVEL_ASCENDING,
			LEVEL_DESCENDING
		}

		public static List<SlotitemModel> __GetAllSlotitems__()
		{
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success)
			{
				List<SlotitemModel> list = new List<SlotitemModel>(api_Result.data.Count);
				{
					foreach (Mem_slotitem value in api_Result.data.Values)
					{
						list.Add(new SlotitemModel(value));
					}
					return list;
				}
			}
			return new List<SlotitemModel>();
		}

		public static List<SlotitemModel> __GetUnsetSlotitems__()
		{
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success)
			{
				List<SlotitemModel> list = new List<SlotitemModel>(api_Result.data.Count);
				{
					foreach (Mem_slotitem value in api_Result.data.Values)
					{
						if (value.Equip_flag == Mem_slotitem.enumEquipSts.Unset)
						{
							list.Add(new SlotitemModel(value));
						}
					}
					return list;
				}
			}
			return new List<SlotitemModel>();
		}

		public static List<SlotitemModel> GetSortedList(List<SlotitemModel> slots, SlotitemSortKey sort_key)
		{
			List<SlotitemModel> range = slots.GetRange(0, slots.Count);
			Sort(range, sort_key);
			return range;
		}

		public static void Sort(List<SlotitemModel> slots, SlotitemSortKey sort_key)
		{
			switch (sort_key)
			{
			case SlotitemSortKey.Type3:
				slots.Sort(delegate(SlotitemModel x, SlotitemModel y)
				{
					int num2 = _CompType3(x, y);
					if (num2 != 0)
					{
						return num2;
					}
					num2 = _CompMstId(x, y);
					return (num2 != 0) ? num2 : _CompGetNo(x, y);
				});
				break;
			case SlotitemSortKey.LEVEL_ASCENDING:
				slots.Sort(delegate(SlotitemModel x, SlotitemModel y)
				{
					int num = _CompType3(x, y);
					if (num != 0)
					{
						return num;
					}
					num = _CompMstId(x, y);
					if (num != 0)
					{
						return num;
					}
					num = _CompLevel(x, y);
					return (num != 0) ? num : _CompGetNo(x, y);
				});
				break;
			case SlotitemSortKey.LEVEL_DESCENDING:
				slots.Sort(delegate(SlotitemModel x, SlotitemModel y)
				{
					int num3 = _CompType3(x, y);
					if (num3 != 0)
					{
						return num3;
					}
					num3 = _CompMstId(x, y);
					if (num3 != 0)
					{
						return num3;
					}
					num3 = _CompLevel(x, y) * -1;
					return (num3 != 0) ? num3 : _CompGetNo(x, y);
				});
				break;
			}
		}

		private static int _CompType3(SlotitemModel a, SlotitemModel b)
		{
			if (a.Type3 > b.Type3)
			{
				return 1;
			}
			if (a.Type3 < b.Type3)
			{
				return -1;
			}
			return 0;
		}

		private static int _CompMstId(SlotitemModel a, SlotitemModel b)
		{
			if (a.MstId > b.MstId)
			{
				return 1;
			}
			if (a.MstId < b.MstId)
			{
				return -1;
			}
			return 0;
		}

		private static int _CompMemId(SlotitemModel a, SlotitemModel b)
		{
			if (a.MemId > b.MemId)
			{
				return 1;
			}
			if (a.MemId < b.MemId)
			{
				return -1;
			}
			return 0;
		}

		private static int _CompGetNo(SlotitemModel a, SlotitemModel b)
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

		private static int _CompLevel(SlotitemModel a, SlotitemModel b)
		{
			if (a.Level > b.Level)
			{
				return 1;
			}
			if (a.Level < b.Level)
			{
				return -1;
			}
			return 0;
		}
	}
}
