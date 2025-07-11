using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita;

public sealed class Utility
{
	public enum SkuFlags
	{
		None = 0,
		Trial = 1,
		Full = 3
	}

	public enum PowerTickType
	{
		Default = 0,
		DisableAutoSuspend = 1,
		DisableDisplayOff = 4,
		DisableDisplayDimming = 6
	}

	public enum MountableContent
	{
		Music,
		Photos
	}

	public enum SystemLanguage
	{
		JAPANESE,
		ENGLISH_US,
		FRENCH,
		SPANISH,
		GERMAN,
		ITALIAN,
		DUTCH,
		PORTUGUESE_PT,
		RUSSIAN,
		KOREAN,
		CHINESE_T,
		CHINESE_S,
		FINNISH,
		SWEDISH,
		DANISH,
		NORWEGIAN,
		POLISH,
		PORTUGUESE_BR,
		ENGLISH_GB,
		TURKISH
	}

	public static extern SkuFlags skuFlags
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool commonDialogIsRunning
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool gcEnable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern int gcDisableMaxHeapSize
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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool SetMonoHeapBehaviours(bool constrain, bool tight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool SetInfoBarState(bool visible, bool white, bool translucent);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void PowerTick(PowerTickType tickType);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int MountContentInternal(int content);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int UnmountContentInternal(int content);

	public static int MountContent(MountableContent content)
	{
		return MountContentInternal((int)content);
	}

	public static int UnmountContent(MountableContent content)
	{
		return UnmountContentInternal((int)content);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void EnableHeapBlockSorting();
}
