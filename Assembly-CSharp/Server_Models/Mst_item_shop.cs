using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_item_shop : Model_Base
	{
		private ushort _cabinet_no;

		private ushort _position_no;

		private ushort _item1_type;

		private int _item1_id;

		private ushort _item2_type;

		private int _item2_id;

		private static string _tableName = "mst_item_shop";

		public ushort Cabinet_no
		{
			get
			{
				return _cabinet_no;
			}
			private set
			{
				_cabinet_no = value;
			}
		}

		public ushort Position_no
		{
			get
			{
				return _position_no;
			}
			private set
			{
				_position_no = value;
			}
		}

		public ushort Item1_type
		{
			get
			{
				return _item1_type;
			}
			private set
			{
				_item1_type = value;
			}
		}

		public int Item1_id
		{
			get
			{
				return _item1_id;
			}
			private set
			{
				_item1_id = value;
			}
		}

		public ushort Item2_type
		{
			get
			{
				return _item2_type;
			}
			private set
			{
				_item2_type = value;
			}
		}

		public int Item2_id
		{
			get
			{
				return _item2_id;
			}
			private set
			{
				_item2_id = value;
			}
		}

		public static string tableName => _tableName;

		public bool IsChildReference()
		{
			return (Item2_type != 0) ? true : false;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Cabinet_no = ushort.Parse(element.Element("Cabinet_no").Value);
			Position_no = ushort.Parse(element.Element("Position_no").Value);
			if (element.Element("Item1") != null)
			{
				string value2 = element.Element("Item1").Value;
				int[] array = Array.ConvertAll(value2.Split(c), (string value) => int.Parse(value));
				Item1_type = (ushort)array[0];
				Item1_id = array[1];
			}
			if (element.Element("Item2") != null)
			{
				string value3 = element.Element("Item2").Value;
				int[] array2 = Array.ConvertAll(value3.Split(c), (string value) => int.Parse(value));
				Item2_type = (ushort)array2[0];
				Item2_id = array2[1];
			}
		}
	}
}
