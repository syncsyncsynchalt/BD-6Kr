using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class GUIText : GUIElement
	{
		public string text
		{
			get;
			set;
		}

		public Material material
		{
			get;
			set;
		}

		public Vector2 pixelOffset
		{
			get
			{
				Internal_GetPixelOffset(out Vector2 output);
				return output;
			}
			set
			{
				Internal_SetPixelOffset(value);
			}
		}

		public Font font
		{
			get;
			set;
		}

		public TextAlignment alignment
		{
			get;
			set;
		}

		public TextAnchor anchor
		{
			get;
			set;
		}

		public float lineSpacing
		{
			get;
			set;
		}

		public float tabSize
		{
			get;
			set;
		}

		public int fontSize
		{
			get;
			set;
		}

		public FontStyle fontStyle
		{
			get;
			set;
		}

		public bool richText
		{
			get;
			set;
		}

		public Color color
		{
			get
			{
				INTERNAL_get_color(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_color(ref value);
			}
		}

		private void Internal_GetPixelOffset(out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_SetPixelOffset(Vector2 p)
		{
			INTERNAL_CALL_Internal_SetPixelOffset(this, ref p);
		}

		private static void INTERNAL_CALL_Internal_SetPixelOffset(GUIText self, ref Vector2 p) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_color(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_color(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
