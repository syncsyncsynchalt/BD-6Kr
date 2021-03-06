using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
	internal sealed class GlobalConfigInternal : IDisposable
	{
		internal IntPtr m_Ptr;

		public GlobalConfigInternal(GlobalConfig config)
		{
			InitWrapper();
			InitThreadAwakeTimeout(config.ThreadAwakeTimeout);
			InitReactorModel((byte)config.ReactorModel);
			InitReactorMaximumReceivedMessages(config.ReactorMaximumReceivedMessages);
			InitReactorMaximumSentMessages(config.ReactorMaximumSentMessages);
			InitMaxPacketSize(config.MaxPacketSize);
		}

		public void InitWrapper() { throw new NotImplementedException("なにこれ"); }

		public void InitThreadAwakeTimeout(uint ms) { throw new NotImplementedException("なにこれ"); }

		public void InitReactorModel(byte model) { throw new NotImplementedException("なにこれ"); }

		public void InitReactorMaximumReceivedMessages(ushort size) { throw new NotImplementedException("なにこれ"); }

		public void InitReactorMaximumSentMessages(ushort size) { throw new NotImplementedException("なにこれ"); }

		public void InitMaxPacketSize(ushort size) { throw new NotImplementedException("なにこれ"); }

		public void Dispose() { throw new NotImplementedException("なにこれ"); }

		~GlobalConfigInternal()
		{
			Dispose();
		}
	}
}
