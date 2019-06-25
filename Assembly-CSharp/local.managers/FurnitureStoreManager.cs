using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.managers
{
	public class FurnitureStoreManager : ManagerBase
	{
		private Dictionary<FurnitureKinds, List<FurnitureModel>> _StoreItems;

		private Dictionary<int, string> _desciptions;

		public FurnitureStoreManager()
		{
			_desciptions = Mst_DataManager.Instance.GetFurnitureText();
			ILookup<int, Mst_furniture> lookup = Mst_furniture.getSaleFurnitureList().ToLookup((Mst_furniture x) => x.Type);
			_StoreItems = new Dictionary<FurnitureKinds, List<FurnitureModel>>();
			foreach (IGrouping<int, Mst_furniture> item in lookup)
			{
				FurnitureKinds key = (FurnitureKinds)item.Key;
				List<Mst_furniture> list = item.ToList();
				List<FurnitureModel> list2 = list.ConvertAll((Converter<Mst_furniture, FurnitureModel>)((Mst_furniture mst) => new __FStoreItemModel__(mst, _desciptions[mst.Id])));
				_StoreItems[key] = list2.FindAll((FurnitureModel item) => !item.IsPossession());
			}
		}

		public int GetStoreItemCount(FurnitureKinds kind)
		{
			return _StoreItems[kind].Count;
		}

		public FurnitureModel[] GetStoreItem(FurnitureKinds kind)
		{
			return _StoreItems[kind].ToArray();
		}

		public int GetWorkerCount()
		{
			return new UseitemUtil().GetCount(52);
		}

		public bool IsValidExchange(FurnitureModel store_item)
		{
			if (!store_item.IsSalled())
			{
				return false;
			}
			if (store_item.IsPossession())
			{
				return false;
			}
			if (base.UserInfo.FCoin < store_item.Price)
			{
				return false;
			}
			if (store_item.IsNeedWorker() && GetWorkerCount() < 1)
			{
				return false;
			}
			return true;
		}

		public bool Exchange(FurnitureModel model)
		{
			Api_Result<object> api_Result = new Api_req_furniture().Buy(model.MstId);
			if (api_Result.state != 0)
			{
				return false;
			}
			_StoreItems[model.Type] = _StoreItems[model.Type].FindAll((FurnitureModel item) => !item.IsPossession());
			return true;
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += $"\n";
			return str + $"\"家具職人\"所有数:{GetWorkerCount()} \"家具コイン\"所有数:{base.UserInfo.FCoin}\n";
		}
	}
}
