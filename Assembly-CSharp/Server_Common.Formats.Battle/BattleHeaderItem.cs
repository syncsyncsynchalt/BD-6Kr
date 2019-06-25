using Server_Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server_Common.Formats.Battle
{
	[DataContract]
	public class BattleHeaderItem
	{
		[DataMember]
		public int Deck_Id;

		[DataMember]
		public BattleShipFmt[] Ships;

		public BattleHeaderItem()
		{
		}

		public BattleHeaderItem(int deck_id, List<Mem_ship> sortieShips)
		{
			Ships = new BattleShipFmt[6];
			Deck_Id = deck_id;
			for (int i = 0; i < sortieShips.Count; i++)
			{
				Ships[i] = new BattleShipFmt(sortieShips[i]);
			}
		}
	}
}
