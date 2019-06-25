using KCV;
using KCV.Utils;
using System;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
[RequireComponent(typeof(UIButtonManager))]
public class UIItemUseConfirm : MonoBehaviour
{
	private UIPanel mPanelThis;

	private UIButtonManager mButtonManager;

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
		mButtonManager = GetComponent<UIButtonManager>();
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
			ChangeFocus(mButton_Positive, playSE: true);
		}
		else if (mKeyController.keyState[10].down)
		{
			ChangeFocus(mButton_Negative, playSE: true);
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
		ChangeFocus(mButton_Positive, playSE: false);
		mButtonManager.IndexChangeAct = delegate
		{
			ChangeFocus(mButtonManager.nowForcusButton, playSE: false);
		};
	}

	public void Show(Action onFinished)
	{
		ChangeFocus(mButton_Positive, playSE: false);
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
		if (mKeyController != null)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
			if (mOnPositiveCallBack != null)
			{
				mOnPositiveCallBack();
			}
		}
	}

	public void SetOnNegativeCallBack(Action onBuyCancel)
	{
		mOnNegativeCallBack = onBuyCancel;
	}

	private void OnCancel()
	{
		if (mKeyController != null && mOnNegativeCallBack != null)
		{
			mOnNegativeCallBack();
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	private void ChangeFocus(UIButton targetButton, bool playSE)
	{
		if (mFocusButton != null)
		{
			if (mFocusButton.Equals(targetButton))
			{
				return;
			}
			mFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
		}
		mFocusButton = targetButton;
		if (mFocusButton != null)
		{
			if (playSE)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_018);
			}
			mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
		}
	}

	public void OnClickPositive()
	{
		ChangeFocus(mButton_Positive, playSE: false);
		Use();
	}

	private void Cancel()
	{
		if (mKeyController != null)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			OnCancel();
		}
	}

	private void Use()
	{
		OnUse();
	}

	public void OnClickNegative()
	{
		ChangeFocus(mButton_Negative, playSE: false);
		Cancel();
	}

	public void Release()
	{
		ChangeFocus(null, playSE: false);
	}

	public void OnTouchOther()
	{
		Cancel();
	}
}
