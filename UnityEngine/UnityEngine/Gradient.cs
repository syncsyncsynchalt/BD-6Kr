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

		private void Init() { throw new NotImplementedException("�Ȃɂ���"); }

		private void Cleanup() { throw new NotImplementedException("�Ȃɂ���"); }

		~Gradient()
		{
			Cleanup();
		}

		public Color Evaluate(float time) { throw new NotImplementedException("�Ȃɂ���"); }

		public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys) { throw new NotImplementedException("�Ȃɂ���"); }
	}
}
