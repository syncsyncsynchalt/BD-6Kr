using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class Camera : Behaviour
	{
		public delegate void CameraCallback(Camera cam);

		public static CameraCallback onPreCull;

		public static CameraCallback onPreRender;

		public static CameraCallback onPostRender;

		[Obsolete("use Camera.fieldOfView instead.")]
		public float fov
		{
			get
			{
				return fieldOfView;
			}
			set
			{
				fieldOfView = value;
			}
		}

		[Obsolete("use Camera.nearClipPlane instead.")]
		public float near
		{
			get
			{
				return nearClipPlane;
			}
			set
			{
				nearClipPlane = value;
			}
		}

		[Obsolete("use Camera.farClipPlane instead.")]
		public float far
		{
			get
			{
				return farClipPlane;
			}
			set
			{
				farClipPlane = value;
			}
		}

		public float fieldOfView
		{
			get;
			set;
		}

		public float nearClipPlane
		{
			get;
			set;
		}

		public float farClipPlane
		{
			get;
			set;
		}

		public RenderingPath renderingPath
		{
			get;
			set;
		}

		public RenderingPath actualRenderingPath
		{
			get;
		}

		public bool hdr
		{
			get;
			set;
		}

		public float orthographicSize
		{
			get;
			set;
		}

		public bool orthographic
		{
			get;
			set;
		}

		public OpaqueSortMode opaqueSortMode
		{
			get;
			set;
		}

		public TransparencySortMode transparencySortMode
		{
			get;
			set;
		}

		public float depth
		{
			get;
			set;
		}

		public float aspect
		{
			get;
			set;
		}

		public int cullingMask
		{
			get;
			set;
		}

		internal static int PreviewCullingLayer
		{
			get;
		}

		public int eventMask
		{
			get;
			set;
		}

		public Color backgroundColor
		{
			get
			{
				INTERNAL_get_backgroundColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_backgroundColor(ref value);
			}
		}

		public Rect rect
		{
			get
			{
				INTERNAL_get_rect(out Rect value);
				return value;
			}
			set
			{
				INTERNAL_set_rect(ref value);
			}
		}

		public Rect pixelRect
		{
			get
			{
				INTERNAL_get_pixelRect(out Rect value);
				return value;
			}
			set
			{
				INTERNAL_set_pixelRect(ref value);
			}
		}

		public RenderTexture targetTexture
		{
			get;
			set;
		}

		public int pixelWidth
		{
			get;
		}

		public int pixelHeight
		{
			get;
		}

		public Matrix4x4 cameraToWorldMatrix
		{
			get
			{
				INTERNAL_get_cameraToWorldMatrix(out Matrix4x4 value);
				return value;
			}
		}

		public Matrix4x4 worldToCameraMatrix
		{
			get
			{
				INTERNAL_get_worldToCameraMatrix(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_worldToCameraMatrix(ref value);
			}
		}

		public Matrix4x4 projectionMatrix
		{
			get
			{
				INTERNAL_get_projectionMatrix(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_projectionMatrix(ref value);
			}
		}

		public Vector3 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector3 value);
				return value;
			}
		}

		public CameraClearFlags clearFlags
		{
			get;
			set;
		}

		public bool stereoEnabled
		{
			get;
		}

		public float stereoSeparation
		{
			get;
			set;
		}

		public float stereoConvergence
		{
			get;
			set;
		}

		public CameraType cameraType
		{
			get;
			set;
		}

		public bool stereoMirrorMode
		{
			get;
			set;
		}

		public int targetDisplay
		{
			get;
			set;
		}

		public static Camera main
		{
			get;
		}

		public static Camera current
		{
			get;
		}

		public static Camera[] allCameras
		{
			get;
		}

		public static int allCamerasCount
		{
			get;
		}

		public bool useOcclusionCulling
		{
			get;
			set;
		}

		public float[] layerCullDistances
		{
			get;
			set;
		}

		public bool layerCullSpherical
		{
			get;
			set;
		}

		public DepthTextureMode depthTextureMode
		{
			get;
			set;
		}

		public bool clearStencilAfterLightingPass
		{
			get;
			set;
		}

		public int commandBufferCount
		{
			get;
		}

		internal string[] GetHDRWarnings() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_backgroundColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_backgroundColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rect(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_rect(ref Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_pixelRect(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_pixelRect(ref Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetTargetBuffersImpl(out RenderBuffer color, out RenderBuffer depth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetTargetBuffersMRTImpl(RenderBuffer[] color, out RenderBuffer depth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTargetBuffers(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
		{
			SetTargetBuffersImpl(out colorBuffer, out depthBuffer);
		}

		public void SetTargetBuffers(RenderBuffer[] colorBuffer, RenderBuffer depthBuffer)
		{
			SetTargetBuffersMRTImpl(colorBuffer, out depthBuffer);
		}

		private void INTERNAL_get_cameraToWorldMatrix(out Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_worldToCameraMatrix(out Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_worldToCameraMatrix(ref Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ResetWorldToCameraMatrix()
		{
			INTERNAL_CALL_ResetWorldToCameraMatrix(this);
		}

		private static void INTERNAL_CALL_ResetWorldToCameraMatrix(Camera self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_projectionMatrix(out Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_projectionMatrix(ref Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ResetProjectionMatrix()
		{
			INTERNAL_CALL_ResetProjectionMatrix(this);
		}

		private static void INTERNAL_CALL_ResetProjectionMatrix(Camera self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ResetAspect()
		{
			INTERNAL_CALL_ResetAspect(this);
		}

		private static void INTERNAL_CALL_ResetAspect(Camera self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_velocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 WorldToScreenPoint(Vector3 position)
		{
			return INTERNAL_CALL_WorldToScreenPoint(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_WorldToScreenPoint(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 WorldToViewportPoint(Vector3 position)
		{
			return INTERNAL_CALL_WorldToViewportPoint(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_WorldToViewportPoint(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 ViewportToWorldPoint(Vector3 position)
		{
			return INTERNAL_CALL_ViewportToWorldPoint(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_ViewportToWorldPoint(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 ScreenToWorldPoint(Vector3 position)
		{
			return INTERNAL_CALL_ScreenToWorldPoint(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_ScreenToWorldPoint(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 ScreenToViewportPoint(Vector3 position)
		{
			return INTERNAL_CALL_ScreenToViewportPoint(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_ScreenToViewportPoint(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 ViewportToScreenPoint(Vector3 position)
		{
			return INTERNAL_CALL_ViewportToScreenPoint(this, ref position);
		}

		private static Vector3 INTERNAL_CALL_ViewportToScreenPoint(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Ray ViewportPointToRay(Vector3 position)
		{
			return INTERNAL_CALL_ViewportPointToRay(this, ref position);
		}

		private static Ray INTERNAL_CALL_ViewportPointToRay(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Ray ScreenPointToRay(Vector3 position)
		{
			return INTERNAL_CALL_ScreenPointToRay(this, ref position);
		}

		private static Ray INTERNAL_CALL_ScreenPointToRay(Camera self, ref Vector3 position) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetAllCameras(Camera[] cameras) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void FireOnPreCull(Camera cam)
		{
			if (onPreCull != null)
			{
				onPreCull(cam);
			}
		}

		private static void FireOnPreRender(Camera cam)
		{
			if (onPreRender != null)
			{
				onPreRender(cam);
			}
		}

		private static void FireOnPostRender(Camera cam)
		{
			if (onPostRender != null)
			{
				onPostRender(cam);
			}
		}

		public void Render() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RenderWithShader(Shader shader, string replacementTag) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetReplacementShader(Shader shader, string replacementTag) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ResetReplacementShader()
		{
			INTERNAL_CALL_ResetReplacementShader(this);
		}

		private static void INTERNAL_CALL_ResetReplacementShader(Camera self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RenderDontRestore() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetupCurrent(Camera cur) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public bool RenderToCubemap(Cubemap cubemap)
		{
			int faceMask = 63;
			return RenderToCubemap(cubemap, faceMask);
		}

		public bool RenderToCubemap(Cubemap cubemap, [DefaultValue("63")] int faceMask)
		{
			return Internal_RenderToCubemapTexture(cubemap, faceMask);
		}

		[ExcludeFromDocs]
		public bool RenderToCubemap(RenderTexture cubemap)
		{
			int faceMask = 63;
			return RenderToCubemap(cubemap, faceMask);
		}

		public bool RenderToCubemap(RenderTexture cubemap, [DefaultValue("63")] int faceMask)
		{
			return Internal_RenderToCubemapRT(cubemap, faceMask);
		}

		private bool Internal_RenderToCubemapRT(RenderTexture cubemap, int faceMask) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool Internal_RenderToCubemapTexture(Cubemap cubemap, int faceMask) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void CopyFrom(Camera other) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal bool IsFiltered(GameObject go) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddCommandBuffer(CameraEvent evt, CommandBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveCommandBuffer(CameraEvent evt, CommandBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveCommandBuffers(CameraEvent evt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveAllCommandBuffers() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public CommandBuffer[] GetCommandBuffers(CameraEvent evt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal GameObject RaycastTry(Ray ray, float distance, int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return INTERNAL_CALL_RaycastTry(this, ref ray, distance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		internal GameObject RaycastTry(Ray ray, float distance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return INTERNAL_CALL_RaycastTry(this, ref ray, distance, layerMask, queryTriggerInteraction);
		}

		private static GameObject INTERNAL_CALL_RaycastTry(Camera self, ref Ray ray, float distance, int layerMask, QueryTriggerInteraction queryTriggerInteraction) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal GameObject RaycastTry2D(Ray ray, float distance, int layerMask)
		{
			return INTERNAL_CALL_RaycastTry2D(this, ref ray, distance, layerMask);
		}

		private static GameObject INTERNAL_CALL_RaycastTry2D(Camera self, ref Ray ray, float distance, int layerMask) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Matrix4x4 CalculateObliqueMatrix(Vector4 clipPlane)
		{
			return INTERNAL_CALL_CalculateObliqueMatrix(this, ref clipPlane);
		}

		private static Matrix4x4 INTERNAL_CALL_CalculateObliqueMatrix(Camera self, ref Vector4 clipPlane) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void OnlyUsedForTesting1()
		{
		}

		internal void OnlyUsedForTesting2()
		{
		}
	}
}
