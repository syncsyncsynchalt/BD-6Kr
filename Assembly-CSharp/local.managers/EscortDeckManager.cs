using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class EscortDeckManager : ManagerBase, IOrganizeManager
	{
		private int _area_id;

		private Api_req_Transport _api;

		private TemporaryEscortDeckModel _init_deck;

		private TemporaryEscortDeckModel _edit_deck;

		private List<ShipModel> _ships;

		private SortKey _pre_sort_key;

		public MapAreaModel MapArea => ManagerBase._area[_area_id];

		public EscortDeckModel EditDeck => _edit_deck;

		public int ShipsCount
		{
			get
			{
				if (_ships == null)
				{
					_UpdateShipList();
				}
				return _ships.Count;
			}
		}

		public SortKey NowSortKey => _pre_sort_key;

		public EscortDeckManager(int area_id)
		{
			_area_id = area_id;
			_api = new Api_req_Transport();
			_CreateMapAreaModel();
			_init_deck = base.UserInfo.__CreateEscortDeckClone__(area_id);
			_edit_deck = base.UserInfo.__CreateEscortDeckClone__(area_id);
		}

		public int GetMamiyaCount()
		{
			return new UseitemUtil().GetCount(54);
		}

		public int GetIrakoCount()
		{
			return new UseitemUtil().GetCount(59);
		}

		public void InitEscortOrganizer()
		{
			_api.initEscortGroup();
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
			if (_ships == null)
			{
				_UpdateShipList();
			}
			return _ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			if (page_no < 1 || ShipsCount / count_in_page + 1 < page_no)
			{
				return new ShipModel[0];
			}
			if (_ships == null)
			{
				_UpdateShipList();
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
			DeckModelBase deck = ship.getDeck();
			if (deck != null)
			{
				if (deck.AreaId != MapArea.Id)
				{
					return false;
				}
				if (!deck.IsEscortDeckMyself())
				{
					return false;
				}
			}
			return _edit_deck.HasShipMemId(ship_mem_id);
		}

		public bool IsValidChange(int deck_id, int selected_index, int ship_mem_id)
		{
			if (deck_id != _edit_deck.Id)
			{
				return false;
			}
			return IsValidChange(selected_index, ship_mem_id);
		}

		public bool IsValidChange(int selected_index, int ship_mem_id)
		{
			return _api.IsValidChange(_edit_deck.Id, selected_index, ship_mem_id, _edit_deck.DeckShips);
		}

		public bool ChangeOrganize(int deck_id, int selected_index, int ship_mem_id)
		{
			if (deck_id != _edit_deck.Id)
			{
				return false;
			}
			return ChangeOrganize(selected_index, ship_mem_id);
		}

		public bool ChangeOrganize(int selected_index, int ship_mem_id)
		{
			if (!_api.IsValidChange(_edit_deck.Id, selected_index, ship_mem_id, _edit_deck.DeckShips))
			{
				return false;
			}
			DeckShips deckShip = _edit_deck.DeckShips;
			if (_api.Change_TempDeck(selected_index, ship_mem_id, ref deckShip))
			{
				base.UserInfo.__UpdateTemporaryEscortDeck__(_edit_deck);
				return true;
			}
			return true;
		}

		public bool IsValidUnset(int ship_mem_id)
		{
			int deck_targetIdx = _edit_deck.DeckShips.Find(ship_mem_id);
			return _api.IsValidChange(_edit_deck.Id, deck_targetIdx, -1, _edit_deck.DeckShips);
		}

		public bool UnsetOrganize(int deck_id, int selected_index)
		{
			if (deck_id != _edit_deck.Id)
			{
				return false;
			}
			return UnsetOrganize(selected_index);
		}

		public bool UnsetOrganize(int selected_index)
		{
			if (!_api.IsValidChange(_edit_deck.Id, selected_index, -1, _edit_deck.DeckShips))
			{
				return false;
			}
			DeckShips deckShip = _edit_deck.DeckShips;
			if (_api.Change_TempDeck(selected_index, -1, ref deckShip))
			{
				base.UserInfo.__UpdateTemporaryEscortDeck__(_edit_deck);
				return true;
			}
			return false;
		}

		public bool IsValidUnsetAll(int deck_id)
		{
			if (deck_id != _edit_deck.Id)
			{
				return false;
			}
			return IsValidUnsetAll();
		}

		public bool IsValidUnsetAll()
		{
			return _api.IsValidChange(_edit_deck.Id, 0, -2, _edit_deck.DeckShips);
		}

		public bool UnsetAllOrganize(int deck_id)
		{
			if (deck_id != _edit_deck.Id)
			{
				return false;
			}
			return UnsetAllOrganize();
		}

		public bool UnsetAllOrganize()
		{
			DeckShips deckShip = _edit_deck.DeckShips;
			if (deckShip.Count() <= 1)
			{
				return false;
			}
			if (_api.Change_TempDeck(0, -2, ref deckShip))
			{
				base.UserInfo.__UpdateTemporaryEscortDeck__(_edit_deck);
				return true;
			}
			return false;
		}

		public string ChangeDeckName(int deck_id, string new_deck_name)
		{
			if (deck_id != _edit_deck.Id)
			{
				return _edit_deck.Name;
			}
			return ChangeDeckName(new_deck_name);
		}

		public string ChangeDeckName(string new_deck_name)
		{
			_edit_deck.ChangeName(new_deck_name);
			return _edit_deck.Name;
		}

		public bool Lock(int ship_mem_id)
		{
			Api_Result<Mem_ship> api_Result = new Api_req_Hensei().Lock(ship_mem_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool IsValidUseSweets(int deck_id)
		{
			if (deck_id != _edit_deck.Id)
			{
				return false;
			}
			return IsValidUseSweets();
		}

		public bool IsValidUseSweets()
		{
			return false;
		}

		public Dictionary<SweetsType, bool> GetAvailableSweets(int deck_id)
		{
			if (deck_id != _edit_deck.Id)
			{
				return null;
			}
			return GetAvailableSweets();
		}

		public Dictionary<SweetsType, bool> GetAvailableSweets()
		{
			Dictionary<SweetsType, bool> dictionary = new Dictionary<SweetsType, bool>();
			dictionary.Add(SweetsType.Mamiya, value: false);
			dictionary.Add(SweetsType.Irako, value: false);
			dictionary.Add(SweetsType.Both, value: false);
			return dictionary;
		}

		public bool UseSweets(int deck_id, SweetsType type)
		{
			if (deck_id != _edit_deck.Id)
			{
				return false;
			}
			return UseSweets(type);
		}

		public bool UseSweets(SweetsType type)
		{
			if (!IsValidUseSweets())
			{
				return false;
			}
			return false;
		}

		public bool HasChanged()
		{
			if (_init_deck.DeckShips.Count() != _edit_deck.DeckShips.Count())
			{
				return true;
			}
			for (int i = 0; i < _init_deck.DeckShips.Count(); i++)
			{
				if (_init_deck.DeckShips[i] != _edit_deck.DeckShips[i])
				{
					return true;
				}
			}
			if (_init_deck.__Name__ != _edit_deck.__Name__)
			{
				return true;
			}
			return false;
		}

		public bool __Commit__()
		{
			Api_Result<Mem_esccort_deck> api_Result = _api.Change(_edit_deck.Id, _edit_deck.DeckShips);
			if (api_Result.state == Api_Result_State.Success)
			{
				if (_init_deck.__Name__ != _edit_deck.__Name__)
				{
					_api.Update_DeckName(_edit_deck.Id, _edit_deck.__Name__);
				}
				base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
				_init_deck = base.UserInfo.__CreateEscortDeckClone__(_edit_deck.AreaId);
				return true;
			}
			return false;
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
			Dictionary<int, int> tmp = _api.Mst_escort_group;
			if (tmp != null)
			{
				_ships = _ships.FindAll((ShipModel ship) => tmp[ship.ShipType] == 1);
			}
			_ships = DeckUtil.GetSortedList(_ships, _pre_sort_key);
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"{base.ToString()}\n";
			empty += $"海域:{MapArea}\n";
			empty += $"初期状態:{_init_deck}\n";
			return empty + $"暫定編成:{_edit_deck}\n";
		}
	}
}
