using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Scene.Marriage
{
	[RequireComponent(typeof(UIButtonManager))]
	public class UIMarriageConfirm : MonoBehaviour
	{
		private UIButtonManager mButtonManager;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UILabel mLabel_FromRingValue;

		[SerializeField]
		private UILabel mLabel_ToRingValue;

		private KeyControl mKeyController;

		private Action mOnNegativeListener;

		private Action mOnPositiveListener;

		private UIButton mFocusButton;

		private void Awake()
		{
			mButtonManager = GetComponent<UIButtonManager>();
			mButtonManager.IndexChangeAct = delegate
			{
				ChangeFocus(mButtonManager.nowForcusButton);
			};
			mButton_Negative.OnEnableAndOnDisableChangeState = true;
			mButton_Positive.OnEnableAndOnDisableChangeState = true;
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				ChangeFocus(mButton_Positive);
			}
			else if (mKeyController.keyState[10].down)
			{
				ChangeFocus(mButton_Negative);
			}
			else if (mKeyController.keyState[1].down)
			{
				if (mFocusButton != null)
				{
					if (mFocusButton.Equals(mButton_Negative))
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
						ClickNegativeEvent();
					}
					else if (mFocusButton.Equals(mButton_Positive))
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
						ClickPositiveEvent();
					}
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				ClickNegativeEvent();
			}
			else if (mKeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (mKeyController.IsLDown())
			{
				ClickNegativeEvent();
			}
		}

		public void Initialize(int fromRingValue, int toRingValue)
		{
			ChangeFocus(mButton_Positive);
			mLabel_FromRingValue.text = fromRingValue.ToString();
			mLabel_ToRingValue.text = toRingValue.ToString();
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void SetOnNegativeListener(Action onNegativeListener)
		{
			mOnNegativeListener = onNegativeListener;
		}

		public void SetOnPositiveListener(Action onPositiveListener)
		{
			mOnPositiveListener = onPositiveListener;
		}

		[Obsolete("Inspector上で使用します")]
		public void TouchNegativeEvent()
		{
			if (mKeyController != null)
			{
				ClickNegativeEvent();
			}
		}

		[Obsolete("Inspector上で使用します")]
		public void TouchPositiveEvent()
		{
			if (mKeyController != null)
			{
				ClickPositiveEvent();
			}
		}

		private void ClickPositiveEvent()
		{
			if (mOnPositiveListener != null)
			{
				mOnPositiveListener();
			}
		}

		private void ClickNegativeEvent()
		{
			if (mOnNegativeListener != null)
			{
				mOnNegativeListener();
			}
		}

		private void ChangeFocus(UIButton target)
		{
			if (mFocusButton != null)
			{
				if (!mFocusButton.Equals(target))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				mFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			mFocusButton = target;
			if (mFocusButton != null)
			{
				mFocusButton.SetActive(isActive: false);
				mFocusButton.SetActive(isActive: true);
				mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		private void OnDestroy()
		{
			mButtonManager = null;
			mButton_Negative = null;
			mButton_Positive = null;
			mLabel_FromRingValue = null;
			mLabel_ToRingValue = null;
			mKeyController = null;
			mOnNegativeListener = null;
			mOnPositiveListener = null;
			mFocusButton = null;
		}
	}
}
