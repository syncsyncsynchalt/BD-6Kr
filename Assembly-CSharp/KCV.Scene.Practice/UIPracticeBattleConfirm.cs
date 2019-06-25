using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(UIButtonManager))]
	public class UIPracticeBattleConfirm : MonoBehaviour
	{
		private UIPanel mPanelThis;

		private UIButtonManager mButtonManager;

		[SerializeField]
		private DialogAnimation mDialogAnimation;

		[SerializeField]
		private UITexture mTexture_FriendDeckFlag;

		[SerializeField]
		private UITexture mTexture_TargetDeckFlag;

		[SerializeField]
		private UIPracticeBattleConfirmShipSlot[] mFriendUIPracticeBattleConfirmShipSlot;

		[SerializeField]
		private UIPracticeBattleConfirmShipSlot[] mTargetUIPracticeBattleConfirmShipSlot;

		[SerializeField]
		private UIButton mButton_Cancel;

		[SerializeField]
		private UIButton mButton_Start;

		[SerializeField]
		private UILabel mLabel_FriendDeckName;

		[SerializeField]
		private UILabel mLabel_TargetDeckName;

		private KeyControl mKeyController;

		private DeckModel mFriendDeck;

		private DeckModel mTargetDeck;

		private bool mMatchValid;

		private UIButton[] mButtonsFocasable;

		private Action mOnStartListener;

		private Action mOnCancelListener;

		private UIButton mFocusButton;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0f;
			mButtonManager = GetComponent<UIButtonManager>();
			mButtonManager.IndexChangeAct = delegate
			{
				int num = Array.IndexOf(mButtonsFocasable, mButtonManager.nowForcusButton);
				if (-1 < num)
				{
					ChangeFocusButton(mButtonManager.nowForcusButton, needSe: false);
				}
			};
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				int num = Array.IndexOf(mButtonsFocasable, mFocusButton);
				int num2 = num + 1;
				if (num2 < mButtonsFocasable.Length)
				{
					ChangeFocusButton(mButtonsFocasable[num2], needSe: true);
				}
			}
			else if (mKeyController.keyState[10].down)
			{
				int num3 = Array.IndexOf(mButtonsFocasable, mFocusButton);
				int num4 = num3 - 1;
				if (0 <= num4)
				{
					ChangeFocusButton(mButtonsFocasable[num4], needSe: true);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				OnSelectFocus();
			}
			else if (mKeyController.keyState[0].down)
			{
				OnCancel();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void SetOnStartListener(Action onStartListener)
		{
			mOnStartListener = onStartListener;
		}

		public void SetOnCancelListener(Action onCancelListener)
		{
			mOnCancelListener = onCancelListener;
		}

		public void OnTouchCancel()
		{
			ChangeFocusButton(mButton_Cancel, needSe: false);
			OnSelectFocus();
		}

		public void OnTouchStart()
		{
			ChangeFocusButton(mButton_Start, needSe: false);
			OnSelectFocus();
		}

		private void OnSelectFocus()
		{
			if (mFocusButton != null)
			{
				if (mFocusButton.Equals(mButton_Start))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					OnStartPractice();
				}
				else if (mFocusButton.Equals(mButton_Cancel))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
					OnCancel();
				}
			}
		}

		private void OnStartPractice()
		{
			if (mOnStartListener != null)
			{
				mOnStartListener();
			}
		}

		private void OnCancel()
		{
			if (mOnCancelListener != null)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				mOnCancelListener();
			}
		}

		public void Initialize(DeckModel friendDeckModel, DeckModel targetDeckModel, bool matchValid)
		{
			mFriendDeck = friendDeckModel;
			mTargetDeck = targetDeckModel;
			mMatchValid = matchValid;
			mLabel_FriendDeckName.text = mFriendDeck.Name;
			mLabel_TargetDeckName.text = mTargetDeck.Name;
			mLabel_FriendDeckName.supportEncoding = false;
			mLabel_TargetDeckName.supportEncoding = false;
			InitializeFriendBanners(mFriendDeck);
			InitializeTargetBanners(mTargetDeck);
			Texture2D mainTexture = Resources.Load($"Textures/Common/DeckFlag/icon_deck{friendDeckModel.Id}") as Texture2D;
			Texture2D mainTexture2 = Resources.Load($"Textures/Common/DeckFlag/icon_deck{targetDeckModel.Id}") as Texture2D;
			mTexture_FriendDeckFlag.mainTexture = mainTexture;
			mTexture_TargetDeckFlag.mainTexture = mainTexture2;
			List<UIButton> list = new List<UIButton>();
			list.Add(mButton_Cancel);
			if (mMatchValid)
			{
				mButton_Start.isEnabled = true;
				list.Add(mButton_Start);
			}
			else
			{
				mButton_Start.isEnabled = false;
			}
			mButtonsFocasable = list.ToArray();
			ChangeFocusButton(mButtonsFocasable[1], needSe: false);
		}

		private void ChangeFocusButton(UIButton targetButton, bool needSe)
		{
			if (mFocusButton != null)
			{
				if (mFocusButton.Equals(targetButton))
				{
					mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
					return;
				}
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

		private void InitializeFriendBanners(DeckModel deckModel)
		{
			InitializeBanners(deckModel, mFriendUIPracticeBattleConfirmShipSlot);
		}

		private void InitializeTargetBanners(DeckModel deckModel)
		{
			InitializeBanners(deckModel, mTargetUIPracticeBattleConfirmShipSlot);
		}

		private void InitializeBanners(DeckModel deckModel, UIPracticeBattleConfirmShipSlot[] banners)
		{
			for (int i = 0; i < banners.Length; i++)
			{
				if (i < deckModel.GetShipCount())
				{
					banners[i].Initialize(i + 1, deckModel.GetShip(i));
				}
				else
				{
					banners[i].InitializeDefault();
				}
			}
		}

		public void Show(Action onFinished)
		{
			ChangeFocusButton(mFocusButton, needSe: false);
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

		public void Hide(Action onFinished)
		{
			if (mDialogAnimation.IsOpen)
			{
				mDialogAnimation.CloseAction = delegate
				{
					mPanelThis.alpha = 0f;
					if (onFinished != null)
					{
						onFinished();
					}
				};
				mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, isOpen: false);
			}
		}

		private void OnDestroy()
		{
			mPanelThis = null;
			mButtonManager = null;
			mDialogAnimation = null;
			mTexture_FriendDeckFlag = null;
			mTexture_TargetDeckFlag = null;
			mFriendUIPracticeBattleConfirmShipSlot = null;
			mTargetUIPracticeBattleConfirmShipSlot = null;
			mButton_Cancel = null;
			mButton_Start = null;
			mLabel_FriendDeckName = null;
			mLabel_TargetDeckName = null;
			mKeyController = null;
			mFriendDeck = null;
			mTargetDeck = null;
			mButtonsFocasable = null;
		}
	}
}
