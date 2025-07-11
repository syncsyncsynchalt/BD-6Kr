using System;
using System.Runtime.InteropServices;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public sealed class AssetBundleRequest : AsyncOperation
{
	internal AssetBundle m_AssetBundle;

	internal string m_Path;

	internal Type m_Type;

	public Object asset => m_AssetBundle.LoadAsset(m_Path, m_Type);

	public Object[] allAssets => m_AssetBundle.LoadAssetWithSubAssets_Internal(m_Path, m_Type);
}
