using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class CharacterJoint : Joint
{
	[Obsolete("TargetRotation not in use for Unity 5 and assumed disabled.", true)]
	public Quaternion targetRotation;

	[Obsolete("TargetAngularVelocity not in use for Unity 5 and assumed disabled.", true)]
	public Vector3 targetAngularVelocity;

	[Obsolete("RotationDrive not in use for Unity 5 and assumed disabled.", true)]
	public JointDrive rotationDrive;

	public Vector3 swingAxis
	{
		get
		{
			INTERNAL_get_swingAxis(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_swingAxis(ref value);
		}
	}

	public SoftJointLimitSpring twistLimitSpring
	{
		get
		{
			INTERNAL_get_twistLimitSpring(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_twistLimitSpring(ref value);
		}
	}

	public SoftJointLimitSpring swingLimitSpring
	{
		get
		{
			INTERNAL_get_swingLimitSpring(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_swingLimitSpring(ref value);
		}
	}

	public SoftJointLimit lowTwistLimit
	{
		get
		{
			INTERNAL_get_lowTwistLimit(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_lowTwistLimit(ref value);
		}
	}

	public SoftJointLimit highTwistLimit
	{
		get
		{
			INTERNAL_get_highTwistLimit(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_highTwistLimit(ref value);
		}
	}

	public SoftJointLimit swing1Limit
	{
		get
		{
			INTERNAL_get_swing1Limit(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_swing1Limit(ref value);
		}
	}

	public SoftJointLimit swing2Limit
	{
		get
		{
			INTERNAL_get_swing2Limit(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_swing2Limit(ref value);
		}
	}

	public extern bool enableProjection
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float projectionDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float projectionAngle
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
	private extern void INTERNAL_get_swingAxis(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_swingAxis(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_twistLimitSpring(out SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_twistLimitSpring(ref SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_swingLimitSpring(out SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_swingLimitSpring(ref SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_lowTwistLimit(out SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_lowTwistLimit(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_highTwistLimit(out SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_highTwistLimit(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_swing1Limit(out SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_swing1Limit(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_swing2Limit(out SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_swing2Limit(ref SoftJointLimit value);
}
