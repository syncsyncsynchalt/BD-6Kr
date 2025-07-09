using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("Raigeki")]
	public class Raigeki : IBattleType
	{
		private int fmtType;

		[XmlElement("F_Rai")]
		public RaigekiInfo F_Rai;

		[XmlElement("E_Rai")]
		public RaigekiInfo E_Rai;

		public int FmtType => fmtType;

		public Raigeki()
		{
			fmtType = 2;
		}
	}
}
