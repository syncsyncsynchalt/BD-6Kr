using Common.Enum;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_material", Namespace = "")]
	public class Mem_material : Model_Base
	{
		[DataMember]
		private enumMaterialCategory _rid;

		[DataMember]
		private int _value;

		private static string _tableName = "mem_material";

		public enumMaterialCategory Rid
		{
			get
			{
				return _rid;
			}
			private set
			{
				_rid = value;
			}
		}

		public int Value
		{
			get
			{
				return _value;
			}
			private set
			{
				_value = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_material()
		{
		}

		public Mem_material(enumMaterialCategory category, int value)
		{
			Rid = category;
			Value = value;
		}

		public int Add_Material(int num)
		{
			int materialLimit = Mst_DataManager.Instance.Mst_item_limit[1].GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, Rid);
			if (materialLimit > Value + num)
			{
				Value += num;
			}
			else
			{
				Value = materialLimit;
			}
			return Value;
		}

		public int Sub_Material(int num)
		{
			if (Value - num >= 0)
			{
				Value -= num;
			}
			return Value;
		}

		protected override void setProperty(XElement element)
		{
			Rid = (enumMaterialCategory)(int)Enum.Parse(typeof(enumMaterialCategory), element.Element("_rid").Value);
			Value = int.Parse(element.Element("_value").Value);
		}
	}
}
