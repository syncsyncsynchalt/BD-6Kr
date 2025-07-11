using System.Runtime.CompilerServices;

namespace UnityEngine;

public class AnchoredJoint2D : Joint2D
{
	public Vector2 anchor
	{
		get
		{
			INTERNAL_get_anchor(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_anchor(ref value);
		}
	}

	public Vector2 connectedAnchor
	{
		get
		{
			INTERNAL_get_connectedAnchor(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_connectedAnchor(ref value);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_anchor(out Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_anchor(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_connectedAnchor(out Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_connectedAnchor(ref Vector2 value);
}
