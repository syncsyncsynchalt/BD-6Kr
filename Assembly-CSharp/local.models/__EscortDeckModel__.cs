using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class __EscortDeckModel__ : EscortDeckModel
	{
		public __EscortDeckModel__(Mem_esccort_deck mem_escort_deck, Dictionary<int, ShipModel> ships)
			: base(mem_escort_deck, ships)
		{
		}

		public TemporaryEscortDeckModel GetCloneDeck(Dictionary<int, ShipModel> ships)
		{
			_mem_escort_deck.Ship.Clone(out DeckShips out_ships);
			return new TemporaryEscortDeckModel(Id, out_ships, _mem_escort_deck.Name, ships);
		}
	}
}
