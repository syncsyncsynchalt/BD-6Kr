using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_useitem", Namespace = "")]
	public class Mem_useitem : Model_Base
	{
		public const int DefItemMaxNum = 3000;

		[DataMember]
		private int _rid;

		[DataMember]
		private int _value;

		private static string _tableName = "mem_useitem";

		public int Rid
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

		public Mem_useitem()
		{
		}

		public Mem_useitem(int rid, int value)
		{
			Rid = rid;
			Value = value;
		}

		public int Add_UseItem(int num)
		{
			Value += num;
			if (Value > 3000)
			{
				Value = 3000;
			}
			return Value;
		}

		public int Sub_UseItem(int num)
		{
			Value -= num;
			if (Value < 0)
			{
				Value = 0;
			}
			return Value;
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Value = int.Parse(element.Element("_value").Value);
		}
	}
}
