using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIPanel))]
	public class UIItemUseLimitOverConfirm : MonoBehaviour
	{
		private UIPanel mPanelThis;

		private KeyControl mKeyController;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private DialogAnimation mDialogAnimation;

		private UIButton mFocusButton;

		private Action mOnPositiveCallBack;

		private Action mOnNegativeCallBack;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0f;
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				ChangeFocus(mButton_Negative);
			}
			else if (mKeyController.keyState[10].down)
			{
				ChangeFocus(mButton_Positive);
			}
			else if (mKeyController.keyState[1].down)
			{
				if (mButton_Negative.Equals(mFocusButton))
				{
					Cancel();
				}
				else if (mButton_Positive.Equals(mFocusButton))
				{
					Use();
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				Cancel();
			}
		}

		public void Initialize()
		{
			ChangeFocus(mButton_Negative);
		}

		public void Show(Action onFinished)
		{
			mPanelThis.alpha = 1f;
			if (!mDialogAnimation.IsOpen)
			{
				mDialogAnimation.OpenAction = delegate
				{
					if (onFinished != null)
					{
						onFinished();
					}
				};
				mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, isOpen: true);
			}
		}

		public void Close(Action onFinished)
		{
			if (mDialogAnimation.IsOpen)
			{
				mDialogAnimation.CloseAction = delegate
				{
					if (onFinished != null)
					{
						onFinished();
					}
					mPanelThis.alpha = 0f;
				};
				mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, isOpen: false);
			}
		}

		public void SetOnPositiveCallBack(Action onBuyCallBack)
		{
			mOnPositiveCallBack = onBuyCallBack;
		}

		private void OnUse()
		{
			if (mOnPositiveCallBack != null)
			{
				mOnPositiveCallBack();
			}
		}

		public void SetOnNegativeCallBack(Action onBuyCancel)
		{
			mOnNegativeCallBack = onBuyCancel;
		}

		private void OnCancel()
		{
			if (mOnNegativeCallBack != null)
			{
				mOnNegativeCallBack();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void ChangeFocus(UIButton targetButton)
		{
			if (mFocusButton != null)
			{
				mFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			mFocusButton = targetButton;
			if (mFocusButton != null)
			{
				mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		public void OnClickPositive()
		{
			ChangeFocus(mButton_Positive);
			Use();
		}

		private void Cancel()
		{
			OnCancel();
		}

		private void Use()
		{
			OnUse();
		}

		public void OnClickNegative()
		{
			ChangeFocus(mButton_Negative);
			Cancel();
		}

		public void OnTouchOther()
		{
			Cancel();
		}

		public void Release()
		{
			ChangeFocus(null);
		}
	}
}
