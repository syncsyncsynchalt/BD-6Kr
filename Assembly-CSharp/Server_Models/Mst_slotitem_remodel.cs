using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_slotitem_remodel : Model_Base
	{
		private int _id;

		private int _position;

		private int _slotitem_id;

		private int _req_material1;

		private int _req_material2;

		private int _req_material3;

		private int _req_material4;

		private int _req_material5;

		private int _req_material6;

		private int _voice_ship_id;

		private int _voice_id;

		private int _enabled;

		private int _priority;

		private string _p_ship_yomi;

		private int _p_ship_id;

		private int _p_stype;

		private static string _tableName = "mst_slotitem_remodel";

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

		public int Position
		{
			get
			{
				return _position;
			}
			private set
			{
				_position = value;
			}
		}

		public int Slotitem_id
		{
			get
			{
				return _slotitem_id;
			}
			private set
			{
				_slotitem_id = value;
			}
		}

		public int Req_material1
		{
			get
			{
				return _req_material1;
			}
			private set
			{
				_req_material1 = value;
			}
		}

		public int Req_material2
		{
			get
			{
				return _req_material2;
			}
			private set
			{
				_req_material2 = value;
			}
		}

		public int Req_material3
		{
			get
			{
				return _req_material3;
			}
			private set
			{
				_req_material3 = value;
			}
		}

		public int Req_material4
		{
			get
			{
				return _req_material4;
			}
			private set
			{
				_req_material4 = value;
			}
		}

		public int Req_material5
		{
			get
			{
				return _req_material5;
			}
			private set
			{
				_req_material5 = value;
			}
		}

		public int Req_material6
		{
			get
			{
				return _req_material6;
			}
			private set
			{
				_req_material6 = value;
			}
		}

		public int Voice_id
		{
			get
			{
				return _voice_id;
			}
			private set
			{
				_voice_id = value;
			}
		}

		public int Enabled
		{
			get
			{
				return _enabled;
			}
			private set
			{
				_enabled = value;
			}
		}

		public int Priority
		{
			get
			{
				return _priority;
			}
			private set
			{
				_priority = value;
			}
		}

		public string P_ship_yomi
		{
			get
			{
				return _p_ship_yomi;
			}
			private set
			{
				_p_ship_yomi = value;
			}
		}

		public int P_ship_id
		{
			get
			{
				return _p_ship_id;
			}
			private set
			{
				_p_ship_id = value;
			}
		}

		public int P_stype
		{
			get
			{
				return _p_stype;
			}
			private set
			{
				_p_stype = value;
			}
		}

		public static string tableName => _tableName;

		public int GetVoiceShipId(int deckSecondShipId)
		{
			if (Voice_id == 0)
			{
				return 0;
			}
			if (_voice_ship_id > 0)
			{
				return _voice_ship_id;
			}
			return deckSecondShipId;
		}

		public bool ValidShipId(List<Mem_ship> ships)
		{
			if (P_ship_id == 0)
			{
				return false;
			}
			if (ships.Count < 2)
			{
				return false;
			}
			if (ships[1].Ship_id != P_ship_id)
			{
				return false;
			}
			return true;
		}

		public bool ValidYomi(List<Mem_ship> ships)
		{
			if (P_ship_yomi == string.Empty)
			{
				return false;
			}
			if (ships.Count < 2)
			{
				return false;
			}
			return string.Equals(Mst_DataManager.Instance.Mst_ship[ships[1].Ship_id].Yomi, _p_ship_yomi);
		}

		public bool ValidStype(List<Mem_ship> ships)
		{
			if (P_stype == 0)
			{
				return false;
			}
			if (ships.Count < 2)
			{
				return false;
			}
			if (ships[1].Stype != P_stype)
			{
				return false;
			}
			return true;
		}

		public bool IsRemodelBase(List<Mem_ship> ships)
		{
			if (P_ship_id != 0)
			{
				return false;
			}
			if (!(P_ship_yomi == string.Empty))
			{
				return false;
			}
			if (P_stype != 0)
			{
				return false;
			}
			return true;
		}

		public List<Mst_slotitem_remodel> GetPriority(List<Mst_slotitem_remodel> remodel_list)
		{
			if (remodel_list.Count >= 2)
			{
				IOrderedEnumerable<Mst_slotitem_remodel> source = from s in remodel_list
					orderby s.Priority descending
					select s;
				return source.ToList();
			}
			return remodel_list;
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			char c = ',';
			string[] array = element.Element("RemodelData").Value.Split(c);
			Enabled = int.Parse(array[10]);
			if (Enabled != 0)
			{
				Position = int.Parse(array[0]);
				Slotitem_id = int.Parse(array[1]);
				Req_material1 = int.Parse(array[2]);
				Req_material2 = int.Parse(array[3]);
				Req_material3 = int.Parse(array[4]);
				Req_material4 = int.Parse(array[5]);
				Req_material5 = int.Parse(array[6]);
				Req_material6 = int.Parse(array[7]);
				_voice_ship_id = int.Parse(array[8]);
				Voice_id = int.Parse(array[9]);
				Enabled = int.Parse(array[10]);
				Priority = int.Parse(array[11]);
				P_ship_yomi = array[12];
				P_ship_id = int.Parse(array[13]);
				P_stype = int.Parse(array[14]);
			}
		}
	}
}
