using Common.Enum;
using Server_Common.Formats.Battle;

namespace local.managers
{
	public class PracticeBattleManager_Read : PracticeBattleManager, IBattleManager_Read
	{
		private AllBattleFmt _tmp_day;

		private AllBattleFmt _tmp_night;

		private BattleResultFmt _tmp_result;

		public PracticeBattleManager_Read(int deck_id, int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			_enemy_deck_id = enemy_deck_id;
			_war_type = enumMapWarType.Normal;
			_is_boss = false;
			DebugBattleMaker.LoadBattleData(out _tmp_day, out _tmp_night, out _tmp_result);
			_phase = CombatPhase.DAY;
			_battleData = _tmp_day;
			BattleHeader header = _battleData.DayBattle.Header;
			_ships_f = _CreateShipData_f(header, practice: false);
			_ships_e = _CreateShipData_e(header, practice: false);
		}

		public override bool HasNightBattle()
		{
			if (_tmp_night == null)
			{
				return false;
			}
			return base.HasNightBattle();
		}

		public override void StartDayToNightBattle()
		{
			_battleData = _tmp_night;
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
		}

		protected override BattleResultFmt _GetBattleResult()
		{
			if (_cache_result_fmt == null)
			{
				_cache_result_fmt = _tmp_result;
			}
			return _cache_result_fmt;
		}
	}
}
