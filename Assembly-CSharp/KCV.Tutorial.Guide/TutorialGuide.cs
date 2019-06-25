using KCV.Strategy;
using UnityEngine;

namespace KCV.Tutorial.Guide
{
	public class TutorialGuide : MonoBehaviour
	{
		public int tutorialID;

		public UILabel[] Text;

		private Transform Number;

		private Transform TutorialTex;

		private UILabel Title;

		private UILabel MainText;

		private static readonly Color32 MainTextColor = new Color32(227, 227, 227, byte.MaxValue);

		private static readonly Color32 TitleTextColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static readonly Vector3 CatPos = new Vector3(45f, -4f, 0f);

		private void Start()
		{
			if (SingletonMonoBehaviour<PortObjectManager>.exist())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SetTutorialGuide(this);
			}
			tutorialID = -1;
			Transform transform = base.transform.FindChild("TutorialGuide");
			transform.FindChild("TutorialTex").localPosition = new Vector3(62f, 88f, 0f);
			Transform transform2 = transform.FindChild("TutorialNumber");
			if (transform2 != null)
			{
				transform2.SetActive(isActive: false);
			}
			Util.InstantiatePrefab("StrategyPrefab/StepTutorialYousei", transform.gameObject);
			Number = base.transform.FindChild("TutorialGuide/TutorialNumber");
			TutorialTex = base.transform.FindChild("TutorialGuide/TutorialTex");
			Transform x = base.transform.FindChild("TutorialGuide/Title");
			if (x != null)
			{
				Title = ((Component)base.transform.FindChild("TutorialGuide/Title")).GetComponent<UILabel>();
				Title.color = TitleTextColor;
				Title.transform.localPositionY(36f);
			}
			MainText = ((Component)base.transform.FindChild("TutorialGuide/Label")).GetComponent<UILabel>();
			MainText.color = MainTextColor;
			Number.SetActive(isActive: false);
			TutorialTex.transform.localPositionX(63f);
			TutorialTex.transform.localPositionY(88f);
			Transform transform3 = base.transform.FindChild("TutorialGuide/ArrowAchor/Arrow");
			UISprite uISprite = (!(transform3 != null)) ? null : ((Component)transform3).GetComponent<UISprite>();
			uISprite.MakePixelPerfect();
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = uISprite.transform;
			UISprite uISprite2 = gameObject.AddComponent<UISprite>();
			uISprite2.atlas = uISprite.atlas;
			uISprite2.spriteName = "tutorial_cat";
			uISprite2.MakePixelPerfect();
			uISprite2.transform.localPosition = CatPos;
		}

		public void Show()
		{
			if (SingletonMonoBehaviour<TutorialGuideManager>.exist() && SingletonMonoBehaviour<TutorialGuideManager>.Instance.model != null && tutorialID != -1 && SingletonMonoBehaviour<TutorialGuideManager>.Instance.model.GetStepTutorialFlg(tutorialID))
			{
				Object.Destroy(base.gameObject);
			}
			else if (!(StrategyTopTaskManager.Instance != null) || StrategyTopTaskManager.GetSailSelect().isRun)
			{
				TweenAlpha.Begin(base.gameObject, 0.5f, 1f);
			}
		}

		public TweenAlpha Hide()
		{
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, 0.2f, 0f);
			tweenAlpha.ResetToBeginning();
			return tweenAlpha;
		}

		public void HideAndDestroy()
		{
			Hide().SetOnFinished(delegate
			{
				Object.Destroy(base.gameObject);
			});
		}

		public void InitText()
		{
			for (int i = 1; i < Text.Length; i++)
			{
				Text[i].alpha = 0f;
			}
			Text[0].alpha = 1f;
		}

		private void OnDestroy()
		{
			Mem.DelAry(ref Text);
		}
	}
}
