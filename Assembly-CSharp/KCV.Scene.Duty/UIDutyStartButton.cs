using KCV.Scene.Port;
using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Duty
{
	[RequireComponent(typeof(UIButtonManager))]
	public class UIDutyStartButton : MonoBehaviour
	{
		private UIButtonManager mUIButtonManager;

		[SerializeField]
		private UISprite mSpriteYousei;

		[SerializeField]
		private UIButton mButtonPositive;

		[SerializeField]
		private UIButton mButtonNegative;

		[SerializeField]
		private UITexture mTexture_Ohyodo;

		[SerializeField]
		private Texture[] mTextureOhyodo;

		private Vector3 mYouseiDefaultPosition;

		private Action mPositiveSelectedCallBack;

		private Action mNegativeSelectedCallBack;

		private Action mSelectedCallBack;

		private UIButton mFocusButton;

		private UIButton _uiOverlayButton;

		private bool isSelected;

		private Action mGoToHouseYouseiCallBack;

		private void Awake()
		{
			mUIButtonManager = GetComponent<UIButtonManager>();
		}

		private void Start()
		{
			mUIButtonManager.IndexChangeAct = delegate
			{
				if (mUIButtonManager.nowForcusButton.Equals(mButtonNegative))
				{
					ChangeFocus(mUIButtonManager.nowForcusButton, needSe: false);
				}
				else if (mUIButtonManager.nowForcusButton.Equals(mButtonPositive))
				{
					FocusPositive();
				}
			};
			_uiOverlayButton = GameObject.Find("OverlayBtn").GetComponent<UIButton>();
			EventDelegate.Add(_uiOverlayButton.onClick, OnClickNegative);
			mYouseiDefaultPosition = mSpriteYousei.transform.localPosition;
			FocusPositive();
		}

		public void ClickFocusButton()
		{
			if (!isSelected)
			{
				mFocusButton.SendMessage("OnClick");
				UIUtil.AnimationOnFocus(mFocusButton.transform, null);
			}
		}

		public void SetOnPositiveSelectedCallBack(Action action)
		{
			mPositiveSelectedCallBack = action;
		}

		public void SetOnNegativeSelectedCallBack(Action action)
		{
			mNegativeSelectedCallBack = action;
		}

		public void SetOnSelectedCallBack(Action action)
		{
			mSelectedCallBack = action;
		}

		public void FocusPositive()
		{
			if (mFocusButton == null || !mFocusButton.Equals(mButtonPositive))
			{
				ChangeFocus(mButtonPositive, needSe: true);
				Vector3 localPosition = mSpriteYousei.transform.localPosition;
				new Vector3(localPosition.x, localPosition.y + 40f, localPosition.z);
				mSpriteYousei.spriteName = "mini_06_c_01";
				Hashtable hashtable = new Hashtable();
				hashtable.Add("y", 45f);
				hashtable.Add("time", 0.3f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easetype", iTween.EaseType.easeOutQuint);
				iTween.MoveTo(mSpriteYousei.gameObject, hashtable);
			}
		}

		public void FocusNegative(bool seFlag)
		{
			if (mFocusButton == null)
			{
				ChangeFocus(mButtonNegative, seFlag);
			}
			else if (!mFocusButton.Equals(mButtonNegative))
			{
				ChangeFocus(mButtonNegative, seFlag);
			}
		}

		private void ChangeFocus(UIButton targetButton, bool needSe)
		{
			if (mFocusButton != null && mFocusButton.Equals(mButtonPositive) && targetButton != null && targetButton.Equals(mButtonNegative))
			{
				RemoveFocus();
			}
			if (mFocusButton != null)
			{
				mFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(mFocusButton.gameObject, value: false);
			}
			mFocusButton = targetButton;
			if (mFocusButton != null)
			{
				if (needSe)
				{
					PlaySE(SEFIleInfos.CommonCursolMove);
				}
				mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(mFocusButton.gameObject, value: true);
				UIUtil.AnimationOnFocus(mFocusButton.transform, null);
			}
		}

		private void PlaySE(SEFIleInfos seType)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				SoundUtils.PlaySE(seType);
			}
		}

		public void RemoveFocus()
		{
			GoToHouseYouse(null);
		}

		public void OnClickPositive()
		{
			if (!isSelected)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_028);
				ChangeFocus(mButtonPositive, needSe: false);
				isSelected = true;
				if (mSelectedCallBack != null)
				{
					mSelectedCallBack();
				}
				mTexture_Ohyodo.mainTexture = mTextureOhyodo[1];
				iTween.tweens.Clear();
				new Vector3(mYouseiDefaultPosition.x, mYouseiDefaultPosition.y + 40f, mYouseiDefaultPosition.z);
				Hashtable hashtable = new Hashtable();
				mSpriteYousei.spriteName = "mini_06_c_02";
				hashtable.Add("y", 80f);
				hashtable.Add("time", 0.3f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easetype", iTween.EaseType.easeInBack);
				hashtable.Add("oncomplete", "OnClickedGoToHouseYouse");
				hashtable.Add("oncompletetarget", base.gameObject);
				iTween.MoveTo(mSpriteYousei.gameObject, hashtable);
				mButtonNegative.isEnabled = false;
				mButtonPositive.isEnabled = false;
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void OnClickNegative()
		{
			if (!isSelected)
			{
				ChangeFocus(mButtonNegative, needSe: false);
				isSelected = true;
				if (mSelectedCallBack != null)
				{
					mSelectedCallBack();
				}
				if (mNegativeSelectedCallBack != null)
				{
					mNegativeSelectedCallBack();
				}
				mButtonNegative.isEnabled = false;
				mButtonPositive.isEnabled = false;
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			}
		}

		private void OnClickedGoToHouseYouse()
		{
			GoToHouseYouse(delegate
			{
				if (mPositiveSelectedCallBack != null)
				{
					mPositiveSelectedCallBack();
				}
			});
		}

		private void GoToHouseYouse(Action callBack)
		{
			mGoToHouseYouseiCallBack = callBack;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("y", mYouseiDefaultPosition.y);
			hashtable.Add("time", 0.3f);
			hashtable.Add("isLocal", true);
			hashtable.Add("easetype", iTween.EaseType.easeInBack);
			hashtable.Add("oncomplete", "OnClickAnimationFinished");
			hashtable.Add("oncompletetarget", base.gameObject);
			iTween.MoveTo(mSpriteYousei.gameObject, hashtable);
		}

		private void OnClickAnimationFinished()
		{
			if (mGoToHouseYouseiCallBack != null)
			{
				mGoToHouseYouseiCallBack();
			}
		}

		public void OnTocuh()
		{
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextureOhyodo);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSpriteYousei);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButtonPositive);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButtonNegative);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Ohyodo);
			UserInterfacePortManager.ReleaseUtils.Release(ref mFocusButton);
			UserInterfacePortManager.ReleaseUtils.Release(ref _uiOverlayButton);
			mPositiveSelectedCallBack = null;
			mNegativeSelectedCallBack = null;
			mSelectedCallBack = null;
			mUIButtonManager = null;
		}
	}
}
