using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Cloud.Service
{
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class CloudService : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		public string serviceFolderName
		{
			get;
		}

		public CloudService(CloudServiceType serviceType)
		{
			InternalCreate(serviceType);
		}

		internal void InternalCreate(CloudServiceType serviceType) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void InternalDestroy() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~CloudService()
		{
			InternalDestroy();
		}

		public void Dispose()
		{
			InternalDestroy();
			GC.SuppressFinalize(this);
		}

		public bool Initialize(string projectId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool StartEventHandler(string sessionInfo, int maxNumberOfEventInQueue, int maxEventTimeoutInSec) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool PauseEventHandler(bool flushEvents) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool StopEventHandler() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool StartEventDispatcher(CloudServiceConfig serviceConfig, Dictionary<string, string> headers)
		{
			return InternalStartEventDispatcher(serviceConfig, FlattenedHeadersFrom(headers));
		}

		private bool InternalStartEventDispatcher(CloudServiceConfig serviceConfig, string[] headers) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool PauseEventDispatcher() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool StopEventDispatcher() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ResetNetworkRetryIndex() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool QueueEvent(string eventData, CloudEventFlags flags) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SaveFileFromServer(string fileName, string url, Dictionary<string, string> headers, object d, string methodName)
		{
			if (methodName == null)
			{
				methodName = string.Empty;
			}
			return InternalSaveFileFromServer(fileName, url, FlattenedHeadersFrom(headers), d, methodName);
		}

		private bool InternalSaveFileFromServer(string fileName, string url, string[] headers, object d, string methodName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SaveFile(string fileName, string data) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public string RestoreFile(string fileName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static string[] FlattenedHeadersFrom(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				return null;
			}
			string[] array = new string[headers.Count * 2];
			int num = 0;
			foreach (KeyValuePair<string, string> header in headers)
			{
				array[num++] = header.Key.ToString();
				array[num++] = header.Value.ToString();
			}
			return array;
		}
	}
}
