using System;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Display
{
	[RequireComponent(typeof(BoxCollider))]
	public class UIDisplaySwipeEventRegion : MonoBehaviour
	{
		public enum ActionType
		{
			None,
			Start,
			Moving,
			FingerUp,
			FingerUpWithVerticalFlick
		}

		public delegate void SwipeJudgeDelegate(ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime);

		[SerializeField]
		private Camera mCamera;

		private SwipeJudgeDelegate mSwipeActionJudgeCallBack;

		private BoxCollider mBoxCollider;

		private Transform mTransformCache;

		private Vector3 mVector3SwipeMoved = Vector3.zero;

		private int mCurrentDragIndex = -1;

		private Vector3 mLastSwipeStartWorldPosition = Vector3.zero;

		private Stopwatch mStopWatch;

		private bool mNeedFlickCheck;

		private SwipeJudgeDelegate mOnSwipeListener;

		private Vector3 mOnDraggingScreenToWorldPointCache = Vector3.zero;

		private Vector3 mMovedPercentage = Vector3.zero;

		public bool isDraging
		{
			get;
			private set;
		}

		public void ResetTouch()
		{
			isDraging = false;
		}

		private void Awake()
		{
			mBoxCollider = GetComponent<BoxCollider>();
			mTransformCache = base.transform;
			mStopWatch = new Stopwatch();
			if (mCamera != null)
			{
				UICamera component = mCamera.GetComponent<UICamera>();
				if (component != null)
				{
					component.allowMultiTouch = false;
				}
			}
		}

		private void OnEnable()
		{
			isDraging = false;
			IT_Gesture.onDraggingStartE += OnDraggingStart;
			IT_Gesture.onDraggingE += OnDragging;
			IT_Gesture.onDraggingEndE += OnDraggingEnd;
		}

		private void OnDisable()
		{
			isDraging = false;
			IT_Gesture.onDraggingStartE -= OnDraggingStart;
			IT_Gesture.onDraggingE -= OnDragging;
			IT_Gesture.onDraggingEndE -= OnDraggingEnd;
		}

		public void SetEventCatchCamera(Camera camera)
		{
			mCamera = camera;
			if (mCamera != null)
			{
				UICamera component = mCamera.GetComponent<UICamera>();
				if (component != null)
				{
					component.allowMultiTouch = false;
				}
			}
		}

		public void SetOnSwipeActionJudgeCallBack(SwipeJudgeDelegate swipeActionJudgeCallBack)
		{
			SetOnSwipeActionJudgeCallBack(swipeActionJudgeCallBack, needFlickCheck: false);
		}

		public void SetOnSwipeActionJudgeCallBack(SwipeJudgeDelegate swipeActionJudgeCallBack, bool needFlickCheck)
		{
			mNeedFlickCheck = needFlickCheck;
			mSwipeActionJudgeCallBack = swipeActionJudgeCallBack;
		}

		public void SetOnSwipeListener(SwipeJudgeDelegate onSwipeListener)
		{
			mOnSwipeListener = onSwipeListener;
		}

		private void OnDraggingStart(DragInfo dragInfo)
		{
			if ((!SingletonMonoBehaviour<UIShortCutMenu>.exist() || !SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus) && mCamera != null)
			{
				Ray ray = mCamera.ScreenPointToRay(dragInfo.pos);
				RaycastHit raycastHit = default(RaycastHit);

                throw new NotImplementedException("‚È‚É‚±‚ê");
                //if (Physics.Raycast(ray, raycastHit, float.PositiveInfinity) && raycastHit.collider.gameObject.transform.Equals(base.transform) && raycastHit.collider.transform == mTransformCache)
				//{
				//	mVector3SwipeMoved.Set(0f, 0f, 0f);
				//	mStopWatch.Reset();
				//	mStopWatch.Start();
				//	isDraging = true;
				//	OnSwipeEventAction(ActionType.Start, dragInfo.delta.x, dragInfo.delta.y, 0f, 0f, 0f);
				//	mLastSwipeStartWorldPosition.Set(dragInfo.pos.x, dragInfo.pos.y, 0f);
				//	mCurrentDragIndex = dragInfo.index;
				//}
			}
		}

		private void OnDragging(DragInfo dragInfo)
		{
			if (isDraging && mCamera != null && dragInfo.index == mCurrentDragIndex)
			{
				mOnDraggingScreenToWorldPointCache.Set(dragInfo.pos.x, dragInfo.pos.y, 0f);
				mCamera.ScreenToWorldPoint(mOnDraggingScreenToWorldPointCache);
				mVector3SwipeMoved.Set(mVector3SwipeMoved.x + dragInfo.delta.x, mVector3SwipeMoved.y - dragInfo.delta.y, 0f);
				Vector3 vector = GenerateMovedPercentage(mVector3SwipeMoved.x, mVector3SwipeMoved.y);
				OnSwipeEventAction(ActionType.Moving, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, mStopWatch.ElapsedMilliseconds);
			}
		}

		private void OnDraggingEnd(DragInfo dragInfo)
		{
			if (isDraging && mCamera != null)
			{
				mStopWatch.Stop();
				Vector3 vector = GenerateMovedPercentage(mVector3SwipeMoved.x, mVector3SwipeMoved.y);
				OnSwipe(ActionType.FingerUp, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, mStopWatch.ElapsedMilliseconds);
				if (mNeedFlickCheck && dragInfo.isFlick)
				{
					if (Math.Abs(vector.x) < Math.Abs(vector.y))
					{
						OnSwipeEventAction(ActionType.FingerUpWithVerticalFlick, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, mStopWatch.ElapsedMilliseconds);
					}
					else
					{
						OnSwipeEventAction(ActionType.FingerUp, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, mStopWatch.ElapsedMilliseconds);
					}
				}
				else
				{
					OnSwipeEventAction(ActionType.FingerUp, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, mStopWatch.ElapsedMilliseconds);
				}
				if (dragInfo.index == mCurrentDragIndex)
				{
					mCurrentDragIndex = -1;
				}
				mVector3SwipeMoved.Set(0f, 0f, 0f);
			}
			isDraging = false;
		}

		private void OnSwipeEventAction(ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (mSwipeActionJudgeCallBack != null)
			{
				mSwipeActionJudgeCallBack(actionType, deltaX, deltaY, movedPercentageX, movedPercentageY, elapsedTime);
			}
		}

		private void OnSwipe(ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (mOnSwipeListener != null)
			{
				mOnSwipeListener(actionType, deltaX, deltaY, movedPercentageX, movedPercentageY, elapsedTime);
			}
		}

		private Vector3 GenerateMovedPercentage(float toX, float toY)
		{
			Vector3 size = mBoxCollider.size;
			float x = size.x;
			Vector3 size2 = mBoxCollider.size;
			float y = size2.y;
			float num = 0f;
			if (toX != 0f)
			{
				num = Math.Abs(toX) / x;
				if (toX < 0f)
				{
					num = 0f - num;
				}
			}
			float num2 = 0f;
			if (toY != 0f)
			{
				num2 = Math.Abs(toY) / y;
				if (toY < 0f)
				{
					num2 = 0f - num2;
				}
			}
			mMovedPercentage.x = num;
			mMovedPercentage.y = num2;
			return mMovedPercentage;
		}

		internal void Release()
		{
			mCamera = null;
			mSwipeActionJudgeCallBack = null;
			mBoxCollider = null;
			mTransformCache = null;
			if (mStopWatch != null)
			{
				mStopWatch.Stop();
			}
			mStopWatch = null;
		}

		private void OnDestroy()
		{
			mCamera = null;
			mSwipeActionJudgeCallBack = null;
			mBoxCollider = null;
			mTransformCache = null;
			if (mStopWatch != null)
			{
				mStopWatch.Stop();
			}
			mStopWatch = null;
		}
	}
}
