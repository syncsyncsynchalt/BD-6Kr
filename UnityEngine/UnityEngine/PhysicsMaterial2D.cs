using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class PhysicsMaterial2D : Object
{
	public extern float bounciness
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float friction
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public PhysicsMaterial2D()
	{
		Internal_Create(this, null);
	}

	public PhysicsMaterial2D(string name)
	{
		Internal_Create(this, name);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void Internal_Create([Writable] PhysicsMaterial2D mat, string name);
}
