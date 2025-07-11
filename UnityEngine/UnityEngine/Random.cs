using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class Random
{
	public static extern int seed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern float value
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static Vector3 insideUnitSphere
	{
		get
		{
			INTERNAL_get_insideUnitSphere(out var result);
			return result;
		}
	}

	public static Vector2 insideUnitCircle
	{
		get
		{
			GetRandomUnitCircle(out var output);
			return output;
		}
	}

	public static Vector3 onUnitSphere
	{
		get
		{
			INTERNAL_get_onUnitSphere(out var result);
			return result;
		}
	}

	public static Quaternion rotation
	{
		get
		{
			INTERNAL_get_rotation(out var result);
			return result;
		}
	}

	public static Quaternion rotationUniform
	{
		get
		{
			INTERNAL_get_rotationUniform(out var result);
			return result;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern float Range(float min, float max);

	public static int Range(int min, int max)
	{
		return RandomRangeInt(min, max);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int RandomRangeInt(int min, int max);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_get_insideUnitSphere(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void GetRandomUnitCircle(out Vector2 output);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_get_onUnitSphere(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_get_rotation(out Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_get_rotationUniform(out Quaternion value);

	[Obsolete("Use Random.Range instead")]
	public static float RandomRange(float min, float max)
	{
		return Range(min, max);
	}

	[Obsolete("Use Random.Range instead")]
	public static int RandomRange(int min, int max)
	{
		return Range(min, max);
	}
}
