using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_rebellionpoint : Model_Base
	{
		private int _id;

		private int _turn_from;

		private int _turn_to;

		private Dictionary<int, int> _area_value;

		private static string _tableName = "mst_rebellionpoint";

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

		public int Turn_from
		{
			get
			{
				return _turn_from;
			}
			private set
			{
				_turn_from = value;
			}
		}

		public int Turn_to
		{
			get
			{
				return _turn_to;
			}
			private set
			{
				_turn_to = value;
			}
		}

		public Dictionary<int, int> Area_value
		{
			get
			{
				return _area_value;
			}
			private set
			{
				_area_value = value;
			}
		}

		public static string tableName => _tableName;

		public Mst_rebellionpoint()
		{
			Area_value = new Dictionary<int, int>();
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Id = int.Parse(element.Element("Id").Value);
			Turn_from = int.Parse(element.Element("Turn_from").Value);
			Turn_to = int.Parse(element.Element("Turn_to").Value);
			string[] array = element.Element("Area_value").Value.Split(c);
			int[] array2 = Array.ConvertAll(array, (string x) => int.Parse(x));
			for (int i = 0; i < array2.Length; i++)
			{
				Area_value.Add(i + 1, array2[i]);
			}
		}
	}
}
