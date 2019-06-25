using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	internal sealed class GUIClip
	{
		public static bool enabled
		{
			get;
		}

		public static Rect topmostRect
		{
			get
			{
				INTERNAL_get_topmostRect(out Rect value);
				return value;
			}
		}

		public static Rect visibleRect
		{
			get
			{
				INTERNAL_get_visibleRect(out Rect value);
				return value;
			}
		}

		public static Vector2 Unclip(Vector2 pos)
		{
			Unclip_Vector2(ref pos);
			return pos;
		}

		public static Rect Unclip(Rect rect)
		{
			Unclip_Rect(ref rect);
			return rect;
		}

		public static Vector2 Clip(Vector2 absolutePos)
		{
			Clip_Vector2(ref absolutePos);
			return absolutePos;
		}

		public static Rect Clip(Rect absoluteRect)
		{
			Internal_Clip_Rect(ref absoluteRect);
			return absoluteRect;
		}

		public static Vector2 GetAbsoluteMousePosition()
		{
			Internal_GetAbsoluteMousePosition(out Vector2 output);
			return output;
		}

		internal static void Push(Rect screenRect, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset)
		{
			INTERNAL_CALL_Push(ref screenRect, ref scrollOffset, ref renderOffset, resetOffset);
		}

		private static void INTERNAL_CALL_Push(ref Rect screenRect, ref Vector2 scrollOffset, ref Vector2 renderOffset, bool resetOffset) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void Pop() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static Rect GetTopRect() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Unclip_Vector2(ref Vector2 pos)
		{
			INTERNAL_CALL_Unclip_Vector2(ref pos);
		}

		private static void INTERNAL_CALL_Unclip_Vector2(ref Vector2 pos) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_topmostRect(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Unclip_Rect(ref Rect rect)
		{
			INTERNAL_CALL_Unclip_Rect(ref rect);
		}

		private static void INTERNAL_CALL_Unclip_Rect(ref Rect rect) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Clip_Vector2(ref Vector2 absolutePos)
		{
			INTERNAL_CALL_Clip_Vector2(ref absolutePos);
		}

		private static void INTERNAL_CALL_Clip_Vector2(ref Vector2 absolutePos) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_Clip_Rect(ref Rect absoluteRect)
		{
			INTERNAL_CALL_Internal_Clip_Rect(ref absoluteRect);
		}

		private static void INTERNAL_CALL_Internal_Clip_Rect(ref Rect absoluteRect) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void Reapply() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static Matrix4x4 GetMatrix() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void SetMatrix(Matrix4x4 m)
		{
			INTERNAL_CALL_SetMatrix(ref m);
		}

		private static void INTERNAL_CALL_SetMatrix(ref Matrix4x4 m) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_visibleRect(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_GetAbsoluteMousePosition(out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
