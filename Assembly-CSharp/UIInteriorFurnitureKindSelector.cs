using Common.Enum;
using DG.Tweening;
using KCV;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager))]
public class UIInteriorFurnitureKindSelector : MonoBehaviour
{
	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Hangings;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Window;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Wall;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Chest;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Floor;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Desk;

	private UIInteriorMenuButton[] mFocasableUIInteriorMenuButtons;

	private UIInteriorMenuButton mFocusUIInteriorMenuButton;

	private UIButtonManager mButtonManager;

	private KeyControl mKeyController;

	private Action<FurnitureKinds> mOnSelectFurnitureKind;

	private Action mOnSelectCancelListener;

	private void Awake()
	{
		mButtonManager = GetComponent<UIButtonManager>();
	}

	public void Initialize()
	{
		DOTween.Kill(this);
		List<UIInteriorMenuButton> list = new List<UIInteriorMenuButton>();
		mUIInteriorMenuButton_Hangings.Initialize(FurnitureKinds.Hangings);
		mUIInteriorMenuButton_Window.Initialize(FurnitureKinds.Window);
		mUIInteriorMenuButton_Wall.Initialize(FurnitureKinds.Wall);
		mUIInteriorMenuButton_Chest.Initialize(FurnitureKinds.Chest);
		mUIInteriorMenuButton_Floor.Initialize(FurnitureKinds.Floor);
		mUIInteriorMenuButton_Desk.Initialize(FurnitureKinds.Desk);
		list.Add(mUIInteriorMenuButton_Hangings);
		list.Add(mUIInteriorMenuButton_Window);
		list.Add(mUIInteriorMenuButton_Wall);
		list.Add(mUIInteriorMenuButton_Desk);
		list.Add(mUIInteriorMenuButton_Floor);
		list.Add(mUIInteriorMenuButton_Chest);
		mFocasableUIInteriorMenuButtons = list.ToArray();
		UIInteriorMenuButton[] array = mFocasableUIInteriorMenuButtons;
		foreach (UIInteriorMenuButton uIInteriorMenuButton in array)
		{
			uIInteriorMenuButton.SetOnClickListener(OnClickMenuListener);
		}
		mButtonManager.IndexChangeAct = delegate
		{
			UIInteriorMenuButton uiInteriorMenuButton = mFocasableUIInteriorMenuButtons[mButtonManager.nowForcusIndex];
			ChangeFocus(uiInteriorMenuButton, needSe: false);
		};
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	private void Update()
	{
		if (mKeyController == null)
		{
			return;
		}
		if (mKeyController.keyState[1].down)
		{
			mFocusUIInteriorMenuButton.Click();
		}
		else if (mKeyController.keyState[0].down)
		{
			OnSelectCancel();
		}
		else if (mKeyController.keyState[14].down)
		{
			int num = Array.IndexOf(mFocasableUIInteriorMenuButtons, mFocusUIInteriorMenuButton);
			int num2 = num - 1;
			if (0 <= num2)
			{
				ChangeFocus(mFocasableUIInteriorMenuButtons[num2], needSe: true);
			}
		}
		else if (mKeyController.keyState[10].down)
		{
			int num3 = Array.IndexOf(mFocasableUIInteriorMenuButtons, mFocusUIInteriorMenuButton);
			int num4 = num3 + 1;
			if (num4 < mFocasableUIInteriorMenuButtons.Length)
			{
				ChangeFocus(mFocasableUIInteriorMenuButtons[num4], needSe: true);
			}
		}
		else if (mKeyController.keyState[8].down)
		{
			int num5 = Array.IndexOf(mFocasableUIInteriorMenuButtons, mFocusUIInteriorMenuButton);
			int num6 = num5 - 3;
			if (0 <= num6)
			{
				ChangeFocus(mFocasableUIInteriorMenuButtons[num6], needSe: true);
			}
		}
		else if (mKeyController.keyState[12].down)
		{
			int num7 = Array.IndexOf(mFocasableUIInteriorMenuButtons, mFocusUIInteriorMenuButton);
			int num8 = num7 + 3;
			if (num8 < mFocasableUIInteriorMenuButtons.Length)
			{
				ChangeFocus(mFocasableUIInteriorMenuButtons[num8], needSe: true);
			}
		}
	}

	public void StartState()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		Sequence s = DOTween.Sequence().SetId(this);
		UIInteriorMenuButton[] array = mFocasableUIInteriorMenuButtons;
		foreach (UIInteriorMenuButton uIInteriorMenuButton in array)
		{
			uIInteriorMenuButton.transform.localScale = new Vector3(0f, 0f);
			Tween t = uIInteriorMenuButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutCirc).SetId(this);
			t.SetDelay(0.075f);
			s.Join(t);
		}
		UIInteriorMenuButton[] array2 = mFocasableUIInteriorMenuButtons;
		foreach (UIInteriorMenuButton uIInteriorMenuButton2 in array2)
		{
			uIInteriorMenuButton2.SetEnableButton(enable: true);
		}
		ChangeFocus(mFocasableUIInteriorMenuButtons[0], needSe: false);
	}

	public void ResumeState()
	{
		UIInteriorMenuButton[] array = mFocasableUIInteriorMenuButtons;
		foreach (UIInteriorMenuButton uIInteriorMenuButton in array)
		{
			uIInteriorMenuButton.SetEnableButton(enable: true);
		}
		ChangeFocus(mFocusUIInteriorMenuButton, needSe: false);
	}

	private void OnClickMenuListener()
	{
		mKeyController.ClearKeyAll();
		mKeyController.firstUpdate = true;
		UIInteriorMenuButton[] array = mFocasableUIInteriorMenuButtons;
		foreach (UIInteriorMenuButton uIInteriorMenuButton in array)
		{
			uIInteriorMenuButton.SetEnableButton(enable: false);
		}
		SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		OnSelectFurnitureKind(mFocusUIInteriorMenuButton.mFurnitureKind);
	}

	private void ChangeFocus(UIInteriorMenuButton uiInteriorMenuButton, bool needSe)
	{
		if (mFocusUIInteriorMenuButton != null)
		{
			mFocusUIInteriorMenuButton.RemoveFocus();
		}
		mFocusUIInteriorMenuButton = uiInteriorMenuButton;
		if (mFocusUIInteriorMenuButton != null)
		{
			if (needSe)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			mFocusUIInteriorMenuButton.Focus();
		}
	}

	private void OnSelectFurnitureKind(FurnitureKinds furnitureKind)
	{
		if (mOnSelectFurnitureKind != null)
		{
			mOnSelectFurnitureKind(furnitureKind);
		}
	}

	public void SetOnSelectFurnitureKindListener(Action<FurnitureKinds> onSelectFurnitureKind)
	{
		mOnSelectFurnitureKind = onSelectFurnitureKind;
	}

	public void SetOnSelectCancelListener(Action onSelectCancelListener)
	{
		mOnSelectCancelListener = onSelectCancelListener;
	}

	private void OnSelectCancel()
	{
		if (mOnSelectCancelListener != null)
		{
			mOnSelectCancelListener();
		}
	}

	private void OnDestroy()
	{
		DOTween.Kill(this);
		for (int i = 0; i < mFocasableUIInteriorMenuButtons.Length; i++)
		{
			mFocasableUIInteriorMenuButtons[i] = null;
		}
		mUIInteriorMenuButton_Hangings = null;
		mUIInteriorMenuButton_Window = null;
		mUIInteriorMenuButton_Wall = null;
		mUIInteriorMenuButton_Chest = null;
		mUIInteriorMenuButton_Floor = null;
		mUIInteriorMenuButton_Desk = null;
		mFocasableUIInteriorMenuButtons = null;
		mFocusUIInteriorMenuButton = null;
		mButtonManager = null;
	}
}
