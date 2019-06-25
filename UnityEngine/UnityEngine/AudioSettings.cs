using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioSettings
	{
		public delegate void AudioConfigurationChangeHandler(bool deviceWasChanged);

		public static AudioSpeakerMode driverCapabilities
		{
			get;
		}

		public static AudioSpeakerMode speakerMode
		{
			get;
			set;
		}

		public static double dspTime
		{
			get;
		}

		public static int outputSampleRate
		{
			get;
			set;
		}

		public static event AudioConfigurationChangeHandler OnAudioConfigurationChanged;

		public static void GetDSPBufferSize(out int bufferLength, out int numBuffers) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("AudioSettings.SetDSPBufferSize is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.")]
		public static void SetDSPBufferSize(int bufferLength, int numBuffers) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static AudioConfiguration GetConfiguration() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool Reset(AudioConfiguration config)
		{
			return INTERNAL_CALL_Reset(ref config);
		}

		private static bool INTERNAL_CALL_Reset(ref AudioConfiguration config) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void InvokeOnAudioConfigurationChanged(bool deviceWasChanged)
		{
			if (AudioSettings.OnAudioConfigurationChanged != null)
			{
				AudioSettings.OnAudioConfigurationChanged(deviceWasChanged);
			}
		}
	}
}
