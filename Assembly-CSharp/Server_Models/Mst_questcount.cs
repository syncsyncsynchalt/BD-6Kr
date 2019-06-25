using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_questcount : Model_Base
	{
		private int _id;

		private HashSet<int> _counter_id;

		private Dictionary<int, int> _clear_num;

		private Dictionary<int, bool> _reset_flag;

		private static string _tableName = "mst_questcount";

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

		public HashSet<int> Counter_id
		{
			get
			{
				return _counter_id;
			}
			private set
			{
				_counter_id = value;
			}
		}

		public Dictionary<int, int> Clear_num
		{
			get
			{
				return _clear_num;
			}
			private set
			{
				_clear_num = value;
			}
		}

		public Dictionary<int, bool> Reset_flag
		{
			get
			{
				return _reset_flag;
			}
			private set
			{
				_reset_flag = value;
			}
		}

		public static string tableName => _tableName;

		public Mst_questcount()
		{
			Counter_id = new HashSet<int>();
			Clear_num = new Dictionary<int, int>();
			Reset_flag = new Dictionary<int, bool>();
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			Id = int.Parse(element.Element("Id").Value);
			string[] array = element.Element("Counter_id").Value.Split(c);
			int[] array2 = Array.ConvertAll(array, (string x) => int.Parse(x));
			string[] array3 = element.Element("Clear_num").Value.Split(c);
			int[] array4 = Array.ConvertAll(array3, (string x) => int.Parse(x));
			byte[] array5 = null;
			if (element.Element("Reset_flag") != null)
			{
				string[] array6 = element.Element("Reset_flag").Value.Split(c);
				array5 = Array.ConvertAll(array6, (string x) => byte.Parse(x));
			}
			for (int i = 0; i < array2.Length; i++)
			{
				int num = array2[i];
				Counter_id.Add(num);
				if (i < array4.Length)
				{
					Clear_num.Add(num, array4[i]);
				}
				if (array5 != null)
				{
					bool value = (array5[i] != 0) ? true : false;
					Reset_flag.Add(num, value);
				}
			}
		}
	}
}
