using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_book", Namespace = "")]
	public class Mem_book : Model_Base
	{
		[DataMember]
		private int _type;

		[DataMember]
		private int _table_id;

		[DataMember]
		private int _flag1;

		[DataMember]
		private int _flag2;

		[DataMember]
		private int _flag3;

		[DataMember]
		private int _flag4;

		[DataMember]
		private int _flag5;

		private static string _tableName = "mem_book";

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

		public int Table_id
		{
			get
			{
				return _table_id;
			}
			private set
			{
				_table_id = value;
			}
		}

		public int Flag1
		{
			get
			{
				return _flag1;
			}
			private set
			{
				_flag1 = value;
			}
		}

		public int Flag2
		{
			get
			{
				return _flag2;
			}
			private set
			{
				_flag2 = value;
			}
		}

		public int Flag3
		{
			get
			{
				return _flag3;
			}
			private set
			{
				_flag3 = value;
			}
		}

		public int Flag4
		{
			get
			{
				return _flag4;
			}
			private set
			{
				_flag4 = value;
			}
		}

		public int Flag5
		{
			get
			{
				return _flag5;
			}
			private set
			{
				_flag5 = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_book()
		{
			Flag1 = 1;
			Flag2 = 0;
			Flag3 = 0;
			Flag4 = 0;
			Flag5 = 0;
		}

		public Mem_book(int type, int mst_id)
			: this()
		{
			Type = type;
			Table_id = mst_id;
		}

		public void UpdateShipBook(bool damage, bool mariage)
		{
			if (Type == 1)
			{
				if (Flag2 == 0 && damage)
				{
					Flag2 = 1;
				}
				if (Flag3 == 0 && mariage)
				{
					Flag3 = 1;
				}
			}
		}

		protected override void setProperty(XElement element)
		{
			Type = int.Parse(element.Element("_type").Value);
			Table_id = int.Parse(element.Element("_table_id").Value);
			Flag1 = int.Parse(element.Element("_flag1").Value);
			Flag2 = int.Parse(element.Element("_flag2").Value);
			Flag3 = int.Parse(element.Element("_flag3").Value);
			Flag4 = int.Parse(element.Element("_flag4").Value);
			Flag5 = int.Parse(element.Element("_flag5").Value);
		}
	}
}
