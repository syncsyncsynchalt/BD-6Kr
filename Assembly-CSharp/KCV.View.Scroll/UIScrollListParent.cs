using DG.Tweening;
using KCV.Display;
using System;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class UIScrollListParent<Model, View> : MonoBehaviour where Model : class where View : UIScrollListChild<Model>
	{
		public delegate void UIScrollListParentAction(ActionType actionType, UIScrollListParent<Model, View> calledObject, UIScrollListChild<Model> actionChild);

		public delegate bool UIScrollListParentCheck(CheckType checkType, UIScrollListParent<Model, View> calledObject, Model checkChild);

		protected Model[] Models;

		private Vector3[] ViewsDefaultLocalPosition;

		protected View[] Views;

		protected View ViewFocus;

		private bool AnimationScrollNow;

		public bool EnableTouchControl = true;

		[SerializeField]
		[Tooltip("子要素のプレハブを設定してください。")]
		private View mPrefab_UIScrollListChild;

		[Tooltip("スワイプイベントを受け取る為のオブジェクト")]
		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		[SerializeField]
		[Tooltip("作った要素を入れる場所")]
		private UIGrid mGridContaints;

		private UIScrollListParentAction mUIScrollListParentAction;

		private UIScrollListParentCheck mUIScrollListParentCheck;

		[SerializeField]
		[Tooltip("ユ\u30fcザが１度に見ることができる要素の最大数を設定してください。予備のオブジェクトの事は考えないでください")]
		private int MAX_USER_VIEW_OBJECTS = 6;

		[SerializeField]
		private int MAX_RELIMINARY_OBJECTS = 1;

		private Vector3 mVector3_GridContaintsDefaultPosition;

		private bool mUseBottomUpReposition;

		[Tooltip("リスト下にフォ\u30fcカスが入った時にフォ\u30fcカス位置を底上げします")]
		[SerializeField]
		private Vector3 mVector3_BottomUpPosition = default(Vector3);

		private int MaxLogicViewObjects;

		private Vector3 mVector3Top;

		private Vector3 mVector3TopDestroyLine;

		private Vector3 mVector3Bottom;

		private Vector3 mVector3BottomDestroyLine;

		protected KeyControl mKeyController;

		private int RollCount;

		private int CursolIndex;

		private float defaultIntervalTime;

		private bool mDefaultClickable = true;

		private Vector3 mVector3_RelocationCache = Vector3.zero;

		protected bool AnimationViewPositionNow
		{
			get;
			private set;
		}

		private void Awake()
		{
			mVector3_GridContaintsDefaultPosition = mGridContaints.transform.localPosition;
			OnAwake();
		}

		private void Start()
		{
			OnStart();
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnStart()
		{
		}

		public void SetOnUIScrollListParentAction(UIScrollListParentAction method)
		{
			mUIScrollListParentAction = method;
		}

		public void SetOnUIScrollListParentCheck(UIScrollListParentCheck method)
		{
			mUIScrollListParentCheck = method;
		}

		protected virtual void Initialize(Model[] models)
		{
			mGridContaints.transform.localPosition = mVector3_GridContaintsDefaultPosition;
			MaxLogicViewObjects = GetUserViewingLength() + GetReliminaryLength();
			Models = models;
			RollCount = 0;
			CursolIndex = 0;
			AnimationScrollNow = false;
			AnimationViewPositionNow = false;
			DestroyGridInChildren();
			View viewPrefab = GetViewPrefab();
			for (int i = 0; i < GetLogicViewingLength(); i++)
			{
				View component = Util.Instantiate(viewPrefab.gameObject, mGridContaints.gameObject).GetComponent<View>();
				component.SetOnActionListener(OnChildAction);
			}
			mGridContaints.Reposition();
			Views = mGridContaints.GetComponentsInChildren<View>();
			ViewsDefaultLocalPosition = new Vector3[Views.Length];
			int num = 0;
			View[] views = Views;
			for (int j = 0; j < views.Length; j++)
			{
				View val = views[j];
				ViewsDefaultLocalPosition[num++] = val.cachedTransform.localPosition;
			}
			OnInitialize(RollCount);
		}

		public void EnableBottomUpMode()
		{
			mUseBottomUpReposition = true;
		}

		public void DisableBottomUpMode()
		{
			mUseBottomUpReposition = false;
		}

		public void UpdateNotify()
		{
			View[] views = Views;
			for (int i = 0; i < views.Length; i++)
			{
				View exists = views[i];
				OnCheck(CheckType.Clickable, this, exists.Model);
				exists.Refresh(exists.Model, (UnityEngine.Object)exists);
			}
		}

		protected void Refresh(Model[] models)
		{
			Models = models;
			if (RollCount >= Models.Length)
			{
				int num = (RollCount / MAX_USER_VIEW_OBJECTS - 1) * MAX_USER_VIEW_OBJECTS;
				if (num < Models.Length && 0 < num)
				{
					RollCount = num;
				}
				else
				{
					RollCount = 0;
				}
			}
			AnimationScrollNow = false;
			AnimationViewPositionNow = false;
			MaxLogicViewObjects = GetUserViewingLength() + GetReliminaryLength();
			OnRefresh(RollCount, CursolIndex);
		}

		public void RefreshAndFirstFocus(Model[] models)
		{
			Models = models;
			AnimationScrollNow = false;
			AnimationViewPositionNow = false;
			MaxLogicViewObjects = GetUserViewingLength() + GetReliminaryLength();
			RollCount = 0;
			OnRefresh(0, 0);
		}

		private void OnRefresh(int rollCount, int focusPosition)
		{
			int logicViewingLength = GetLogicViewingLength();
			int num = rollCount % logicViewingLength;
			for (int i = 0; i < logicViewingLength; i++)
			{
				View child = Views[i];
				int num2 = rollCount + i - num;
				if (i < num)
				{
					num2 += logicViewingLength;
				}
				OnUpdateChild(num2, child, (num2 >= Models.Length) ? ((Model)null) : Models[num2]);
			}
			float childHeight = GetChildHeight();
			mVector3Top = new Vector3(0f, 0f, 0f);
			mVector3TopDestroyLine = new Vector3(0f, mVector3Top.y + childHeight, 0f);
			mVector3Bottom = new Vector3(0f, (0f - childHeight) * (float)GetLogicViewingLength(), 0f);
			mVector3BottomDestroyLine = new Vector3(0f, (0f - childHeight) * (float)(GetLogicViewingLength() - 1), 0f);
			mGridContaints.Reposition();
			RefreshPosition(0f, 0f);
			mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(UIDisplaySwipeEventRegionDelegate);
			int modelSize = GetModelSize();
			if (rollCount + focusPosition < modelSize)
			{
				int num3 = (rollCount + focusPosition) % GetLogicViewingLength();
				ChangeFocusView(Views[num3], isFirstFocus: false);
				if (mUseBottomUpReposition)
				{
					if (GetUserViewingLength() / 2 < focusPosition)
					{
						BottomUp();
					}
					else
					{
						BottomDown();
					}
				}
			}
			else if (0 < modelSize)
			{
				Debug.Log("HeadIndex:" + num + " focusPos:" + focusPosition + " modSize:" + modelSize);
				ChangeFocusView(Views[num], isFirstFocus: false);
				BottomDown();
			}
			View[] views = Views;
			for (int j = 0; j < views.Length; j++)
			{
				View val = views[j];
				val.cachedTransform.localPosition = ViewsDefaultLocalPosition[val.SortIndex - RollCount];
			}
			OnFinishedInitialize();
		}

		private void BottomUp()
		{
			mGridContaints.transform.DOLocalMove(mVector3_BottomUpPosition, 0.3f);
		}

		private void BottomDown()
		{
			mGridContaints.transform.DOLocalMove(mVector3_GridContaintsDefaultPosition, 0.3f);
		}

		protected virtual void OnChangedModels(Model[] models)
		{
		}

		public void ForceInputKey(KeyControl.KeyName keyName)
		{
			switch (keyName)
			{
			case KeyControl.KeyName.UP:
				if (!AnimationViewPositionNow)
				{
					ShowChildPrev();
				}
				break;
			case KeyControl.KeyName.DOWN:
				if (!AnimationViewPositionNow)
				{
					ShowChildNext();
				}
				break;
			case KeyControl.KeyName.MARU:
				OnKeyPressCircle();
				break;
			case KeyControl.KeyName.L:
				MoveByView(-GetUserViewingLength());
				break;
			case KeyControl.KeyName.R:
				MoveByView(GetUserViewingLength());
				break;
			case KeyControl.KeyName.BATU:
				OnAction(ActionType.OnBack, this, ViewFocus);
				break;
			}
		}

		public virtual void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
			if (mKeyController == null)
			{
				if ((UnityEngine.Object)ViewFocus != (UnityEngine.Object)null)
				{
					ViewFocus.StopBlink();
				}
				EnableTouchControl = false;
			}
			else
			{
				ChangeFocusView(ViewFocus, isFirstFocus: false);
				EnableTouchControl = true;
			}
		}

		private void setViewFocusHover(bool isHover)
		{
			if (!((UnityEngine.Object)ViewFocus == (UnityEngine.Object)null))
			{
				if (isHover)
				{
					ViewFocus.Hover();
				}
				else
				{
					ViewFocus.RemoveHover();
				}
			}
		}

		public KeyControl GetKeyController()
		{
			if (mKeyController == null)
			{
				mKeyController = new KeyControl();
			}
			setViewFocusHover(isHover: true);
			EnableTouchControl = true;
			return mKeyController;
		}

		public int GetModelSize()
		{
			return Models.Length;
		}

		public void SetCamera(Camera cam)
		{
			mUIDisplaySwipeEventRegion.SetEventCatchCamera(cam);
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if ((mKeyController.keyState[12].holdTime > 1.5f || mKeyController.keyState[8].holdTime > 1.5f) && mKeyController.KeyInputInterval == defaultIntervalTime)
				{
					mKeyController.KeyInputInterval /= 3f;
				}
				else
				{
					mKeyController.KeyInputInterval = defaultIntervalTime;
				}
				if (mKeyController.keyState[8].down)
				{
					OnKeyPressUp();
				}
				else if (mKeyController.keyState[12].down)
				{
					OnKeyPressDown();
				}
				else if (mKeyController.keyState[1].down)
				{
					OnKeyPressCircle();
				}
				else if (mKeyController.keyState[0].down)
				{
					OnKeyPressCross();
				}
				else if (mKeyController.keyState[14].down)
				{
					OnKeyPressLeft();
				}
				else if (mKeyController.keyState[10].down)
				{
					OnKeyPressRight();
				}
				else if (mKeyController.keyState[3].down)
				{
					OnKeyPressTriangle();
				}
			}
			OnUpdate();
		}

		protected virtual void OnKeyPressTriangle()
		{
		}

		protected virtual void OnKeyPressRight()
		{
			if (!AnimationViewPositionNow)
			{
				ShowChildNextPage();
			}
		}

		protected virtual void OnKeyPressLeft()
		{
			if (!AnimationViewPositionNow)
			{
				ShowChildPrevPage();
			}
		}

		protected virtual void OnKeyPressUp()
		{
			if (!AnimationViewPositionNow)
			{
				ShowChildPrev();
			}
		}

		protected virtual void OnKeyPressDown()
		{
			if (!AnimationViewPositionNow)
			{
				ShowChildNext();
			}
		}

		protected virtual void OnKeyPressL()
		{
			MoveByView(-GetUserViewingLength());
		}

		protected virtual void OnKeyPressR()
		{
			MoveByView(GetUserViewingLength());
		}

		protected virtual void OnKeyPressCross()
		{
			OnAction(ActionType.OnBack, this, ViewFocus);
		}

		protected virtual void OnKeyPressCircle()
		{
			if (!mUIDisplaySwipeEventRegion.isDraging && (UnityEngine.Object)ViewFocus != (UnityEngine.Object)null && ViewFocus.Model != null && ViewFocus.mIsClickable)
			{
				OnAction(ActionType.OnButtonSelect, this, ViewFocus);
			}
		}

		protected virtual void OnAction(ActionType actionType, UIScrollListParent<Model, View> calledObject, View actionChild)
		{
			if (mUIScrollListParentAction != null)
			{
				mUIScrollListParentAction(actionType, calledObject, actionChild);
			}
		}

		public void SetDefaultClickable(bool defaultClickable)
		{
			mDefaultClickable = defaultClickable;
		}

		private bool OnCheck(CheckType checkType, UIScrollListParent<Model, View> calledObject, Model checkChild)
		{
			if (mUIScrollListParentCheck != null)
			{
				return mUIScrollListParentCheck(checkType, calledObject, checkChild);
			}
			return mDefaultClickable;
		}

		private void ShowChildPrev()
		{
			if (RollCount == 0 && CursolIndex == 0)
			{
				return;
			}
			CursolIndex--;
			if (0 <= CursolIndex)
			{
				int rollCount = RollCount;
				int sortIndex = ViewFocus.SortIndex;
				if (rollCount < sortIndex)
				{
					View viewFromSortIndex = GetViewFromSortIndex(ViewFocus.SortIndex - 1);
					ChangeFocusView(viewFromSortIndex, isFirstFocus: false);
				}
				if (mUseBottomUpReposition)
				{
					BottomDown();
				}
				return;
			}
			CursolIndex = 0;
			RollCount--;
			View[] views = Views;
			for (int i = 0; i < views.Length; i++)
			{
				View val = views[i];
				if (val.SortIndex == RollCount + GetLogicViewingLength())
				{
					OnUpdateChild(RollCount, val, Models[RollCount]);
					ChangeFocusView(val, isFirstFocus: false);
				}
				int num = val.SortIndex - RollCount;
				Vector3 localPosition = ViewsDefaultLocalPosition[num];
				val.cachedTransform.localPosition = localPosition;
			}
		}

		private void ShowChildNext()
		{
			if (Models.Length - 1 < RollCount + CursolIndex + 1)
			{
				return;
			}
			CursolIndex++;
			if (CursolIndex < GetUserViewingLength())
			{
				if (mUseBottomUpReposition)
				{
					BottomUp();
				}
				View viewFromSortIndex = GetViewFromSortIndex(ViewFocus.SortIndex + 1);
				ChangeFocusView(viewFromSortIndex, isFirstFocus: false);
				return;
			}
			CursolIndex = GetUserViewingLength();
			RollCount++;
			View[] views = Views;
			for (int i = 0; i < views.Length; i++)
			{
				View val = views[i];
				int loopIndex = GetLoopIndex(val.SortIndex, GetChildrenViewsCount(), -RollCount);
				Vector3 localPosition = ViewsDefaultLocalPosition[loopIndex];
				if (loopIndex == GetLogicViewingLength() - 1)
				{
					int num = RollCount + GetLogicViewingLength() - 1;
					if (Models.Length <= num)
					{
						OnUpdateChild(num, val, (Model)null);
					}
					else
					{
						OnUpdateChild(num, val, Models[num]);
					}
				}
				if (loopIndex == GetUserViewingLength() - 1)
				{
					ChangeFocusView(val, isFirstFocus: false);
				}
				val.cachedTransform.localPosition = localPosition;
			}
		}

		private void ShowChildNextPage()
		{
			int num = RollCount / MAX_USER_VIEW_OBJECTS + ((0 < RollCount % MAX_USER_VIEW_OBJECTS) ? 1 : 0);
			int num2 = num + 1;
			int num3 = num2 * MAX_USER_VIEW_OBJECTS;
			if (num3 < GetModelSize() - MAX_USER_VIEW_OBJECTS)
			{
				RollCount = num3;
				OnInitialize(num3);
				if (mUseBottomUpReposition)
				{
					BottomDown();
				}
			}
			else
			{
				int num4 = GetModelSize() - MAX_USER_VIEW_OBJECTS;
				if (0 < num4)
				{
					OnInitialize(RollCount = num4);
				}
			}
		}

		private void ShowChildPrevPage()
		{
			int num = RollCount / MAX_USER_VIEW_OBJECTS + ((0 < RollCount % MAX_USER_VIEW_OBJECTS) ? 1 : 0);
			int num2 = num - 1;
			int num3 = num2 * MAX_USER_VIEW_OBJECTS;
			if (0 <= num3)
			{
				RollCount = num3;
				OnInitialize(num3);
			}
			if (mUseBottomUpReposition)
			{
				BottomDown();
			}
		}

		private void DestroyGridInChildren()
		{
			foreach (Transform child in mGridContaints.GetChildList())
			{
				NGUITools.Destroy(child);
			}
		}

		protected virtual void OnChildAction(ActionType actionType, UIScrollListChild<Model> actionChild)
		{
			if (mKeyController != null && actionType == ActionType.OnTouch && actionChild.Model != null && actionChild.mIsClickable)
			{
				ChangeFocusView((View)actionChild, isFirstFocus: false);
				OnAction(actionType, this, (View)actionChild);
			}
		}

		private View GetViewFromSortIndex(int sortIndex)
		{
			return Views[GetLoopIndex(sortIndex, GetLogicViewingLength(), 0)];
		}

		private View GetViewFromLiteralIndex(int literalIndex)
		{
			return Views[GetLoopIndex(RollCount, GetLogicViewingLength(), literalIndex)];
		}

		private void OnInitialize(int startModelIdx)
		{
			int num = startModelIdx % GetLogicViewingLength();
			for (int i = 0; i < GetLogicViewingLength(); i++)
			{
				View child = Views[i];
				int num2 = startModelIdx + i - num;
				if (i < num)
				{
					num2 += GetLogicViewingLength();
				}
				OnUpdateChild(num2, child, (num2 >= Models.Length) ? ((Model)null) : Models[num2]);
			}
			float childHeight = GetChildHeight();
			mVector3Top = new Vector3(0f, 0f, 0f);
			mVector3TopDestroyLine = new Vector3(0f, mVector3Top.y + childHeight, 0f);
			mVector3Bottom = new Vector3(0f, (0f - childHeight) * (float)GetLogicViewingLength(), 0f);
			mVector3BottomDestroyLine = new Vector3(0f, (0f - childHeight) * (float)(GetLogicViewingLength() - 1), 0f);
			mGridContaints.Reposition();
			RefreshPosition(0f, 0f);
			mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(UIDisplaySwipeEventRegionDelegate);
			if (0 < Models.Length && 0 <= num && num < Models.Length)
			{
				ChangeFocusView(Views[num], isFirstFocus: true);
			}
			View[] views = Views;
			for (int j = 0; j < views.Length; j++)
			{
				View val = views[j];
				val.cachedTransform.localPosition = ViewsDefaultLocalPosition[val.SortIndex - RollCount];
			}
			OnFinishedInitialize();
		}

		protected virtual void OnFinishedInitialize()
		{
		}

		protected void ChangeFocusView(View targetView, bool isFirstFocus)
		{
			if ((UnityEngine.Object)ViewFocus != (UnityEngine.Object)null && ViewFocus.Equals(targetView))
			{
				if (mKeyController != null)
				{
					ViewFocus.Hover();
				}
				return;
			}
			if ((UnityEngine.Object)ViewFocus != (UnityEngine.Object)null)
			{
				ViewFocus.RemoveHover();
			}
			ViewFocus = targetView;
			if ((UnityEngine.Object)ViewFocus != (UnityEngine.Object)null)
			{
				if (mKeyController == null)
				{
					ViewFocus.StopBlink();
				}
				else
				{
					ViewFocus.Hover();
				}
				CursolIndex = ViewFocus.SortIndex - RollCount;
				if (isFirstFocus)
				{
					OnAction(ActionType.OnChangeFirstFocus, this, ViewFocus);
				}
				else
				{
					OnAction(ActionType.OnChangeFocus, this, ViewFocus);
				}
			}
		}

		private void RefreshPosition(float distanceX, float distanceY)
		{
			if (distanceY < 0f && RollCount <= 0)
			{
				Relocation(distanceX, distanceY);
			}
			else if (0f < distanceY && GetModelSize() <= GetIndexUseViewingBottom() + 1)
			{
				Relocation(distanceX, distanceY);
			}
			else if (0f < distanceY)
			{
				RefreshUpSwipeedPostion(distanceX, distanceY);
			}
			else if (distanceY < 0f)
			{
				RefreshDownSwipedPosition(distanceX, distanceY);
			}
		}

		private void RefreshUpSwipeedPostion(float distanceX, float distanceY)
		{
			Relocation(distanceX, distanceY);
			View[] childrenViews = GetChildrenViews();
			View[] array = childrenViews;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				if (IsInDestroyArea(val, DestroyAreaType.Top))
				{
					RollCount++;
					int currentPage = Array.IndexOf(childrenViews, val);
					int loopIndex = GetLoopIndex(currentPage, GetLogicViewingLength(), -1);
					Vector3 localPosition = val.GenerateConnectToPosition(childrenViews[loopIndex], ConnectType.Bottom);
					val.cachedTransform.localPosition = localPosition;
					int num = RollCount + childrenViews.Length - 1;
					Model model = (Model)null;
					if (num < Models.Length)
					{
						model = Models[num];
					}
					OnUpdateChild(num, val, model);
				}
			}
		}

		private void RefreshDownSwipedPosition(float distanceX, float distanceY)
		{
			View[] childrenViews = GetChildrenViews();
			Relocation(distanceX, distanceY);
			int num = childrenViews.Length - 1;
			while (0 <= num)
			{
				View val = childrenViews[num];
				if (IsInDestroyArea(val, DestroyAreaType.Botton))
				{
					RollCount--;
					int currentPage = Array.IndexOf(childrenViews, val);
					int loopIndex = GetLoopIndex(currentPage, GetLogicViewingLength(), 1);
					Vector3 localPosition = val.GenerateConnectToPosition(childrenViews[loopIndex], ConnectType.Top);
					val.cachedTransform.localPosition = localPosition;
					int rollCount = RollCount;
					Model model = (Model)null;
					if (rollCount >= 0 && rollCount < Models.Length)
					{
						model = Models[rollCount];
					}
					OnUpdateChild(rollCount, val, model);
				}
				num--;
			}
		}

		private void RelocationWithAnimation()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			View[] childrenViews = GetChildrenViews();
			for (int i = 0; i < childrenViews.Length; i++)
			{
				View val = childrenViews[i];
				int loopIndex = GetLoopIndex(val.SortIndex, GetLogicViewingLength(), -RollCount);
				Tween t = val.cachedTransform.DOLocalMove(ViewsDefaultLocalPosition[loopIndex], 0.3f);
				sequence.Join(t);
			}
			AnimationViewPositionNow = true;
			sequence.OnComplete(delegate
			{
				if (mUseBottomUpReposition)
				{
					BottomDown();
				}
				AnimationViewPositionNow = false;
			});
		}

		private void Relocation(float distanceX, float distanceY)
		{
			View[] childrenViews = GetChildrenViews();
			for (int i = 0; i < childrenViews.Length; i++)
			{
				View val = childrenViews[i];
				Vector3 localPosition = val.cachedTransform.localPosition;
				float x = localPosition.x;
				Vector3 localPosition2 = val.cachedTransform.localPosition;
				float new_y = localPosition2.y + distanceY;
				Vector3 localPosition3 = val.cachedTransform.localPosition;
				float z = localPosition3.z;
				mVector3_RelocationCache.Set(x, new_y, z);
				val.cachedTransform.localPosition = mVector3_RelocationCache;
			}
		}

		private bool IsInDestroyArea(View target, DestroyAreaType type)
		{
			switch (type)
			{
			case DestroyAreaType.Top:
			{
				float y = mVector3TopDestroyLine.y;
				Vector3 localPosition2 = target.transform.localPosition;
				return y <= localPosition2.y;
			}
			case DestroyAreaType.Botton:
			{
				Vector3 localPosition = target.transform.localPosition;
				return localPosition.y < mVector3BottomDestroyLine.y;
			}
			default:
				return false;
			}
		}

		private void OnUpdateChild(int nextIndex, View child, Model model)
		{
			child.UpdateIndex(nextIndex);
			if (model != null)
			{
				bool clickable = OnCheck(CheckType.Clickable, this, model);
				child.Initialize(model, clickable);
				child.Show();
			}
			else
			{
				child.ReleaseModel();
				child.Hide();
			}
		}

		private int GetReliminaryLength()
		{
			return MAX_RELIMINARY_OBJECTS;
		}

		private int GetLogicViewingLength()
		{
			return MaxLogicViewObjects;
		}

		private int GetUserViewingLength()
		{
			return MAX_USER_VIEW_OBJECTS;
		}

		private int GetLoopIndex(int currentPage, int pageLength, int step)
		{
			return (currentPage + pageLength + step % pageLength) % pageLength;
		}

		private int GetIndexUseViewingTop()
		{
			return RollCount;
		}

		private int GetIndexLogicViewingTop()
		{
			return RollCount;
		}

		private int GetIndexUseViewingBottom()
		{
			if (GetUserViewingLength() < GetReliminaryLength())
			{
				return RollCount + (GetLogicViewingLength() - (GetLogicViewingLength() - GetUserViewingLength())) - 1;
			}
			return RollCount + (GetLogicViewingLength() - (GetLogicViewingLength() - GetUserViewingLength())) - 1;
		}

		private int GetIndexLogicViewingBottom()
		{
			return RollCount + GetChildrenViewsCount();
		}

		private float GetChildHeight()
		{
			return mGridContaints.cellHeight;
		}

		private View[] GetChildrenViews()
		{
			return Views;
		}

		private int GetChildrenViewsCount()
		{
			return GetChildrenViews().Length;
		}

		private void UIDisplaySwipeEventRegionDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (AnimationViewPositionNow || AnimationScrollNow || !EnableTouchControl)
			{
				return;
			}
			float childHeight = GetChildHeight();
			if (childHeight < Math.Abs(deltaY))
			{
				if (0.3f < deltaY)
				{
					deltaY = childHeight;
				}
				else if (deltaY < -0.3f)
				{
					deltaY = 0f - childHeight;
				}
			}
			switch (actionType)
			{
			case UIDisplaySwipeEventRegion.ActionType.Start:
				break;
			case UIDisplaySwipeEventRegion.ActionType.Moving:
				CursolIndex = 0;
				RefreshPosition(deltaX, deltaY);
				break;
			case UIDisplaySwipeEventRegion.ActionType.FingerUp:
			{
				RelocationWithAnimation();
				int loopIndex = GetLoopIndex(RollCount, GetLogicViewingLength(), 0);
				if (loopIndex >= 0 && loopIndex < Views.Length)
				{
					ChangeFocusView(Views[loopIndex], isFirstFocus: false);
				}
				break;
			}
			}
		}

		private void MoveByView(int moveCount)
		{
			if (!AnimationViewPositionNow && !AnimationScrollNow && 0 < moveCount)
			{
				AnimationScrollNow = true;
				SwipeToBottomCoroutine(moveCount, delegate
				{
					ChangeFocusView(Views[GetLoopIndex(RollCount, GetLogicViewingLength(), 0)], isFirstFocus: false);
				});
			}
			else if (!AnimationViewPositionNow && !AnimationScrollNow && moveCount < 0)
			{
				Debug.Log("Move By View else if ::" + moveCount);
				AnimationScrollNow = true;
				SwipeToTopCoroutine(Math.Abs(moveCount), delegate
				{
					ChangeFocusView(Views[GetLoopIndex(RollCount, GetLogicViewingLength(), 0)], isFirstFocus: false);
				});
			}
		}

		private void SwipeToTopCoroutine(int moveCount, Action onFinished)
		{
			int num = RollCount - moveCount;
			float num2 = 0f - GetChildHeight();
			while (num < RollCount && 0 < RollCount)
			{
				RefreshPosition(0f, num2 / 2f);
			}
			RelocationWithAnimation();
			AnimationScrollNow = false;
			onFinished?.Invoke();
		}

		private void SwipeToBottomCoroutine(int moveCount, Action onFinished)
		{
			int num = RollCount + moveCount;
			float childHeight = GetChildHeight();
			while (RollCount < num && RollCount + GetLogicViewingLength() <= GetModelSize())
			{
				RefreshPosition(0f, childHeight / 2f);
			}
			RelocationWithAnimation();
			AnimationScrollNow = false;
			onFinished?.Invoke();
		}

		protected virtual View GetViewPrefab()
		{
			return mPrefab_UIScrollListChild;
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			Models = null;
			ViewsDefaultLocalPosition = null;
			Views = null;
			ViewFocus = (View)null;
			mPrefab_UIScrollListChild = (View)null;
			mUIDisplaySwipeEventRegion = null;
			mGridContaints = null;
			mUIScrollListParentAction = null;
			mUIScrollListParentCheck = null;
			mKeyController = null;
		}
	}
}
