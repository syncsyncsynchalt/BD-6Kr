using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class AudioSettings
{
	public delegate void AudioConfigurationChangeHandler(bool deviceWasChanged);

	public static extern AudioSpeakerMode driverCapabilities
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern AudioSpeakerMode speakerMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern double dspTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern int outputSampleRate
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static event AudioConfigurationChangeHandler OnAudioConfigurationChanged;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void GetDSPBufferSize(out int bufferLength, out int numBuffers);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("AudioSettings.SetDSPBufferSize is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.")]
	[WrapperlessIcall]
	public static extern void SetDSPBufferSize(int bufferLength, int numBuffers);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern AudioConfiguration GetConfiguration();

	public static bool Reset(AudioConfiguration config)
	{
		return INTERNAL_CALL_Reset(ref config);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool INTERNAL_CALL_Reset(ref AudioConfiguration config);

	internal static void InvokeOnAudioConfigurationChanged(bool deviceWasChanged)
	{
		if (AudioSettings.OnAudioConfigurationChanged != null)
		{
			AudioSettings.OnAudioConfigurationChanged(deviceWasChanged);
		}
	}
}
