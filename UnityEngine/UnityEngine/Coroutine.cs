using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Coroutine : YieldInstruction
	{
		internal IntPtr m_Ptr;

		private Coroutine()
		{
		}

		private void ReleaseCoroutine() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~Coroutine()
		{
			ReleaseCoroutine();
		}
	}
}
