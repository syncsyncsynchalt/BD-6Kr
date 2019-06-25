using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.Experimental.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public class DownloadHandler : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		public byte[] data => GetData();

		public string text => GetText();

		internal DownloadHandler()
		{
		}

		private void InternalDestroy() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~DownloadHandler()
		{
			InternalDestroy();
		}

		public void Dispose()
		{
			InternalDestroy();
			GC.SuppressFinalize(this);
		}

		protected virtual byte[] GetData()
		{
			return null;
		}

		protected virtual string GetText()
		{
			byte[] data = GetData();
			if (data != null && data.Length > 0)
			{
				return Encoding.UTF8.GetString(data, 0, data.Length);
			}
			return string.Empty;
		}

		protected virtual bool ReceiveData(byte[] data, int dataLength)
		{
			return true;
		}

		protected virtual void ReceiveContentLength(int contentLength)
		{
		}

		protected virtual void CompleteContent()
		{
		}

		protected virtual float GetProgress()
		{
			return 0.5f;
		}
	}
}
