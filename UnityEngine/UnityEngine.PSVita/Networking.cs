using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita;

public sealed class Networking
{
	public static extern bool enableUDPP2P
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}
}
