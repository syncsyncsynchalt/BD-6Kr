using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class User_BookFmt<T> where T : IBookData, new()
	{
		public int IndexNo;

		public List<int> Ids;

		public List<List<int>> State;

		public T Detail;

		public User_BookFmt(int table_id, List<int> ids, List<List<int>> state, string info, string cname, object hosoku)
		{
			Detail = new T();
			IndexNo = Detail.GetSortNo(table_id);
			Ids = ids;
			State = state;
			Detail.SetBookData(table_id, info, cname, hosoku);
		}
	}
}
