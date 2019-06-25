using local.models.battle;

namespace local.managers
{
	public class SortieBattleManager : SortieBattleManagerBase
	{
		public SortieBattleManager(string enemy_deck_name)
			: base(enemy_deck_name)
		{
		}

		public override CommandPhaseModel GetCommandPhaseModel()
		{
			if (_cache_command == null && IsExistCommandPhase())
			{
				_cache_command = new __CommandPhaseModel_Sortie__(this, _reqBattle);
			}
			return _cache_command;
		}
	}
}
