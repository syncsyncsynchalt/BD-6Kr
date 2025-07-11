using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public class Texture : Object
{
	public static extern int masterTextureLimit
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern AnisotropicFiltering anisotropicFiltering
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public virtual int width
	{
		get
		{
			return Internal_GetWidth(this);
		}
		set
		{
			throw new Exception("not implemented");
		}
	}

	public virtual int height
	{
		get
		{
			return Internal_GetHeight(this);
		}
		set
		{
			throw new Exception("not implemented");
		}
	}

	public extern FilterMode filterMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int anisoLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern TextureWrapMode wrapMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float mipMapBias
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Vector2 texelSize
	{
		get
		{
			INTERNAL_get_texelSize(out var value);
			return value;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetGlobalAnisotropicFilteringLimits(int forcedMin, int globalMax);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int Internal_GetWidth(Texture mono);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int Internal_GetHeight(Texture mono);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_texelSize(out Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern IntPtr GetNativeTexturePtr();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	[Obsolete("Use GetNativeTexturePtr instead.")]
	public extern int GetNativeTextureID();
}
