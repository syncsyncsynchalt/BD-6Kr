using UnityEngine;

namespace KCV
{
	public static class PrefabFile
	{
		public static GameObject Load(string path)
		{
			return Resources.Load($"{AppDataPath.PrefabFilePath}/{path}") as GameObject;
		}

		public static GameObject LoadAsync(string path)
		{
			return Resources.LoadAsync($"{AppDataPath.PrefabFilePath}/{path}").asset as GameObject;
		}

		public static GameObject Load(PrefabFileInfos prefabInfos)
		{
			return Resources.Load($"{AppDataPath.PrefabFilePath}/{prefabInfos.PrefabPath()}") as GameObject;
		}

		public static GameObject LoadAsync(PrefabFileInfos prefabInfos)
		{
			return Resources.LoadAsync($"{AppDataPath.PrefabFilePath}/{prefabInfos.PrefabPath()}").asset as GameObject;
		}

		public static T Load<T>(string path) where T : Component
		{
			return Resources.Load<T>($"{AppDataPath.PrefabFilePath}/{path}");
		}

		public static T LoadAsync<T>(string path) where T : Component
		{
			return (T)Resources.LoadAsync<T>($"{AppDataPath.PrefabFilePath}/{path}").asset;
		}

		public static T Load<T>(PrefabFileInfos prefabInfos) where T : Component
		{
			return Resources.Load<T>($"{AppDataPath.PrefabFilePath}/{prefabInfos.PrefabPath()}");
		}

		public static T LoadAsync<T>(PrefabFileInfos prefabInfos) where T : Component
		{
			return (T)Resources.LoadAsync<T>($"{AppDataPath.PrefabFilePath}/{prefabInfos.PrefabPath()}").asset;
		}

		public static GameObject Instantiate(string path, Transform parent = null)
		{
			GameObject gameObject = Object.Instantiate(Load(path), Vector3.zero, Quaternion.identity) as GameObject;
			if (gameObject == null)
			{
				return null;
			}
			if (parent != null)
			{
				gameObject.transform.parent = parent;
			}
			gameObject.transform.localScale = Vector3.one;
			return gameObject;
		}

		public static T Instantiate<T>(string path, Transform parent = null) where T : Component
		{
			GameObject gameObject = Object.Instantiate(Load(path), Vector3.zero, Quaternion.identity) as GameObject;
			if (gameObject == null)
			{
				return (T)null;
			}
			if (parent != null)
			{
				gameObject.transform.parent = parent;
			}
			gameObject.transform.localScale = Vector3.one;
			return gameObject.SafeGetComponent<T>();
		}

		public static GameObject Instantiate(PrefabFileInfos prefabInfos, Transform parent = null)
		{
			return Instantiate(prefabInfos.PrefabPath(), parent);
		}

		public static T Instantiate<T>(PrefabFileInfos prefabInfo, Transform parent = null) where T : Component
		{
			GameObject gameObject = Object.Instantiate(Load(prefabInfo.PrefabPath()), Vector3.zero, Quaternion.identity) as GameObject;
			if (gameObject == null)
			{
				return (T)null;
			}
			if (parent != null)
			{
				gameObject.transform.parent = parent;
			}
			gameObject.transform.localScale = Vector3.one;
			return gameObject.SafeGetComponent<T>();
		}

		public static T Instantiate<T>(PrefabFileInfos prefabInfo, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null) where T : Component
		{
			GameObject gameObject = Object.Instantiate(Load(prefabInfo.PrefabPath()), Vector3.zero, Quaternion.identity) as GameObject;
			if (gameObject == null)
			{
				return (T)null;
			}
			if (parent != null)
			{
				gameObject.transform.parent = parent;
			}
			gameObject.transform.localPosition = pos;
			gameObject.transform.localRotation = rot;
			gameObject.transform.localScale = scale;
			return gameObject.SafeGetComponent<T>();
		}
	}
}
