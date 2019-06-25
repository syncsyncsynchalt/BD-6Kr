using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class MarriageFeather : MonoBehaviour
	{
		private const float I_R_VEL_MAX = (float)Math.PI * 7f / 12f;

		private const float I_R_VEL_MIN = (float)Math.PI * 5f / 12f;

		private const float I_C_VEL_MAX = 0.15f;

		private const float I_C_VEL_MIN = 0.12f;

		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.6f;

		private const float ALIGN_STRENGTH_MAX = 1.5f;

		private const float ALIGN_STRENGTH_MIN = 0.8f;

		private const int TYPES = 3;

		private bool on;

		private float startTime;

		private float delay;

		private Vector3 vel;

		private float rot;

		private float rVel;

		private float cVel;

		private float sca;

		private float alStr;

		[SerializeField]
		private UISprite sprite;

		[SerializeField]
		private Transform bgTrans;

		private Vector3 lastPos;

		private readonly string[] NAMES = new string[3]
		{
			"Feather_1_hor",
			"Feather_2_hor",
			"Feather_3_hor"
		};

		public void Awake()
		{
			on = false;
			startTime = 0f;
			delay = 0f;
		}

		public void Update()
		{
			if (on && Time.time > delay)
			{
				Vector3 vector = base.transform.localPosition - bgTrans.localPosition - Vector3.up * 610f;
				Vector3 b = new Vector3(vector.x * Mathf.Cos(rVel) - vector.y * Mathf.Sin(rVel), vector.x * Mathf.Sin(rVel) + vector.y * Mathf.Cos(rVel), 0f);
				Vector3 a = (0f - Mathf.Pow(vector.magnitude * cVel, 1.4f)) * vector.normalized;
				vel = a + b;
				if (Time.time > startTime + 3.5f)
				{
					vel *= Mathf.Sqrt(Time.time - startTime - 2.5f);
				}
				float num = 180f / (float)Math.PI * Mathf.Atan2(vel.y, vel.x);
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				rot = num - localEulerAngles.z;
				if (rot > 360f)
				{
					rot -= 360f;
				}
				else if (rot < 0f)
				{
					rot += 360f;
				}
				base.transform.localPosition += vel * Time.deltaTime + (bgTrans.localPosition - lastPos);
				lastPos = bgTrans.localPosition;
				base.transform.Rotate(Vector3.forward, rot * Time.deltaTime * alStr);
				base.transform.localScale = Vector3.one * sca * Mathf.Pow(vector.magnitude / 600f, 0.65f);
			}
		}

		public void Initialize()
		{
			on = true;
			sprite.spriteName = NAMES[(int)(UnityEngine.Random.value * 3f)];
			sprite.MakePixelPerfect();
			startTime = Time.time;
			delay = Time.time + UnityEngine.Random.value * 4.5f;
			sca = UnityEngine.Random.value * 0.399999976f + 0.6f;
			base.transform.localScale = new Vector3(sca, sca, 1f);
			lastPos = bgTrans.localPosition;
			base.transform.localPosition = new Vector3(-600f, -400f - 500f * UnityEngine.Random.value, 0f) + lastPos;
			base.transform.Rotate(UnityEngine.Random.value * 359.99f * Vector3.forward);
			rVel = UnityEngine.Random.value * ((float)Math.PI / 6f) + (float)Math.PI * 5f / 12f;
			cVel = UnityEngine.Random.value * 0.0300000086f + 0.12f;
			alStr = UnityEngine.Random.value * 0.7f + 0.8f;
		}
	}
}
