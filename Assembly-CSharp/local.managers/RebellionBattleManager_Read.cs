using Common.Enum;
using local.models;
using Server_Common.Formats.Battle;

namespace local.managers
{
	public class RebellionBattleManager_Read : RebellionBattleManager, IBattleManager_Read
	{
		private AllBattleFmt _tmp_day;

		private AllBattleFmt _tmp_night;

		private BattleResultFmt _tmp_result;

		public RebellionBattleManager_Read(bool is_boss, bool last_cell, MapModel map)
			: base(string.Empty)
		{
			_enemy_deck_id = -1;
			_is_boss = is_boss;
			_last_cell = last_cell;
			_map = map;
			DebugBattleMaker.LoadBattleData(out _tmp_day, out _tmp_night, out _tmp_result);
			BattleHeader header;
			if (_tmp_day == null)
			{
				_war_type = enumMapWarType.Midnight;
				_phase = CombatPhase.NIGHT;
				_battleData = _tmp_night;
				header = _battleData.NightBattle.Header;
			}
			else
			{
				_war_type = enumMapWarType.Normal;
				_phase = CombatPhase.DAY;
				_battleData = _tmp_day;
				header = _battleData.DayBattle.Header;
				if (_battleData.DayBattle.AirBattle2 != null)
				{
					_war_type = enumMapWarType.AirBattle;
				}
			}
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
