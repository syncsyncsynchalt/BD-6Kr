using local.models;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class ItemStoreManager : ManagerBase
	{
		public const int CABINET_NO = 1;

		private Api_req_store _req_store;

		private Dictionary<int, List<Mst_item_shop>> _mst_cabinets;

		private List<ItemStoreModel> _items;

		public List<ItemStoreModel> Items => _items;

		public ItemStoreManager()
		{
		}

		public ItemStoreManager(Dictionary<int, List<Mst_item_shop>> mst_cabinets)
		{
			_mst_cabinets = mst_cabinets;
		}

		public virtual void Init()
		{
			_Init(all_item: false);
		}

		public bool IsValidBuy(int paymentitem_mst_id, int count)
		{
			ItemStoreModel item = Items.Find((ItemStoreModel i) => i != null && i.MstId == paymentitem_mst_id);
			return IsValidBuy(item, count);
		}

		public bool IsValidBuy(ItemStoreModel item, int count)
		{
			if (item == null)
			{
				return false;
			}
			if (item.Count == 0)
			{
				return false;
			}
			if (count <= 0)
			{
				return false;
			}
			if (item.Count > 0 && count > item.Count)
			{
				return false;
			}
			if (item.Price * count > base.UserInfo.SPoint)
			{
				return false;
			}
			if (!_req_store.IsBuy(item.MstId, count))
			{
				return false;
			}
			return true;
		}

		public bool BuyItem(int paymentitem_mst_id, int count)
		{
			ItemStoreModel itemStoreModel = Items.Find((ItemStoreModel i) => i != null && i.MstId == paymentitem_mst_id);
			if (!IsValidBuy(itemStoreModel, count))
			{
				return false;
			}
			Api_Result<object> api_Result = _req_store.Buy(itemStoreModel.MstId, count);
			if (api_Result.state != 0)
			{
				return false;
			}
			return true;
		}

		protected void _Init(bool all_item)
		{
			if (_req_store == null)
			{
				_req_store = new Api_req_store();
			}
			Dictionary<int, Mst_payitem> storeList = _req_store.GetStoreList();
			_items = new List<ItemStoreModel>();
			if (_mst_cabinets == null)
			{
				_mst_cabinets = Mst_DataManager.Instance.GetMstCabinet();
			}
			List<Mst_item_shop> list = _mst_cabinets[1];
			list = list.GetRange(0, list.Count);
			if (all_item)
			{
				foreach (Mst_payitem value2 in storeList.Values)
				{
					if (!(value2.Name == string.Empty))
					{
						ItemStoreModel item = new ItemStoreModel(value2);
						_items.Add(item);
					}
				}
				_items.Sort((ItemStoreModel a, ItemStoreModel b) => (a.MstId > b.MstId) ? 1 : (-1));
			}
			else
			{
				foreach (Mst_item_shop item3 in list)
				{
					storeList.TryGetValue(item3.Item1_id, out Mst_payitem value);
					if (value == null)
					{
						_items.Add(null);
					}
					else
					{
						ItemStoreModel item2 = new ItemStoreModel(value);
						_items.Add(item2);
					}
				}
			}
		}

		public ItemlistManager CreateListManager()
		{
			return new ItemlistManager(_mst_cabinets);
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n";
			str += "-- アイテム屋商品 --\n";
			for (int i = 0; i < Items.Count; i++)
			{
				str += $"[{i}] {Items[i]}\n";
			}
			return str + "\n";
		}
	}
}
