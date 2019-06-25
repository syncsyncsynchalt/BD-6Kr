using Common.Enum;
using Server_Common.Formats;
using System.Collections.Generic;

namespace local.models
{
	public class CellModel
	{
		private User_MapCellInfo _data;

		public int AreaId => _data.Mst_mapcell.Maparea_id;

		public int MapNo => _data.Mst_mapcell.Mapinfo_no;

		public int CellNo => _data.Mst_mapcell.No;

		public int ColorNo => _data.Mst_mapcell.Color_no;

		public enumMapEventType EventType => _data.Mst_mapcell.Event_1;

		public enumMapWarType WarType => _data.Mst_mapcell.Event_2;

		public bool Passed => _data.Passed;

		public CellModel(User_MapCellInfo data)
		{
			_data = data;
		}

		public List<int> GetLinkNo()
		{
			return _data.Mst_mapcell.GetLinkNo();
		}

		public override string ToString()
		{
			string[] array = new string[10]
			{
				"白",
				"青",
				"緑",
				"紫",
				"赤",
				"赤(BOSS)",
				"赤",
				"航空戦マス",
				"補給EO",
				"航空偵察"
			};
			string str = $"#{AreaId}-{MapNo} セル{CellNo}({array[ColorNo]})";
			str += $" {EventType}-{WarType}";
			return str + string.Format(" {0}", (!Passed) ? "未通過" : "通過済");
		}
	}
}
