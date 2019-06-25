using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class AudioSource : Behaviour
	{
		public float volume
		{
			get;
			set;
		}

		public float pitch
		{
			get;
			set;
		}

		public float time
		{
			get;
			set;
		}

		public int timeSamples
		{
			get;
			set;
		}

		public AudioClip clip
		{
			get;
			set;
		}

		public AudioMixerGroup outputAudioMixerGroup
		{
			get;
			set;
		}

		public bool isPlaying
		{
			get;
		}

		public bool loop
		{
			get;
			set;
		}

		public bool ignoreListenerVolume
		{
			get;
			set;
		}

		public bool playOnAwake
		{
			get;
			set;
		}

		public bool ignoreListenerPause
		{
			get;
			set;
		}

		public AudioVelocityUpdateMode velocityUpdateMode
		{
			get;
			set;
		}

		public float panStereo
		{
			get;
			set;
		}

		public float spatialBlend
		{
			get;
			set;
		}

		public bool spatialize
		{
			get;
			set;
		}

		public float reverbZoneMix
		{
			get;
			set;
		}

		public bool bypassEffects
		{
			get;
			set;
		}

		public bool bypassListenerEffects
		{
			get;
			set;
		}

		public bool bypassReverbZones
		{
			get;
			set;
		}

		public float dopplerLevel
		{
			get;
			set;
		}

		public float spread
		{
			get;
			set;
		}

		public int priority
		{
			get;
			set;
		}

		public bool mute
		{
			get;
			set;
		}

		public float minDistance
		{
			get;
			set;
		}

		public float maxDistance
		{
			get;
			set;
		}

		public AudioRolloffMode rolloffMode
		{
			get;
			set;
		}

		[Obsolete("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public float minVolume
		{
			get;
			set;
		}

		[Obsolete("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public float maxVolume
		{
			get;
			set;
		}

		[Obsolete("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public float rolloffFactor
		{
			get;
			set;
		}

		public void Play([DefaultValue("0")] ulong delay) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void Play()
		{
			ulong delay = 0uL;
			Play(delay);
		}

		public void PlayDelayed(float delay) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void PlayScheduled(double time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetScheduledStartTime(double time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetScheduledEndTime(double time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Stop() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Pause()
		{
			INTERNAL_CALL_Pause(this);
		}

		private static void INTERNAL_CALL_Pause(AudioSource self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void UnPause()
		{
			INTERNAL_CALL_UnPause(this);
		}

		private static void INTERNAL_CALL_UnPause(AudioSource self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void PlayOneShot(AudioClip clip, [DefaultValue("1.0F")] float volumeScale) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void PlayOneShot(AudioClip clip)
		{
			float volumeScale = 1f;
			PlayOneShot(clip, volumeScale);
		}

		[ExcludeFromDocs]
		public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
		{
			float volume = 1f;
			PlayClipAtPoint(clip, position, volume);
		}

		public static void PlayClipAtPoint(AudioClip clip, Vector3 position, [DefaultValue("1.0F")] float volume)
		{
			GameObject gameObject = new GameObject("One shot audio");
			gameObject.transform.position = position;
			AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
			audioSource.clip = clip;
			audioSource.spatialBlend = 1f;
			audioSource.volume = volume;
			audioSource.Play();
			Object.Destroy(gameObject, clip.length * ((!(Time.timeScale < 0.01f)) ? Time.timeScale : 0.01f));
		}

		public void SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimationCurve GetCustomCurve(AudioSourceCurveType type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetOutputDataHelper(float[] samples, int channel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("GetOutputData return a float[] is deprecated, use GetOutputData passing a pre allocated array instead.")]
		public float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			GetOutputDataHelper(array, channel);
			return array;
		}

		public void GetOutputData(float[] samples, int channel)
		{
			GetOutputDataHelper(samples, channel);
		}

		private void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetSpectrumData passing a pre allocated array instead.")]
		public float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			GetSpectrumDataHelper(array, channel, window);
			return array;
		}

		public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			GetSpectrumDataHelper(samples, channel, window);
		}

		public bool SetSpatializerFloat(int index, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool GetSpatializerFloat(int index, out float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
