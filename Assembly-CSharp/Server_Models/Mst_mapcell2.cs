using Common.Enum;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapcell2 : Model_Base
	{
		private int _id;

		private int _map_no;

		private int _maparea_id;

		private int _mapinfo_no;

		private int _no;

		private int _color_no;

		private string _link_no;

		private enumMapEventType _event_1;

		private enumMapWarType _event_2;

		private int _event_point_1;

		private int _event_point_2;

		private int _table_no1;

		private int _table_no2;

		private int _next_no_1;

		private int _next_no_2;

		private int _next_no_3;

		private int _next_no_4;

		private string _next_rate;

		private string _next_rate_req;

		private int _req_ship_count;

		private string _req_shiptype;

		private int _item_no;

		private int _item_count;

		private static string _tableName = "mst_mapcell2";

		public int Id
		{
			get
			{
				return _id;
			}
			private set
			{
				_id = value;
			}
		}

		public int Map_no
		{
			get
			{
				return _map_no;
			}
			private set
			{
				_map_no = value;
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

		public int No
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

		public int Color_no
		{
			get
			{
				return _color_no;
			}
			private set
			{
				_color_no = value;
			}
		}

		public string Link_no
		{
			get
			{
				return _link_no;
			}
			private set
			{
				_link_no = value;
			}
		}

		public enumMapEventType Event_1
		{
			get
			{
				return _event_1;
			}
			private set
			{
				_event_1 = value;
			}
		}

		public enumMapWarType Event_2
		{
			get
			{
				return _event_2;
			}
			private set
			{
				_event_2 = value;
			}
		}

		public int Event_point_1
		{
			get
			{
				return _event_point_1;
			}
			private set
			{
				_event_point_1 = value;
			}
		}

		public int Event_point_2
		{
			get
			{
				return _event_point_2;
			}
			private set
			{
				_event_point_2 = value;
			}
		}

		public int Table_no1
		{
			get
			{
				return _table_no1;
			}
			private set
			{
				_table_no1 = value;
			}
		}

		public int Table_no2
		{
			get
			{
				return _table_no2;
			}
			private set
			{
				_table_no2 = value;
			}
		}

		public int Next_no_1
		{
			get
			{
				return _next_no_1;
			}
			private set
			{
				_next_no_1 = value;
			}
		}

		public int Next_no_2
		{
			get
			{
				return _next_no_2;
			}
			private set
			{
				_next_no_2 = value;
			}
		}

		public int Next_no_3
		{
			get
			{
				return _next_no_3;
			}
			private set
			{
				_next_no_3 = value;
			}
		}

		public int Next_no_4
		{
			get
			{
				return _next_no_4;
			}
			private set
			{
				_next_no_4 = value;
			}
		}

		public string Next_rate
		{
			get
			{
				return _next_rate;
			}
			private set
			{
				_next_rate = value;
			}
		}

		public string Next_rate_req
		{
			get
			{
				return _next_rate_req;
			}
			private set
			{
				_next_rate_req = value;
			}
		}

		public int Req_ship_count
		{
			get
			{
				return _req_ship_count;
			}
			private set
			{
				_req_ship_count = value;
			}
		}

		public string Req_shiptype
		{
			get
			{
				return _req_shiptype;
			}
			private set
			{
				_req_shiptype = value;
			}
		}

		public int Item_no
		{
			get
			{
				return _item_no;
			}
			private set
			{
				_item_no = value;
			}
		}

		public int Item_count
		{
			get
			{
				return _item_count;
			}
			private set
			{
				_item_count = value;
			}
		}

		public static string tableName => _tableName;

		public List<int> GetLinkNo()
		{
			if (string.IsNullOrEmpty(Link_no))
			{
				return new List<int>();
			}
			string[] array = Link_no.Split(',');
			return new List<int>(Array.ConvertAll(array, (string x) => int.Parse(x)));
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Map_no = int.Parse(element.Element("Map_no").Value);
			Maparea_id = int.Parse(element.Element("Maparea_id").Value);
			Mapinfo_no = int.Parse(element.Element("Mapinfo_no").Value);
			No = int.Parse(element.Element("No").Value);
			Color_no = int.Parse(element.Element("Color_no").Value);
			Link_no = element.Element("Link_no").Value;
			Event_1 = (enumMapEventType)int.Parse(element.Element("Event_1").Value);
			Event_2 = (enumMapWarType)int.Parse(element.Element("Event_2").Value);
			Event_point_1 = int.Parse(element.Element("Event_point_1").Value);
			Event_point_2 = int.Parse(element.Element("Event_point_2").Value);
			Table_no1 = int.Parse(element.Element("Table_no1").Value);
			Table_no2 = int.Parse(element.Element("Table_no2").Value);
			Next_no_1 = int.Parse(element.Element("Next_no_1").Value);
			Next_no_2 = int.Parse(element.Element("Next_no_2").Value);
			Next_no_3 = int.Parse(element.Element("Next_no_3").Value);
			Next_no_4 = int.Parse(element.Element("Next_no_4").Value);
			Next_rate = element.Element("Next_rate").Value;
			Next_rate_req = element.Element("Next_rate_req").Value;
			Req_ship_count = int.Parse(element.Element("Req_ship_count").Value);
			Req_shiptype = element.Element("Req_shiptype").Value;
			Item_no = int.Parse(element.Element("Item_no").Value);
			Item_count = int.Parse(element.Element("Item_count").Value);
			disableEnemyToNonActiveArea();
		}

		public bool IsNext()
		{
			return (Next_no_1 > 0 || Next_no_2 > 0 || Next_no_3 > 0 || Next_no_4 > 0) ? true : false;
		}

		private void disableEnemyToNonActiveArea()
		{
			if (Event_1 == enumMapEventType.War_Normal)
			{
				HashSet<int> hashSet = new HashSet<int>();
				if (hashSet.Contains(Maparea_id))
				{
					Event_1 = enumMapEventType.Stupid;
					Event_2 = enumMapWarType.None;
				}
			}
		}
	}
}
