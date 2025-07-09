using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("AirBattle")]
	public class AirBattle
	{
		[XmlElement("StageFlag")]
		public bool[] StageFlag;

		[XmlElement("F_PlaneFrom")]
		public List<int> F_PlaneFrom;

		[XmlElement("E_PlaneFrom")]
		public List<int> E_PlaneFrom;

		[XmlElement("Air1")]
		public AirBattle1 Air1;

		[XmlElement("Air2")]
		public AirBattle2 Air2;

		[XmlElement("Air3")]
		public AirBattle3 Air3;

		public AirBattle()
		{
			StageFlag = new bool[3];
			F_PlaneFrom = new List<int>();
			E_PlaneFrom = new List<int>();
		}

		public void SetStageFlag()
		{
			StageFlag[0] = ((Air1 != null) ? true : false);
			StageFlag[1] = ((Air2 != null) ? true : false);
			StageFlag[2] = ((Air3 != null) ? true : false);
		}
	}
}
