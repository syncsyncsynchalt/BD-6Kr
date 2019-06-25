using Common.Enum;
using Common.Struct;
using Server_Models;

namespace Server_Common.Formats
{
	public class User_HistoryFmt
	{
		public HistoryType Type;

		public TurnString DateString;

		public string AreaName;

		public Mst_mapinfo MapInfo;

		public Mst_ship FlagShip;

		public User_HistoryFmt()
		{
		}

		public User_HistoryFmt(Mem_history memObj)
		{
			setHistoryType(memObj);
			DateString = Comm_UserDatas.Instance.User_turn.GetTurnString(memObj.Turn);
			if (Mst_DataManager.Instance.Mst_mapinfo.TryGetValue(memObj.MapinfoId, out MapInfo))
			{
				AreaName = Mst_DataManager.Instance.Mst_maparea[MapInfo.Maparea_id].Name;
			}
			Mst_DataManager.Instance.Mst_ship.TryGetValue(memObj.FlagShipId, out FlagShip);
		}

		private void setHistoryType(Mem_history mem_history)
		{
			if (mem_history.Type == 1)
			{
				if (mem_history.MapClearNum == 1)
				{
					Type = HistoryType.MapClear1;
				}
				else if (mem_history.MapClearNum == 2)
				{
					Type = HistoryType.MapClear2;
				}
				else
				{
					Type = HistoryType.MapClear3;
				}
			}
			else if (mem_history.Type == 2)
			{
				Type = ((!mem_history.TankerLostAll) ? HistoryType.TankerLostHalf : HistoryType.TankerLostAll);
			}
			else if (mem_history.Type == 3)
			{
				Type = HistoryType.NewAreaOpen;
			}
			else if (mem_history.Type == 4)
			{
				switch (mem_history.GameEndType)
				{
				case 1:
					Type = HistoryType.GameClear;
					break;
				case 2:
					Type = HistoryType.GameOverLost;
					break;
				case 3:
					Type = HistoryType.GameOverTurn;
					break;
				}
			}
		}
	}
}
