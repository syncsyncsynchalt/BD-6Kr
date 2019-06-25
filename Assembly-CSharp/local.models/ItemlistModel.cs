using Common.Enum;
using Server_Common;
using Server_Models;

namespace local.models
{
	public class ItemlistModel
	{
		private Mst_useitem _mst_data;

		private Mem_useitem _mem_data;

		private string _description = string.Empty;

		private int _override_count;

		public int MstId => (_mst_data != null) ? _mst_data.Id : 0;

		public int Category => (_mst_data != null) ? _mst_data.Category : 0;

		public string Name => (_mst_data != null) ? _mst_data.Name : string.Empty;

		public string Description
		{
			get
			{
				if (_description == null)
				{
					return string.Empty;
				}
				if (MstId == 53)
				{
					int portMaxExtendNum = Comm_UserDatas.Instance.User_basic.GetPortMaxExtendNum();
					int portMaxSlotItemNum = Comm_UserDatas.Instance.User_basic.GetPortMaxSlotItemNum();
					return string.Format(_description, portMaxExtendNum, portMaxSlotItemNum);
				}
				return _description;
			}
		}

		public string Description2 => (_mst_data != null) ? _mst_data.Description2 : string.Empty;

		public int Price => (_mst_data != null) ? _mst_data.Price : 0;

		public int Count
		{
			get
			{
				if (_override_count > 0)
				{
					return _override_count;
				}
				return (_mem_data != null) ? _mem_data.Value : 0;
			}
		}

		public ItemlistModel(Mst_useitem mst_data, Mem_useitem mem_data, string description)
		{
			_mst_data = mst_data;
			_mem_data = mem_data;
			_description = description;
		}

		public bool IsUsable()
		{
			return _mst_data != null && _mst_data.Usetype == 4;
		}

		public int GetSpendCountInUse(ItemExchangeKinds use_type)
		{
			if (_mst_data == null)
			{
				return 0;
			}
			return _mst_data.GetItemExchangeNum(use_type);
		}

		public void __SetOverrideCount__(int value)
		{
			_override_count = value;
		}

		public override string ToString()
		{
			return $"[{MstId}]{Name} {Count}個所有";
		}
	}
}
