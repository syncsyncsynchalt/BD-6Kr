namespace Server_Common.Formats.Battle
{
	public class AirBattle2
	{
		public AirFireInfo F_AntiFire;

		public AirFireInfo E_AntiFire;

		public LostPlaneInfo F_LostInfo;

		public LostPlaneInfo E_LostInfo;

		public AirBattle2()
		{
			F_LostInfo = new LostPlaneInfo();
			E_LostInfo = new LostPlaneInfo();
		}
	}
}
