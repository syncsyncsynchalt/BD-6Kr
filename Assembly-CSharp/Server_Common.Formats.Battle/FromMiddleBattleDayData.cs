using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("FromMiddleBattleDayData")]
	public class FromMiddleBattleDayData
	{
		[XmlElement("Production")]
		public DayBattleProductionFmt Production;

		[XmlIgnore]
		public IBattleType F_BattleData;

		[XmlIgnore]
		public IBattleType E_BattleData;
	}
}
