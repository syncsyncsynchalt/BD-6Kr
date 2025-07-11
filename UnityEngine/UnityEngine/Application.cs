using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using UnityEngine.Internal;

namespace UnityEngine;

public sealed class Application
{
	public delegate void LogCallback(string condition, string stackTrace, LogType type);

	internal static AdvertisingIdentifierCallback OnAdvertisingIdentifierCallback;

	private static LogCallback s_LogCallbackHandler;

	private static LogCallback s_LogCallbackHandlerThreaded;

	private static volatile LogCallback s_RegisterLogCallbackDeprecated;

	public static extern int loadedLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string loadedLevelName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	[Obsolete("This property is deprecated, please use LoadLevelAsync to detect if a specific scene is currently loading.")]
	public static extern bool isLoadingLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern int levelCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern int streamedBytes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool isPlaying
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool isEditor
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool isWebPlayer
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern RuntimePlatform platform
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
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
			RuntimePlatform runtimePlatform = platform;
			return runtimePlatform == RuntimePlatform.PS3 || runtimePlatform == RuntimePlatform.PS4 || runtimePlatform == RuntimePlatform.XBOX360 || runtimePlatform == RuntimePlatform.XboxOne;
		}
	}

	public static extern bool runInBackground
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	[Obsolete("use Application.isEditor instead")]
	public static bool isPlayer => !isEditor;

	internal static extern bool isBatchmode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal static extern bool isHumanControllingUs
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal static extern bool isRunningUnitTests
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string dataPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string streamingAssetsPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	[SecurityCritical]
	public static extern string persistentDataPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string temporaryCachePath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string srcValue
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string absoluteURL
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string unityVersion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string version
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string bundleIdentifier
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern ApplicationInstallMode installMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern ApplicationSandboxType sandboxType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string productName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string companyName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string cloudProjectId
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool webSecurityEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string webSecurityHostUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern int targetFrameRate
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern SystemLanguage systemLanguage
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern StackTraceLogType stackTraceLogType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern ThreadPriority backgroundLoadingPriority
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern NetworkReachability internetReachability
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool genuine
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool genuineCheckAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal static extern bool submitAnalytics
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool isShowingSplashScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void Quit();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void CancelQuit();

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern AsyncOperation LoadLevelAsync(string monoLevelName, int index, bool additive, bool mustCompleteNextFrame);

	public static bool UnloadLevel(int index)
	{
		return UnloadLevel(string.Empty, index);
	}

	public static bool UnloadLevel(string scenePath)
	{
		return UnloadLevel(scenePath, -1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool UnloadLevel(string monoScenePath, int index);

	public static void LoadLevelAdditive(int index)
	{
		LoadLevelAsync(null, index, additive: true, mustCompleteNextFrame: true);
	}

	public static void LoadLevelAdditive(string name)
	{
		LoadLevelAsync(name, -1, additive: true, mustCompleteNextFrame: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern float GetStreamProgressForLevelByName(string levelName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern float GetStreamProgressForLevel(int levelIndex);

	public static float GetStreamProgressForLevel(string levelName)
	{
		return GetStreamProgressForLevelByName(levelName);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool CanStreamedLevelBeLoadedByName(string levelName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool CanStreamedLevelBeLoaded(int levelIndex);

	public static bool CanStreamedLevelBeLoaded(string levelName)
	{
		return CanStreamedLevelBeLoadedByName(levelName);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void CaptureScreenshot(string filename, [DefaultValue("0")] int superSize);

	[ExcludeFromDocs]
	public static void CaptureScreenshot(string filename)
	{
		int superSize = 0;
		CaptureScreenshot(filename, superSize);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool HasProLicense();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern bool HasAdvancedLicense();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern bool HasARGV(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern string GetValueForARGV(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Use Object.DontDestroyOnLoad instead")]
	[WrapperlessIcall]
	public static extern void DontDestroyOnLoad(Object mono);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void Internal_ExternalCall(string script);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern int GetBuildUnityVersion();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern int GetNumericUnityVersion(string version);

	internal static void InvokeOnAdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled)
	{
		if (OnAdvertisingIdentifierCallback != null)
		{
			OnAdvertisingIdentifierCallback(advertisingId, trackingEnabled);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void OpenURL(string url);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	[Obsolete("For internal use only")]
	public static extern void CommitSuicide(int mode);

	private static void CallLogCallback(string logString, string stackTrace, LogType type, bool invokedOnMainThread)
	{
		if (invokedOnMainThread)
		{
			s_LogCallbackHandler?.Invoke(logString, stackTrace, type);
		}
		s_LogCallbackHandlerThreaded?.Invoke(logString, stackTrace, type);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void SetLogCallbackDefined(bool defined, bool threaded);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern AsyncOperation RequestUserAuthorization(UserAuthorization mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool HasUserAuthorization(UserAuthorization mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern void ReplyToUserAuthorizationRequest(bool reply, [DefaultValue("false")] bool remember);

	[ExcludeFromDocs]
	internal static void ReplyToUserAuthorizationRequest(bool reply)
	{
		bool remember = false;
		ReplyToUserAuthorizationRequest(reply, remember);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int GetUserAuthorizationRequestMode_Internal();

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
