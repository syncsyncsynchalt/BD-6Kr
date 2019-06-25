using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;

	public static T Instance
	{
		get
		{
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
				if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
				{
					return (T)null;
				}
			}
			return instance;
		}
		set
		{
			instance = value;
		}
	}

	public static T GetInstance()
	{
		return instance;
	}

	protected virtual void Awake()
	{
		CheckInstance();
	}

	public bool CheckInstance()
	{
		if ((UnityEngine.Object)instance != (UnityEngine.Object)null && instance != this)
		{
			DebugUtils.SLog("★★SingletonObject Destroy " + base.gameObject.name);
			UnityEngine.Object.Destroy(base.gameObject);
			return false;
		}
		instance = Instance;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		return true;
	}

	[Obsolete]
	public static bool exist()
	{
		if ((UnityEngine.Object)instance != (UnityEngine.Object)null)
		{
			return true;
		}
		return false;
	}
}
