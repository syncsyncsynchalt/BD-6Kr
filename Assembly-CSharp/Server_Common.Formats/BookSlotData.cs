using Server_Models;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class BookSlotData : IBookData
	{
		private Mst_slotitem _mstData;

		private List<int> _enableStype;

		private string _info;

		public Mst_slotitem MstData
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

		public List<int> EnableStype
		{
			get
			{
				return _enableStype;
			}
			private set
			{
				_enableStype = value;
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

		public void SetBookData(int mst_id, string info, string cname, object hosoku)
		{
			MstData = Mst_DataManager.Instance.Mst_Slotitem[mst_id];
			Info = info;
			EnableStype = ((Dictionary<int, List<int>>)hosoku)[MstData.Type3];
		}

		public int GetSortNo(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_Slotitem[mst_id].Sortno;
		}
	}
}
