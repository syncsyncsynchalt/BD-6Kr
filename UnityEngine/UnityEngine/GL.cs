using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class GL
	{
		public const int TRIANGLES = 4;

		public const int TRIANGLE_STRIP = 5;

		public const int QUADS = 7;

		public const int LINES = 1;

		public static Matrix4x4 modelview
		{
			get
			{
				INTERNAL_get_modelview(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_modelview(ref value);
			}
		}

		public static bool wireframe
		{
			get;
			set;
		}

		public static bool sRGBWrite
		{
			get;
			set;
		}

		public static bool invertCulling
		{
			get;
			set;
		}

		public static void Vertex3(float x, float y, float z) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Vertex(Vector3 v)
		{
			INTERNAL_CALL_Vertex(ref v);
		}

		private static void INTERNAL_CALL_Vertex(ref Vector3 v) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Color(Color c)
		{
			INTERNAL_CALL_Color(ref c);
		}

		private static void INTERNAL_CALL_Color(ref Color c) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void TexCoord(Vector3 v)
		{
			INTERNAL_CALL_TexCoord(ref v);
		}

		private static void INTERNAL_CALL_TexCoord(ref Vector3 v) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void TexCoord2(float x, float y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void TexCoord3(float x, float y, float z) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void MultiTexCoord2(int unit, float x, float y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void MultiTexCoord3(int unit, float x, float y, float z) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void MultiTexCoord(int unit, Vector3 v)
		{
			INTERNAL_CALL_MultiTexCoord(unit, ref v);
		}

		private static void INTERNAL_CALL_MultiTexCoord(int unit, ref Vector3 v) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Begin(int mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void End() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void LoadOrtho() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void LoadPixelMatrix() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void LoadPixelMatrixArgs(float left, float right, float bottom, float top) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void LoadPixelMatrix(float left, float right, float bottom, float top)
		{
			LoadPixelMatrixArgs(left, right, bottom, top);
		}

		public static void Viewport(Rect pixelRect)
		{
			INTERNAL_CALL_Viewport(ref pixelRect);
		}

		private static void INTERNAL_CALL_Viewport(ref Rect pixelRect) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void LoadProjectionMatrix(Matrix4x4 mat)
		{
			INTERNAL_CALL_LoadProjectionMatrix(ref mat);
		}

		private static void INTERNAL_CALL_LoadProjectionMatrix(ref Matrix4x4 mat) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void LoadIdentity() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_modelview(out Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_modelview(ref Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void MultMatrix(Matrix4x4 mat)
		{
			INTERNAL_CALL_MultMatrix(ref mat);
		}

		private static void INTERNAL_CALL_MultMatrix(ref Matrix4x4 mat) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void PushMatrix() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void PopMatrix() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture)
		{
			return INTERNAL_CALL_GetGPUProjectionMatrix(ref proj, renderIntoTexture);
		}

		private static Matrix4x4 INTERNAL_CALL_GetGPUProjectionMatrix(ref Matrix4x4 proj, bool renderIntoTexture) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use invertCulling property")]
		public static void SetRevertBackfacing(bool revertBackFaces) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			float depth = 1f;
			Clear(clearDepth, clearColor, backgroundColor, depth);
		}

		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
		{
			Internal_Clear(clearDepth, clearColor, backgroundColor, depth);
		}

		private static void Internal_Clear(bool clearDepth, bool clearColor, Color backgroundColor, float depth)
		{
			INTERNAL_CALL_Internal_Clear(clearDepth, clearColor, ref backgroundColor, depth);
		}

		private static void INTERNAL_CALL_Internal_Clear(bool clearDepth, bool clearColor, ref Color backgroundColor, float depth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void ClearWithSkybox(bool clearDepth, Camera camera) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void InvalidateState() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("IssuePluginEvent(eventID) is deprecated. Use IssuePluginEvent(callback, eventID) instead.")]
		public static void IssuePluginEvent(int eventID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void IssuePluginEvent(IntPtr callback, int eventID)
		{
			if (callback == IntPtr.Zero)
			{
				throw new ArgumentException("Null callback specified.");
			}
			IssuePluginEventInternal(callback, eventID);
		}

		private static void IssuePluginEventInternal(IntPtr callback, int eventID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void RenderTargetBarrier() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
