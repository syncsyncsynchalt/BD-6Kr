using Common.Enum;
using local.models;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class InteriorManager : ManagerBase
	{
		private int _deck_id;

		private Dictionary<FurnitureKinds, List<FurnitureModel>> _dict;

		private Dictionary<FurnitureKinds, FurnitureModel> _room_cache;

		public DeckModel Deck => base.UserInfo.GetDeck(_deck_id);

		public InteriorManager(int deck_id)
		{
			_deck_id = deck_id;
			Dictionary<int, string> desciptions = Mst_DataManager.Instance.GetFurnitureText();
			_dict = new Dictionary<FurnitureKinds, List<FurnitureModel>>();
			Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> api_Result = new Api_get_Member().FurnitureList();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				foreach (KeyValuePair<FurnitureKinds, List<Mst_furniture>> datum in api_Result.data)
				{
					List<FurnitureModel> value = datum.Value.ConvertAll((Mst_furniture f) => new FurnitureModel(f, desciptions[f.Id]));
					_dict.Add(datum.Key, value);
				}
			}
		}

		public FurnitureModel[] GetFurnitures(FurnitureKinds kind)
		{
			return _dict[kind].ToArray();
		}

		public FurnitureModel GetFurniture(FurnitureKinds kind, int furniture_mst_id)
		{
			return _dict[kind].Find((FurnitureModel f) => f.MstId == furniture_mst_id);
		}

		public Dictionary<FurnitureKinds, FurnitureModel> GetRoomInfo()
		{
			if (_room_cache == null && Comm_UserDatas.Instance.User_room.TryGetValue(_deck_id, out Mem_room value))
			{
				_room_cache = new Dictionary<FurnitureKinds, FurnitureModel>();
				foreach (KeyValuePair<FurnitureKinds, List<FurnitureModel>> item in _dict)
				{
					int room_furniture = -1;
					switch (item.Key)
					{
					case FurnitureKinds.Floor:
						room_furniture = value.Furniture1;
						break;
					case FurnitureKinds.Wall:
						room_furniture = value.Furniture2;
						break;
					case FurnitureKinds.Window:
						room_furniture = value.Furniture3;
						break;
					case FurnitureKinds.Hangings:
						room_furniture = value.Furniture4;
						break;
					case FurnitureKinds.Chest:
						room_furniture = value.Furniture5;
						break;
					case FurnitureKinds.Desk:
						room_furniture = value.Furniture6;
						break;
					}
					_room_cache[item.Key] = item.Value.Find((FurnitureModel f) => f.MstId == room_furniture);
				}
			}
			return _room_cache;
		}

		public bool ChangeRoom(FurnitureKinds fType, int furniture_mst_id)
		{
			Api_Result<int> api_Result = new Api_req_furniture().Change(_deck_id, fType, furniture_mst_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				_room_cache = null;
				return true;
			}
			return false;
		}
	}
}
