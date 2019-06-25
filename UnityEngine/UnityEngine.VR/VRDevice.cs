using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.VR
{
	public sealed class VRDevice
	{
		public static bool isPresent
		{
			get;
		}

		public static string family
		{
			get;
		}

		public static string model
		{
			get;
		}

		public static IntPtr GetNativePtr() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
