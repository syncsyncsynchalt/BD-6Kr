using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita;

public sealed class PSVitaPlayerPrefs
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void LoadFromByteArray(byte[] bytes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern byte[] SaveToByteArray();
}
