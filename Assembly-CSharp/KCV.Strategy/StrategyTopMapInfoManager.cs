using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTopMapInfoManager : MonoBehaviour
	{
		private UISprite mSprite_Background;

		private UISprite[] mSprites_Item;

		private UILabel mLabel_OperationTitle;

		private UISprite mSprite_Arrow;

		private UISprite mSprite_ArrowGlow;

		private GameObject mGameObject_Button;

		private UISprite mSprite_ButtonBack;

		private UISprite mSprite_ButtonCircle;

		private UISprite mSprite_ButtonFront;

		private UISprite mSprite_ButtonText;

		private int iCnt;

		private bool locked;

		private float timer;

		[SerializeField]
		private TypewriterEffect TypeWriter;

		private void Awake()
		{
			mSprite_Background = ((Component)base.transform.Find("BG")).GetComponent<UISprite>();
			mSprites_Item = new UISprite[4];
			for (int i = 0; i < 4; i++)
			{
				mSprites_Item[i] = ((Component)base.transform.Find("BG/Item" + (i + 1))).GetComponent<UISprite>();
			}
			mLabel_OperationTitle = ((Component)base.transform.Find("OperationText")).GetComponent<UILabel>();
			mSprite_Arrow = ((Component)base.transform.Find("Arrow")).GetComponent<UISprite>();
			mSprite_ArrowGlow = ((Component)base.transform.Find("ArrowGlow")).GetComponent<UISprite>();
			mGameObject_Button = base.transform.Find("Btn").gameObject;
			mSprite_ButtonBack = ((Component)mGameObject_Button.transform.Find("Back")).GetComponent<UISprite>();
			mSprite_ButtonCircle = ((Component)mGameObject_Button.transform.Find("Circle")).GetComponent<UISprite>();
			mSprite_ButtonFront = ((Component)mGameObject_Button.transform.Find("Front")).GetComponent<UISprite>();
			mSprite_ButtonText = ((Component)mGameObject_Button.transform.Find("Text")).GetComponent<UISprite>();
			mSprite_Background.transform.localScale = new Vector3(0.0001f, 0.0001f, 1f);
			mSprite_Background.alpha = 0f;
			mSprites_Item[0].alpha = 0f;
			mSprites_Item[1].alpha = 0f;
			mSprites_Item[2].alpha = 0f;
			mSprites_Item[3].alpha = 0f;
			mLabel_OperationTitle.alpha = 0f;
			mSprite_Arrow.alpha = 0f;
			mSprite_ArrowGlow.alpha = 0f;
			mGameObject_Button.transform.localScale = new Vector3(0.0001f, 0.0001f, 1f);
			mSprite_ButtonBack.alpha = 0f;
			mSprite_ButtonCircle.alpha = 0f;
			mSprite_ButtonFront.alpha = 0f;
			mSprite_ButtonText.alpha = 0f;
			iCnt = 0;
			locked = true;
			timer = 0f;
			TypeWriter.enabled = false;
		}

		private void Update()
		{
			if (locked)
			{
				return;
			}
			if (mSprite_ButtonBack.alpha > 0f)
			{
				mSprite_ButtonCircle.transform.Rotate(-20f * Vector3.forward * Time.deltaTime);
			}
			if (mSprite_Arrow.alpha == 1f)
			{
				if (timer == 0f)
				{
					timer = Time.time - 4.712389f;
				}
				mSprite_ArrowGlow.alpha = 0.5f + 0.5f * Mathf.Sin(4f * (Time.time - timer));
			}
		}

		public void Die(bool btnPress = false)
		{
			iTween.Stop(base.gameObject, includechildren: true);
			TypeWriter.gameObject.SetActive(false);
			iTween.ScaleTo(mSprite_Background.gameObject, iTween.Hash("scale", new Vector3(0.0001f, 0.0001f, 1f), "time", 0.25f, "easetype", iTween.EaseType.easeInOutQuad));
			if (btnPress)
			{
				Vector3 localScale = mGameObject_Button.transform.localScale;
				if (localScale.x >= 0.8f)
				{
					iTween.ScaleTo(mGameObject_Button, iTween.Hash("scale", new Vector3(0.6f, 0.6f, 1f), "time", 0.1f, "easetype", iTween.EaseType.easeInQuad));
					iTween.ScaleTo(mGameObject_Button, iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", 0.3f, "delay", 0.1f, "easetype", iTween.EaseType.easeOutElastic));
					iTween.ValueTo(base.gameObject, iTween.Hash("from", mSprite_Background.alpha, "to", 0, "time", 0.2f, "delay", 0.2f, "onupdate", "FadePopups", "onupdatetarget", base.gameObject));
					Object.Destroy(base.gameObject, 0.45f);
					goto IL_0327;
				}
			}
			iTween.ScaleTo(mGameObject_Button, iTween.Hash("scale", new Vector3(0.0001f, 0.0001f, 1f), "time", 0.25f, "easetype", iTween.EaseType.easeInOutQuad));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", mSprite_Background.alpha, "to", 0, "time", 0.1f, "delay", 0.15f, "onupdate", "FadePopups", "onupdatetarget", base.gameObject));
			Object.Destroy(base.gameObject, 0.3f);
			goto IL_0327;
			IL_0327:
			mSprite_Arrow.gameObject.SetActive(false);
			mSprite_ArrowGlow.gameObject.SetActive(false);
			mLabel_OperationTitle.gameObject.SetActive(false);
		}

		public void FadePopups(float f)
		{
			mSprite_Background.alpha = f;
			mSprite_ButtonBack.alpha = f;
			mSprite_ButtonFront.alpha = f;
			if (!locked)
			{
				mSprite_ButtonCircle.alpha = f;
				mSprite_ButtonText.alpha = f;
			}
		}

		public void FadeInfo(float f)
		{
			mLabel_OperationTitle.alpha = f;
			for (int i = 0; i < iCnt; i++)
			{
				mSprites_Item[i].alpha = f;
			}
			mSprite_Arrow.alpha = f;
			if (mSprite_ArrowGlow.alpha > 0f)
			{
				mSprite_ArrowGlow.alpha /= 2f;
			}
		}

		public IEnumerator ShowPopups()
		{
			yield return new WaitForSeconds(0f);
			mSprite_Background.alpha = 1f;
			mSprite_ButtonBack.alpha = 1f;
			mSprite_ButtonFront.alpha = 1f;
			if (!locked)
			{
				mSprite_ButtonCircle.alpha = 1f;
				mSprite_ButtonText.alpha = 1f;
			}
		}

		public IEnumerator StartText(string det)
		{
			yield return new WaitForSeconds(0.5f);
			TweenAlpha.Begin(TypeWriter.gameObject, 0.2f, 1f);
			TypeWriter.enabled = true;
		}
	}
}
