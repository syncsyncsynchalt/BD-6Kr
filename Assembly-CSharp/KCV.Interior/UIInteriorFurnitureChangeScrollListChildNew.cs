using KCV.View.ScrollView;
using System;
using UnityEngine;

namespace KCV.Interior
{
	[RequireComponent(typeof(UIWidget))]
	public class UIInteriorFurnitureChangeScrollListChildNew : MonoBehaviour, UIScrollListItem<UIInteriorFurnitureChangeScrollListChildModelNew, UIInteriorFurnitureChangeScrollListChildNew>
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private Transform mEquipMark;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private GameObject mBackground;

		private Transform mCachedTransform;

		private int mRealIndex;

		private UIInteriorFurnitureChangeScrollListChildModelNew mModel;

		private Action<UIInteriorFurnitureChangeScrollListChildNew> mOnTouchListener;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
		}

		public void Initialize(int realIndex, UIInteriorFurnitureChangeScrollListChildModelNew model)
		{
			mRealIndex = realIndex;
			mModel = model;
			mLabel_Name.text = model.GetName();
			if (model.IsConfiguredInDeck())
			{
				mEquipMark.SetActive(isActive: true);
			}
			else
			{
				mEquipMark.SetActive(isActive: false);
			}
			mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault(int realIndex)
		{
			mRealIndex = realIndex;
			mModel = null;
			mWidgetThis.alpha = 1E-07f;
		}

		public int GetRealIndex()
		{
			return mRealIndex;
		}

		public UIInteriorFurnitureChangeScrollListChildModelNew GetModel()
		{
			return mModel;
		}

		public int GetHeight()
		{
			return 88;
		}

		public void SetOnTouchListener(Action<UIInteriorFurnitureChangeScrollListChildNew> onTouchListener)
		{
			mOnTouchListener = onTouchListener;
		}

		public void Touch()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(mBackground, value: true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(mBackground, value: false);
		}

		public Transform GetTransform()
		{
			if (mCachedTransform == null)
			{
				mCachedTransform = base.transform;
			}
			return mCachedTransform;
		}

		private void OnDestroy()
		{
			mEquipMark = null;
			mLabel_Name = null;
			mBackground = null;
			mCachedTransform = null;
			mModel = null;
		}
	}
}
