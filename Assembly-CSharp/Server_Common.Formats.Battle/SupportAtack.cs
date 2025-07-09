using Common.Enum;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("SupportAtack")]
	public class SupportAtack
	{
		[XmlElement("Deck_Id")]
		public int Deck_Id;

		[XmlElement("Undressing_Flag")]
		public bool[] Undressing_Flag;

		[XmlElement("SupportType")]
		public BattleSupportKinds SupportType;

		[XmlElement("AirBattle")]
		public AirBattle AirBattle;

		[XmlElement("Hourai")]
		public Support_HouRai Hourai;

		public SupportAtack()
		{
			Undressing_Flag = new bool[6];
		}
	}
}
