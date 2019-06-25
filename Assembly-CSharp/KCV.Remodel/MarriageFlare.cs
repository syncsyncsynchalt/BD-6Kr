using UnityEngine;

namespace KCV.Remodel
{
	public class MarriageFlare : MonoBehaviour
	{
		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.6f;

		private const float A_VEL_MAX = 2.5f;

		private const float A_VEL_MIN = 1.5f;

		private const int TYPES = 4;

		private bool on;

		private float delay;

		private float speed;

		[SerializeField]
		private UISprite sprite;

		private readonly string[] NAMES = new string[4]
		{
			"WhiteLight_2",
			"WhiteLight_3",
			"WhiteLight_4",
			"WhiteLight_7"
		};

		public void Awake()
		{
			on = false;
			delay = 0f;
			speed = 0f;
			sprite.alpha = 0f;
		}

		public void Update()
		{
		}

		public void Initialize()
		{
			on = true;
			Loop();
		}

		public void Loop()
		{
			sprite.spriteName = NAMES[(int)(Random.value * 4f)];
			sprite.MakePixelPerfect();
			delay = Random.value * 4f;
			speed = Random.value * 1f + 1.5f;
			float num = Random.value * 0.399999976f + 0.6f;
			base.transform.localScale = new Vector3(num, num, 1f);
			base.transform.localPosition = new Vector3(430f - 860f * Random.value, 222f - 444f * Random.value, 0f);
			base.transform.Rotate(Random.value * 359.99f * Vector3.forward);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", speed / 2f, "delay", delay, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", speed / 2f, "delay", delay + speed / 2f, "onupdate", "Alpha", "onupdatetarget", base.gameObject, "oncomplete", "Loop", "oncompletetarget", base.gameObject));
		}

		public void Alpha(float f)
		{
			sprite.alpha = f;
		}
	}
}
