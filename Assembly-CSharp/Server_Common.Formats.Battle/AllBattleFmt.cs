using Common.Enum;

namespace Server_Common.Formats.Battle
{
	public class AllBattleFmt
	{
		public BattleFormationKinds1[] Formation;

		public BattleFormationKinds2 BattleFormation;

		public DayBattleFmt DayBattle;

		public NightBattleFmt NightBattle;

		private AllBattleFmt()
		{
			Formation = new BattleFormationKinds1[2];
		}

		public AllBattleFmt(BattleFormationKinds1 fFormation, BattleFormationKinds1 eFormation, BattleFormationKinds2 battleFormation)
			: this()
		{
			Formation[0] = fFormation;
			Formation[1] = eFormation;
			BattleFormation = battleFormation;
		}
	}
}
