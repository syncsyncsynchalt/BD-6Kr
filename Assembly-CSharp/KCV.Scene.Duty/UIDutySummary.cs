using KCV.View;
using local.models;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutySummary : BaseUISummary<DutyModel>
	{
		public enum SelectType
		{
			Action,
			CallDetail,
			Back,
			Hover
		}

		public delegate void UIDutySummaryAction(SelectType type, UIDutySummary summary);

		[SerializeField]
		private UISprite mSpriteType;

		[SerializeField]
		private UILabel mLabelTitle;

		[SerializeField]
		private UIDutyStatus mDutyStatus;

		[SerializeField]
		private UIButton mButtonAction;

		[SerializeField]
		private TweenScale IconAnim;

		private KeyControl mKeyController;

		private UIButton mFocusButton;

		private UIDutySummaryAction mDutySummaryActionCallBack;

		public void SetCallBackSummaryAction(UIDutySummaryAction callBack)
		{
			mDutySummaryActionCallBack = callBack;
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[1].down)
			{
				if (mFocusButton != null)
				{
					mButtonAction.SendMessage("OnClick");
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				CallBackAction(SelectType.Back);
			}
		}

		private void InitializeButtonColor(UIButton target)
		{
			target.hover = Util.CursolColor;
			target.defaultColor = Color.white;
			target.pressed = Color.white;
			target.disabledColor = Color.white;
		}

		public override void Initialize(int index, DutyModel model)
		{
			base.Initialize(index, model);
			InitializeButtonColor(mButtonAction);
			mLabelTitle.text = model.Title;
			mDutyStatus.Initialize(model);
			mSpriteType.spriteName = GetSpriteNameDutyType(model.Category);
		}

		private string GetSpriteNameDutyType(int category)
		{
			int num = 0;
			switch (category)
			{
			case 1:
				num = 6;
				break;
			case 2:
				num = 1;
				break;
			case 3:
				num = 2;
				break;
			case 4:
				num = 3;
				break;
			case 5:
				num = 4;
				break;
			case 6:
				num = 7;
				break;
			case 7:
				num = 5;
				break;
			}
			return $"duty_tag{num}";
		}

		public override void Hover()
		{
			base.Hover();
			mButtonAction.SafeGetTweenScale(base.gameObject.transform.localScale, new Vector3(1.04f, 1.04f, 1f), 0.1f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			mButtonAction.ResetDefaultColor();
			mButtonAction.SetState(UIButtonColor.State.Hover, immediate: true);
			mButtonAction.defaultColor = mButtonAction.hover;
			IconAnim.enabled = true;
			UISelectedObject.SelectedOneObjectBlink(mButtonAction.transform.FindChild("Background").gameObject, value: true);
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			mButtonAction.SafeGetTweenScale(new Vector3(1f, 1f, 1f), base.gameObject.transform.localScale, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			mButtonAction.ResetDefaultColor();
			mButtonAction.SetState(UIButtonColor.State.Normal, immediate: true);
			IconAnim.enabled = false;
			IconAnim.transform.localScale = Vector3.one;
			UISelectedObject.SelectedOneObjectBlink(mButtonAction.transform.FindChild("Background").gameObject, value: false);
		}

		public void OnClickAction()
		{
			CallBackAction(SelectType.Action);
		}

		public void OnClickCallDetail()
		{
			CallBackAction(SelectType.CallDetail);
		}

		private void CallBackAction(SelectType type)
		{
			if (mDutySummaryActionCallBack != null)
			{
				mDutySummaryActionCallBack(type, this);
			}
		}
	}
}
