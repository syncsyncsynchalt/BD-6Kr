using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_radingrate : Model_Base
	{
		private int _maparea_id;

		private int _rading_type;

		private int _air_rate;

		private int _air_karyoku;

		private int _submarine_rate;

		private int _submarine_karyoku;

		private static string _tableName = "mst_radingrate";

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

		public int Air_rate
		{
			get
			{
				return _air_rate;
			}
			private set
			{
				_air_rate = value;
			}
		}

		public int Air_karyoku
		{
			get
			{
				return _air_karyoku;
			}
			private set
			{
				_air_karyoku = value;
			}
		}

		public int Submarine_rate
		{
			get
			{
				return _submarine_rate;
			}
			private set
			{
				_submarine_rate = value;
			}
		}

		public int Submarine_karyoku
		{
			get
			{
				return _submarine_karyoku;
			}
			private set
			{
				_submarine_karyoku = value;
			}
		}

		public static string tableName => _tableName;

		public int[] GetRadingValues(RadingKind kind)
		{
			int[] array = new int[2];
			if (kind == RadingKind.AIR_ATTACK)
			{
				array[0] = Air_rate;
				array[1] = Air_karyoku;
			}
			else
			{
				array[0] = Submarine_rate;
				array[1] = Submarine_karyoku;
			}
			return array;
		}

		public int GetRadingRate(RadingKind kind)
		{
			return (kind != RadingKind.AIR_ATTACK) ? Submarine_rate : Air_rate;
		}

		public int GetRadingPow(RadingKind kind)
		{
			return (kind != RadingKind.AIR_ATTACK) ? Submarine_karyoku : Air_karyoku;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Maparea_id = int.Parse(element.Element("Maparea_id").Value);
			Rading_type = int.Parse(element.Element("Rading_type").Value);
			string value2 = element.Element("Air_value").Value;
			int[] array = Array.ConvertAll(value2.Split(c), (string value) => int.Parse(value));
			Air_rate = array[0];
			Air_karyoku = array[1];
			string value3 = element.Element("Submarin_value").Value;
			array = Array.ConvertAll(value3.Split(c), (string value) => int.Parse(value));
			Submarine_rate = array[0];
			Submarine_karyoku = array[1];
		}
	}
}
