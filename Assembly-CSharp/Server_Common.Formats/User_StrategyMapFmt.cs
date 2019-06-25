using Common.Enum;
using Server_Models;

namespace Server_Common.Formats
{
	public class User_StrategyMapFmt
	{
		private bool isActiveArea;

		private Mst_maparea maparea;

		private RebellionState rebellionState;

		public bool IsActiveArea
		{
			get
			{
				return isActiveArea;
			}
			set
			{
				isActiveArea = value;
			}
		}

		public Mst_maparea Maparea
		{
			get
			{
				return maparea;
			}
			set
			{
				maparea = value;
			}
		}

		public RebellionState RebellionState
		{
			get
			{
				return rebellionState;
			}
			set
			{
				rebellionState = value;
			}
		}

		public User_StrategyMapFmt(Mst_maparea mst_maparea, bool flag)
		{
			maparea = mst_maparea;
			IsActiveArea = flag;
		}
	}
}
