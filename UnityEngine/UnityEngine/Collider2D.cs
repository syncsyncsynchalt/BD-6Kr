using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine;

public class Collider2D : Behaviour
{
	public extern bool isTrigger
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool usedByEffector
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Vector2 offset
	{
		get
		{
			INTERNAL_get_offset(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_offset(ref value);
		}
	}

	public extern Rigidbody2D attachedRigidbody
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int shapeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Bounds bounds
	{
		get
		{
			INTERNAL_get_bounds(out var value);
			return value;
		}
	}

	internal extern ColliderErrorState2D errorState
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern PhysicsMaterial2D sharedMaterial
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
	private extern void INTERNAL_get_offset(out Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_offset(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_bounds(out Bounds value);

	public bool OverlapPoint(Vector2 point)
	{
		return INTERNAL_CALL_OverlapPoint(this, ref point);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_OverlapPoint(Collider2D self, ref Vector2 point);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool IsTouching(Collider2D collider);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

	[ExcludeFromDocs]
	public bool IsTouchingLayers()
	{
		int layerMask = -1;
		return IsTouchingLayers(layerMask);
	}
}
