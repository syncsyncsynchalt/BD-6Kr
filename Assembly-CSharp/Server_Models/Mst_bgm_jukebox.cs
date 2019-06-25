using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_bgm_jukebox : Model_Base
	{
		private int _id;

		private string _name;

		private string remarks;

		private int _bgm_id;

		private int _r_coins;

		private int _bgm_flag;

		private int _loops;

		private static string _tableName = "mst_bgm_jukebox";

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

		public string Remarks
		{
			get
			{
				return remarks;
			}
			private set
			{
				remarks = value;
			}
		}

		public int Bgm_id
		{
			get
			{
				return _bgm_id;
			}
			private set
			{
				_bgm_id = value;
			}
		}

		public int R_coins
		{
			get
			{
				return _r_coins;
			}
			private set
			{
				_r_coins = value;
			}
		}

		public int Bgm_flag
		{
			get
			{
				return _bgm_flag;
			}
			private set
			{
				_bgm_flag = value;
			}
		}

		public int Loops
		{
			get
			{
				return _loops;
			}
			private set
			{
				_loops = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			string[] array = element.Element("Jukebox_record").Value.Split(',');
			Id = int.Parse(array[0]);
			Name = array[1];
			Remarks = array[2];
			Bgm_id = int.Parse(array[3]);
			R_coins = int.Parse(array[4]);
			Bgm_flag = int.Parse(array[5]);
			Loops = int.Parse(array[6]);
		}
	}
}
