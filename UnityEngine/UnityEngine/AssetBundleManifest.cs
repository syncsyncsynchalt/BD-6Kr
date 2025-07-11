using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class AssetBundleManifest : Object
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern string[] GetAllAssetBundles();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern string[] GetAllAssetBundlesWithVariant();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Hash128 GetAssetBundleHash(string assetBundleName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern string[] GetDirectDependencies(string assetBundleName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern string[] GetAllDependencies(string assetBundleName);
}
