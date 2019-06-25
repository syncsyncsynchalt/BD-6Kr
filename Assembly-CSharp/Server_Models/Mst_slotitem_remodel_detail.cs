using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_slotitem_remodel_detail : Model_Base
	{
		private int _id;

		private int _level_from;

		private int _level_to;

		private int _change_flag;

		private int _success_rate1;

		private int _success_rate2;

		private int _req_material5_1;

		private int _req_material6_1;

		private int _req_material5_2;

		private int _req_material6_2;

		private int _req_slotitem_id;

		private int _req_slotitems;

		private int _new_slotitem_id;

		private int _new_slotitem_level;

		private static string _tableName = "mst_slotitem_remodel_detail";

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

		public int Level_from
		{
			get
			{
				return _level_from;
			}
			private set
			{
				_level_from = value;
			}
		}

		public int Level_to
		{
			get
			{
				return _level_to;
			}
			private set
			{
				_level_to = value;
			}
		}

		public int Change_flag
		{
			get
			{
				return _change_flag;
			}
			private set
			{
				_change_flag = value;
			}
		}

		public int Success_rate1
		{
			get
			{
				return _success_rate1;
			}
			private set
			{
				_success_rate1 = value;
			}
		}

		public int Success_rate2
		{
			get
			{
				return _success_rate2;
			}
			private set
			{
				_success_rate2 = value;
			}
		}

		public int Req_material5_1
		{
			get
			{
				return _req_material5_1;
			}
			private set
			{
				_req_material5_1 = value;
			}
		}

		public int Req_material6_1
		{
			get
			{
				return _req_material6_1;
			}
			private set
			{
				_req_material6_1 = value;
			}
		}

		public int Req_material5_2
		{
			get
			{
				return _req_material5_2;
			}
			private set
			{
				_req_material5_2 = value;
			}
		}

		public int Req_material6_2
		{
			get
			{
				return _req_material6_2;
			}
			private set
			{
				_req_material6_2 = value;
			}
		}

		public int Req_slotitem_id
		{
			get
			{
				return _req_slotitem_id;
			}
			private set
			{
				_req_slotitem_id = value;
			}
		}

		public int Req_slotitems
		{
			get
			{
				return _req_slotitems;
			}
			private set
			{
				_req_slotitems = value;
			}
		}

		public int New_slotitem_id
		{
			get
			{
				return _new_slotitem_id;
			}
			private set
			{
				_new_slotitem_id = value;
			}
		}

		public int New_slotitem_level
		{
			get
			{
				return _new_slotitem_level;
			}
			private set
			{
				_new_slotitem_level = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			char c = ',';
			string[] array = element.Element("RemodelData").Value.Split(c);
			Level_from = int.Parse(array[0]);
			Level_to = int.Parse(array[1]);
			Change_flag = int.Parse(array[2]);
			Success_rate1 = int.Parse(array[3]);
			Success_rate2 = int.Parse(array[4]);
			Req_material5_1 = int.Parse(array[5]);
			Req_material6_1 = int.Parse(array[6]);
			Req_material5_2 = int.Parse(array[7]);
			Req_material6_2 = int.Parse(array[8]);
			Req_slotitem_id = int.Parse(array[9]);
			Req_slotitems = int.Parse(array[10]);
			New_slotitem_id = int.Parse(array[11]);
			New_slotitem_level = int.Parse(array[12]);
		}
	}
}
