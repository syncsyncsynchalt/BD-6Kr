using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common
{
	public static class DeckUtils
	{
		public static bool IsInNdock(int areaid, List<Mem_ship> ships)
		{
			if (ships == null || ships.Count == 0)
			{
				return false;
			}
			IEnumerable<Mem_ndock> enumerable = from x in Comm_UserDatas.Instance.User_ndock.Values
				where x.Area_id == areaid && x.State == NdockStates.RESTORE
				select x;
			if (enumerable == null || enumerable.Count() == 0)
			{
				return false;
			}
			IEnumerable<int> rids = from ship in ships
				select ship.Rid;
			return enumerable.Any((Mem_ndock ndock) => rids.Contains(ndock.Ship_id));
		}

		public static EscortCheckKinds IsValidChangeEscort(List<Mem_ship> escortShips, Dictionary<int, int> stype_group, int index)
		{
			if (index > -1)
			{
				if (stype_group[escortShips[index].Stype] == 0)
				{
					return EscortCheckKinds.NotEscortShip;
				}
				if (index == 0 && (escortShips[index].Stype == 1 || escortShips[index].Stype == 2))
				{
					return EscortCheckKinds.NotFlagShip;
				}
				return EscortCheckKinds.OK;
			}
			return IsValidChangeEscortDeck(escortShips, stype_group);
		}

		private static EscortCheckKinds IsValidChangeEscortDeck(List<Mem_ship> escortShips, Dictionary<int, int> stype_group)
		{
			ILookup<int, Mem_ship> lookup = escortShips.ToLookup((Mem_ship x) => x.Stype);
			Dictionary<int, int> dictionary = stype_group.Values.Distinct().ToDictionary((int value) => value, (int value) => 0);
			List<EscortCheckKinds> list = new List<EscortCheckKinds>();
			Dictionary<EscortCheckKinds, bool> dictionary2 = new Dictionary<EscortCheckKinds, bool>();
			foreach (object value in Enum.GetValues(typeof(EscortCheckKinds)))
			{
				EscortCheckKinds escortCheckKinds = (EscortCheckKinds)(int)value;
				list.Add(escortCheckKinds);
				dictionary2.Add(escortCheckKinds, value: true);
			}
			foreach (IGrouping<int, Mem_ship> item in lookup)
			{
				int key = stype_group[item.Key];
				dictionary[key] = item.Count();
			}
			int num = dictionary[0];
			int num2 = dictionary[1];
			if (num >= 1)
			{
				dictionary2[EscortCheckKinds.NotEscortShip] = false;
			}
			else if (escortShips[0].Stype == 1 || escortShips[0].Stype == 2)
			{
				dictionary2[EscortCheckKinds.NotFlagShip] = false;
			}
			for (int i = 0; i < list.Count(); i++)
			{
				if (!dictionary2[list[i]])
				{
					return list[i];
				}
			}
			return EscortCheckKinds.OK;
		}
	}
}
