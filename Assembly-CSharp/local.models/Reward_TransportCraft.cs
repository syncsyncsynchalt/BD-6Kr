namespace local.models
{
	public class Reward_TransportCraft : IReward
	{
		private int _num;

		public int Num => _num;

		public Reward_TransportCraft(int num)
		{
			_num = num;
		}

		public override string ToString()
		{
			return $"輸送船報酬: {_num}隻";
		}
	}
}
