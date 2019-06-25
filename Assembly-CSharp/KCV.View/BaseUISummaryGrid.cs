using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.View
{
	public class BaseUISummaryGrid<View, Model> : MonoBehaviour where View : BaseUISummary<Model> where Model : class
	{
		[SerializeField]
		private int MAX_VIEW_ITEMS = 5;

		private int mCurrentPageIndex;

		[SerializeField]
		private View mPrefab;

		[SerializeField]
		private UIGrid mTarget;

		private Model[] mModels;

		private Stack<Transform> mCacheListObjects;

		public virtual void Initialize(Model[] models)
		{
			mCacheListObjects = new Stack<Transform>(MAX_VIEW_ITEMS);
			mModels = models;
			ClearViews();
		}

		public void CreateView(Model[] models)
		{
			int num = 0;
			foreach (Model model in models)
			{
				Transform transform = null;
				if (0 < mCacheListObjects.Count)
				{
					transform = mCacheListObjects.Pop();
				}
				View val2 = (View)null;
				View val;
				if (transform == null)
				{
					val = GenerateView(mTarget, mPrefab, model);
				}
				else
				{
					transform.gameObject.SetActive(true);
					val = ((Component)transform).GetComponent<View>();
				}
				val.transform.parent = mTarget.transform;
				val.Initialize(num++, model);
			}
		}

		public void ClearViews()
		{
			List<Transform> childList = mTarget.GetChildList();
			foreach (Transform item in childList)
			{
				mCacheListObjects.Push(item);
				item.parent = null;
				item.gameObject.SetActive(false);
			}
			while (0 < mCacheListObjects.Count)
			{
				NGUITools.Destroy(mCacheListObjects.Pop().gameObject);
			}
		}

		public virtual View GenerateView(UIGrid target, View prefab, Model model)
		{
			return Util.Instantiate(prefab.gameObject, target.gameObject).GetComponent<View>();
		}

		public virtual void OnFinishedCreateViews()
		{
			View[] summaryViews = GetSummaryViews();
			mTarget.Reposition();
			View[] array = summaryViews;
			for (int i = 0; i < array.Length; i++)
			{
				View val = array[i];
				val.Show();
			}
		}

		public virtual View[] GetSummaryViews()
		{
			return mTarget.gameObject.GetComponentsInChildren<View>();
		}

		public virtual View GetSummaryView(int index)
		{
			View[] summaryViews = GetSummaryViews();
			if (summaryViews == null)
			{
				return (View)null;
			}
			if (index < 0)
			{
				return (View)null;
			}
			if (summaryViews.Length <= index)
			{
				return (View)null;
			}
			return summaryViews[index];
		}

		private Model[] GetPageInModels(int pageIndex)
		{
			return mModels.Skip(pageIndex * MAX_VIEW_ITEMS).Take(MAX_VIEW_ITEMS).ToArray();
		}

		private void ChangePage(int pageIndex)
		{
			mCurrentPageIndex = pageIndex;
			Model[] pageInModels = GetPageInModels(pageIndex);
			ClearViews();
			CreateView(pageInModels);
			mTarget.Reposition();
			OnFinishedCreateViews();
		}

		public int GetPageSize()
		{
			int num = mModels.Length;
			int num2 = num / MAX_VIEW_ITEMS;
			return num2 + ((num % MAX_VIEW_ITEMS != 0) ? 1 : 0);
		}

		public virtual bool GoToPage(int pageIndex)
		{
			if (pageIndex < 0)
			{
				return false;
			}
			if (GetPageSize() <= pageIndex)
			{
				return false;
			}
			ChangePage(pageIndex);
			return true;
		}

		public int GetCurrentPageIndex()
		{
			return mCurrentPageIndex;
		}

		public int GetCurrentViewCount()
		{
			return GetSummaryViews().Length;
		}

		private void OnDestroy()
		{
			mPrefab = (View)null;
			mTarget = null;
			mModels = null;
			mCacheListObjects = null;
		}
	}
}
