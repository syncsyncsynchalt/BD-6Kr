using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesConvertShipSnowflake : MonoBehaviour
	{
		private const float I_VEL_MAX = 400f;

		private const float I_VEL_MIN = 150f;

		private const float I_R_VEL_MAX = 200f;

		private const float I_R_VEL_MIN = 100f;

		private const float GRAV = -500f;

		private const float SCALE_MAX = 0.75f;

		private const float SCALE_MIN = 0.02f;

		private const float S_VEL = 0.6f;

		private PortUpgradesConvertShipManager manager;

		private bool on;

		private Vector3 vel;

		private float rVel;

		private void Awake()
		{
			on = false;
			vel = Vector3.zero;
			rVel = 0f;
		}

		private void Update()
		{
			if (!on)
			{
				return;
			}
			base.transform.localPosition += vel * Time.deltaTime;
			Vector3 localPosition = base.transform.localPosition;
			if (!(localPosition.x > 580f))
			{
				Vector3 localPosition2 = base.transform.localPosition;
				if (!(localPosition2.x < -580f))
				{
					Vector3 localPosition3 = base.transform.localPosition;
					if (!(localPosition3.y < -372f))
					{
						base.transform.localScale += new Vector3(0.6f * Time.deltaTime, 0.6f * Time.deltaTime, 0f);
						vel += new Vector3(0f, -500f, 0f) * Time.deltaTime;
						base.transform.Rotate(rVel * Vector3.forward * Time.deltaTime);
						return;
					}
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void SetManagerReference()
		{
			try
			{
				manager = ((Component)base.transform.parent.parent).GetComponent<PortUpgradesConvertShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesConvertShipSnowflake.cs");
			}
			if (manager == null)
			{
				Debug.Log("PortUpgradesConvertShipManager.cs is not attached to ../..");
			}
		}

		public void Initialize()
		{
			on = true;
			float num = (float)App.rand.NextDouble() * 0.73f + 0.02f;
			base.transform.localScale = new Vector3(num, num, 1f);
			base.transform.localPosition = new Vector3(0f, 200f, 0f);
			base.transform.Rotate(App.rand.Next(360) * Vector3.forward);
			int num2 = App.rand.Next(360);
			num = (float)App.rand.NextDouble() * 250f + 150f;
			vel = num * new Vector3(Mathf.Cos(num2), Mathf.Sin(num2), 0f);
			rVel = (float)App.rand.NextDouble() * 100f + 100f;
		}
	}
}
