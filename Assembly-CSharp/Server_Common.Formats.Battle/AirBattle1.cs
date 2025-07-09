using Common.Enum;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("AirBattle1")]
	public class AirBattle1
	{
		[XmlElement("F_LostInfo")]
		public LostPlaneInfo F_LostInfo;

		[XmlElement("F_TouchPlane")]
		public int F_TouchPlane;

		[XmlElement("E_LostInfo")]
		public LostPlaneInfo E_LostInfo;

		[XmlElement("E_TouchPlane")]
		public int E_TouchPlane;

		[XmlElement("SeikuKind")]
		public BattleSeikuKinds SeikuKind;

		public AirBattle1()
		{
			F_LostInfo = new LostPlaneInfo();
			E_LostInfo = new LostPlaneInfo();
			SeikuKind = BattleSeikuKinds.None;
			F_TouchPlane = 0;
			E_TouchPlane = 0;
		}
	}
}
