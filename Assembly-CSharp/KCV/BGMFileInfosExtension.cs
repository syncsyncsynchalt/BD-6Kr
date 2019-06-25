namespace KCV
{
	public static class BGMFileInfosExtension
	{
		public static string BGMFileName(this BGMFileInfos info)
		{
			switch (info)
			{
			case BGMFileInfos.Strategy:
				return "103";
			case BGMFileInfos.RewardGet:
				return "RewardGet";
			case BGMFileInfos.PortTools:
				return "sound_bgm";
			default:
			{
				int num = (int)info;
				return num.ToString();
			}
			}
		}
	}
}
