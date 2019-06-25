using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine
{
	public sealed class ComputeBuffer : IDisposable
	{
		internal IntPtr m_Ptr;

		public int count
		{
			get;
		}

		public int stride
		{
			get;
		}

		public ComputeBuffer(int count, int stride)
			: this(count, stride, ComputeBufferType.Default)
		{
		}

		public ComputeBuffer(int count, int stride, ComputeBufferType type)
		{
			m_Ptr = IntPtr.Zero;
			InitBuffer(this, count, stride, type);
		}

		~ComputeBuffer()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			DestroyBuffer(this);
			m_Ptr = IntPtr.Zero;
		}

		private static void InitBuffer(ComputeBuffer buf, int count, int stride, ComputeBufferType type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void DestroyBuffer(ComputeBuffer buf) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Release()
		{
			Dispose();
		}

		[SecuritySafeCritical]
		public void SetData(Array data)
		{
			InternalSetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		[SecurityCritical]
		private void InternalSetData(Array data, int elemSize) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[SecuritySafeCritical]
		public void GetData(Array data)
		{
			InternalGetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		[SecurityCritical]
		private void InternalGetData(Array data, int elemSize) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void CopyCount(ComputeBuffer src, ComputeBuffer dst, int dstOffset) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
