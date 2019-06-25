using Common.Enum;
using KCV;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIItemExchangeMedalConfirm : MonoBehaviour
{
	[SerializeField]
	private DialogAnimation mDialogAnimation;

	[SerializeField]
	private UIButton mButton_Plan;

	[SerializeField]
	private UIButton mButton_Screw;

	[SerializeField]
	private UIButton mButton_Materials;

	private UIButton[] mFocasableButtons;

	private UIButton mFocusButton;

	private UIPanel mPanelThis;

	private ItemlistModel mModel;

	private ItemlistManager mItemlistCheckUtils;

	private KeyControl mKeyController;

	private Action<ItemExchangeKinds> mOnExchangeCallBack;

	private Action mOnCancelCallBack;

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
			int num = Array.IndexOf(mFocasableButtons, mFocusButton);
			int num2 = num - 1;
			if (0 <= num2)
			{
				ChangeFocus(mFocasableButtons[num2], needSe: true);
			}
		}
		else if (mKeyController.keyState[10].down)
		{
			int num3 = Array.IndexOf(mFocasableButtons, mFocusButton);
			int num4 = num3 + 1;
			if (num4 < mFocasableButtons.Length)
			{
				ChangeFocus(mFocasableButtons[num4], needSe: true);
			}
		}
		else if (mKeyController.keyState[1].down)
		{
			if (mButton_Plan.Equals(mFocusButton))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
				SelectExchange(ItemExchangeKinds.PLAN);
			}
			else if (mButton_Screw.Equals(mFocusButton))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
				SelectExchange(ItemExchangeKinds.REMODEL);
			}
			else if (mButton_Materials.Equals(mFocusButton))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
				SelectExchange(ItemExchangeKinds.NONE);
			}
		}
		else if (mKeyController.keyState[0].down)
		{
			CancelExchange();
		}
	}

	public void Initialize(ItemlistModel itemStoreModel, ItemlistManager checkUtils)
	{
		mModel = itemStoreModel;
		mItemlistCheckUtils = checkUtils;
		List<UIButton> list = new List<UIButton>();
		ItemlistModel listItem = checkUtils.GetListItem(57);
		if (listItem != null && 0 < listItem.Count)
		{
			if (4 <= listItem.Count)
			{
				mButton_Plan.isEnabled = true;
				list.Add(mButton_Plan);
			}
			else
			{
				mButton_Plan.isEnabled = false;
			}
			list.Add(mButton_Screw);
			list.Add(mButton_Materials);
			mFocasableButtons = list.ToArray();
			ChangeFocus(mFocasableButtons[0], needSe: false);
		}
		((Component)mButton_Plan.transform.parent.FindChild("Label_Message")).GetComponent<UILabel>().text = "「資源」や「改修資材」に交換出来ます。\nまた、勲章4個を「改装設計図」1枚に交換可能です。";
	}

	public void Show(Action onFinished)
	{
		mPanelThis.alpha = 1f;
		if (!mDialogAnimation.IsOpen)
		{
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
			mDialogAnimation.CloseAction = delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
				mPanelThis.alpha = 0f;
			};
			mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, isOpen: false);
		}
	}

	public void SetOnExchangeItemSelectedCallBack(Action<ItemExchangeKinds> onExchangeCallBack)
	{
		mOnExchangeCallBack = onExchangeCallBack;
	}

	private void SelectExchange(ItemExchangeKinds exchangeKinds)
	{
		if (mKeyController != null && mOnExchangeCallBack != null)
		{
			mOnExchangeCallBack(exchangeKinds);
		}
	}

	public void SetOnCancelCallBack(Action onCancelCallBack)
	{
		mOnCancelCallBack = onCancelCallBack;
	}

	private void OnCancel()
	{
		if (mKeyController != null && mOnCancelCallBack != null)
		{
			mOnCancelCallBack();
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

	public void OnTouchPlan()
	{
		if (0 <= Array.IndexOf(mFocasableButtons, mButton_Plan))
		{
			ChangeFocus(mButton_Plan, needSe: false);
			SelectExchange(ItemExchangeKinds.PLAN);
		}
	}

	public void OnTouchScrew()
	{
		if (0 <= Array.IndexOf(mFocasableButtons, mButton_Screw))
		{
			ChangeFocus(mButton_Screw, needSe: false);
			SelectExchange(ItemExchangeKinds.REMODEL);
		}
	}

	public void OnTouchMaterials()
	{
		if (0 <= Array.IndexOf(mFocasableButtons, mButton_Materials))
		{
			ChangeFocus(mButton_Materials, needSe: false);
			SelectExchange(ItemExchangeKinds.NONE);
		}
	}

	private void CancelExchange()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		OnCancel();
	}

	public void OnClickNegative()
	{
		ChangeFocus(null, needSe: false);
		CancelExchange();
	}

	public void Release()
	{
		mModel = null;
		mItemlistCheckUtils = null;
		mFocusButton = null;
		mFocasableButtons = null;
	}
}
