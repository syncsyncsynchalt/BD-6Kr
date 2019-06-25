namespace local.models
{
	public class Reward_SPoint : IReward
	{
		private int _spoint;

		public int SPoint => _spoint;

		public Reward_SPoint(int point)
		{
			_spoint = point;
		}

		public override string ToString()
		{
			return $"戦略ポイント報酬: {SPoint}";
		}
	}
}
