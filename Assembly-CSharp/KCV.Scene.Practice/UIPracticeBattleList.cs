using Common.Enum;
using DG.Tweening;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIButtonManager))]
	[RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleList : MonoBehaviour
	{
		private enum AnimationType
		{
			ShowHide
		}

		private UIWidget mWidgetThis;

		[SerializeField]
		private UIPracticeBattleListChild[] mUIPracticeBattleListChildren_PracticeTargetAll;

		[SerializeField]
		private UIGrid mGrid_Focasable;

		[SerializeField]
		private Transform mTransform_TouchBackArea;

		[SerializeField]
		private Transform mTransform_ObjectPool;

		private UIPracticeBattleListChild[] mUIPracticeBattleListChildren_Focasable;

		private List<DeckModel> mRivalDecks;

		private UIButtonManager mUIButtonManager;

		private Action<DeckModel, List<IsGoCondition>> mOnSelectedDeckListener;

		private UIPracticeBattleListChild mFocus;

		private Action mOnBackCallBack;

		private KeyControl mKeyController;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mUIButtonManager = GetComponent<UIButtonManager>();
			mUIButtonManager.IndexChangeAct = delegate
			{
				UIPracticeBattleListChild component = ((Component)mUIButtonManager.nowForcusButton.transform.parent).GetComponent<UIPracticeBattleListChild>();
				ChangeFocus(component, needSe: false);
			};
			mWidgetThis.alpha = 0f;
		}

		public void Initialize(List<DeckModel> rivalDecks, PracticeManager deckCheckUtil)
		{
			mRivalDecks = rivalDecks;
			base.transform.localScale = Vector3.one;
			List<UIPracticeBattleListChild> list = new List<UIPracticeBattleListChild>();
			int num = 0;
			UIPracticeBattleListChild[] array = mUIPracticeBattleListChildren_PracticeTargetAll;
			foreach (UIPracticeBattleListChild uIPracticeBattleListChild in array)
			{
				uIPracticeBattleListChild.alpha = 0f;
				uIPracticeBattleListChild.transform.localPosition = Vector3.zero;
				uIPracticeBattleListChild.transform.parent = mTransform_ObjectPool;
				uIPracticeBattleListChild.SetActive(isActive: false);
				uIPracticeBattleListChild.SetOnClickListener(null);
				if (num < mRivalDecks.Count)
				{
					DeckModel deckModel = mRivalDecks[num];
					List<IsGoCondition> conditions = deckCheckUtil.IsValidPractice(deckModel.Id);
					uIPracticeBattleListChild.Initialize(deckModel, conditions);
					uIPracticeBattleListChild.SetOnClickListener(OnClickChild);
					uIPracticeBattleListChild.alpha = 1f;
					uIPracticeBattleListChild.transform.parent = mGrid_Focasable.transform;
					uIPracticeBattleListChild.transform.localPosition = Vector3.zero;
					uIPracticeBattleListChild.transform.localScale = Vector3.one;
					uIPracticeBattleListChild.SetActive(isActive: true);
					uIPracticeBattleListChild.ParentHasChanged();
					list.Add(uIPracticeBattleListChild);
				}
				num++;
			}
			mUIPracticeBattleListChildren_Focasable = list.ToArray();
		}

		private void OnClickChild(UIPracticeBattleListChild child)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			ChangeFocus(child, needSe: true);
			OnDeckSelectedDeck(child.GetDeckModel(), child.GetConditions());
		}

		private void OnDeckSelectedDeck(DeckModel deckModel, List<IsGoCondition> conditions)
		{
			if (mOnSelectedDeckListener != null)
			{
				mOnSelectedDeckListener(deckModel, conditions);
			}
		}

		public void SetOnSelectedDeckListener(Action<DeckModel, List<IsGoCondition>> onSelectedDeckListener)
		{
			mOnSelectedDeckListener = onSelectedDeckListener;
		}

		public void Show(Action onFinishedanimation)
		{
			ChangeFocus(mUIPracticeBattleListChildren_Focasable[0], needSe: false);
			if (DOTween.IsTweening(AnimationType.ShowHide))
			{
				DOTween.Kill(AnimationType.ShowHide);
			}
			DOTween.Sequence().SetId(AnimationType.ShowHide).Append(DOVirtual.Float(mWidgetThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				mWidgetThis.alpha = alpha;
			}))
				.Join(DOVirtual.Float(0f, 66f, 0.4f, delegate(float cellHeight)
				{
					mGrid_Focasable.cellHeight = cellHeight;
					mGrid_Focasable.Reposition();
				}))
				.OnComplete(delegate
				{
					if (onFinishedanimation != null)
					{
						onFinishedanimation();
					}
				});
		}

		private void ChangeFocus(UIPracticeBattleListChild child, bool needSe)
		{
			if (mFocus != null)
			{
				mFocus.RemoveHover();
			}
			mFocus = child;
			if (mFocus != null)
			{
				mFocus.Hover();
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
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

		public void Hide(Action onFinishedAnimation)
		{
			if (DOTween.IsTweening(AnimationType.ShowHide))
			{
				DOTween.Kill(AnimationType.ShowHide);
			}
			Tween t = DOVirtual.Float(1f, 0f, 0.2f, delegate(float alpha)
			{
				mWidgetThis.alpha = alpha;
			});
			DOTween.Sequence().Append(t).Join(base.transform.DOScale(new Vector3(0.9f, 0.9f), 0.4f))
				.SetId(AnimationType.ShowHide);
			onFinishedAnimation?.Invoke();
		}

		public void SetOnBackCallBack(Action OnCancelBattleTargetSelect)
		{
			mOnBackCallBack = OnCancelBattleTargetSelect;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
			if (mKeyController != null)
			{
				mTransform_TouchBackArea.SetActive(isActive: true);
			}
			else
			{
				mTransform_TouchBackArea.SetActive(isActive: false);
			}
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[1].down)
			{
				OnClickChild(mFocus);
			}
			else if (mKeyController.keyState[0].down)
			{
				OnBack();
			}
			else if (mKeyController.keyState[8].down)
			{
				int num = Array.IndexOf(mUIPracticeBattleListChildren_Focasable, mFocus);
				int num2 = num - 1;
				if (0 <= num2)
				{
					ChangeFocus(mUIPracticeBattleListChildren_Focasable[num2], needSe: true);
				}
			}
			else if (mKeyController.keyState[12].down)
			{
				int num3 = Array.IndexOf(mUIPracticeBattleListChildren_Focasable, mFocus);
				int num4 = num3 + 1;
				if (num4 < mUIPracticeBattleListChildren_Focasable.Length)
				{
					ChangeFocus(mUIPracticeBattleListChildren_Focasable[num4], needSe: true);
				}
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(AnimationType.ShowHide))
			{
				DOTween.Kill(AnimationType.ShowHide);
			}
			mWidgetThis = null;
			mUIPracticeBattleListChildren_PracticeTargetAll = null;
			mGrid_Focasable = null;
			mTransform_TouchBackArea = null;
			mTransform_ObjectPool = null;
			mUIPracticeBattleListChildren_Focasable = null;
			mRivalDecks = null;
			mUIButtonManager = null;
		}
	}
}
