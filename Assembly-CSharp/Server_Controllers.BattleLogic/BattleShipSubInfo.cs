using Server_Models;

namespace Server_Controllers.BattleLogic
{
	public class BattleShipSubInfo
	{
		private Mem_ship shipInstance;

		private int deckIdx;

		private int totalDamage;

		private int attackNo;

		public Mem_ship ShipInstance
		{
			get
			{
				return shipInstance;
			}
			private set
			{
				shipInstance = value;
			}
		}

		public int DeckIdx
		{
			get
			{
				return deckIdx;
			}
			private set
			{
				deckIdx = value;
			}
		}

		public int TotalDamage
		{
			get
			{
				return totalDamage;
			}
			set
			{
				totalDamage = value;
			}
		}

		public int AttackNo
		{
			get
			{
				return attackNo;
			}
			private set
			{
				attackNo = value;
			}
		}

		public BattleShipSubInfo(int deck_idx, Mem_ship ship)
		{
			DeckIdx = deck_idx;
			ShipInstance = ship;
		}

		public BattleShipSubInfo(int deck_idx, Mem_ship ship, int attackNo)
		{
			DeckIdx = deck_idx;
			ShipInstance = ship;
			AttackNo = attackNo;
		}
	}
}
