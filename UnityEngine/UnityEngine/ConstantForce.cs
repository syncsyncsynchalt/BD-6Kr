using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class ConstantForce : Behaviour
{
	public Vector3 force
	{
		get
		{
			INTERNAL_get_force(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_force(ref value);
		}
	}

	public Vector3 relativeForce
	{
		get
		{
			INTERNAL_get_relativeForce(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_relativeForce(ref value);
		}
	}

	public Vector3 torque
	{
		get
		{
			INTERNAL_get_torque(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_torque(ref value);
		}
	}

	public Vector3 relativeTorque
	{
		get
		{
			INTERNAL_get_relativeTorque(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_relativeTorque(ref value);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_force(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_force(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_relativeForce(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_relativeForce(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_torque(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_torque(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_relativeTorque(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_relativeTorque(ref Vector3 value);
}
