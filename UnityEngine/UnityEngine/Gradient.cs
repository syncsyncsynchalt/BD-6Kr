using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Gradient
	{
		internal IntPtr m_Ptr;

		public GradientColorKey[] colorKeys
		{
			get;
			set;
		}

		public GradientAlphaKey[] alphaKeys
		{
			get;
			set;
		}

		public Gradient()
		{
			Init();
		}

		private void Init() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Cleanup() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~Gradient()
		{
			Cleanup();
		}

		public Color Evaluate(float time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
