using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipLeaf : MonoBehaviour
	{
		private const float A_VEL = 1f;

		private const float DIST = 40f;

		private PortUpgradesModernizeShipManager manager;

		private GameObject dashInit;

		private GameObject[] dashes;

		private UISprite[] sprites;

		private bool on;

		private int pos;

		private float timer;

		private Vector3 prevPos;

		public void Awake()
		{
			manager = ((Component)base.transform.parent.parent).GetComponent<PortUpgradesModernizeShipManager>();
			dashInit = base.transform.parent.Find("DashInit").gameObject;
			dashes = new GameObject[20];
			sprites = new UISprite[20];
			on = false;
			pos = 19;
			timer = 0f;
			GetComponent<Animation>().Stop();
		}

		public void Update()
		{
			if (!on)
			{
				return;
			}
			if (dashes[pos] == null || Vector3.Distance(dashes[pos].transform.localPosition, base.transform.localPosition) > 40f)
			{
				pos = ++pos % 20;
				dashes[pos] = UnityEngine.Object.Instantiate(dashInit);
				sprites[pos] = dashes[pos].GetComponent<UISprite>();
				dashes[pos].transform.parent = base.transform.parent;
				dashes[pos].transform.localScale = new Vector3(1f, 1f, 1f);
				dashes[pos].transform.localPosition = base.transform.localPosition;
				Vector3 vector = base.transform.localPosition - prevPos;
				dashes[pos].transform.Rotate(Vector3.forward, 180f / (float)Math.PI * Mathf.Atan2(vector.y, vector.x));
				sprites[pos].alpha = 1f;
			}
			for (int i = 0; i < 20; i++)
			{
				if (dashes[i] != null)
				{
					sprites[i].alpha -= Mathf.Min(1f * Time.deltaTime, sprites[i].alpha);
					if (sprites[i].alpha < 1f * Time.deltaTime)
					{
						UnityEngine.Object.Destroy(dashes[i]);
					}
				}
			}
			prevPos = base.transform.localPosition;
			bool flag = true;
			for (int j = 0; j < 20; j++)
			{
				if (dashes[j] != null)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Vector3 localPosition = base.transform.localPosition;
				if (localPosition.x > 500f)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		public void Initialize()
		{
			on = true;
			GetComponent<Animation>().Play("fail_leaf");
		}

		private void OnDestroy()
		{
			manager = null;
			dashInit = null;
			if (dashes != null)
			{
				for (int i = 0; i < dashes.Length; i++)
				{
					dashes[i] = null;
				}
			}
			dashes = null;
			if (sprites != null)
			{
				for (int j = 0; j < sprites.Length; j++)
				{
					UserInterfacePortManager.ReleaseUtils.Release(ref sprites[j]);
				}
			}
			sprites = null;
		}
	}
}
