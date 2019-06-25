using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_equip_ship : Model_Base
	{
		private int _id;

		private List<int> _equip;

		private static string _tableName = "mst_equip_ship";

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

		public List<int> Equip
		{
			get
			{
				return _equip;
			}
			private set
			{
				_equip = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Ship_id").Value);
			Equip = Array.ConvertAll(element.Element("Equip_type").Value.Split(','), (string x) => int.Parse(x)).ToList();
		}
	}
}
