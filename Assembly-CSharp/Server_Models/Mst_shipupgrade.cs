using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipupgrade : Model_Base
	{
		private int _id;

		private int _original_ship_id;

		private int _upgrade_type;

		private int _upgrade_level;

		private int _drawing_count;

		private static string _tableName = "mst_shipupgrade";

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

		public int Original_ship_id
		{
			get
			{
				return _original_ship_id;
			}
			private set
			{
				_original_ship_id = value;
			}
		}

		public int Upgrade_type
		{
			get
			{
				return _upgrade_type;
			}
			private set
			{
				_upgrade_type = value;
			}
		}

		public int Upgrade_level
		{
			get
			{
				return _upgrade_level;
			}
			private set
			{
				_upgrade_level = value;
			}
		}

		public int Drawing_count
		{
			get
			{
				return _drawing_count;
			}
			private set
			{
				_drawing_count = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Original_ship_id = int.Parse(element.Element("Original_ship_id").Value);
			Upgrade_type = int.Parse(element.Element("Upgrade_type").Value);
			Upgrade_level = int.Parse(element.Element("Upgrade_level").Value);
			Drawing_count = int.Parse(element.Element("Drawing_count").Value);
		}
	}
}
