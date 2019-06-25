using System;
using System.Collections;
using UnityEngine;

public static class AsyncLoadScene
{
	private static AsyncOperation _async;

	public static AsyncOperation Async => _async;

	public static float Progress
	{
		get;
		private set;
	}

	public static bool IsLoadComplate
	{
		get
		{
			if (_async != null && _async.progress >= 0.9f)
			{
				return true;
			}
			return false;
		}
		private set
		{
		}
	}

	public static void Begin(string sceneName, bool allowSceneActivation = false, int priority = 0)
	{
		_async = Application.LoadLevelAsync(sceneName);
		_async.priority = priority;
		_async.allowSceneActivation = allowSceneActivation;
		if (_async != null)
		{
			Debug.Log("nullじゃない");
		}
		else
		{
			Debug.Log("null");
		}
		if (_async != null && !_async.isDone)
		{
			Progress = _async.progress;
		}
	}

	public static IEnumerator AsyncBegin(string sceneName, bool allowSceneActivation = false, int priority = 0)
	{
		_async = Application.LoadLevelAsync(sceneName);
		_async.priority = priority;
		_async.allowSceneActivation = false;
		while (!_async.isDone)
		{
			Progress = _async.progress;
			if (_async.progress >= 0.9f && allowSceneActivation)
			{
				_async.allowSceneActivation = true;
				Release();
			}
			yield return new WaitForSeconds(0f);
		}
		yield return _async;
	}

	public static bool ScemeActivation()
	{
		if ((double)_async.progress >= 0.9)
		{
			_async.allowSceneActivation = true;
			Release();
			return true;
		}
		return false;
	}

	private static void Release()
	{
		_async = null;
		Progress = 0f;
		IsLoadComplate = false;
	}

	public static void LoadLevelAsyncScene(MonoBehaviour mono, string sceneName, Action callback)
	{
		mono.StartCoroutine(LoadLevelAsyncScene(sceneName, callback));
	}

	public static void LoadLevelAsyncScene(MonoBehaviour mono, Generics.Scene scene, Action callback)
	{
		mono.StartCoroutine(LoadLevelAsyncScene(scene.ToString(), callback));
	}

	private static IEnumerator LoadLevelAsyncScene(string sceneName, Action callback)
	{
		yield return Application.LoadLevelAsync(sceneName);
		callback?.Invoke();
	}

	public static void LoadAdditiveAsyncScene(MonoBehaviour mono, string sceneName, Action callback)
	{
		mono.StartCoroutine(AdditiveAsyncScene(sceneName, callback));
	}

	private static IEnumerator AdditiveAsyncScene(string sceneName, Action callback)
	{
		yield return Application.LoadLevelAdditiveAsync(sceneName);
		callback?.Invoke();
	}
}
