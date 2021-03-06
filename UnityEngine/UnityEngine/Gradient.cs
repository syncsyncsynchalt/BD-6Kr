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

		private void Init() { throw new NotImplementedException("なにこれ"); }

		private void Cleanup() { throw new NotImplementedException("なにこれ"); }

		~Gradient()
		{
			Cleanup();
		}

		public Color Evaluate(float time) { throw new NotImplementedException("なにこれ"); }

		public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys) { throw new NotImplementedException("なにこれ"); }
	}
}
