using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIWidget))]
	public class UIItemStoreChild : MonoBehaviour, UIScrollListItem<ItemStoreModel, UIItemStoreChild>
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Description;

		[SerializeField]
		private UITexture mTexture_Icon;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UITexture mTexture_Background;

		private int mRealIndex;

		private ItemStoreModel mItemStoreModel;

		[SerializeField]
		[Header("DEBUG")]
		private int msterId;

		private Action<UIItemStoreChild> mOnTouchListener;

		private Transform mTransform;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 1E-07f;
		}

		public void Release()
		{
			mLabel_Name.text = string.Empty;
			mLabel_Price.text = string.Empty;
			mLabel_Description.text = string.Empty;
			mTexture_Icon.mainTexture = null;
		}

		public void Initialize(int realIndex, ItemStoreModel model)
		{
			msterId = model.MstId;
			mItemStoreModel = model;
			mRealIndex = realIndex;
			mLabel_Name.text = model.Name;
			mLabel_Price.text = model.Price.ToString();
			mLabel_Description.text = model.Description;
			mTexture_Icon.mainTexture = UserInterfaceItemManager.RequestItemStoreIcon(mItemStoreModel.MstId);
			if (model.Count == 0)
			{
				mWidgetThis.alpha = 0.45f;
			}
			else
			{
				mWidgetThis.alpha = 1f;
			}
		}

		public void InitializeDefault(int realIndex)
		{
			mRealIndex = realIndex;
			mItemStoreModel = null;
			mLabel_Name.text = string.Empty;
			mLabel_Price.text = string.Empty;
			mLabel_Description.text = string.Empty;
			mTexture_Icon.mainTexture = null;
			mWidgetThis.alpha = 1E-07f;
		}

		public int GetRealIndex()
		{
			return mRealIndex;
		}

		public ItemStoreModel GetModel()
		{
			return mItemStoreModel;
		}

		public int GetHeight()
		{
			return 108;
		}

		public void SetOnTouchListener(Action<UIItemStoreChild> onTouchListener)
		{
			mOnTouchListener = onTouchListener;
		}

		[Obsolete("Inspector上で設定して使用します")]
		private void OnClick()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(mTexture_Background.gameObject, value: true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(mTexture_Background.gameObject, value: false);
		}

		public Transform GetTransform()
		{
			if (mTransform == null)
			{
				mTransform = base.transform;
			}
			return mTransform;
		}

		private void OnDestroy()
		{
			if (mTexture_Icon != null)
			{
				mTexture_Icon.mainTexture = null;
			}
			if (mLabel_Name != null)
			{
				mLabel_Name.text = string.Empty;
			}
			if (mLabel_Description != null)
			{
				mLabel_Description.text = string.Empty;
			}
			if (mLabel_Price != null)
			{
				mLabel_Price.text = string.Empty;
			}
			if (mWidgetThis != null)
			{
				mWidgetThis.RemoveFromPanel();
			}
			if (mTexture_Background != null)
			{
				mTexture_Background.mainTexture = null;
			}
			mWidgetThis = null;
			mLabel_Name = null;
			mLabel_Description = null;
			mTexture_Icon = null;
			mLabel_Price = null;
			mTexture_Background = null;
		}
	}
}
