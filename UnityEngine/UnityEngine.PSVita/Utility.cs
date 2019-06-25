using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita
{
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

		public static SkuFlags skuFlags
		{
			get;
		}

		public static bool commonDialogIsRunning
		{
			get;
		}

		public static bool gcEnable
		{
			get;
			set;
		}

		public static int gcDisableMaxHeapSize
		{
			get;
			set;
		}

		public static SystemLanguage systemLanguage
		{
			get;
		}

		public static bool SetMonoHeapBehaviours(bool constrain, bool tight) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool SetInfoBarState(bool visible, bool white, bool translucent) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void PowerTick(PowerTickType tickType) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int MountContentInternal(int content) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int UnmountContentInternal(int content) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int MountContent(MountableContent content)
		{
			return MountContentInternal((int)content);
		}

		public static int UnmountContent(MountableContent content)
		{
			return UnmountContentInternal((int)content);
		}

		public static void EnableHeapBlockSorting() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
