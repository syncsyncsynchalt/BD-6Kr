namespace Server_Common.Formats
{
	public class EventMapInfo
	{
		public enum enumEventState
		{
			Close,
			Open,
			Clear
		}

		public int Event_hp;

		public int Event_maxhp;

		public enumEventState Event_state;

		public int Damage;

		public EventMapInfo()
		{
			Event_hp = 0;
			Event_maxhp = 0;
			Event_state = enumEventState.Close;
			Damage = 0;
		}
	}
}
