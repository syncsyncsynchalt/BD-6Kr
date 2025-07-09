using Common.Enum;
using Common.Struct;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class UserInfoModel
	{
		private Mem_basic _basic;

		private Dictionary<int, ShipModel> _ships;

		private Dictionary<int, DeckModel> _decks;

		private Dictionary<int, __EscortDeckModel__> _escort_decks;

		private TutorialModel _tutorialFlgs;

		public DifficultKind Difficulty => _basic.Difficult;

		public string Name => _basic.Nickname;

		public int Level => _basic.UserLevel();

		public int Rank => _basic.UserRank();

		public int SPoint => _basic.Strategy_point;

		public int FCoin => _basic.Fcoin;

		public int DeckCount => _decks.Count;

		public int MaxDutyExecuteCount => _basic.Max_quest;

		public int[] DeckIDs
		{
			get
			{
				int[] array = new int[_decks.Keys.Count];
				_decks.Keys.CopyTo(array, 0);
				return array;
			}
		}

		public TutorialModel Tutorial => _tutorialFlgs;

		public int StartMapCount => Comm_UserDatas.Instance.User_trophy.Start_map_count;

		public int WinSCount => Comm_UserDatas.Instance.User_trophy.Win_S_count;

		public int RevampCount => Comm_UserDatas.Instance.User_trophy.Revamp_count;

		public UserInfoModel()
		{
			_basic = null;
			_ships = null;
			_decks = null;
			_escort_decks = null;
			_tutorialFlgs = new TutorialModel();
			_Init();
		}

		public int GetMaterialMaxNum()
		{
			return _basic.GetMaterialMaxNum();
		}

		public List<ShipModel> __GetShipList__()
		{
			List<ShipModel> list = new List<ShipModel>();
			foreach (ShipModel value in _ships.Values)
			{
				list.Add(value);
			}
			return list;
		}

		public DeckModel GetDeck(int deck_id)
		{
			if (_decks.ContainsKey(deck_id))
			{
				return _decks[deck_id];
			}
			return null;
		}

		public DeckModel GetDeckByShipMemId(int ship_mem_id)
		{
			foreach (DeckModel value in _decks.Values)
			{
				if (value.HasShipMemId(ship_mem_id))
				{
					return value;
				}
			}
			return null;
		}

		public DeckModel[] GetDecks()
		{
			List<DeckModel> list = new List<DeckModel>();
			list.AddRange(_decks.Values);
			return list.ToArray();
		}

		public DeckModel[] GetDecksFromArea(int area_id)
		{
			List<DeckModel> list = new List<DeckModel>();
			list.AddRange(_decks.Values);
			return list.FindAll((DeckModel deck) => deck.AreaId == area_id).ToArray();
		}

		public List<int> __GetShipMemIdInDecks__()
		{
			List<int> list = new List<int>();
			foreach (DeckModel value in _decks.Values)
			{
				list.AddRange(value.__GetShipMemIds__());
			}
			return list;
		}

		public EscortDeckModel GetEscortDeck(int area_id)
		{
			if (_escort_decks.ContainsKey(area_id))
			{
				return _escort_decks[area_id];
			}
			return null;
		}

		public EscortDeckModel GetEscortDeckByShipMemId(int ship_mem_id)
		{
			foreach (__EscortDeckModel__ value in _escort_decks.Values)
			{
				if (value.HasShipMemId(ship_mem_id))
				{
					return value;
				}
			}
			return null;
		}

		public List<int> __GetShipMemIdInEscortDecks__()
		{
			List<int> list = new List<int>();
			foreach (__EscortDeckModel__ value in _escort_decks.Values)
			{
				list.AddRange(value.__GetShipMemIds__());
			}
			return list;
		}

		public List<int> __GetShipMemIdInAllDecks__()
		{
			List<int> list = __GetShipMemIdInDecks__();
			list.AddRange(__GetShipMemIdInEscortDecks__());
			return list;
		}

		public HashSet<int> __GetShipMemIdHashInBothDeck__()
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (DeckModel value in _decks.Values)
			{
				ShipModel[] ships = value.GetShips();
				for (int i = 0; i < ships.Length; i++)
				{
					if (ships[i] != null)
					{
						hashSet.Add(ships[i].MemId);
					}
				}
			}
			foreach (__EscortDeckModel__ value2 in _escort_decks.Values)
			{
				ShipModel[] ships = value2.GetShips();
				for (int j = 0; j < ships.Length; j++)
				{
					if (ships[j] != null)
					{
						hashSet.Add(ships[j].MemId);
					}
				}
			}
			return hashSet;
		}

		public TemporaryEscortDeckModel __CreateEscortDeckClone__(int area_id)
		{
			return ((__EscortDeckModel__)GetEscortDeck(area_id))?.GetCloneDeck(_ships);
		}

		public ShipModel GetShip(int ship_mem_id)
		{
			_ships.TryGetValue(ship_mem_id, out ShipModel value);
			return value;
		}

		public ShipModel GetFlagShip(int deck_id)
		{
			DeckModel deck = GetDeck(deck_id);
			return deck.GetFlagShip();
		}

		public int GetDeckID(int ship_mem_id)
		{
			return GetDeckByShipMemId(ship_mem_id)?.Id ?? (-1);
		}

		public int GetEscortDeckId(int ship_mem_id)
		{
			return GetEscortDeckByShipMemId(ship_mem_id)?.Id ?? (-1);
		}

		public MemberMaxInfo ShipCountData()
		{
			return local.utils.Utils.ShipCountData();
		}

		public MemberMaxInfo SlotitemCountData()
		{
			return local.utils.Utils.SlotitemCountData();
		}

		public int GetPortBGMId(int deck_id)
		{
			if (Comm_UserDatas.Instance.User_room.TryGetValue(deck_id, out Mem_room value))
			{
				return value.Bgm_id;
			}
			return 0;
		}

		private void _Init()
		{
			_decks = new Dictionary<int, DeckModel>();
			_escort_decks = new Dictionary<int, __EscortDeckModel__>();
			Api_get_Member api_get_Member = new Api_get_Member();
			Api_Result<Mem_basic> api_Result = api_get_Member.Basic();
			if (api_Result.state == Api_Result_State.Success)
			{
				_basic = api_Result.data;
				_tutorialFlgs.__Update__(_basic);
			}
			__UpdateShips__(api_get_Member);
			__UpdateDeck__(api_get_Member);
			__UpdateEscortDeck__(api_get_Member);
		}

		public void __UpdateShips__()
		{
			__UpdateShips__(new Api_get_Member());
		}

		public void __UpdateShips__(Api_get_Member api_get_mem)
		{
			_ships = new Dictionary<int, ShipModel>();
			Api_Result<Dictionary<int, Mem_ship>> api_Result = api_get_mem.Ship(null);
			if (api_Result.state == Api_Result_State.Success)
			{
				foreach (Mem_ship value in api_Result.data.Values)
				{
					_ships.Add(value.Rid, new ShipModel(value));
				}
			}
			__UpdateDeck__(api_get_mem);
		}

		public void __RemoveGekichinShips__(List<int> ids)
		{
			if (_ships == null)
			{
				_ships = new Dictionary<int, ShipModel>();
			}
			if (ids == null)
			{
				return;
			}
			for (int i = 0; i < ids.Count; i++)
			{
				int key = ids[i];
				if (_ships.ContainsKey(key))
				{
					_ships.Remove(key);
				}
			}
		}

		public void __UpdateDeck__()
		{
			__UpdateDeck__(new Api_get_Member());
		}

		public void __UpdateDeck__(Api_get_Member api_get_mem)
		{
			Api_Result<Dictionary<int, Mem_deck>> api_Result = api_get_mem.Deck();
			if (api_Result.state == Api_Result_State.Success)
			{
				foreach (int key in api_Result.data.Keys)
				{
					Mem_deck mem_deck = api_Result.data[key];
					if (_decks.TryGetValue(key, out DeckModel value))
					{
						value.__Update__(mem_deck, _ships);
					}
					else
					{
						_decks[key] = new DeckModel(mem_deck, _ships);
					}
				}
			}
		}

		public void __UpdateEscortDeck__(Api_get_Member api_get_mem)
		{
			Api_Result<Dictionary<int, Mem_esccort_deck>> api_Result = api_get_mem.EscortDeck();
			if (api_Result.state == Api_Result_State.Success)
			{
				foreach (int key in api_Result.data.Keys)
				{
					Mem_esccort_deck mem_escort_deck = api_Result.data[key];
					if (_escort_decks.TryGetValue(key, out __EscortDeckModel__ value))
					{
						value.__Update__(mem_escort_deck, _ships);
					}
					else
					{
						_escort_decks[key] = new __EscortDeckModel__(mem_escort_deck, _ships);
					}
				}
			}
		}

		public void __UpdateTemporaryEscortDeck__(TemporaryEscortDeckModel deck)
		{
			deck.__Update__(_ships);
		}

		public override string ToString()
		{
			return $"提督名:{Name}  提督レベル:{Level}  保有艦隊数:{DeckCount}  ゲーム難易度:{Difficulty}  所有戦略P:{SPoint} Tutorial:({Tutorial})";
		}
	}
}
