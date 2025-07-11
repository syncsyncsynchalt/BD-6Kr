using System.Runtime.CompilerServices;

namespace UnityEngine;

internal struct RenderBufferHelper
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern int GetLoadAction(out RenderBuffer b);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern void SetLoadAction(out RenderBuffer b, int a);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern int GetStoreAction(out RenderBuffer b);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern void SetStoreAction(out RenderBuffer b, int a);
}
