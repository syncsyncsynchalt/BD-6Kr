using Common.Enum;
using Server_Common;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_store
	{
		private Dictionary<int, Mst_payitem> mst_payitem;

		public Api_req_store()
		{
			mst_payitem = Mst_DataManager.Instance.GetPayitem();
		}

		public Dictionary<int, Mst_payitem> GetStoreList()
		{
			return mst_payitem.ToDictionary((KeyValuePair<int, Mst_payitem> x) => x.Key, (KeyValuePair<int, Mst_payitem> y) => y.Value);
		}

		public bool IsBuy(int mst_id, int buyNum)
		{
			if (buyNum == 0)
			{
				return false;
			}
			if (Comm_UserDatas.Instance.User_basic.Strategy_point < mst_payitem[mst_id].Price * buyNum)
			{
				return false;
			}
			int buyNum2 = mst_payitem[mst_id].GetBuyNum();
			switch (buyNum2)
			{
			case -1:
				return true;
			default:
				if (buyNum2 >= buyNum)
				{
					return true;
				}
				goto case 0;
			case 0:
				return false;
			}
		}

		public Api_Result<object> Buy(int mst_id, int buyNum)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			Mst_payitem value = null;
			if (!mst_payitem.TryGetValue(mst_id, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			foreach (PayItemEffectInfo item in value.Items)
			{
				if (item.Type == 1)
				{
					Comm_UserDatas.Instance.Add_Useitem(item.MstId, item.Count);
				}
				else if (item.Type == 2)
				{
					IEnumerable<int> source = Enumerable.Repeat(item.MstId, item.Count);
					Comm_UserDatas.Instance.Add_Slot(source.ToList());
				}
				else if (item.Type == 3)
				{
					enumMaterialCategory mstId = (enumMaterialCategory)item.MstId;
					Comm_UserDatas.Instance.User_material[mstId].Add_Material(item.Count);
				}
			}
			Comm_UserDatas.Instance.User_basic.SubPoint(value.Price * buyNum);
			return api_Result;
		}
	}
}
