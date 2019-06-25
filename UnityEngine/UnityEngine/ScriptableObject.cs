using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public class ScriptableObject : Object
	{
		public ScriptableObject()
		{
			Internal_CreateScriptableObject(this);
		}

		private static void Internal_CreateScriptableObject([Writable] ScriptableObject self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use EditorUtility.SetDirty instead")]
		public void SetDirty()
		{
			INTERNAL_CALL_SetDirty(this);
		}

		private static void INTERNAL_CALL_SetDirty(ScriptableObject self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static ScriptableObject CreateInstance(string className) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static ScriptableObject CreateInstance(Type type)
		{
			return CreateInstanceFromType(type);
		}

		private static ScriptableObject CreateInstanceFromType(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static T CreateInstance<T>() where T : ScriptableObject
		{
			return (T)CreateInstance(typeof(T));
		}
	}
}
