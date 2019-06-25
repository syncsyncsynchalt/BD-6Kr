using Common.Enum;
using local.models;
using local.models.battle;
using Server_Controllers;

namespace local.managers
{
	public class RebellionBattleManager : SortieBattleManagerBase
	{
		public RebellionBattleManager(string enemy_deck_name)
			: base(enemy_deck_name)
		{
		}

		public virtual void __Init__(Api_req_SortieBattle reqBattle, enumMapWarType warType, BattleFormationKinds1 formationId, MapModel map, bool lastCell, bool isBoss, bool changeableDeck)
		{
			_changeable_deck = changeableDeck;
			base.__Init__(reqBattle, warType, formationId, map, null, lastCell, isBoss);
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
