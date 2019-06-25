using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipSpirit : MonoBehaviour
	{
		private const float VEL = 200f;

		private const float VEL2 = 20f;

		private const float VEL_JIT_MAX = 20f;

		private const float VEL_JIT_MIN = 10f;

		private const float R_VEL = 150f;

		private const float R_VEL2 = 100f;

		private const float S_VEL = 0.2f;

		private PortUpgradesModernizeShipManager manager;

		private UISprite[] elements;

		private bool on;

		private Vector3 vel;

		private Vector3[] velJit;

		private void Awake()
		{
			elements = new UISprite[4];
			try
			{
				elements[0] = ((Component)base.transform.Find("Red")).GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Red not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (elements[0] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Red");
			}
			try
			{
				elements[1] = ((Component)base.transform.Find("Blue")).GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Blue not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (elements[1] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Blue");
			}
			try
			{
				elements[2] = ((Component)base.transform.Find("Orange")).GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Orange not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (elements[2] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Orange");
			}
			try
			{
				elements[3] = ((Component)base.transform.Find("Yellow")).GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Yellow not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (elements[3] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Yellow");
			}
			on = false;
			vel = Vector3.zero;
			velJit = new Vector3[4];
		}

		private void Update()
		{
			if (!on)
			{
				return;
			}
			if (Vector3.Magnitude(base.transform.localPosition) <= 200f * Time.deltaTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			base.transform.RotateAround(base.transform.parent.TransformPoint(0f, 0f, 0f), Vector3.forward, 150f * Time.deltaTime);
			base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			base.transform.localPosition += 200f * Vector3.Normalize(-base.transform.localPosition) * Time.deltaTime;
			for (int i = 0; i < 4; i++)
			{
				Vector3 localPosition = elements[i].transform.localPosition;
				float y = localPosition.y;
				Vector3 localPosition2 = elements[i].transform.localPosition;
				float f = Mathf.Atan2(y, localPosition2.x);
				elements[i].transform.localPosition += new Vector3(Mathf.Sin(f), 0f - Mathf.Cos(f), 0f) * 100f * Time.deltaTime;
				elements[i].transform.localPosition += velJit[i] * Mathf.Sin(Time.time) * Time.deltaTime;
				elements[i].transform.localPosition += 20f * Vector3.Normalize(-elements[i].transform.localPosition) * Time.deltaTime;
				elements[i].transform.localScale -= new Vector3(0.2f * Time.deltaTime, 0.2f * Time.deltaTime, 0f);
				if (Vector3.Magnitude(base.transform.localPosition) < 200f)
				{
					elements[i].alpha -= Mathf.Min(Time.deltaTime, elements[i].alpha);
				}
			}
		}

		public void SetManagerReference()
		{
			try
			{
				manager = ((Component)base.transform.parent.parent).GetComponent<PortUpgradesModernizeShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesModernizeShipSparkle.cs");
			}
			if (manager == null)
			{
				Debug.Log("PortUpgradesModernizeShipManager.cs is not attached to ../..");
			}
		}

		public void Initialize(bool fire, bool torp, bool aaa, bool armor)
		{
			on = true;
			for (int i = 0; i < 4; i++)
			{
				float d = (float)App.rand.NextDouble() * 10f + 10f;
				int num = App.rand.Next(360);
				velJit[i] = d * new Vector3(Mathf.Cos(num), Mathf.Sin(num), 0f);
			}
			if (!fire)
			{
				elements[0].alpha = 0f;
			}
			if (!torp)
			{
				elements[1].alpha = 0f;
			}
			if (!aaa)
			{
				elements[2].alpha = 0f;
			}
			if (!armor)
			{
				elements[3].alpha = 0f;
			}
		}
	}
}
