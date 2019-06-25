using System;
using System.Runtime.CompilerServices;
using UnityEngineInternal;

namespace UnityEngine
{
	public sealed class Resources
	{
		internal static T[] ConvertObjects<T>(Object[] rawObjects) where T : Object
		{
			if (rawObjects == null)
			{
				return null;
			}
			T[] array = new T[rawObjects.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (T)rawObjects[i];
			}
			return array;
		}

		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		public static Object[] FindObjectsOfTypeAll(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static T[] FindObjectsOfTypeAll<T>() where T : Object
		{
			return ConvertObjects<T>(FindObjectsOfTypeAll(typeof(T)));
		}

		public static Object Load(string path)
		{
			return Load(path, typeof(Object));
		}

		public static T Load<T>(string path) where T : Object
		{
			return (T)Load(path, typeof(T));
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
		public static Object Load(string path, Type systemTypeInstance) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static ResourceRequest LoadAsync(string path)
		{
			return LoadAsync(path, typeof(Object));
		}

		public static ResourceRequest LoadAsync<T>(string path) where T : Object
		{
			return LoadAsync(path, typeof(T));
		}

		public static ResourceRequest LoadAsync(string path, Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Object[] LoadAll(string path, Type systemTypeInstance) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Object[] LoadAll(string path)
		{
			return LoadAll(path, typeof(Object));
		}

		public static T[] LoadAll<T>(string path) where T : Object
		{
			return ConvertObjects<T>(LoadAll(path, typeof(T)));
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public static Object GetBuiltinResource(Type type, string path) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static T GetBuiltinResource<T>(string path) where T : Object
		{
			return (T)GetBuiltinResource(typeof(T), path);
		}

		public static void UnloadAsset(Object assetToUnload) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static AsyncOperation UnloadUnusedAssets() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
