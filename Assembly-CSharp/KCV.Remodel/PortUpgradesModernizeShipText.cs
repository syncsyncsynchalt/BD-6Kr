using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipText : MonoBehaviour
	{
		private PortUpgradesModernizeShipManager manager;

		private UILabel label;

		private string text;

		private float speed;

		private float left;

		private bool texting;

		private int pos;

		private float timer;

		public void Awake()
		{
			try
			{
				manager = ((Component)base.transform.parent.parent).GetComponent<PortUpgradesModernizeShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesModernizeShipText.cs");
			}
			if (manager == null)
			{
				Debug.Log("PortUpgradesModernizeShipManager.cs is not attached to ../..");
			}
			label = GetComponent<UILabel>();
			if (label == null)
			{
				Debug.Log("UILabel.cs is not attached to .");
			}
			label.alpha = 0f;
			text = string.Empty;
			speed = 0f;
			Vector3 localPosition = base.transform.localPosition;
			left = localPosition.x;
			texting = false;
			pos = 0;
			timer = 0f;
		}

		public void Update()
		{
			if (!texting)
			{
				return;
			}
			if (pos >= text.Length)
			{
				texting = false;
				return;
			}
			timer += Time.deltaTime;
			if (timer > speed)
			{
				label.text += text[pos];
				Transform transform = base.transform;
				float x = left;
				Vector3 localPosition = base.transform.localPosition;
				float y = localPosition.y;
				Vector3 localPosition2 = base.transform.localPosition;
				transform.localPosition = new Vector3(x, y, localPosition2.z);
				pos++;
				timer -= speed;
			}
		}

		public void Initialize(string text, float speed, int width)
		{
			label.alpha = 1f;
			this.text = text;
			this.speed = speed;
			label.width = width;
		}

		public void Reset()
		{
			label.text = string.Empty;
			pos = 0;
			texting = false;
		}

		public void Text()
		{
			label.text = string.Empty;
			pos = 0;
			texting = true;
		}

		private void OnDestroy()
		{
			manager = null;
			label = null;
		}
	}
}
