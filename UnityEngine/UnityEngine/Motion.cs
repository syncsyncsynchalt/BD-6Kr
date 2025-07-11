using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public class Motion : Object
{
	public extern float averageDuration
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern float averageAngularSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Vector3 averageSpeed
	{
		get
		{
			INTERNAL_get_averageSpeed(out var value);
			return value;
		}
	}

	public extern float apparentSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool isLooping
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool legacy
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool isHumanMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	[Obsolete("isAnimatorMotion is not supported anymore. Use !legacy instead.", true)]
	public extern bool isAnimatorMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_averageSpeed(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	[Obsolete("ValidateIfRetargetable is not supported anymore. Use isHumanMotion instead.", true)]
	public extern bool ValidateIfRetargetable(bool val);
}
