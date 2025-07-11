using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class ProceduralMaterial : Material
{
	public static extern bool isSupported
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal ProceduralMaterial()
		: base((Material)null)
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void StopRebuilds();
}
