using UnityEngine;

namespace FCamera
{
	public class FadeSample : MonoBehaviour
	{
		public Texture2D texture;

		public Texture2D startMask;

		public Texture2D endMask;

		[Range(0f, 3f)]
		public float fadeinTime = 0.4f;

		[Range(0f, 3f)]
		public float fadeoutTime = 1.4f;

		private int nextScene;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				LoadLevel();
			}
		}

		private void LoadLevel()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.UpdateTexture(texture);
			SingletonMonoBehaviour<FadeCamera>.Instance.UpdateMaskTexture(startMask);
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(fadeinTime, delegate
			{
				Application.LoadLevel(nextScene);
				SingletonMonoBehaviour<FadeCamera>.Instance.UpdateMaskTexture(endMask);
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(fadeoutTime, null);
			});
		}
	}
}
