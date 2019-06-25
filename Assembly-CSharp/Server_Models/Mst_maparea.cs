using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_maparea : Model_Base
	{
		private int _id;

		private string _no;

		private string _name;

		private int _evt_flag;

		private int _material_1;

		private int _material_2;

		private int _material_3;

		private int _material_4;

		private int _req_tanker_num;

		private int _ndocks_init;

		private int _ndocks_max;

		private int _erc_air_rate;

		private int _erc_submarine_rate;

		private int _distance;

		private List<int> _neighboring_area;

		private static string _tableName = "mst_maparea";

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

		public string No
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

		public int Evt_flag
		{
			get
			{
				return _evt_flag;
			}
			private set
			{
				_evt_flag = value;
			}
		}

		public int Material_1
		{
			get
			{
				return _material_1;
			}
			private set
			{
				_material_1 = value;
			}
		}

		public int Material_2
		{
			get
			{
				return _material_2;
			}
			private set
			{
				_material_2 = value;
			}
		}

		public int Material_3
		{
			get
			{
				return _material_3;
			}
			private set
			{
				_material_3 = value;
			}
		}

		public int Material_4
		{
			get
			{
				return _material_4;
			}
			private set
			{
				_material_4 = value;
			}
		}

		public int Req_tanker_num
		{
			get
			{
				return _req_tanker_num;
			}
			private set
			{
				_req_tanker_num = value;
			}
		}

		public int Ndocks_init
		{
			get
			{
				return _ndocks_init;
			}
			private set
			{
				_ndocks_init = value;
			}
		}

		public int Ndocks_max
		{
			get
			{
				return _ndocks_max;
			}
			private set
			{
				_ndocks_max = value;
			}
		}

		public int Erc_air_rate
		{
			get
			{
				return _erc_air_rate;
			}
			private set
			{
				_erc_air_rate = value;
			}
		}

		public int Erc_submarine_rate
		{
			get
			{
				return _erc_submarine_rate;
			}
			private set
			{
				_erc_submarine_rate = value;
			}
		}

		public int Distance
		{
			get
			{
				return _distance;
			}
			private set
			{
				_distance = value;
			}
		}

		public List<int> Neighboring_area
		{
			get
			{
				return _neighboring_area;
			}
			private set
			{
				_neighboring_area = value;
			}
		}

		public static string tableName => _tableName;

		public int GetMaterialLimitTankerNum()
		{
			return (int)((double)Req_tanker_num * 1.2);
		}

		public int GetUIMaterialLimitTankerNum()
		{
			int materialLimitTankerNum = GetMaterialLimitTankerNum();
			return (Req_tanker_num != materialLimitTankerNum) ? materialLimitTankerNum : (materialLimitTankerNum + 1);
		}

		public bool IsActiveArea()
		{
			int key = Mst_mapinfo.ConvertMapInfoId(Id, 1);
			if (Mst_DataManager.Instance.Mst_mapinfo[key].GetUser_MapinfoData() == null)
			{
				return false;
			}
			return true;
		}

		public static int MaxMapNum(DifficultKind kind, int area_id)
		{
			switch (area_id)
			{
			case 15:
				return (kind < DifficultKind.KOU) ? 3 : 4;
			case 16:
				return (kind < DifficultKind.OTU) ? 3 : 4;
			case 17:
				return (kind < DifficultKind.SHI) ? 3 : 4;
			default:
				return 4;
			}
		}

		protected override void setProperty(XElement element)
		{
			char[] separator = new char[1]
			{
				','
			};
			Id = int.Parse(element.Element("Id").Value);
			No = element.Element("No").Value;
			Name = element.Element("Name").Value;
			Evt_flag = int.Parse(element.Element("Evt_flag").Value);
			Material_1 = int.Parse(element.Element("Material_1").Value);
			Material_2 = int.Parse(element.Element("Material_2").Value);
			Material_3 = int.Parse(element.Element("Material_3").Value);
			Material_4 = int.Parse(element.Element("Material_4").Value);
			Req_tanker_num = int.Parse(element.Element("Req_tanker_num").Value);
			Ndocks_init = int.Parse(element.Element("Ndocks_init").Value);
			Ndocks_max = int.Parse(element.Element("Ndocks_max").Value);
			Erc_air_rate = int.Parse(element.Element("Erc_air_rate").Value);
			Erc_submarine_rate = int.Parse(element.Element("Erc_submarine_rate").Value);
			Distance = int.Parse(element.Element("Distance").Value);
			string[] array = element.Element("Neighboring_area").Value.Split(separator);
			Neighboring_area = Array.ConvertAll(array, (string x) => int.Parse(x)).ToList();
		}

		public bool IsOpenArea()
		{
			if (Evt_flag == 0)
			{
				return true;
			}
			return false;
		}

		public void TakeMaterialNum(Dictionary<int, Mem_mapclear> mapclear, int tankerNum, ref Dictionary<enumMaterialCategory, int> addValues, bool randMaxFlag, DeckShips deckShip)
		{
			if (tankerNum == 0 || !IsActiveArea())
			{
				return;
			}
			int num = deckShip.getMemShip().Count((Mem_ship x) => x.IsEscortDeffender());
			double num2 = tankerNum;
			double num3 = Req_tanker_num;
			Dictionary<enumMaterialCategory, double> dictionary = new Dictionary<enumMaterialCategory, double>();
			dictionary.Add(enumMaterialCategory.Fuel, 0.0);
			dictionary.Add(enumMaterialCategory.Bull, 0.0);
			dictionary.Add(enumMaterialCategory.Steel, 0.0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0.0);
			Dictionary<enumMaterialCategory, double> dictionary2 = dictionary;
			int materialLimitTankerNum = GetMaterialLimitTankerNum();
			if (tankerNum <= Req_tanker_num)
			{
				dictionary2[enumMaterialCategory.Fuel] = (int)((double)Material_1 * num2 / num3);
				dictionary2[enumMaterialCategory.Bull] = (int)((double)Material_2 * num2 / num3);
				dictionary2[enumMaterialCategory.Steel] = (int)((double)Material_3 * num2 / num3);
				dictionary2[enumMaterialCategory.Bauxite] = (int)((double)Material_4 * num2 / num3);
				if (num == 0)
				{
					double min = (!randMaxFlag) ? 0.5 : 0.75;
					dictionary2[enumMaterialCategory.Fuel] *= Utils.GetRandDouble(min, 0.75, 0.01, 2);
					dictionary2[enumMaterialCategory.Bull] *= Utils.GetRandDouble(min, 0.75, 0.01, 2);
					dictionary2[enumMaterialCategory.Steel] *= Utils.GetRandDouble(min, 0.75, 0.01, 2);
					dictionary2[enumMaterialCategory.Bauxite] *= Utils.GetRandDouble(min, 0.75, 0.01, 2);
				}
				int num4 = num * 2;
				if (num4 < tankerNum && num < 6)
				{
					double min2 = (!randMaxFlag) ? 0.75 : 1.0;
					dictionary2[enumMaterialCategory.Fuel] *= Utils.GetRandDouble(min2, 1.0, 0.01, 2);
					dictionary2[enumMaterialCategory.Bull] *= Utils.GetRandDouble(min2, 1.0, 0.01, 2);
					dictionary2[enumMaterialCategory.Steel] *= Utils.GetRandDouble(min2, 1.0, 0.01, 2);
					dictionary2[enumMaterialCategory.Bauxite] *= Utils.GetRandDouble(min2, 1.0, 0.01, 2);
				}
			}
			else if (tankerNum >= materialLimitTankerNum)
			{
				double min3 = (!randMaxFlag) ? 1.0 : 1.3;
				dictionary2[enumMaterialCategory.Fuel] = (double)Material_1 * Utils.GetRandDouble(min3, 1.3, 0.1, 1);
				dictionary2[enumMaterialCategory.Bull] = (double)Material_2 * Utils.GetRandDouble(min3, 1.3, 0.1, 1);
				dictionary2[enumMaterialCategory.Steel] = (double)Material_3 * Utils.GetRandDouble(min3, 1.3, 0.1, 1);
				dictionary2[enumMaterialCategory.Bauxite] = (double)Material_4 * Utils.GetRandDouble(min3, 1.3, 0.1, 1);
				if (num == 0)
				{
					min3 = ((!randMaxFlag) ? 0.5 : 0.85);
					dictionary2[enumMaterialCategory.Fuel] *= Utils.GetRandDouble(min3, 0.85, 0.01, 2);
					dictionary2[enumMaterialCategory.Bull] *= Utils.GetRandDouble(min3, 0.85, 0.01, 2);
					dictionary2[enumMaterialCategory.Steel] *= Utils.GetRandDouble(min3, 0.85, 0.01, 2);
					dictionary2[enumMaterialCategory.Bauxite] *= Utils.GetRandDouble(min3, 0.85, 0.01, 2);
				}
				int num5 = num * 2;
				if (num5 < tankerNum && num < 6)
				{
					min3 = ((!randMaxFlag) ? 0.75 : 0.95);
					dictionary2[enumMaterialCategory.Fuel] *= Utils.GetRandDouble(min3, 0.95, 0.01, 2);
					dictionary2[enumMaterialCategory.Bull] *= Utils.GetRandDouble(min3, 0.95, 0.01, 2);
					dictionary2[enumMaterialCategory.Steel] *= Utils.GetRandDouble(min3, 0.95, 0.01, 2);
					dictionary2[enumMaterialCategory.Bauxite] *= Utils.GetRandDouble(min3, 0.95, 0.01, 2);
				}
			}
			double num6 = 1.0;
			switch (Comm_UserDatas.Instance.User_basic.Difficult)
			{
			case DifficultKind.SHI:
				num6 = 1.0;
				break;
			case DifficultKind.KOU:
				num6 = 2.0;
				break;
			case DifficultKind.OTU:
				num6 = 2.5;
				break;
			case DifficultKind.HEI:
				num6 = 3.0;
				break;
			case DifficultKind.TEI:
				num6 = 4.0;
				break;
			}
			foreach (KeyValuePair<enumMaterialCategory, double> item in dictionary2)
			{
				double num7 = (item.Key != enumMaterialCategory.Bauxite) ? 1.0 : 0.65;
				double a = item.Value * num6 * num7;
				int num8 = (int)(Math.Ceiling(a) / 5.0);
				int num9 = 5 * num8;
				Dictionary<enumMaterialCategory, int> dictionary3;
				Dictionary<enumMaterialCategory, int> dictionary4 = dictionary3 = addValues;
				enumMaterialCategory key;
				enumMaterialCategory key2 = key = item.Key;
				int num10 = dictionary3[key];
				dictionary4[key2] = num10 + num9;
			}
		}
	}
}
