using DG.Tweening;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Base
{
	[RequireComponent(typeof(UIPanel))]
	public class UICircleMenu<Model, View> : MonoBehaviour where View : UICircleCategory<Model>
	{
		[SerializeField]
		private View mPrefab_View;

		[SerializeField]
		private Transform mTransform_Categorys;

		[SerializeField]
		private Vector3[] mVector3DefaultPosition;

		[SerializeField]
		private int[] mDefaultDepths;

		protected View mCenterView;

		[SerializeField]
		private float ANIMATION_TIME_MOVE = 0.2f;

		[SerializeField]
		private Ease ANIMATION_EASE_MOVE = Ease.OutSine;

		private int mDisplayCategoryValue;

		private UIPanel mPanelThis;

		protected Model[] mCategories;

		public bool mIsAnimationNow
		{
			get;
			protected set;
		}

		public void Initialize(Model[] categories, int displayCategoryValue)
		{
			mCategories = categories;
			mDisplayCategoryValue = displayCategoryValue;
			int num = 0;
			foreach (Model category in categories)
			{
				View component = Util.Instantiate(mPrefab_View.gameObject, mTransform_Categorys.gameObject).GetComponent<View>();
				component.Initialize(num, category);
				num++;
			}
			View[] views = GetViews();
			InitDefaultPosition(views, 6f);
			InitDefaultDepth(views);
			int num2 = 0;
			View[] array = views;
			for (int j = 0; j < array.Length; j++)
			{
				View val = array[j];
				val.transform.localPosition = mVector3DefaultPosition[num2];
				num2++;
			}
			Reposition(0);
			mCenterView = views[0];
			View[] array2 = views;
			for (int k = 0; k < array2.Length; k++)
			{
				View obj = array2[k];
				if (mCenterView.Equals(obj))
				{
					obj.OnFirstDisplay(ANIMATION_TIME_MOVE, isCenter: true, Ease.OutBounce);
				}
				else
				{
					obj.OnFirstDisplay(ANIMATION_TIME_MOVE, isCenter: false, Ease.OutBounce);
				}
			}
			mIsAnimationNow = false;
		}

		private void InitDefaultPosition(View[] views, float radius)
		{
			mVector3DefaultPosition = new Vector3[views.Length];
			int num = views.Length / 2;
			for (int i = 0; i < views.Length; i++)
			{
				float f = (float)GetLoopIndex(i) * (float)Math.PI * 2f / (float)views.Length;
				float d = (float)(360 / views.Length) * radius;
				float x = Mathf.Sin(f);
				float y = 0f;
				float z = 0f;
				mVector3DefaultPosition[i] = new Vector3(x, y, z) * d;
			}
			InitOriginalDefaultPosition(ref mVector3DefaultPosition);
		}

		protected virtual void InitOriginalDefaultPosition(ref Vector3[] defaultPositions)
		{
		}

		private void InitDefaultDepth(View[] views)
		{
			mDefaultDepths = new int[views.Length];
			int num = views.Length / 2;
			for (int i = 0; i < views.Length; i++)
			{
				if (i != 0)
				{
					if (i <= num)
					{
						mDefaultDepths[i] = num - i;
					}
					else if (views.Length % 2 == 1 && i == num + 1)
					{
						mDefaultDepths[i] = 0;
					}
					else
					{
						mDefaultDepths[i] = num - (views.Length - i) % num;
					}
				}
				else
				{
					mDefaultDepths[i] = num;
				}
			}
		}

		public Vector3[] GetDefaultPositions()
		{
			return mVector3DefaultPosition;
		}

		public void Next()
		{
			View[] views = GetViews();
			int num = Array.IndexOf(views, mCenterView);
			int loopIndex = GetLoopIndex(num + 1);
			ChangeCenterView(views[loopIndex], needSe: true);
			Reposition(loopIndex);
		}

		public void Prev()
		{
			View[] views = GetViews();
			int num = Array.IndexOf(views, mCenterView);
			int loopIndex = GetLoopIndex(num - 1);
			ChangeCenterView(views[loopIndex], needSe: true);
			Reposition(loopIndex);
		}

		protected int GetLoopIndex(int value)
		{
			if (value == 0)
			{
				return 0;
			}
			if (value == -1)
			{
				return mCategories.Length - 1;
			}
			if (value < 0)
			{
				return mCategories.Length + value;
			}
			return value % mCategories.Length;
		}

		public void Show()
		{
			View[] views = GetViews();
			View[] array = views;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				val.Show();
			}
		}

		private void Reposition(int centerIndex)
		{
			View[] views = mTransform_Categorys.GetComponentsInChildren<View>();
			int num = views.Length / 2;
			int i;
			for (i = 0; i < views.Length; i++)
			{
				int nextDepth = mDefaultDepths[GetLoopIndex(i - centerIndex)];
				if (nextDepth < 1)
				{
					views[i].OnOutDisplay(ANIMATION_TIME_MOVE, ANIMATION_EASE_MOVE, delegate
					{
						views[i].SetDepth(nextDepth, addDefaultDepth: false);
					});
				}
				else if (views[i].GetDepth() < 1 && 1 <= nextDepth)
				{
					views[i].SetDepth(nextDepth, addDefaultDepth: false);
					views[i].OnInDisplay(ANIMATION_TIME_MOVE, ANIMATION_EASE_MOVE, delegate
					{
					});
				}
				OnMoveView(views[i], GetDefaultPositions()[GetLoopIndex(i - centerIndex)]);
			}
		}

		private void OnMoveView(View view, Vector3 moveTo)
		{
			mIsAnimationNow = true;
			view.transform.DOLocalMove(moveTo, ANIMATION_TIME_MOVE).SetEase(ANIMATION_EASE_MOVE).OnComplete(delegate
			{
				mIsAnimationNow = false;
			})
				.PlayForward();
		}

		private void ChangeCenterView(View nextCenterView, bool needSe)
		{
			if ((UnityEngine.Object)mCenterView != (UnityEngine.Object)null)
			{
				mCenterView.OnOtherThanCenter(ANIMATION_TIME_MOVE, ANIMATION_EASE_MOVE);
			}
			mCenterView = nextCenterView;
			if ((UnityEngine.Object)mCenterView != (UnityEngine.Object)null)
			{
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				mCenterView.OnCenter(ANIMATION_TIME_MOVE, ANIMATION_EASE_MOVE);
			}
		}

		public virtual void OnReposition(int nextCenterPosition, View[] views)
		{
		}

		public View[] GetViews()
		{
			return mTransform_Categorys.GetComponentsInChildren<View>();
		}
	}
}
