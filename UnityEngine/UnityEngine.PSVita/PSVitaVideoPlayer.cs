using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita;

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
		TIMED_TEXT_DELIVERY = 16,
		WARNING_ID = 32,
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

	public static extern long subtitleTimeStamp
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern string subtitleText
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern long videoDuration
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern long videoTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal PSVitaVideoPlayer()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool PlayEx(string path, PlayParams vidParams);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool Play(string path, Looping loop, Mode fullscreenvideo);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetVolume(float volume);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void Stop();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool Pause();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool TransferMemToHeap();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool TransferMemToMonoHeap();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool Resume();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool SetTrickSpeed(TrickSpeeds speed);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool JumpToTime(ulong jumpTimeMsec);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern ulong GetCurrentTime();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern ulong GetVideoLength();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void Init(RenderTexture renderTexture);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void Update();
}
