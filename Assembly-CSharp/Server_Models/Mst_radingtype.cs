using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_radingtype : Model_Base
	{
		private int _difficult;

		private int _turn_from;

		private int _turn_to;

		private int _rading_type;

		private int _rading_rate;

		private static string _tableName = "mst_radingtype";

		public int Difficult
		{
			get
			{
				return _difficult;
			}
			private set
			{
				_difficult = value;
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

		public int Rading_type
		{
			get
			{
				return _rading_type;
			}
			private set
			{
				_rading_type = value;
			}
		}

		public int Rading_rate
		{
			get
			{
				return _rading_rate;
			}
			private set
			{
				_rading_rate = value;
			}
		}

		public static string tableName => _tableName;

		public static Mst_radingtype GetRadingRecord(List<Mst_radingtype> types, int nowTurn)
		{
			foreach (Mst_radingtype type in types)
			{
				if (type.Turn_from <= nowTurn)
				{
					return type;
				}
			}
			return null;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Difficult = int.Parse(element.Element("Difficult").Value);
			string value2 = element.Element("Turn_value").Value;
			int[] array = Array.ConvertAll(value2.Split(c), (string value) => int.Parse(value));
			Turn_from = array[0];
			Turn_to = array[1];
			Rading_type = int.Parse(element.Element("Rading_type").Value);
			Rading_rate = int.Parse(element.Element("Rading_rate").Value);
		}
	}
}
