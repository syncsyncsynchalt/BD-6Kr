using Common.Enum;
using DG.Tweening;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIButtonManager))]
	[RequireComponent(typeof(UIPanel))]
	public class UIPracticeDeckTypeSelect : MonoBehaviour
	{
		private UIPanel mPanelThis;

		private UIButtonManager mButtonManager;

		[SerializeField]
		private UIPracticeDeckTypeSelectChild[] mUIPracticeDeckTypeSelectChildrenAll;

		[SerializeField]
		private UIGrid mGridFocasable;

		[SerializeField]
		private Transform mTransform_TouchBackArea;

		[SerializeField]
		private Transform mTransform_ObjectPools;

		private UIPracticeDeckTypeSelectChild[] mUIPracticeDeckTypeSelectChildrenFocusable;

		private Tween mTweenShowHide;

		private Action<DeckPracticeType> mOnSelectedDeckPracticeTypeCallBack;

		private UIPracticeDeckTypeSelectChild mFocus;

		private Action mOnBackCallBack;

		private KeyControl mKeyController;

		private List<DeckPracticeType> mDeckPracticeTypes;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mButtonManager = GetComponent<UIButtonManager>();
			mButtonManager.IndexChangeAct = delegate
			{
				UIButton nowForcusButton = mButtonManager.nowForcusButton;
				ChangeFocus(nowForcusButton.GetComponent<UIPracticeDeckTypeSelectChild>());
			};
			mPanelThis.alpha = 0f;
		}

		public void OnDeckTypeSelect(UIPracticeDeckTypeSelectChild selectedView)
		{
			ChangeFocus(selectedView);
			OnSelectedDeckPracticeType(mFocus.GetDeckPracticeType());
		}

		private void ChangeFocus(UIPracticeDeckTypeSelectChild target)
		{
			if (mFocus != null)
			{
				mFocus.RemoveHover();
			}
			mFocus = target;
			if (mFocus != null)
			{
				mFocus.Hover();
			}
		}

		public void SetOnSelectedDeckPracticeTypeCallBack(Action<DeckPracticeType> OnSelectedDeckPracticeTypeCallBack)
		{
			mOnSelectedDeckPracticeTypeCallBack = OnSelectedDeckPracticeTypeCallBack;
		}

		public void SetOnBackCallBack(Action OnBackCallBack)
		{
			mOnBackCallBack = OnBackCallBack;
		}

		public void OnTouchBack()
		{
			OnBack();
		}

		private void OnBack()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			if (mOnBackCallBack != null)
			{
				mOnBackCallBack();
			}
		}

		private void OnSelectedDeckPracticeType(DeckPracticeType deckPracticeType)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			if (mOnSelectedDeckPracticeTypeCallBack != null)
			{
				mOnSelectedDeckPracticeTypeCallBack(deckPracticeType);
			}
		}

		public void SetKeyController(KeyControl keyControl)
		{
			mKeyController = keyControl;
			if (mKeyController != null)
			{
				mTransform_TouchBackArea.SetActive(isActive: true);
			}
			else
			{
				mTransform_TouchBackArea.SetActive(isActive: false);
			}
		}

		public void Initialize(Dictionary<DeckPracticeType, bool> selectableDeckPracticeTypes)
		{
			base.transform.localScale = Vector3.one;
			mGridFocasable.cellHeight = 0f;
			mUIPracticeDeckTypeSelectChildrenFocusable = null;
			mFocus = null;
			List<UIPracticeDeckTypeSelectChild> list = new List<UIPracticeDeckTypeSelectChild>();
			UIPracticeDeckTypeSelectChild[] array = mUIPracticeDeckTypeSelectChildrenAll;
			foreach (UIPracticeDeckTypeSelectChild uIPracticeDeckTypeSelectChild in array)
			{
				uIPracticeDeckTypeSelectChild.gameObject.SetActive(false);
				uIPracticeDeckTypeSelectChild.transform.localPosition = Vector3.zero;
				uIPracticeDeckTypeSelectChild.SetOnClickListener(null);
				DeckPracticeType deckPracticeType = uIPracticeDeckTypeSelectChild.GetDeckPracticeType();
				if (selectableDeckPracticeTypes.ContainsKey(deckPracticeType))
				{
					list.Add(uIPracticeDeckTypeSelectChild);
					uIPracticeDeckTypeSelectChild.gameObject.SetActive(true);
					uIPracticeDeckTypeSelectChild.transform.parent = mGridFocasable.transform;
					uIPracticeDeckTypeSelectChild.transform.localPosition = Vector3.zero;
					uIPracticeDeckTypeSelectChild.transform.localScale = Vector3.one;
					uIPracticeDeckTypeSelectChild.ParentHasChanged();
					uIPracticeDeckTypeSelectChild.SetOnClickListener(OnDeckTypeSelect);
				}
				else
				{
					uIPracticeDeckTypeSelectChild.transform.parent = mTransform_ObjectPools;
				}
			}
			mUIPracticeDeckTypeSelectChildrenFocusable = list.ToArray();
		}

		public void Show(Action onFinishedanimation)
		{
			ChangeFocus(mUIPracticeDeckTypeSelectChildrenFocusable[0]);
			if (mTweenShowHide != null && mTweenShowHide.IsPlaying())
			{
				mTweenShowHide.Kill();
			}
			mTweenShowHide = DOTween.Sequence().Append(DOVirtual.Float(mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			})).Join(DOVirtual.Float(0f, 72f, 0.4f, delegate(float cellHeight)
			{
				mGridFocasable.cellHeight = cellHeight;
				mGridFocasable.Reposition();
			}).SetEase(Ease.OutQuart))
				.OnComplete(delegate
				{
					if (onFinishedanimation != null)
					{
						onFinishedanimation();
					}
				});
		}

		public void Hide(Action onFinishedAnimation)
		{
			if (mTweenShowHide != null && mTweenShowHide.IsPlaying())
			{
				mTweenShowHide.Kill();
			}
			mTweenShowHide = DOVirtual.Float(mPanelThis.alpha, 0f, 0.2f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			});
			DOTween.Sequence().Append(mTweenShowHide).Join(base.transform.DOScale(new Vector3(0.9f, 0.9f), 0.4f));
			onFinishedAnimation?.Invoke();
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[8].down)
			{
				int num = Array.IndexOf(mUIPracticeDeckTypeSelectChildrenFocusable, mFocus);
				int num2 = num - 1;
				if (0 <= num2)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					ChangeFocus(mUIPracticeDeckTypeSelectChildrenFocusable[num2]);
				}
			}
			else if (mKeyController.keyState[12].down)
			{
				int num3 = Array.IndexOf(mUIPracticeDeckTypeSelectChildrenFocusable, mFocus);
				int num4 = num3 + 1;
				if (num4 < mUIPracticeDeckTypeSelectChildrenFocusable.Length)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					ChangeFocus(mUIPracticeDeckTypeSelectChildrenFocusable[num4]);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				OnDeckTypeSelect(mFocus);
			}
			else if (mKeyController.keyState[0].down)
			{
				OnBack();
			}
		}

		public void DisableButtonAll()
		{
			UIPracticeDeckTypeSelectChild[] array = mUIPracticeDeckTypeSelectChildrenAll;
			foreach (UIPracticeDeckTypeSelectChild uIPracticeDeckTypeSelectChild in array)
			{
				uIPracticeDeckTypeSelectChild.Enabled(isEnabled: false);
			}
		}
	}
}
