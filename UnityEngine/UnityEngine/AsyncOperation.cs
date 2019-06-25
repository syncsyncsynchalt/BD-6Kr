using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public class AsyncOperation : YieldInstruction
	{
		internal IntPtr m_Ptr;

		public bool isDone
		{
			get;
		}

		public float progress
		{
			get;
		}

		public int priority
		{
			get;
			set;
		}

		public bool allowSceneActivation
		{
			get;
			set;
		}

		private void InternalDestroy() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~AsyncOperation()
		{
			InternalDestroy();
		}
	}
}
