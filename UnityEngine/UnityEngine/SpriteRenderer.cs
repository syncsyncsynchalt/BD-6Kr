using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class SpriteRenderer : Renderer
	{
		public Sprite sprite
		{
			get
			{
				return GetSprite_INTERNAL();
			}
			set
			{
				SetSprite_INTERNAL(value);
			}
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

		private Sprite GetSprite_INTERNAL() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetSprite_INTERNAL(Sprite sprite) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_color(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_color(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
