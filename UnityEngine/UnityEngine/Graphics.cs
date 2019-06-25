using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class Graphics
	{
		public static RenderBuffer activeColorBuffer
		{
			get
			{
				GetActiveColorBuffer(out RenderBuffer res);
				return res;
			}
		}

		public static RenderBuffer activeDepthBuffer
		{
			get
			{
				GetActiveDepthBuffer(out RenderBuffer res);
				return res;
			}
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
		{
			bool receiveShadows = true;
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			MaterialPropertyBlock properties = null;
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			MaterialPropertyBlock properties = null;
			int submeshIndex = 0;
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			MaterialPropertyBlock properties = null;
			int submeshIndex = 0;
			Camera camera = null;
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, [DefaultValue("null")] Camera camera, [DefaultValue("0")] int submeshIndex, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("true")] bool castShadows, [DefaultValue("true")] bool receiveShadows)
		{
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
		{
			Transform probeAnchor = null;
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
		{
			Transform probeAnchor = null;
			bool receiveShadows = true;
			DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
		}

		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("null")] Transform probeAnchor)
		{
			Internal_DrawMeshTRArguments arguments = default(Internal_DrawMeshTRArguments);
			arguments.position = position;
			arguments.rotation = rotation;
			arguments.layer = layer;
			arguments.submeshIndex = submeshIndex;
			arguments.castShadows = (int)castShadows;
			arguments.receiveShadows = (receiveShadows ? 1 : 0);
			arguments.reflectionProbeAnchorInstanceID = ((probeAnchor != null) ? probeAnchor.GetInstanceID() : 0);
			Internal_DrawMeshTR(ref arguments, properties, material, mesh, camera);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
		{
			bool receiveShadows = true;
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			MaterialPropertyBlock properties = null;
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			MaterialPropertyBlock properties = null;
			int submeshIndex = 0;
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer)
		{
			bool receiveShadows = true;
			bool castShadows = true;
			MaterialPropertyBlock properties = null;
			int submeshIndex = 0;
			Camera camera = null;
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
		}

		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, [DefaultValue("null")] Camera camera, [DefaultValue("0")] int submeshIndex, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("true")] bool castShadows, [DefaultValue("true")] bool receiveShadows)
		{
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
		{
			Transform probeAnchor = null;
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
		{
			Transform probeAnchor = null;
			bool receiveShadows = true;
			DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
		}

		public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("null")] Transform probeAnchor)
		{
			Internal_DrawMeshMatrixArguments arguments = default(Internal_DrawMeshMatrixArguments);
			arguments.matrix = matrix;
			arguments.layer = layer;
			arguments.submeshIndex = submeshIndex;
			arguments.castShadows = (int)castShadows;
			arguments.receiveShadows = (receiveShadows ? 1 : 0);
			arguments.reflectionProbeAnchorInstanceID = ((probeAnchor != null) ? probeAnchor.GetInstanceID() : 0);
			Internal_DrawMeshMatrix(ref arguments, properties, material, mesh, camera);
		}

		private static void Internal_DrawMeshTR(ref Internal_DrawMeshTRArguments arguments, MaterialPropertyBlock properties, Material material, Mesh mesh, Camera camera) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_DrawMeshMatrix(ref Internal_DrawMeshMatrixArguments arguments, MaterialPropertyBlock properties, Material material, Mesh mesh, Camera camera) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation)
		{
			Internal_DrawMeshNow1(mesh, position, rotation, -1);
		}

		public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex)
		{
			Internal_DrawMeshNow1(mesh, position, rotation, materialIndex);
		}

		public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix)
		{
			Internal_DrawMeshNow2(mesh, matrix, -1);
		}

		public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix, int materialIndex)
		{
			Internal_DrawMeshNow2(mesh, matrix, materialIndex);
		}

		private static void Internal_DrawMeshNow1(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex)
		{
			INTERNAL_CALL_Internal_DrawMeshNow1(mesh, ref position, ref rotation, materialIndex);
		}

		private static void INTERNAL_CALL_Internal_DrawMeshNow1(Mesh mesh, ref Vector3 position, ref Quaternion rotation, int materialIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_DrawMeshNow2(Mesh mesh, Matrix4x4 matrix, int materialIndex)
		{
			INTERNAL_CALL_Internal_DrawMeshNow2(mesh, ref matrix, materialIndex);
		}

		private static void INTERNAL_CALL_Internal_DrawMeshNow2(Mesh mesh, ref Matrix4x4 matrix, int materialIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void DrawProcedural(MeshTopology topology, int vertexCount, [DefaultValue("1")] int instanceCount) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void DrawProcedural(MeshTopology topology, int vertexCount)
		{
			int instanceCount = 1;
			DrawProcedural(topology, vertexCount, instanceCount);
		}

		public static void DrawProceduralIndirect(MeshTopology topology, ComputeBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void DrawProceduralIndirect(MeshTopology topology, ComputeBuffer bufferWithArgs)
		{
			int argsOffset = 0;
			DrawProceduralIndirect(topology, bufferWithArgs, argsOffset);
		}

		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture)
		{
			Material mat = null;
			DrawTexture(screenRect, texture, mat);
		}

		public static void DrawTexture(Rect screenRect, Texture texture, [DefaultValue("null")] Material mat)
		{
			DrawTexture(screenRect, texture, 0, 0, 0, 0, mat);
		}

		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
		{
			Material mat = null;
			DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
		}

		public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat)
		{
			DrawTexture(screenRect, texture, new Rect(0f, 0f, 1f, 1f), leftBorder, rightBorder, topBorder, bottomBorder, mat);
		}

		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
		{
			Material mat = null;
			DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, mat);
		}

		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat)
		{
			InternalDrawTextureArguments arguments = default(InternalDrawTextureArguments);
			arguments.screenRect = screenRect;
			arguments.texture = texture;
			arguments.sourceRect = sourceRect;
			arguments.leftBorder = leftBorder;
			arguments.rightBorder = rightBorder;
			arguments.topBorder = topBorder;
			arguments.bottomBorder = bottomBorder;
			Color32 color = default(Color32);
			color.r = (color.g = (color.b = (color.a = 128)));
			arguments.color = color;
			arguments.mat = mat;
			DrawTexture(ref arguments);
		}

		[ExcludeFromDocs]
		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color)
		{
			Material mat = null;
			DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat);
		}

		public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, [DefaultValue("null")] Material mat)
		{
			InternalDrawTextureArguments arguments = default(InternalDrawTextureArguments);
			arguments.screenRect = screenRect;
			arguments.texture = texture;
			arguments.sourceRect = sourceRect;
			arguments.leftBorder = leftBorder;
			arguments.rightBorder = rightBorder;
			arguments.topBorder = topBorder;
			arguments.bottomBorder = bottomBorder;
			arguments.color = color;
			arguments.mat = mat;
			DrawTexture(ref arguments);
		}

		internal static void DrawTexture(ref InternalDrawTextureArguments arguments) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void ExecuteCommandBuffer(CommandBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Blit(Texture source, RenderTexture dest) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void Blit(Texture source, RenderTexture dest, Material mat)
		{
			int pass = -1;
			Blit(source, dest, mat, pass);
		}

		public static void Blit(Texture source, RenderTexture dest, Material mat, [DefaultValue("-1")] int pass)
		{
			Internal_BlitMaterial(source, dest, mat, pass, setRT: true);
		}

		[ExcludeFromDocs]
		public static void Blit(Texture source, Material mat)
		{
			int pass = -1;
			Blit(source, mat, pass);
		}

		public static void Blit(Texture source, Material mat, [DefaultValue("-1")] int pass)
		{
			Internal_BlitMaterial(source, null, mat, pass, setRT: false);
		}

		private static void Internal_BlitMaterial(Texture source, RenderTexture dest, Material mat, int pass, bool setRT) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void BlitMultiTap(Texture source, RenderTexture dest, Material mat, params Vector2[] offsets)
		{
			Internal_BlitMultiTap(source, dest, mat, offsets);
		}

		private static void Internal_BlitMultiTap(Texture source, RenderTexture dest, Material mat, Vector2[] offsets) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetNullRT() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetRTFullSetup(out RenderBuffer color, out RenderBuffer depth, int mip, int face, RenderBufferLoadAction colorLoad, RenderBufferStoreAction colorStore, RenderBufferLoadAction depthLoad, RenderBufferStoreAction depthStore) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetRTSimple(out RenderBuffer color, out RenderBuffer depth, int mip, int face) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetMRTFullSetup(RenderBuffer[] color, out RenderBuffer depth, int mip, int face, RenderBufferLoadAction[] colorLoad, RenderBufferStoreAction[] colorStore, RenderBufferLoadAction depthLoad, RenderBufferStoreAction depthStore) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetMRTSimple(RenderBuffer[] color, out RenderBuffer depth, int mip, int face) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void GetActiveColorBuffer(out RenderBuffer res) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void GetActiveDepthBuffer(out RenderBuffer res) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetRandomWriteTarget(int index, RenderTexture uav)
		{
			Internal_SetRandomWriteTargetRT(index, uav);
		}

		public static void SetRandomWriteTarget(int index, ComputeBuffer uav)
		{
			Internal_SetRandomWriteTargetBuffer(index, uav);
		}

		public static void ClearRandomWriteTargets() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetRandomWriteTargetRT(int index, RenderTexture uav) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetRandomWriteTargetBuffer(int index, ComputeBuffer uav) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void SetupVertexLights(Light[] lights) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void CheckLoadActionValid(RenderBufferLoadAction load, string bufferType)
		{
			if (load != 0 && load != RenderBufferLoadAction.DontCare)
			{
				throw new ArgumentException("Bad " + bufferType + " LoadAction");
			}
		}

		internal static void CheckStoreActionValid(RenderBufferStoreAction store, string bufferType)
		{
			if (store != 0 && store != RenderBufferStoreAction.DontCare)
			{
				throw new ArgumentException("Bad " + bufferType + " StoreAction");
			}
		}

		internal static void SetRenderTargetImpl(RenderTargetSetup setup)
		{
			if (setup.color.Length == 0)
			{
				throw new ArgumentException("Invalid color buffer count for SetRenderTarget");
			}
			if (setup.color.Length != setup.colorLoad.Length)
			{
				throw new ArgumentException("Color LoadAction and Buffer arrays have different sizes");
			}
			if (setup.color.Length != setup.colorStore.Length)
			{
				throw new ArgumentException("Color StoreAction and Buffer arrays have different sizes");
			}
			RenderBufferLoadAction[] colorLoad = setup.colorLoad;
			foreach (RenderBufferLoadAction load in colorLoad)
			{
				CheckLoadActionValid(load, "Color");
			}
			RenderBufferStoreAction[] colorStore = setup.colorStore;
			foreach (RenderBufferStoreAction store in colorStore)
			{
				CheckStoreActionValid(store, "Color");
			}
			CheckLoadActionValid(setup.depthLoad, "Depth");
			CheckStoreActionValid(setup.depthStore, "Depth");
			if (setup.cubemapFace != -1 && (setup.cubemapFace < 0 || setup.cubemapFace > 5))
			{
				throw new ArgumentException("Bad CubemapFace");
			}
			Internal_SetMRTFullSetup(setup.color, out setup.depth, setup.mipLevel, setup.cubemapFace, setup.colorLoad, setup.colorStore, setup.depthLoad, setup.depthStore);
		}

		internal static void SetRenderTargetImpl(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, int face)
		{
			RenderBuffer color = colorBuffer;
			RenderBuffer depth = depthBuffer;
			Internal_SetRTSimple(out color, out depth, mipLevel, face);
		}

		internal static void SetRenderTargetImpl(RenderTexture rt, int mipLevel, int face)
		{
			if ((bool)rt)
			{
				SetRenderTargetImpl(rt.colorBuffer, rt.depthBuffer, mipLevel, face);
			}
			else
			{
				Internal_SetNullRT();
			}
		}

		internal static void SetRenderTargetImpl(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer, int mipLevel, int face)
		{
			RenderBuffer depth = depthBuffer;
			Internal_SetMRTSimple(colorBuffers, out depth, mipLevel, face);
		}

		public static void SetRenderTarget(RenderTexture rt)
		{
			SetRenderTargetImpl(rt, 0, -1);
		}

		public static void SetRenderTarget(RenderTexture rt, int mipLevel)
		{
			SetRenderTargetImpl(rt, mipLevel, -1);
		}

		public static void SetRenderTarget(RenderTexture rt, int mipLevel, CubemapFace face)
		{
			SetRenderTargetImpl(rt, mipLevel, (int)face);
		}

		public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
		{
			SetRenderTargetImpl(colorBuffer, depthBuffer, 0, -1);
		}

		public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel)
		{
			SetRenderTargetImpl(colorBuffer, depthBuffer, mipLevel, -1);
		}

		public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, CubemapFace face)
		{
			SetRenderTargetImpl(colorBuffer, depthBuffer, mipLevel, (int)face);
		}

		public static void SetRenderTarget(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer)
		{
			SetRenderTargetImpl(colorBuffers, depthBuffer, 0, -1);
		}

		public static void SetRenderTarget(RenderTargetSetup setup)
		{
			SetRenderTargetImpl(setup);
		}
	}
}
