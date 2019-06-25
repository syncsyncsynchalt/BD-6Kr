using Common.Enum;
using local.models;
using local.models.battle;
using Server_Controllers;

namespace local.managers
{
	public class RebellionBattleManager_Write : RebellionBattleManager
	{
		public RebellionBattleManager_Write(string enemy_deck_name)
			: base(enemy_deck_name)
		{
		}

		public override void __Init__(Api_req_SortieBattle reqBattle, enumMapWarType warType, BattleFormationKinds1 formationId, MapModel map, bool lastCell, bool isBoss, bool changeableDeck)
		{
			_changeable_deck = changeableDeck;
			base.__Init__(reqBattle, warType, formationId, map, null, lastCell, isBoss);
			switch (warType)
			{
			case enumMapWarType.Normal:
			case enumMapWarType.AirBattle:
				DebugBattleMaker.SerializeDayBattle(_battleData);
				break;
			case enumMapWarType.Midnight:
				DebugBattleMaker.SerializeNightBattle(_battleData);
				break;
			case enumMapWarType.Night_To_Day:
				DebugBattleMaker.SerializeNightBattle(_battleData);
				break;
			}
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
