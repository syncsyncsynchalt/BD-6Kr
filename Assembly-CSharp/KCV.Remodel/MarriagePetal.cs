using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class MarriagePetal : MonoBehaviour
	{
		private const float I_VEL_MAX = 300f;

		private const float I_VEL_MIN = 200f;

		private const float I_R_VEL_MAX = 70f;

		private const float I_R_VEL_MIN = 50f;

		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.8f;

		private const int TYPES = 2;

		private bool on;

		private float delay;

		private bool loop;

		[SerializeField]
		private UISprite sprite;

		private readonly string[] NAMES = new string[2]
		{
			"Petal_1",
			"Petal_2"
		};

		private Vector3 vel;

		private float rVel;

		private Vector3 rPivot;

		public void Awake()
		{
			on = false;
			delay = 0f;
			loop = false;
		}

		public void Update()
		{
			if (!on || !(Time.time > delay))
			{
				return;
			}
			base.transform.localPosition += (vel + 0.35f * Mathf.Sin(2f * Time.time) * Vector3.Magnitude(vel) * Vector3.left) * Time.deltaTime;
			base.transform.Rotate(rPivot, rVel * Time.deltaTime * Mathf.Sin((float)Math.PI * 2f * Time.time));
			Vector3 localPosition = base.transform.localPosition;
			if (!(localPosition.x < -550f))
			{
				Vector3 localPosition2 = base.transform.localPosition;
				if (!(localPosition2.y < -350f))
				{
					return;
				}
			}
			if (!loop)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				base.transform.localPosition = new Vector3(500f, 50f + 300f * UnityEngine.Random.value, 0f);
			}
		}

		public void Initialize(bool loop)
		{
			on = true;
			sprite.spriteName = NAMES[(int)(UnityEngine.Random.value * 2f)];
			sprite.MakePixelPerfect();
			if (loop)
			{
				delay = Time.time + UnityEngine.Random.value * 4f;
			}
			else
			{
				delay = Time.time + UnityEngine.Random.value * 2f;
			}
			this.loop = loop;
			float num = UnityEngine.Random.value * 0.199999988f + 0.8f;
			base.transform.localScale = new Vector3(num, num, 1f);
			if (loop)
			{
				base.transform.localPosition = new Vector3(500f, 50f + 300f * UnityEngine.Random.value, 0f);
			}
			else
			{
				base.transform.localPosition = new Vector3(-100f + 300f * UnityEngine.Random.value, 300f, 0f);
			}
			base.transform.Rotate(UnityEngine.Random.value * 359.99f * Vector3.forward);
			num = UnityEngine.Random.value * 100f + 200f;
			float f = (!loop) ? ((25f + UnityEngine.Random.value) * (float)Math.PI / 18f) : ((21f + UnityEngine.Random.value) * (float)Math.PI / 18f);
			vel = num * new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
			rVel = UnityEngine.Random.value * 20f + 50f;
			rPivot = UnityEngine.Random.onUnitSphere;
		}
	}
}
