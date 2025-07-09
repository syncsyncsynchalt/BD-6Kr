using Common.Enum;
using DG.Tweening;
using KCV.Scene.Port;
using KCV.View.ScrollView;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Interior
{
	public class UIInteriorFurnitureChangeScrollListNew : UIScrollList<UIInteriorFurnitureChangeScrollListChildModelNew, UIInteriorFurnitureChangeScrollListChildNew>
	{
		private enum TweenAnimationType
		{
			ShowHide
		}

		[SerializeField]
		private UITexture mTexture_Header;

		[SerializeField]
		private UILabel mLabel_Genre;

		[SerializeField]
		private UITexture mTexture_TouchBackArea;

		[SerializeField]
		private OnClickEventSender mOnClickEventSender_Back;

		[SerializeField]
		private OnClickEventSender mOnClickEventSender_Next;

		private KeyControl mKeyController;

		private Action<UIInteriorFurnitureChangeScrollListChildNew> mOnSelectedListener;

		private Action<UIInteriorFurnitureChangeScrollListChildNew> mOnChangedListener;

		private Action mOnBackListener;

		public void Show()
		{
			mOnClickEventSender_Back.SetClickable(clickable: true);
			mOnClickEventSender_Next.SetClickable(clickable: true);
			if (DOTween.IsTweening(TweenAnimationType.ShowHide))
			{
				DOTween.Kill(TweenAnimationType.ShowHide);
			}
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.gameObject, 0.3f);
			tweenPosition.from = base.transform.localPosition;
			TweenPosition tweenPosition2 = tweenPosition;
			Vector3 localPosition = base.transform.localPosition;
			float y = localPosition.y;
			Vector3 localPosition2 = base.transform.localPosition;
			tweenPosition2.to = new Vector3(0f, y, localPosition2.z);
			tweenPosition.ignoreTimeScale = true;
		}

		public void Hide()
		{
			mOnClickEventSender_Back.SetClickable(clickable: false);
			mOnClickEventSender_Next.SetClickable(clickable: false);
			if (DOTween.IsTweening(TweenAnimationType.ShowHide))
			{
				DOTween.Kill(TweenAnimationType.ShowHide);
			}
			Sequence sequence = DOTween.Sequence();
			Tween t = base.transform.DOLocalMoveX(-960f, 0.6f).SetEase(Ease.OutCirc);
			sequence.Append(t);
			sequence.SetId(TweenAnimationType.ShowHide);
		}

		public void Initialize(int deckId, FurnitureKinds furnitureKind, FurnitureModel[] models, Camera camera)
		{
			UIInteriorFurnitureChangeScrollListChildModelNew[] models2 = GenerateChildDTOs(deckId, models);
			UpdateHeader(furnitureKind);
			Initialize(models2);
			SetSwipeEventCamera(camera);
		}

		private UIInteriorFurnitureChangeScrollListChildModelNew[] GenerateChildDTOs(int deckId, FurnitureModel[] models)
		{
			List<UIInteriorFurnitureChangeScrollListChildModelNew> list = new List<UIInteriorFurnitureChangeScrollListChildModelNew>();
			foreach (FurnitureModel model in models)
			{
				UIInteriorFurnitureChangeScrollListChildModelNew item = new UIInteriorFurnitureChangeScrollListChildModelNew(deckId, model);
				list.Add(item);
			}
			return list.ToArray();
		}

		public new void RefreshViews()
		{
			base.RefreshViews();
		}

		protected override void OnUpdate()
		{
			if (mKeyController != null)
			{
				if (mKeyController.IsUpDown())
				{
					PrevFocus();
				}
				else if (mKeyController.IsDownDown())
				{
					NextFocus();
				}
				else if (mKeyController.IsLeftDown())
				{
					PrevPageOrHeadFocus();
				}
				else if (mKeyController.IsRightDown())
				{
					NextPageOrTailFocus();
				}
				else if (mKeyController.IsMaruDown())
				{
					Select();
				}
				else if (mKeyController.IsBatuDown())
				{
					Back();
				}
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchSelect()
		{
			mOnClickEventSender_Back.SetClickable(clickable: false);
			mOnClickEventSender_Next.SetClickable(clickable: false);
			Select();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchBack()
		{
			mOnClickEventSender_Back.SetClickable(clickable: false);
			mOnClickEventSender_Next.SetClickable(clickable: false);
			Back();
		}

		private void Back()
		{
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void UpdateHeader(FurnitureKinds furnitureKinds)
		{
			switch (furnitureKinds)
			{
			case FurnitureKinds.Wall:
				mLabel_Genre.text = "壁紙";
				mTexture_Header.color = new Color(161f, 121f / 255f, 91f / 255f);
				break;
			case FurnitureKinds.Floor:
				mLabel_Genre.text = "床";
				mTexture_Header.color = new Color(56f / 85f, 36f / 85f, 0.4117647f);
				break;
			case FurnitureKinds.Desk:
				mLabel_Genre.text = "椅子＋机";
				mTexture_Header.color = new Color(134f / 255f, 39f / 85f, 148f / 255f);
				break;
			case FurnitureKinds.Window:
				mLabel_Genre.text = "窓枠＋カーテン";
				mTexture_Header.color = new Color(20f / 51f, 152f / 255f, 0.5882353f);
				break;
			case FurnitureKinds.Hangings:
				mLabel_Genre.text = "装飾";
				mTexture_Header.color = new Color(0.470588237f, 0.7058824f, 26f / 51f);
				break;
			case FurnitureKinds.Chest:
				mLabel_Genre.text = "家具";
				mTexture_Header.color = new Color(0.5882353f, 0.5882353f, 20f / 51f);
				break;
			}
		}

		internal void SetOnSelectedListener(Action<UIInteriorFurnitureChangeScrollListChildNew> onSelectedListener)
		{
			mOnSelectedListener = onSelectedListener;
		}

		internal void SetOnChangedItemListener(Action<UIInteriorFurnitureChangeScrollListChildNew> onChangedListener)
		{
			mOnChangedListener = onChangedListener;
		}

		protected override void OnChangedFocusView(UIInteriorFurnitureChangeScrollListChildNew focusToView)
		{
			if (mOnChangedListener != null)
			{
				mOnChangedListener(focusToView);
			}
		}

		protected override void OnSelect(UIInteriorFurnitureChangeScrollListChildNew view)
		{
			if (mOnSelectedListener != null)
			{
				mOnSelectedListener(view);
			}
		}

		internal void SetOnBackListener(Action onBackListener)
		{
			mOnBackListener = onBackListener;
		}

		public new void StartControl()
		{
			HeadFocus();
			base.StartControl();
		}

		public new void LockControl()
		{
			base.LockControl();
		}

		internal void ResumeControl()
		{
			base.StartControl();
			mOnClickEventSender_Back.SetClickable(clickable: true);
			mOnClickEventSender_Next.SetClickable(clickable: true);
		}

		protected override void OnCallDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Header);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Genre);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_TouchBackArea);
			mKeyController = null;
			mOnSelectedListener = null;
			mOnChangedListener = null;
			mOnBackListener = null;
		}
	}
}
