using System;
using System.Collections;
using UniRx.InternalUtil;
using UnityEngine;

namespace UniRx
{
	public sealed class MainThreadDispatcher : MonoBehaviour
	{
		public enum CullingMode
		{
			Disabled,
			Self,
			All
		}

		public static CullingMode cullingMode = CullingMode.Self;

		private ThreadSafeQueueWorker queueWorker = new ThreadSafeQueueWorker();

		private Action<Exception> unhandledExceptionCallback = delegate(Exception ex)
		{
			Debug.LogException(ex);
		};

		private static MainThreadDispatcher instance;

		private static bool initialized;

		private static bool isQuitting;

		[ThreadStatic]
		private static object mainThreadToken;

		private Subject<bool> onApplicationFocus;

		private Subject<bool> onApplicationPause;

		private Subject<Unit> onApplicationQuit;

		public static string InstanceName
		{
			get
			{
				if (instance == null)
				{
					throw new NullReferenceException("MainThreadDispatcher is not initialized.");
				}
				return instance.name;
			}
		}

		public static bool IsInitialized => initialized && instance != null;

		private static MainThreadDispatcher Instance
		{
			get
			{
				Initialize();
				return instance;
			}
		}

		public static void Post(Action action)
		{
			MainThreadDispatcher mainThreadDispatcher = Instance;
			if (mainThreadDispatcher != null)
			{
				mainThreadDispatcher.queueWorker.Enqueue(action);
			}
		}

		public static void Send(Action action)
		{
			if (mainThreadToken != null)
			{
				try
				{
					action();
				}
				catch (Exception obj)
				{
					MainThreadDispatcher mainThreadDispatcher = Instance;
					if (mainThreadDispatcher != null)
					{
						mainThreadDispatcher.unhandledExceptionCallback(obj);
					}
				}
			}
			else
			{
				Post(action);
			}
		}

		public static void UnsafeSend(Action action)
		{
			try
			{
				action();
			}
			catch (Exception obj)
			{
				MainThreadDispatcher mainThreadDispatcher = Instance;
				if (mainThreadDispatcher != null)
				{
					mainThreadDispatcher.unhandledExceptionCallback(obj);
				}
			}
		}

		public static void SendStartCoroutine(IEnumerator routine)
		{
			if (mainThreadToken != null)
			{
				StartCoroutine(routine);
				return;
			}
			MainThreadDispatcher mainThreadDispatcher = Instance;
			if (mainThreadDispatcher != null)
			{
				mainThreadDispatcher.queueWorker.Enqueue(delegate
				{
					MainThreadDispatcher mainThreadDispatcher2 = Instance;
					if (mainThreadDispatcher2 != null)
					{
						((MonoBehaviour)mainThreadDispatcher2).StartCoroutine_Auto(routine);
					}
				});
			}
		}

		public new static Coroutine StartCoroutine(IEnumerator routine)
		{
			MainThreadDispatcher mainThreadDispatcher = Instance;
			if (mainThreadDispatcher != null)
			{
				return ((MonoBehaviour)mainThreadDispatcher).StartCoroutine_Auto(routine);
			}
			return null;
		}

		public static void RegisterUnhandledExceptionCallback(Action<Exception> exceptionCallback)
		{
			if (exceptionCallback == null)
			{
				Instance.unhandledExceptionCallback = Stubs.Ignore<Exception>;
			}
			else
			{
				Instance.unhandledExceptionCallback = exceptionCallback;
			}
		}

		public static void Initialize()
		{
			if (initialized)
			{
				return;
			}
			MainThreadDispatcher x;
			try
			{
				x = UnityEngine.Object.FindObjectOfType<MainThreadDispatcher>();
			}
			catch
			{
				Exception ex = new Exception("UniRx requires a MainThreadDispatcher component created on the main thread. Make sure it is added to the scene before calling UniRx from a worker thread.");
				Debug.LogException(ex);
				throw ex;
			}
			if (!isQuitting)
			{
				if (x == null)
				{
					instance = new GameObject("MainThreadDispatcher").AddComponent<MainThreadDispatcher>();
				}
				else
				{
					instance = x;
				}
				UnityEngine.Object.DontDestroyOnLoad(instance);
				mainThreadToken = new object();
				initialized = true;
			}
		}

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				mainThreadToken = new object();
				initialized = true;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			else if (cullingMode == CullingMode.Self)
			{
				Debug.LogWarning("There is already a MainThreadDispatcher in the scene. Removing myself...");
				DestroyDispatcher(this);
			}
			else if (cullingMode == CullingMode.All)
			{
				Debug.LogWarning("There is already a MainThreadDispatcher in the scene. Cleaning up all excess dispatchers...");
				CullAllExcessDispatchers();
			}
			else
			{
				Debug.LogWarning("There is already a MainThreadDispatcher in the scene.");
			}
		}

		private static void DestroyDispatcher(MainThreadDispatcher aDispatcher)
		{
			if (!(aDispatcher != instance))
			{
				return;
			}
			Component[] components = aDispatcher.gameObject.GetComponents<Component>();
			if (aDispatcher.gameObject.transform.childCount == 0 && components.Length == 2)
			{
				if (components[0] is Transform && components[1] is MainThreadDispatcher)
				{
					UnityEngine.Object.Destroy(aDispatcher.gameObject);
				}
			}
			else
			{
				UnityEngine.Object.Destroy(aDispatcher);
			}
		}

		public static void CullAllExcessDispatchers()
		{
			MainThreadDispatcher[] array = UnityEngine.Object.FindObjectsOfType<MainThreadDispatcher>();
			for (int i = 0; i < array.Length; i++)
			{
				DestroyDispatcher(array[i]);
			}
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = UnityEngine.Object.FindObjectOfType<MainThreadDispatcher>();
				initialized = (instance != null);
			}
		}

		private void Update()
		{
			queueWorker.ExecuteAll(unhandledExceptionCallback);
		}

		private void OnLevelWasLoaded(int level)
		{
		}

		private void OnApplicationFocus(bool focus)
		{
			if (onApplicationFocus != null)
			{
				onApplicationFocus.OnNext(focus);
			}
		}

		public static IObservable<bool> OnApplicationFocusAsObservable()
		{
			return Instance.onApplicationFocus ?? (Instance.onApplicationFocus = new Subject<bool>());
		}

		private void OnApplicationPause(bool pause)
		{
			if (onApplicationPause != null)
			{
				onApplicationPause.OnNext(pause);
			}
		}

		public static IObservable<bool> OnApplicationPauseAsObservable()
		{
			return Instance.onApplicationPause ?? (Instance.onApplicationPause = new Subject<bool>());
		}

		private void OnApplicationQuit()
		{
			isQuitting = true;
			if (onApplicationQuit != null)
			{
				onApplicationQuit.OnNext(Unit.Default);
			}
		}

		public static IObservable<Unit> OnApplicationQuitAsObservable()
		{
			return Instance.onApplicationQuit ?? (Instance.onApplicationQuit = new Subject<Unit>());
		}
	}
}
