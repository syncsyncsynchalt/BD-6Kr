using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRevampSetting : MonoBehaviour
	{
		public enum ActionType
		{
			CancelRevamp,
			StartRevamp
		}

		public delegate void UIRevampSettingAction(ActionType actionType, UIRevampSetting calledObject);

		public delegate RevampValidationResult UIRevampSettingStateCheck(RevampRecipeDetailModel revampDetailModel);

		private const int LEVEL_MAX = 10;

		private UIRevampSettingAction mUIRevampSettingActionCallBack;

		private UIRevampSettingStateCheck mRevampSettingStateCheckDelegate;

		[SerializeField]
		private UISprite mSprite_RequireSlotItemState;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_Devkit;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private UILabel mLabel_RevampKit;

		[SerializeField]
		private UIButton mButton_Start;

		[SerializeField]
		private UIButton mButton_Cancel;

		[SerializeField]
		private UIButton mButton_Switch;

		[SerializeField]
		private UIRevampIcon mRevampIcon;

		[SerializeField]
		private UIYouseiSwitch mYousei_Switch;

		[SerializeField]
		private UISprite[] mSprites_Star;

		[SerializeField]
		private Vector3 mVector3_HidePosition;

		[SerializeField]
		private Vector3 mVector3_ShowPosition;

		private UIPanel mPanelThis;

		private UIButton[] mButtonsFocusable;

		private RevampRecipeDetailModel mRevampRecipeDetailModel;

		private UIButton mButtonFocus;

		private KeyControl mKeyController;

		private UIButton _uiOverlayButton;

		private UIYouseiSwitch.ActionType mSwitchState;

		public void SetOnRevampSettingActionCallBack(UIRevampSettingAction callBack)
		{
			mUIRevampSettingActionCallBack = callBack;
		}

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0.01f;
		}

		private void Start()
		{
			mPanelThis.alpha = 1f;
			mYousei_Switch.SetYouseiSwitchActionCallBack(UIYouseiSwitchActionCallBack);
			mYousei_Switch.Enabled = true;
			_uiOverlayButton = GameObject.Find("UIRevampSetting/OverlayBtn").GetComponent<UIButton>();
			EventDelegate.Add(_uiOverlayButton.onClick, _onClickOverlayButton);
		}

		private void _onClickOverlayButton()
		{
			OnCallBack(ActionType.CancelRevamp);
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[0].down)
			{
				mButton_Cancel.SendMessage("OnClick");
			}
			else if (mKeyController.keyState[14].down)
			{
				int num = Array.IndexOf(mButtonsFocusable, mButtonFocus);
				int num2 = num - 1;
				if (0 <= num2)
				{
					ChangeFocusButton(mButtonsFocusable[num2]);
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (mKeyController.keyState[10].down)
			{
				int num3 = Array.IndexOf(mButtonsFocusable, mButtonFocus);
				int num4 = num3 + 1;
				if (num4 < mButtonsFocusable.Length)
				{
					ChangeFocusButton(mButtonsFocusable[num4]);
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				mButtonFocus.SendMessage("OnClick");
			}
			else if (mKeyController.IsShikakuDown())
			{
				ChangeFocusButton(mButton_Switch);
				mYousei_Switch.ClickSwitch();
			}
		}

		public void Initialize(RevampRecipeDetailModel recipeDetail, UIRevampSettingStateCheck stateCheckDelegate, Camera prodCamera)
		{
			mSwitchState = UIYouseiSwitch.ActionType.OFF;
			mRevampSettingStateCheckDelegate = stateCheckDelegate;
			mRevampRecipeDetailModel = recipeDetail;
			mLabel_Name.text = recipeDetail.Slotitem.Name;
			if (0 < recipeDetail.RequiredSlotitemCount)
			{
				mSprite_RequireSlotItemState.spriteName = "txt_need_on";
			}
			else
			{
				mSprite_RequireSlotItemState.spriteName = "txt_need_off";
			}
			for (int i = 0; i < mRevampRecipeDetailModel.Slotitem.Level; i++)
			{
				mSprites_Star[i].spriteName = "icon_star";
			}
			mRevampIcon.Initialize(recipeDetail.Slotitem.MstId, recipeDetail.Slotitem.Level, prodCamera);
			UpdateRevampRecipeDetail(mRevampRecipeDetailModel);
			ChangeFocusButton(mButtonsFocusable[0]);
		}

		private void ChangeFocusButton(UIButton target)
		{
			if (mButtonFocus != null)
			{
				mButtonFocus.SetState(UIButtonColor.State.Normal, immediate: true);
				if (mButtonFocus.name == "Button_Switch")
				{
					UISelectedObject.SelectedOneObjectBlink(mButtonFocus, value: false);
				}
				else
				{
					UISelectedObject.SelectedOneButtonZoomUpDown(mButtonFocus, value: false);
				}
			}
			mButtonFocus = target;
			if (mButtonFocus != null)
			{
				mButtonFocus.SetState(UIButtonColor.State.Hover, immediate: true);
				if (mButtonFocus.name == "Button_Switch")
				{
					UISelectedObject.SelectedOneObjectBlink(mButtonFocus, value: true);
				}
				else
				{
					UISelectedObject.SelectedOneButtonZoomUpDown(mButtonFocus, value: true);
				}
			}
		}

		public bool IsDetermined()
		{
			switch (mSwitchState)
			{
			case UIYouseiSwitch.ActionType.ON:
				return true;
			case UIYouseiSwitch.ActionType.OFF:
				return false;
			default:
				return false;
			}
		}

		private void UIYouseiSwitchActionCallBack(UIYouseiSwitch.ActionType actionType)
		{
			switch (actionType)
			{
			case UIYouseiSwitch.ActionType.ON:
				mSwitchState = UIYouseiSwitch.ActionType.ON;
				mRevampRecipeDetailModel.Determined = true;
				break;
			case UIYouseiSwitch.ActionType.OFF:
				mSwitchState = UIYouseiSwitch.ActionType.OFF;
				mRevampRecipeDetailModel.Determined = false;
				break;
			}
			UpdateRevampRecipeDetail(mRevampRecipeDetailModel);
		}

		private void UpdateRevampRecipeDetail(RevampRecipeDetailModel recipeDetail)
		{
			RevampValidationResult revampValidationResult = mRevampSettingStateCheckDelegate(recipeDetail);
			List<UIButton> list = new List<UIButton>();
			if (revampValidationResult == RevampValidationResult.OK)
			{
				mButton_Start.SetState(UIButtonColor.State.Normal, immediate: true);
				mButton_Start.isEnabled = true;
				list.Add(mButton_Cancel);
				list.Add(mButton_Switch);
				list.Add(mButton_Start);
				mButtonsFocusable = list.ToArray();
				SoundUtils.PlaySE(SEFIleInfos.SE_005);
			}
			else
			{
				mButton_Start.SetState(UIButtonColor.State.Disabled, immediate: true);
				mButton_Start.isEnabled = false;
				list.Add(mButton_Cancel);
				list.Add(mButton_Switch);
				mButtonsFocusable = list.ToArray();
				SoundUtils.PlaySE(SEFIleInfos.SE_006);
			}
			mLabel_Fuel.text = recipeDetail.Fuel.ToString();
			mLabel_Steel.text = recipeDetail.Steel.ToString();
			mLabel_Devkit.text = recipeDetail.DevKit.ToString();
			mLabel_Ammo.text = recipeDetail.Ammo.ToString();
			mLabel_Bauxite.text = recipeDetail.Baux.ToString();
			mLabel_RevampKit.text = recipeDetail.RevKit.ToString();
		}

		private void OnCallBack(ActionType actionType)
		{
			if (mUIRevampSettingActionCallBack != null)
			{
				mUIRevampSettingActionCallBack(actionType, this);
			}
		}

		public void OnClickStartRevamp()
		{
			mKeyController = null;
			_uiOverlayButton.isEnabled = false;
			OnCallBack(ActionType.StartRevamp);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void OnClickCancelRevamp()
		{
			mKeyController = null;
			_uiOverlayButton.isEnabled = false;
			OnCallBack(ActionType.CancelRevamp);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		}

		public KeyControl GetKeyController()
		{
			mKeyController = new KeyControl();
			return mKeyController;
		}

		public void Show(Action shownCallBack)
		{
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.gameObject, 0.3f);
			tweenPosition.from = mVector3_HidePosition;
			tweenPosition.to = mVector3_ShowPosition;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				if (shownCallBack != null)
				{
					shownCallBack();
				}
			});
			tweenPosition.PlayForward();
		}

		public void Hide(Action hiddenCallBack)
		{
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.gameObject, 0.3f);
			tweenPosition.from = base.gameObject.transform.localPosition;
			tweenPosition.to = mVector3_HidePosition;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				if (hiddenCallBack != null)
				{
					hiddenCallBack();
				}
			});
		}

		public RevampRecipeDetailModel GetRevampRecipeDetailModel()
		{
			return mRevampRecipeDetailModel;
		}

		private void OnDestroy()
		{
			mUIRevampSettingActionCallBack = null;
			mRevampSettingStateCheckDelegate = null;
			mSprite_RequireSlotItemState = null;
			mLabel_Name = null;
			mLabel_Fuel = null;
			mLabel_Steel = null;
			mLabel_Devkit = null;
			mLabel_Ammo = null;
			mLabel_Bauxite = null;
			mLabel_RevampKit = null;
			mButton_Start = null;
			mButton_Cancel = null;
			mButton_Switch = null;
			mRevampIcon = null;
			mYousei_Switch = null;
			mSprites_Star = null;
			mPanelThis = null;
			mButtonsFocusable = null;
			mRevampRecipeDetailModel = null;
			mButtonFocus = null;
			mKeyController = null;
			_uiOverlayButton = null;
		}
	}
}
