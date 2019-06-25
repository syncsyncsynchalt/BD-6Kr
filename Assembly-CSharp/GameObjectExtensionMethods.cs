using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensionMethods
{
	private static Hashtable _hash = new Hashtable();

	public static void MoveTo(this GameObject obj, Hashtable hash)
	{
		_hash = hash;
		iTween.MoveTo(obj, hash);
	}

	public static void MoveTo(this GameObject obj, Vector3 v0, float ftime)
	{
		iTween.MoveTo(obj, v0, ftime);
	}

	public static void MoveTo(this GameObject obj, Vector3 v0, float ftime, bool local)
	{
		_hash.Clear();
		_hash.Add("position", v0);
		_hash.Add("time", ftime);
		_hash.Add("isLocal", local);
		iTween.MoveTo(obj, _hash);
	}

	public static void MoveTo(this GameObject obj, Vector3 from, Vector3 to, float ftime, bool local)
	{
		obj.transform.position(from);
		_hash.Clear();
		_hash.Add("position", to);
		_hash.Add("time", ftime);
		_hash.Add("isLocal", local);
		iTween.MoveTo(obj, _hash);
	}

	public static void MoveTo(this GameObject obj, Vector3 from, Vector3 to, float ftime, float fdelay, bool local)
	{
		obj.transform.position(from);
		_hash.Clear();
		_hash.Add("position", to);
		_hash.Add("time", ftime);
		_hash.Add("delay", fdelay);
		_hash.Add("isLocal", local);
		iTween.MoveTo(obj, _hash);
	}

	public static void MoveToBezier(this GameObject obj, float ftime, Vector3 start, Vector3 mid1, Vector3 mid2, Vector3 end)
	{
		Vector3 pvr = obj.transform.position;
		Bezier.Interpolate(ref pvr, start, end, Mathe.MinMax2F01(ftime), mid1, mid2);
		obj.transform.position(pvr);
	}

	public static void ScaleTo(this GameObject obj, Hashtable hash)
	{
		_hash = hash;
		iTween.ScaleTo(obj, hash);
	}

	public static void ScaleTo(this GameObject obj, Vector3 scale, float ftime)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("scale", scale);
		hashtable.Add("time", ftime);
		iTween.ScaleTo(obj, hashtable);
	}

	public static void ScaleTo(this GameObject obj, Vector3 scale, float ftime, float fdelay)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("scale", scale);
		hashtable.Add("time", ftime);
		hashtable.Add("delay", fdelay);
		iTween.ScaleTo(obj, hashtable);
	}

	public static void ValueTo(this GameObject obj, Hashtable hash)
	{
		iTween.ValueTo(obj, hash);
	}

	public static void ValueTo(this GameObject obj, float from = 0f, float to = 0f, float time = 0f, string method = "")
	{
		_hash.Clear();
		_hash.Add("from", from);
		_hash.Add("to", to);
		_hash.Add("time", time);
		_hash.Add("onupdate", method);
		iTween.ValueTo(obj, _hash);
	}

	public static void RotateTo(this GameObject obj, Hashtable hash)
	{
		_hash = hash;
		iTween.RotateTo(obj, hash);
	}

	public static void RotateTo(this GameObject obj, Vector3 rot, float time)
	{
		iTween.RotateTo(obj, rot, time);
	}

	public static void RotatoTo(this GameObject obj, Vector3 rot, float time, Action callback)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("rotation", rot);
		if (callback != null)
		{
			hashtable.Add("oncomplate", callback);
		}
		hashtable.Add("time", time);
		iTween.RotateTo(obj, hashtable);
	}

	public static void ResourcesLoad(this GameObject obj, string path)
	{
		obj = (Resources.Load(path) as GameObject);
	}

	public static void FindParentToChild(this GameObject obj, Transform parent, string objName)
	{
		obj = parent.FindChild(objName).gameObject;
		if (obj == null)
		{
			DebugUtils.NullReferenceException(objName + " not found. parent is " + parent.name);
		}
	}

	public static GameObject[] GetChildren(this GameObject self, bool includeInactive = false)
	{
		return (from c in self.GetComponentsInChildren<Transform>(includeInactive)
			where c != self.transform
			select c.gameObject).ToArray();
	}

	public static T GetComponent<T>(this GameObject obj) where T : Component
	{
		return obj.GetComponent<T>() ?? obj.AddComponent<T>();
	}

	public static Component GetComponent(this GameObject gameObject, Type componentType)
	{
		return gameObject.GetComponent(componentType) ?? gameObject.AddComponent(componentType);
	}

	public static T SafeGetComponent<T>(this GameObject gameObject) where T : Component
	{
		if ((UnityEngine.Object)gameObject.GetComponent<T>() != (UnityEngine.Object)null)
		{
			return gameObject.GetComponent<T>();
		}
		return gameObject.AddComponent<T>();
	}

	public static void Discard(this GameObject gameObject)
	{
		foreach (Transform item in gameObject.transform)
		{
			if ((UnityEngine.Object)item.gameObject.GetComponent<Renderer>() != null)
			{
				UnityEngine.Object.Destroy(item.gameObject.GetComponent<Renderer>().material);
			}
			UnityEngine.Object.Destroy(item.gameObject);
		}
		if ((UnityEngine.Object)gameObject.GetComponent<Renderer>() != null)
		{
			UnityEngine.Object.Destroy(gameObject.GetComponent<Renderer>().material);
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	public static void SetActiveChildren(this GameObject gameObject, bool isActive)
	{
		GameObject[] children = gameObject.GetChildren(includeInactive: true);
		GameObject[] array = children;
		foreach (GameObject gameObject2 in array)
		{
			gameObject2.SetActive(isActive);
		}
	}

	public static void SafeGetTweenAlpha(this GameObject obj, float from, float to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if ((bool)obj.GetComponent<TweenAlpha>())
		{
			TweenAlpha component = obj.GetComponent<TweenAlpha>();
			component.ResetToBeginning();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.delay = delay;
			component.method = method;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.PlayForward();
		}
		else
		{
			TweenAlpha component = obj.AddComponent<TweenAlpha>();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.delay = delay;
			component.method = method;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.PlayForward();
		}
	}

	public static void SafeGetTweenScale(this GameObject obj, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if ((bool)obj.GetComponent<TweenScale>())
		{
			TweenScale component = obj.GetComponent<TweenScale>();
			component.ResetToBeginning();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.delay = delay;
			component.method = method;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.PlayForward();
		}
		else
		{
			TweenScale component = obj.AddComponent<TweenScale>();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.method = method;
			component.delay = delay;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.PlayForward();
		}
	}

	public static void SafeGetTweenPosition(this GameObject obj, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if ((bool)obj.GetComponent<TweenPosition>())
		{
			TweenPosition component = obj.GetComponent<TweenPosition>();
			component.ResetToBeginning();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.delay = delay;
			component.method = method;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.PlayForward();
		}
		else
		{
			TweenPosition component = obj.SafeGetComponent<TweenPosition>();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.delay = delay;
			component.method = method;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.Play(forward: true);
		}
	}

	public static void SafeGetTweenRotation(this GameObject obj, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if ((bool)obj.GetComponent<TweenRotation>())
		{
			TweenRotation component = obj.GetComponent<TweenRotation>();
			component.ResetToBeginning();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.delay = delay;
			component.method = method;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.PlayForward();
		}
		else
		{
			TweenRotation component = obj.AddComponent<TweenRotation>();
			component.from = from;
			component.to = to;
			component.duration = duration;
			component.delay = delay;
			component.method = method;
			component.style = style;
			component.eventReceiver = eventReceiver;
			if (component.eventReceiver != null)
			{
				component.callWhenFinished = callWhenFinished;
			}
			component.Play(forward: true);
		}
	}
}
