using KCV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugUtils : SingletonMonoBehaviour<DebugUtils>
{
	private static bool isDebug = false;

	private static bool isDebugMethodName = true;

	private static List<string> strLines = new List<string>();

	private static UILabel _Log;

	private static int _lineCnt = 0;

	private static float checkTime;

	public static bool IsDebug => isDebug;

	private void OnDestroy()
	{
		if ((bool)GameObject.Find("_DebugUtils"))
		{
			UnityEngine.Object.Destroy(GameObject.Find("_DebugUtils").gameObject);
		}
	}

	public static void Log(object msg)
	{
		if (isDebug)
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance != null && !SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw)
			{
				UnityEngine.Debug.Log(msg);
			}
			else
			{
				SLog(msg);
			}
		}
	}

	public static void Log(object className, object msg)
	{
		if (isDebug)
		{
			string name = new StackTrace().GetFrame(1).GetMethod().Name;
			UnityEngine.Debug.Log("[" + className + "::" + name + "]" + msg);
		}
	}

	public static void Log(object msg, UnityEngine.Object context)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log(msg, context);
		}
	}

	public static void MethodLog(object methodName)
	{
		if (isDebugMethodName)
		{
			UnityEngine.Debug.Log(methodName);
		}
	}

	public static void SLog(object text)
	{
		if (!isDebug || SingletonMonoBehaviour<AppInformation>.instance == null)
		{
			return;
		}
		if (!SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw)
		{
			Log(text);
		}
		else if (Application.isPlaying)
		{
			if (strLines.Count > 20)
			{
				strLines.RemoveAt(0);
			}
			if (!GameObject.Find("_DebugUtils"))
			{
				GameObject gameObject = new GameObject("_DebugUtils");
				SingletonMonoBehaviour<DebugUtils>.instance = gameObject.AddComponent<DebugUtils>();
				gameObject.transform.position = new Vector3(9999f, 9999f, 0f);
				UIRoot uIRoot = gameObject.AddComponent<UIRoot>();
				uIRoot.scalingStyle = UIRoot.Scaling.Constrained;
				uIRoot.fitHeight = (uIRoot.fitWidth = true);
				uIRoot.manualWidth = 960;
				uIRoot.manualHeight = 544;
				uIRoot.SetLayer(Generics.Layers.UI2D.IntLayer());
				UIPanel component = gameObject.AddComponent<UIPanel>();
				component.SetLayer(Generics.Layers.UI2D.IntLayer());
				gameObject.transform.AddChild<Camera>("_DebugUtils Camera");
				Camera component2 = ((Component)gameObject.transform.FindChild("_DebugUtils Camera")).GetComponent<Camera>();
				component2.SetLayer(Generics.Layers.UI2D.IntLayer());
				component2.clearFlags = CameraClearFlags.Depth;
				component2.depth = 100f;
				component2.cullingMask = 512;
				component2.orthographic = true;
				component2.nearClipPlane = -0.5f;
				component2.farClipPlane = 0.5f;
				component2.orthographicSize = 1f;
				component2.AddComponent<UICamera>();
				gameObject.transform.AddChild<UILabel>("_Logs");
				_Log = ((Component)gameObject.transform.FindChild("_Logs")).GetComponent<UILabel>();
				_Log.SetLayer(Generics.Layers.UI.IntLayer());
				_Log.transform.localPosition = new Vector3(-477f, 265f, 0f);
				_Log.fontSize = 16;
				_Log.MakePixelPerfect();
				_Log.trueTypeFont = (Resources.Load("Fonts/A-OTF-ShinGoPro-Regular") as Font);
				_Log.effectStyle = UILabel.Effect.Outline;
				_Log.color = Color.white;
				_Log.effectColor = Color.black;
				_Log.overflowMethod = UILabel.Overflow.ResizeFreely;
				_Log.alpha = 0.85f;
				_Log.pivot = UIWidget.Pivot.TopLeft;
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			_Log.text += $"{text}[{Time.time}]\n";
			_lineCnt++;
			if (_lineCnt > 18)
			{
				int count = _Log.text.IndexOf("\n", 0) + 1;
				_Log.text = _Log.text.Remove(0, count);
			}
		}
		else
		{
			Log(text);
		}
	}

	public static void ClearSLog()
	{
		if (_Log != null)
		{
			_Log.text = string.Empty;
		}
		_lineCnt = 0;
	}

	public static void Error(string msg)
	{
		if (isDebug)
		{
			UnityEngine.Debug.LogError(msg);
		}
	}

	public static void Warning(string msg)
	{
		if (isDebug)
		{
			UnityEngine.Debug.LogWarning(msg);
		}
	}

	public static void NullReferenceException(string msg)
	{
		if (isDebug)
		{
			throw new NullReferenceException(msg);
		}
	}

	public static void FindChkNull(object obj, string msg)
	{
		if (isDebug && obj == null)
		{
			throw new NullReferenceException(msg);
		}
	}

	public static void FindChkNull(string msg)
	{
		if (isDebug)
		{
			throw new NullReferenceException(msg);
		}
	}

	public static void ExceptionError(Exception e)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log(e.ToString());
		}
	}

	public static void dbgPause()
	{
	}

	public static void dbgBreak()
	{
	}

	public static void dbgAssert(bool bi)
	{
		if (!bi)
		{
			Log("dbgAssert : Faild.\n");
			dbgBreak();
		}
	}

	public static void dbgAssert(bool bi, string msg)
	{
		if (!bi)
		{
			Log("dbgAssert : Faild. _msg::" + msg);
			dbgBreak();
		}
	}

	public static void WaitForSecond(MonoBehaviour mono, float time, Action callback)
	{
		if (isDebug)
		{
			mono.StartCoroutine(WaitForSecond(time, callback));
		}
	}

	private static IEnumerator WaitForSecond(float time, Action callback)
	{
		yield return new WaitForSeconds(time);
		callback?.Invoke();
	}

	public static void WaitForSecond(MonoBehaviour mono, float time, Action<bool> callback = null)
	{
		mono.StartCoroutine(WaitForSecond(time, callback));
	}

	private static IEnumerator WaitForSecond(float time, Action<bool> callback)
	{
		yield return new WaitForSeconds(time);
		callback?.Invoke(obj: false);
	}

	public static void Trace()
	{
		if (isDebug)
		{
			StackFrame stackFrame = new StackFrame(1, fNeedFileInfo: true);
			string fileName = stackFrame.GetFileName();
			string text = stackFrame.GetMethod().ToString();
			int fileLineNumber = stackFrame.GetFileLineNumber();
			fileName = fileName.Replace(Application.dataPath, string.Empty);
			UnityEngine.Debug.Log(fileName + "\n|Method=" + text + "|Line=" + fileLineNumber);
		}
	}

	public static void StartCheckTime()
	{
		checkTime = 0f;
		checkTime = Time.realtimeSinceStartup;
	}

	public static void CheckTimeLap()
	{
		Log("check time Lap: " + (Time.realtimeSinceStartup - checkTime).ToString("0.00000"));
	}

	public static void EndCheckTime()
	{
		checkTime = Time.realtimeSinceStartup - checkTime;
		Log("check time End: " + checkTime.ToString("0.00000000000"));
	}
}
