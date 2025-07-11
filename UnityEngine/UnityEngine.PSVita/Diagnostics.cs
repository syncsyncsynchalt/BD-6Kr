using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita;

public sealed class Diagnostics
{
	public static extern bool enableHUD
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetFreeMemoryLPDDR();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetFreeMemoryCDRAM();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetFreeMemoryPHYSCONT();
}
