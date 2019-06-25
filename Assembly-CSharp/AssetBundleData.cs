using System;
using System.Collections;
using UnityEngine;

public class AssetBundleData
{
	public static T Load<T>(string path, int version) where T : class
	{
		T val2 = (T)null;
		WWW val = WWW.LoadFromCacheOrDownload(path, version);
		try
		{
			if (val.error != null)
			{
				return (T)null;
			}
			AssetBundle assetBundle = val.assetBundle;
			T result = assetBundle.mainAsset as T;
			assetBundle.Unload(false);
			return result;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static T LoadStreamingAssets<T>(string path, int version) where T : class
	{
		return Load<T>("file://" + $"{Application.dataPath}/StreamingAssets/{path}", version);
	}

	public static IEnumerator LoadAsync<T>(string path, int version, Action<T> onComplate) where T : class
	{
		WWW www = WWW.LoadFromCacheOrDownload(path, version);
		try
		{
			yield return www;
			if (www.error != null)
			{
				onComplate?.Invoke((T)null);
			}
			else
			{
				AssetBundle bundle = www.assetBundle;
				onComplate?.Invoke(bundle.mainAsset as T);
				bundle.Unload(false);
			}
		}
		finally
		{
			((IDisposable)www)?.Dispose();
		}
	}

	public static IEnumerator LoadAsyncStreamingAssets<T>(string path, int version, Action<T> onComplate) where T : class
	{
		return LoadAsync("file://" + $"{Application.dataPath}/StreamingAssets/{path}", version, onComplate);
	}
}
