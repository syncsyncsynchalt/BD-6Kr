using System;
using UnityEngine;

namespace KCV
{
	public class StrategyTopText : MonoBehaviour
	{
		private UILabel label;

		private UILabel labelDup;

		private UILabel labelFade;

		public string text;

		public float speed;

		public bool texting;

		public int pos;

		public float timer;

		public void Awake()
		{
			label = base.gameObject.GetComponent<UILabel>();
			if (label == null)
			{
				Debug.Log("Warning: script not attached");
			}
			try
			{
				labelDup = ((Component)base.gameObject.transform.parent.Find("OperationDetailsText2")).GetComponent<UILabel>();
			}
			catch (Exception)
			{
				Debug.Log("Warning: OperationDetailsText2 not found");
			}
			if (labelDup == null)
			{
				Debug.Log("Warning: script not attached");
			}
			try
			{
				labelFade = ((Component)base.gameObject.transform.parent.Find("OperationDetailsText3")).GetComponent<UILabel>();
			}
			catch (Exception)
			{
				Debug.Log("Warning: OperationDetailsText3 not found");
			}
			if (labelFade == null)
			{
				Debug.Log("Warning: script not attached");
			}
			text = string.Empty;
			speed = 0.02f;
			texting = false;
			pos = 0;
			timer = 0f;
			label.alpha = 0f;
			labelDup.alpha = 0f;
			labelFade.alpha = 0f;
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
			while (timer > speed)
			{
				label.text += text[pos];
				labelDup.text += text[pos];
				if (text[pos] != '\n')
				{
					timer -= speed;
				}
				pos++;
			}
			if (pos < text.Length - 1)
			{
				labelFade.text = label.text + text[pos];
			}
			labelFade.alpha = timer / speed;
		}

		public void Reset()
		{
			label.text = string.Empty;
			labelDup.text = string.Empty;
			labelFade.text = string.Empty;
			pos = 0;
			texting = false;
		}

		public void Text(string s)
		{
			if (s.Length != 0)
			{
				label.alpha = 1f;
				labelDup.alpha = 1f;
				labelFade.alpha = 0f;
				text = s.Replace("\\n", "\n");
				label.text = string.Empty;
				labelDup.text = string.Empty;
				labelFade.text = string.Empty + s[0];
				pos = 0;
				texting = true;
			}
		}

		public void Stop()
		{
			texting = false;
			labelFade.alpha = label.alpha;
			label.alpha = 0f;
			labelDup.alpha = 0f;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", labelFade.alpha, "to", 0, "time", 0.2f, "onupdate", "TextAlpha", "onupdatetarget", base.gameObject));
		}

		public void TextAlpha(float f)
		{
			labelFade.alpha = f;
		}
	}
}
