using Server_Models;

namespace Server_Common.Formats
{
	public class BookShipData : IBookData
	{
		private Mst_ship _mstData;

		private string _info;

		private string _className;

		public Mst_ship MstData
		{
			get
			{
				return _mstData;
			}
			private set
			{
				_mstData = value;
			}
		}

		public string Info
		{
			get
			{
				return _info;
			}
			private set
			{
				_info = value;
			}
		}

		public string ClassName
		{
			get
			{
				return _className;
			}
			private set
			{
				_className = value;
			}
		}

		public void SetBookData(int mst_id, string info, string cname, object hosoku)
		{
			MstData = Mst_DataManager.Instance.Mst_ship[mst_id];
			Info = info;
			ClassName = cname;
		}

		public int GetSortNo(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_ship[mst_id].Bookno;
		}
	}
}
