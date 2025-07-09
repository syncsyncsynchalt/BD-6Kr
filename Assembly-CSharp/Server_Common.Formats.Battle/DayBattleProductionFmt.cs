using Common.Enum;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("DayBattleProductionFmt")]
	public class DayBattleProductionFmt
	{
		[XmlElement("BoxNo")]
		public int BoxNo;

		[XmlElement("productionKind")]
		public BattleCommand productionKind;

		[XmlElement("Withdrawal")]
		public bool Withdrawal;

		[XmlElement("FSPP")]
		public int FSPP;

		[XmlElement("RSPP")]
		public int RSPP;

		[XmlElement("TSPP")]
		public int TSPP;
	}
}
