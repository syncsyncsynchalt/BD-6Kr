using Server_Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_furniture : Model_Base
	{
		private int _id;

		private int _type;

		private int _no;

		private string _title;

		private string _description;

		private int _rarity;

		private int _price;

		private int _season;

		private int _sale_from;

		private int _sale_to;

		private static string _tableName = "mst_furniture";

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

		public string Title
		{
			get
			{
				return _title;
			}
			private set
			{
				_title = value;
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

		public int Rarity
		{
			get
			{
				return _rarity;
			}
			private set
			{
				_rarity = value;
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

		public int Season
		{
			get
			{
				return _season;
			}
			private set
			{
				_season = value;
			}
		}

		public int Sale_from
		{
			get
			{
				return _sale_from;
			}
			private set
			{
				_sale_from = value;
			}
		}

		public int Sale_to
		{
			get
			{
				return _sale_to;
			}
			private set
			{
				_sale_to = value;
			}
		}

		public int Saleflg => IsSale(Comm_UserDatas.Instance.User_turn.GetDateTime().Month);

		public static string tableName => _tableName;

		public static List<Mst_furniture> getSaleFurnitureList()
		{
			int month = Comm_UserDatas.Instance.User_turn.GetDateTime().Month;
			return (from data in Mst_DataManager.Instance.Mst_furniture.Values
				where data.IsSale(month) == 1
				orderby data.Id
				select data).ToList();
		}

		public int IsSale(int nowMonth)
		{
			int result = 0;
			if (Sale_from > Sale_to)
			{
				result = ((Sale_from <= nowMonth || Sale_to >= nowMonth) ? 1 : 0);
			}
			else if (nowMonth >= Sale_from && nowMonth <= Sale_to)
			{
				result = 1;
			}
			return result;
		}

		public bool IsRequireWorker()
		{
			return Price >= 2000 && Price < 20000;
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Type = int.Parse(element.Element("Type").Value);
			No = int.Parse(element.Element("No").Value);
			Title = element.Element("Title").Value;
			Description = string.Empty;
			Rarity = int.Parse(element.Element("Rarity").Value);
			Price = int.Parse(element.Element("Price").Value);
			Season = int.Parse(element.Element("Season").Value);
			Sale_from = int.Parse(element.Element("Sale_from").Value);
			Sale_to = int.Parse(element.Element("Sale_to").Value);
		}
	}
}
