using KCV.EscortOrganize;
using KCV.Organize;
using local.models;

public class DeployOrganizeScrollListChild : OrganizeScrollListChild
{
	protected override bool IsDeckInShip(ShipModel shipModel)
	{
		DeckModelBase deck = shipModel.getDeck();
		bool flag = deck != null && !deck.IsEscortDeckMyself();
		bool flag2 = false;
		ShipModel[] ships = EscortOrganizeTaskManager.GetEscortManager().EditDeck.GetShips();
		ShipModel[] array = ships;
		foreach (ShipModel shipModel2 in array)
		{
			if (shipModel2.MemId == shipModel.MemId)
			{
				flag2 = true;
				break;
			}
		}
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
