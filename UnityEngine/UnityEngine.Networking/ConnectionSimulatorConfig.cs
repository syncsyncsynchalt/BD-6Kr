using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
	public sealed class ConnectionSimulatorConfig : IDisposable
	{
		internal IntPtr m_Ptr;

		public ConnectionSimulatorConfig(int outMinDelay, int outAvgDelay, int inMinDelay, int inAvgDelay, float packetLossPercentage) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Dispose() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~ConnectionSimulatorConfig()
		{
			Dispose();
		}
	}
}
