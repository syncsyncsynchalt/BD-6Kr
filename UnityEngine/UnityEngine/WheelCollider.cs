using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class WheelCollider : Collider
{
	public Vector3 center
	{
		get
		{
			INTERNAL_get_center(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_center(ref value);
		}
	}

	public extern float radius
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float suspensionDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public JointSpring suspensionSpring
	{
		get
		{
			INTERNAL_get_suspensionSpring(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_suspensionSpring(ref value);
		}
	}

	public extern float forceAppPointDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float mass
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float wheelDampingRate
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public WheelFrictionCurve forwardFriction
	{
		get
		{
			INTERNAL_get_forwardFriction(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_forwardFriction(ref value);
		}
	}

	public WheelFrictionCurve sidewaysFriction
	{
		get
		{
			INTERNAL_get_sidewaysFriction(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_sidewaysFriction(ref value);
		}
	}

	public extern float motorTorque
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float brakeTorque
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float steerAngle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool isGrounded
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern float sprungMass
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern float rpm
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_center(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_center(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_suspensionSpring(out JointSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_suspensionSpring(ref JointSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_forwardFriction(out WheelFrictionCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_forwardFriction(ref WheelFrictionCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_sidewaysFriction(out WheelFrictionCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_sidewaysFriction(ref WheelFrictionCurve value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void ConfigureVehicleSubsteps(float speedThreshold, int stepsBelowThreshold, int stepsAboveThreshold);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool GetGroundHit(out WheelHit hit);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void GetWorldPose(out Vector3 pos, out Quaternion quat);
}
