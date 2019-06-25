using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRevampBalloon : MonoBehaviour
	{
		public class MessageBuilder
		{
			private string mMessage;

			public MessageBuilder(int defaultR, int defaultG, int defaultB)
			{
				mMessage = GenerateColorTag(defaultR, defaultG, defaultB);
			}

			public MessageBuilder AddMessage(string message)
			{
				return AddMessage(message, lineBreak: true);
			}

			public MessageBuilder AddMessage(string message, bool lineBreak)
			{
				mMessage += message;
				if (lineBreak)
				{
					mMessage += "\n";
				}
				return this;
			}

			public MessageBuilder AddMessage(string message, bool lineBreak, int r, int g, int b)
			{
				mMessage = mMessage + GenerateColorTag(r, g, b) + message + "[-]";
				if (lineBreak)
				{
					mMessage += "\n";
				}
				return this;
			}

			public MessageBuilder AddWait(int waitSecond)
			{
				mMessage += "[W]";
				return this;
			}

			public MessageBuilder AddLineBreak(int value)
			{
				for (int i = 0; i < value; i++)
				{
					mMessage += "\n";
				}
				return this;
			}

			private string GenerateColorTag(int defaultR, int defaultG, int defaultB)
			{
				return $"[{defaultR:X2}{defaultG:X2}{defaultB:X2}]";
			}

			public string Build()
			{
				return mMessage;
			}
		}

		private UIPanel mPanelThis;

		[SerializeField]
		private UILabel mLabel_Message;

		[SerializeField]
		private UISprite mSprite_Balloon;

		private Coroutine mAnimationCoroutine;

		[SerializeField]
		private Transform mTransform_TouchNextArea;

		public float alpha
		{
			set
			{
				if (mPanelThis != null)
				{
					mPanelThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
		}

		public MessageBuilder GetMessageBuilder()
		{
			return new MessageBuilder(0, 0, 0);
		}

		public void SayMessage(string message)
		{
			if (mAnimationCoroutine != null)
			{
				StopCoroutine(mAnimationCoroutine);
			}
			mAnimationCoroutine = StartCoroutine(SayMessageCoroutine(message, delegate
			{
				mAnimationCoroutine = null;
			}));
		}

		public KeyControl SayMessage(string message, Action keyActionCallBack)
		{
			KeyControl keyControl = new KeyControl();
			if (mAnimationCoroutine != null)
			{
				StopCoroutine(mAnimationCoroutine);
			}
			KeyControl keyController = keyControl;
			mAnimationCoroutine = StartCoroutine(SayMessageCoroutine(message, delegate
			{
				mAnimationCoroutine = null;
				StartCoroutine(WaitKey(keyController, KeyControl.KeyName.MARU, keyActionCallBack));
			}));
			return keyController;
		}

		public void OnTouchNextArea()
		{
			mTransform_TouchNextArea.SetActive(isActive: false);
		}

		private IEnumerator WaitKey(KeyControl keyController, KeyControl.KeyName waitKey, Action callBack)
		{
			mTransform_TouchNextArea.SetActive(isActive: true);
			keyController.ClearKeyAll();
			keyController.firstUpdate = true;
			while (keyController != null)
			{
				if (keyController.keyState[(int)waitKey].down || !mTransform_TouchNextArea.gameObject.activeSelf)
				{
					callBack?.Invoke();
					keyController = null;
				}
				yield return null;
			}
			mTransform_TouchNextArea.SetActive(isActive: false);
		}

		public bool IsAnimationNow()
		{
			return mAnimationCoroutine != null;
		}

		private IEnumerator SayMessageCoroutine(string message, Action finished)
		{
			int defaultHeight = 68;
			mLabel_Message.text = string.Empty;
			int lineBreakCount = (from c in message.ToList()
				where c.Equals('\n')
				select c).Count() + 1;
			mSprite_Balloon.SetDimensions(mSprite_Balloon.width, defaultHeight + lineBreakCount * 24);
			char[] readText = message.ToCharArray();
			for (int charIndex = 0; charIndex < readText.Length; charIndex++)
			{
				string cache2 = string.Empty;
				if (readText[charIndex] == '[')
				{
					for (; charIndex < readText.Length && readText[charIndex] != ']'; charIndex++)
					{
						cache2 += readText[charIndex];
					}
					cache2 += readText[charIndex];
					if (cache2.ToString().Equals("[W]"))
					{
						yield return new WaitForSeconds(1f);
					}
					else
					{
						mLabel_Message.text += cache2;
					}
				}
				else
				{
					mLabel_Message.text += readText[charIndex];
				}
				yield return new WaitForSeconds(0.05f);
			}
			finished?.Invoke();
		}

		private void OnDestroy()
		{
			mPanelThis = null;
			mLabel_Message = null;
			mSprite_Balloon = null;
			mAnimationCoroutine = null;
		}
	}
}
