using Server_Models;

namespace local.models
{
	public class Reward_Useitem : IReward, IReward_Useitem
	{
		private Mst_useitem _mst;

		private int _count = 1;

		public int Id => _mst.Id;

		public string Name => _mst.Name;

		public int Count => _count;

		public Reward_Useitem(int mst_id)
		{
			_Init(mst_id, 1);
		}

		public Reward_Useitem(int mst_id, int count)
		{
			_Init(mst_id, count);
		}

		public Reward_Useitem(Mst_useitem mst)
		{
			_Init(mst, 1);
		}

		public Reward_Useitem(Mst_useitem mst, int count)
		{
			_Init(mst, count);
		}

		private void _Init(int mst_id, int count)
		{
			Mst_DataManager.Instance.Mst_useitem.TryGetValue(mst_id, out _mst);
			_count = count;
		}

		private void _Init(Mst_useitem mst, int count)
		{
			_mst = mst;
			_count = count;
		}

		public override string ToString()
		{
			return $"{Name}(ID:{Id}) 個数:{Count}";
		}
	}
}
