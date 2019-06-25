using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class MarriageSparkle : MonoBehaviour
	{
		private const float SCALE_MAX = 0.8f;

		private const float SCALE_MIN = 0.6f;

		private bool on;

		private float delay;

		private float iSca;

		private Vector3 iPos;

		private bool bobbing;

		[SerializeField]
		private UISprite sprite;

		public void Awake()
		{
			on = false;
			delay = 0f;
			iSca = 1f;
			bobbing = false;
			sprite.alpha = 0f;
		}

		public void Update()
		{
			if (on && Time.time > delay)
			{
				sprite.alpha = Mathf.Max(0f, 0.25f + 0.5f * Mathf.Sin(3f * (Time.time - delay)));
				base.transform.localScale = iSca * (0.85f + 0.15f * Mathf.Sin(12f * Time.time - delay)) * Vector3.one;
				if (bobbing)
				{
					base.transform.localPosition = iPos + (-95f + 10f * (float)Math.Sin(Time.time * 1.2f)) * Vector3.up;
				}
			}
		}

		public void Initialize(bool bob)
		{
			bobbing = bob;
			if (bob)
			{
				iSca = UnityEngine.Random.value * 0.199999988f + 0.6f;
				iPos = 30f * UnityEngine.Random.onUnitSphere;
				iPos = new Vector3(iPos.x, iPos.y, 0f);
				delay = Time.time + UnityEngine.Random.value * 2f;
			}
			else
			{
				base.transform.localPosition = new Vector3(20f - 40f * UnityEngine.Random.value, -63f + 10f * UnityEngine.Random.value, 0f);
				iSca = UnityEngine.Random.value * 0.199999988f + 0.6f;
				delay = Time.time + UnityEngine.Random.value * 0.25f;
			}
			on = true;
		}
	}
}
