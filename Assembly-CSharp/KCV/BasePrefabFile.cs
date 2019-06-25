using System;
using UnityEngine;

namespace KCV
{
	public class BasePrefabFile : IDisposable
	{
		private bool _isDisposed;

		public BasePrefabFile()
		{
			_isDisposed = false;
		}

		public static Transform PassesPrefab(ref Transform prefab)
		{
			Transform result = prefab;
			prefab = null;
			return result;
		}

		public static T InstantiatePrefab<T>(ref T instance, ref Transform prefab, Transform parent)
		{
			return Util.Instantiate(ref instance, ref prefab, parent);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
				}
				_isDisposed = true;
			}
		}
	}
}
