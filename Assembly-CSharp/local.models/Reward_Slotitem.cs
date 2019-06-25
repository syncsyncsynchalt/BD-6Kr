using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class Reward_Slotitem : IReward, IReward_Slotitem
	{
		private Mst_slotitem _mst;

		private int _count = 1;

		public int Id => _mst.Id;

		public string Name => _mst.Name;

		public int Rare => _mst.Rare;

		public int Count => _count;

		public string Type3Name
		{
			get
			{
				int key = (_mst != null) ? _mst.Type3 : 0;
				if (Mst_DataManager.Instance.GetSlotItemEquipTypeName().TryGetValue(key, out KeyValuePair<int, string> value))
				{
					return (value.Key != 1) ? string.Empty : value.Value;
				}
				return string.Empty;
			}
		}

		public Reward_Slotitem(int mst_id)
		{
			_Init(mst_id, 1);
		}

		public Reward_Slotitem(int mst_id, int count)
		{
			_Init(mst_id, count);
		}

		public Reward_Slotitem(Mst_slotitem mst)
		{
			_Init(mst, 1);
		}

		public Reward_Slotitem(Mst_slotitem mst, int count)
		{
			_Init(mst, count);
		}

		private void _Init(int mst_id, int count)
		{
			Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(mst_id, out _mst);
			_count = count;
		}

		private void _Init(Mst_slotitem mst, int count)
		{
			_mst = mst;
			_count = count;
		}

		public override string ToString()
		{
			return $"{Type3Name} {Name}(ID:{Id}) レア度:{Rare} 個数:{Count}";
		}
	}
}
