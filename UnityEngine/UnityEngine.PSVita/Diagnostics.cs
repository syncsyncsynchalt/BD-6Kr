using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita
{
	public sealed class Diagnostics
	{
		public static bool enableHUD
		{
			get;
			set;
		}

		public static int GetFreeMemoryLPDDR() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetFreeMemoryCDRAM() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetFreeMemoryPHYSCONT() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
