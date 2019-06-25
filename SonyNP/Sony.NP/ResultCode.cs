using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public struct ResultCode
	{
		private IntPtr _serviceName;

		public ErrorCode lastError;

		public int lastErrorSCE;

		public string className => Marshal.PtrToStringAnsi(_serviceName);
	}
}
