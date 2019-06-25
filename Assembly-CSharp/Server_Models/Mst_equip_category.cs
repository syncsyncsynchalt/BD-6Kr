using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_equip_category : Model_Base
	{
		private int _id;

		private SlotitemCategory _slotitem_type;

		private static string _tableName = "mst_equip_category";

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

		public SlotitemCategory Slotitem_type
		{
			get
			{
				return _slotitem_type;
			}
			private set
			{
				_slotitem_type = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Slotitem_type = (SlotitemCategory)(int)Enum.Parse(typeof(SlotitemCategory), element.Element("Slotitem_type").Value);
		}
	}
}
