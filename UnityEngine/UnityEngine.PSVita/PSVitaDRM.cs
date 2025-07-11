using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.PSVita;

public sealed class PSVitaDRM
{
	public struct DrmContentFinder
	{
		public int dirHandle;

		private IntPtr _contentDir;

		public string contentDir => Marshal.PtrToStringAnsi(_contentDir);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool ContentFinderOpen(ref DrmContentFinder finder);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool ContentFinderNext(ref DrmContentFinder finder);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool ContentFinderClose(ref DrmContentFinder finder);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool ContentOpen(string contentDir);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool ContentClose(string contentDir);
}
