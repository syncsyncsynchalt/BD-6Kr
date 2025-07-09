using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("AirBattle2")]
	public class AirBattle2
	{
		[XmlElement("F_AntiFire")]
		public AirFireInfo F_AntiFire;

		[XmlElement("E_AntiFire")]
		public AirFireInfo E_AntiFire;

		[XmlElement("F_LostInfo")]
		public LostPlaneInfo F_LostInfo;

		[XmlElement("E_LostInfo")]
		public LostPlaneInfo E_LostInfo;

		public AirBattle2()
		{
			F_LostInfo = new LostPlaneInfo();
			E_LostInfo = new LostPlaneInfo();
		}
	}
}
