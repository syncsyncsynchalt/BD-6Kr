using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.PSVita
{
	public sealed class PSVitaDRM
	{
		public struct DrmContentFinder
		{
			public int dirHandle;

			private IntPtr _contentDir;

			public string contentDir => Marshal.PtrToStringAnsi(_contentDir);
		}

		public static bool ContentFinderOpen(ref DrmContentFinder finder) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool ContentFinderNext(ref DrmContentFinder finder) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool ContentFinderClose(ref DrmContentFinder finder) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool ContentOpen(string contentDir) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool ContentClose(string contentDir) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
