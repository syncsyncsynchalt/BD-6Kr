namespace KCV.Strategy
{
	public class StrategyUtils
	{
		public static bool ChkStateRebellionTaskIsRun(StrategyRebellionTaskManagerMode iMode)
		{
			if (StrategyTaskManager.GetStrategyRebellion().GetMode() != StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF)
			{
				return (StrategyTaskManager.GetStrategyRebellion().GetMode() == iMode) ? true : false;
			}
			return true;
		}
	}
}
