using Common.Enum;

namespace KCV.BattleCut
{
	public static class BattleCommandExtensions
	{
		public static string GetString(this BattleCommand iCommand)
		{
			string empty = string.Empty;
			switch (iCommand)
			{
			case BattleCommand.None:
				return "なし";
			case BattleCommand.Sekkin:
				return "接近";
			case BattleCommand.Hougeki:
				return "砲撃";
			case BattleCommand.Raigeki:
				return "雷撃";
			case BattleCommand.Ridatu:
				return "離脱";
			case BattleCommand.Taisen:
				return "対潜";
			case BattleCommand.Kaihi:
				return "回避";
			case BattleCommand.Kouku:
				return "航空";
			case BattleCommand.Totugeki:
				return "突撃";
			case BattleCommand.Tousha:
				return "統射";
			default:
				return string.Empty;
			}
		}
	}
}
