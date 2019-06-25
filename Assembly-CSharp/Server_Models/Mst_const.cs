using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_const : Model_Base
	{
		private MstConstDataIndex _id;

		private int _int_value;

		private static string _tableName = "mst_const";

		public MstConstDataIndex Id
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

		public int Int_value
		{
			get
			{
				return _int_value;
			}
			private set
			{
				_int_value = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			Id = (MstConstDataIndex)int.Parse(element.Element("Id").Value);
			Int_value = int.Parse(element.Element("Int_value").Value);
		}
	}
}
