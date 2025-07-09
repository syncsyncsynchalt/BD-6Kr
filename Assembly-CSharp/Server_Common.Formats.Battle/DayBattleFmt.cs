using Server_Models;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("DayBattleFmt")]
	public class DayBattleFmt
	{
		[XmlElement("Header")]
		public BattleHeader Header;

		[XmlElement("Search")]
		public SearchInfo[] Search;

		[XmlElement("OpeningProduction")]
		public DayBattleProductionFmt OpeningProduction;

		[XmlElement("AirBattle")]
		public AirBattle AirBattle;

		[XmlElement("AirBattle2")]
		public AirBattle AirBattle2;

		[XmlElement("SupportAtack")]
		public SupportAtack SupportAtack;

		[XmlElement("OpeningAtack")]
		public Raigeki OpeningAtack;

		[XmlElement("FromMiddleDayBattle")]
		public List<FromMiddleBattleDayData> FromMiddleDayBattle;

		[XmlElement("Raigeki")]
		public Raigeki Raigeki;

		[XmlElement("ValidMidnight")]
		public bool ValidMidnight;

		public DayBattleFmt()
		{
		}

		public DayBattleFmt(int deck_id, List<Mem_ship> myShip, List<Mem_ship> enemyShip)
		{
			ValidMidnight = false;
			Search = new SearchInfo[2];
			BattleHeaderItem f_DeckShip = new BattleHeaderItem(deck_id, myShip);
			BattleHeaderItem e_DeckShip = new BattleHeaderItem(0, enemyShip);
			Header = new BattleHeader(f_DeckShip, e_DeckShip);
		}
	}
}
