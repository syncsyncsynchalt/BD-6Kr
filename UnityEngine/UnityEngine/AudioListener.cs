using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioListener : Behaviour
	{
		public static float volume
		{
			get;
			set;
		}

		public static bool pause
		{
			get;
			set;
		}

		public AudioVelocityUpdateMode velocityUpdateMode
		{
			get;
			set;
		}

		private static void GetOutputDataHelper(float[] samples, int channel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			GetOutputDataHelper(array, channel);
			return array;
		}

		public static void GetOutputData(float[] samples, int channel)
		{
			GetOutputDataHelper(samples, channel);
		}

		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			GetSpectrumDataHelper(array, channel, window);
			return array;
		}

		public static void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			GetSpectrumDataHelper(samples, channel, window);
		}
	}
}
