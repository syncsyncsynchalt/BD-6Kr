using KCV.EscortOrganize;
using KCV.Organize;
using local.models;
using System.Linq;

public class EscortOrganizeShipListChild : OrganizeShipListChild
{
	protected override bool IsDeckInShip(ShipModel shipModel)
	{
		DeckModelBase deck = shipModel.getDeck();
		bool flag = deck != null;
		bool flag2 = EscortOrganizeTaskManager.GetEscortManager().EditDeck.GetShips().Contains(shipModel);
		return flag || flag2;
	}

	protected override DeckModelBase GetDeckFromShip(ShipModel shipModel)
	{
		DeckModelBase deck = shipModel.getDeck();
		if (deck != null)
		{
			return deck;
		}
		return EscortOrganizeTaskManager.GetEscortManager().EditDeck;
	}
}
