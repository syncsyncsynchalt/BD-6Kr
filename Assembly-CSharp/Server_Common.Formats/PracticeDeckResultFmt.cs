using Common.Struct;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class PracticeDeckResultFmt
	{
		public MissionResultFmt PracticeResult;

		public Dictionary<int, PowUpInfo> PowerUpData;

		public PracticeDeckResultFmt()
		{
			PracticeResult = new MissionResultFmt();
			PowerUpData = new Dictionary<int, PowUpInfo>();
		}
	}
}
