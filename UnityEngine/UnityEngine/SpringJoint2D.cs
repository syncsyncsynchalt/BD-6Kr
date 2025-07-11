using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class SpringJoint2D : AnchoredJoint2D
{
	public extern float distance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float dampingRatio
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float frequency
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Vector2 GetReactionForce(float timeStep)
	{
		SpringJoint2D_CUSTOM_INTERNAL_GetReactionForce(this, timeStep, out var value);
		return value;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void SpringJoint2D_CUSTOM_INTERNAL_GetReactionForce(SpringJoint2D joint, float timeStep, out Vector2 value);

	public float GetReactionTorque(float timeStep)
	{
		return INTERNAL_CALL_GetReactionTorque(this, timeStep);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern float INTERNAL_CALL_GetReactionTorque(SpringJoint2D self, float timeStep);
}
