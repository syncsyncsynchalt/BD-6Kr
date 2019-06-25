using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	[RequireComponent(typeof(UIPanel))]
	public class OrganizeScrollListParent : UIScrollList<ShipModel, OrganizeScrollListChild>
	{
		public delegate void OnSelectCallBack(ShipModel ship);

		private UIPanel mPanelThis;

		[SerializeField]
		private TweenPosition TweenPos;

		[SerializeField]
		private UIShipSortButton SortButton;

		[SerializeField]
		private UIButtonMessage BackButton;

		private KeyControl mKeyController;

		private Action _OnCancelCallBack;

		private OnSelectCallBack _OnSelectCallBack;

		private bool _isFirst = true;

		private int _before_focus;

		public bool isOpen;

		private bool mFirstCalledInitialize;

		protected override void OnAwake()
		{
			mPanelThis = GetComponent<UIPanel>();
			if (TweenPos == null)
			{
				TweenPos = GetComponent<TweenPosition>();
			}
		}

		protected override void OnStart()
		{
		}

		public void Initialize(IOrganizeManager manager, Camera camera)
		{
			if (!mFirstCalledInitialize)
			{
				SortButton.SetSortKey(SortKey.UNORGANIZED);
				mFirstCalledInitialize = true;
			}
			_isFirst = true;
			_before_focus = 0;
			SetSwipeEventCamera(camera);
			OrganizeScrollListChild[] mViews = base.mViews;
			foreach (OrganizeScrollListChild organizeScrollListChild in mViews)
			{
				organizeScrollListChild.setManager(manager);
			}
			SortButton.InitializeForOrganize(manager.GetShipList());
			SortButton.SetClickable(clickable: true);
			SortButton.SetOnSortedShipsListener(OnSorted);
			SortButton.SetCheckClicableDelegate(CheckSortButtonClickable);
			SortButton.ReSort();
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void OnDestroy()
		{
			mKeyController = null;
			TweenPos = null;
			SortButton = null;
			BackButton = null;
		}

		public void SetBackButtonEnable(bool isEnable)
		{
			BackButton.GetComponent<BoxCollider2D>().enabled = isEnable;
		}

		private void OnSorted(ShipModel[] ships)
		{
			Refresh(ships, firstPage: true);
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
		}

		public void MovePosition(int x, bool isOpen = false, Action Onfinished = null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				base.gameObject.SetActive(true);
			}
			if (mPanelThis != null)
			{
				mPanelThis.widgetsAreStatic = true;
			}
			this.isOpen = isOpen;
			TweenPos.onFinished.Clear();
			TweenPos.from = TweenPos.transform.localPosition;
			TweenPosition tweenPos = TweenPos;
			float x2 = x;
			Vector3 localPosition = TweenPos.transform.localPosition;
			tweenPos.to = new Vector3(x2, localPosition.y, 0f);
			TweenPos.ResetToBeginning();
			TweenPos.PlayForward();
			TweenPos.ignoreTimeScale = true;
			if (Onfinished != null)
			{
				TweenPos.SetOnFinished(delegate
				{
					if (mPanelThis != null)
					{
						mPanelThis.widgetsAreStatic = false;
					}
					Onfinished();
				});
			}
		}

		protected override void OnUpdate()
		{
			if (mKeyController != null && base.mState == ListState.Waiting)
			{
				if (mKeyController.keyState[12].down)
				{
					NextFocus();
				}
				else if (mKeyController.keyState[8].down)
				{
					PrevFocus();
				}
				else if (mKeyController.keyState[14].down)
				{
					PrevPageOrHeadFocus();
				}
				else if (mKeyController.keyState[10].down)
				{
					NextPageOrTailFocus();
				}
				else if (mKeyController.keyState[1].down)
				{
					mKeyController.firstUpdate = true;
					mKeyController.ClearKeyAll();
					Select();
				}
				else if (mKeyController.keyState[2].down)
				{
					mCurrentFocusView.SwitchShipLockState();
				}
				else if (mKeyController.keyState[0].down)
				{
					OnCancel();
				}
				else if (mKeyController.keyState[3].down)
				{
					SortButton.OnClickSortButton();
				}
			}
		}

		public void OnCancel()
		{
			mKeyController = null;
			LockControl();
			_OnCancelCallBack();
		}

		protected override void OnChangedFocusView(OrganizeScrollListChild focusToView)
		{
			if (base.mState == ListState.Waiting && mModels != null && mModels.Length > 0)
			{
				if (!_isFirst && _before_focus != focusToView.GetRealIndex())
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				_isFirst = false;
				_before_focus = focusToView.GetRealIndex();
				CommonPopupDialog.Instance.StartPopup(focusToView.GetRealIndex() + 1 + "/" + mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			}
		}

		protected override void OnSelect(OrganizeScrollListChild view)
		{
			if (view.GetModel() != null && isOpen)
			{
				_OnSelectCallBack(view.GetModel());
				mKeyController = null;
				LockControl();
			}
		}

		protected override void OnCallDestroy()
		{
		}

		protected override bool OnSelectable(OrganizeScrollListChild view)
		{
			return true;
		}

		public void SetOnSelect(OnSelectCallBack onSelect)
		{
			_OnSelectCallBack = onSelect;
		}

		public void SetOnCancel(Action onCancel)
		{
			_OnCancelCallBack = onCancel;
		}

		public void ChangeLockBtnState()
		{
			if (mCurrentFocusView != null)
			{
				mCurrentFocusView.SwitchShipLockState();
			}
		}

		private bool CheckSortButtonClickable()
		{
			return base.mState == ListState.Waiting;
		}

		public ShipModel GetFocusModel()
		{
			return mCurrentFocusView.GetModel();
		}

		public void SetBackButton(GameObject target, string FunctionName)
		{
			BackButton.target = target;
			BackButton.functionName = FunctionName;
		}

		internal void ResumeControl()
		{
			base.StartControl();
		}

		internal new void StartControl()
		{
			base.StartControl();
		}

		internal new void RefreshViews()
		{
			if (SortButton.CurrentSortKey == SortKey.UNORGANIZED)
			{
				ShipModel[] array = mModels = SortButton.SortModels(SortKey.UNORGANIZED);
			}
			base.RefreshViews();
		}
	}
}
