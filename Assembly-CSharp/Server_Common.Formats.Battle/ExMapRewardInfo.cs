using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class ExMapRewardInfo
	{
		private int _getExMapRate;

		private List<ItemGetFmt> _getExMapItem;

		public int GetExMapRate
		{
			get
			{
				return _getExMapRate;
			}
			set
			{
				_getExMapRate = value;
			}
		}

		public List<ItemGetFmt> GetExMapItem
		{
			get
			{
				return _getExMapItem;
			}
			set
			{
				_getExMapItem = value;
			}
		}
	}
}
