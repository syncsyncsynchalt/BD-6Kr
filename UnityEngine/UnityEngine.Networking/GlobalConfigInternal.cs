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

		public void InitWrapper() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitThreadAwakeTimeout(uint ms) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitReactorModel(byte model) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitReactorMaximumReceivedMessages(ushort size) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitReactorMaximumSentMessages(ushort size) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitMaxPacketSize(ushort size) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Dispose() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~GlobalConfigInternal()
		{
			Dispose();
		}
	}
}
