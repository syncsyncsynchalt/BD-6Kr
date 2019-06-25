using Server_Common;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_mapcomp", Namespace = "")]
	public class Mem_mapcomp : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private int _mapinfo_no;

		[DataMember]
		private HashSet<int> _no;

		private static string _tableName = "mem_mapcomp";

		public int Rid
		{
			get
			{
				return _rid;
			}
			private set
			{
				_rid = value;
			}
		}

		public int Maparea_id
		{
			get
			{
				return _maparea_id;
			}
			private set
			{
				_maparea_id = value;
			}
		}

		public int Mapinfo_no
		{
			get
			{
				return _mapinfo_no;
			}
			private set
			{
				_mapinfo_no = value;
			}
		}

		public HashSet<int> No
		{
			get
			{
				return _no;
			}
			private set
			{
				_no = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_mapcomp()
		{
			No = new HashSet<int>();
		}

		public Mem_mapcomp(int mapinfo_id, int maparea_id, int mapinfo_no)
			: this()
		{
			Rid = mapinfo_id;
			Maparea_id = maparea_id;
			Mapinfo_no = mapinfo_no;
		}

		public void Insert()
		{
			if (!Comm_UserDatas.Instance.User_mapcomp.ContainsKey(Rid))
			{
				Comm_UserDatas.Instance.User_mapcomp.Add(Rid, this);
			}
		}

		public void AddPassCell(int cell_no)
		{
			No.Add(cell_no);
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Maparea_id = int.Parse(element.Element("_maparea_id").Value);
			Mapinfo_no = int.Parse(element.Element("_mapinfo_no").Value);
			foreach (XElement item in element.Element("_no").Elements())
			{
				No.Add(int.Parse(item.Value));
			}
		}
	}
}
