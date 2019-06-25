namespace Server_Common.Formats
{
	public class User_MapinfoFmt
	{
		public enum enumExBossType
		{
			Normal,
			Defeat,
			MapHp
		}

		public int Id;

		public bool Cleared;

		public enumExBossType Boss_type;

		public EventMapInfo Eventmap;

		public int Defeat_count;

		public bool IsGo;

		public User_MapinfoFmt()
		{
			Boss_type = enumExBossType.Normal;
			Eventmap = new EventMapInfo();
			Cleared = false;
			IsGo = false;
		}
	}
}
