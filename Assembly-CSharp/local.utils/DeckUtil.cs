using Common.Enum;
using local.models;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.utils
{
	public static class DeckUtil
	{
		public static HashSet<BattleFormationKinds1> GetSelectableFormations(DeckModel deck)
		{
			HashSet<BattleFormationKinds1> hashSet = new HashSet<BattleFormationKinds1>();
			if (deck != null)
			{
				int shipCount = deck.GetShipCount();
				hashSet.Add(BattleFormationKinds1.TanJuu);
				if (shipCount >= 4)
				{
					hashSet.Add(BattleFormationKinds1.FukuJuu);
					hashSet.Add(BattleFormationKinds1.Teikei);
					hashSet.Add(BattleFormationKinds1.TanOu);
					if (shipCount >= 5)
					{
						hashSet.Add(BattleFormationKinds1.Rinkei);
					}
				}
			}
			return hashSet;
		}

		public static int __IsInDeck__(int ship_mem_id)
		{
			return __IsInDeck__(ship_mem_id, checkPartnerShip: true);
		}

		public static int __IsInDeck__(int ship_mem_id, bool checkPartnerShip)
		{
			Dictionary<int, Mem_deck> data = new Api_get_Member().Deck().data;
			int[] array = data[1].Search_ShipIdx(data, ship_mem_id);
			if (checkPartnerShip && array[0] == 1 && array[1] == 0)
			{
				return 0;
			}
			return array[0];
		}

		public static List<ShipModel> GetSortedList(List<ShipModel> ships, SortKey sort_key)
		{
			List<ShipModel> range = ships.GetRange(0, ships.Count);
			switch (sort_key)
			{
			case SortKey.LEVEL:
				range.Sort((ShipModel a, ShipModel b) => _CompLevel(a, b));
				break;
			case SortKey.SHIPTYPE:
				range.Sort((ShipModel a, ShipModel b) => _CompSType(a, b));
				break;
			case SortKey.DAMAGE:
				range.Sort((ShipModel a, ShipModel b) => _CompDamage(a, b));
				break;
			case SortKey.NEW:
				range.Sort((ShipModel a, ShipModel b) => __CompGetNo(a, b) * -1);
				break;
			case SortKey.LEVEL_LOCK:
				range.Sort((ShipModel a, ShipModel b) => _CompLevelLock(a, b));
				break;
			case SortKey.LOCK_LEVEL:
				range.Sort((ShipModel a, ShipModel b) => _CompLockLevel(a, b));
				break;
			case SortKey.UNORGANIZED:
			{
				Dictionary<int, int> o_map = new Dictionary<int, int>();
				for (int i = 1; i <= 8; i++)
				{
					if (Comm_UserDatas.Instance.User_deck.TryGetValue(i, out Mem_deck value))
					{
						for (int j = 0; j < value.Ship.Count(); j++)
						{
							o_map.Add(value.Ship[j], i * 10 + j);
						}
					}
				}
				for (int k = 1; k <= 17; k++)
				{
					if (Comm_UserDatas.Instance.User_EscortDeck.TryGetValue(k, out Mem_esccort_deck value2))
					{
						for (int l = 0; l < value2.Ship.Count(); l++)
						{
							o_map.Add(value2.Ship[l], k * 100 + l);
						}
					}
				}
				range.Sort((ShipModel a, ShipModel b) => _CompUnOrganize(a, b, o_map));
				break;
			}
			}
			return range;
		}

		private static int _CompLevel(ShipModel a, ShipModel b)
		{
			int num = __CompLevel(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __CompSortNo(a, b);
			if (num != 0)
			{
				return num;
			}
			return __CompGetNo(a, b);
		}

		private static int _CompSType(ShipModel a, ShipModel b)
		{
			int num = __CompStype(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __CompSortNo(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __CompLevel(a, b);
			if (num != 0)
			{
				return num;
			}
			return __CompGetNo(a, b);
		}

		private static int _CompDamage(ShipModel a, ShipModel b)
		{
			int num = __CompDamage(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __CompSortNo(a, b);
			if (num != 0)
			{
				return num;
			}
			return __CompGetNo(a, b);
		}

		private static int _CompLevelLock(ShipModel a, ShipModel b)
		{
			int num = __CompLevel(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __CompLock(a, b);
			if (num != 0)
			{
				return num;
			}
			return __CompLov(a, b);
		}

		private static int _CompLockLevel(ShipModel a, ShipModel b)
		{
			int num = __CompLock(a, b);
			if (num != 0)
			{
				return num;
			}
			num = __CompLevel(a, b);
			if (num != 0)
			{
				return num;
			}
			return __CompLov(a, b);
		}

		private static int _CompUnOrganize(ShipModel a, ShipModel b, Dictionary<int, int> organize_map)
		{
			int num = __CompOrganize(a, b, organize_map);
			if (num != 0)
			{
				return num;
			}
			return _CompLevel(a, b);
		}

		private static int __CompLevel(ShipModel a, ShipModel b)
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

		private static int __CompSortNo(ShipModel a, ShipModel b)
		{
			if (a.SortNo > b.SortNo)
			{
				return 1;
			}
			if (a.SortNo < b.SortNo)
			{
				return -1;
			}
			return 0;
		}

		private static int __CompMemId(ShipModel a, ShipModel b)
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

		private static int __CompGetNo(ShipModel a, ShipModel b)
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

		private static int __CompStype(ShipModel a, ShipModel b)
		{
			if (a.ShipType < b.ShipType)
			{
				return 1;
			}
			if (a.ShipType > b.ShipType)
			{
				return -1;
			}
			return 0;
		}

		private static int __CompDamage(ShipModel a, ShipModel b)
		{
			if (a.TaikyuRate > b.TaikyuRate)
			{
				return 1;
			}
			if (a.TaikyuRate < b.TaikyuRate)
			{
				return -1;
			}
			return 0;
		}

		private static int __CompLock(ShipModel a, ShipModel b)
		{
			bool flag = a.IsLocked();
			bool flag2 = b.IsLocked();
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

		private static int __CompLov(ShipModel a, ShipModel b)
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

		private static int __CompOrganize(ShipModel a, ShipModel b, Dictionary<int, int> organize_map)
		{
			organize_map.TryGetValue(a.MemId, out int value);
			organize_map.TryGetValue(b.MemId, out int value2);
			if (value > value2)
			{
				return 1;
			}
			if (value < value2)
			{
				return -1;
			}
			return 0;
		}
	}
}
