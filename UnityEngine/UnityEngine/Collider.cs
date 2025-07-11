using System.Runtime.CompilerServices;

namespace UnityEngine;

public class Collider : Component
{
	public extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Rigidbody attachedRigidbody
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool isTrigger
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float contactOffset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern PhysicMaterial material
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern PhysicMaterial sharedMaterial
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Bounds bounds
	{
		get
		{
			INTERNAL_get_bounds(out var value);
			return value;
		}
	}

	public Vector3 ClosestPointOnBounds(Vector3 position)
	{
		return INTERNAL_CALL_ClosestPointOnBounds(this, ref position);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern Vector3 INTERNAL_CALL_ClosestPointOnBounds(Collider self, ref Vector3 position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_bounds(out Bounds value);

	private static bool Internal_Raycast(Collider col, Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		return INTERNAL_CALL_Internal_Raycast(col, ref ray, out hitInfo, maxDistance);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_Internal_Raycast(Collider col, ref Ray ray, out RaycastHit hitInfo, float maxDistance);

	public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		return Internal_Raycast(this, ray, out hitInfo, maxDistance);
	}
}
