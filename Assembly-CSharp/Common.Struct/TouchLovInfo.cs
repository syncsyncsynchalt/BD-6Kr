namespace Common.Struct
{
	public struct TouchLovInfo
	{
		public bool BackTouch;

		public int VoiceType;

		public int AddValueOnce;

		public int AddValueSecond;

		public int SubValue;

		public int SubMoreThanLimitCount;

		public TouchLovInfo(int voicetype, bool backTouch)
		{
			BackTouch = backTouch;
			VoiceType = voicetype;
			AddValueOnce = 0;
			AddValueSecond = 0;
			SubValue = 0;
			SubMoreThanLimitCount = 0;
			if (backTouch)
			{
				setSumValueBackTouch(voicetype);
			}
			else
			{
				setSumValueNormalTouch(voicetype);
			}
		}

		private void setSumValueNormalTouch(int voicetype)
		{
			switch (voicetype)
			{
			case 2:
				AddValueOnce = 1;
				break;
			case 3:
				AddValueOnce = 2;
				break;
			case 4:
				AddValueOnce = 2;
				SubValue = -3;
				SubMoreThanLimitCount = 2;
				break;
			}
		}

		private void setSumValueBackTouch(int voicetype)
		{
			switch (voicetype)
			{
			case 3:
				AddValueOnce = 2;
				break;
			case 4:
				SubValue = -3;
				break;
			case 2:
			case 28:
				AddValueOnce = 3;
				AddValueSecond = 2;
				break;
			}
		}

		public int GetSumValue(int nowTouchNum)
		{
			if (SubMoreThanLimitCount > 0 && SubMoreThanLimitCount <= nowTouchNum)
			{
				return SubValue;
			}
			if (SubValue > 0)
			{
				return SubValue;
			}
			switch (nowTouchNum)
			{
			case 1:
				return AddValueOnce;
			case 2:
				return AddValueSecond;
			default:
				return 0;
			}
		}
	}
}
