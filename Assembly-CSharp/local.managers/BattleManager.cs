using Common.Enum;
using local.models;
using local.models.battle;
using local.utils;
using Server_Common.Formats.Battle;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class BattleManager : ManagerBase
	{
		protected AllBattleFmt _battleData;

		protected CombatPhase _phase;

		protected List<ShipModel_BattleAll> _ships_f;

		protected List<ShipModel_BattleAll> _ships_e;

		protected int _ship_count_f;

		protected int _ship_count_e;

		protected SakutekiModel _cache_sakuteki;

		protected RationModel _cache_ration;

		protected CommandPhaseModel _cache_command;

		protected KoukuuModel _cache_kouku;

		protected IShienModel _cache_shien;

		protected RaigekiModel _cache_kaimaku;

		protected EffectModel _cache_opening_effect;

		protected List<CmdActionPhaseModel> _cache_cmd_actions;

		protected RaigekiModel _cache_raigeki;

		protected KoukuuModel _cache_kouku2;

		protected NightCombatModel _cache_opening_n;

		protected HougekiListModel _cache_hougeki_n;

		protected BattleResultFmt _cache_result_fmt;

		protected int _enemy_deck_id;

		protected enumMapWarType _war_type;

		protected bool _is_boss;

		protected bool _last_cell;

		protected MapModel _map;

		protected List<MapModel> _maps;

		protected bool _changeable_deck;

		protected int _recovery_item_use_count_at_start;

		protected int _recovery_item_use_count_in_battle;

		public int DeckId => _GetCurrentHeader().F_DeckShip1.Deck_Id;

		public int EnemyDeckId => _enemy_deck_id;

		public virtual string EnemyDeckName => string.Empty;

		public ShipModel_BattleAll[] Ships_f => _ships_f.ToArray();

		public ShipModel_BattleAll[] Ships_e => _ships_e.ToArray();

		public int ShipCount_f => _ship_count_f;

		public int ShipCount_e => _ship_count_e;

		public BattleFormationKinds1 FormationId_f => _battleData.Formation[0];

		public BattleFormationKinds1 FormationId_e => _battleData.Formation[1];

		public BattleFormationKinds2 CrossFormationId => _battleData.BattleFormation;

		public bool IsPractice => _enemy_deck_id != -1;

		public enumMapWarType WarType => _war_type;

		public bool BossBattle => _is_boss;

		public string AreaName => ManagerBase._area[_map.AreaId].Name;

		public MapModel Map => _map;

		public List<MapModel> Maps => _maps;

		public bool ChangeableDeck => _changeable_deck;

		public int RecoveryItemUseCountAtStart => _recovery_item_use_count_at_start;

		public int RecoveryItemUseCountInBattle => _recovery_item_use_count_in_battle;

		public BattleManager()
		{
		}

		public int GetBgmId()
		{
			bool master_loaded;
			return GetBgmId(_phase == CombatPhase.DAY, BossBattle, out master_loaded);
		}

		public int GetBgmId(bool is_day, bool is_boss, out bool master_loaded)
		{
			int key = (!is_day) ? 1 : 0;
			int index = is_boss ? 1 : 0;
			Dictionary<int, List<int>> battleBgm = Mst_DataManager.Instance.UiBattleMaster.BattleBgm;
			master_loaded = (battleBgm != null);
			if (master_loaded)
			{
				return battleBgm[key][index];
			}
			return is_day ? 1 : 2;
		}

		public ShipModel_BattleAll GetShip(int tmp_id)
		{
			List<ShipModel_BattleAll> list = new List<ShipModel_BattleAll>(Ships_f);
			list.AddRange(Ships_e);
			return list.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == tmp_id);
		}

		public ShipModel_BattleAll GetShip(int index, bool is_friend)
		{
			List<ShipModel_BattleAll> list = (!is_friend) ? _ships_e : _ships_f;
			if (0 <= index && index < list.Count)
			{
				ShipModel_BattleAll shipModel_BattleAll = list[index];
				if (shipModel_BattleAll != null)
				{
					return shipModel_BattleAll;
				}
			}
			return null;
		}

		public BossInsertModel GetBossInsertData()
		{
			return (!_is_boss) ? null : new BossInsertModel(Ships_e[0]);
		}

		public bool IsFiascoSakuteki()
		{
			if (_phase != 0)
			{
				return false;
			}
			if (_battleData.DayBattle.Search == null)
			{
				return false;
			}
			SearchInfo searchInfo = _battleData.DayBattle.Search[0];
			if (searchInfo.UsePlane.Count == 0 && (searchInfo.SearchValue == BattleSearchValues.Lost || searchInfo.SearchValue == BattleSearchValues.Faile || searchInfo.SearchValue == BattleSearchValues.NotFound))
			{
				return true;
			}
			return false;
		}

		public bool IsNotFiascoSakuteki()
		{
			if (_phase != 0)
			{
				return false;
			}
			if (_battleData.DayBattle.Search == null)
			{
				return false;
			}
			SearchInfo searchInfo = _battleData.DayBattle.Search[0];
			if (searchInfo.UsePlane.Count > 0)
			{
				return true;
			}
			if (searchInfo.SearchValue == BattleSearchValues.Success || searchInfo.SearchValue == BattleSearchValues.Success_Lost || searchInfo.SearchValue == BattleSearchValues.Found)
			{
				return true;
			}
			return false;
		}

		public bool IsExistSakutekiData()
		{
			if (_phase == CombatPhase.DAY)
			{
				return true;
			}
			return false;
		}

		public SakutekiModel GetSakutekiData()
		{
			if (_cache_sakuteki == null && _phase == CombatPhase.DAY)
			{
				_cache_sakuteki = new SakutekiModel(_battleData.DayBattle.Search, _ships_f, _ships_e);
			}
			return _cache_sakuteki;
		}

		public bool IsExistRationPhase()
		{
			return _cache_ration != null;
		}

		public RationModel GetRationModel()
		{
			return _cache_ration;
		}

		public bool IsExistCommandPhase()
		{
			return true;
		}

		public bool IsValidPresetCommand()
		{
			CommandPhaseModel commandPhaseModel = GetCommandPhaseModel();
			return commandPhaseModel?.IsValidCommand(commandPhaseModel.GetPresetCommand()) ?? false;
		}

		public bool IsTakeCommand()
		{
			if (_cache_command == null)
			{
				return false;
			}
			return _cache_command.IsTakeCommand();
		}

		public abstract CommandPhaseModel GetCommandPhaseModel();

		public EffectModel GetOpeningEffectData()
		{
			return _cache_opening_effect;
		}

		public EffectModel GetEffectData(int index)
		{
			if (_cache_cmd_actions != null && index < _cache_cmd_actions.Count)
			{
				CmdActionPhaseModel cmdActionPhaseModel = _cache_cmd_actions[index];
				if (cmdActionPhaseModel != null && cmdActionPhaseModel.Effect != null)
				{
					return cmdActionPhaseModel.Effect;
				}
			}
			return null;
		}

		public bool IsExistKoukuuData()
		{
			return _cache_kouku != null;
		}

		public KoukuuModel GetKoukuuData()
		{
			return _cache_kouku;
		}

		public bool IsExistShienData()
		{
			return _cache_shien != null;
		}

		public IShienModel GetShienData()
		{
			return _cache_shien;
		}

		public bool IsExistKaimakuData()
		{
			return _cache_kaimaku != null;
		}

		public RaigekiModel GetKaimakuData()
		{
			return _cache_kaimaku;
		}

		public bool IsExistHougekiPhase_Day()
		{
			return _cache_cmd_actions != null;
		}

		public List<CmdActionPhaseModel> GetHougekiData_Day()
		{
			return _cache_cmd_actions;
		}

		public CmdActionPhaseModel GetHougekiData_Day(int index)
		{
			if (_cache_cmd_actions != null && index < _cache_cmd_actions.Count)
			{
				return _cache_cmd_actions[index];
			}
			return null;
		}

		public bool IsExistHougekiPhase_Night()
		{
			return _cache_hougeki_n != null;
		}

		public HougekiListModel GetHougekiList_Night()
		{
			return _cache_hougeki_n;
		}

		public bool IsExistRaigekiData()
		{
			return _cache_raigeki != null;
		}

		public RaigekiModel GetRaigekiData()
		{
			return _cache_raigeki;
		}

		public bool IsExistKoukuuData2()
		{
			return _cache_kouku2 != null;
		}

		public KoukuuModel GetKoukuuData2()
		{
			return _cache_kouku2;
		}

		public virtual bool HasNightBattle()
		{
			return _phase == CombatPhase.DAY && _battleData.DayBattle.ValidMidnight;
		}

		public NightCombatModel GetNightCombatData()
		{
			return _cache_opening_n;
		}

		public virtual void StartDayToNightBattle()
		{
		}

		public abstract BattleResultModel GetBattleResult();

		public ShipModel_BattleAll[] GetSubMarineShips_f()
		{
			if (_ships_f == null)
			{
				return new ShipModel_BattleAll[0];
			}
			return _ships_f.FindAll((ShipModel_BattleAll ship) => ship.IsSubMarine()).ToArray();
		}

		public ShipModel_BattleAll[] GetSubMarineShips_e()
		{
			if (_ships_e == null)
			{
				return new ShipModel_BattleAll[0];
			}
			return _ships_e.FindAll((ShipModel_BattleAll ship) => ship.IsSubMarine()).ToArray();
		}

		public bool IsSubMarineAllShip_f()
		{
			if (_ships_f == null)
			{
				return false;
			}
			return _ships_f.Count == GetSubMarineShips_f().Length;
		}

		public bool IsSubMarineAllShip_e()
		{
			if (_ships_e == null)
			{
				return false;
			}
			return _ships_e.Count == GetSubMarineShips_e().Length;
		}

		public bool HasPossibilityTaihaShingun()
		{
			if (this is PracticeBattleManager)
			{
				return false;
			}
			if (!_ships_f[0].HasRecoverMegami() && !_ships_f[0].HasRecoverYouin())
			{
				return false;
			}
			if (_last_cell)
			{
				return false;
			}
			return true;
		}

		public ShipModel[] GetEscapeCandidate()
		{
			if (_cache_result_fmt != null && _cache_result_fmt.EscapeInfo != null && _cache_result_fmt.EscapeInfo.TowShips.Count > 0 && _cache_result_fmt.EscapeInfo.EscapeShips.Count > 0)
			{
				int ship_mem_id = _cache_result_fmt.EscapeInfo.TowShips[0];
				ShipModel ship = base.UserInfo.GetShip(ship_mem_id);
				int ship_mem_id2 = _cache_result_fmt.EscapeInfo.EscapeShips[0];
				ShipModel ship2 = base.UserInfo.GetShip(ship_mem_id2);
				if (ship != null && ship2 != null)
				{
					return new ShipModel[2]
					{
						ship,
						ship2
					};
				}
			}
			return null;
		}

		public virtual bool SendOffEscapes()
		{
			return false;
		}

		public ShipRecoveryType IsUseRecoverySlotitem(int ship_tmp_id)
		{
			bool flag = _battleData.DayBattle != null && GetCommandPhaseModel().IsTakeCommand();
			bool flag2 = _battleData.NightBattle != null;
			if (!flag && !flag2)
			{
				return ShipRecoveryType.None;
			}
			return GetShip(ship_tmp_id)?.IsUseRecoverySlotitem() ?? ShipRecoveryType.None;
		}

		public ShipRecoveryType IsUseRecoverySlotitem(int ship_tmp_id, bool is_day)
		{
			if (is_day)
			{
				if (_battleData.DayBattle == null || !GetCommandPhaseModel().IsTakeCommand())
				{
					return ShipRecoveryType.None;
				}
				return GetShip(ship_tmp_id)?.IsUseRecoverySlotitemAtFirstCombat() ?? ShipRecoveryType.None;
			}
			if (_battleData.NightBattle == null)
			{
				return ShipRecoveryType.None;
			}
			ShipModel_BattleAll ship = GetShip(ship_tmp_id);
			if (ship == null)
			{
				return ShipRecoveryType.None;
			}
			if (_battleData.DayBattle == null)
			{
				return ship.IsUseRecoverySlotitemAtFirstCombat();
			}
			return ship.IsUseRecoverySlotitemAtSecondCombat();
		}

		public void IncrementRecoveryItemCountWithTrophyUnlock()
		{
			_recovery_item_use_count_in_battle++;
			TrophyUtil.Unlock_At_Battle(_recovery_item_use_count_at_start, _recovery_item_use_count_in_battle);
		}

		protected Dictionary<int, ShipModel_BattleAll> _GetShipsDic()
		{
			Dictionary<int, ShipModel_BattleAll> dictionary = new Dictionary<int, ShipModel_BattleAll>();
			for (int i = 0; i < _ships_f.Count; i++)
			{
				if (_ships_f[i] != null)
				{
					dictionary[_ships_f[i].TmpId] = _ships_f[i];
				}
			}
			for (int j = 0; j < _ships_e.Count; j++)
			{
				if (_ships_e[j] != null)
				{
					dictionary[_ships_e[j].TmpId] = _ships_e[j];
				}
			}
			return dictionary;
		}

		protected List<ShipModel_BattleAll> _CreateShipData_f(BattleHeader header, bool practice)
		{
			BattleShipFmt[] ships = header.F_DeckShip1.Ships;
			return _CreateShipData(ships, is_friend: true, practice, out _ship_count_f);
		}

		protected List<ShipModel_BattleAll> _CreateShipData_e(BattleHeader header, bool practice)
		{
			BattleShipFmt[] ships = header.E_DeckShip1.Ships;
			return _CreateShipData(ships, is_friend: false, practice, out _ship_count_e);
		}

		protected List<ShipModel_BattleAll> _CreateShipData(BattleShipFmt[] ship_fmts, bool is_friend, bool practice, out int count)
		{
			count = 0;
			List<ShipModel_BattleAll> list = new List<ShipModel_BattleAll>();
			for (int i = 0; i < 6; i++)
			{
				BattleShipFmt battleShipFmt = ship_fmts[i];
				if (battleShipFmt == null)
				{
					list.Add(null);
					continue;
				}
				ShipModel_BattleAll item = new ShipModel_BattleResult(battleShipFmt, i, is_friend, practice);
				list.Add(item);
				count++;
			}
			return list;
		}

		protected virtual BattleResultFmt _GetBattleResult()
		{
			return null;
		}

		protected BattleHeader _GetCurrentHeader()
		{
			return (_phase != 0) ? _battleData.NightBattle.Header : _battleData.DayBattle.Header;
		}

		public void __createCacheDataBeforeCommand__()
		{
			if (_phase == CombatPhase.DAY && !IsTakeCommand())
			{
				_cache_ration = null;
				if (_GetRationData() != null)
				{
					_cache_ration = new RationModel(_ships_f, _GetRationData());
				}
			}
		}

		public void __createCacheDataAfterCommand__()
		{
			if (_phase != 0 || !IsTakeCommand())
			{
				return;
			}
			_cache_opening_effect = null;
			if (_battleData.DayBattle.OpeningProduction != null)
			{
				_cache_opening_effect = new __EffectModel__(_battleData.DayBattle.OpeningProduction);
			}
			_cache_kouku = null;
			if (_battleData.DayBattle.AirBattle != null)
			{
				int count = _battleData.DayBattle.AirBattle.F_PlaneFrom.Count;
				int count2 = _battleData.DayBattle.AirBattle.E_PlaneFrom.Count;
				if (count > 0 || count2 > 0)
				{
					_cache_kouku = new KoukuuModel(_ships_f, _ships_e, _battleData.DayBattle.AirBattle);
				}
			}
			_cache_shien = null;
			if (_battleData.DayBattle.SupportAtack != null)
			{
				SupportAtack supportAtack = _battleData.DayBattle.SupportAtack;
				int deck_Id = supportAtack.Deck_Id;
				DeckModel deck = base.UserInfo.GetDeck(deck_Id);
				switch (supportAtack.SupportType)
				{
					case BattleSupportKinds.AirAtack:
						_cache_shien = new ShienModel_Air(deck, _ships_f, _ships_e, supportAtack);
						break;
					case BattleSupportKinds.Hougeki:
						_cache_shien = new ShienModel_Hou(deck, _ships_f, _ships_e, supportAtack);
						break;
					case BattleSupportKinds.Raigeki:
						_cache_shien = new ShienModel_Rai(deck, _ships_f, _ships_e, supportAtack);
						break;
				}
			}
			_cache_kaimaku = null;
			if (_battleData.DayBattle.OpeningAtack != null)
			{
				_cache_kaimaku = new RaigekiModel(_ships_f, _ships_e, _battleData.DayBattle.OpeningAtack);
				if (_cache_kaimaku.Count_f == 0 && _cache_kaimaku.Count_e == 0)
				{
					_cache_kaimaku = null;
				}
			}
			_cache_cmd_actions = null;
			if (_battleData.DayBattle.FromMiddleDayBattle != null)
			{
				_cache_cmd_actions = new List<CmdActionPhaseModel>();
				Dictionary<int, ShipModel_BattleAll> ships = _GetShipsDic();
				for (int i = 0; i < _battleData.DayBattle.FromMiddleDayBattle.Count; i++)
				{
					FromMiddleBattleDayData data = _battleData.DayBattle.FromMiddleDayBattle[i];
					CmdActionPhaseModel item = new CmdActionPhaseModel(data, ships);
					_cache_cmd_actions.Add(item);
				}
				if (_cache_cmd_actions.TrueForAll((CmdActionPhaseModel model) => model == null || !model.HasAction()))
				{
					_cache_cmd_actions = null;
				}
				else if (_cache_cmd_actions.Count == 0)
				{
					_cache_cmd_actions = null;
				}
			}
			_cache_raigeki = null;
			if (_battleData.DayBattle.Raigeki != null)
			{
				_cache_raigeki = new RaigekiModel(_ships_f, _ships_e, _battleData.DayBattle.Raigeki);
				if (_cache_raigeki.Count_f == 0 && _cache_raigeki.Count_e == 0)
				{
					_cache_raigeki = null;
				}
			}
			_cache_kouku2 = null;
			if (_battleData.DayBattle.AirBattle2 != null)
			{
				int count3 = _battleData.DayBattle.AirBattle2.F_PlaneFrom.Count;
				int count4 = _battleData.DayBattle.AirBattle2.E_PlaneFrom.Count;
				if (count3 > 0 || count4 > 0)
				{
					_cache_kouku2 = new KoukuuModel(_ships_f, _ships_e, _battleData.DayBattle.AirBattle2);
				}
			}
			if (_cache_opening_effect != null)
			{
				ShipModel_Battle nextActionShip = _GetFirstActionShip(0);
				((__EffectModel__)_cache_opening_effect).SetNextActionShip(nextActionShip);
			}
			if (_cache_cmd_actions == null)
			{
				return;
			}
			for (int j = 0; j < _cache_cmd_actions.Count; j++)
			{
				CmdActionPhaseModel cmdActionPhaseModel = _cache_cmd_actions[j];
				if (cmdActionPhaseModel != null && cmdActionPhaseModel.Effect != null)
				{
					ShipModel_Battle nextActionShip = _GetFirstActionShip(j + 1);
					((__EffectModel__)cmdActionPhaseModel.Effect).SetNextActionShip(nextActionShip);
				}
			}
		}

		public void __createCacheDataNight__()
		{
			if (_phase == CombatPhase.NIGHT)
			{
				RationModel ration = null;
				if (_GetRationData() != null)
				{
					ration = new RationModel(_ships_f, _GetRationData());
				}
				_cache_opening_n = new NightCombatModel(this, _battleData.NightBattle, ration);
				_cache_hougeki_n = null;
				if (_battleData.NightBattle.Hougeki != null)
				{
					List<Hougeki<BattleAtackKinds_Night>> hougeki = _battleData.NightBattle.Hougeki;
					_cache_hougeki_n = new HougekiListModel(hougeki, _GetShipsDic());
				}
			}
		}

		private ShipModel_Battle _GetFirstActionShip(int order)
		{
			if (order <= 0)
			{
				if (IsExistKoukuuData())
				{
					ShipModel_Battle firstActionShip = GetKoukuuData().GetFirstActionShip();
					if (firstActionShip != null)
					{
						return firstActionShip;
					}
				}
				if (IsExistShienData())
				{
					return null;
				}
				if (IsExistKaimakuData())
				{
					ShipModel_Battle firstActionShip2 = GetKaimakuData().GetFirstActionShip();
					if (firstActionShip2 != null)
					{
						return firstActionShip2;
					}
				}
				if (GetEffectData(0) != null)
				{
					return null;
				}
			}
			if (order <= 1)
			{
				CmdActionPhaseModel hougekiData_Day = GetHougekiData_Day(0);
				if (hougekiData_Day != null)
				{
					ShipModel_Battle firstActionShip3 = hougekiData_Day.GetFirstActionShip();
					if (firstActionShip3 != null)
					{
						return firstActionShip3;
					}
					if (GetEffectData(1) != null)
					{
						return null;
					}
				}
			}
			if (order <= 2)
			{
				CmdActionPhaseModel hougekiData_Day2 = GetHougekiData_Day(1);
				if (hougekiData_Day2 != null)
				{
					ShipModel_Battle firstActionShip4 = hougekiData_Day2.GetFirstActionShip();
					if (firstActionShip4 != null)
					{
						return firstActionShip4;
					}
					if (GetEffectData(2) != null)
					{
						return null;
					}
				}
			}
			if (order <= 3)
			{
				CmdActionPhaseModel hougekiData_Day3 = GetHougekiData_Day(2);
				if (hougekiData_Day3 != null)
				{
					ShipModel_Battle firstActionShip5 = hougekiData_Day3.GetFirstActionShip();
					if (firstActionShip5 != null)
					{
						return firstActionShip5;
					}
					if (GetEffectData(3) != null)
					{
						return null;
					}
				}
			}
			if (order <= 4)
			{
				CmdActionPhaseModel hougekiData_Day4 = GetHougekiData_Day(3);
				if (hougekiData_Day4 != null)
				{
					ShipModel_Battle firstActionShip6 = hougekiData_Day4.GetFirstActionShip();
					if (firstActionShip6 != null)
					{
						return firstActionShip6;
					}
					if (GetEffectData(4) != null)
					{
						return null;
					}
				}
			}
			if (order <= 5)
			{
				CmdActionPhaseModel hougekiData_Day5 = GetHougekiData_Day(4);
				if (hougekiData_Day5 != null)
				{
					ShipModel_Battle firstActionShip7 = hougekiData_Day5.GetFirstActionShip();
					if (firstActionShip7 != null)
					{
						return firstActionShip7;
					}
				}
				if (IsExistRaigekiData())
				{
					ShipModel_Battle firstActionShip7 = GetRaigekiData().GetFirstActionShip();
					if (firstActionShip7 != null)
					{
						return firstActionShip7;
					}
				}
				if (IsExistKoukuuData2())
				{
					ShipModel_Battle firstActionShip7 = GetKoukuuData2().GetFirstActionShip();
					if (firstActionShip7 != null)
					{
						return firstActionShip7;
					}
				}
			}
			return null;
		}

		private Dictionary<int, List<Mst_slotitem>> _GetRationData()
		{
			if (_phase == CombatPhase.DAY)
			{
				return _battleData.DayBattle?.Header?.UseRationShips;
			}
			return _battleData.NightBattle?.Header?.UseRationShips;
		}

		public override string ToString()
		{
			string str = "\n";
			bool master_loaded;
			int bgmId = GetBgmId(is_day: true, BossBattle, out master_loaded);
			bool master_loaded2;
			int bgmId2 = GetBgmId(is_day: false, BossBattle, out master_loaded2);
			str += string.Format("[BGM - 昼] {0}{1}\t[BGM - 夜] {2}{3}\n", bgmId, (!master_loaded) ? "(マスタ未ロード)" : string.Empty, bgmId2, (!master_loaded2) ? "(マスタ未ロード)" : string.Empty);
			for (int i = 0; i < Ships_f.Length; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = Ships_f[i];
				str = ((shipModel_BattleAll == null) ? (str + $"[{i}] -\n") : (str + $"[{i}] {shipModel_BattleAll}\n"));
			}
			str += $"== 「{EnemyDeckName}」 ==\n";
			for (int j = 0; j < Ships_e.Length; j++)
			{
				ShipModel_BattleAll shipModel_BattleAll2 = Ships_e[j];
				str = ((shipModel_BattleAll2 == null) ? (str + $"[{j}] -\n") : (str + $"[{j}] {shipModel_BattleAll2}\n"));
			}
			return str + $"自側陣形:{FormationId_f} 敵側陣形:{FormationId_e} 交戦体系:{CrossFormationId}";
		}
	}
}
