using Common.Enum;
using KCV;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Linq;
using UnityEngine;

public class UIFurnitureStoreTabList : UIScrollList<FurnitureModel, UIFurnitureStoreTabListChild>
{
	[SerializeField]
	private Transform mTransform_NextButton;

	[SerializeField]
	private Transform mTransform_PrevButton;

	[SerializeField]
	private Transform mTransform_SoldOut;

	private FurnitureKinds mNowCategory = FurnitureKinds.Wall;

	private FurnitureStoreManager mFurnitureStoreManager;

	private KeyControl mKeyController;

	private Action mOnRequestChangeMode;

	private Action mOnBackListener;

	private Action<UIFurnitureStoreTabListChild> mOnSelectedFurnitureListener;

	protected override void OnAwake()
	{
		base.OnAwake();
		UIFurnitureStoreTabListChild[] mViews = base.mViews;
		foreach (UIFurnitureStoreTabListChild uIFurnitureStoreTabListChild in mViews)
		{
			uIFurnitureStoreTabListChild.SetOnReleaseRequestFurnitureTextureListener(ReleaseRequestFurnitureTextureFromChild);
		}
		mTransform_SoldOut.SetActive(isActive: false);
	}

	private void ReleaseRequestFurnitureTextureFromChild(Texture texture)
	{
		if (texture == null)
		{
			return;
		}
		int num = 0;
		UIFurnitureStoreTabListChild[] mViews = base.mViews;
		foreach (UIFurnitureStoreTabListChild uIFurnitureStoreTabListChild in mViews)
		{
			UITexture furnitureTextureView = uIFurnitureStoreTabListChild.GetFurnitureTextureView();
			if (furnitureTextureView != null && furnitureTextureView.mainTexture != null && furnitureTextureView.mainTexture.Equals(texture))
			{
				num++;
			}
		}
		if (num < 1)
		{
			Resources.UnloadAsset(texture);
		}
	}

	public void ChangeCategory(FurnitureKinds kinds)
	{
		if (mNowCategory != kinds)
		{
			KillScrollAnimation();
			mNowCategory = kinds;
			FurnitureModel[] models = mFurnitureStoreManager.GetStoreItem(kinds).Take(10).ToArray();
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			Refresh(models, firstPage: true);
			HeadFocus();
			StopFocusBlink();
		}
	}

	private void Update()
	{
		if (mKeyController != null && base.mState == ListState.Waiting)
		{
			if (mKeyController.IsUpDown())
			{
				PrevFocus();
			}
			else if (mKeyController.IsDownDown())
			{
				NextFocus();
			}
			else if (mKeyController.IsLeftDown())
			{
				PrevPageOrHeadFocus();
			}
			else if (mKeyController.IsRightDown())
			{
				NextPageOrTailFocus();
			}
			else if (mKeyController.IsBatuDown())
			{
				Back();
			}
			else if (mKeyController.IsMaruDown())
			{
				Select();
			}
		}
	}

	public void Refresh()
	{
		FurnitureModel[] array = mModels = mFurnitureStoreManager.GetStoreItem(mNowCategory).Take(10).ToArray();
		RefreshViews();
		if (mCurrentFocusView.GetModel() == null && mModels.Length != 0)
		{
			TailFocus();
		}
		else if (mCurrentFocusView.GetRealIndex() == mModels.Length - 1)
		{
			TailFocus();
		}
		if (mModels.Length == 0)
		{
			mTransform_NextButton.SetActive(isActive: false);
			mTransform_PrevButton.SetActive(isActive: false);
			mTransform_SoldOut.SetActive(isActive: true);
		}
		else
		{
			mTransform_NextButton.SetActive(isActive: false);
			mTransform_PrevButton.SetActive(isActive: false);
			mTransform_SoldOut.SetActive(isActive: false);
		}
	}

	protected override bool EqualsModel(FurnitureModel targetA, FurnitureModel targetB)
	{
		if (targetA == null)
		{
			return false;
		}
		if (targetB == null)
		{
			return false;
		}
		return targetA.MstId == targetB.MstId;
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void Initialize(FurnitureStoreManager manager)
	{
		mFurnitureStoreManager = manager;
		mNowCategory = FurnitureKinds.Wall;
		FurnitureModel[] models = mFurnitureStoreManager.GetStoreItem(mNowCategory).Take(10).ToArray();
		base.ChangeImmediateContentPosition(ContentDirection.Hell);
		Initialize(models);
		if (mModels.Length == 0)
		{
			mTransform_NextButton.SetActive(isActive: false);
			mTransform_PrevButton.SetActive(isActive: false);
			mTransform_SoldOut.SetActive(isActive: true);
			return;
		}
		if (3 < mModels.Length)
		{
			mTransform_NextButton.SetActive(isActive: true);
		}
		else
		{
			mTransform_NextButton.SetActive(isActive: false);
		}
		mTransform_PrevButton.SetActive(isActive: false);
		mTransform_SoldOut.SetActive(isActive: false);
	}

	protected override void OnChangedFocusView(UIFurnitureStoreTabListChild focusToView)
	{
		if (0 == focusToView.GetRealIndex())
		{
			mTransform_PrevButton.SetActive(isActive: false);
		}
		else
		{
			mTransform_PrevButton.SetActive(isActive: true);
		}
		if (mModels.Length == 0)
		{
			mTransform_NextButton.SetActive(isActive: false);
		}
		else if (mModels.Length - 1 <= mCurrentFocusView.GetRealIndex())
		{
			mTransform_NextButton.SetActive(isActive: false);
		}
		else
		{
			mTransform_NextButton.SetActive(isActive: true);
		}
		SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
	}

	protected override bool OnSelectable(UIFurnitureStoreTabListChild view)
	{
		bool result = false;
		if (view != null)
		{
			FurnitureModel model = view.GetModel();
			if (model != null && !model.IsPossession())
			{
				result = true;
			}
		}
		return result;
	}

	protected override void OnSelect(UIFurnitureStoreTabListChild view)
	{
		mKeyController.ClearKeyAll();
		mKeyController.firstUpdate = true;
		if (mOnSelectedFurnitureListener != null)
		{
			mOnSelectedFurnitureListener(view);
		}
	}

	public void SetOnRequestChangeMode(Action onRequestChangeMode)
	{
		mOnRequestChangeMode = onRequestChangeMode;
	}

	public void SetOnBackListener(Action onBackListener)
	{
		mOnBackListener = onBackListener;
	}

	public void SetOnSelectedFurnitureListener(Action<UIFurnitureStoreTabListChild> onSelectedFurnitureListener)
	{
		mOnSelectedFurnitureListener = onSelectedFurnitureListener;
	}

	private void Back()
	{
		StopFocusBlink();
		if (mOnBackListener != null)
		{
			mOnBackListener();
		}
	}

	private void OnRequestChangeMode()
	{
		if (mOnRequestChangeMode != null)
		{
			mOnRequestChangeMode();
		}
	}

	protected override void OnCallDestroy()
	{
		mFurnitureStoreManager = null;
		mKeyController = null;
		mOnRequestChangeMode = null;
	}

	internal new void StartControl()
	{
		HeadFocus();
		StartFocusBlink();
		base.StartControl();
	}

	internal new void LockControl()
	{
		base.LockControl();
	}

	internal void ResumeControl()
	{
		if (mCurrentFocusView == null)
		{
			HeadFocus();
		}
		StartFocusBlink();
		ResumeFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchPrev()
	{
		PrevFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchNext()
	{
		NextFocus();
	}

	internal new void SetSwipeEventCamera(Camera camera)
	{
		base.SetSwipeEventCamera(camera);
	}
}
