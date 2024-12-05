using Common.Enum;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Server_Common
{
	public static class Utils
	{
		private static string masterCurrentPath = string.Empty;

		public static void initMasterPath()
		{
			if (!(masterCurrentPath != string.Empty))
			{
                masterCurrentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml/tables/master/");
                if (!Directory.Exists(masterCurrentPath))
				{
					Directory.CreateDirectory(masterCurrentPath);
				}
			}
		}

		public static string getTableDirMaster(string tableName)
		{
			if (!tableName.Contains("mst"))
			{
				return string.Empty;
			}
			if (masterCurrentPath == string.Empty)
			{
				initMasterPath();
			}
			return masterCurrentPath;
		}

		public static IEnumerable<XElement> Xml_Result(string tableName, string recordName, string sortName)
		{
			string tableDirMaster = getTableDirMaster(tableName);
			string str = tableName + ".xml";
			string text = tableDirMaster + str;
			if (!File.Exists(text))
			{
				return null;
			}
			try
			{
				if (string.IsNullOrEmpty(sortName))
				{
					return from datas in XElement.Load(text).Elements(recordName)
						select (datas);
				}
				return from datas in XElement.Load(text).Elements(recordName)
					orderby int.Parse(datas.Element(sortName).Value)
					select datas;
			}
			catch
			{
				return null;
			}
		}

		public static IEnumerable<XElement> Xml_Result_Where(string tableName, string recordName, Dictionary<string, string> where_dict)
		{
			string tableDirMaster = getTableDirMaster(tableName);
			string str = tableName + ".xml";
			string text = tableDirMaster + str;
			if (!File.Exists(text))
			{
				return null;
			}
			IEnumerable<XElement> enumerable = XElement.Load(text).Elements(recordName).Where(delegate(XElement x)
			{
				foreach (KeyValuePair<string, string> item in where_dict)
				{
					if (!x.Element(item.Key).Value.Equals(item.Value))
					{
						return false;
					}
				}
				return true;
			});
			if (enumerable.Count() == 0)
			{
				enumerable = null;
			}
			return enumerable;
		}

		public static IEnumerable<XElement> Xml_Result_To_Path(string path, string recordName, string sortName)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			try
			{
				if (string.IsNullOrEmpty(sortName))
				{
					return from datas in XElement.Load(path).Elements(recordName)
						select (datas);
				}
				return from datas in XElement.Load(path).Elements(recordName)
					orderby int.Parse(datas.Element(sortName).Value)
					select datas;
			}
			catch
			{
				return null;
			}
		}

		public static bool IsBattleWin(BattleWinRankKinds rank)
		{
			if (rank >= BattleWinRankKinds.B)
			{
				return true;
			}
			return false;
		}

		public static double GetRandDouble(double min, double max, double up_keisu, int scale)
		{
			List<int> list = new List<int>();
			int num = (int)Math.Pow(10.0, scale);
			int num2 = (int)(min * (double)num);
			int num3 = (int)(max * (double)num);
			int num4 = (int)(up_keisu * (double)num);
			for (int i = num2; i <= num3; i += num4)
			{
				list.Add(i);
			}
			var anon = (from value in list
				select new
				{
					value
				} into x
				orderby Guid.NewGuid()
				select x).First();
			return (double)anon.value / (double)num;
		}

		public static int GetRandomRateIndex(List<double> rateValues)
		{
			double num = GetRandDouble(1.0, 100.0, 1.0, 1);
			int result = 0;
			for (int i = 0; i < rateValues.Count; i++)
			{
				num -= rateValues[i];
				if (num <= 0.0)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public static bool IsValidNewGamePlus()
		{
			if (Comm_UserDatas.Instance.User_plus.GetLapNum() > 0)
			{
				return true;
			}
			return false;
		}

		public static bool IsGameOver()
		{
			if (Comm_UserDatas.Instance.User_kdock.Count == 0)
			{
				return true;
			}
			if (IsTurnOver())
			{
				return true;
			}
			return false;
		}

		public static bool IsTurnOver()
		{
			return (Comm_UserDatas.Instance.User_turn.Total_turn >= 3600) ? true : false;
		}

		public static bool IsGameClear()
		{
			Dictionary<int, Mem_mapclear> user_mapclear = Comm_UserDatas.Instance.User_mapclear;
			if (user_mapclear == null)
			{
				return false;
			}
			int num = 17;
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			int mapinfo_no = Mst_maparea.MaxMapNum(difficult, num);
			int key = Mst_mapinfo.ConvertMapInfoId(num, mapinfo_no);
			if (!user_mapclear.ContainsKey(key))
			{
				return false;
			}
			if (!user_mapclear[key].Cleared)
			{
				return false;
			}
			return true;
		}

		public static int GetBookRegNum(int type)
		{
			if (type == 1)
			{
				int maxNo = Mst_DataManager.Instance.Mst_const[MstConstDataIndex.Book_max_ships].Int_value;
				Dictionary<int, Mst_ship> mst = Mst_DataManager.Instance.Mst_ship;
				return Comm_UserDatas.Instance.Ship_book.Values.Count(delegate(Mem_book x)
				{
					if (!mst.ContainsKey(x.Table_id))
					{
						return false;
					}
					return (maxNo >= mst[x.Table_id].Bookno) ? true : false;
				});
			}
			return Comm_UserDatas.Instance.Slot_book.Count;
		}

		public static Dictionary<int, Mst_mapinfo> GetActiveMap()
		{
			Dictionary<int, Mst_mapinfo> dictionary = new Dictionary<int, Mst_mapinfo>();
			foreach (Mst_mapinfo value in Mst_DataManager.Instance.Mst_mapinfo.Values)
			{
				User_MapinfoFmt user_MapinfoData = value.GetUser_MapinfoData();
				if (user_MapinfoData != null && user_MapinfoData.IsGo)
				{
					dictionary.Add(value.Id, value);
				}
			}
			return dictionary;
		}
	}
}
