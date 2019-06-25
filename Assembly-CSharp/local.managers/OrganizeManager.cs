using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace local.managers
{
	public class OrganizeManager : ManagerBase, IOrganizeManager
	{
		private int _area_id;

		private Api_req_Hensei _api;

		private List<ShipModel> _ships;

		private SortKey _pre_sort_key;

		public MapAreaModel MapArea => ManagerBase._area[_area_id];

		public int ShipsCount => _ships.Count;

		public SortKey NowSortKey => _pre_sort_key;

		public OrganizeManager(int area_id)
		{
			_area_id = area_id;
			_api = new Api_req_Hensei();
			_CreateMapAreaModel();
			_UpdateShipList();
		}

		public int GetMamiyaCount()
		{
			return new UseitemUtil().GetCount(54);
		}

		public int GetIrakoCount()
		{
			return new UseitemUtil().GetCount(59);
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (_pre_sort_key != new_sort_key)
			{
				_pre_sort_key = new_sort_key;
				_ships = DeckUtil.GetSortedList(_ships, _pre_sort_key);
				return true;
			}
			return false;
		}

		public ShipModel[] GetShipList()
		{
			return _ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			if (page_no < 1 || ShipsCount / count_in_page + 1 < page_no)
			{
				return new ShipModel[0];
			}
			int val = (page_no - 1) * count_in_page;
			val = Math.Max(val, 0);
			val = Math.Min(val, _ships.Count);
			int val2 = _ships.Count - val;
			val2 = Math.Max(val2, 0);
			val2 = Math.Min(val2, count_in_page);
			return _ships.GetRange(val, val2).ToArray();
		}

		public bool IsValidShip(int ship_mem_id)
		{
			ShipModel ship = base.UserInfo.GetShip(ship_mem_id);
			if (ship.IsBling())
			{
				return false;
			}
			if (ship.IsInMission())
			{
				return false;
			}
			DeckModelBase deck = ship.getDeck();
			if (deck != null)
			{
				if (deck.IsEscortDeckMyself())
				{
					return false;
				}
				if (deck.IsActionEnd())
				{
					return false;
				}
			}
			return true;
		}

		public bool IsValidChange(int deck_id, int selected_index, int ship_mem_id)
		{
			return _api.IsValidChange(deck_id, selected_index, ship_mem_id);
		}

		public bool ChangeOrganize(int deck_id, int selected_index, int ship_mem_id)
		{
			Api_Result<Hashtable> api_Result = _api.Change(deck_id, selected_index, ship_mem_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				Api_get_Member api_get_mem = new Api_get_Member();
				base.UserInfo.__UpdateDeck__(api_get_mem);
				_UpdateShipList();
				return true;
			}
			return false;
		}

		public bool IsValidUnset(int ship_mem_id)
		{
			int num = DeckUtil.__IsInDeck__(ship_mem_id, checkPartnerShip: false);
			if (num == -1)
			{
				return false;
			}
			int shipIndex = base.UserInfo.GetDeck(num).GetShipIndex(ship_mem_id);
			return _api.IsValidChange(num, shipIndex, -1);
		}

		public bool UnsetOrganize(int deck_id, int selected_index)
		{
			Api_Result<Hashtable> api_Result = _api.Change(deck_id, selected_index, -1);
			if (api_Result.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateDeck__(new Api_get_Member());
				_UpdateShipList();
				return true;
			}
			return false;
		}

		public bool IsValidUnsetAll(int deck_id)
		{
			return _api.IsValidChange(deck_id, 0, -2);
		}

		public bool UnsetAllOrganize(int deck_id)
		{
			Api_Result<Hashtable> api_Result = _api.Change(deck_id, 0, -2);
			if (api_Result.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateDeck__(new Api_get_Member());
				_UpdateShipList();
				return true;
			}
			return false;
		}

		public string ChangeDeckName(int deck_id, string new_deck_name)
		{
			string name = base.UserInfo.GetDeck(deck_id).Name;
			Api_Result<Hashtable> api_Result = new Api_req_Member().Update_DeckName(deck_id, new_deck_name);
			if (api_Result.state == Api_Result_State.Success)
			{
				return new_deck_name;
			}
			return name;
		}

		public bool Lock(int ship_mem_id)
		{
			Api_Result<Mem_ship> api_Result = _api.Lock(ship_mem_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool IsValidUseSweets(int deck_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			if (deck == null)
			{
				return false;
			}
			if (deck.Count == 0)
			{
				return false;
			}
			if (deck.MissionState != 0)
			{
				return false;
			}
			Dictionary<SweetsType, bool> availableSweets = GetAvailableSweets(deck_id);
			return availableSweets.ContainsValue(value: true);
		}

		public Dictionary<SweetsType, bool> GetAvailableSweets(int deck_id)
		{
			Dictionary<SweetsType, bool> dictionary = new Dictionary<SweetsType, bool>();
			bool[] array = new Api_req_Member().itemuse_cond_check(deck_id);
			dictionary.Add(SweetsType.Mamiya, array[0]);
			dictionary.Add(SweetsType.Irako, array[1]);
			dictionary.Add(SweetsType.Both, dictionary[SweetsType.Irako] && GetMamiyaCount() > 0);
			return dictionary;
		}

		public bool UseSweets(int deck_id, SweetsType type)
		{
			if (!IsValidUseSweets(deck_id))
			{
				return false;
			}
			HashSet<int> useitem_id;
			if (type == SweetsType.Both)
			{
				HashSet<int> hashSet = new HashSet<int>();
				hashSet.Add(54);
				hashSet.Add(59);
				useitem_id = hashSet;
			}
			else
			{
				HashSet<int> hashSet = new HashSet<int>();
				hashSet.Add((int)type);
				useitem_id = hashSet;
			}
			Api_Result<bool> api_Result = new Api_req_Member().itemuse_cond(deck_id, useitem_id);
			return api_Result.state == Api_Result_State.Success;
		}

		private void _UpdateShipList()
		{
			List<ShipModel> list = base.UserInfo.__GetShipList__();
			if (MapArea.Id == 1)
			{
				_ships = list;
			}
			else
			{
				_ships = GetAreaShips(MapArea.Id, list);
				_ships.AddRange(GetDepotShips(list));
			}
			_ships = DeckUtil.GetSortedList(_ships, _pre_sort_key);
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"{base.ToString()}\n";
			empty += $"海域:{MapArea}\n";
			empty += $"給糧艦(間宮)の所有数:{GetMamiyaCount()}";
			return empty + $"\t給糧艦(伊良湖)の所有数:{GetIrakoCount()}";
		}
	}
}
