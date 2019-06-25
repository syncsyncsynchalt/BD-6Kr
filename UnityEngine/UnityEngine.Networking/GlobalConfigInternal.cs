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

		public void InitWrapper() { throw new NotImplementedException("�Ȃɂ���"); }

		public void InitThreadAwakeTimeout(uint ms) { throw new NotImplementedException("�Ȃɂ���"); }

		public void InitReactorModel(byte model) { throw new NotImplementedException("�Ȃɂ���"); }

		public void InitReactorMaximumReceivedMessages(ushort size) { throw new NotImplementedException("�Ȃɂ���"); }

		public void InitReactorMaximumSentMessages(ushort size) { throw new NotImplementedException("�Ȃɂ���"); }

		public void InitMaxPacketSize(ushort size) { throw new NotImplementedException("�Ȃɂ���"); }

		public void Dispose() { throw new NotImplementedException("�Ȃɂ���"); }

		~GlobalConfigInternal()
		{
			Dispose();
		}
	}
}
