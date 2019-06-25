namespace Server_Models
{
	public class PayItemEffectInfo
	{
		public int Type;

		public int MstId;

		public int Count;

		public PayItemEffectInfo(int[] itemData)
		{
			Type = itemData[0];
			MstId = itemData[1];
			Count = itemData[2];
		}
	}
}
