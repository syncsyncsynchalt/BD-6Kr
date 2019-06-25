using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine.Rendering
{
	public sealed class CommandBuffer : IDisposable
	{
		internal IntPtr m_Ptr;

		public string name
		{
			get;
			set;
		}

		public int sizeInBytes
		{
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

		private static void InitBuffer(CommandBuffer buf) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void ReleaseBuffer() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Release()
		{
			Dispose();
		}

		public void Clear() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private static void INTERNAL_CALL_DrawMesh(CommandBuffer self, Mesh mesh, ref Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void DrawRenderer(Renderer renderer, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private static void INTERNAL_CALL_DrawProcedural(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private static void INTERNAL_CALL_DrawProceduralIndirect(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private void SetRenderTarget_Single(ref RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetRenderTarget_ColDepth(ref RenderTargetIdentifier color, ref RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetRenderTarget_Multiple(RenderTargetIdentifier[] color, ref RenderTargetIdentifier depth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private void Blit_Texture(Texture source, ref RenderTargetIdentifier dest, Material mat, int pass) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public void GetTemporaryRT(int nameID, int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("FilterMode.Point")] FilterMode filter, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public void ReleaseTemporaryRT(int nameID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private static void INTERNAL_CALL_ClearRenderTarget(CommandBuffer self, bool clearDepth, bool clearColor, ref Color backgroundColor, float depth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetGlobalFloat(string name, float value)
		{
			SetGlobalFloat(Shader.PropertyToID(name), value);
		}

		public void SetGlobalFloat(int nameID, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetGlobalVector(string name, Vector4 value)
		{
			SetGlobalVector(Shader.PropertyToID(name), value);
		}

		public void SetGlobalVector(int nameID, Vector4 value)
		{
			INTERNAL_CALL_SetGlobalVector(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_SetGlobalVector(CommandBuffer self, int nameID, ref Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetGlobalColor(string name, Color value)
		{
			SetGlobalColor(Shader.PropertyToID(name), value);
		}

		public void SetGlobalColor(int nameID, Color value)
		{
			INTERNAL_CALL_SetGlobalColor(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_SetGlobalColor(CommandBuffer self, int nameID, ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetGlobalMatrix(string name, Matrix4x4 value)
		{
			SetGlobalMatrix(Shader.PropertyToID(name), value);
		}

		public void SetGlobalMatrix(int nameID, Matrix4x4 value)
		{
			INTERNAL_CALL_SetGlobalMatrix(this, nameID, ref value);
		}

		private static void INTERNAL_CALL_SetGlobalMatrix(CommandBuffer self, int nameID, ref Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetGlobalTexture(string name, RenderTargetIdentifier value)
		{
			SetGlobalTexture(Shader.PropertyToID(name), value);
		}

		public void SetGlobalTexture(int nameID, RenderTargetIdentifier value)
		{
			SetGlobalTexture_Impl(nameID, ref value);
		}

		private void SetGlobalTexture_Impl(int nameID, ref RenderTargetIdentifier rt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void IssuePluginEvent(IntPtr callback, int eventID)
		{
			if (callback == IntPtr.Zero)
			{
				throw new ArgumentException("Null callback specified.");
			}
			IssuePluginEventInternal(callback, eventID);
		}

		private void IssuePluginEventInternal(IntPtr callback, int eventID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
