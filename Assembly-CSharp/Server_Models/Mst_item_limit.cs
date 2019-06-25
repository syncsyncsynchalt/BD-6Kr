using Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_item_limit : Model_Base
	{
		private int _id;

		private int _material_id;

		private int _useitem_id;

		private int _slotitem_id;

		private int _max_items;

		private static string _tableName = "mst_item_limit";

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

		public int Material_id
		{
			get
			{
				return _material_id;
			}
			private set
			{
				_material_id = value;
			}
		}

		public int Useitem_id
		{
			get
			{
				return _useitem_id;
			}
			private set
			{
				_useitem_id = value;
			}
		}

		public int Slotitem_id
		{
			get
			{
				return _slotitem_id;
			}
			private set
			{
				_slotitem_id = value;
			}
		}

		public int Max_items
		{
			get
			{
				return _max_items;
			}
			private set
			{
				_max_items = value;
			}
		}

		public static string tableName => _tableName;

		public int GetMaterialLimit(Dictionary<int, Mst_item_limit> mst_data, enumMaterialCategory category)
		{
			return mst_data.Values.FirstOrDefault((Mst_item_limit x) => (x.Material_id == (int)category) ? true : false)?.Max_items ?? 0;
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Material_id = int.Parse(element.Element("Material_id").Value);
			Useitem_id = int.Parse(element.Element("Useitem_id").Value);
			Slotitem_id = int.Parse(element.Element("Slotitem_id").Value);
			Max_items = int.Parse(element.Element("Max_items").Value);
		}
	}
}
