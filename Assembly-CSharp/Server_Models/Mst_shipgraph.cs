using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipgraph : Model_Base
	{
		private int _id;

		private int _sortno;

		private int _boko_n_x;

		private int _boko_n_y;

		private int _boko_d_x;

		private int _boko_d_y;

		private int _face_n_x;

		private int _face_n_y;

		private int _face_d_x;

		private int _face_d_y;

		private int _slotitem_category_n_x;

		private int _slotitem_category_n_y;

		private int _slotitem_category_d_x;

		private int _slotitem_category_d_y;

		private int _ship_display_center_n_x;

		private int _ship_display_center_n_y;

		private int _ship_display_center_d_x;

		private int _ship_display_center_d_y;

		private int _weda_x;

		private int _weda_y;

		private int _wedb_x;

		private int _wedb_y;

		private int _L2dSize_W;

		private int _L2dSize_H;

		private int _L2dBias_X;

		private int _L2dBias_Y;

		private static string _tableName = "mst_shipgraph";

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

		public int Sortno
		{
			get
			{
				return _sortno;
			}
			private set
			{
				_sortno = value;
			}
		}

		public int Boko_n_x
		{
			get
			{
				return _boko_n_x;
			}
			private set
			{
				_boko_n_x = value;
			}
		}

		public int Boko_n_y
		{
			get
			{
				return _boko_n_y;
			}
			private set
			{
				_boko_n_y = value;
			}
		}

		public int Boko_d_x
		{
			get
			{
				return _boko_d_x;
			}
			private set
			{
				_boko_d_x = value;
			}
		}

		public int Boko_d_y
		{
			get
			{
				return _boko_d_y;
			}
			private set
			{
				_boko_d_y = value;
			}
		}

		public int Face_n_x
		{
			get
			{
				return _face_n_x;
			}
			private set
			{
				_face_n_x = value;
			}
		}

		public int Face_n_y
		{
			get
			{
				return _face_n_y;
			}
			private set
			{
				_face_n_y = value;
			}
		}

		public int Face_d_x
		{
			get
			{
				return _face_d_x;
			}
			private set
			{
				_face_d_x = value;
			}
		}

		public int Face_d_y
		{
			get
			{
				return _face_d_y;
			}
			private set
			{
				_face_d_y = value;
			}
		}

		public int Slotitem_category_n_x
		{
			get
			{
				return _slotitem_category_n_x;
			}
			private set
			{
				_slotitem_category_n_x = value;
			}
		}

		public int Slotitem_category_n_y
		{
			get
			{
				return _slotitem_category_n_y;
			}
			private set
			{
				_slotitem_category_n_y = value;
			}
		}

		public int Slotitem_category_d_x
		{
			get
			{
				return _slotitem_category_d_x;
			}
			private set
			{
				_slotitem_category_d_x = value;
			}
		}

		public int Slotitem_category_d_y
		{
			get
			{
				return _slotitem_category_d_y;
			}
			private set
			{
				_slotitem_category_d_y = value;
			}
		}

		public int Ship_display_center_n_x
		{
			get
			{
				return _ship_display_center_n_x;
			}
			private set
			{
				_ship_display_center_n_x = value;
			}
		}

		public int Ship_display_center_n_y
		{
			get
			{
				return _ship_display_center_n_y;
			}
			private set
			{
				_ship_display_center_n_y = value;
			}
		}

		public int Ship_display_center_d_x
		{
			get
			{
				return _ship_display_center_d_x;
			}
			private set
			{
				_ship_display_center_d_x = value;
			}
		}

		public int Ship_display_center_d_y
		{
			get
			{
				return _ship_display_center_d_y;
			}
			private set
			{
				_ship_display_center_d_y = value;
			}
		}

		public int Weda_x
		{
			get
			{
				return _weda_x;
			}
			private set
			{
				_weda_x = value;
			}
		}

		public int Weda_y
		{
			get
			{
				return _weda_y;
			}
			private set
			{
				_weda_y = value;
			}
		}

		public int Wedb_x
		{
			get
			{
				return _wedb_x;
			}
			private set
			{
				_wedb_x = value;
			}
		}

		public int Wedb_y
		{
			get
			{
				return _wedb_y;
			}
			private set
			{
				_wedb_y = value;
			}
		}

		public int L2dSize_W
		{
			get
			{
				return _L2dSize_W;
			}
			private set
			{
				_L2dSize_W = value;
			}
		}

		public int L2dSize_H
		{
			get
			{
				return _L2dSize_H;
			}
			private set
			{
				_L2dSize_H = value;
			}
		}

		public int L2dBias_X
		{
			get
			{
				return _L2dBias_X;
			}
			private set
			{
				_L2dBias_X = value;
			}
		}

		public int L2dBias_Y
		{
			get
			{
				return _L2dBias_Y;
			}
			private set
			{
				_L2dBias_Y = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Sortno = int.Parse(element.Element("Sortno").Value);
			char c = ',';
			if (Sortno > 0)
			{
				int[] array = Array.ConvertAll(element.Element("Boko").Value.Split(c), (string x) => int.Parse(x));
				Boko_n_x = array[0];
				Boko_n_y = array[1];
				Boko_d_x = array[2];
				Boko_d_y = array[3];
				int[] array2 = Array.ConvertAll(element.Element("Face").Value.Split(c), (string x) => int.Parse(x));
				Face_n_x = array2[0];
				Face_n_y = array2[1];
				Face_d_x = array2[2];
				Face_d_y = array2[3];
				int[] array3 = Array.ConvertAll(element.Element("SlotCategory").Value.Split(c), (string x) => int.Parse(x));
				Slotitem_category_n_x = array3[0];
				Slotitem_category_n_y = array3[1];
				Slotitem_category_d_x = array3[2];
				Slotitem_category_d_y = array3[3];
				int[] array4 = Array.ConvertAll(element.Element("ShipDispCenter").Value.Split(c), (string x) => int.Parse(x));
				Ship_display_center_n_x = array4[0];
				Ship_display_center_n_y = array4[1];
				Ship_display_center_d_x = array4[2];
				Ship_display_center_d_y = array4[3];
				int[] array5 = Array.ConvertAll(element.Element("Wed").Value.Split(c), (string x) => int.Parse(x));
				Weda_x = array5[0];
				Weda_y = array5[1];
				Wedb_x = array5[2];
				Wedb_y = array5[3];
				int[] array6 = (element.Element("L2dSize") == null) ? new int[2]
				{
					666,
					666
				} : Array.ConvertAll(element.Element("L2dSize").Value.Split(c), (string x) => int.Parse(x));
				L2dSize_W = array6[0];
				L2dSize_H = array6[1];
				int[] array7 = (element.Element("L2dBias") == null) ? new int[2] : Array.ConvertAll(element.Element("L2dBias").Value.Split(c), (string x) => int.Parse(x));
				L2dBias_X = array7[0];
				L2dBias_Y = array7[1];
			}
		}
	}
}
