using Common.Struct;
using Server_Common.Formats;

namespace local.models
{
	public class HistoryModel_AreaStart : HistoryModelBase
	{
		public int AreaId => _fmt.MapInfo.Maparea_id;

		public string AreaName => _fmt.AreaName;

		public int MapNo => _fmt.MapInfo.No;

		public int MapId => _fmt.MapInfo.Id;

		public string MapName => _fmt.MapInfo.Name;

		public HistoryModel_AreaStart(User_HistoryFmt fmt)
			: base(fmt)
		{
		}

		public override string ToString()
		{
			TurnString dateStruct = base.DateStruct;
			string year = dateStruct.Year;
			TurnString dateStruct2 = base.DateStruct;
			string month = dateStruct2.Month;
			TurnString dateStruct3 = base.DateStruct;
			string text = $"{year}年{month} {dateStruct3.Day}日";
			return $"{text} {AreaName}[#{AreaId}-{MapNo}]({MapName}) 攻略開始";
		}
	}
}
