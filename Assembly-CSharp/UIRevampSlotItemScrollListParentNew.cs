using KCV;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class UIRevampSlotItemScrollListParentNew : UIScrollList<SlotitemModel, UIRevampSlotItemScrollListChildNew>
{
	[SerializeField]
	private UIButton mOverlayBtn2;

	private KeyControl mKEyController;

	private Action mOnBackListener;

	private Action<UIRevampSlotItemScrollListChildNew> mOnSelectedSlotItemListener;

	public new void Initialize(SlotitemModel[] models)
	{
		base.Initialize(models);
		base.ChangeImmediateContentPosition(ContentDirection.Hell);
		HeadFocus();
	}

	internal void SetCamera(Camera cameraTouchEventCatch)
	{
		SetSwipeEventCamera(cameraTouchEventCatch);
	}

	internal KeyControl GetKeyController()
	{
		if (mKEyController == null)
		{
			mKEyController = new KeyControl();
		}
		return mKEyController;
	}

	public void SetOnBackListener(Action onBackListener)
	{
		mOnBackListener = onBackListener;
	}

	private void Back()
	{
		if (mOnBackListener != null)
		{
			mOnBackListener();
		}
	}

	public void SetOnSelectedSlotItemListener(Action<UIRevampSlotItemScrollListChildNew> onSelectedSlotItemListener)
	{
		mOnSelectedSlotItemListener = onSelectedSlotItemListener;
	}

	protected override void OnSelect(UIRevampSlotItemScrollListChildNew view)
	{
		if (mOnSelectedSlotItemListener != null)
		{
			mOnSelectedSlotItemListener(view);
		}
	}

	private void Update()
	{
		if (mKEyController != null && base.mState == ListState.Waiting)
		{
			if (mKEyController.IsUpDown())
			{
				PrevFocus();
			}
			else if (mKEyController.IsDownDown())
			{
				NextFocus();
			}
			else if (mKEyController.IsLeftDown())
			{
				PrevPageOrHeadFocus();
			}
			else if (mKEyController.IsRightDown())
			{
				NextPageOrTailFocus();
			}
			else if (mKEyController.IsMaruDown())
			{
				Select();
			}
			else if (mKEyController.IsBatuDown())
			{
				Back();
			}
		}
	}

	protected override void OnChangedFocusView(UIRevampSlotItemScrollListChildNew focusToView)
	{
		if (0 < mModels.Length && base.mState == ListState.Waiting && mCurrentFocusView != null)
		{
			int realIndex = mCurrentFocusView.GetRealIndex();
			CommonPopupDialog.Instance.StartPopup(realIndex + 1 + "/" + mModels.Length, 0, CommonPopupDialogMessage.PlayType.Long);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
	}

	internal new void StartControl()
	{
		base.StartControl();
	}

	public UIButton GetOverlayBtn2()
	{
		return mOverlayBtn2;
	}

	protected override void OnCallDestroy()
	{
		mOverlayBtn2 = null;
		mKEyController = null;
		mOnBackListener = null;
		mOnSelectedSlotItemListener = null;
	}
}
