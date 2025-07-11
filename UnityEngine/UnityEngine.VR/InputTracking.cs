using System.Runtime.CompilerServices;

namespace UnityEngine.VR;

public sealed class InputTracking
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Vector3 GetLocalPosition(VRNode node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Quaternion GetLocalRotation(VRNode node);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void Recenter();
}
