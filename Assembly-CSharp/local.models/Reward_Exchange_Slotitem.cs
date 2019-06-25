namespace local.models
{
	public class Reward_Exchange_Slotitem : IReward, IReward_Exchange_Slotitem
	{
		private Reward_Slotitem _item_from;

		private Reward_Slotitem _item_to;

		private bool _consumed_tojoin;

		public IReward_Slotitem ItemFrom => _item_from;

		public IReward_Slotitem ItemTo => _item_to;

		public Reward_Exchange_Slotitem(int mst_id_from, int mst_id_to, bool consumed_tojoin)
		{
			_item_from = new Reward_Slotitem(mst_id_from);
			_item_to = new Reward_Slotitem(mst_id_to);
			_consumed_tojoin = consumed_tojoin;
		}

		public bool IsCosumedTojoin()
		{
			return _consumed_tojoin;
		}

		public override string ToString()
		{
			return string.Format("機種転換報酬: {0} -> {1}{2}", ItemFrom, ItemTo, (!IsCosumedTojoin()) ? string.Empty : "[熟練搭乗員使用]");
		}
	}
}
