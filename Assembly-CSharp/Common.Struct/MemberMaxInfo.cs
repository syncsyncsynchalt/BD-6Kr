namespace Common.Struct
{
	public struct MemberMaxInfo
	{
		public int NowCount;

		public int MaxCount;

		public MemberMaxInfo(int now, int max)
		{
			NowCount = now;
			MaxCount = max;
		}
	}
}
