using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.Experimental.Networking;

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void InternalDestroy();

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
		byte[] array = GetData();
		if (array != null && array.Length > 0)
		{
			return Encoding.UTF8.GetString(array, 0, array.Length);
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
