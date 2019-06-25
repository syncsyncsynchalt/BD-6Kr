using Common.Enum;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_quest : Model_Base
	{
		private int _id;

		private int _category;

		private int _type;

		private int _torigger;

		private int _sub_torigger;

		private string _name;

		private string _details;

		private int _get_1_type;

		private int _get_1_count;

		private int _get_1_id;

		private int _get_2_type;

		private int _get_2_count;

		private int _get_2_id;

		private int _mat1;

		private int _mat2;

		private int _mat3;

		private int _mat4;

		private static string _tableName = "mst_quest";

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

		public int Category
		{
			get
			{
				return _category;
			}
			private set
			{
				_category = value;
			}
		}

		public int Type
		{
			get
			{
				return _type;
			}
			private set
			{
				_type = value;
			}
		}

		public int Torigger
		{
			get
			{
				return _torigger;
			}
			private set
			{
				_torigger = value;
			}
		}

		public int Sub_torigger
		{
			get
			{
				return _sub_torigger;
			}
			private set
			{
				_sub_torigger = value;
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

		public int Get_1_type
		{
			get
			{
				return _get_1_type;
			}
			private set
			{
				_get_1_type = value;
			}
		}

		public int Get_1_count
		{
			get
			{
				return _get_1_count;
			}
			private set
			{
				_get_1_count = value;
			}
		}

		public int Get_1_id
		{
			get
			{
				return _get_1_id;
			}
			private set
			{
				_get_1_id = value;
			}
		}

		public int Get_2_type
		{
			get
			{
				return _get_2_type;
			}
			private set
			{
				_get_2_type = value;
			}
		}

		public int Get_2_count
		{
			get
			{
				return _get_2_count;
			}
			private set
			{
				_get_2_count = value;
			}
		}

		public int Get_2_id
		{
			get
			{
				return _get_2_id;
			}
			private set
			{
				_get_2_id = value;
			}
		}

		public int Mat1
		{
			get
			{
				return _mat1;
			}
			private set
			{
				_mat1 = value;
			}
		}

		public int Mat2
		{
			get
			{
				return _mat2;
			}
			private set
			{
				_mat2 = value;
			}
		}

		public int Mat3
		{
			get
			{
				return _mat3;
			}
			private set
			{
				_mat3 = value;
			}
		}

		public int Mat4
		{
			get
			{
				return _mat4;
			}
			private set
			{
				_mat4 = value;
			}
		}

		public static string tableName => _tableName;

		public Dictionary<enumMaterialCategory, int> GetMaterialValues()
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			foreach (int value in Enum.GetValues(typeof(enumMaterialCategory)))
			{
				dictionary.Add((enumMaterialCategory)value, 0);
			}
			dictionary[enumMaterialCategory.Fuel] = Mat1;
			dictionary[enumMaterialCategory.Bull] = Mat2;
			dictionary[enumMaterialCategory.Steel] = Mat3;
			dictionary[enumMaterialCategory.Bauxite] = Mat4;
			dictionary[enumMaterialCategory.Build_Kit] = getItemToMaterialBounus(enumMaterialCategory.Build_Kit);
			dictionary[enumMaterialCategory.Repair_Kit] = getItemToMaterialBounus(enumMaterialCategory.Repair_Kit);
			dictionary[enumMaterialCategory.Dev_Kit] = getItemToMaterialBounus(enumMaterialCategory.Dev_Kit);
			dictionary[enumMaterialCategory.Revamp_Kit] = getItemToMaterialBounus(enumMaterialCategory.Revamp_Kit);
			return dictionary;
		}

		public int GetSpointNum()
		{
			int num = 101;
			if (Get_1_type == num)
			{
				return Get_1_count;
			}
			if (Get_2_type == num)
			{
				return Get_2_count;
			}
			return 0;
		}

		private int getItemToMaterialBounus(enumMaterialCategory getType)
		{
			int num = 1;
			if (Get_1_type == num && getType == (enumMaterialCategory)Get_1_id)
			{
				return Get_1_count;
			}
			if (Get_2_type == num && getType == (enumMaterialCategory)Get_2_id)
			{
				return Get_2_count;
			}
			return 0;
		}

		public int GetSlotModelChangeId(int type)
		{
			int result = 0;
			if (type == 1 && Get_1_type == 15)
			{
				result = Get_1_id;
			}
			if (type == 2 && Get_2_type == 15)
			{
				result = Get_2_id;
			}
			return result;
		}

		public bool IsLevelMax(List<int> mst_slotitemchange)
		{
			if (mst_slotitemchange[2] == 1)
			{
				return true;
			}
			return false;
		}

		public bool IsUseCrew(List<int> mst_slotitemchange)
		{
			return (mst_slotitemchange[3] == 1) ? true : false;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Id = int.Parse(element.Element("Id").Value);
			Category = int.Parse(element.Element("Category").Value);
			Type = int.Parse(element.Element("Type").Value);
			if (element.Element("Torigger") != null)
			{
				string[] array = element.Element("Torigger").Value.Split(c);
				int[] array2 = Array.ConvertAll(array, (string x) => int.Parse(x));
				Torigger = array2[0];
				if (array2.Length > 1)
				{
					Sub_torigger = array2[1];
				}
			}
			Name = element.Element("Name").Value;
			Details = element.Element("Details").Value;
			if (element.Element("Get_1") != null)
			{
				string[] array3 = element.Element("Get_1").Value.Split(c);
				int[] array4 = Array.ConvertAll(array3, (string x) => int.Parse(x));
				Get_1_type = array4[0];
				Get_1_count = array4[1];
				Get_1_id = array4[2];
			}
			if (element.Element("Get_2") != null)
			{
				string[] array5 = element.Element("Get_2").Value.Split(c);
				int[] array6 = Array.ConvertAll(array5, (string x) => int.Parse(x));
				Get_2_type = array6[0];
				Get_2_count = array6[1];
				Get_2_id = array6[2];
			}
			string[] array7 = element.Element("Mat").Value.Split(c);
			int[] array8 = Array.ConvertAll(array7, (string x) => int.Parse(x));
			Mat1 = array8[0];
			Mat2 = array8[1];
			Mat3 = array8[2];
			Mat4 = array8[3];
		}
	}
}
