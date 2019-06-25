using Common.Enum;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class Reward_Furniture : IReward
	{
		private int _mst_id;

		private Mst_furniture _mst;

		public int MstId => _mst_id;

		public string Name => (_mst == null) ? string.Empty : _mst.Title;

		public FurnitureKinds Type => (FurnitureKinds)TypeId;

		public int TypeId => (_mst != null) ? _mst.Type : 0;

		public int NoInType => (_mst != null) ? _mst.No : 0;

		public Reward_Furniture(int mst_id)
		{
			_mst_id = mst_id;
		}

		public void __Init__(Dictionary<FurnitureKinds, List<Mst_furniture>> all_mst)
		{
			foreach (List<Mst_furniture> value in all_mst.Values)
			{
				_mst = value.Find((Mst_furniture item) => item.Id == _mst_id);
				if (_mst != null)
				{
					break;
				}
			}
		}

		public override string ToString()
		{
			return $"{Name}(ID:{MstId}) TypeNo:{TypeId}-{NoInType}";
		}
	}
}
