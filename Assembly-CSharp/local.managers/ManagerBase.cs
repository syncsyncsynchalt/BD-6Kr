using Common.Struct;
using local.models;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class ManagerBase
	{
		protected static TurnState _turn_state = TurnState.CONTINOUS;

		protected static Dictionary<int, MapAreaModel> _area;

		protected static MaterialModel _materialModel;

		protected static UserInfoModel _userInfoModel;

		protected static SettingModel _settingModel;

		protected _TankerManager _tanker_manager;

		public int Turn => Comm_UserDatas.Instance.User_turn.Total_turn;

		public DateTime Datetime => Comm_UserDatas.Instance.User_turn.GetDateTime();

		public TurnString DatetimeString => Comm_UserDatas.Instance.User_turn.GetTurnString();

		public TurnState TurnState => _turn_state;

		public MaterialModel Material => _materialModel;

		public UserInfoModel UserInfo => _userInfoModel;

		public SettingModel Settings => _settingModel;

		public ManagerBase()
		{
			if (_area == null)
			{
				_area = new Dictionary<int, MapAreaModel>();
			}
		}

		public static bool IsInitialized()
		{
			return _materialModel != null && _userInfoModel != null && _settingModel != null;
		}

		public static DeckModelBase getDeck(int ship_mem_id)
		{
			DeckModelBase deckModelBase = _userInfoModel.GetDeckByShipMemId(ship_mem_id);
			if (deckModelBase == null)
			{
				deckModelBase = _userInfoModel.GetEscortDeckByShipMemId(ship_mem_id);
			}
			return deckModelBase;
		}

		public static int IsInEscortDeck(int ship_mem_id)
		{
			return _userInfoModel.GetEscortDeckId(ship_mem_id);
		}

		public static void initialize()
		{
			_materialModel = new MaterialModel();
			_userInfoModel = new UserInfoModel();
			_settingModel = new SettingModel();
			_materialModel.Update();
		}

		public List<ShipModel> GetAreaShips(int area_id, List<ShipModel> all_ships)
		{
			return GetAreaShips(area_id, use_deck: true, use_edeck: true, all_ships);
		}

		public List<ShipModel> GetAreaShips(int area_id, bool use_deck, bool use_edeck, List<ShipModel> all_ships)
		{
			List<ShipModel> list = new List<ShipModel>();
			MapAreaModel mapAreaModel = _area[area_id];
			if (use_deck)
			{
				DeckModel[] decks = mapAreaModel.GetDecks();
				DeckModel[] array = decks;
				foreach (DeckModel deckModel in array)
				{
					list.AddRange(deckModel.GetShips());
				}
			}
			if (use_edeck)
			{
				list.AddRange(mapAreaModel.GetEscortDeck().GetShips());
			}
			HashSet<int> hashSet = mapAreaModel.__GetRepairingShipMemIdsHash__();
			foreach (int item in hashSet)
			{
				ShipModel ship2 = UserInfo.GetShip(item);
				if (ship2.IsInDeck() == -1 && ship2.IsInEscortDeck() == -1)
				{
					list.Add(ship2);
				}
			}
			if (all_ships == null)
			{
				all_ships = UserInfo.__GetShipList__();
			}
			List<ShipModel> collection = all_ships.FindAll((ShipModel ship) => ship.IsBlingWait() && ship.AreaIdBeforeBlingWait == area_id);
			list.AddRange(collection);
			return list;
		}

		public List<ShipModel> GetDepotShips(List<ShipModel> all_ships)
		{
			HashSet<int> o_ship_memids = UserInfo.__GetShipMemIdHashInBothDeck__();
			HashSet<int> r_ship_memids = __GetNDockShipMemIdsHash__();
			if (all_ships == null)
			{
				all_ships = UserInfo.__GetShipList__();
			}
			return all_ships.FindAll((ShipModel ship) => !o_ship_memids.Contains(ship.MemId) && !r_ship_memids.Contains(ship.MemId) && !ship.IsBlingWait());
		}

		public HashSet<int> __GetNDockShipMemIdsHash__()
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (MapAreaModel value in _area.Values)
			{
				HashSet<int> hashSet2 = value.__GetRepairingShipMemIdsHash__();
				foreach (int item in hashSet2)
				{
					hashSet.Add(item);
				}
			}
			return hashSet;
		}

		protected void _UpdateTankerManager()
		{
			if (_tanker_manager == null)
			{
				_tanker_manager = new _TankerManager();
			}
			else
			{
				_tanker_manager.Update();
			}
		}

		protected void _CreateMapAreaModel()
		{
			_UpdateTankerManager();
			Dictionary<int, MapAreaModel> dictionary = new Dictionary<int, MapAreaModel>();
			foreach (int key in _area.Keys)
			{
				dictionary.Add(key, _area[key]);
			}
			_area.Clear();
			Api_get_Member api_get_Member = new Api_get_Member();
			Api_Result<Dictionary<int, User_StrategyMapFmt>> api_Result = api_get_Member.StrategyInfo();
			Dictionary<int, List<Mem_ndock>> data = api_get_Member.AreaNdock().data;
			if (api_Result.state == Api_Result_State.Success)
			{
				Dictionary<int, User_StrategyMapFmt> data2 = api_Result.data;
				foreach (int key2 in data2.Keys)
				{
					if (dictionary.ContainsKey(key2))
					{
						dictionary[key2].__Update__(UserInfo, data2[key2], data, _tanker_manager);
						_area.Add(key2, dictionary[key2]);
						dictionary.Remove(key2);
					}
					else
					{
						_area.Add(key2, new MapAreaModel(UserInfo, data2[key2], data, _tanker_manager));
					}
				}
			}
		}

		public override string ToString()
		{
			string str = $"{UserInfo}\n{Material} 所持家具コイン:{UserInfo.FCoin}\n";
			str += $"総ターン数:{Turn}\t日時:{Datetime}";
			string str2 = str;
			object[] array = new object[4];
			TurnString datetimeString = DatetimeString;
			array[0] = datetimeString.Year;
			TurnString datetimeString2 = DatetimeString;
			array[1] = datetimeString2.Month;
			TurnString datetimeString3 = DatetimeString;
			array[2] = datetimeString3.Day;
			TurnString datetimeString4 = DatetimeString;
			array[3] = datetimeString4.DayOfWeek;
			str = str2 + string.Format("({0}年{1} {2}日 {3})\n", array);
			return str + $"{Settings}";
		}
	}
}
