using Common.Enum;
using local.models;
using local.models.battle;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers;
using System.Collections.Generic;

namespace local.managers
{
	public class PracticeBattleManager : BattleManager
	{
		private Api_req_PracticeBattle _reqBattle;

		private BattleResultModel _cache_result;

		public override string EnemyDeckName => base.UserInfo.GetDeck(base.EnemyDeckId).Name;

		public virtual void __Init__(int deck_id, int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			_recovery_item_use_count_at_start = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			_enemy_deck_id = enemy_deck_id;
			_war_type = enumMapWarType.Normal;
			_is_boss = false;
			_last_cell = true;
			_reqBattle = new Api_req_PracticeBattle(deck_id, enemy_deck_id);
			_battleData = _reqBattle.GetDayPreBattleInfo(formation_id).data;
			_phase = CombatPhase.DAY;
			BattleHeader header = _battleData.DayBattle.Header;
			_ships_f = _CreateShipData_f(header, practice: true);
			_ships_e = _CreateShipData_e(header, practice: true);
			__createCacheDataBeforeCommand__();
		}

		public override CommandPhaseModel GetCommandPhaseModel()
		{
			if (_cache_command == null && IsExistCommandPhase())
			{
				_cache_command = new __CommandPhaseModel_Prac__(this, _reqBattle);
			}
			return _cache_command;
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
			Dictionary<int, int> dic = new Dictionary<int, int>();
			DeckModel deck = base.UserInfo.GetDeck(base.DeckId);
			deck.__CreateShipExpRatesDictionary__(ref dic);
			deck = base.UserInfo.GetDeck(base.EnemyDeckId);
			deck.__CreateShipExpRatesDictionary__(ref dic);
			BattleResultFmt battleResultFmt = _GetBattleResult();
			if (battleResultFmt == null)
			{
				return null;
			}
			_cache_result = new BattleResultModel(base.DeckId, base.EnemyDeckId, this, battleResultFmt, _ships_f, _ships_e, dic);
			return _cache_result;
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
