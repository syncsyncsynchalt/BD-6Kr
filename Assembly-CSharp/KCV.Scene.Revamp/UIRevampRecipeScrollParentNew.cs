using DG.Tweening;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeScrollParentNew : UIScrollList<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>
	{
		public enum AnimationType
		{
			SlotIn
		}

		private RevampManager mRevampManager;

		private Action<UIRevampRecipeScrollChildNew> mOnSelectedRecipeListener;

		private Action mOnFinishedSlotInAnimationListener;

		private Action mOnBackListener;

		private KeyControl mKeyController;

		public float duration = 0.3f;

		public Ease easing = Ease.InExpo;

		public void Initialize(RevampManager revampManager)
		{
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			mRevampManager = revampManager;
			RevampRecipeModel[] array = mRevampManager.GetRecipes().ToArray();
			List<RevampRecipeScrollUIModel> list = new List<RevampRecipeScrollUIModel>();
			RevampRecipeModel[] array2 = array;
			foreach (RevampRecipeModel revampRecipeModel in array2)
			{
				bool clickable = 0 < mRevampManager.GetSlotitemList(revampRecipeModel.RecipeId).Length;
				RevampRecipeScrollUIModel item = new RevampRecipeScrollUIModel(revampRecipeModel, clickable);
				list.Add(item);
			}
			Initialize(list.ToArray());
		}

		protected override void OnChangedFocusView(UIRevampRecipeScrollChildNew focusToView)
		{
			if (0 < mModels.Length && base.mState == ListState.Waiting && mCurrentFocusView != null)
			{
				int realIndex = mCurrentFocusView.GetRealIndex();
				CommonPopupDialog.Instance.StartPopup(realIndex + 1 + "/" + mModels.Length, 0, CommonPopupDialogMessage.PlayType.Long);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void PlaySlotInAnimation()
		{
			if (DOTween.IsTweening(AnimationType.SlotIn))
			{
				DOTween.Kill(AnimationType.SlotIn);
			}
			Sequence sequence = DOTween.Sequence().SetId(AnimationType.SlotIn);
			UIRevampRecipeScrollChildNew slot = mViews[0];
			UIRevampRecipeScrollChildNew slot2 = mViews[1];
			UIRevampRecipeScrollChildNew slot3 = mViews[2];
			sequence.Append(GenerateSlotInTween(slot));
			sequence.Append(GenerateSlotInTween(slot2));
			sequence.Append(GenerateSlotInTween(slot3));
			sequence.OnComplete(delegate
			{
				NotifyFinishedSlotInAnimation();
			});
			sequence.PlayForward();
		}

		protected override bool OnSelectable(UIRevampRecipeScrollChildNew view)
		{
			if (view.GetModel().Clickable)
			{
				return true;
			}
			return false;
		}

		private Tween GenerateSlotInTween(UIRevampRecipeScrollChildNew slot)
		{
			slot.alpha = 0f;
			slot.transform.localPositionX(-300f);
			Sequence sequence = DOTween.Sequence();
			Tween t = slot.transform.DOLocalMoveX(0f, duration).SetEase(easing);
			Tween t2 = DOVirtual.Float(0f, 1f, duration, delegate(float alpha)
			{
				slot.alpha = alpha;
			});
			sequence.Append(t);
			sequence.Join(t2);
			sequence.OnStart(delegate
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_019);
			});
			return sequence;
		}

		public void SetOnFinishedSlotInAnimationListener(Action onFinishedSlotInAnimationListener)
		{
			mOnFinishedSlotInAnimationListener = onFinishedSlotInAnimationListener;
		}

		private void NotifyFinishedSlotInAnimation()
		{
			if (mOnFinishedSlotInAnimationListener != null)
			{
				mOnFinishedSlotInAnimationListener();
			}
		}

		internal void SetOnSelectedListener(Action<UIRevampRecipeScrollChildNew> onSelectedRecipeListener)
		{
			mOnSelectedRecipeListener = onSelectedRecipeListener;
		}

		protected override void OnSelect(UIRevampRecipeScrollChildNew view)
		{
			if (mOnSelectedRecipeListener != null)
			{
				mOnSelectedRecipeListener(view);
			}
		}

		internal void SetCamera(Camera cameraTouchEventCatch)
		{
			SetSwipeEventCamera(cameraTouchEventCatch);
		}

		internal KeyControl GetKeyController()
		{
			if (mKeyController == null)
			{
				mKeyController = new KeyControl();
			}
			return mKeyController;
		}

		internal new void StartControl()
		{
			HeadFocus();
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			base.StartControl();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				PlaySlotInAnimation();
			}
			if (mKeyController != null && base.mState == ListState.Waiting)
			{
				if (mKeyController.IsUpDown())
				{
					PrevFocus();
				}
				else if (mKeyController.IsDownDown())
				{
					NextFocus();
				}
				else if (mKeyController.IsMaruDown())
				{
					mKeyController.ClearKeyAll();
					mKeyController.firstUpdate = true;
					Select();
				}
				else if (mKeyController.IsBatuDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				}
			}
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			mRevampManager = null;
			mOnSelectedRecipeListener = null;
			mOnFinishedSlotInAnimationListener = null;
			mOnBackListener = null;
			mKeyController = null;
		}

		internal new void LockControl()
		{
			base.LockControl();
		}
	}
}
