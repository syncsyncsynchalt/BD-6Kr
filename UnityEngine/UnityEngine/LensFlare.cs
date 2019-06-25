using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class LensFlare : Behaviour
	{
		public Flare flare
		{
			get;
			set;
		}

		public float brightness
		{
			get;
			set;
		}

		public float fadeSpeed
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

		private void INTERNAL_get_color(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_color(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
