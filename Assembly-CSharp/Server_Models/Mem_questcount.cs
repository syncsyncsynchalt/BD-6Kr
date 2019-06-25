using Server_Common;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_questcount", Namespace = "")]
	public class Mem_questcount : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _value;

		private static string _tableName = "mem_questcount";

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

		public Mem_questcount()
		{
		}

		public Mem_questcount(int quest_id, int value)
		{
			Rid = quest_id;
			Value = value;
		}

		public void AddCount(int addValue)
		{
			Value += addValue;
		}

		public void Reset(bool deleteFlag)
		{
			Value = 0;
			if (deleteFlag)
			{
				Comm_UserDatas.Instance.User_questcount.Remove(Rid);
			}
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Value = int.Parse(element.Element("_value").Value);
		}
	}
}
