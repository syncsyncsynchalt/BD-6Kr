using KCV.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIPanel))]
	public class UIItemStoreBuyConfirm : MonoBehaviour
	{
		private UIPanel mPanelThis;

		private ItemStoreModel mModel;

		private ItemStoreManager mItemStoreCheckUtils;

		private KeyControl mKeyController;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UITexture mTexture_Icon;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UIItemAkashi mAkashi;

		[SerializeField]
		private DialogAnimation mDialogAnimation;

		private UIButton mFocusButton;

		private Action<ItemStoreModel, int> mOnBuyCallBack;

		private Action mOnBuyCancelCallBack;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0f;
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				ChangeFocus(mButton_Positive, needSe: true);
			}
			else if (mKeyController.keyState[10].down)
			{
				ChangeFocus(mButton_Negative, needSe: true);
			}
			else if (mKeyController.keyState[1].down)
			{
				if (mButton_Negative.Equals(mFocusButton))
				{
					BuyCancel();
				}
				else if (mButton_Positive.Equals(mFocusButton))
				{
					Buy();
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				BuyCancel();
			}
		}

		public void Initialize(ItemStoreModel itemStoreModel, ItemStoreManager checkUtils)
		{
			if (itemStoreModel != null)
			{
				mModel = itemStoreModel;
				mItemStoreCheckUtils = checkUtils;
				mLabel_Name.text = mModel.Name;
				mLabel_Price.text = mModel.Price.ToString();
				mTexture_Icon.mainTexture = UserInterfaceItemManager.RequestItemStoreIcon(mModel.MstId);
				ChangeFocus(mButton_Positive, needSe: false);
				mAkashi.SetKeyController(null);
			}
		}

		public void Show(Action onFinished)
		{
			mPanelThis.alpha = 1f;
			if (!mDialogAnimation.IsOpen)
			{
				mAkashi.Show();
				mDialogAnimation.OpenAction = delegate
				{
					if (onFinished != null)
					{
						onFinished();
					}
				};
				mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, isOpen: true);
			}
		}

		public void Close(Action onFinished)
		{
			if (mDialogAnimation.IsOpen)
			{
				mAkashi.Hide();
				mDialogAnimation.CloseAction = delegate
				{
					if (onFinished != null)
					{
						onFinished();
					}
					mPanelThis.alpha = 0f;
				};
				mDialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, isOpen: false);
			}
		}

		public void SetOnBuyStartCallBack(Action<ItemStoreModel, int> onBuyCallBack)
		{
			mOnBuyCallBack = onBuyCallBack;
		}

		private void OnBuy(ItemStoreModel itemStoreModel, int count)
		{
			if (mOnBuyCallBack != null)
			{
				mOnBuyCallBack(itemStoreModel, count);
			}
		}

		public void SetOnBuyCancelCallBack(Action onBuyCancel)
		{
			mOnBuyCancelCallBack = onBuyCancel;
		}

		private void OnBuyCancel()
		{
			if (mOnBuyCancelCallBack != null)
			{
				mOnBuyCancelCallBack();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void ChangeFocus(UIButton targetButton, bool needSe)
		{
			if (mFocusButton != null)
			{
				mFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
				if (mFocusButton == targetButton)
				{
					needSe = false;
				}
			}
			mFocusButton = targetButton;
			if (mFocusButton != null)
			{
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		public void OnClickPositive()
		{
			ChangeFocus(mButton_Positive, needSe: false);
			Buy();
		}

		private void BuyCancel()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			OnBuyCancel();
		}

		private void Buy()
		{
			int count = 1;
			if (mItemStoreCheckUtils.IsValidBuy(mModel.MstId, count))
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_004);
				OnBuy(mModel, count);
			}
		}

		public void OnClickNegative()
		{
			BuyCancel();
		}

		public void OnTouchOther()
		{
			BuyCancel();
		}

		public void Release()
		{
			mModel = null;
			mItemStoreCheckUtils = null;
			mFocusButton = null;
		}
	}
}
