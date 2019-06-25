using Common.Enum;
using Server_Common;
using Server_Models;

namespace local.models
{
	public class FurnitureModel
	{
		protected Mst_furniture _mst;

		private string _description;

		public int MstId => _mst.Id;

		public string Name => _mst.Title;

		public FurnitureKinds Type => (FurnitureKinds)TypeId;

		public int TypeId => _mst.Type;

		public int NoInType => _mst.No;

		public int Price => _mst.Price;

		public int Rarity => _mst.Rarity;

		public int SeasonId => _mst.Season;

		public string Description => _description;

		public FurnitureModel(Mst_furniture mst, string description)
		{
			_mst = mst;
			_description = description;
		}

		public bool IsSalled()
		{
			return (_mst.Saleflg == 1) ? true : false;
		}

		public bool IsPossession()
		{
			return Comm_UserDatas.Instance.User_furniture.ContainsKey(MstId);
		}

		public bool IsNeedWorker()
		{
			return _mst.IsRequireWorker();
		}

		public virtual bool GetSettingFlg(int deck_id)
		{
			if (Comm_UserDatas.Instance.User_room.TryGetValue(deck_id, out Mem_room value))
			{
				switch (Type)
				{
				case FurnitureKinds.Floor:
					return value.Furniture1 == MstId;
				case FurnitureKinds.Wall:
					return value.Furniture2 == MstId;
				case FurnitureKinds.Window:
					return value.Furniture3 == MstId;
				case FurnitureKinds.Hangings:
					return value.Furniture4 == MstId;
				case FurnitureKinds.Chest:
					return value.Furniture5 == MstId;
				case FurnitureKinds.Desk:
					return value.Furniture6 == MstId;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return $"{Name}(ID:{MstId}) TypeNo:{TypeId}-{NoInType}";
		}
	}
}
