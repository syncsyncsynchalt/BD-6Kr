using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine.Rendering;

public sealed class CommandBuffer : IDisposable
{
	internal IntPtr m_Ptr;

	public extern string name
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int sizeInBytes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public CommandBuffer()
	{
		m_Ptr = IntPtr.Zero;
		InitBuffer(this);
	}

	~CommandBuffer()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		ReleaseBuffer();
		m_Ptr = IntPtr.Zero;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void InitBuffer(CommandBuffer buf);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void ReleaseBuffer();

	public void Release()
	{
		Dispose();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Clear();

	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass, [DefaultValue("null")] MaterialPropertyBlock properties)
	{
		INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
	}

	[ExcludeFromDocs]
	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass)
	{
		MaterialPropertyBlock properties = null;
		INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
	}

	[ExcludeFromDocs]
	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex)
	{
		MaterialPropertyBlock properties = null;
		int shaderPass = -1;
		INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
	}

	[ExcludeFromDocs]
	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material)
	{
		MaterialPropertyBlock properties = null;
		int shaderPass = -1;
		int submeshIndex = 0;
		INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_DrawMesh(CommandBuffer self, Mesh mesh, ref Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void DrawRenderer(Renderer renderer, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass);

	[ExcludeFromDocs]
	public void DrawRenderer(Renderer renderer, Material material, int submeshIndex)
	{
		int shaderPass = -1;
		DrawRenderer(renderer, material, submeshIndex, shaderPass);
	}

	[ExcludeFromDocs]
	public void DrawRenderer(Renderer renderer, Material material)
	{
		int shaderPass = -1;
		int submeshIndex = 0;
		DrawRenderer(renderer, material, submeshIndex, shaderPass);
	}

	public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, [DefaultValue("1")] int instanceCount, [DefaultValue("null")] MaterialPropertyBlock properties)
	{
		INTERNAL_CALL_DrawProcedural(this, ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
	}

	[ExcludeFromDocs]
	public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount)
	{
		MaterialPropertyBlock properties = null;
		INTERNAL_CALL_DrawProcedural(this, ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
	}

	[ExcludeFromDocs]
	public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount)
	{
		MaterialPropertyBlock properties = null;
		int instanceCount = 1;
		INTERNAL_CALL_DrawProcedural(this, ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_DrawProcedural(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties);

	public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset, [DefaultValue("null")] MaterialPropertyBlock properties)
	{
		INTERNAL_CALL_DrawProceduralIndirect(this, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
	}

	[ExcludeFromDocs]
	public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset)
	{
		MaterialPropertyBlock properties = null;
		INTERNAL_CALL_DrawProceduralIndirect(this, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
	}

	[ExcludeFromDocs]
	public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs)
	{
		MaterialPropertyBlock properties = null;
		int argsOffset = 0;
		INTERNAL_CALL_DrawProceduralIndirect(this, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_DrawProceduralIndirect(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

	public void SetRenderTarget(RenderTargetIdentifier rt)
	{
		SetRenderTarget_Single(ref rt, 0, CubemapFace.Unknown);
	}

	public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel)
	{
		SetRenderTarget_Single(ref rt, mipLevel, CubemapFace.Unknown);
	}

	public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace)
	{
		SetRenderTarget_Single(ref rt, mipLevel, cubemapFace);
	}

	public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth)
	{
		SetRenderTarget_ColDepth(ref color, ref depth, 0, CubemapFace.Unknown);
	}

	public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel)
	{
		SetRenderTarget_ColDepth(ref color, ref depth, mipLevel, CubemapFace.Unknown);
	}

	public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace)
	{
		SetRenderTarget_ColDepth(ref color, ref depth, mipLevel, cubemapFace);
	}

	public void SetRenderTarget(RenderTargetIdentifier[] colors, RenderTargetIdentifier depth)
	{
		SetRenderTarget_Multiple(colors, ref depth);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetRenderTarget_Single(ref RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetRenderTarget_ColDepth(ref RenderTargetIdentifier color, ref RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetRenderTarget_Multiple(RenderTargetIdentifier[] color, ref RenderTargetIdentifier depth);

	public void Blit(Texture source, RenderTargetIdentifier dest)
	{
		Blit_Texture(source, ref dest, null, -1);
	}

	public void Blit(Texture source, RenderTargetIdentifier dest, Material mat)
	{
		Blit_Texture(source, ref dest, mat, -1);
	}

	public void Blit(Texture source, RenderTargetIdentifier dest, Material mat, int pass)
	{
		Blit_Texture(source, ref dest, mat, pass);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void Blit_Texture(Texture source, ref RenderTargetIdentifier dest, Material mat, int pass);

	public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest)
	{
		Blit_Identifier(ref source, ref dest, null, -1);
	}

	public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat)
	{
		Blit_Identifier(ref source, ref dest, mat, -1);
	}

	public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass)
	{
		Blit_Identifier(ref source, ref dest, mat, pass);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass);

	[ExcludeFromDocs]
	private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, Material mat)
	{
		int pass = -1;
		Blit_Identifier(ref source, ref dest, mat, pass);
	}

	[ExcludeFromDocs]
	private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest)
	{
		int pass = -1;
		Material mat = null;
		Blit_Identifier(ref source, ref dest, mat, pass);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void GetTemporaryRT(int nameID, int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("FilterMode.Point")] FilterMode filter, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing);

	[ExcludeFromDocs]
	public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite)
	{
		int antiAliasing = 1;
		GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
	}

	[ExcludeFromDocs]
	public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format)
	{
		int antiAliasing = 1;
		RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
		GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
	}

	[ExcludeFromDocs]
	public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter)
	{
		int antiAliasing = 1;
		RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
		RenderTextureFormat format = RenderTextureFormat.Default;
		GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
	}

	[ExcludeFromDocs]
	public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer)
	{
		int antiAliasing = 1;
		RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
		RenderTextureFormat format = RenderTextureFormat.Default;
		FilterMode filter = FilterMode.Point;
		GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
	}

	[ExcludeFromDocs]
	public void GetTemporaryRT(int nameID, int width, int height)
	{
		int antiAliasing = 1;
		RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
		RenderTextureFormat format = RenderTextureFormat.Default;
		FilterMode filter = FilterMode.Point;
		int depthBuffer = 0;
		GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void ReleaseTemporaryRT(int nameID);

	public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
	{
		INTERNAL_CALL_ClearRenderTarget(this, clearDepth, clearColor, ref backgroundColor, depth);
	}

	[ExcludeFromDocs]
	public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor)
	{
		float depth = 1f;
		INTERNAL_CALL_ClearRenderTarget(this, clearDepth, clearColor, ref backgroundColor, depth);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_ClearRenderTarget(CommandBuffer self, bool clearDepth, bool clearColor, ref Color backgroundColor, float depth);

	public void SetGlobalFloat(string name, float value)
	{
		SetGlobalFloat(Shader.PropertyToID(name), value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetGlobalFloat(int nameID, float value);

	public void SetGlobalVector(string name, Vector4 value)
	{
		SetGlobalVector(Shader.PropertyToID(name), value);
	}

	public void SetGlobalVector(int nameID, Vector4 value)
	{
		INTERNAL_CALL_SetGlobalVector(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetGlobalVector(CommandBuffer self, int nameID, ref Vector4 value);

	public void SetGlobalColor(string name, Color value)
	{
		SetGlobalColor(Shader.PropertyToID(name), value);
	}

	public void SetGlobalColor(int nameID, Color value)
	{
		INTERNAL_CALL_SetGlobalColor(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetGlobalColor(CommandBuffer self, int nameID, ref Color value);

	public void SetGlobalMatrix(string name, Matrix4x4 value)
	{
		SetGlobalMatrix(Shader.PropertyToID(name), value);
	}

	public void SetGlobalMatrix(int nameID, Matrix4x4 value)
	{
		INTERNAL_CALL_SetGlobalMatrix(this, nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetGlobalMatrix(CommandBuffer self, int nameID, ref Matrix4x4 value);

	public void SetGlobalTexture(string name, RenderTargetIdentifier value)
	{
		SetGlobalTexture(Shader.PropertyToID(name), value);
	}

	public void SetGlobalTexture(int nameID, RenderTargetIdentifier value)
	{
		SetGlobalTexture_Impl(nameID, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetGlobalTexture_Impl(int nameID, ref RenderTargetIdentifier rt);

	public void IssuePluginEvent(IntPtr callback, int eventID)
	{
		if (callback == IntPtr.Zero)
		{
			throw new ArgumentException("Null callback specified.");
		}
		IssuePluginEventInternal(callback, eventID);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void IssuePluginEventInternal(IntPtr callback, int eventID);
}
