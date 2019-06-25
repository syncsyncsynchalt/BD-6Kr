using Common.Enum;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_useitem : Model_Base
	{
		public const int __USEITEM_DOCKKEY__ = 49;

		public const int __USEITEM_KAGUSYOKUNIN__ = 52;

		public const int __USEITEM_BOKOKAKUCHO__ = 53;

		public const int __USEITEM_YUBIWA__ = 55;

		public const int __USEITEM_SEKKEISHO__ = 58;

		public const int __USEITEM_SHIREIBU__ = 63;

		private int _id;

		private int _usetype;

		private int _category;

		private string _name;

		private string _description;

		private string _description2;

		private int _price;

		private static string _tableName = "mst_useitem";

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

		public int Usetype
		{
			get
			{
				return _usetype;
			}
			private set
			{
				_usetype = value;
			}
		}

		public int Category
		{
			get
			{
				return _category;
			}
			private set
			{
				_category = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			private set
			{
				_name = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			private set
			{
				_description = value;
			}
		}

		public string Description2
		{
			get
			{
				return _description2;
			}
			private set
			{
				_description2 = value;
			}
		}

		public int Price
		{
			get
			{
				return _price;
			}
			private set
			{
				_price = value;
			}
		}

		public static string tableName => _tableName;

		public int GetItemExchangeNum(ItemExchangeKinds exchange_type)
		{
			switch (exchange_type)
			{
			case ItemExchangeKinds.NONE:
				return 1;
			case ItemExchangeKinds.PLAN:
				if (Id == 57)
				{
					return 4;
				}
				break;
			}
			if (exchange_type == ItemExchangeKinds.REMODEL && Id == 57)
			{
				return 1;
			}
			if (exchange_type == ItemExchangeKinds.PRESENT_MATERIAL && Id == 60)
			{
				return 1;
			}
			if (exchange_type == ItemExchangeKinds.PRESENT_IRAKO && Id == 60)
			{
				return 1;
			}
			return 0;
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Usetype = int.Parse(element.Element("Usetype").Value);
			Category = int.Parse(element.Element("Category").Value);
			Name = element.Element("Name").Value;
			Description = element.Element("Description").Value;
			Description2 = element.Element("Description2").Value;
			Price = int.Parse(element.Element("Price").Value);
		}
	}
}
