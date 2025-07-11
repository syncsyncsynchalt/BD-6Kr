using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class BoxCollider2D : Collider2D
{
	public Vector2 size
	{
		get
		{
			INTERNAL_get_size(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_size(ref value);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_size(out Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_size(ref Vector2 value);
}
