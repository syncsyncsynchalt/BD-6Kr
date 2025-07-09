using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Application
	{
		public delegate void LogCallback(string condition, string stackTrace, LogType type);

		internal static AdvertisingIdentifierCallback OnAdvertisingIdentifierCallback;

		private static LogCallback s_LogCallbackHandler;

		private static LogCallback s_LogCallbackHandlerThreaded;

		private static volatile LogCallback s_RegisterLogCallbackDeprecated;

		public static int loadedLevel
		{
			get;
		}

		public static string loadedLevelName
		{
			get;
		}

		[Obsolete("This property is deprecated, please use LoadLevelAsync to detect if a specific scene is currently loading.")]
		public static bool isLoadingLevel
		{
			get;
		}

		public static int levelCount
		{
			get;
		}

		public static int streamedBytes
		{
			get;
		}

		public static bool isPlaying
		{
			get;
		}

		public static bool isEditor
		{
			get;
		}

		public static bool isWebPlayer
		{
			get;
		}

		public static RuntimePlatform platform
		{
			get;
		}

		public static bool isMobilePlatform
		{
			get
			{
				switch (platform)
				{
					case RuntimePlatform.IPhonePlayer:
					case RuntimePlatform.Android:
					case RuntimePlatform.MetroPlayerX86:
					case RuntimePlatform.MetroPlayerX64:
					case RuntimePlatform.MetroPlayerARM:
					case RuntimePlatform.WP8Player:
					case RuntimePlatform.BlackBerryPlayer:
					case RuntimePlatform.TizenPlayer:
						return true;
					default:
						return false;
				}
			}
		}

		public static bool isConsolePlatform
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				return platform == RuntimePlatform.PS3 || platform == RuntimePlatform.PS4 || platform == RuntimePlatform.XBOX360 || platform == RuntimePlatform.XboxOne;
			}
		}

		public static bool runInBackground
		{
			get;
			set;
		}

		[Obsolete("use Application.isEditor instead")]
		public static bool isPlayer => !isEditor;

		internal static bool isBatchmode
		{
			get;
		}

		internal static bool isHumanControllingUs
		{
			get;
		}

		internal static bool isRunningUnitTests
		{
			get;
		}

		public static string dataPath
		{
			get;
		}

		public static string streamingAssetsPath
		{
			get { return System.IO.Directory.GetCurrentDirectory(); }
		}

		public static string persistentDataPath
		{
			get;
		}

		public static string temporaryCachePath
		{
			get;
		}

		public static string srcValue
		{
			get;
		}

		public static string absoluteURL
		{
			get;
		}

		public static string unityVersion
		{
			get;
		}

		public static string version
		{
			get;
		}

		public static string bundleIdentifier
		{
			get;
		}

		public static ApplicationInstallMode installMode
		{
			get;
		}

		public static ApplicationSandboxType sandboxType
		{
			get;
		}

		public static string productName
		{
			get;
		}

		public static string companyName
		{
			get;
		}

		public static string cloudProjectId
		{
			get;
		}

		public static bool webSecurityEnabled
		{
			get;
		}

		public static string webSecurityHostUrl
		{
			get;
		}

		public static int targetFrameRate
		{
			get;
			set;
		}

		public static SystemLanguage systemLanguage
		{
			get;
		}

		public static StackTraceLogType stackTraceLogType
		{
			get;
			set;
		}

		public static ThreadPriority backgroundLoadingPriority
		{
			get;
			set;
		}

		public static NetworkReachability internetReachability
		{
			get;
		}

		public static bool genuine
		{
			get;
		}

		public static bool genuineCheckAvailable
		{
			get;
		}

		internal static bool submitAnalytics
		{
			get;
		}

		public static bool isShowingSplashScreen
		{
			get;
		}

		public static event LogCallback logMessageReceived
		{
			add
			{
				s_LogCallbackHandler = (LogCallback)Delegate.Combine(s_LogCallbackHandler, value);
				SetLogCallbackDefined(defined: true, s_LogCallbackHandlerThreaded != null);
			}
			remove
			{
				s_LogCallbackHandler = (LogCallback)Delegate.Remove(s_LogCallbackHandler, value);
			}
		}

		public static event LogCallback logMessageReceivedThreaded
		{
			add
			{
				s_LogCallbackHandlerThreaded = (LogCallback)Delegate.Combine(s_LogCallbackHandlerThreaded, value);
				SetLogCallbackDefined(defined: true, threaded: true);
			}
			remove
			{
				s_LogCallbackHandlerThreaded = (LogCallback)Delegate.Remove(s_LogCallbackHandlerThreaded, value);
			}
		}

		public static void Quit() { throw new NotImplementedException("�Ȃɂ���"); }

		public static void CancelQuit() { throw new NotImplementedException("�Ȃɂ���"); }

		public static void LoadLevel(int index)
		{
			LoadLevelAsync(null, index, additive: false, mustCompleteNextFrame: true);
		}

		public static void LoadLevel(string name)
		{
			LoadLevelAsync(name, -1, additive: false, mustCompleteNextFrame: true);
		}

		public static AsyncOperation LoadLevelAsync(int index)
		{
			return LoadLevelAsync(null, index, additive: false, mustCompleteNextFrame: false);
		}

		public static AsyncOperation LoadLevelAsync(string levelName)
		{
			return LoadLevelAsync(levelName, -1, additive: false, mustCompleteNextFrame: false);
		}

		public static AsyncOperation LoadLevelAdditiveAsync(int index)
		{
			return LoadLevelAsync(null, index, additive: true, mustCompleteNextFrame: false);
		}

		public static AsyncOperation LoadLevelAdditiveAsync(string levelName)
		{
			return LoadLevelAsync(levelName, -1, additive: true, mustCompleteNextFrame: false);
		}

		private static AsyncOperation LoadLevelAsync(string monoLevelName, int index, bool additive, bool mustCompleteNextFrame) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool UnloadLevel(int index)
		{
			return UnloadLevel(string.Empty, index);
		}

		public static bool UnloadLevel(string scenePath)
		{
			return UnloadLevel(scenePath, -1);
		}

		private static bool UnloadLevel(string monoScenePath, int index) { throw new NotImplementedException("�Ȃɂ���"); }

		public static void LoadLevelAdditive(int index)
		{
			LoadLevelAsync(null, index, additive: true, mustCompleteNextFrame: true);
		}

		public static void LoadLevelAdditive(string name)
		{
			LoadLevelAsync(name, -1, additive: true, mustCompleteNextFrame: true);
		}

		private static float GetStreamProgressForLevelByName(string levelName) { throw new NotImplementedException("�Ȃɂ���"); }

		public static float GetStreamProgressForLevel(int levelIndex) { throw new NotImplementedException("�Ȃɂ���"); }

		public static float GetStreamProgressForLevel(string levelName)
		{
			return GetStreamProgressForLevelByName(levelName);
		}

		private static bool CanStreamedLevelBeLoadedByName(string levelName) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool CanStreamedLevelBeLoaded(int levelIndex) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool CanStreamedLevelBeLoaded(string levelName)
		{
			return CanStreamedLevelBeLoadedByName(levelName);
		}

		public static void CaptureScreenshot(string filename, [DefaultValue("0")] int superSize) { throw new NotImplementedException("�Ȃɂ���"); }

		[ExcludeFromDocs]
		public static void CaptureScreenshot(string filename)
		{
			int superSize = 0;
			CaptureScreenshot(filename, superSize);
		}

		public static bool HasProLicense() { throw new NotImplementedException("�Ȃɂ���"); }

		internal static bool HasAdvancedLicense() { throw new NotImplementedException("�Ȃɂ���"); }

		internal static bool HasARGV(string name) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static string GetValueForARGV(string name) { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("Use Object.DontDestroyOnLoad instead")]
		public static void DontDestroyOnLoad(Object mono) { throw new NotImplementedException("�Ȃɂ���"); }

		private static string ObjectToJSString(object o)
		{
			if (o == null)
			{
				return "null";
			}
			if (o is string)
			{
				string text = o.ToString().Replace("\\", "\\\\");
				text = text.Replace("\"", "\\\"");
				text = text.Replace("\n", "\\n");
				text = text.Replace("\r", "\\r");
				text = text.Replace("\0", string.Empty);
				text = text.Replace("\u2028", string.Empty);
				text = text.Replace("\u2029", string.Empty);
				return '"' + text + '"';
			}
			if (o is int || o is short || o is uint || o is ushort || o is byte)
			{
				return o.ToString();
			}
			if (o is float)
			{
				NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
				return ((float)o).ToString(numberFormat);
			}
			if (o is double)
			{
				NumberFormatInfo numberFormat2 = CultureInfo.InvariantCulture.NumberFormat;
				return ((double)o).ToString(numberFormat2);
			}
			if (o is char)
			{
				if ((char)o == '"')
				{
					return "\"\\\"\"";
				}
				return '"' + o.ToString() + '"';
			}
			if (o is IList)
			{
				IList list = (IList)o;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("new Array(");
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(ObjectToJSString(list[i]));
				}
				stringBuilder.Append(")");
				return stringBuilder.ToString();
			}
			return ObjectToJSString(o.ToString());
		}

		public static void ExternalCall(string functionName, params object[] args)
		{
			Internal_ExternalCall(BuildInvocationForArguments(functionName, args));
		}

		private static string BuildInvocationForArguments(string functionName, params object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			int num = args.Length;
			for (int i = 0; i < num; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(ObjectToJSString(args[i]));
			}
			stringBuilder.Append(')');
			stringBuilder.Append(';');
			return stringBuilder.ToString();
		}

		public static void ExternalEval(string script)
		{
			if (script.Length > 0 && script[script.Length - 1] != ';')
			{
				script += ';';
			}
			Internal_ExternalCall(script);
		}

		private static void Internal_ExternalCall(string script) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static int GetBuildUnityVersion() { throw new NotImplementedException("�Ȃɂ���"); }

		internal static int GetNumericUnityVersion(string version) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static void InvokeOnAdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled)
		{
			if (OnAdvertisingIdentifierCallback != null)
			{
				OnAdvertisingIdentifierCallback(advertisingId, trackingEnabled);
			}
		}

		public static void OpenURL(string url) { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("For internal use only")]
		public static void CommitSuicide(int mode) { throw new NotImplementedException("�Ȃɂ���"); }

		private static void CallLogCallback(string logString, string stackTrace, LogType type, bool invokedOnMainThread)
		{
			if (invokedOnMainThread)
			{
				s_LogCallbackHandler?.Invoke(logString, stackTrace, type);
			}
			s_LogCallbackHandlerThreaded?.Invoke(logString, stackTrace, type);
		}

		private static void SetLogCallbackDefined(bool defined, bool threaded) { throw new NotImplementedException("�Ȃɂ���"); }

		public static AsyncOperation RequestUserAuthorization(UserAuthorization mode) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool HasUserAuthorization(UserAuthorization mode) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static void ReplyToUserAuthorizationRequest(bool reply, [DefaultValue("false")] bool remember) { throw new NotImplementedException("�Ȃɂ���"); }

		[ExcludeFromDocs]
		internal static void ReplyToUserAuthorizationRequest(bool reply)
		{
			bool remember = false;
			ReplyToUserAuthorizationRequest(reply, remember);
		}

		private static int GetUserAuthorizationRequestMode_Internal() { throw new NotImplementedException("�Ȃɂ���"); }

		internal static UserAuthorization GetUserAuthorizationRequestMode()
		{
			return (UserAuthorization)GetUserAuthorizationRequestMode_Internal();
		}

		[Obsolete("Application.RegisterLogCallback is deprecated. Use Application.logMessageReceived instead.")]
		public static void RegisterLogCallback(LogCallback handler)
		{
			RegisterLogCallback(handler, threaded: false);
		}

		[Obsolete("Application.RegisterLogCallbackThreaded is deprecated. Use Application.logMessageReceivedThreaded instead.")]
		public static void RegisterLogCallbackThreaded(LogCallback handler)
		{
			RegisterLogCallback(handler, threaded: true);
		}

		private static void RegisterLogCallback(LogCallback handler, bool threaded)
		{
			if (s_RegisterLogCallbackDeprecated != null)
			{
				logMessageReceived -= s_RegisterLogCallbackDeprecated;
				logMessageReceivedThreaded -= s_RegisterLogCallbackDeprecated;
			}
			s_RegisterLogCallbackDeprecated = handler;
			if (handler != null)
			{
				if (threaded)
				{
					logMessageReceivedThreaded += handler;
				}
				else
				{
					logMessageReceived += handler;
				}
			}
		}
	}
}
