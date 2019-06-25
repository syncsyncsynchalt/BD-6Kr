using DG.Tweening;
using KCV.Display;
using KCV.Utils;
using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyGrid : BaseUISummaryGrid<UIDutySummary, DutyModel>
	{
		private enum ChangeType
		{
			None,
			Left,
			Right,
			Update
		}

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		private KeyControl mKeyController;

		private UIDutySummary mHoverSummary;

		private UIDutySummary.UIDutySummaryAction mSummarySelectedCallBack;

		private ChangeType mListChangeType;

		private Action mOnChangePageDutyGrid;

		private void Awake()
		{
			mUIDisplaySwipeEventRegion.SetOnSwipeListener(OnSwipeEventListener);
		}

		private void OnSwipeEventListener(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if (mKeyController == null || actionType != UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				return;
			}
			float num = 0.1f;
			if (num < Math.Abs(movePercentageX))
			{
				if (0f < movePercentageX)
				{
					MoveToLeftPage();
				}
				else if (movePercentageX < 0f)
				{
					MoveToRightPage();
				}
			}
		}

		public void SetOnSummarySelectedCallBack(UIDutySummary.UIDutySummaryAction summaryActionCallBack)
		{
			mSummarySelectedCallBack = summaryActionCallBack;
		}

		public override void Initialize(DutyModel[] models)
		{
			mListChangeType = ChangeType.Update;
			base.Initialize(models);
			OnChangePage();
		}

		public override UIDutySummary GenerateView(UIGrid target, UIDutySummary prefab, DutyModel model)
		{
			UIDutySummary uIDutySummary = base.GenerateView(target, prefab, model);
			uIDutySummary.SetCallBackSummaryAction(UIDutySummaryActionCallBack);
			return uIDutySummary;
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[8].down)
			{
				if (mHoverSummary != null)
				{
					int num = mHoverSummary.GetIndex() - 1;
					if (0 <= num)
					{
						ChangeHoverSummary(GetSummaryView(num), changedSEFlag: true);
					}
				}
			}
			else if (mKeyController.keyState[12].down)
			{
				if (mHoverSummary != null)
				{
					int num2 = mHoverSummary.GetIndex() + 1;
					if (num2 < GetSummaryViews().Length)
					{
						ChangeHoverSummary(GetSummaryView(num2), changedSEFlag: true);
					}
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				if (mSummarySelectedCallBack != null && mHoverSummary != null)
				{
					mSummarySelectedCallBack(UIDutySummary.SelectType.Hover, mHoverSummary);
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			}
			else if (mKeyController.keyState[5].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (mKeyController.keyState[14].down)
			{
				MoveToLeftPage();
			}
			else if (mKeyController.keyState[10].down)
			{
				MoveToRightPage();
			}
			else if (mKeyController.keyState[2].down && mSummarySelectedCallBack != null && mHoverSummary != null)
			{
				mSummarySelectedCallBack(UIDutySummary.SelectType.CallDetail, mHoverSummary);
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchMoveLeftPage()
		{
			if (mKeyController != null)
			{
				MoveToLeftPage();
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchMoveRightPage()
		{
			if (mKeyController != null)
			{
				MoveToRightPage();
			}
		}

		private void MoveToLeftPage()
		{
			mListChangeType = ChangeType.Left;
			if (GoToPage(GetCurrentPageIndex() - 1))
			{
				OnChangePage();
				ChangeHoverSummary(GetSummaryView(0), changedSEFlag: false);
				PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void MoveToRightPage()
		{
			mListChangeType = ChangeType.Right;
			if (GoToPage(GetCurrentPageIndex() + 1))
			{
				OnChangePage();
				ChangeHoverSummary(GetSummaryView(0), changedSEFlag: false);
				PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void UIDutySummaryActionCallBack(UIDutySummary.SelectType type, UIDutySummary targetObject)
		{
			ChangeHoverSummary(targetObject, changedSEFlag: false);
			if (mSummarySelectedCallBack != null)
			{
				mSummarySelectedCallBack(type, targetObject);
			}
		}

		public override bool GoToPage(int pageIndex)
		{
			return GoToPage(pageIndex, focus: true);
		}

		public bool GoToPage(int pageIndex, bool focus)
		{
			bool flag = base.GoToPage(pageIndex);
			if (flag)
			{
				mListChangeType = ChangeType.Update;
				if (focus)
				{
					ChangeHoverSummary(GetSummaryView(0), changedSEFlag: false);
				}
			}
			return flag;
		}

		private void ChangeHoverSummary(UIDutySummary summary, bool changedSEFlag)
		{
			if (mHoverSummary != null)
			{
				mHoverSummary.RemoveHover();
				mHoverSummary.DepthBack();
			}
			mHoverSummary = summary;
			if (mHoverSummary != null)
			{
				if (changedSEFlag)
				{
					PlaySE(SEFIleInfos.CommonCursolMove);
				}
				mHoverSummary.Hover();
				mHoverSummary.DepthFront();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public KeyControl GetKeyController()
		{
			if (mKeyController == null)
			{
				mKeyController = new KeyControl();
			}
			return mKeyController;
		}

		public void FirstFocus()
		{
			UIDutySummary summaryView = GetSummaryView(0);
			ChangeHoverSummary(summaryView, changedSEFlag: false);
		}

		private void PlaySE(SEFIleInfos seType)
		{
			if ((SingletonMonoBehaviour<SoundManager>.Instance != null) ? true : false)
			{
				SoundUtils.PlaySE(seType);
			}
		}

		public override void OnFinishedCreateViews()
		{
			OnFinishedCreateViewsCoroutine();
		}

		private void OnFinishedCreateViewsCoroutine()
		{
			UIDutySummary[] summaryViews = GetSummaryViews();
			int num = 0;
			int num2;
			switch (mListChangeType)
			{
			case ChangeType.Left:
				num2 = -1;
				break;
			case ChangeType.Right:
				num2 = 1;
				break;
			default:
				num2 = 0;
				break;
			}
			UIDutySummary[] array = summaryViews;
			UIDutySummary dsumt;
			UIDutySummary dsum;
			foreach (UIDutySummary uIDutySummary in array)
			{
				uIDutySummary.SetDepth(summaryViews.Length - num);
				Vector3 localPosition = uIDutySummary.gameObject.transform.localPosition;
				Sequence s = DOTween.Sequence();
				switch (mListChangeType)
				{
				case ChangeType.Left:
				case ChangeType.Right:
				{
					uIDutySummary.gameObject.transform.localPosition = new Vector3(960f * (float)num2, localPosition.y, localPosition.z);
					Tween t3 = uIDutySummary.transform.DOLocalMoveX(0f, 0.6f).SetDelay(0.05f * (float)num).SetEase(Ease.OutQuint);
					uIDutySummary.Show();
					s.Join(t3);
					break;
				}
				case ChangeType.Update:
				{
					uIDutySummary.gameObject.transform.localPosition = new Vector3(localPosition.x, 0f, localPosition.z);
					Sequence sequence2 = DOTween.Sequence();
					Tween t4 = uIDutySummary.transform.DOLocalMoveY(localPosition.y, 0.6f).SetEase(Ease.OutQuint);
					sequence2.Append(t4);
					dsumt = uIDutySummary;
					if (num != 0)
					{
						Tween t5 = DOVirtual.Float(0f, 1f, 0.7f, delegate(float alpha)
						{
							dsumt.GetPanel().alpha = alpha;
						});
						sequence2.Join(t5);
					}
					uIDutySummary.Show();
					s.Join(sequence2);
					break;
				}
				case ChangeType.None:
				{
					uIDutySummary.gameObject.transform.localPosition = new Vector3(localPosition.x, 0f, localPosition.z);
					Sequence sequence = DOTween.Sequence();
					Tween t = uIDutySummary.transform.DOLocalMoveY(localPosition.y, 0.6f).SetEase(Ease.OutQuint);
					sequence.Append(t);
					dsum = uIDutySummary;
					if (num != 0)
					{
						Tween t2 = DOVirtual.Float(0f, 1f, 0.6f, delegate(float alpha)
						{
							dsum.GetPanel().alpha = alpha;
						});
						sequence.Join(t2);
					}
					uIDutySummary.Show();
					s.Join(sequence);
					break;
				}
				}
				num++;
			}
		}

		internal void SetOnChangePageListener(Action onChangePageDutyGrid)
		{
			mOnChangePageDutyGrid = onChangePageDutyGrid;
		}

		private void OnChangePage()
		{
			if (mOnChangePageDutyGrid != null)
			{
				mOnChangePageDutyGrid();
			}
		}

		private void OnDestroy()
		{
			mUIDisplaySwipeEventRegion = null;
			mKeyController = null;
			mHoverSummary = null;
			mSummarySelectedCallBack = null;
		}
	}
}
