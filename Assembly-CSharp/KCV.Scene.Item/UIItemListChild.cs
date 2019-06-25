using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIWidget))]
	public class UIItemListChild : MonoBehaviour
	{
		[SerializeField]
		private UISprite mSprite_Icon;

		[SerializeField]
		private UILabel mLabel_Count;

		private UIWidget mWidgetThis;

		private Action<UIItemListChild> mOnTouchListener;

		public ItemlistModel mModel
		{
			get;
			private set;
		}

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mSprite_Icon.spriteName = string.Empty;
		}

		public void Initialize(ItemlistModel model)
		{
			mModel = model;
			mLabel_Count.text = mModel.Count.ToString();
			if (mModel != null && 0 < mModel.Count)
			{
				mSprite_Icon.SetActive(isActive: true);
				mLabel_Count.SetActive(isActive: true);
				mSprite_Icon.spriteName = $"item_{mModel.MstId}";
				mLabel_Count.text = mModel.Count.ToString();
			}
			else
			{
				mSprite_Icon.spriteName = string.Empty;
				mLabel_Count.text = string.Empty;
				mSprite_Icon.SetActive(isActive: false);
				mLabel_Count.SetActive(isActive: false);
			}
		}

		public bool IsFosable()
		{
			return mModel != null && 0 < mModel.Count;
		}

		public void Focus()
		{
			NGUITools.AdjustDepth(base.transform.gameObject, 1);
		}

		public void RemoveFocus()
		{
			NGUITools.AdjustDepth(base.transform.gameObject, -1);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Icon);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Count);
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
			mModel = null;
			mOnTouchListener = null;
		}

		public void SetOnTouchListener(Action<UIItemListChild> onClickListener)
		{
			mOnTouchListener = onClickListener;
		}

		public void OnTouchItemListChild()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}
	}
}
