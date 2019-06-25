using System;
using System.Runtime.InteropServices;

namespace Sony.Vita.SavedGame
{
	public struct ResultCode
	{
		private IntPtr _className;

		public ErrorCode lastError;

		public int lastErrorSCE;

		public string className => Marshal.PtrToStringAnsi(_className);
	}
}
