using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipSparkle : MonoBehaviour
	{
		private const float S_VEL_MAX = 2f;

		private const float S_VEL_MIN = 0.8f;

		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.75f;

		private PortUpgradesModernizeShipManager manager;

		private UISprite sprite;

		private bool on;

		private float sVel;

		private float sMax;

		private void Awake()
		{
			sprite = GetComponent<UISprite>();
			if (sprite == null)
			{
				Debug.Log("UISprite.cs is not attached to .");
			}
			sprite.alpha = 0f;
			base.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
			on = false;
			sVel = 0f;
			sMax = 0f;
		}

		private void Update()
		{
			if (on)
			{
				base.transform.localScale += new Vector3(sVel * Time.deltaTime, sVel * Time.deltaTime, 0f);
				Vector3 localScale = base.transform.localScale;
				if (localScale.x >= sMax)
				{
					sVel = Mathf.Min(0f - sVel, sVel);
				}
				UISprite uISprite = sprite;
				Vector3 localScale2 = base.transform.localScale;
				uISprite.alpha = Mathf.Min(localScale2.x / sMax, 1f);
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

		public void Initialize()
		{
			on = true;
			sVel = (float)App.rand.NextDouble() * 1.2f + 0.8f;
			sMax = (float)App.rand.NextDouble() * 0.25f + 0.75f;
			UnityEngine.Object.Destroy(base.gameObject, 2f * sVel * sMax);
		}
	}
}
