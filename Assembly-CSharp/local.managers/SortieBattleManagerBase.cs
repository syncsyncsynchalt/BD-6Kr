using Common.Enum;
using local.models;
using local.models.battle;
using local.utils;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class SortieBattleManagerBase : BattleManager
	{
		public Api_req_SortieBattle _reqBattle;

		private BattleResultModel _cache_result;

		private string _enemy_deck_name;

		public override string EnemyDeckName => _enemy_deck_name;

		public SortieBattleManagerBase(string enemy_deck_name)
		{
			_enemy_deck_name = enemy_deck_name;
		}

		public virtual void __Init__(Api_req_SortieBattle reqBattle, enumMapWarType warType, BattleFormationKinds1 formationId, MapModel map, List<MapModel> maps, bool lastCell, bool isBoss)
		{
			_recovery_item_use_count_at_start = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			_enemy_deck_id = -1;
			_reqBattle = reqBattle;
			_war_type = warType;
			_is_boss = isBoss;
			_last_cell = lastCell;
			_map = map;
			_maps = maps;
			BattleHeader header = null;
			switch (warType)
			{
				case enumMapWarType.Normal:
				case enumMapWarType.AirBattle:
					_battleData = _reqBattle.GetDayPreBattleInfo(formationId).data;
					_phase = CombatPhase.DAY;
					header = _battleData.DayBattle.Header;
					break;
				case enumMapWarType.Midnight:
					_battleData = _reqBattle.Night_Sp(formationId).data;
					_phase = CombatPhase.NIGHT;
					header = _battleData.NightBattle.Header;
					break;
				default:
					throw new Exception("Logic Error");
				case enumMapWarType.Night_To_Day:
					break;
			}
			_ships_f = _CreateShipData_f(header, practice: false);
			_ships_e = _CreateShipData_e(header, practice: false);
			if (_phase == CombatPhase.DAY)
			{
				__createCacheDataBeforeCommand__();
			}
			else
			{
				__createCacheDataNight__();
			}
		}

		public override void StartDayToNightBattle()
		{
			_battleData = _reqBattle.NightBattle().data;
			_phase = CombatPhase.NIGHT;
			for (int i = 0; i < _ships_f.Count; i++)
			{
				if (_ships_f[i] != null)
				{
					_ships_f[i].__CreateStarter__();
				}
			}
			for (int j = 0; j < _ships_e.Count; j++)
			{
				if (_ships_e[j] != null)
				{
					_ships_e[j].__CreateStarter__();
				}
			}
			__createCacheDataNight__();
		}

		public override BattleResultModel GetBattleResult()
		{
			if (_cache_result != null)
			{
				return _cache_result;
			}
			DeckModel deck = base.UserInfo.GetDeck(base.DeckId);
			Dictionary<int, int> dic = new Dictionary<int, int>();
			deck.__CreateShipExpRatesDictionary__(ref dic);
			BattleResultFmt battleResultFmt = _GetBattleResult();
			if (battleResultFmt == null)
			{
				return null;
			}
			_cache_result = new BattleResultModel(base.DeckId, this, battleResultFmt, _ships_f, _ships_e, _map, dic);
			if (_cache_result.WinRank == BattleWinRankKinds.S)
			{
				Comm_UserDatas.Instance.User_trophy.Win_S_count++;
			}
			List<int> reOpenMapIDs = _cache_result.ReOpenMapIDs;
			if (reOpenMapIDs != null && reOpenMapIDs.Count > 0)
			{
				for (int i = 0; i < reOpenMapIDs.Count; i++)
				{
					int num = (int)Math.Floor((double)reOpenMapIDs[i] / 10.0);
					int num2 = reOpenMapIDs[i] % 10;
					if (base.Map.AreaId != num && num2 == 1)
					{
						TrophyUtil.__tmp_area_reopen__ = true;
					}
				}
			}
			return _cache_result;
		}

		public override bool SendOffEscapes()
		{
			ShipModel[] escapeCandidate = GetEscapeCandidate();
			if (escapeCandidate.Length != 2)
			{
				return false;
			}
			ShipModel escapeShip = escapeCandidate[1];
			ShipModel towShip = escapeCandidate[0];
			if (escapeShip == null || towShip == null)
			{
				return false;
			}
			bool flag = _reqBattle.GoBackPort(escapeShip.MemId, towShip.MemId);
			if (flag)
			{
				_ships_f.Find((ShipModel_BattleAll item) => item != null && item.TmpId == towShip.MemId)?.__UpdateEscapeStatus__(value: true);
				_ships_f.Find((ShipModel_BattleAll item) => item != null && item.TmpId == escapeShip.MemId)?.__UpdateEscapeStatus__(value: true);
			}
			return flag;
		}

		protected override BattleResultFmt _GetBattleResult()
		{
			if (_cache_result_fmt == null)
			{
				Api_Result<BattleResultFmt> api_Result = _reqBattle.BattleResult();
				if (api_Result.state == Api_Result_State.Success)
				{
					_cache_result_fmt = api_Result.data;
				}
			}
			return _cache_result_fmt;
		}
	}
}
