using Common.Enum;
using local.models.battle;

namespace local.managers
{
	public class PracticeBattleManager_Write : PracticeBattleManager
	{
		public override void __Init__(int deck_id, int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			DebugBattleMaker.SerializeDayBattle(_battleData);
			base.__Init__(deck_id, enemy_deck_id, formation_id);
		}

		public override void StartDayToNightBattle()
		{
			base.StartDayToNightBattle();
			DebugBattleMaker.SerializeNightBattle(_battleData);
		}

		public override BattleResultModel GetBattleResult()
		{
			BattleResultModel battleResult = base.GetBattleResult();
			DebugBattleMaker.SerializeBattleResult(_cache_result_fmt);
			return battleResult;
		}
	}
}
