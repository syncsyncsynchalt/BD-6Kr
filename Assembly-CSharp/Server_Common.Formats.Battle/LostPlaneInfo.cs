using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("LostPlaneInfo")]
	public class LostPlaneInfo
	{
		[XmlElement("Count")]
		public int Count;

		[XmlElement("LostCount")]
		public int LostCount;

		public LostPlaneInfo()
		{
			Count = 0;
			LostCount = 0;
		}
	}
}
