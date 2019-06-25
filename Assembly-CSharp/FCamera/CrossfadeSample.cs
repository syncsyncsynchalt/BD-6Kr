using System;
using System.Collections;
using UnityEngine;

namespace FCamera
{
	public class CrossfadeSample : MonoBehaviour
	{
		private Texture2D crossfadeTexture;

		public Texture maskTexture;

		public float fadeoutTime = 1.4f;

		private int nextScene;

		private void Start()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.UpdateMaskTexture(maskTexture);
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				LoadLevel(nextScene);
			}
		}

		private void LoadLevel(int nextLevel)
		{
			StartCoroutine(CaptureScreen(delegate
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.UpdateTexture(crossfadeTexture);
				Application.LoadLevel(nextLevel);
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(fadeoutTime, delegate
				{
					UnityEngine.Object.Destroy(crossfadeTexture);
				});
			}));
		}

		private IEnumerator CaptureScreen(Action action)
		{
			yield return new WaitForEndOfFrame();
			crossfadeTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, mipmap: false);
			crossfadeTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
			crossfadeTexture.Apply();
			action();
		}
	}
}
