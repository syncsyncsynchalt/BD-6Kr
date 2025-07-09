using Common.Enum;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("SearchInfo")]
	public class SearchInfo
	{
		[XmlElement("SearchValue")]
		public BattleSearchValues SearchValue;

		[XmlElement("UsePlane")]
		public List<SearchUsePlane> UsePlane;

		public SearchInfo()
		{
		}
	}
}
