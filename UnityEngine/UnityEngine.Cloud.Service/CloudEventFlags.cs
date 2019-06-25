using System;

namespace UnityEngine.Cloud.Service
{
	[Flags]
	internal enum CloudEventFlags
	{
		None = 0x0,
		HighPriority = 0x1,
		CacheImmediately = 0x2
	}
}
