using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Debug
	{
		public static bool developerConsoleVisible
		{
			get;
			set;
		}

		public static bool isDebugBuild
		{
			get;
		}

		public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
			INTERNAL_CALL_DrawLine(ref start, ref end, ref color, duration, depthTest);
		}

		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
		{
			bool depthTest = true;
			INTERNAL_CALL_DrawLine(ref start, ref end, ref color, duration, depthTest);
		}

		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end, Color color)
		{
			bool depthTest = true;
			float duration = 0f;
			INTERNAL_CALL_DrawLine(ref start, ref end, ref color, duration, depthTest);
		}

		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end)
		{
			bool depthTest = true;
			float duration = 0f;
			Color color = Color.white;
			INTERNAL_CALL_DrawLine(ref start, ref end, ref color, duration, depthTest);
		}

		private static void INTERNAL_CALL_DrawLine(ref Vector3 start, ref Vector3 end, ref Color color, float duration, bool depthTest) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
		{
			bool depthTest = true;
			DrawRay(start, dir, color, duration, depthTest);
		}

		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color)
		{
			bool depthTest = true;
			float duration = 0f;
			DrawRay(start, dir, color, duration, depthTest);
		}

		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir)
		{
			bool depthTest = true;
			float duration = 0f;
			Color white = Color.white;
			DrawRay(start, dir, white, duration, depthTest);
		}

		public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
			DrawLine(start, start + dir, color, duration, depthTest);
		}

		public static void Break() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void DebugBreak() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_Log(int level, string msg, [Writable] Object obj) {
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

		private static void Internal_LogException(Exception exception, [Writable] Object obj) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Log(object message)
		{
			Internal_Log(0, (message == null) ? "Null" : message.ToString(), null);
		}

		public static void Log(object message, Object context)
		{
			Internal_Log(0, (message == null) ? "Null" : message.ToString(), context);
		}

		public static void LogFormat(string format, params object[] args)
		{
			Log(string.Format(format, args));
		}

		public static void LogFormat(Object context, string format, params object[] args)
		{
			Log(string.Format(format, args), context);
		}

		public static void LogError(object message)
		{
			Internal_Log(2, (message == null) ? "Null" : message.ToString(), null);
		}

		public static void LogError(object message, Object context)
		{
			Internal_Log(2, message.ToString(), context);
		}

		public static void LogErrorFormat(string format, params object[] args)
		{
			LogError(string.Format(format, args));
		}

		public static void LogErrorFormat(Object context, string format, params object[] args)
		{
			LogError(string.Format(format, args), context);
		}

		public static void ClearDeveloperConsole() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void WriteLineToLogFile(string message) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void LogException(Exception exception)
		{
			Internal_LogException(exception, null);
		}

		public static void LogException(Exception exception, Object context)
		{
			Internal_LogException(exception, context);
		}

		public static void LogWarning(object message)
		{
			Internal_Log(1, message.ToString(), null);
		}

		public static void LogWarning(object message, Object context)
		{
			Internal_Log(1, message.ToString(), context);
		}

		public static void LogWarningFormat(string format, params object[] args)
		{
			LogWarning(string.Format(format, args));
		}

		public static void LogWarningFormat(Object context, string format, params object[] args)
		{
			LogWarning(string.Format(format, args), context);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				LogAssertion(null);
			}
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				LogAssertion(message);
			}
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string format, params object[] args)
		{
			if (!condition)
			{
				LogAssertion(string.Format(format, args));
			}
		}

		internal static void LogAssertion(string message)
		{
			Internal_Log(3, message, null);
		}

		internal static void OpenConsoleFile() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
