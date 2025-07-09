using KCV.Display;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.View.Scroll
{
	public abstract class UIScrollListParentNew<Model, View> : MonoBehaviour where Model : class where View : UIScrollListChildNew<Model, View>
	{
		protected Model[] models;

		private Vector3[] defaultViewPos;

		protected View[] views;

		private bool swipeBounceBackAnimating;

		public bool controllable;

		[SerializeField]
		[Tooltip("子要素のプレハブを設定してください。")]
		private View childPrefab;

		[SerializeField]
		[Tooltip("スワイプイベントを受け取る為のオブジェクト")]
		private UIDisplaySwipeEventRegion swipeEventRegion;

		[Tooltip("作った要素を入れる場所")]
		[SerializeField]
		private UIGrid container;

		[SerializeField]
		[Tooltip("ユーザが１度に見ることができる要素の最大数を設定してください。予備のオブジェクトの事は考えないでください")]
		private int MAX_USER_VIEW_OBJECTS = 6;

		[SerializeField]
		private int MAX_RELIMINARY_OBJECTS = 1;

		private int maxViewCount;

		private float topDestroyY;

		private float bottomDestroyY;

		protected KeyControl keyController;

		private int rollCount;

		private int cursorIndex;

		private float defaultIntervalTime;

		private Coroutine initCoroutine;

		private UIScrollListParentHandler<View> handler;

		protected View currentChild
		{
			get
			{
				UIToggle activeToggle = UIToggle.GetActiveToggle(getListGroupNo());
				return (!(activeToggle == null)) ? activeToggle.GetComponentInParent<View>() : ((View)null);
			}
		}

		public virtual int getListGroupNo()
		{
			return ((UnityEngine.Object)this).GetHashCode();
		}

		public void Init(Model[] models, UIScrollListParentHandler<View> handler)
		{
			Init(models, handler, 0);
		}

		public void Init(Model[] models, UIScrollListParentHandler<View> handler, int topIdx)
		{
			Debug.Log("ReplaceNow XD");
		}

		public virtual void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.keyController = keyController;
		}

		public void SetCamera(Camera camera)
		{
			swipeEventRegion.SetEventCatchCamera(camera);
		}

		protected void Update()
		{
			if (!controllable || keyController == null)
			{
				return;
			}
			if ((keyController.keyState[12].holdTime > 1.5f || keyController.keyState[8].holdTime > 1.5f) && keyController.KeyInputInterval == defaultIntervalTime)
			{
				keyController.KeyInputInterval /= 3f;
			}
			else
			{
				keyController.KeyInputInterval = defaultIntervalTime;
			}
			if (keyController.IsUpDown())
			{
				if (!swipeBounceBackAnimating)
				{
					MovePrev();
				}
			}
			else if (keyController.IsDownDown())
			{
				if (!swipeBounceBackAnimating)
				{
					MoveNext();
				}
			}
			else if (keyController.IsMaruDown())
			{
				if ((UnityEngine.Object)this.currentChild != (UnityEngine.Object)null)
				{
					View currentChild = this.currentChild;
					currentChild.OnClick();
				}
			}
			else if (keyController.IsBatuDown())
			{
				handler.OnCancel();
				BounceBack();
				View viewByModelIndex = GetViewByModelIndex(rollCount);
				viewByModelIndex.DoSelect();
			}
			else if (keyController.IsSankakuDown())
			{
				OnPressSankaku();
			}
		}

		protected virtual void OnPressSankaku()
		{
		}

		private void ProcessSelect()
		{
			if (0 < models.Length)
			{
				UIScrollListParentHandler<View> uIScrollListParentHandler = handler;
				View currentChild = this.currentChild;
				uIScrollListParentHandler.OnSelect(currentChild.modelIndex, this.currentChild);
			}
		}

		private void ProcessFocusChanging(View child)
		{
			handler.OnChangeFocus(child.modelIndex, child);
		}

		private void MovePrev()
		{
			if (rollCount == 0 && cursorIndex == 0)
			{
				return;
			}
			if (0 < cursorIndex)
			{
				cursorIndex--;
				View viewByModelIndex = GetViewByModelIndex(rollCount + cursorIndex);
				viewByModelIndex.DoSelect();
			}
			else
			{
				rollCount--;
				View[] array = views;
				for (int i = 0; i < array.Length; i++)
				{
					View child = array[i];
					if (child.modelIndex == rollCount + maxViewCount)
					{
						updateChild(rollCount, child, GetModel(rollCount));
						child.DoSelect();
					}
					child.transform.localPosition = GetDefaultViewPos(child.modelIndex - rollCount);
				}
			}
			ProcessFocusChanging(currentChild);
		}

		private void MoveNext()
		{
			if (models.Length <= rollCount + cursorIndex + 1)
			{
				return;
			}
			if (cursorIndex < MAX_USER_VIEW_OBJECTS - 1)
			{
				cursorIndex++;
				View viewByModelIndex = GetViewByModelIndex(rollCount + cursorIndex);
				viewByModelIndex.DoSelect();
			}
			else
			{
				cursorIndex = MAX_USER_VIEW_OBJECTS - 1;
				rollCount++;
				View[] array = views;
				for (int i = 0; i < array.Length; i++)
				{
					View child = array[i];
					int viewIndexByModelIndex = GetViewIndexByModelIndex(child.modelIndex - rollCount);
					if (viewIndexByModelIndex == maxViewCount - 1)
					{
						int num = rollCount + maxViewCount - 1;
						updateChild(num, child, GetModel(num));
					}
					if (viewIndexByModelIndex == MAX_USER_VIEW_OBJECTS - 1)
					{
						child.DoSelect();
					}
					child.transform.localPosition = defaultViewPos[viewIndexByModelIndex];
				}
			}
			ProcessFocusChanging(currentChild);
		}

		private void DestroyChildren()
		{
			foreach (Transform child in container.GetChildList())
			{
				NGUITools.Destroy(child);
			}
		}

		public void OnChildSelect(UIScrollListChildNew<Model, View> child)
		{
			if (controllable && keyController != null)
			{
				View currentChild = this.currentChild;
				cursorIndex = currentChild.modelIndex - rollCount;
				ProcessSelect();
			}
		}

		private void CalculateDefaultViewPos()
		{
			defaultViewPos = new Vector3[views.Length];
			int num = 0;
			View[] array = views;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				defaultViewPos[num++] = val.transform.localPosition;
			}
		}

		private IEnumerator InitCoroutine(int startModelIdx)
		{
			DestroyChildren();
			int tmpIdx = startModelIdx % maxViewCount;
			for (int index = 0; index < maxViewCount; index++)
			{
				View child = Util.Instantiate(childPrefab.gameObject, container.gameObject).GetComponent<View>();
				child.group = getListGroupNo();
				int modelIdx = startModelIdx + index - tmpIdx;
				if (index < tmpIdx)
				{
					modelIdx += maxViewCount;
				}
				updateChild(modelIdx, child, GetModel(modelIdx));
			}
			container.Reposition();
			views = container.GetComponentsInChildren<View>();
			CalculateDefaultViewPos();
			bottomDestroyY = (0f - (topDestroyY = GetChildHeight())) * (float)(maxViewCount - 1);
			container.Reposition();
			RefreshPosition(0f);
			swipeEventRegion.SetOnSwipeActionJudgeCallBack(OnSwipeAction);
			if (models.Length > 0)
			{
				views[0].DoSelect();
				cursorIndex = 0;
			}
			View[] array = views;
			for (int i = 0; i < array.Length; i++)
			{
				View child2 = array[i];
				child2.transform.localPosition = defaultViewPos[child2.modelIndex - rollCount];
			}
			yield return null;
		}

		private void RefreshPosition(float moveY)
		{
			if (moveY < 0f && rollCount <= 0)
			{
				Relocation(moveY);
			}
			else if (0f < moveY && models.Length <= rollCount + MAX_USER_VIEW_OBJECTS)
			{
				Relocation(moveY);
			}
			else if (0f < moveY)
			{
				RefreshUpSwipedPostion(moveY);
			}
			else if (moveY < 0f)
			{
				RefreshDownSwipedPosition(moveY);
			}
		}

		private void RefreshUpSwipedPostion(float moveY)
		{
			Relocation(moveY);
			View[] array = views;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				float num = topDestroyY;
				Vector3 localPosition = val.transform.localPosition;
				if (num <= localPosition.y)
				{
					rollCount++;
					int num2 = Array.IndexOf(views, val);
					int viewIndexByModelIndex = GetViewIndexByModelIndex(num2 - 1);
					Vector3 localPosition2 = views[viewIndexByModelIndex].transform.localPosition;
					val.UpdateLocalPosition(localPosition2.x, localPosition2.y - GetChildHeight(), localPosition2.z);
					int num3 = rollCount + views.Length - 1;
					Model model = (Model)null;
					if (num3 < models.Length)
					{
						model = models[num3];
					}
					updateChild(num3, val, model);
				}
			}
		}

		private void RefreshDownSwipedPosition(float moveY)
		{
			Relocation(moveY);
			int num = views.Length - 1;
			while (0 <= num)
			{
				View val = views[num];
				Vector3 localPosition = val.transform.localPosition;
				if (localPosition.y < bottomDestroyY)
				{
					rollCount--;
					int num2 = Array.IndexOf(views, val);
					int viewIndexByModelIndex = GetViewIndexByModelIndex(num2 + 1);
					Vector3 localPosition2 = views[viewIndexByModelIndex].transform.localPosition;
					val.UpdateLocalPosition(localPosition2.x, localPosition2.y + GetChildHeight(), localPosition2.z);
					updateChild(rollCount, val, GetModel(rollCount));
				}
				num--;
			}
		}

		private void BounceBack()
		{
			for (int i = 0; i < views.Length; i++)
			{
				View val = views[i];
				TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(val.gameObject, 0.3f);
				tweenPosition.from = val.transform.localPosition;
				tweenPosition.to = GetDefaultViewPos(val.modelIndex - rollCount);
				if (i == views.Length - 1)
				{
					swipeBounceBackAnimating = true;
					EventDelegate.Set(tweenPosition.onFinished, delegate
					{
						swipeBounceBackAnimating = false;
					});
				}
				tweenPosition.PlayForward();
			}
		}

		private void Relocation(float moveY)
		{
			View[] array = views;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				Transform transform = val.transform;
				Vector3 localPosition = val.transform.localPosition;
				float x = localPosition.x;
				Vector3 localPosition2 = val.transform.localPosition;
				float y = localPosition2.y + moveY;
				Vector3 localPosition3 = val.transform.localPosition;
				transform.localPosition = new Vector3(x, y, localPosition3.z);
			}
		}

		private void updateChild(int nextIndex, View child, Model model)
		{
			child.Init(this, model, nextIndex);
			if (model != null)
			{
				child.Show();
			}
			else
			{
				child.Hide();
			}
		}

		private View GetViewByModelIndex(int modelIdx)
		{
			return views[GetViewIndexByModelIndex(modelIdx)];
		}

		private Vector3 GetDefaultViewPos(int modelIdx)
		{
			return defaultViewPos[GetViewIndexByModelIndex(modelIdx)];
		}

		private int GetViewIndexByModelIndex(int modelIdx)
		{
			while (modelIdx < 0)
			{
				modelIdx += maxViewCount;
			}
			return modelIdx % maxViewCount;
		}

		private float GetChildHeight()
		{
			return container.cellHeight;
		}

		private void OnSwipeAction(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (controllable && !swipeBounceBackAnimating)
			{
				float num = deltaY;
				float childHeight = GetChildHeight();
				if (childHeight < num)
				{
					num = childHeight;
				}
				else if (num < 0f - childHeight)
				{
					num = 0f - childHeight;
				}
				switch (actionType)
				{
				case UIDisplaySwipeEventRegion.ActionType.Moving:
					cursorIndex = 0;
					RefreshPosition(num);
					break;
				case UIDisplaySwipeEventRegion.ActionType.FingerUp:
				{
					BounceBack();
					View viewByModelIndex = GetViewByModelIndex(rollCount);
					viewByModelIndex.DoSelect();
					break;
				}
				}
			}
		}

		private Model GetModel(int modelIdx)
		{
			if (modelIdx < 0 || modelIdx >= models.Length)
			{
				return (Model)null;
			}
			return models[modelIdx];
		}

		public void SetEnabled(bool enabled)
		{
			base.enabled = enabled;
			controllable = enabled;
			if (views != null)
			{
				for (int i = 0; i < views.Length; i++)
				{
					views[i].enabled = enabled;
				}
			}
		}

		public void SetFocus(bool focused)
		{
			bool enabled = base.enabled;
			SetEnabled(enabled: true);
			View currentChild = this.currentChild;
			if ((UnityEngine.Object)currentChild != (UnityEngine.Object)null)
			{
				if (focused)
				{
					currentChild.UpdateHover();
				}
				else
				{
					currentChild.HideHover();
				}
			}
			SetEnabled(enabled);
		}

		private void OnDestroy()
		{
			OnCallDestroy();
			models = null;
			defaultViewPos = null;
			if (views != null)
			{
				for (int i = 0; i < views.Length; i++)
				{
					if ((UnityEngine.Object)views[i] != (UnityEngine.Object)null)
					{
						views[i] = (View)null;
					}
				}
			}
			views = null;
			childPrefab = (View)null;
			swipeEventRegion = null;
			container = null;
			keyController = null;
			initCoroutine = null;
		}

		protected virtual void OnCallDestroy()
		{
		}
	}
}
