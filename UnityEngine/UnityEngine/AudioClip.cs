using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioClip : Object
	{
		public delegate void PCMReaderCallback(float[] data);

		public delegate void PCMSetPositionCallback(int position);

		public float length
		{
			get;
		}

		public int samples
		{
			get;
		}

		public int channels
		{
			get;
		}

		public int frequency
		{
			get;
		}

		[Obsolete("Use AudioClip.loadState instead to get more detailed information about the loading process.")]
		public bool isReadyToPlay
		{
			get;
		}

		public AudioClipLoadType loadType
		{
			get;
		}

		public bool preloadAudioData
		{
			get;
		}

		public AudioDataLoadState loadState
		{
			get;
		}

		public bool loadInBackground
		{
			get;
		}

		private event PCMReaderCallback m_PCMReaderCallback;

		private event PCMSetPositionCallback m_PCMSetPositionCallback;

		public bool LoadAudioData() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool UnloadAudioData() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool GetData(float[] data, int offsetSamples) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SetData(float[] data, int offsetSamples) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream)
		{
			return Create(name, lengthSamples, channels, frequency, stream);
		}

		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, PCMReaderCallback pcmreadercallback)
		{
			return Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
		}

		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, PCMReaderCallback pcmreadercallback, PCMSetPositionCallback pcmsetpositioncallback)
		{
			return Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, pcmsetpositioncallback);
		}

		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
		{
			return Create(name, lengthSamples, channels, frequency, stream, null, null);
		}

		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, PCMReaderCallback pcmreadercallback)
		{
			return Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
		}

		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, PCMReaderCallback pcmreadercallback, PCMSetPositionCallback pcmsetpositioncallback)
		{
			if (name == null)
			{
				throw new NullReferenceException();
			}
			if (lengthSamples <= 0)
			{
				throw new ArgumentException("Length of created clip must be larger than 0");
			}
			if (channels <= 0)
			{
				throw new ArgumentException("Number of channels in created clip must be greater than 0");
			}
			if (frequency <= 0)
			{
				throw new ArgumentException("Frequency in created clip must be greater than 0");
			}
			AudioClip audioClip = Construct_Internal();
			if (pcmreadercallback != null)
			{
				AudioClip audioClip2 = audioClip;
				audioClip2.m_PCMReaderCallback = (PCMReaderCallback)Delegate.Combine(audioClip2.m_PCMReaderCallback, pcmreadercallback);
			}
			if (pcmsetpositioncallback != null)
			{
				AudioClip audioClip3 = audioClip;
				audioClip3.m_PCMSetPositionCallback = (PCMSetPositionCallback)Delegate.Combine(audioClip3.m_PCMSetPositionCallback, pcmsetpositioncallback);
			}
			audioClip.Init_Internal(name, lengthSamples, channels, frequency, stream);
			return audioClip;
		}

		private void InvokePCMReaderCallback_Internal(float[] data)
		{
			if (this.m_PCMReaderCallback != null)
			{
				this.m_PCMReaderCallback(data);
			}
		}

		private void InvokePCMSetPositionCallback_Internal(int position)
		{
			if (this.m_PCMSetPositionCallback != null)
			{
				this.m_PCMSetPositionCallback(position);
			}
		}

		private static AudioClip Construct_Internal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Init_Internal(string name, int lengthSamples, int channels, int frequency, bool stream) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
