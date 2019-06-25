using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipgraphbattle : Model_Base
	{
		private int _id;

		private int _foot_x;

		private int _foot_y;

		private int _foot_d_x;

		private int _foot_d_y;

		private int _pog_x;

		private int _pog_y;

		private int _pog_d_x;

		private int _pog_d_y;

		private int _pog_sp_x;

		private int _pog_sp_y;

		private int _pog_sp_d_x;

		private int _pog_sp_d_y;

		private int _pog_sp_ensyu_x;

		private int _pog_sp_ensyu_y;

		private int _pog_sp_ensyu_d_x;

		private int _pog_sp_ensyu_d_y;

		private int _cutin_x;

		private int _cutin_y;

		private int _cutin_d_x;

		private int _cutin_d_y;

		private int _cutin_sp1_x;

		private int _cutin_sp1_y;

		private int _cutin_sp1_d_x;

		private int _cutin_sp1_d_y;

		private double _scale_mag;

		private static string _tableName = "mst_shipgraphbattle";

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

		public int Foot_x
		{
			get
			{
				return _foot_x;
			}
			private set
			{
				_foot_x = value;
			}
		}

		public int Foot_y
		{
			get
			{
				return _foot_y;
			}
			private set
			{
				_foot_y = value;
			}
		}

		public int Foot_d_x
		{
			get
			{
				return _foot_d_x;
			}
			private set
			{
				_foot_d_x = value;
			}
		}

		public int Foot_d_y
		{
			get
			{
				return _foot_d_y;
			}
			private set
			{
				_foot_d_y = value;
			}
		}

		public int Pog_x
		{
			get
			{
				return _pog_x;
			}
			private set
			{
				_pog_x = value;
			}
		}

		public int Pog_y
		{
			get
			{
				return _pog_y;
			}
			private set
			{
				_pog_y = value;
			}
		}

		public int Pog_d_x
		{
			get
			{
				return _pog_d_x;
			}
			private set
			{
				_pog_d_x = value;
			}
		}

		public int Pog_d_y
		{
			get
			{
				return _pog_d_y;
			}
			private set
			{
				_pog_d_y = value;
			}
		}

		public int Pog_sp_x
		{
			get
			{
				return _pog_sp_x;
			}
			private set
			{
				_pog_sp_x = value;
			}
		}

		public int Pog_sp_y
		{
			get
			{
				return _pog_sp_y;
			}
			private set
			{
				_pog_sp_y = value;
			}
		}

		public int Pog_sp_d_x
		{
			get
			{
				return _pog_sp_d_x;
			}
			private set
			{
				_pog_sp_d_x = value;
			}
		}

		public int Pog_sp_d_y
		{
			get
			{
				return _pog_sp_d_y;
			}
			private set
			{
				_pog_sp_d_y = value;
			}
		}

		public int Pog_sp_ensyu_x
		{
			get
			{
				return _pog_sp_ensyu_x;
			}
			private set
			{
				_pog_sp_ensyu_x = value;
			}
		}

		public int Pog_sp_ensyu_y
		{
			get
			{
				return _pog_sp_ensyu_y;
			}
			private set
			{
				_pog_sp_ensyu_y = value;
			}
		}

		public int Pog_sp_ensyu_d_x
		{
			get
			{
				return _pog_sp_ensyu_d_x;
			}
			private set
			{
				_pog_sp_ensyu_d_x = value;
			}
		}

		public int Pog_sp_ensyu_d_y
		{
			get
			{
				return _pog_sp_ensyu_d_y;
			}
			private set
			{
				_pog_sp_ensyu_d_y = value;
			}
		}

		public int Cutin_x
		{
			get
			{
				return _cutin_x;
			}
			private set
			{
				_cutin_x = value;
			}
		}

		public int Cutin_y
		{
			get
			{
				return _cutin_y;
			}
			private set
			{
				_cutin_y = value;
			}
		}

		public int Cutin_d_x
		{
			get
			{
				return _cutin_d_x;
			}
			private set
			{
				_cutin_d_x = value;
			}
		}

		public int Cutin_d_y
		{
			get
			{
				return _cutin_d_y;
			}
			private set
			{
				_cutin_d_y = value;
			}
		}

		public int Cutin_sp1_x
		{
			get
			{
				return _cutin_sp1_x;
			}
			private set
			{
				_cutin_sp1_x = value;
			}
		}

		public int Cutin_sp1_y
		{
			get
			{
				return _cutin_sp1_y;
			}
			private set
			{
				_cutin_sp1_y = value;
			}
		}

		public int Cutin_sp1_d_x
		{
			get
			{
				return _cutin_sp1_d_x;
			}
			private set
			{
				_cutin_sp1_d_x = value;
			}
		}

		public int Cutin_sp1_d_y
		{
			get
			{
				return _cutin_sp1_d_y;
			}
			private set
			{
				_cutin_sp1_d_y = value;
			}
		}

		public double Scale_mag
		{
			get
			{
				return _scale_mag;
			}
			private set
			{
				_scale_mag = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			char c = ',';
			if (element.Element("BattlePos") != null)
			{
				string[] array = element.Element("BattlePos").Value.Split(c);
				string text = array[24];
				array[24] = "0";
				int[] array2 = Array.ConvertAll(array, (string x) => int.Parse(x));
				Foot_x = array2[0];
				Foot_y = array2[1];
				Foot_d_x = array2[2];
				Foot_d_y = array2[3];
				Pog_x = array2[4];
				Pog_y = array2[5];
				Pog_d_x = array2[6];
				Pog_d_y = array2[7];
				Pog_sp_x = array2[8];
				Pog_sp_y = array2[9];
				Pog_sp_d_x = array2[10];
				Pog_sp_d_y = array2[11];
				Pog_sp_ensyu_x = array2[12];
				Pog_sp_ensyu_y = array2[13];
				Pog_sp_ensyu_d_x = array2[14];
				Pog_sp_ensyu_d_y = array2[15];
				Cutin_x = array2[16];
				Cutin_y = array2[17];
				Cutin_d_x = array2[18];
				Cutin_d_y = array2[19];
				Cutin_sp1_x = array2[20];
				Cutin_sp1_y = array2[21];
				Cutin_sp1_d_x = array2[22];
				Cutin_sp1_d_y = array2[23];
				if (text != string.Empty)
				{
					Scale_mag = double.Parse(text);
				}
			}
		}
	}
}
