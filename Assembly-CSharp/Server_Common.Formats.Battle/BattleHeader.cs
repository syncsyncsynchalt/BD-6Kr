using Server_Models;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("BattleHeader")]
	public class BattleHeader
	{
		[XmlElement("F_DeckShip1")]
		public BattleHeaderItem F_DeckShip1;

		[XmlElement("E_DeckShip1")]
		public BattleHeaderItem E_DeckShip1;

		[XmlIgnore]
		public Dictionary<int, List<Mst_slotitem>> UseRationShips;

		public BattleHeader()
		{
		}

		public BattleHeader(BattleHeaderItem F_DeckShip, BattleHeaderItem E_DeckShip)
		{
			F_DeckShip1 = F_DeckShip;
			E_DeckShip1 = E_DeckShip;
		}
	}
}
