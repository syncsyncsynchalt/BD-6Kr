using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class MapSupplyFmt
	{
		public int useShip;

		public List<int> givenShip;

		public MapSupplyFmt()
		{
		}

		public MapSupplyFmt(int use_ship, List<int> supply_ships)
		{
			useShip = use_ship;
			givenShip = supply_ships;
			givenShip.Remove(useShip);
		}
	}
}
