using Server_Models;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("NightBattleFmt")]
	public class NightBattleFmt
	{
		[XmlElement("Header")]
		public BattleHeader Header;

		[XmlElement("F_TouchPlane")]
		public int F_TouchPlane;

		[XmlElement("E_TouchPlane")]
		public int E_TouchPlane;

		[XmlElement("F_FlareId")]
		public int F_FlareId;

		[XmlElement("E_FlareId")]
		public int E_FlareId;

		[XmlElement("F_SearchId")]
		public int F_SearchId;

		[XmlElement("E_SearchId")]
		public int E_SearchId;

		[XmlElement("Hougeki")]
		public List<Hougeki<BattleAtackKinds_Night>> Hougeki;

		public NightBattleFmt()
		{
		}

		public NightBattleFmt(int deck_id, List<Mem_ship> myShip, List<Mem_ship> enemyShip)
		{
			BattleHeaderItem f_DeckShip = new BattleHeaderItem(deck_id, myShip);
			BattleHeaderItem e_DeckShip = new BattleHeaderItem(0, enemyShip);
			Header = new BattleHeader(f_DeckShip, e_DeckShip);
			Hougeki = new List<Hougeki<BattleAtackKinds_Night>>();
		}
	}
}
