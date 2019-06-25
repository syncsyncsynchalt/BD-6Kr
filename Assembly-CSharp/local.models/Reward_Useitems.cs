using System.Collections.Generic;

namespace local.models
{
	public class Reward_Useitems : IReward
	{
		private List<IReward_Useitem> _use_items;

		public IReward_Useitem[] Rewards => _use_items.ToArray();

		public Reward_Useitems()
		{
			_use_items = new List<IReward_Useitem>();
		}

		public void __Add__(int mst_id, int count)
		{
			int num = _use_items.FindIndex((IReward_Useitem i) => i.Id == mst_id);
			if (num == -1)
			{
				_use_items.Add(new Reward_Useitem(mst_id, count));
				_use_items.Sort((IReward_Useitem a, IReward_Useitem b) => a.Id - b.Id);
			}
			else
			{
				IReward_Useitem reward_Useitem = _use_items[num];
				_use_items[num] = new Reward_Useitem(mst_id, count + reward_Useitem.Count);
			}
		}

		public override string ToString()
		{
			string str = $"複数使用アイテム:[ ";
			for (int i = 0; i < _use_items.Count; i++)
			{
				str += $"({_use_items[i]})";
				if (i < _use_items.Count - 1)
				{
					str += ", ";
				}
			}
			return str + "]";
		}
	}
}
