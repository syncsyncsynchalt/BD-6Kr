using Server_Common.Formats;
using System.Collections.Generic;

namespace local.models
{
	public class MapSupplyModel
	{
		private ShipModel _ship;

		private List<ShipModel> _given_ships;

		public ShipModel Ship => _ship;

		public List<ShipModel> GivenShips => _given_ships;

		public MapSupplyModel(DeckModel deck, MapSupplyFmt fmt)
		{
			_ship = deck.GetShipFromMemId(fmt.useShip);
			_given_ships = fmt.givenShip.ConvertAll((int mem_id) => deck.GetShipFromMemId(mem_id));
		}
	}
}
