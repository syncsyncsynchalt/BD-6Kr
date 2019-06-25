using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public static class MonobehaviourLogExtensions
{
	public static void Log(this MonoBehaviour self, string msg)
	{
		string name = new StackTrace().GetFrame(1).GetMethod().Name;
		string message = string.Format("{2} - {0} {1}", self, name, msg);
		UnityEngine.Debug.Log(message, self);
	}

	public static void Log(this MonoBehaviour self, object msg)
	{
		string name = new StackTrace().GetFrame(1).GetMethod().Name;
		string message = string.Format("{2} - {0} {1}", self, name, msg.ToString());
		UnityEngine.Debug.Log(message, self);
	}

	public static Coroutine DelayAction(this MonoBehaviour self, float delayTime, Action callback)
	{
		return self.StartCoroutine(_delayAction(delayTime, callback));
	}

	private static IEnumerator _delayAction(float delayTime, Action callback)
	{
		yield return new WaitForSeconds(delayTime);
		callback?.Invoke();
	}

	public static void DelayActionFrame(this MonoBehaviour self, int delayFrame, Action callback)
	{
		self.StartCoroutine(_delayActionFrame(delayFrame, callback));
	}

	private static IEnumerator _delayActionFrame(int delayFrame, Action callback)
	{
		for (int i = 0; i < delayFrame; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		callback?.Invoke();
	}

	public static void DelayActionCoroutine(this MonoBehaviour self, Coroutine cor, Action callback)
	{
		self.StartCoroutine(_delayActionCoroutine(cor, callback));
	}

	private static IEnumerator _delayActionCoroutine(Coroutine cor, Action callback)
	{
		yield return cor;
		callback?.Invoke();
	}

	public static T GetComponentThis<T>(this MonoBehaviour self, ref T instance)
	{
		if (instance != null)
		{
			return instance;
		}
		return instance = self.GetComponent<T>();
	}
}
