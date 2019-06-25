using Common.Enum;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common.Formats.Battle
{
	public class EscapeInfo
	{
		public List<int> EscapeShips;

		public List<int> TowShips;

		private readonly HashSet<int> spItemID;

		public EscapeInfo()
		{
			spItemID = new HashSet<int>
			{
				107
			};
		}

		public EscapeInfo(List<Mem_ship> ships)
			: this()
		{
			if (haveFlagShipRequireItem(ships[0]))
			{
				IEnumerable<Mem_ship> enumerable = ships.Skip(1);
				setEscapeData(enumerable, enumerable);
			}
		}

		public EscapeInfo(List<Mem_ship> ships, List<Mem_ship> ships_combined)
			: this()
		{
			if (haveFlagShipRequireItem(ships[0]))
			{
				IEnumerable<Mem_ship> collection = ships.Skip(1);
				IEnumerable<Mem_ship> enumerable = ships_combined.Skip(1);
				List<Mem_ship> list = new List<Mem_ship>(collection);
				list.AddRange(enumerable);
				setEscapeData(list, enumerable);
			}
		}

		public bool ValidEscape()
		{
			if (EscapeShips == null || EscapeShips.Count == 0)
			{
				return false;
			}
			if (TowShips == null || TowShips.Count == 0)
			{
				return false;
			}
			return true;
		}

		private bool haveFlagShipRequireItem(Mem_ship flagShip)
		{
			Dictionary<int, int> mstSlotItemNum_OrderId = flagShip.GetMstSlotItemNum_OrderId(spItemID);
			return mstSlotItemNum_OrderId.Values.Any((int x) => x > 0);
		}

		private void setEscapeData(IEnumerable<Mem_ship> ships, IEnumerable<Mem_ship> enableTowShips)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			foreach (Mem_ship ship in ships)
			{
				if (ship.IsFight())
				{
					DamageState damageState = ship.Get_DamageState();
					bool flag = enableTowShips.Contains(ship);
					switch (damageState)
					{
					case DamageState.Taiha:
						list.Add(ship.Rid);
						break;
					case DamageState.Normal:
						if (ship.Stype == 2 && flag)
						{
							list2.Add(ship.Rid);
						}
						break;
					}
				}
			}
			if (list.Count > 0 && list2.Count > 0)
			{
				EscapeShips = list;
				TowShips = list2;
			}
		}
	}
}
