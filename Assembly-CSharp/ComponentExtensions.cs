using System;
using UnityEngine;

public static class ComponentExtensions
{
	public static T AddComponent<T>(this Component component) where T : Component
	{
		return component.gameObject.AddComponent<T>();
	}

	public static Component AddComponent(this Component component, Type componentType)
	{
		return component.gameObject.AddComponent(componentType);
	}

	public static Component AddChild(this Component component)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = component.transform;
		return component;
	}

	public static Component AddChild(this Component component, string childName)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = component.transform;
		gameObject.name = childName;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		return component;
	}

	public static Component AddChild<T>(this Component component, string childName) where T : Component
	{
		component.AddChild(childName);
		component.transform.FindChild(childName).AddComponent<T>();
		return component;
	}

	public static Component AddChild<T>(this Component component, string childName, ref T instance) where T : Component
	{
		component.AddChild(childName);
		instance = component.transform.FindChild(childName).AddComponent<T>();
		return component;
	}

	public static Component SetActive(this Component component, bool isActive)
	{
		component.gameObject.SetActive(isActive);
		return component;
	}

	public static void SetActiveChildren(this Component component, bool isActive)
	{
		component.gameObject.SetActiveChildren(isActive);
	}

	public static Component SetLayer(this Component component, int layer)
	{
		if (component.gameObject.layer != layer)
		{
			component.gameObject.layer = layer;
		}
		return component;
	}

	public static Component SetLayer(this Component component, int layer, bool includeChildren)
	{
		component = component.SetLayer(layer);
		if (includeChildren)
		{
			foreach (Transform item in component.transform)
			{
				if (item.gameObject.layer != layer)
				{
					item.SetLayer(layer);
				}
			}
			return component;
		}
		return component;
	}

	public static Component SetRenderQueue(this Component component, int queue)
	{
		if ((UnityEngine.Object)component.GetComponent<Renderer>() != null && component.GetComponent<Renderer>().material != null)
		{
			component.GetComponent<Renderer>().material.renderQueue = queue;
		}
		return component;
	}

	public static Component SetRenderQueue(this Component component, int queue, bool includeChildren)
	{
		component = component.SetRenderQueue(queue);
		if (includeChildren)
		{
			foreach (Transform item in component.transform)
			{
				item.SetRenderQueue(queue);
			}
			return component;
		}
		return component;
	}

	public static T SafeGetComponent<T>(this Component component) where T : Component
	{
		if ((UnityEngine.Object)component.GetComponent<T>() != (UnityEngine.Object)null)
		{
			return component.GetComponent<T>();
		}
		return component.AddComponent<T>();
	}

	public static Component SafeGetComponent(this Component component, Type componentType)
	{
		if (component.GetComponent(componentType) != null)
		{
			return component.GetComponent(componentType);
		}
		return component.AddComponent(componentType);
	}

	public static void SafeGetTweenAlpha(this Component component, float from, float to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		component.gameObject.SafeGetTweenAlpha(from, to, duration, delay, method, style, eventReceiver, callWhenFinished);
	}

	public static void SafeGetTweenScale(this Component component, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		component.gameObject.SafeGetTweenScale(from, to, duration, delay, method, style, eventReceiver, callWhenFinished);
	}

	public static void SafeGetTweenPosition(this Component component, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		component.gameObject.SafeGetTweenPosition(from, to, duration, delay, method, style, eventReceiver, callWhenFinished);
	}

	public static void SafeGetTweenRotation(this Component component, Vector3 from, Vector3 to, float duration, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		TweenRotation tweenRotation = component.SafeGetComponent<TweenRotation>();
		tweenRotation.from = from;
		tweenRotation.to = to;
		tweenRotation.duration = duration;
		tweenRotation.method = method;
		tweenRotation.style = style;
		tweenRotation.eventReceiver = eventReceiver;
		tweenRotation.ResetToBeginning();
		if (tweenRotation.eventReceiver != null)
		{
			tweenRotation.callWhenFinished = callWhenFinished;
		}
		tweenRotation.Play(forward: true);
	}
}
