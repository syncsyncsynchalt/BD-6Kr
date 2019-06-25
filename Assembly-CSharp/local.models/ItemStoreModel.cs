using Server_Common;
using Server_Models;

namespace local.models
{
	public class ItemStoreModel
	{
		private Mst_payitem _mst_data;

		public int MstId => (_mst_data != null) ? _mst_data.Id : 0;

		public string Name => (_mst_data != null) ? _mst_data.Name : string.Empty;

		public string Description
		{
			get
			{
				if (_mst_data == null)
				{
					return string.Empty;
				}
				if (MstId == 16)
				{
					int portMaxExtendNum = Comm_UserDatas.Instance.User_basic.GetPortMaxExtendNum();
					return string.Format(_mst_data.Description, portMaxExtendNum);
				}
				return _mst_data.Description;
			}
		}

		public int Price => (_mst_data != null) ? _mst_data.Price : 0;

		public int Count => (_mst_data != null) ? _mst_data.GetBuyNum() : 0;

		public ItemStoreModel(Mst_payitem mst_data)
		{
			_mst_data = mst_data;
		}

		public override string ToString()
		{
			if (Count < 0)
			{
				return $"[{MstId}]{Name}  価格{Price}";
			}
			if (Count == 0)
			{
				return $"[{MstId}][購入不可]{Name}  価格{Price}";
			}
			return string.Format("[{0}][残{3}]{1}  価格{2}", MstId, Name, Price, Count);
		}
	}
}
