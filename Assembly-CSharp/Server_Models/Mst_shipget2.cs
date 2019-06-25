using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipget2 : Model_Base
	{
		private int _id;

		private int _type;

		private int _maparea_id;

		private int _mapinfo_no;

		private int _ship_id;

		private static string _tableName = "mst_shipget2";

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

		public int Type
		{
			get
			{
				return _type;
			}
			private set
			{
				_type = value;
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

		public int Ship_id
		{
			get
			{
				return _ship_id;
			}
			private set
			{
				_ship_id = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Type = int.Parse(element.Element("Type").Value);
			Maparea_id = int.Parse(element.Element("Maparea_id").Value);
			Mapinfo_no = int.Parse(element.Element("Mapinfo_no").Value);
			Ship_id = int.Parse(element.Element("Ship_id").Value);
		}
	}
}
