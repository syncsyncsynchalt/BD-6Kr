using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class GUITexture : GUIElement
	{
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

		public Texture texture
		{
			get;
			set;
		}

		public Rect pixelInset
		{
			get
			{
				INTERNAL_get_pixelInset(out Rect value);
				return value;
			}
			set
			{
				INTERNAL_set_pixelInset(ref value);
			}
		}

		public RectOffset border
		{
			get;
			set;
		}

		private void INTERNAL_get_color(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_color(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_pixelInset(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_pixelInset(ref Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
