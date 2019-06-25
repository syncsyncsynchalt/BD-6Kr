using Common.Enum;
using Server_Common;
using Server_Models;

namespace Server_Controllers
{
	public class Api_req_furniture
	{
		public Api_Result<int> Change(int deck_rid, FurnitureKinds furnitureKind, int furnitureId)
		{
			Api_Result<int> api_Result = new Api_Result<int>();
			Mem_room value = null;
			if (!Comm_UserDatas.Instance.User_room.TryGetValue(deck_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int season = Mst_DataManager.Instance.Mst_furniture[furnitureId].Season;
			int value2 = 0;
			Mst_DataManager.Instance.Mst_bgm_season.TryGetValue(season, out value2);
			if (value2 == 0)
			{
				int key = value[furnitureKind];
				int season2 = Mst_DataManager.Instance.Mst_furniture[key].Season;
				int value3 = 0;
				Mst_DataManager.Instance.Mst_bgm_season.TryGetValue(season2, out value3);
				if (value3 == value.Bgm_id)
				{
					value.SetFurniture(furnitureKind, furnitureId, value2);
				}
				else
				{
					value.SetFurniture(furnitureKind, furnitureId);
				}
			}
			else
			{
				value.SetFurniture(furnitureKind, furnitureId, value2);
			}
			api_Result.data = 1;
			return api_Result;
		}

		public Api_Result<object> Buy(int mst_id)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			Mst_furniture value = null;
			if (!Mst_DataManager.Instance.Mst_furniture.TryGetValue(mst_id, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (Comm_UserDatas.Instance.User_furniture.ContainsKey(mst_id))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
			if (value.Saleflg == 0 || user_basic.Fcoin < value.Price)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_useitem value2 = null;
			if (value.IsRequireWorker())
			{
				if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(52, out value2))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				if (value2.Value <= 0)
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			if (!Comm_UserDatas.Instance.Add_Furniture(mst_id))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			user_basic.SubCoin(value.Price);
			value2?.Sub_UseItem(1);
			return api_Result;
		}

		public Api_Result<bool> BuyMusic(int deck_rid, int music_id)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Mst_bgm_jukebox jukeBoxItem = Mst_DataManager.Instance.GetJukeBoxItem(music_id);
			if (jukeBoxItem == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (Comm_UserDatas.Instance.User_basic.Fcoin < jukeBoxItem.R_coins)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Comm_UserDatas.Instance.User_basic.SubCoin(jukeBoxItem.R_coins);
			api_Result.state = Api_Result_State.Success;
			api_Result.data = true;
			return api_Result;
		}

		public Api_Result<bool> SetPortBGMFromJukeBoxList(int deck_rid, int music_id)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Mst_bgm_jukebox jukeBoxItem = Mst_DataManager.Instance.GetJukeBoxItem(music_id);
			if (jukeBoxItem == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (jukeBoxItem.Bgm_flag == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = true;
			Mem_room value = null;
			if (!Comm_UserDatas.Instance.User_room.TryGetValue(deck_rid, out value))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			value.SetPortBgmFromJuke(music_id);
			api_Result.state = Api_Result_State.Success;
			api_Result.data = true;
			return api_Result;
		}
	}
}
