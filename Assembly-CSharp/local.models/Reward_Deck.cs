namespace local.models
{
	public class Reward_Deck : IReward
	{
		private int _deck_id;

		public int DeckId => _deck_id;

		public Reward_Deck(int deck_id)
		{
			_deck_id = deck_id;
		}

		public override string ToString()
		{
			return $"デッキ開放 第{DeckId}艦隊";
		}
	}
}
