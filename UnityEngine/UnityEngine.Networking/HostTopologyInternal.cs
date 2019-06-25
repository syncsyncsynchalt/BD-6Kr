using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
	internal sealed class HostTopologyInternal : IDisposable
	{
		internal IntPtr m_Ptr;

		public HostTopologyInternal(HostTopology topology)
		{
			ConnectionConfigInternal config = new ConnectionConfigInternal(topology.DefaultConfig);
			InitWrapper(config, topology.MaxDefaultConnections);
			for (int i = 1; i <= topology.SpecialConnectionConfigsCount; i++)
			{
				ConnectionConfig specialConnectionConfig = topology.GetSpecialConnectionConfig(i);
				ConnectionConfigInternal config2 = new ConnectionConfigInternal(specialConnectionConfig);
				AddSpecialConnectionConfig(config2);
			}
			InitOtherParameters(topology);
		}

		public void InitWrapper(ConnectionConfigInternal config, int maxDefaultConnections) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private int AddSpecialConnectionConfig(ConnectionConfigInternal config)
		{
			return AddSpecialConnectionConfigWrapper(config);
		}

		public int AddSpecialConnectionConfigWrapper(ConnectionConfigInternal config) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void InitOtherParameters(HostTopology topology)
		{
			InitReceivedPoolSize(topology.ReceivedMessagePoolSize);
			InitSentMessagePoolSize(topology.SentMessagePoolSize);
			InitMessagePoolSizeGrowthFactor(topology.MessagePoolSizeGrowthFactor);
		}

		public void InitReceivedPoolSize(ushort pool) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitSentMessagePoolSize(ushort pool) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitMessagePoolSizeGrowthFactor(float factor) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Dispose() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~HostTopologyInternal()
		{
			Dispose();
		}
	}
}
