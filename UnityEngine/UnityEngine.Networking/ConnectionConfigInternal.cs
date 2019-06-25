using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
	internal sealed class ConnectionConfigInternal : IDisposable
	{
		internal IntPtr m_Ptr;

		public int ChannelSize
		{
			get;
		}

		private ConnectionConfigInternal()
		{
		}

		public ConnectionConfigInternal(ConnectionConfig config)
		{
			if (config == null)
			{
				throw new NullReferenceException("config is not defined");
			}
			InitWrapper();
			InitPacketSize(config.PacketSize);
			InitFragmentSize(config.FragmentSize);
			InitResendTimeout(config.ResendTimeout);
			InitDisconnectTimeout(config.DisconnectTimeout);
			InitConnectTimeout(config.ConnectTimeout);
			InitMinUpdateTimeout(config.MinUpdateTimeout);
			InitPingTimeout(config.PingTimeout);
			InitReducedPingTimeout(config.ReducedPingTimeout);
			InitAllCostTimeout(config.AllCostTimeout);
			InitNetworkDropThreshold(config.NetworkDropThreshold);
			InitOverflowDropThreshold(config.OverflowDropThreshold);
			InitMaxConnectionAttempt(config.MaxConnectionAttempt);
			InitAckDelay(config.AckDelay);
			InitMaxCombinedReliableMessageSize(config.MaxCombinedReliableMessageSize);
			InitMaxCombinedReliableMessageCount(config.MaxCombinedReliableMessageCount);
			InitMaxSentMessageQueueSize(config.MaxSentMessageQueueSize);
			InitIsAcksLong(config.IsAcksLong);
			for (byte b = 0; b < config.ChannelCount; b = (byte)(b + 1))
			{
				AddChannel(config.GetChannel(b));
			}
		}

		public void InitWrapper() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public byte AddChannel(QosType value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public QosType GetChannel(int i) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitPacketSize(ushort value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitFragmentSize(ushort value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitResendTimeout(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitDisconnectTimeout(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitConnectTimeout(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitMinUpdateTimeout(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitPingTimeout(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitReducedPingTimeout(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitAllCostTimeout(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitNetworkDropThreshold(byte value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitOverflowDropThreshold(byte value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitMaxConnectionAttempt(byte value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitAckDelay(uint value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitMaxCombinedReliableMessageSize(ushort value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitMaxCombinedReliableMessageCount(ushort value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitMaxSentMessageQueueSize(ushort value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitIsAcksLong(bool value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Dispose() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~ConnectionConfigInternal()
		{
			Dispose();
		}
	}
}
