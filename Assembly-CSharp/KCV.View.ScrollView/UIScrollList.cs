using DG.Tweening;
using KCV.Display;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.View.ScrollView
{
	public abstract class UIScrollList<Model, View> : MonoBehaviour where Model : class where View : MonoBehaviour, UIScrollListItem<Model, View>
	{
		public enum ScrollDirection
		{
			Heaven,
			Hell
		}

		public enum ListState
		{
			Default,
			MoveFromKey,
			MoveFromFinger,
			MoveFromFingerScrolling,
			Waiting,
			Lock
		}

		private enum ViewModelValueState
		{
			EvenUserViewAndModel,
			EvenWorkSpaceViewAndModel,
			BiggerUserView,
			BiggerWorkSpaceView,
			BiggerModel,
			Default
		}

		public enum ContentDirection
		{
			Heaven,
			Hell
		}

		private enum AnimationType
		{
			ListMoveFromFingerScroll,
			RepositionForScroll,
			ContainerMove
		}

		[SerializeField]
		private float mBottomUpContentPosition;

		[SerializeField]
		private float mBottomDownContentPosition;

		[SerializeField]
		protected int mUserViewCount;

		[SerializeField]
		protected Transform mTransform_ContentPosition;

		private UIPanel mPanelContainer;

		[SerializeField]
		protected View[] mViews;

		protected View[] mViews_WorkSpace;

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		[SerializeField]
		private Ease mListMoveEaseType = Ease.Linear;

		[SerializeField]
		private float MOVE_ITEM_LEVEL = 10f;

		public float FRAME_MOVE_SECOND = 0.8f;

		private ContentDirection mContentDirection = ContentDirection.Hell;

		private float mOuterHead;

		private float mOuterTail;

		private Vector3 mLastTouchedPosition;

		protected View mCurrentFocusView;

		protected float mScrollCheckLevel = 300f;

		protected Model[] mModels;

		private Vector3[] mViewDefaultPositions;

		protected ListState mState
		{
			get;
			private set;
		}

		protected ListState GetListState()
		{
			return mState;
		}

		protected void LockControl()
		{
			ChangeState(ListState.Lock);
		}

		protected void StartControl()
		{
			ChangeState(ListState.Waiting);
		}

		protected virtual void OnChangedFocusView(View focusToView)
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnCallDestroy()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual bool OnSelectable(View view)
		{
			return true;
		}

		protected virtual void OnSelect(View view)
		{
		}

		private void Awake()
		{
			mPanelContainer = ((Component)mTransform_ContentPosition.parent).GetComponent<UIPanel>();
			MemoryViewRangePositions();
			MemoryViewsDefaultPosition();
			SettingViewListeners();
			mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(OnDisplayEvents, needFlickCheck: true);
			OnAwake();
		}

		public void StartStaticWidgetChildren()
		{
			if (mPanelContainer == null)
			{
				mPanelContainer = ((Component)mTransform_ContentPosition.parent).GetComponent<UIPanel>();
			}
			if (mPanelContainer != null)
			{
				mPanelContainer.widgetsAreStatic = true;
			}
		}

		public void StopStaticWidgetChildren()
		{
			if (mPanelContainer == null)
			{
				mPanelContainer = ((Component)mTransform_ContentPosition.parent).GetComponent<UIPanel>();
			}
			if (mPanelContainer != null)
			{
				mPanelContainer.widgetsAreStatic = false;
			}
		}

		private void Start()
		{
			OnStart();
		}

		private void OnDestroy()
		{
			OnCallDestroy();
			KillAnimationContainerMove();
			KillAnimationListMoveFromFingerScroll();
			KillAnimationAndForceCompleteRepositionForScroll();
			ReleaseMemberViews();
			ReleaseWorksSpaceViews();
			mTransform_ContentPosition = null;
			mUIDisplaySwipeEventRegion = null;
			mCurrentFocusView = (View)null;
			mModels = null;
			mPanelContainer = null;
		}

		private void Update()
		{
			OnUpdate();
		}

		public void HeadFocus()
		{
			switch (mContentDirection)
			{
			case ContentDirection.Hell:
			{
				float y = mViewDefaultPositions[0].y;
				Vector3 localPosition = mViews_WorkSpace[0].GetTransform().localPosition;
				if (y < localPosition.y)
				{
					ChangeFocusView(mViews_WorkSpace[1]);
				}
				else
				{
					ChangeFocusView(mViews_WorkSpace[0]);
				}
				break;
			}
			case ContentDirection.Heaven:
				ChangeFocusView(mViews_WorkSpace[1]);
				break;
			}
		}

		public void RemoveFocus()
		{
			ChangeFocusView((View)null);
		}

		public void ChangeHeadPage()
		{
			View[] array = new View[mViews.Length];
			for (int i = 0; i < mViews.Length; i++)
			{
				View val = mViews[i];
				if (i < mModels.Length)
				{
					val.Initialize(i, mModels[i]);
				}
				else
				{
					val.InitializeDefault(i);
				}
				val.GetTransform().localPosition = mViewDefaultPositions[i];
				array[i] = val;
			}
			mViews_WorkSpace = array;
		}

		public void ChangePageFromModel(Model model)
		{
			if (mModels == null || mModels.Length == 0)
			{
				ChangeContentPosition(ContentDirection.Hell);
				return;
			}
			if (mModels.Length - 1 == ((!((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null)) ? (-1) : mCurrentFocusView.GetRealIndex()))
			{
				ChangeContentPosition(ContentDirection.Heaven);
			}
			else
			{
				ChangeContentPosition(ContentDirection.Hell);
			}
			int modelIndex = GetModelIndex(model);
			if (-1 < modelIndex)
			{
				if (modelIndex + mUserViewCount < mModels.Length)
				{
					View[] array = new View[mViews.Length];
					for (int i = 0; i < mViews.Length; i++)
					{
						View val = mViews[i];
						int num = modelIndex + i;
						if (num < mModels.Length)
						{
							val.Initialize(num, mModels[num]);
						}
						else
						{
							val.InitializeDefault(num);
						}
						val.GetTransform().localPosition = mViewDefaultPositions[i];
						array[i] = val;
					}
					mViews_WorkSpace = array;
					View[] array2 = mViews;
					for (int j = 0; j < array2.Length; j++)
					{
						View view = array2[j];
						if (EqualsModel(model, view.GetModel()))
						{
							ChangeFocusView(view);
						}
					}
					return;
				}
				ChangeTailPage();
				View[] array3 = mViews;
				for (int k = 0; k < array3.Length; k++)
				{
					View view2 = array3[k];
					if (EqualsModel(model, view2.GetModel()))
					{
						ChangeFocusView(view2);
					}
				}
			}
			else
			{
				ChangeHeadPage();
			}
		}

		protected virtual bool EqualsModel(Model targetA, Model targetB)
		{
			if (targetA == null)
			{
				return false;
			}
			if (targetB == null)
			{
				return false;
			}
			return targetA.Equals(targetB);
		}

		protected virtual int GetModelIndex(Model model)
		{
			return Array.IndexOf(mModels, model);
		}

		public void TailFocusForOnFinishedScroll()
		{
			ListState mState = this.mState;
			ChangeState(ListState.Waiting);
			int num = mUserViewCount;
			int num2 = mModels.Length;
			if (num == num2)
			{
				ChangeHeadPage();
				ChangeFocusView(mViews_WorkSpace[mModels.Length - 1]);
				if (mCurrentFocusView.GetRealIndex() == mUserViewCount - 1)
				{
					ChangeImmediateContentPosition(ContentDirection.Heaven);
				}
				else
				{
					ChangeImmediateContentPosition(ContentDirection.Hell);
				}
			}
			else if (num < num2)
			{
				ChangeImmediateContentPosition(ContentDirection.Heaven);
				ChangeFocusToUserViewTail();
			}
			else if (num2 < num)
			{
				ChangeHeadPage();
				ChangeFocusView(mViews_WorkSpace[mModels.Length - 1]);
				ChangeImmediateContentPosition(ContentDirection.Hell);
			}
			ChangeState(mState);
		}

		public void TailFocus()
		{
			ListState mState = this.mState;
			ChangeState(ListState.Waiting);
			int num = mUserViewCount;
			int num2 = mModels.Length;
			if (num == num2)
			{
				ChangeHeadPage();
				ChangeFocusView(mViews_WorkSpace[mModels.Length - 1]);
				if (mCurrentFocusView.GetRealIndex() == mUserViewCount - 1)
				{
					ChangeImmediateContentPosition(ContentDirection.Heaven);
				}
				else
				{
					ChangeImmediateContentPosition(ContentDirection.Hell);
				}
			}
			else if (num < num2)
			{
				ChangeImmediateContentPosition(ContentDirection.Heaven);
				ChangeTailPage();
				ChangeFocusToUserViewTail();
			}
			else if (num2 < num)
			{
				ChangeHeadPage();
				ChangeFocusView(mViews_WorkSpace[mModels.Length - 1]);
				ChangeImmediateContentPosition(ContentDirection.Hell);
			}
			ChangeState(mState);
		}

		public void TailFocusPage()
		{
			ListState mState = this.mState;
			ChangeState(ListState.Waiting);
			int num = mUserViewCount;
			int num2 = mModels.Length;
			if (num == num2)
			{
				ChangeHeadPage();
				if (mCurrentFocusView.GetRealIndex() == mUserViewCount - 1)
				{
					ChangeImmediateContentPosition(ContentDirection.Heaven);
				}
				else
				{
					ChangeImmediateContentPosition(ContentDirection.Hell);
				}
			}
			else if (num < num2)
			{
				ChangeImmediateContentPosition(ContentDirection.Heaven);
				ChangeTailPage();
			}
			else if (num2 < num)
			{
				ChangeHeadPage();
				ChangeImmediateContentPosition(ContentDirection.Hell);
			}
			ChangeState(mState);
		}

		public void TailFocusMember()
		{
			ViewModelValueState viewModelValueState = CompareUserViewModelValue();
			ViewModelValueState viewModelValueState2 = viewModelValueState;
			if (viewModelValueState2 == ViewModelValueState.BiggerModel)
			{
				ChangeFocusView((View)null);
				mCurrentFocusView = mViews_WorkSpace[mUserViewCount - 1];
			}
			else
			{
				ChangeFocusView((View)null);
				mCurrentFocusView = mViews_WorkSpace[mModels.Length - 1];
			}
		}

		protected void ResumeFocus()
		{
			ChangeState(ListState.Waiting);
			ChangeFocusView(mCurrentFocusView);
		}

		protected void Refresh(Model[] models, bool firstPage)
		{
			if (mModels != null)
			{
				int num2 = mModels.Length;
			}
			int? num3 = models?.Length;
			if ((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null)
			{
				mCurrentFocusView.GetRealIndex();
			}
			mModels = models;
			if (firstPage)
			{
				ChangeHeadPage();
				ChangeFocusToUserViewHead();
			}
			else
			{
				if (!((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null) || mCurrentFocusView.GetModel() == null)
				{
					return;
				}
				Model[] array = mModels;
				int num = 0;
				while (true)
				{
					if (num < array.Length)
					{
						Model targetB = array[num];
						if (EqualsModel(mCurrentFocusView.GetModel(), targetB))
						{
							break;
						}
						num++;
						continue;
					}
					return;
				}
				ChangeState(ListState.Waiting);
				ChangePageFromModel(mCurrentFocusView.GetModel());
				ChangeState(ListState.Lock);
				Array.IndexOf(mModels, mCurrentFocusView.GetModel());
			}
		}

		protected void RefreshViews()
		{
			View[] array = mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				int realIndex = val.GetRealIndex();
				if (realIndex < mModels.Length)
				{
					val.Initialize(realIndex, mModels[realIndex]);
				}
				else
				{
					val.InitializeDefault(realIndex);
				}
			}
		}

		protected void Initialize(Model[] models)
		{
			mModels = models;
			mViews_WorkSpace = mViews;
			mContentDirection = ContentDirection.Hell;
			ChangeState(ListState.Lock);
			KillAnimationListMoveFromFingerScroll();
			for (int i = 0; i < mViews.Length; i++)
			{
				mViews[i].GetTransform().localPosition = mViewDefaultPositions[i];
			}
			RemoveFocus();
			int num = 0;
			View[] array = mViews_WorkSpace;
			for (int j = 0; j < array.Length; j++)
			{
				View val = array[j];
				if (num < mModels.Length)
				{
					Model model = mModels[num];
					val.Initialize(num, model);
				}
				else
				{
					val.InitializeDefault(num);
				}
				num++;
			}
			ChangeContentPosition(ContentDirection.Hell);
			LockControl();
		}

		protected void Select()
		{
			if (mState == ListState.Waiting && !((UnityEngine.Object)mCurrentFocusView == (UnityEngine.Object)null) && OnSelectable(mCurrentFocusView))
			{
				OnSelect(mCurrentFocusView);
			}
		}

		protected void NextFocus()
		{
			if (mState == ListState.Waiting && mModels != null && mModels.Length != 0 && (UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null)
			{
				RepositionNow();
				if (!ChangeNextFocus())
				{
					ChangeContentPosition(ContentDirection.Hell);
					ChangeHeadPage();
					ChangeFocusToUserViewHead();
				}
			}
		}

		public void RepositionImmediate()
		{
			PlayAnimationRepositionForScroll(null);
		}

		protected void PrevFocus()
		{
			if (mState != ListState.Waiting || mModels == null || mModels.Length == 0 || !((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null))
			{
				return;
			}
			RepositionNow();
			if (!ChangePrevViewFocus())
			{
				if (mUserViewCount <= mModels.Length)
				{
					ChangeContentPosition(ContentDirection.Heaven);
				}
				else
				{
					ChangeContentPosition(ContentDirection.Hell);
				}
				ChangeTailPage();
				ChangeFocusToUserViewTail();
			}
		}

		protected void SetSwipeEventCamera(Camera camera)
		{
			mUIDisplaySwipeEventRegion.SetEventCatchCamera(camera);
		}

		protected void ChangeFocusToUserViewHead()
		{
			ChangeFocusView(mViews_WorkSpace[0]);
		}

		protected void PrevPageOrHeadFocus()
		{
			if (mState == ListState.Waiting && mModels != null && mModels.Length != 0)
			{
				RepositionNow();
				if ((UnityEngine.Object)mViews_WorkSpace[0] != (UnityEngine.Object)mCurrentFocusView)
				{
					ChangeContentPosition(ContentDirection.Hell);
					ChangeFocusView(mViews_WorkSpace[0]);
				}
				else
				{
					ChangePage(mUserViewCount, -1);
					ChangeFocusView(mViews_WorkSpace[0]);
				}
			}
		}

		protected void NextPageOrTailFocus()
		{
			if (mState != ListState.Waiting || mModels == null || mModels.Length == 0)
			{
				return;
			}
			RepositionNow();
			if ((UnityEngine.Object)mViews_WorkSpace[mUserViewCount - 1] != (UnityEngine.Object)mCurrentFocusView && mUserViewCount <= mModels.Length)
			{
				ChangeContentPosition(ContentDirection.Heaven);
				ChangeFocusView(mViews_WorkSpace[mUserViewCount - 1]);
				return;
			}
			if (ChangePage(mUserViewCount, 1))
			{
				ChangeContentPosition(ContentDirection.Heaven);
				ChangeFocusView(mViews_WorkSpace[mUserViewCount - 1]);
				return;
			}
			if (mUserViewCount < mModels.Length)
			{
				ChangeContentPosition(ContentDirection.Heaven);
				ChangeFocusView(mViews_WorkSpace[mUserViewCount - 1]);
				return;
			}
			if (mModels.Length == 1)
			{
				ChangeContentPosition(ContentDirection.Hell);
				ChangeFocusView(mViews_WorkSpace[0]);
				return;
			}
			ViewModelValueState viewModelValueState = CompareUserViewModelValue();
			if (viewModelValueState == ViewModelValueState.BiggerModel)
			{
				ChangeContentPosition(ContentDirection.Heaven);
			}
			else if (mUserViewCount < mModels.Length)
			{
				ChangeContentPosition(ContentDirection.Hell);
			}
			else
			{
				ChangeContentPosition(ContentDirection.Heaven);
			}
			ChangeFocusView(mViews_WorkSpace[mModels.Length - 1]);
		}

		private void ChangeContentPosition(ContentDirection contentPosition)
		{
			if (mContentDirection != contentPosition && mModels != null && (mModels.Length >= mUserViewCount || contentPosition != 0))
			{
				mContentDirection = contentPosition;
				KillAnimationContainerMove();
				switch (mContentDirection)
				{
				case ContentDirection.Heaven:
					mTransform_ContentPosition.DOLocalMoveY(mBottomUpContentPosition, 0.15f).SetId(AnimationType.ContainerMove);
					break;
				case ContentDirection.Hell:
					mTransform_ContentPosition.DOLocalMoveY(mBottomDownContentPosition, 0.15f).SetId(AnimationType.ContainerMove);
					break;
				}
			}
		}

		protected void ChangeImmediateContentPosition(ContentDirection contentPosition)
		{
			KillAnimationContainerMove();
			mContentDirection = contentPosition;
			switch (mContentDirection)
			{
			case ContentDirection.Heaven:
				mTransform_ContentPosition.localPositionY(mBottomUpContentPosition);
				break;
			case ContentDirection.Hell:
				mTransform_ContentPosition.localPositionY(mBottomDownContentPosition);
				break;
			}
		}

		private void SettingViewListeners()
		{
			View[] array = mViews;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				val.SetOnTouchListener(OnTouchListener);
			}
		}

		private void HeadFocusMember()
		{
			switch (mContentDirection)
			{
			case ContentDirection.Hell:
			{
				float y = mViewDefaultPositions[0].y;
				Vector3 localPosition = mViews_WorkSpace[0].GetTransform().localPosition;
				if (y < localPosition.y)
				{
					ChangeFocusView((View)null);
					mCurrentFocusView = mViews_WorkSpace[1];
				}
				else
				{
					ChangeFocusView((View)null);
					mCurrentFocusView = mViews_WorkSpace[0];
				}
				break;
			}
			case ContentDirection.Heaven:
				ChangeFocusView(mViews_WorkSpace[1]);
				break;
			}
		}

		private void ReleaseMemberViews()
		{
			if (mViews != null)
			{
				for (int i = 0; i < mViews.Length; i++)
				{
					mViews[i] = (View)null;
				}
			}
			mViews = null;
		}

		private void ReleaseWorksSpaceViews()
		{
			if (mViews_WorkSpace != null)
			{
				for (int i = 0; i < mViews_WorkSpace.Length; i++)
				{
					mViews_WorkSpace[i] = (View)null;
				}
			}
			mViews_WorkSpace = null;
		}

		private void MemoryViewsDefaultPosition()
		{
			mViewDefaultPositions = new Vector3[mViews.Length];
			for (int i = 0; i < mViews.Length; i++)
			{
				mViewDefaultPositions[i] = mViews[i].GetTransform().localPosition;
			}
		}

		private void MemoryViewRangePositions()
		{
			mOuterHead = mViews[0].GetHeight();
			mOuterTail = -(mViews[0].GetHeight() * mViews.Length);
		}

		private void OnDisplayEvents(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if ((mState == ListState.Waiting || mState == ListState.MoveFromFinger || mState == ListState.MoveFromFingerScrolling) && mModels != null && mModels.Length != 0)
			{
				switch (actionType)
				{
				case UIDisplaySwipeEventRegion.ActionType.Start:
					OnPressEvent(UICamera.lastTouchPosition, pressed: true, isFlick: false);
					break;
				case UIDisplaySwipeEventRegion.ActionType.FingerUp:
					OnPressEvent(UICamera.lastTouchPosition, pressed: false, isFlick: false);
					break;
				case UIDisplaySwipeEventRegion.ActionType.FingerUpWithVerticalFlick:
					OnPressEvent(UICamera.lastTouchPosition, pressed: false, isFlick: true);
					break;
				case UIDisplaySwipeEventRegion.ActionType.Moving:
					OnDragging(new Vector2(deltaX, deltaY));
					break;
				}
			}
		}

		private void OnTouchListener(View view)
		{
			if (mState != ListState.Waiting)
			{
				return;
			}
			switch (mContentDirection)
			{
			case ContentDirection.Hell:
			{
				Vector3 localPosition2 = view.transform.localPosition;
				float num2 = localPosition2.y - (float)view.GetHeight();
				float num3 = -(view.GetHeight() * mUserViewCount);
				if (num2 <= num3)
				{
					return;
				}
				break;
			}
			case ContentDirection.Heaven:
			{
				Vector3 localPosition = view.transform.localPosition;
				float y = localPosition.y;
				float num = mBottomUpContentPosition + y;
				if (0f < num)
				{
					return;
				}
				break;
			}
			}
			ChangeFocusView(view);
			Select();
		}

		private Tween PlayAnimationListMoveFromFingerScroll(float scrollPower, Action onFinished)
		{
			ScrollDirection scrollDirection = CheckDirection(scrollPower);
			float to = (float)mViews[0].GetHeight() * MOVE_ITEM_LEVEL;
			float movedDistance = 0f;
			return DOVirtual.Float(0f, to, FRAME_MOVE_SECOND, delegate(float moveDistance)
			{
				float num = moveDistance - movedDistance;
				switch (scrollDirection)
				{
				case ScrollDirection.Hell:
					MoveToHellFromFingerFlick(0f - num);
					break;
				case ScrollDirection.Heaven:
					MoveToHeavenFromFingerFlick(num);
					break;
				}
				movedDistance = moveDistance;
			}).OnComplete(delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
			}).SetEase(mListMoveEaseType)
				.SetId(AnimationType.ListMoveFromFingerScroll);
		}

		private void MoveToHellFromFingerFlick(float scrollDistance)
		{
			if (mViews_WorkSpace[0].GetRealIndex() == 0)
			{
				ContentDirection contentPosition = CheckContentDirection(scrollDistance);
				if (mUserViewCount < mModels.Length)
				{
					ChangeContentPosition(contentPosition);
				}
				else
				{
					ChangeContentPosition(ContentDirection.Hell);
				}
				KillAnimationListMoveFromFingerScroll();
				PlayAnimationRepositionForScroll(OnFinishedAnimationRepositionForScroll);
				return;
			}
			switch (CheckDirection(scrollDistance))
			{
			case ScrollDirection.Heaven:
				if (mViews_WorkSpace[0].GetRealIndex() == 0)
				{
					PlayAnimationRepositionForScroll(OnFinishedAnimationRepositionForScroll);
					break;
				}
				HeadFocus();
				ChangeContentPosition(ContentDirection.Heaven);
				break;
			case ScrollDirection.Hell:
				ChangeContentPosition(ContentDirection.Hell);
				break;
			}
			Scroll(scrollDistance);
		}

		private void MoveToHeavenFromFingerFlick(float scrollDistance)
		{
			int num = (mViews_WorkSpace.Length < mModels.Length) ? mUserViewCount : ((mModels.Length != 1) ? ((mModels.Length >= mUserViewCount) ? (mModels.Length - 1) : (mUserViewCount - 1)) : 0);
			ViewModelValueState viewModelValueState = CompareWorkSpaceViewModelValue();
			bool flag = false;
			switch (viewModelValueState)
			{
			case ViewModelValueState.BiggerModel:
				flag = (mModels.Length <= mViews_WorkSpace[num].GetRealIndex());
				break;
			case ViewModelValueState.BiggerWorkSpaceView:
				flag = (mModels.Length <= mViews_WorkSpace[mUserViewCount].GetRealIndex());
				break;
			}
			if (flag)
			{
				KillAnimationListMoveFromFingerScroll();
				PlayAnimationRepositionForScroll(OnFinishedAnimationRepositionForScroll);
				return;
			}
			switch (CheckDirection(scrollDistance))
			{
			case ScrollDirection.Heaven:
				ChangeContentPosition(ContentDirection.Heaven);
				break;
			case ScrollDirection.Hell:
				ChangeContentPosition(ContentDirection.Hell);
				break;
			}
			Scroll(scrollDistance);
		}

		protected void ChangeFocusView(View view)
		{
			if ((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null)
			{
				mCurrentFocusView.RemoveHover();
			}
			mCurrentFocusView = view;
			if ((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null)
			{
				mCurrentFocusView.Hover();
				OnChangedFocusView(mCurrentFocusView);
			}
		}

		public void StopFocusBlink()
		{
			if ((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null)
			{
				mCurrentFocusView.RemoveHover();
			}
		}

		public void StartFocusBlink()
		{
			View[] array = mViews;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				val.RemoveHover();
			}
			if ((UnityEngine.Object)mCurrentFocusView != (UnityEngine.Object)null)
			{
				mCurrentFocusView.Hover();
			}
		}

		protected void ChangeState(ListState state)
		{
			mState = state;
		}

		private ViewModelValueState CompareUserViewModelValue()
		{
			if (mModels.Length == mUserViewCount)
			{
				return ViewModelValueState.EvenUserViewAndModel;
			}
			if (mUserViewCount < mModels.Length)
			{
				return ViewModelValueState.BiggerModel;
			}
			if (mModels.Length < mUserViewCount)
			{
				return ViewModelValueState.BiggerUserView;
			}
			return ViewModelValueState.Default;
		}

		private ViewModelValueState CompareWorkSpaceViewModelValue()
		{
			if (mModels.Length == mViews_WorkSpace.Length)
			{
				return ViewModelValueState.EvenWorkSpaceViewAndModel;
			}
			if (mViews_WorkSpace.Length < mModels.Length)
			{
				return ViewModelValueState.BiggerModel;
			}
			if (mModels.Length < mViews_WorkSpace.Length)
			{
				return ViewModelValueState.BiggerWorkSpaceView;
			}
			return ViewModelValueState.Default;
		}

		protected void ChangeTailPage()
		{
			if (CompareUserViewModelValue() == ViewModelValueState.EvenUserViewAndModel || mUserViewCount >= mModels.Length)
			{
				return;
			}
			for (int i = 0; i < mViews.Length; i++)
			{
				mViews[i].GetTransform().localPosition = mViewDefaultPositions[i];
				int num = mModels.Length - mUserViewCount + i;
				if (num < mModels.Length)
				{
					Model model = mModels[num];
					mViews[i].Initialize(num, model);
				}
				else
				{
					mViews[i].InitializeDefault(num);
				}
			}
			mViews_WorkSpace = mViews;
		}

		private void ChangeFocusToUserViewTail()
		{
			if (mUserViewCount < mModels.Length)
			{
				ChangeFocusView(mViews_WorkSpace[mUserViewCount - 1]);
			}
			else
			{
				ChangeFocusView(mViews_WorkSpace[mModels.Length - 1]);
			}
		}

		private bool ChangeNextFocus()
		{
			int num = Array.IndexOf(mViews_WorkSpace, mCurrentFocusView);
			int realIndex = mCurrentFocusView.GetRealIndex();
			int num2 = num + 1;
			int num3 = realIndex + 1;
			if (num3 < mModels.Length)
			{
				if (num2 < mUserViewCount)
				{
					if (num2 == mUserViewCount - 1)
					{
						ChangeContentPosition(ContentDirection.Heaven);
					}
					ChangeFocusView(mViews_WorkSpace[num2]);
					return true;
				}
				if (Scroll(1, paddingRollControl: true))
				{
					View view = mViews_WorkSpace[mUserViewCount - 1];
					ChangeFocusView(view);
					return true;
				}
				View view2 = mViews_WorkSpace[mUserViewCount];
				ChangeFocusView(view2);
				return true;
			}
			return false;
		}

		private bool ChangePrevViewFocus()
		{
			int num = Array.IndexOf(mViews_WorkSpace, mCurrentFocusView);
			int realIndex = mCurrentFocusView.GetRealIndex();
			int num2 = num - 1;
			int num3 = realIndex - 1;
			if (num2 == 0)
			{
				ChangeContentPosition(ContentDirection.Hell);
			}
			if (realIndex == 0 && num == 0)
			{
				return false;
			}
			if (0 <= num2)
			{
				ChangeFocusView(mViews_WorkSpace[num2]);
				return true;
			}
			if (num2 < 0 && 0 <= num3 && Scroll(-1, paddingRollControl: false))
			{
				ChangeFocusView(mViews_WorkSpace[0]);
				return true;
			}
			return false;
		}

		private void OnDragging(Vector2 delta)
		{
			if (mState != ListState.Waiting && mState != ListState.MoveFromFinger)
			{
				return;
			}
			RemoveFocus();
			CompareUserViewModelValue();
			ScrollDirection scrollDirection = CheckDirection(delta.y);
			if (mUserViewCount < mModels.Length)
			{
				switch (scrollDirection)
				{
				case ScrollDirection.Hell:
					ChangeContentPosition(ContentDirection.Hell);
					break;
				case ScrollDirection.Heaven:
					ChangeContentPosition(ContentDirection.Heaven);
					break;
				}
			}
			RemoveFocus();
			ChangeState(ListState.MoveFromFinger);
			Scroll(delta.y);
		}

		private ContentDirection CheckContentDirection(float value)
		{
			if (0f < value)
			{
				return ContentDirection.Heaven;
			}
			if (value < 0f)
			{
				return ContentDirection.Hell;
			}
			return mContentDirection;
		}

		private void OnPressEvent(Vector3 eventPosition, bool pressed, bool isFlick)
		{
			if (pressed)
			{
				KillAnimationAndForceCompleteRepositionForScroll();
				mLastTouchedPosition = eventPosition;
				return;
			}
			Vector3 diff = -(mLastTouchedPosition - eventPosition);
			if (isFlick)
			{
				KillAnimationAndForceCompleteRepositionForScroll();
				KillAnimationListMoveFromFingerScroll();
				ChangeState(ListState.MoveFromFingerScrolling);
				PlayAnimationListMoveFromFingerScroll(diff.y, delegate
				{
					ChangeState(ListState.Waiting);
					ContentDirection contentDirection = CheckContentDirection(diff.y);
					ChangeContentPosition(contentDirection);
					switch (contentDirection)
					{
					case ContentDirection.Heaven:
						TailFocusForOnFinishedScroll();
						break;
					case ContentDirection.Hell:
						HeadFocus();
						break;
					}
				});
				return;
			}
			switch (CheckContentDirection(diff.y))
			{
			case ContentDirection.Hell:
				ChangeContentPosition(ContentDirection.Hell);
				HeadFocusMember();
				if (mCurrentFocusView.GetRealIndex() == 0)
				{
					PlayAnimationRepositionForScroll(OnFinishedAnimationRepositionForScroll);
					break;
				}
				ChangeFocusView(mCurrentFocusView);
				ChangeState(ListState.Waiting);
				break;
			case ContentDirection.Heaven:
				ChangeContentPosition(ContentDirection.Heaven);
				TailFocusMember();
				if (mCurrentFocusView.GetRealIndex() == mModels.Length - 1)
				{
					PlayAnimationRepositionForScroll(OnFinishedAnimationRepositionForScroll);
					break;
				}
				ChangeFocusView(mCurrentFocusView);
				ChangeState(ListState.Waiting);
				break;
			}
		}

		private void OnFinishedAnimationRepositionForScroll()
		{
			if (mUserViewCount < mModels.Length)
			{
				switch (mContentDirection)
				{
				case ContentDirection.Heaven:
					ChangeFocusToUserViewTail();
					break;
				case ContentDirection.Hell:
					ChangeFocusToUserViewHead();
					break;
				}
			}
			else
			{
				ChangeFocusToUserViewHead();
			}
			ChangeState(ListState.Waiting);
		}

		private void KillAnimationAndForceCompleteRepositionForScroll()
		{
			if (DOTween.IsTweening(AnimationType.RepositionForScroll))
			{
				DOTween.Kill(AnimationType.RepositionForScroll, complete: true);
			}
		}

		public void KillScrollAnimation()
		{
			KillAnimationListMoveFromFingerScroll();
			KillAnimationAndForceCompleteRepositionForScroll();
		}

		private void KillAnimationContainerMove()
		{
			if (DOTween.IsTweening(AnimationType.ContainerMove))
			{
				DOTween.Kill(AnimationType.ContainerMove);
			}
		}

		private void KillAnimationListMoveFromFingerScroll()
		{
			if (DOTween.IsTweening(AnimationType.ListMoveFromFingerScroll))
			{
				DOTween.Kill(AnimationType.ListMoveFromFingerScroll);
			}
		}

		private Tween PlayAnimationRepositionForScroll(Action onComplete)
		{
			if (DOTween.IsTweening(AnimationType.RepositionForScroll))
			{
				DOTween.Kill(AnimationType.RepositionForScroll, complete: true);
			}
			int num = 0;
			View[] array = mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View value = array[i];
				int num2 = Array.IndexOf(mViews_WorkSpace, value);
				int num3 = Utils.LoopValue(num2 - 1, 0, mViews_WorkSpace.Length);
				int num4 = value.GetRealIndex() + mViews_WorkSpace.Length;
				if (num4 < mModels.Length)
				{
					float num5 = mOuterHead;
					Vector3 localPosition = value.GetTransform().localPosition;
					if (num5 < localPosition.y)
					{
						value.GetTransform().localPosition = mViews_WorkSpace[num3].GetTransform().localPosition;
						value.GetTransform().AddLocalPositionY(-value.GetHeight());
						value.Initialize(num4, mModels[num4]);
						num--;
					}
				}
			}
			if (0 < num || num < 0)
			{
				mViews_WorkSpace = RollReferenceViews(mViews_WorkSpace, num);
			}
			Sequence sequence = DOTween.Sequence();
			sequence.SetId(AnimationType.RepositionForScroll);
			for (int j = 0; j < mViews.Length; j++)
			{
				Tween t = mViews_WorkSpace[j].GetTransform().DOLocalMoveY(mViewDefaultPositions[j].y, 0.3f);
				sequence.Join(t);
			}
			sequence.OnComplete(delegate
			{
				if (onComplete != null)
				{
					onComplete();
				}
			});
			return sequence;
		}

		private void PlayAnimationFingerUpReposition(Action onComplete)
		{
			if (!DOTween.IsTweening(AnimationType.RepositionForScroll))
			{
				Sequence sequence = DOTween.Sequence();
				sequence.SetId(AnimationType.RepositionForScroll);
				for (int i = 0; i < mViews.Length; i++)
				{
					Tween t = mViews_WorkSpace[i].GetTransform().DOLocalMoveY(mViewDefaultPositions[i].y, 0.3f);
					sequence.Join(t);
				}
				sequence.OnComplete(delegate
				{
					if (onComplete != null)
					{
						onComplete();
					}
				});
			}
		}

		private ScrollDirection CheckDirection(float directionY)
		{
			ScrollDirection result = (mContentDirection != 0) ? ScrollDirection.Hell : ScrollDirection.Heaven;
			if (0f < directionY)
			{
				result = ScrollDirection.Heaven;
			}
			else if (directionY < 0f)
			{
				result = ScrollDirection.Hell;
			}
			return result;
		}

		private bool ChangePage(int pageInValue, int changeCount)
		{
			ScrollDirection scrollDirection = CheckDirection(-changeCount);
			for (int i = 0; i < Math.Abs(changeCount) * pageInValue; i++)
			{
				switch (scrollDirection)
				{
				case ScrollDirection.Heaven:
					Scroll(-1, paddingRollControl: true);
					ChangeContentPosition(ContentDirection.Hell);
					break;
				case ScrollDirection.Hell:
					if (!Scroll(1, paddingRollControl: true))
					{
						ChangeContentPosition(ContentDirection.Heaven);
						return false;
					}
					break;
				}
			}
			return true;
		}

		private bool Scroll(int count, bool paddingRollControl)
		{
			if (mState != ListState.Waiting)
			{
				return false;
			}
			switch (CheckDirection(count))
			{
			case ScrollDirection.Heaven:
				if (paddingRollControl)
				{
					if (mModels.Length - 1 < mViews_WorkSpace[mUserViewCount - 1].GetRealIndex() + count)
					{
						return false;
					}
				}
				else
				{
					View[] array2 = mViews_WorkSpace;
					for (int k = 0; k < array2.Length; k++)
					{
						View val3 = array2[k];
						if (val3.GetRealIndex() == mModels.Length - 1)
						{
							return false;
						}
					}
				}
				for (int l = 0; l < mViews_WorkSpace.Length; l++)
				{
					View val4 = mViews_WorkSpace[l];
					int num3 = l - count;
					bool flag3 = false;
					flag3 |= (num3 < 0);
					if (flag3 | (mViews_WorkSpace.Length <= num3))
					{
						int num4 = val4.GetRealIndex() + (mViews_WorkSpace.Length - 1) + count;
						bool flag4 = 0 <= num4;
						if (flag4 & (num4 < mModels.Length))
						{
							val4.Initialize(num4, mModels[num4]);
						}
						else
						{
							val4.InitializeDefault(num4);
						}
					}
				}
				break;
			case ScrollDirection.Hell:
			{
				View[] array = mViews_WorkSpace;
				for (int i = 0; i < array.Length; i++)
				{
					View val = array[i];
					if (val.GetRealIndex() == 0)
					{
						return false;
					}
				}
				for (int j = 0; j < mViews_WorkSpace.Length; j++)
				{
					View val2 = mViews_WorkSpace[j];
					int num = j - count;
					bool flag = false;
					flag |= (num < 0);
					if (flag | (mViews_WorkSpace.Length <= num))
					{
						int num2 = val2.GetRealIndex() - mViews_WorkSpace.Length + 1 + count;
						bool flag2 = 0 <= num2;
						if (flag2 & (num2 < mModels.Length))
						{
							val2.Initialize(num2, mModels[num2]);
						}
						else
						{
							val2.InitializeDefault(num2);
						}
					}
				}
				break;
			}
			}
			mViews_WorkSpace = RollReferenceViews(mViews_WorkSpace, -count);
			RepositionNow();
			return true;
		}

		private void RepositionNow()
		{
			for (int i = 0; i < mViews_WorkSpace.Length; i++)
			{
				mViews_WorkSpace[i].GetTransform().localPosition = mViewDefaultPositions[i];
			}
		}

		private int ScrollToHeaven(float moveY)
		{
			int num = 0;
			View[] array = mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				if (IsViewHeadInRange(val, mOuterHead, mOuterTail))
				{
					continue;
				}
				int num2 = Array.IndexOf(mViews_WorkSpace, val);
				int num3 = Utils.LoopValue(num2 - 1, 0, mViews_WorkSpace.Length);
				int num4 = val.GetRealIndex() + mViews_WorkSpace.Length;
				if (num4 < mModels.Length)
				{
					float num5 = mOuterHead;
					Vector3 localPosition = val.GetTransform().localPosition;
					if (num5 < localPosition.y)
					{
						Model model = mModels[num4];
						val.GetTransform().localPosition = mViews_WorkSpace[num3].GetTransform().localPosition;
						val.GetTransform().AddLocalPositionY(-val.GetHeight());
						val.Initialize(num4, model);
						num--;
					}
				}
				else if (val.GetRealIndex() < mModels.Length - mUserViewCount)
				{
					val.GetTransform().localPosition = mViews_WorkSpace[num3].GetTransform().localPosition;
					val.GetTransform().AddLocalPositionY(-val.GetHeight());
					val.InitializeDefault(num4);
					num--;
				}
			}
			return num;
		}

		private int ScrollToHell(float moveY)
		{
			int num = 0;
			View[] array = mViews_WorkSpace.Reverse().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				if (IsViewTailInRange(val, mOuterHead, mOuterTail))
				{
					continue;
				}
				int num2 = Array.IndexOf(mViews_WorkSpace, val);
				int num3 = Utils.LoopValue(num2 + 1, 0, mViews_WorkSpace.Length);
				int num4 = val.GetRealIndex() - mViews_WorkSpace.Length;
				if (0 <= num4)
				{
					Vector3 localPosition = val.GetTransform().localPosition;
					if (localPosition.y - (float)val.GetHeight() < mOuterTail)
					{
						val.GetTransform().localPosition = mViews_WorkSpace[num3].GetTransform().localPosition;
						val.GetTransform().AddLocalPositionY(val.GetHeight());
						val.Initialize(num4, mModels[num4]);
						num++;
					}
				}
			}
			return num;
		}

		private bool Scroll(float moveY)
		{
			if (mState != ListState.Waiting && mState != ListState.MoveFromFinger && mState != ListState.MoveFromFingerScrolling)
			{
				return false;
			}
			ScrollDirection scrollDirection = CheckDirection(moveY);
			View[] array = mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				val.GetTransform().AddLocalPositionY(moveY);
			}
			int num = 0;
			switch (scrollDirection)
			{
			case ScrollDirection.Heaven:
				num = ScrollToHeaven(moveY);
				break;
			case ScrollDirection.Hell:
				num = ScrollToHell(moveY);
				break;
			}
			if (num != 0)
			{
				mViews_WorkSpace = RollReferenceViews(mViews_WorkSpace, num);
			}
			return true;
		}

		private View[] RollReferenceViews(View[] views, int rollCount)
		{
			View[] array = new View[views.Length];
			for (int i = 0; i < views.Length; i++)
			{
				int num = Utils.LoopValue(i + rollCount, 0, views.Length);
				array[num] = views[i];
			}
			return array;
		}

		private bool IsViewHeadInRange(View view, float from, float to)
		{
			Vector3 localPosition = view.GetTransform().localPosition;
			float y = localPosition.y;
			return Utils.RangeEqualsIn(y, from, to);
		}

		private bool IsViewTailInRange(View view, float from, float to)
		{
			Vector3 localPosition = view.GetTransform().localPosition;
			float currentPosition = localPosition.y - (float)view.GetHeight();
			return Utils.RangeEqualsIn(currentPosition, from, to);
		}
	}
}
