using System;
using System.Runtime.CompilerServices;
using UnityEngineInternal;

namespace UnityEngine
{
	public sealed class AssetBundle : Object
	{
		public Object mainAsset
		{
			get;
		}

		public static AssetBundleCreateRequest CreateFromMemory(byte[] binary) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static AssetBundle CreateFromMemoryImmediate(byte[] binary) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static AssetBundle CreateFromFile(string path) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Contains(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
		public Object Load(string name)
		{
			return null;
		}

		[Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
		public T Load<T>(string name) where T : Object
		{
			return (T)null;
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
		[Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
		public Object Load(string name, Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Method LoadAsync has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAssetAsync instead and check the documentation for details.", true)]
		public AssetBundleRequest LoadAsync(string name, Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
		public Object[] LoadAll(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
		public Object[] LoadAll()
		{
			return null;
		}

		[Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
		public T[] LoadAll<T>() where T : Object
		{
			return null;
		}

		public Object LoadAsset(string name)
		{
			return LoadAsset(name, typeof(Object));
		}

		public T LoadAsset<T>(string name) where T : Object
		{
			return (T)LoadAsset(name, typeof(T));
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
		public Object LoadAsset(string name, Type type)
		{
			if (name == null)
			{
				throw new NullReferenceException("The input asset name cannot be null.");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("The input asset name cannot be empty.");
			}
			if (type == null)
			{
				throw new NullReferenceException("The input type cannot be null.");
			}
			return LoadAsset_Internal(name, type);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
		private Object LoadAsset_Internal(string name, Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AssetBundleRequest LoadAssetAsync(string name)
		{
			return LoadAssetAsync(name, typeof(Object));
		}

		public AssetBundleRequest LoadAssetAsync<T>(string name)
		{
			return LoadAssetAsync(name, typeof(T));
		}

		public AssetBundleRequest LoadAssetAsync(string name, Type type)
		{
			if (name == null)
			{
				throw new NullReferenceException("The input asset name cannot be null.");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("The input asset name cannot be empty.");
			}
			if (type == null)
			{
				throw new NullReferenceException("The input type cannot be null.");
			}
			return LoadAssetAsync_Internal(name, type);
		}

		private AssetBundleRequest LoadAssetAsync_Internal(string name, Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Object[] LoadAssetWithSubAssets(string name)
		{
			return LoadAssetWithSubAssets(name, typeof(Object));
		}

		public T[] LoadAssetWithSubAssets<T>(string name) where T : Object
		{
			return Resources.ConvertObjects<T>(LoadAssetWithSubAssets(name, typeof(T)));
		}

		public Object[] LoadAssetWithSubAssets(string name, Type type)
		{
			if (name == null)
			{
				throw new NullReferenceException("The input asset name cannot be null.");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("The input asset name cannot be empty.");
			}
			if (type == null)
			{
				throw new NullReferenceException("The input type cannot be null.");
			}
			return LoadAssetWithSubAssets_Internal(name, type);
		}

		internal Object[] LoadAssetWithSubAssets_Internal(string name, Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AssetBundleRequest LoadAssetWithSubAssetsAsync(string name)
		{
			return LoadAssetWithSubAssetsAsync(name, typeof(Object));
		}

		public AssetBundleRequest LoadAssetWithSubAssetsAsync<T>(string name)
		{
			return LoadAssetWithSubAssetsAsync(name, typeof(T));
		}

		public AssetBundleRequest LoadAssetWithSubAssetsAsync(string name, Type type)
		{
			if (name == null)
			{
				throw new NullReferenceException("The input asset name cannot be null.");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("The input asset name cannot be empty.");
			}
			if (type == null)
			{
				throw new NullReferenceException("The input type cannot be null.");
			}
			return LoadAssetWithSubAssetsAsync_Internal(name, type);
		}

		private AssetBundleRequest LoadAssetWithSubAssetsAsync_Internal(string name, Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Object[] LoadAllAssets()
		{
			return LoadAllAssets(typeof(Object));
		}

		public T[] LoadAllAssets<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(LoadAllAssets(typeof(T)));
		}

		public Object[] LoadAllAssets(Type type)
		{
			if (type == null)
			{
				throw new NullReferenceException("The input type cannot be null.");
			}
			return LoadAssetWithSubAssets_Internal(string.Empty, type);
		}

		public AssetBundleRequest LoadAllAssetsAsync()
		{
			return LoadAllAssetsAsync(typeof(Object));
		}

		public AssetBundleRequest LoadAllAssetsAsync<T>()
		{
			return LoadAllAssetsAsync(typeof(T));
		}

		public AssetBundleRequest LoadAllAssetsAsync(Type type)
		{
			if (type == null)
			{
				throw new NullReferenceException("The input type cannot be null.");
			}
			return LoadAssetWithSubAssetsAsync_Internal(string.Empty, type);
		}

		public void Unload(bool unloadAllLoadedObjects) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("This method is deprecated. Use GetAllAssetNames() instead.")]
		public string[] AllAssetNames()
		{
			return GetAllAssetNames();
		}

		public string[] GetAllAssetNames() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public string[] GetAllScenePaths() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
