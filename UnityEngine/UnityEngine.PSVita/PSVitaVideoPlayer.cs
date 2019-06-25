using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita
{
	public sealed class PSVitaVideoPlayer
	{
		public enum Looping
		{
			None,
			Continuous
		}

		public enum Mode
		{
			FullscreenVideo,
			RenderToTexture
		}

		public enum MovieEvent
		{
			STOP = 1,
			READY = 2,
			PLAY = 3,
			PAUSE = 4,
			BUFFERING = 5,
			TIMED_TEXT_DELIVERY = 0x10,
			WARNING_ID = 0x20,
			ENCRYPTION = 48
		}

		public enum TrickSpeeds
		{
			Normal = 100,
			FF_2X = 200,
			FF_4X = 400,
			FF_8X = 800,
			FF_16X = 1600,
			FF_MAX = 3200,
			RW_8X = -800,
			RW_16X = -1600,
			RW_MAX = -3200
		}

		public struct PlayParams
		{
			public Looping loopSetting;

			public Mode modeSetting;

			public string audioLanguage;

			public int audioStreamIndex;

			public string textLanguage;

			public int textStreamIndex;

			public float volume;
		}

		public static long subtitleTimeStamp
		{
			get;
		}

		public static string subtitleText
		{
			get;
		}

		public static long videoDuration
		{
			get;
		}

		public static long videoTime
		{
			get;
		}

		internal PSVitaVideoPlayer()
		{
		}

		public static bool PlayEx(string path, PlayParams vidParams) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool Play(string path, Looping loop, Mode fullscreenvideo) { throw new NotImplementedException("�Ȃɂ���"); }

		public static void SetVolume(float volume) { throw new NotImplementedException("�Ȃɂ���"); }

		public static void Stop() { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool Pause() { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool TransferMemToHeap() { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool TransferMemToMonoHeap() { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool Resume() { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool SetTrickSpeed(TrickSpeeds speed) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool JumpToTime(ulong jumpTimeMsec) { throw new NotImplementedException("�Ȃɂ���"); }

		public static ulong GetCurrentTime() { throw new NotImplementedException("�Ȃɂ���"); }

		public static ulong GetVideoLength() { throw new NotImplementedException("�Ȃɂ���"); }

		public static void Init(RenderTexture renderTexture) { throw new NotImplementedException("�Ȃɂ���"); }

		public static void Update() { throw new NotImplementedException("�Ȃɂ���"); }
	}
}
