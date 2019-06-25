using DG.Tweening;
using KCV.Interior;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	[RequireComponent(typeof(UIButtonManager))]
	[RequireComponent(typeof(UIPanel))]
	public class UIFurniturePurchaseDialog : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Category;

		[SerializeField]
		private UITexture mTexture_Thumbnail;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private Transform[] mTransforms_Rate;

		[SerializeField]
		private UILabel mLabel_WorkerCount;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIButton mButton_Preview;

		private UIButton[] mFocasableButtons;

		private UIPanel mPanelThis;

		private UIButtonManager mButtonManager;

		private UIButton mButtonFocus;

		private Action mOnSelectNegativeListener;

		private Action mOnSelectPositiveListener;

		private Action mOnSelectPreviewListener;

		private KeyControl mKeyController;

		private FurnitureModel mFurnitureModel;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mButtonManager = GetComponent<UIButtonManager>();
			mButtonManager.IndexChangeAct = delegate
			{
				if (0 <= Array.IndexOf(mFocasableButtons, mButtonManager.nowForcusButton))
				{
					ChangeFocus(mButtonManager.nowForcusButton, needSe: false);
				}
			};
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchNegative()
		{
			OnSelectNegative();
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchPositive()
		{
			OnSelectPositive();
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchPreview()
		{
			OnSelectPreview();
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				int num = Array.IndexOf(mFocasableButtons, mButtonFocus);
				int num2 = num - 1;
				if (0 <= num2)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					ChangeFocus(mFocasableButtons[num2], needSe: true);
				}
			}
			else if (mKeyController.keyState[10].down)
			{
				int num3 = Array.IndexOf(mFocasableButtons, mButtonFocus);
				int num4 = num3 + 1;
				if (num4 < mFocasableButtons.Length)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					ChangeFocus(mFocasableButtons[num4], needSe: true);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				if (mButton_Negative.Equals(mButtonFocus))
				{
					OnSelectNegative();
				}
				else if (mButton_Positive.Equals(mButtonFocus))
				{
					OnSelectPositive();
				}
				else if (mButton_Preview.Equals(mButtonFocus))
				{
					OnSelectPreview();
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				OnSelectNegative();
			}
		}

		private void OnSelectNegative()
		{
			if (mOnSelectNegativeListener != null)
			{
				mOnSelectNegativeListener();
			}
		}

		public void SetOnSelectNegativeListener(Action onSelectNegativeListener)
		{
			mOnSelectNegativeListener = onSelectNegativeListener;
		}

		private void OnSelectPositive()
		{
			if (mOnSelectPositiveListener != null)
			{
				mOnSelectPositiveListener();
			}
		}

		public void SetOnSelectPositiveListener(Action onSelectPositiveListener)
		{
			mOnSelectPositiveListener = onSelectPositiveListener;
		}

		private void OnSelectPreview()
		{
			if (mOnSelectPreviewListener != null)
			{
				mOnSelectPreviewListener();
			}
		}

		public void SetOnSelectPreviewListener(Action onSelectPreviewListener)
		{
			mOnSelectPreviewListener = onSelectPreviewListener;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void Initialize(FurnitureModel furnitureModel, bool isValidBuy)
		{
			mFurnitureModel = furnitureModel;
			mLabel_Category.text = $"購入 - {UserInterfaceInteriorManager.FurnitureKindToString(furnitureModel.Type)} - ";
			List<UIButton> list = new List<UIButton>();
			list.Add(mButton_Negative);
			if (isValidBuy)
			{
				list.Add(mButton_Positive);
			}
			list.Add(mButton_Preview);
			mFocasableButtons = list.ToArray();
			mButtonManager.UpdateButtons(mFocasableButtons);
			ChangeFocus(mFocasableButtons[0], needSe: false);
			if (isValidBuy)
			{
				mButton_Positive.enabled = true;
				mButton_Positive.isEnabled = true;
				mButton_Positive.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			else
			{
				mButton_Positive.enabled = false;
				mButton_Positive.SetState(UIButtonColor.State.Disabled, immediate: true);
			}
			mTexture_Thumbnail.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(mFurnitureModel.Type, mFurnitureModel.MstId);
			mLabel_WorkerCount.text = ((!mFurnitureModel.IsNeedWorker()) ? "不要" : "必要");
			mLabel_Price.text = mFurnitureModel.Price.ToString();
			mLabel_Name.text = mFurnitureModel.Name;
			for (int i = 0; i < mTransforms_Rate.Length; i++)
			{
				if (i < mFurnitureModel.Rarity)
				{
					mTransforms_Rate[i].SetActive(isActive: true);
				}
				else
				{
					mTransforms_Rate[i].SetActive(isActive: false);
				}
			}
		}

		public void Show()
		{
			base.transform.DOScale(Vector3.one, 0.3f);
			DOVirtual.Float(mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			});
		}

		public void Hide()
		{
			base.transform.DOScale(Vector3.zero, 0.3f);
			DOVirtual.Float(mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			});
		}

		private void ChangeFocus(UIButton targetButton, bool needSe)
		{
			if (mButtonFocus != null)
			{
				mButtonFocus.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			mButtonFocus = targetButton;
			if (mButtonFocus != null)
			{
				mButtonFocus.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		public FurnitureModel GetModel()
		{
			return mFurnitureModel;
		}

		internal void ResumeFocus()
		{
			if (mButtonFocus != null)
			{
				mButtonFocus.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}
	}
}
