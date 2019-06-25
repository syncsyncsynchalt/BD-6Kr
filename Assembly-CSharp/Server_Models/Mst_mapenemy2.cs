using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapenemy2 : Model_Base
	{
		private int _id;

		private int _maparea_id;

		private int _mapinfo_no;

		private int _enemy_list_id;

		private int _boss;

		private int _deck_id;

		private string _deck_name;

		private int _formation_id;

		private int _e1_id;

		private int _e1_lv;

		private int _e2_id;

		private int _e2_lv;

		private int _e3_id;

		private int _e3_lv;

		private int _e4_id;

		private int _e4_lv;

		private int _e5_id;

		private int _e5_lv;

		private int _e6_id;

		private int _e6_lv;

		private int _geth;

		private int _experience;

		private static string _tableName = "mst_mapenemy2";

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

		public int Mapinfo_no
		{
			get
			{
				return _mapinfo_no;
			}
			private set
			{
				_mapinfo_no = value;
			}
		}

		public int Enemy_list_id
		{
			get
			{
				return _enemy_list_id;
			}
			private set
			{
				_enemy_list_id = value;
			}
		}

		public int Boss
		{
			get
			{
				return _boss;
			}
			private set
			{
				_boss = value;
			}
		}

		public int Deck_id
		{
			get
			{
				return _deck_id;
			}
			private set
			{
				_deck_id = value;
			}
		}

		public string Deck_name
		{
			get
			{
				return _deck_name;
			}
			private set
			{
				_deck_name = value;
			}
		}

		public int Formation_id
		{
			get
			{
				return _formation_id;
			}
			private set
			{
				_formation_id = value;
			}
		}

		public int E1_id
		{
			get
			{
				return _e1_id;
			}
			private set
			{
				_e1_id = value;
			}
		}

		public int E1_lv
		{
			get
			{
				return _e1_lv;
			}
			private set
			{
				_e1_lv = value;
			}
		}

		public int E2_id
		{
			get
			{
				return _e2_id;
			}
			private set
			{
				_e2_id = value;
			}
		}

		public int E2_lv
		{
			get
			{
				return _e2_lv;
			}
			private set
			{
				_e2_lv = value;
			}
		}

		public int E3_id
		{
			get
			{
				return _e3_id;
			}
			private set
			{
				_e3_id = value;
			}
		}

		public int E3_lv
		{
			get
			{
				return _e3_lv;
			}
			private set
			{
				_e3_lv = value;
			}
		}

		public int E4_id
		{
			get
			{
				return _e4_id;
			}
			private set
			{
				_e4_id = value;
			}
		}

		public int E4_lv
		{
			get
			{
				return _e4_lv;
			}
			private set
			{
				_e4_lv = value;
			}
		}

		public int E5_id
		{
			get
			{
				return _e5_id;
			}
			private set
			{
				_e5_id = value;
			}
		}

		public int E5_lv
		{
			get
			{
				return _e5_lv;
			}
			private set
			{
				_e5_lv = value;
			}
		}

		public int E6_id
		{
			get
			{
				return _e6_id;
			}
			private set
			{
				_e6_id = value;
			}
		}

		public int E6_lv
		{
			get
			{
				return _e6_lv;
			}
			private set
			{
				_e6_lv = value;
			}
		}

		public int Geth
		{
			get
			{
				return _geth;
			}
			private set
			{
				_geth = value;
			}
		}

		public int Experience
		{
			get
			{
				return _experience;
			}
			private set
			{
				_experience = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Maparea_id = int.Parse(element.Element("Maparea_id").Value);
			Mapinfo_no = int.Parse(element.Element("Mapinfo_no").Value);
			Enemy_list_id = int.Parse(element.Element("Enemy_list_id").Value);
			Boss = int.Parse(element.Element("Boss").Value);
			Deck_id = int.Parse(element.Element("Deck_id").Value);
			Deck_name = element.Element("Deck_name").Value;
			Formation_id = int.Parse(element.Element("Formation_id").Value);
			E1_id = int.Parse(element.Element("E1_id").Value);
			E1_lv = int.Parse(element.Element("E1_lv").Value);
			E2_id = int.Parse(element.Element("E2_id").Value);
			E2_lv = int.Parse(element.Element("E2_lv").Value);
			E3_id = int.Parse(element.Element("E3_id").Value);
			E3_lv = int.Parse(element.Element("E3_lv").Value);
			E4_id = int.Parse(element.Element("E4_id").Value);
			E4_lv = int.Parse(element.Element("E4_lv").Value);
			E5_id = int.Parse(element.Element("E5_id").Value);
			E5_lv = int.Parse(element.Element("E5_lv").Value);
			E6_id = int.Parse(element.Element("E6_id").Value);
			E6_lv = int.Parse(element.Element("E6_lv").Value);
			Geth = int.Parse(element.Element("Geth").Value);
			Experience = int.Parse(element.Element("Experience").Value);
		}

		public void GetEnemyShips(out List<Mem_ship> out_ship, out List<List<Mst_slotitem>> out_slot)
		{
			out_ship = new List<Mem_ship>();
			out_slot = new List<List<Mst_slotitem>>();
			if (E1_id > 0)
			{
				out_ship.Add(getMemShip(-1, E1_id, E1_lv));
			}
			if (E2_id > 0)
			{
				out_ship.Add(getMemShip(-2, E2_id, E2_lv));
			}
			if (E3_id > 0)
			{
				out_ship.Add(getMemShip(-3, E3_id, E3_lv));
			}
			if (E4_id > 0)
			{
				out_ship.Add(getMemShip(-4, E4_id, E4_lv));
			}
			if (E5_id > 0)
			{
				out_ship.Add(getMemShip(-5, E5_id, E5_lv));
			}
			if (E6_id > 0)
			{
				out_ship.Add(getMemShip(-6, E6_id, E6_lv));
			}
			foreach (Mem_ship item in out_ship)
			{
				List<Mst_slotitem> slot = new List<Mst_slotitem>();
				item.Slot.ForEach(delegate(int x)
				{
					Mst_slotitem value = null;
					if (Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(x, out value))
					{
						slot.Add(value);
					}
				});
				out_slot.Add(slot);
			}
		}

		private Mem_ship getMemShip(int rid, int mst_id, int level)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[mst_id];
			Array values = Enum.GetValues(typeof(Mem_ship.enumKyoukaIdx));
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			foreach (object item in values)
			{
				dictionary.Add((Mem_ship.enumKyoukaIdx)(int)item, 0);
			}
			Mem_shipBase baseData = new Mem_shipBase(rid, mst_ship, level, dictionary);
			Mem_ship mem_ship = new Mem_ship();
			mem_ship.Set_ShipParam(baseData, mst_ship, enemy_flag: true);
			return mem_ship;
		}
	}
}
