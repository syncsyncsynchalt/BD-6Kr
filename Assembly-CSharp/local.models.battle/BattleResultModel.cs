using Common.Enum;
using local.managers;
using Server_Common.Formats;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class BattleResultModel
	{
		private string _deck_name = string.Empty;

		private string _user_name = string.Empty;

		private BattleResultFmt _fmt;

		private List<int> _new_opened_area_ids;

		private List<int> _new_opened_map_ids;

		private List<ShipModel_BattleResult> _ships_f;

		private List<ShipModel_BattleResult> _ships_e;

		private int _hp_start_f;

		private int _hp_start_e;

		private int _hp_end_f;

		private int _hp_end_e;

		private bool _first_area_clear;

		private ShipModel_BattleResult _mvp_ship;

		public BattleWinRankKinds WinRank => _fmt.WinRank;

		public string DeckName => _deck_name;

		public string UserName => _user_name;

		public int UserLevel => _fmt.BasicLevel;

		public int SPoint => _fmt.GetSpoint;

		public ShipModel_BattleResult[] Ships_f => _ships_f.ToArray();

		public ShipModel_BattleResult[] Ships_e => _ships_e.ToArray();

		public string MapName => _fmt.QuestName;

		public int BaseExp => _fmt.GetBaseExp;

		public int HPStart_f => _hp_start_f;

		public int HPStart_e => _hp_start_e;

		public int HPEnd_f => _hp_end_f;

		public int HPEnd_e => _hp_end_e;

		public string EnemyName => _fmt.EnemyName;

		public bool FirstClear => _fmt.FirstClear;

		public bool FirstAreaClear => _first_area_clear;

		public int[] NewOpenAreaIDs => _new_opened_area_ids.ToArray();

		public int[] NewOpenMapIDs => _new_opened_map_ids.ToArray();

		public List<int> ReOpenMapIDs => _fmt.ReOpenMapId;

		public ShipModel_BattleResult MvpShip => _mvp_ship;

		public BattleResultModel(int deck_id, BattleManager bManager, BattleResultFmt fmt, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, MapModel map, Dictionary<int, int> exp_rates_before)
		{
			_Init(deck_id, -1, bManager, fmt, ships_f, ships_e, exp_rates_before);
		}

		public BattleResultModel(int deck_id, int enemy_deck_id, BattleManager bManager, BattleResultFmt fmt, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Dictionary<int, int> exp_rates_before)
		{
			_Init(deck_id, enemy_deck_id, bManager, fmt, ships_f, ships_e, exp_rates_before);
		}

		public List<IReward> GetRewardItems()
		{
			return _ConvertItemGetFmts(_fmt.GetItem);
		}

		public List<IReward> GetAreaRewardItems()
		{
			if (_fmt.AreaClearRewardItem == null)
			{
				return null;
			}
			List<IReward> list = new List<IReward>();
			list.Add(_ConvertItemGetFmt(_fmt.AreaClearRewardItem));
			return list;
		}

		public List<MapEventItemModel> GetAirReconnaissanceItems()
		{
			if (_fmt.GetAirReconnaissanceItems == null)
			{
				return null;
			}
			return _fmt.GetAirReconnaissanceItems.ConvertAll((MapItemGetFmt i) => new MapEventItemModel(i));
		}

		private void _Init(int deck_id, int enemy_deck_id, BattleManager bManager, BattleResultFmt fmt, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Dictionary<int, int> exp_rates_before)
		{
			UserInfoModel userInfo = bManager.UserInfo;
			DeckModel deck = userInfo.GetDeck(deck_id);
			_deck_name = deck.Name;
			_user_name = userInfo.Name;
			_fmt = fmt;
			_ships_f = ships_f.ConvertAll((ShipModel_BattleAll ship) => (ShipModel_BattleResult)ship);
			_ships_e = ships_e.ConvertAll((ShipModel_BattleAll ship) => (ShipModel_BattleResult)ship);
			_mvp_ship = _ships_f.Find((ShipModel_BattleResult ship) => ship != null && ship.TmpId == _fmt.MvpShip);
			for (int i = 0; i < Ships_f.Length; i++)
			{
				ShipModel_BattleResult shipModel_BattleResult = Ships_f[i];
				if (shipModel_BattleResult != null)
				{
					_SetShipExp(deck, shipModel_BattleResult, exp_rates_before);
					_hp_start_f += shipModel_BattleResult.HpStart;
					_hp_end_f += shipModel_BattleResult.HpEnd;
				}
			}
			DeckModel deck2 = userInfo.GetDeck(enemy_deck_id);
			for (int j = 0; j < Ships_e.Length; j++)
			{
				ShipModel_BattleResult shipModel_BattleResult2 = Ships_e[j];
				if (shipModel_BattleResult2 != null)
				{
					_SetShipExp(deck2, shipModel_BattleResult2, exp_rates_before);
					_hp_start_e += shipModel_BattleResult2.HpStart;
					_hp_end_e += shipModel_BattleResult2.HpEnd;
				}
			}
			_new_opened_map_ids = fmt.NewOpenMapId.GetRange(0, fmt.NewOpenMapId.Count);
			_new_opened_map_ids.AddRange(fmt.ReOpenMapId);
			_new_opened_area_ids = _new_opened_map_ids.FindAll((int map_id) => map_id % 10 == 1);
			_new_opened_area_ids = _new_opened_area_ids.ConvertAll((int map_id) => (int)Math.Floor((double)map_id / 10.0));
			_first_area_clear = fmt.FirstAreaComplete;
		}

		private void _SetShipExp(DeckModel deck, ShipModel_BattleResult ship, Dictionary<int, int> exp_rates_before)
		{
			if (deck == null)
			{
				ship.__InitResultData__(0, null, 0, null);
				return;
			}
			ShipModel shipFromMemId = deck.GetShipFromMemId(ship.TmpId);
			int value = 0;
			exp_rates_before.TryGetValue(ship.TmpId, out value);
			int value2 = 0;
			_fmt.GetShipExp.TryGetValue(ship.TmpId, out value2);
			List<int> value3 = null;
			_fmt.LevelUpInfo.TryGetValue(ship.TmpId, out value3);
			ship.__InitResultData__(value, shipFromMemId, value2, value3);
		}

		private List<IReward> _ConvertItemGetFmts(List<ItemGetFmt> fmts)
		{
			if (fmts == null)
			{
				return new List<IReward>();
			}
			return fmts.ConvertAll((ItemGetFmt item) => _ConvertItemGetFmt(item));
		}

		private IReward _ConvertItemGetFmt(ItemGetFmt fmt)
		{
			if (fmt.Category == ItemGetKinds.Ship)
			{
				return new Reward_Ship(fmt.Id);
			}
			if (fmt.Category == ItemGetKinds.SlotItem)
			{
				return new Reward_Slotitem(fmt.Id, fmt.Count);
			}
			if (fmt.Category == ItemGetKinds.UseItem)
			{
				return new Reward_Useitem(fmt.Id, fmt.Count);
			}
			return null;
		}

		public string ToString(int[] values)
		{
			if (values == null)
			{
				return string.Empty;
			}
			string text = string.Empty;
			for (int i = 0; i < values.Length; i++)
			{
				text += values[i];
				if (i < values.Length - 1)
				{
					text += ",";
				}
			}
			return text;
		}

		public override string ToString()
		{
			string str = "-- BattleResultDTO --\n";
			str += $"勝利ランク：{WinRank}   提督名：{UserName}   戦闘後の提督レベル：{UserLevel}\n";
			str += $"海域名：{MapName} 味方艦隊名：{DeckName}  敵艦隊名：{EnemyName}   海域基本経験値：{BaseExp}\n";
			for (int i = 0; i < Ships_f.Length; i++)
			{
				ShipModel_BattleResult shipModel_BattleResult = Ships_f[i];
				if (shipModel_BattleResult != null)
				{
					str += string.Format("[{0}] ID:({1}) {2} 状態:{3} {4} {5}\n", i, shipModel_BattleResult.MstId, shipModel_BattleResult.Name, shipModel_BattleResult.DmgStateEnd, shipModel_BattleResult.ExpInfo, (shipModel_BattleResult != MvpShip) ? string.Empty : "[MVP]");
				}
			}
			for (int j = 0; j < Ships_e.Length; j++)
			{
				ShipModel_BattleResult shipModel_BattleResult2 = Ships_e[j];
				if (shipModel_BattleResult2 != null)
				{
					str += $"[{j}] ID:({shipModel_BattleResult2.MstId}) {shipModel_BattleResult2.Name} 状態:{shipModel_BattleResult2.DmgStateEnd}  種類(読み): {shipModel_BattleResult2.Yomi} {shipModel_BattleResult2.ExpInfo}\n";
				}
			}
			str += $"自分側HP {HPStart_f}->{HPEnd_f}\n";
			str += $"相手側HP {HPStart_e}->{HPEnd_e}\n";
			List<IReward> rewardItems = GetRewardItems();
			if (rewardItems.Count > 0)
			{
				str += "《ドロップあり》\n";
				for (int k = 0; k < rewardItems.Count; k++)
				{
					str += $"{rewardItems[k]}\n";
				}
			}
			else
			{
				str += "《ドロップなし》\n";
			}
			rewardItems = GetAreaRewardItems();
			if (rewardItems != null && rewardItems.Count > 0)
			{
				str += "《海域クリア報酬あり》\n";
				for (int l = 0; l < rewardItems.Count; l++)
				{
					str += $"{rewardItems[l]}\n";
				}
			}
			else
			{
				str += "《海域クリア報酬なし》\n";
			}
			if (FirstClear || FirstAreaClear)
			{
				if (FirstClear)
				{
					str += $"[初回マップクリア]";
				}
				if (FirstAreaClear)
				{
					str += $"[初回海域クリア]";
				}
				str += $"\n";
			}
			str += $"-新開放海域-\n";
			for (int m = 0; m < _new_opened_area_ids.Count; m++)
			{
				str += $"{_new_opened_area_ids[m]}\n";
			}
			str += $"-新開放マップ-\n";
			for (int n = 0; n < _new_opened_map_ids.Count; n++)
			{
				str += $"{_new_opened_map_ids[n]}\n";
			}
			str += $"-獲得戦略ポイント:{SPoint}\n";
			return str + "---------------------";
		}
	}
}
