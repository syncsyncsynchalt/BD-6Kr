using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapinfo : Model_Base
	{
		private int _id;

		private int _maparea_id;

		private int _no;

		private List<int> _required_ids;

		private int _level;

		private int _item1;

		private int _item2;

		private int _item3;

		private int _item4;

		private int _maxcell;

		private int _ship_exp;

		private int _member_exp;

		private int _clear_exp;

		private string _name;

		private string _opetext;

		private string _infotext;

		private static string _tableName = "mst_mapinfo";

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

		public int No
		{
			get
			{
				return _no;
			}
			private set
			{
				_no = value;
			}
		}

		public List<int> Required_ids
		{
			get
			{
				return getRequiredIds();
			}
			private set
			{
				_required_ids = value;
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

		public int Item1
		{
			get
			{
				return _item1;
			}
			private set
			{
				_item1 = value;
			}
		}

		public int Item2
		{
			get
			{
				return _item2;
			}
			private set
			{
				_item2 = value;
			}
		}

		public int Item3
		{
			get
			{
				return _item3;
			}
			private set
			{
				_item3 = value;
			}
		}

		public int Item4
		{
			get
			{
				return _item4;
			}
			private set
			{
				_item4 = value;
			}
		}

		public int Maxcell
		{
			get
			{
				return _maxcell;
			}
			private set
			{
				_maxcell = value;
			}
		}

		public int Ship_exp
		{
			get
			{
				return _ship_exp;
			}
			private set
			{
				_ship_exp = value;
			}
		}

		public int Member_exp
		{
			get
			{
				return _member_exp;
			}
			private set
			{
				_member_exp = value;
			}
		}

		public int Clear_exp
		{
			get
			{
				return _clear_exp;
			}
			private set
			{
				_clear_exp = value;
			}
		}

		public int Clear_spoint => getSpoint();

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

		public string Opetext
		{
			get
			{
				return _opetext;
			}
			private set
			{
				_opetext = value;
			}
		}

		public string Infotext
		{
			get
			{
				return _infotext;
			}
			private set
			{
				_infotext = value;
			}
		}

		public static string tableName => _tableName;

		public static int ConvertMapInfoId(int maparea_id, int mapinfo_no)
		{
			string s = maparea_id.ToString() + mapinfo_no.ToString();
			int result = 0;
			int.TryParse(s, out result);
			return result;
		}

		public User_MapinfoFmt GetUser_MapinfoData()
		{
			if (!IsOpenMapSys())
			{
				return null;
			}
			Dictionary<int, Mem_mapclear> user_mapclear = Comm_UserDatas.Instance.User_mapclear;
			Mem_mapclear value = null;
			Mem_mapclear value2 = null;
			int key = ConvertMapInfoId(Maparea_id, 1);
			if (user_mapclear.TryGetValue(key, out value2) && value2.State == MapClearState.InvationClose)
			{
				return null;
			}
			bool cleared = false;
			if (user_mapclear.TryGetValue(Id, out value))
			{
				if (value.State == MapClearState.InvationClose)
				{
					return null;
				}
				cleared = ((value.State == MapClearState.Cleard) ? true : false);
			}
			Mem_mapclear value3 = null;
			bool flag = false;
			if (No != 1)
			{
				int key2 = ConvertMapInfoId(Maparea_id, No - 1);
				user_mapclear.TryGetValue(key2, out value3);
				if (value3 != null && value3.State != 0)
				{
					return null;
				}
				if (value != null && value.State == MapClearState.InvationOpen)
				{
					flag = true;
				}
			}
			if (Required_ids.Count != 0)
			{
				foreach (int required_id in Required_ids)
				{
					Mem_mapclear value4 = null;
					if (!user_mapclear.TryGetValue(required_id, out value4))
					{
						return null;
					}
					if (!value4.Cleared)
					{
						return null;
					}
					if (flag && value4.State != 0)
					{
						return null;
					}
				}
			}
			User_MapinfoFmt user_MapinfoFmt = new User_MapinfoFmt();
			user_MapinfoFmt.Id = Id;
			user_MapinfoFmt.Cleared = cleared;
			user_MapinfoFmt.IsGo = true;
			return user_MapinfoFmt;
		}

		public bool IsOpenMapSys()
		{
			Mst_maparea value = null;
			if (!Mst_DataManager.Instance.Mst_maparea.TryGetValue(Maparea_id, out value))
			{
				return false;
			}
			if (!value.IsOpenArea())
			{
				return false;
			}
			if (Required_ids.Count > 0 && Required_ids[0] == 999)
			{
				return false;
			}
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			int num = Mst_maparea.MaxMapNum(difficult, Maparea_id);
			if (No < 5 && No > num)
			{
				return false;
			}
			return true;
		}

		private int getSpoint()
		{
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			int num = Mst_maparea.MaxMapNum(difficult, Maparea_id);
			if (Maparea_id == 17 && No == num)
			{
				return 0;
			}
			if (No == num)
			{
				return 1000;
			}
			return 300;
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Maparea_id = int.Parse(element.Element("Maparea_id").Value);
			No = int.Parse(element.Element("No").Value);
			Required_ids = getRequiredIds(element.Element("Required_ids").Value);
			Level = int.Parse(element.Element("Level").Value);
			Item1 = int.Parse(element.Element("Item1").Value);
			Item2 = int.Parse(element.Element("Item2").Value);
			Item3 = int.Parse(element.Element("Item3").Value);
			Item4 = int.Parse(element.Element("Item4").Value);
			Maxcell = int.Parse(element.Element("Maxcell").Value);
			Ship_exp = int.Parse(element.Element("Ship_exp").Value);
			Member_exp = int.Parse(element.Element("Member_exp").Value);
			Clear_exp = int.Parse(element.Element("Clear_exp").Value);
			Name = element.Element("Name").Value;
			Opetext = element.Element("Opetext").Value;
			Infotext = element.Element("Infotext").Value;
		}

		private List<int> getRequiredIds(string target)
		{
			if (string.IsNullOrEmpty(target) || target.Equals("0"))
			{
				return new List<int>();
			}
			return Array.ConvertAll(target.Split(','), (string x) => int.Parse(x)).ToList();
		}

		private List<int> getRequiredIds()
		{
			Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
			if (user_basic == null)
			{
				return _required_ids;
			}
			if (Maparea_id == 14 && No == 1)
			{
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				dictionary.Add(1, _required_ids);
				dictionary.Add(2, new List<int>
				{
					54,
					43
				});
				Dictionary<int, List<int>> dictionary2 = dictionary;
				List<int> result = dictionary2[1];
				{
					foreach (KeyValuePair<int, List<int>> item in dictionary2)
					{
						if (isReqIdComplete(item.Value))
						{
							return item.Value;
						}
					}
					return result;
				}
			}
			if (Maparea_id == 16 && No == 1)
			{
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				dictionary.Add(1, _required_ids);
				dictionary.Add(2, new List<int>
				{
					63,
					122,
					141
				});
				dictionary.Add(3, new List<int>
				{
					63,
					122,
					151
				});
				Dictionary<int, List<int>> dictionary3 = dictionary;
				List<int> result = dictionary3[1];
				{
					foreach (KeyValuePair<int, List<int>> item2 in dictionary3)
					{
						if (isReqIdComplete(item2.Value))
						{
							return item2.Value;
						}
					}
					return result;
				}
			}
			if (Maparea_id == 17 && No == 1)
			{
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				dictionary.Add(1, _required_ids);
				dictionary.Add(2, new List<int>
				{
					124,
					153
				});
				dictionary.Add(3, new List<int>
				{
					124,
					163
				});
				dictionary.Add(4, new List<int>
				{
					152,
					162
				});
				Dictionary<int, List<int>> dictionary4 = dictionary;
				List<int> result = dictionary4[1];
				{
					foreach (KeyValuePair<int, List<int>> item3 in dictionary4)
					{
						if (isReqIdComplete(item3.Value))
						{
							return item3.Value;
						}
					}
					return result;
				}
			}
			return _required_ids;
		}

		private bool isReqIdComplete(List<int> mapinfo_ids)
		{
			Dictionary<int, Mem_mapclear> clearDict = Comm_UserDatas.Instance.User_mapclear;
			return mapinfo_ids.TrueForAll(delegate(int x)
			{
				Mem_mapclear value = null;
				return clearDict.TryGetValue(x, out value) && ((value.State == MapClearState.Cleard) ? true : false);
			});
		}
	}
}
