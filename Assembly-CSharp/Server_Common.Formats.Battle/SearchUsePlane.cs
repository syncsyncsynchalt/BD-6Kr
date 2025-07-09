using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	[Serializable]
	[XmlRoot("SearchUsePlane")]
	public class SearchUsePlane
	{
		[XmlElement("Rid")]
		public int Rid;

		[XmlElement("MstIds")]
		public List<int> MstIds;

		public SearchUsePlane()
		{
		}

		public SearchUsePlane(int rid, List<int> mst_ids)
		{
			Rid = rid;
			MstIds = mst_ids;
		}
	}
}
