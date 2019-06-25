using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIPracticeMenu : MonoBehaviour
	{
		public enum SelectType
		{
			Back,
			DeckPractice,
			BattlePractice
		}

		[SerializeField]
		private UIButton mButton_BattlePractice;

		[SerializeField]
		private UIButton mButton_DeckPractice;

		[SerializeField]
		private Vector3 mVector3_SelectPosition;

		private Vector3 mVector3_DefaultPositionBattlePractice;

		private Vector3 mVector3_DefaultPositionDeckPractice;

		private UIButton mButtonTarget;

		private KeyControl mKeyController;

		private DeckModel mDeckModel;

		private int mDefaultDepth = 10000;

		private Action<SelectType> mMenuSelectedCallBack;

		[Obsolete("UIButtonのSerializeFieldに設定します.")]
		public void OnTouchDeckPractice()
		{
			ChangeFocusButton(mButton_DeckPractice, needSe: true);
			OnClickFocusTarget();
		}

		[Obsolete("UIButtonのSerializeFieldに設定します.")]
		public void OnTocuhBattlePractice()
		{
			ChangeFocusButton(mButton_BattlePractice, needSe: true);
			OnClickFocusTarget();
		}

		[Obsolete("UIButtonのSerializeFieldに設定します.")]
		public void OnTouchBack()
		{
			OnBack();
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
			if (mKeyController != null)
			{
				mButton_BattlePractice.SetEnableCollider2D(enabled: true);
				mButton_DeckPractice.SetEnableCollider2D(enabled: true);
				if (mButtonTarget != null)
				{
					ChangeFocusButton(mButtonTarget, needSe: false);
				}
			}
			else
			{
				mButton_BattlePractice.SetEnableCollider2D(enabled: false);
				mButton_DeckPractice.SetEnableCollider2D(enabled: false);
			}
		}

		public void Initialize(DeckModel currentDeckModel)
		{
			mDeckModel = currentDeckModel;
			ChangeFocusButton(mButton_DeckPractice, needSe: false);
		}

		public void SetOnSelectedCallBack(Action<SelectType> menuSelectedCallBack)
		{
			mMenuSelectedCallBack = menuSelectedCallBack;
		}

		private void ChangeFocusButton(UIButton focusTarget, bool needSe)
		{
			if (mButtonTarget != null)
			{
				mButtonTarget.GetSprite().depth = mDefaultDepth;
				mButtonTarget.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			mButtonTarget = focusTarget;
			if (mButtonTarget != null)
			{
				mButtonTarget.GetSprite().depth = mDefaultDepth + 1;
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				mButtonTarget.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		private void OnClickFocusTarget()
		{
			if (mButtonTarget != null)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				if (mButtonTarget.Equals(mButton_BattlePractice))
				{
					OnSelectedMenu(SelectType.BattlePractice);
				}
				else if (mButtonTarget.Equals(mButton_DeckPractice))
				{
					OnSelectedMenu(SelectType.DeckPractice);
				}
			}
		}

		private void OnBack()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			OnSelectedMenu(SelectType.Back);
		}

		private void OnSelectedMenu(SelectType selectType)
		{
			if (mMenuSelectedCallBack != null && mKeyController != null)
			{
				mMenuSelectedCallBack(selectType);
			}
		}

		private void Start()
		{
			mVector3_DefaultPositionBattlePractice = mButton_BattlePractice.transform.localPosition;
			mVector3_DefaultPositionDeckPractice = mButton_DeckPractice.transform.localPosition;
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[1].down)
				{
					OnClickFocusTarget();
				}
				else if (mKeyController.keyState[8].down || mKeyController.keyState[14].down)
				{
					ChangeFocusButton(mButton_DeckPractice, needSe: true);
				}
				else if (mKeyController.keyState[12].down || mKeyController.keyState[10].down)
				{
					ChangeFocusButton(mButton_BattlePractice, needSe: true);
				}
				else if (mKeyController.keyState[0].down)
				{
					OnBack();
				}
			}
		}

		public void MoveToButtonCenterFocus(Action onFinishedAnimation)
		{
			mButton_BattlePractice.transform.DOLocalMove(mVector3_SelectPosition, 0.3f).SetEase(Ease.OutCirc);
			mButton_DeckPractice.transform.DOLocalMove(mVector3_SelectPosition, 0.3f).OnComplete(delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation();
				}
			}).SetEase(Ease.OutCirc);
		}

		public void MoveToButtonDefaultFocus(Action onFinishedAnimation)
		{
			mButton_BattlePractice.transform.DOLocalMove(mVector3_DefaultPositionBattlePractice, 0.4f).SetEase(Ease.OutCirc);
			mButton_DeckPractice.transform.DOLocalMove(mVector3_DefaultPositionDeckPractice, 0.4f).OnComplete(delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation();
				}
			}).SetEase(Ease.OutCirc);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_BattlePractice);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_DeckPractice);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButtonTarget);
			mKeyController = null;
			mDeckModel = null;
			mMenuSelectedCallBack = null;
		}
	}
}
