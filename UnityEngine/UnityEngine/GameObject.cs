using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	public sealed class GameObject : Object
	{
		public Transform transform
		{
			get;
		}

		public int layer
		{
			get;
			set;
		}

		[Obsolete("GameObject.active is obsolete. Use GameObject.SetActive(), GameObject.activeSelf or GameObject.activeInHierarchy.")]
		public bool active
		{
			get;
			set;
		}

		public bool activeSelf
		{
			get;
		}

		public bool activeInHierarchy
		{
			get;
		}

		public bool isStatic
		{
			get;
			set;
		}

		internal bool isStaticBatchable
		{
			get;
		}

		public string tag
		{
			get;
			set;
		}

		public GameObject gameObject => this;

		public GameObject(string name)
		{
			Internal_CreateGameObject(this, name);
		}

		public GameObject()
		{
			Internal_CreateGameObject(this, null);
		}

		public GameObject(string name, params Type[] components)
		{
			Internal_CreateGameObject(this, name);
			foreach (Type componentType in components)
			{
				AddComponent(componentType);
			}
		}

		public static GameObject CreatePrimitive(PrimitiveType type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponent(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public T GetComponent<T>()
		{
            throw new NotImplementedException();
            //CastHelper<T> castHelper = default(CastHelper<T>);
			//GetComponentFastPath(typeof(T), new IntPtr(&castHelper.onePointerFurtherThanT));
			//return castHelper.t;
		}

		internal Component GetComponentByName(string type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Component GetComponent(string type)
		{
			return GetComponentByName(type);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type type)
		{
			if (activeInHierarchy)
			{
				Component component = GetComponent(type);
				if (component != null)
				{
					return component;
				}
			}
			Transform transform = this.transform;
			if (transform != null)
			{
				foreach (Transform item in transform)
				{
					Component componentInChildren = item.gameObject.GetComponentInChildren(type);
					if (componentInChildren != null)
					{
						return componentInChildren;
					}
				}
			}
			return null;
		}

		public T GetComponentInChildren<T>()
		{
			return (T)(object)GetComponentInChildren(typeof(T));
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type type)
		{
			if (activeInHierarchy)
			{
				Component component = GetComponent(type);
				if (component != null)
				{
					return component;
				}
			}
			Transform parent = transform.parent;
			if (parent != null)
			{
				while (parent != null)
				{
					if (parent.gameObject.activeInHierarchy)
					{
						Component component2 = parent.gameObject.GetComponent(type);
						if (component2 != null)
						{
							return component2;
						}
					}
					parent = parent.parent;
				}
			}
			return null;
		}

		public T GetComponentInParent<T>()
		{
			return (T)(object)GetComponentInParent(typeof(T));
		}

		public Component[] GetComponents(Type type)
		{
			return (Component[])GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: false, includeInactive: true, reverse: false, null);
		}

		public T[] GetComponents<T>()
		{
			return (T[])GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: false, includeInactive: true, reverse: false, null);
		}

		public void GetComponents(Type type, List<Component> results)
		{
			GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: false, includeInactive: true, reverse: false, results);
		}

		public void GetComponents<T>(List<T> results)
		{
			GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: false, recursive: false, includeInactive: true, reverse: false, results);
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type type)
		{
			bool includeInactive = false;
			return GetComponentsInChildren(type, includeInactive);
		}

		public Component[] GetComponentsInChildren(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: true, includeInactive, reverse: false, null);
		}

		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return (T[])GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: false, null);
		}

		public void GetComponentsInChildren<T>(bool includeInactive, List<T> results)
		{
			GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: false, results);
		}

		public T[] GetComponentsInChildren<T>()
		{
			return GetComponentsInChildren<T>(includeInactive: false);
		}

		public void GetComponentsInChildren<T>(List<T> results)
		{
			GetComponentsInChildren(includeInactive: false, results);
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type type)
		{
			bool includeInactive = false;
			return GetComponentsInParent(type, includeInactive);
		}

		public Component[] GetComponentsInParent(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: true, includeInactive, reverse: true, null);
		}

		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: true, results);
		}

		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return (T[])GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: true, null);
		}

		public T[] GetComponentsInParent<T>()
		{
			return GetComponentsInParent<T>(includeInactive: false);
		}

		private Array GetComponentsInternal(Type type, bool useSearchTypeAsArrayReturnType, bool recursive, bool includeInactive, bool reverse, object resultList) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal Component AddComponentInternal(string className) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetActive(bool value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("gameObject.SetActiveRecursively() is obsolete. Use GameObject.SetActive(), which is now inherited by children.")]
		public void SetActiveRecursively(bool state) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool CompareTag(string tag) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static GameObject FindGameObjectWithTag(string tag) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static GameObject FindWithTag(string tag)
		{
			return FindGameObjectWithTag(tag);
		}

		public static GameObject[] FindGameObjectsWithTag(string tag) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			SendMessageUpwards(methodName, value, options);
		}

		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			SendMessageUpwards(methodName, value, options);
		}

		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			SendMessageUpwards(methodName, null, options);
		}

		public void SendMessage(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SendMessage(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			SendMessage(methodName, value, options);
		}

		[ExcludeFromDocs]
		public void SendMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			SendMessage(methodName, value, options);
		}

		public void SendMessage(string methodName, SendMessageOptions options)
		{
			SendMessage(methodName, null, options);
		}

		public void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			BroadcastMessage(methodName, parameter, options);
		}

		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object parameter = null;
			BroadcastMessage(methodName, parameter, options);
		}

		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			BroadcastMessage(methodName, null, options);
		}

		private Component Internal_AddComponentWithType(Type componentType) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component AddComponent(Type componentType)
		{
			return Internal_AddComponentWithType(componentType);
		}

		public T AddComponent<T>() where T : Component
		{
			return AddComponent(typeof(T)) as T;
		}

		private static void Internal_CreateGameObject([Writable] GameObject mono, string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static GameObject Find(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
