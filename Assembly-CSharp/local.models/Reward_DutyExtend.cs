namespace local.models
{
	public class Reward_DutyExtend : IReward
	{
		private int _max_num;

		private int MaxNum => _max_num;

		public Reward_DutyExtend(int max_num)
		{
			_max_num = max_num;
		}

		public override string ToString()
		{
			return $"任務拡張報酬: 最大同時遂行数:{_max_num}";
		}
	}
}
