using KCV;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIInteriorFurniturePreviewWaiter : MonoBehaviour
{
	[SerializeField]
	private UIButton mButton_TouchBackArea;

	private KeyControl mKeyController;

	private Action mOnBackListener;

	private bool mIsWaiting;

	private void Awake()
	{
		if (mIsWaiting)
		{
			mButton_TouchBackArea.isEnabled = true;
		}
		else
		{
			mButton_TouchBackArea.isEnabled = false;
		}
	}

	public void SetOnBackListener(Action onBackListener)
	{
		mOnBackListener = onBackListener;
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void StartWait()
	{
		mIsWaiting = true;
		mButton_TouchBackArea.isEnabled = true;
	}

	public void ResumeWait()
	{
		mIsWaiting = true;
		mButton_TouchBackArea.isEnabled = true;
	}

	public void StopWait()
	{
		if (mIsWaiting)
		{
			mIsWaiting = false;
			mButton_TouchBackArea.isEnabled = false;
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchBack()
	{
		OnBack();
	}

	private void OnBack()
	{
		if (mIsWaiting)
		{
			mIsWaiting = false;
			mButton_TouchBackArea.isEnabled = false;
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
		}
	}

	private void Update()
	{
		if (mKeyController != null && mIsWaiting)
		{
			if (mKeyController.keyState[1].down)
			{
				OnBack();
			}
			else if (mKeyController.keyState[0].down)
			{
				OnBack();
			}
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref mButton_TouchBackArea);
		mKeyController = null;
		mOnBackListener = null;
		mIsWaiting = false;
	}
}
