using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemListChildDetail : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Description;

		[SerializeField]
		private UISprite mSprite_Icon;

		[SerializeField]
		private UIButton mButton_Use;

		[SerializeField]
		private UIItemYousei mItemYousei;

		private KeyControl mKeyController;

		private Action<ItemlistModel> mUseCallBack;

		private Action mOnCalcelCallBack;

		private ItemlistModel mModel;

		private void Awake()
		{
			mSprite_Icon.spriteName = string.Empty;
		}

		public void UpdateInfo(ItemlistModel model)
		{
			mModel = model;
			if (Usable())
			{
				base.transform.SetActive(isActive: true);
				mLabel_Name.text = model.Name;
				mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(15, 1, model.Description);
				mSprite_Icon.spriteName = $"item_{mModel.MstId}";
				if (mModel.IsUsable())
				{
					mButton_Use.transform.SetActive(isActive: true);
				}
				else
				{
					mButton_Use.transform.SetActive(isActive: false);
				}
			}
			else
			{
				base.transform.SetActive(isActive: false);
			}
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[1].down)
				{
					OnUse();
				}
				else if (mKeyController.keyState[0].down)
				{
					OnCancel();
				}
			}
		}

		public void OnTouchUse()
		{
			OnUse();
		}

		public bool Usable()
		{
			return mModel != null && 0 < mModel.Count;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
			if (mKeyController != null)
			{
				mButton_Use.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		public void SetOnUseCallBack(Action<ItemlistModel> onUseCallBack)
		{
			mUseCallBack = onUseCallBack;
		}

		private void OnUse()
		{
			if (mUseCallBack != null)
			{
				mUseCallBack(mModel);
				mButton_Use.SetState(UIButtonColor.State.Normal, immediate: true);
			}
		}

		public void SetOnCancelCallBack(Action onCancelCallBack)
		{
			mOnCalcelCallBack = onCancelCallBack;
		}

		private void OnCancel()
		{
			if (mOnCalcelCallBack != null)
			{
				mOnCalcelCallBack();
				mButton_Use.SetState(UIButtonColor.State.Normal, immediate: true);
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Name);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Description);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Icon);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Use);
			mItemYousei = null;
			mKeyController = null;
			mUseCallBack = null;
			mOnCalcelCallBack = null;
			mModel = null;
		}

		internal void Clean()
		{
			mModel = null;
			mLabel_Name.text = string.Empty;
			mLabel_Description.text = string.Empty;
			mSprite_Icon.spriteName = string.Empty;
		}
	}
}
