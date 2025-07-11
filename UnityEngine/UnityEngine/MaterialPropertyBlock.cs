using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class MaterialPropertyBlock
{
	internal IntPtr m_Ptr;

	public extern bool isEmpty
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public MaterialPropertyBlock()
	{
		InitBlock();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void InitBlock();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void DestroyBlock();

	~MaterialPropertyBlock()
	{
		DestroyBlock();
	}

	public void SetFloat(string name, float value)
	{
		SetFloat(Shader.PropertyToID(name), value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetFloat(int nameID, float value);

	public void SetVector(string name, Vector4 value)
	{
		SetVector(Shader.PropertyToID(name), value);
	}

	public void SetVector(int nameID, Vector4 value)
	{
		INTERNAL_CALL_SetVector(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetVector(MaterialPropertyBlock self, int nameID, ref Vector4 value);

	public void SetColor(string name, Color value)
	{
		SetColor(Shader.PropertyToID(name), value);
	}

	public void SetColor(int nameID, Color value)
	{
		INTERNAL_CALL_SetColor(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetColor(MaterialPropertyBlock self, int nameID, ref Color value);

	public void SetMatrix(string name, Matrix4x4 value)
	{
		SetMatrix(Shader.PropertyToID(name), value);
	}

	public void SetMatrix(int nameID, Matrix4x4 value)
	{
		INTERNAL_CALL_SetMatrix(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetMatrix(MaterialPropertyBlock self, int nameID, ref Matrix4x4 value);

	public void SetTexture(string name, Texture value)
	{
		SetTexture(Shader.PropertyToID(name), value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetTexture(int nameID, Texture value);

	public void AddFloat(string name, float value)
	{
		AddFloat(Shader.PropertyToID(name), value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void AddFloat(int nameID, float value);

	public void AddVector(string name, Vector4 value)
	{
		AddVector(Shader.PropertyToID(name), value);
	}

	public void AddVector(int nameID, Vector4 value)
	{
		INTERNAL_CALL_AddVector(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_AddVector(MaterialPropertyBlock self, int nameID, ref Vector4 value);

	public void AddColor(string name, Color value)
	{
		AddColor(Shader.PropertyToID(name), value);
	}

	public void AddColor(int nameID, Color value)
	{
		INTERNAL_CALL_AddColor(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_AddColor(MaterialPropertyBlock self, int nameID, ref Color value);

	public void AddMatrix(string name, Matrix4x4 value)
	{
		AddMatrix(Shader.PropertyToID(name), value);
	}

	public void AddMatrix(int nameID, Matrix4x4 value)
	{
		INTERNAL_CALL_AddMatrix(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_AddMatrix(MaterialPropertyBlock self, int nameID, ref Matrix4x4 value);

	public void AddTexture(string name, Texture value)
	{
		AddTexture(Shader.PropertyToID(name), value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void AddTexture(int nameID, Texture value);

	public float GetFloat(string name)
	{
		return GetFloat(Shader.PropertyToID(name));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float GetFloat(int nameID);

	public Vector4 GetVector(string name)
	{
		return GetVector(Shader.PropertyToID(name));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Vector4 GetVector(int nameID);

	public Matrix4x4 GetMatrix(string name)
	{
		return GetMatrix(Shader.PropertyToID(name));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Matrix4x4 GetMatrix(int nameID);

	public Texture GetTexture(string name)
	{
		return GetTexture(Shader.PropertyToID(name));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Texture GetTexture(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Clear();
}
