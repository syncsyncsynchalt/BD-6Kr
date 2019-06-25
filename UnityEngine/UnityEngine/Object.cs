using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public class Object
	{
		private int m_InstanceID;

		private IntPtr m_CachedPtr;

		public string name
		{
			get;
			set;
		}

		public HideFlags hideFlags
		{
			get;
			set;
		}

		private static Object Internal_CloneSingle(Object data) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static Object Internal_InstantiateSingle(Object data, Vector3 pos, Quaternion rot)
		{
			return INTERNAL_CALL_Internal_InstantiateSingle(data, ref pos, ref rot);
		}

		private static Object INTERNAL_CALL_Internal_InstantiateSingle(Object data, ref Vector3 pos, ref Quaternion rot) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Destroy(Object obj, [DefaultValue("0.0F")] float t) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void Destroy(Object obj)
		{
			float t = 0f;
			Destroy(obj, t);
		}

		public static void DestroyImmediate(Object obj, [DefaultValue("false")] bool allowDestroyingAssets) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void DestroyImmediate(Object obj)
		{
			bool allowDestroyingAssets = false;
			DestroyImmediate(obj, allowDestroyingAssets);
		}

		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		public static Object[] FindObjectsOfType(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void DontDestroyOnLoad(Object target) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void DestroyObject(Object obj, [DefaultValue("0.0F")] float t) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void DestroyObject(Object obj)
		{
			float t = 0f;
			DestroyObject(obj, t);
		}

		[Obsolete("use Object.FindObjectsOfType instead.")]
		public static Object[] FindSceneObjectsOfType(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("use Resources.FindObjectsOfTypeAll instead.")]
		public static Object[] FindObjectsOfTypeIncludingAssets(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Please use Resources.FindObjectsOfTypeAll instead")]
		public static Object[] FindObjectsOfTypeAll(Type type)
		{
			return Resources.FindObjectsOfTypeAll(type);
		}

		public override string ToString() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static bool DoesObjectWithInstanceIDExist(int instanceID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public override bool Equals(object o)
		{
			return CompareBaseObjects(this, o as Object);
		}

		public override int GetHashCode()
		{
			return GetInstanceID();
		}

		private static bool CompareBaseObjects(Object lhs, Object rhs)
		{
			bool flag = (object)lhs == null;
			bool flag2 = (object)rhs == null;
			if (flag2 && flag)
			{
				return true;
			}
			if (flag2)
			{
				return !IsNativeObjectAlive(lhs);
			}
			if (flag)
			{
				return !IsNativeObjectAlive(rhs);
			}
			return lhs.m_InstanceID == rhs.m_InstanceID;
		}

		private static bool IsNativeObjectAlive(Object o)
		{
			return o.GetCachedPtr() != IntPtr.Zero;
		}

		public int GetInstanceID()
		{
			return m_InstanceID;
		}

		private IntPtr GetCachedPtr()
		{
			return m_CachedPtr;
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
		{
			CheckNullArgument(original, "The thing you want to instantiate is null.");
			return Internal_InstantiateSingle(original, position, rotation);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original)
		{
			CheckNullArgument(original, "The thing you want to instantiate is null.");
			return Internal_CloneSingle(original);
		}

		public static T Instantiate<T>(T original) where T : Object
		{
			CheckNullArgument(original, "The thing you want to instantiate is null.");
			return (T)Internal_CloneSingle(original);
		}

		private static void CheckNullArgument(object arg, string message)
		{
			if (arg == null)
			{
				throw new ArgumentException(message);
			}
		}

		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(FindObjectsOfType(typeof(T)));
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public static Object FindObjectOfType(Type type)
		{
            var klass = Activator.CreateInstance(type);
            return (UnityEngine.Object) klass;

            // Object[] array = FindObjectsOfType(type);
			// if (array.Length > 0)
			// {
			//  return array[0];
			// }
			// return null;
		}

		public static T FindObjectOfType<T>() where T : Object
		{
			return (T)FindObjectOfType(typeof(T));
		}

		public static implicit operator bool(Object exists)
		{
			return !CompareBaseObjects(exists, null);
		}

		public static bool operator ==(Object x, Object y)
		{
			return CompareBaseObjects(x, y);
		}

		public static bool operator !=(Object x, Object y)
		{
			return !CompareBaseObjects(x, y);
		}
	}
}
