using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mission2 : Model_Base
	{
		private int _id;

		private int _maparea_id;

		private string _name;

		private string _details;

		private MissionType _mission_type;

		private int _time;

		private int _rp_sub;

		private int _difficulty;

		private double _use_fuel;

		private double _use_bull;

		private string _required_ids;

		private int _win_exp_member;

		private int _win_exp_ship;

		private int _win_mat1;

		private int _win_mat2;

		private int _win_mat3;

		private int _win_mat4;

		private int _win_item1;

		private int _win_item1_num;

		private int _win_item2;

		private int _win_item2_num;

		private int _win_spoint1;

		private int _win_spoint2;

		private int _level;

		private int _flagship_level;

		private int _stype_num1;

		private int _stype_num2;

		private int _stype_num3;

		private int _stype_num4;

		private int _stype_num5;

		private int _stype_num6;

		private int _stype_num7;

		private int _stype_num8;

		private int _stype_num9;

		private int _deck_num;

		private int _drum_ship_num;

		private int _drum_total_num1;

		private int _drum_total_num2;

		private int _flagship_stype1;

		private int _flagship_stype2;

		private int _flagship_level_check_type;

		private int _tanker_num;

		private int _tanker_num_max;

		private static string _tableName = "mst_mission2";

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

		public string Details
		{
			get
			{
				return _details;
			}
			private set
			{
				_details = value;
			}
		}

		public MissionType Mission_type
		{
			get
			{
				return _mission_type;
			}
			private set
			{
				_mission_type = value;
			}
		}

		public int Time
		{
			get
			{
				return _time;
			}
			private set
			{
				_time = value;
			}
		}

		public int Rp_sub
		{
			get
			{
				return _rp_sub;
			}
			private set
			{
				_rp_sub = value;
			}
		}

		public int Difficulty
		{
			get
			{
				return _difficulty;
			}
			private set
			{
				_difficulty = value;
			}
		}

		public double Use_fuel
		{
			get
			{
				return _use_fuel;
			}
			private set
			{
				_use_fuel = value;
			}
		}

		public double Use_bull
		{
			get
			{
				return _use_bull;
			}
			private set
			{
				_use_bull = value;
			}
		}

		public string Required_ids
		{
			get
			{
				return _required_ids;
			}
			private set
			{
				_required_ids = value;
			}
		}

		public int Win_exp_member
		{
			get
			{
				return _win_exp_member;
			}
			private set
			{
				_win_exp_member = value;
			}
		}

		public int Win_exp_ship
		{
			get
			{
				return _win_exp_ship;
			}
			private set
			{
				_win_exp_ship = value;
			}
		}

		public int Win_mat1
		{
			get
			{
				return _win_mat1;
			}
			private set
			{
				_win_mat1 = value;
			}
		}

		public int Win_mat2
		{
			get
			{
				return _win_mat2;
			}
			private set
			{
				_win_mat2 = value;
			}
		}

		public int Win_mat3
		{
			get
			{
				return _win_mat3;
			}
			private set
			{
				_win_mat3 = value;
			}
		}

		public int Win_mat4
		{
			get
			{
				return _win_mat4;
			}
			private set
			{
				_win_mat4 = value;
			}
		}

		public int Win_item1
		{
			get
			{
				return _win_item1;
			}
			private set
			{
				_win_item1 = value;
			}
		}

		public int Win_item1_num
		{
			get
			{
				return _win_item1_num;
			}
			private set
			{
				_win_item1_num = value;
			}
		}

		public int Win_item2
		{
			get
			{
				return _win_item2;
			}
			private set
			{
				_win_item2 = value;
			}
		}

		public int Win_item2_num
		{
			get
			{
				return _win_item2_num;
			}
			private set
			{
				_win_item2_num = value;
			}
		}

		public int Win_spoint1
		{
			get
			{
				return _win_spoint1;
			}
			private set
			{
				_win_spoint1 = value;
			}
		}

		public int Win_spoint2
		{
			get
			{
				return _win_spoint2;
			}
			private set
			{
				_win_spoint2 = value;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
			private set
			{
				_level = value;
			}
		}

		public int Flagship_level
		{
			get
			{
				return _flagship_level;
			}
			private set
			{
				_flagship_level = value;
			}
		}

		public int Stype_num1
		{
			get
			{
				return _stype_num1;
			}
			private set
			{
				_stype_num1 = value;
			}
		}

		public int Stype_num2
		{
			get
			{
				return _stype_num2;
			}
			private set
			{
				_stype_num2 = value;
			}
		}

		public int Stype_num3
		{
			get
			{
				return _stype_num3;
			}
			private set
			{
				_stype_num3 = value;
			}
		}

		public int Stype_num4
		{
			get
			{
				return _stype_num4;
			}
			private set
			{
				_stype_num4 = value;
			}
		}

		public int Stype_num5
		{
			get
			{
				return _stype_num5;
			}
			private set
			{
				_stype_num5 = value;
			}
		}

		public int Stype_num6
		{
			get
			{
				return _stype_num6;
			}
			private set
			{
				_stype_num6 = value;
			}
		}

		public int Stype_num7
		{
			get
			{
				return _stype_num7;
			}
			private set
			{
				_stype_num7 = value;
			}
		}

		public int Stype_num8
		{
			get
			{
				return _stype_num8;
			}
			private set
			{
				_stype_num8 = value;
			}
		}

		public int Stype_num9
		{
			get
			{
				return _stype_num9;
			}
			private set
			{
				_stype_num9 = value;
			}
		}

		public int Deck_num
		{
			get
			{
				return _deck_num;
			}
			private set
			{
				_deck_num = value;
			}
		}

		public int Drum_ship_num
		{
			get
			{
				return _drum_ship_num;
			}
			private set
			{
				_drum_ship_num = value;
			}
		}

		public int Drum_total_num1
		{
			get
			{
				return _drum_total_num1;
			}
			private set
			{
				_drum_total_num1 = value;
			}
		}

		public int Drum_total_num2
		{
			get
			{
				return _drum_total_num2;
			}
			private set
			{
				_drum_total_num2 = value;
			}
		}

		public int Flagship_stype1
		{
			get
			{
				return _flagship_stype1;
			}
			private set
			{
				_flagship_stype1 = value;
			}
		}

		public int Flagship_stype2
		{
			get
			{
				return _flagship_stype2;
			}
			private set
			{
				_flagship_stype2 = value;
			}
		}

		public int Flagship_level_check_type
		{
			get
			{
				return _flagship_level_check_type;
			}
			private set
			{
				_flagship_level_check_type = value;
			}
		}

		public int Tanker_num
		{
			get
			{
				return _tanker_num;
			}
			private set
			{
				_tanker_num = value;
			}
		}

		public int Tanker_num_max
		{
			get
			{
				return _tanker_num_max;
			}
			private set
			{
				_tanker_num_max = value;
			}
		}

		public static string tableName => _tableName;

		public bool IsGreatSuccessCondition()
		{
			if (Drum_total_num2 > 0)
			{
				return true;
			}
			if (Flagship_stype2 > 0)
			{
				return true;
			}
			if (Flagship_level_check_type == 2)
			{
				return true;
			}
			return false;
		}

		public bool IsSupportMission()
		{
			if (Mission_type == MissionType.SupportForward || Mission_type == MissionType.SupportBoss)
			{
				return true;
			}
			return false;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Id = int.Parse(element.Element("Id").Value);
			Maparea_id = int.Parse(element.Element("Maparea_id").Value);
			Name = element.Element("Name").Value;
			Details = element.Element("Details").Value;
			Mission_type = (MissionType)int.Parse(element.Element("Mission_type").Value);
			Time = int.Parse(element.Element("Time").Value);
			Rp_sub = int.Parse(element.Element("Rp_sub").Value);
			Difficulty = int.Parse(element.Element("Difficulty").Value);
			double[] array = Array.ConvertAll(element.Element("Use_mat").Value.Split(c), (string x) => double.Parse(x));
			Use_fuel = array[0];
			Use_bull = array[1];
			Required_ids = element.Element("Required_ids").Value;
			int[] array2 = Array.ConvertAll(element.Element("Win_exp").Value.Split(c), (string x) => int.Parse(x));
			Win_exp_member = array2[0];
			Win_exp_ship = array2[1];
			int[] array3 = Array.ConvertAll(element.Element("Win_mat").Value.Split(c), (string x) => int.Parse(x));
			Win_mat1 = array3[0];
			Win_mat2 = array3[1];
			Win_mat3 = array3[2];
			Win_mat4 = array3[3];
			int[] array4 = Array.ConvertAll(element.Element("Win_item1").Value.Split(c), (string x) => int.Parse(x));
			Win_item1 = array4[0];
			Win_item1_num = array4[1];
			int[] array5 = Array.ConvertAll(element.Element("Win_item2").Value.Split(c), (string x) => int.Parse(x));
			Win_item2 = array5[0];
			Win_item2_num = array5[1];
			int[] array6 = Array.ConvertAll(element.Element("Win_spoint").Value.Split(c), (string x) => int.Parse(x));
			Win_spoint1 = array6[0];
			Win_spoint2 = array6[1];
			Level = int.Parse(element.Element("Level").Value);
			Flagship_level_check_type = int.Parse(element.Element("Flagship_level_check_type").Value);
			Flagship_level = int.Parse(element.Element("Flagship_level").Value);
			int[] array7 = Array.ConvertAll(element.Element("Stype_num").Value.Split(c), (string x) => int.Parse(x));
			Stype_num1 = array7[0];
			Stype_num2 = array7[1];
			Stype_num3 = array7[2];
			Stype_num4 = array7[3];
			Stype_num5 = array7[4];
			Stype_num6 = array7[5];
			Stype_num7 = array7[6];
			Stype_num8 = array7[7];
			Stype_num9 = array7[8];
			Deck_num = int.Parse(element.Element("Deck_num").Value);
			int[] array8 = Array.ConvertAll(element.Element("Drum_num").Value.Split(c), (string x) => int.Parse(x));
			Drum_ship_num = array8[0];
			Drum_total_num1 = array8[1];
			Drum_total_num2 = array8[2];
			int[] array9 = Array.ConvertAll(element.Element("Flagship_stype").Value.Split(c), (string x) => int.Parse(x));
			Flagship_stype1 = array9[0];
			Flagship_stype2 = array9[1];
			int[] array10 = Array.ConvertAll(element.Element("Tanker_num").Value.Split(c), (string x) => int.Parse(x));
			Tanker_num = array10[0];
			Tanker_num_max = 16;
		}
	}
}
