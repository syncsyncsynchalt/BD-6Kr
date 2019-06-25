using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public class PracticeManager : ManagerBase
	{
		private Api_req_PracticeDeck _reqPrac;

		private DeckModel _current_deck;

		private Dictionary<DeckPracticeType, bool> _valid_deck_prac_type_dic;

		private List<DeckModel> _rival_decks;

		private Dictionary<int, List<IsGoCondition>> _validation_results;

		public DeckModel CurrentDeck => _current_deck;

		public Dictionary<DeckPracticeType, bool> DeckPracticeTypeDic => _valid_deck_prac_type_dic;

		public List<DeckModel> RivalDecks => _rival_decks;

		public MapAreaModel MapArea => ManagerBase._area[_current_deck.AreaId];

		public PracticeManager(int deck_id)
		{
			_current_deck = base.UserInfo.GetDeck(deck_id);
			_CreateMapAreaModel();
			_reqPrac = new Api_req_PracticeDeck();
			_valid_deck_prac_type_dic = new Dictionary<DeckPracticeType, bool>();
			Api_Result<Mem_deckpractice> api_Result = new Api_get_Member().DeckPractice();
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_deckpractice data = api_Result.data;
				_InitValidDeckPracType(DeckPracticeType.Normal, data, ref _valid_deck_prac_type_dic);
				_InitValidDeckPracType(DeckPracticeType.Hou, data, ref _valid_deck_prac_type_dic);
				_InitValidDeckPracType(DeckPracticeType.Rai, data, ref _valid_deck_prac_type_dic);
				_InitValidDeckPracType(DeckPracticeType.Taisen, data, ref _valid_deck_prac_type_dic);
				_InitValidDeckPracType(DeckPracticeType.Kouku, data, ref _valid_deck_prac_type_dic);
				_InitValidDeckPracType(DeckPracticeType.Sougou, data, ref _valid_deck_prac_type_dic);
			}
			DeckModel[] decks = MapArea.GetDecks();
			_rival_decks = new List<DeckModel>();
			_validation_results = new Dictionary<int, List<IsGoCondition>>();
			foreach (DeckModel deckModel in decks)
			{
				if (deckModel.Count != 0 && deckModel.Id != _current_deck.Id)
				{
					_rival_decks.Add(deckModel);
					_validation_results.Add(deckModel.Id, deckModel.IsValidPractice());
				}
			}
			_validation_results.Add(_current_deck.Id, _current_deck.IsValidPractice());
		}

		public List<IsGoCondition> IsValidPractice(int deck_id)
		{
			if (_validation_results.TryGetValue(deck_id, out List<IsGoCondition> value))
			{
				return value;
			}
			List<IsGoCondition> list = new List<IsGoCondition>();
			list.Add(IsGoCondition.Invalid);
			return list;
		}

		public bool IsValidDeckPractice()
		{
			return IsValidPractice(_current_deck.Id).Count == 0;
		}

		public DeckPracticeResultModel StartDeckPractice(DeckPracticeType type)
		{
			Dictionary<int, int> dic = new Dictionary<int, int>();
			CurrentDeck.__CreateShipExpRatesDictionary__(ref dic);
			Api_Result<PracticeDeckResultFmt> resultData = _reqPrac.GetResultData(type, CurrentDeck.Id);
			if (resultData.state != 0)
			{
				return null;
			}
			return new DeckPracticeResultModel(type, resultData.data, base.UserInfo, dic);
		}

		public bool IsValidBattlePractice()
		{
			List<IsGoCondition> list = IsValidPractice(_current_deck.Id);
			if (list.Count > 0)
			{
				return false;
			}
			for (int i = 0; i < _rival_decks.Count; i++)
			{
				DeckModel deckModel = _rival_decks[i];
				if (IsValidPractice(deckModel.Id).Count == 0)
				{
					return true;
				}
			}
			return false;
		}

		public PracticeBattleManager StartBattlePractice(int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			if (!IsValidBattlePractice())
			{
				return null;
			}
			if (_rival_decks.Find((DeckModel d) => d.Id == enemy_deck_id) == null)
			{
				return null;
			}
			PracticeBattleManager practiceBattleManager = new PracticeBattleManager();
			practiceBattleManager.__Init__(_current_deck.Id, enemy_deck_id, formation_id);
			return practiceBattleManager;
		}

		public PracticeBattleManager StartBattlePractice_Write(int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			if (!IsValidBattlePractice())
			{
				return null;
			}
			if (_rival_decks.Find((DeckModel d) => d.Id == enemy_deck_id) == null)
			{
				return null;
			}
			PracticeBattleManager practiceBattleManager = new PracticeBattleManager_Write();
			practiceBattleManager.__Init__(_current_deck.Id, enemy_deck_id, formation_id);
			return practiceBattleManager;
		}

		public PracticeBattleManager StartBattlePractice_Read(int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			return new PracticeBattleManager_Read(_current_deck.Id, enemy_deck_id, formation_id);
		}

		private void _InitValidDeckPracType(DeckPracticeType type, Mem_deckpractice mem_dp, ref Dictionary<DeckPracticeType, bool> list)
		{
			if (mem_dp[type])
			{
				_valid_deck_prac_type_dic[type] = _reqPrac.PrackticeDeckCheck(type, CurrentDeck.Id);
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"{base.ToString()}\n";
			empty += $"選択中の艦隊-{CurrentDeck}\n";
			empty += $"--選択可能な艦隊演習--\n";
			empty += ToString(DeckPracticeType.Normal, DeckPracticeTypeDic);
			empty += ToString(DeckPracticeType.Hou, DeckPracticeTypeDic);
			empty += ToString(DeckPracticeType.Rai, DeckPracticeTypeDic);
			empty += ToString(DeckPracticeType.Taisen, DeckPracticeTypeDic);
			empty += ToString(DeckPracticeType.Kouku, DeckPracticeTypeDic);
			empty += ToString(DeckPracticeType.Sougou, DeckPracticeTypeDic);
			empty += "\n";
			empty += $"--選択可能な対抗演習相手--\n";
			for (int i = 0; i < RivalDecks.Count; i++)
			{
				List<IsGoCondition> list = IsValidPractice(RivalDecks[i].Id);
				if (list.Count == 0)
				{
					empty += $"{RivalDecks[i]}\n";
					continue;
				}
				empty += $"{RivalDecks[i]} - [";
				for (int j = 0; j < list.Count; j++)
				{
					empty = empty + list[j] + ", ";
				}
				empty += "]\n";
			}
			return empty;
		}

		private string ToString(DeckPracticeType type, Dictionary<DeckPracticeType, bool> dic)
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add("艦隊行動演習");
			list.Add("砲戦演習");
			list.Add("雷撃戦演習");
			list.Add("対潜戦演習");
			list.Add("航空戦演習");
			list.Add("総合演習");
			List<string> list2 = list;
			string str = $"{list2[(int)type]}-{type} ";
			str = ((!dic.ContainsKey(type)) ? (str + $"[未開放]") : (str + string.Format("{0}", (!dic[type]) ? "[選択不可]" : string.Empty)));
			return str + "\n";
		}
	}
}
