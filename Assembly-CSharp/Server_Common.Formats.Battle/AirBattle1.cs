using Common.Enum;

namespace Server_Common.Formats.Battle
{
	public class AirBattle1
	{
		public LostPlaneInfo F_LostInfo;

		public int F_TouchPlane;

		public LostPlaneInfo E_LostInfo;

		public int E_TouchPlane;

		public BattleSeikuKinds SeikuKind;

		public AirBattle1()
		{
			F_LostInfo = new LostPlaneInfo();
			E_LostInfo = new LostPlaneInfo();
			SeikuKind = BattleSeikuKinds.None;
			F_TouchPlane = 0;
			E_TouchPlane = 0;
		}
	}
}
