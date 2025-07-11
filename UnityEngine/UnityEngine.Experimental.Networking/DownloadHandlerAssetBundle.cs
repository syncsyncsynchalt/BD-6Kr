using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking;

[StructLayout(LayoutKind.Sequential)]
public sealed class DownloadHandlerAssetBundle : DownloadHandler
{
	protected override byte[] GetData()
	{
		throw new NotSupportedException("Raw data access is not supported for asset bundles");
	}

	protected override string GetText()
	{
		throw new NotSupportedException("String access is not supported for asset bundles");
	}
}
