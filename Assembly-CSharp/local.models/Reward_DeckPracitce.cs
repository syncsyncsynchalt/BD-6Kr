using Common.Enum;

namespace local.models
{
	public class Reward_DeckPracitce : IReward
	{
		private DeckPracticeType _type;

		public DeckPracticeType type => _type;

		public Reward_DeckPracitce(int opened_deckpractice_id)
		{
			_type = (DeckPracticeType)opened_deckpractice_id;
		}

		public override string ToString()
		{
			return $"演習タイプ開放報酬: {type}";
		}
	}
}
