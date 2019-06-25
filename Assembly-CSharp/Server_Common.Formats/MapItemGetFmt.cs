namespace Server_Common.Formats
{
	public class MapItemGetFmt
	{
		public enum enumCategory
		{
			None,
			Furniture,
			Slotitem,
			Ship,
			Material,
			UseItem
		}

		public enumCategory Category;

		public int Id;

		public int GetCount;
	}
}
