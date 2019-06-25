using System;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
	public class LeanAudio : MonoBehaviour
	{
		public static float MIN_FREQEUNCY_PERIOD = 1E-05f;

		public static int PROCESSING_ITERATIONS_MAX = 50000;

		public static List<float> generatedWaveDistances;

		public static LeanAudioOptions options()
		{
			return new LeanAudioOptions();
		}

		public static AudioClip createAudio(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null)
		{
			if (options == null)
			{
				options = new LeanAudioOptions();
			}
			float[] wave = createAudioWave(volume, frequency, options);
			return createAudioFromWave(wave, options);
		}

		private static float[] createAudioWave(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options)
		{
            throw new NotImplementedException("‚È‚É‚±‚ê");
            // float time = volume.get_Item(volume.length - 1).time;

			List<float> list = new List<float>();
			generatedWaveDistances = new List<float>();
			float num = 0f;
			for (int i = 0; i < PROCESSING_ITERATIONS_MAX; i++)
			{
				float num2 = frequency.Evaluate(num);
				if (num2 < MIN_FREQEUNCY_PERIOD)
				{
					num2 = MIN_FREQEUNCY_PERIOD;
				}
				float num3 = volume.Evaluate(num + 0.5f * num2);
				if (options.vibrato != null)
				{
					for (int j = 0; j < options.vibrato.Length; j++)
					{
						float num4 = Mathf.Abs(Mathf.Sin(1.5708f + num * (1f / options.vibrato[j][0]) * (float)Math.PI));
						float num5 = 1f - options.vibrato[j][1];
						num4 = options.vibrato[j][1] + num5 * num4;
						num3 *= num4;
					}
				}

                throw new NotImplementedException("‚È‚É‚±‚ê");
                // if (num + 0.5f * num2 >= time)
				// {
			    //	break;
				//}

				generatedWaveDistances.Add(num2);
				num += num2;
				list.Add(num);
				list.Add((i % 2 != 0) ? num3 : (0f - num3));
				if (i >= PROCESSING_ITERATIONS_MAX - 1)
				{
					Debug.LogError("LeanAudio has reached it's processing cap. To avoid this error increase the number of iterations ex: LeanAudio.PROCESSING_ITERATIONS_MAX = " + PROCESSING_ITERATIONS_MAX * 2);
				}
			}
			float[] array = new float[list.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = list[k];
			}
			return array;
		}

		private static AudioClip createAudioFromWave(float[] wave, LeanAudioOptions options)
		{
			float num = wave[wave.Length - 2];
			float[] array = new float[(int)((float)options.frequencyRate * num)];
			int num2 = 0;
			float num3 = wave[num2];
			float num4 = 0f;
			float num11 = wave[num2];
			float num5 = wave[num2 + 1];
			for (int i = 0; i < array.Length; i++)
			{
				float num6 = (float)i / (float)options.frequencyRate;
				if (num6 > wave[num2])
				{
					num4 = wave[num2];
					num2 += 2;
					num3 = wave[num2] - wave[num2 - 2];
					num5 = wave[num2 + 1];
				}
				float num7 = num6 - num4;
				float num8 = num7 / num3;
				float num9 = Mathf.Sin(num8 * (float)Math.PI);
				num9 = (array[i] = num9 * num5);
			}
			int num10 = array.Length;
			AudioClip val = AudioClip.Create("Generated Audio", num10, 1, options.frequencyRate, false);
			val.SetData(array, 0);
			return val;
		}

		public static AudioClip generateAudioFromCurve(AnimationCurve curve, int frequencyRate = 44100)
		{
            float time = curve[(curve.length - 1)].time;
            float num = time;
			float[] array = new float[(int)((float)frequencyRate * num)];
			for (int i = 0; i < array.Length; i++)
			{
				float num2 = (float)i / (float)frequencyRate;
				array[i] = curve.Evaluate(num2);
			}
			int num3 = array.Length;
			AudioClip val = AudioClip.Create("Generated Audio", num3, 1, frequencyRate, false);
			val.SetData(array, 0);
			return val;
		}

		public static void playAudio(AudioClip audio, Vector3 pos, float volume, float pitch)
		{
			AudioSource val = playClipAt(audio, pos);
            val.minDistance = 1f;
            val.pitch = pitch;
			val.volume = volume;
		}

		public static AudioSource playClipAt(AudioClip clip, Vector3 pos)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.position = pos;
			AudioSource val = gameObject.AddComponent<AudioSource>();
			val.clip = clip;
			val.Play();
			UnityEngine.Object.Destroy(gameObject, clip.length);
			return val;
		}

		public static void printOutAudioClip(AudioClip audioClip, ref AnimationCurve curve, float scaleX = 1f)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			float[] array = new float[audioClip.samples * audioClip.channels];
			audioClip.GetData(array, 0);
			int i = 0;
			Keyframe[] array2 = new Keyframe[array.Length];
			for (; i < array.Length; i++)
			{
				array2[i] = new Keyframe((float)i * scaleX, array[i]);
			}
			curve = new AnimationCurve(array2);
		}
	}
}
