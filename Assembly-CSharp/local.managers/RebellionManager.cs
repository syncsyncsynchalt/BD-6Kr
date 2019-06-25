using Common.Enum;
using local.models;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class RebellionManager : ManagerBase
	{
		private int _area_id;

		private MapModel _map;

		private Api_req_Mission _req_mission;

		private List<DeckModel> _decks;

		public MapAreaModel MapArea => ManagerBase._area[_area_id];

		public MapModel Map => _map;

		public List<DeckModel> Decks => _decks;

		public RebellionManager(int area_id)
		{
			_area_id = area_id;
			int key = _area_id * 10 + 7;
			Mst_DataManager.Instance.Mst_mapinfo.TryGetValue(key, out Mst_mapinfo value);
			_map = new MapModel(value, null);
			_req_mission = new Api_req_Mission();
			_decks = new List<DeckModel>();
			DeckModel[] decksFromArea = base.UserInfo.GetDecksFromArea(MapArea.Id);
			for (int i = 0; i < decksFromArea.Length; i++)
			{
				if (decksFromArea[i].Count > 0)
				{
					_decks.Add(decksFromArea[i]);
				}
			}
		}

		public List<IsGoCondition> IsValidMissionSub(int deck_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidMission();
			HashSet<IsGoCondition> collection = _req_mission.ValidStart(deck_id, -1, 0);
			List<IsGoCondition> list2 = new List<IsGoCondition>(collection);
			for (int i = 0; i < list2.Count; i++)
			{
				if (!list.Contains(list2[i]))
				{
					list.Add(list2[i]);
				}
			}
			list2.Remove(IsGoCondition.Deck1);
			list2.Sort();
			return list2;
		}

		public List<IsGoCondition> IsValid_MissionMain(int deck_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidMission();
			HashSet<IsGoCondition> collection = _req_mission.ValidStart(deck_id, -2, 0);
			List<IsGoCondition> list2 = new List<IsGoCondition>(collection);
			for (int i = 0; i < list2.Count; i++)
			{
				if (!list.Contains(list2[i]))
				{
					list.Add(list2[i]);
				}
			}
			list2.Remove(IsGoCondition.Deck1);
			list2.Sort();
			return list2;
		}

		public bool IsGoRebellion(int sub_deck_id, int main_deck_id, int sub_support_deck_id, int main_support_deck_id)
		{
			if (MapArea.RState != RebellionState.Invation)
			{
				return false;
			}
			DeckModel deckModel = _decks.Find((DeckModel deck) => deck.Id == main_deck_id);
			if (deckModel == null)
			{
				return false;
			}
			if (main_deck_id == sub_deck_id || main_deck_id == sub_support_deck_id || main_deck_id == main_support_deck_id)
			{
				return false;
			}
			if (sub_deck_id != -1)
			{
				if (sub_deck_id == sub_support_deck_id || sub_deck_id == main_support_deck_id)
				{
					return false;
				}
				DeckModel deckModel2 = _decks.Find((DeckModel deck) => deck.Id == sub_deck_id);
				if (deckModel2 == null)
				{
					return false;
				}
			}
			if (sub_support_deck_id != -1)
			{
				if (sub_support_deck_id == main_support_deck_id)
				{
					return false;
				}
				DeckModel deckModel3 = _decks.Find((DeckModel deck) => deck.Id == sub_support_deck_id);
				if (deckModel3 == null)
				{
					return false;
				}
				if (IsValidMissionSub(sub_support_deck_id).Count > 0)
				{
					return false;
				}
			}
			if (main_support_deck_id != -1)
			{
				DeckModel deckModel4 = _decks.Find((DeckModel deck) => deck.Id == main_support_deck_id);
				if (deckModel4 == null)
				{
					return false;
				}
				if (IsValid_MissionMain(main_support_deck_id).Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		public RebellionMapManager GoRebellion(int sub_deck_id, int main_deck_id, int sub_support_deck_id, int main_support_deck_id)
		{
			if (sub_support_deck_id != -1)
			{
				Api_Result<Mem_deck> api_Result = _req_mission.Start(sub_support_deck_id, -1, 0);
				if (api_Result.state != 0)
				{
					return null;
				}
			}
			if (main_support_deck_id != -1)
			{
				Api_Result<Mem_deck> api_Result2 = _req_mission.Start(main_support_deck_id, -2, 0);
				if (api_Result2.state != 0)
				{
					return null;
				}
			}
			DeckModel mainDeck = _decks.Find((DeckModel deck) => deck.Id == main_deck_id);
			DeckModel subDeck = _decks.Find((DeckModel deck) => deck.Id == sub_deck_id);
			return new RebellionMapManager(_map, mainDeck, subDeck);
		}

		public void NotGoRebellion()
		{
			new Api_req_StrategeMap().ExecuteRebellionLostArea(MapArea.Id);
			base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
			_CreateMapAreaModel();
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
