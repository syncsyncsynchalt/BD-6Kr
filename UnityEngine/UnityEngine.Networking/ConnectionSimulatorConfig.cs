using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
	public sealed class ConnectionSimulatorConfig : IDisposable
	{
		internal IntPtr m_Ptr;

		public ConnectionSimulatorConfig(int outMinDelay, int outAvgDelay, int inMinDelay, int inAvgDelay, float packetLossPercentage) { throw new NotImplementedException("なにこれ"); }

		public void Dispose() { throw new NotImplementedException("なにこれ"); }

		~ConnectionSimulatorConfig()
		{
			Dispose();
		}
	}
}
