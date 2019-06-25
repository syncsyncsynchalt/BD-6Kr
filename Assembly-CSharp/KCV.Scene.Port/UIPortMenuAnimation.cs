using DG.Tweening;
using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortMenuAnimation : MonoBehaviour
	{
		[SerializeField]
		private Animation mAnim_MennuCollect;

		private Action mOnFinishedCollectAnimationListener;

		private UIPortMenuButton mTargetMenuButton;

		private bool mIsSubMenu;

		public void Initialize(UIPortMenuButton targetMenuButton)
		{
			if (mTargetMenuButton != null)
			{
				NGUITools.AdjustDepth(mTargetMenuButton.gameObject, -10);
				mTargetMenuButton = null;
			}
			mTargetMenuButton = targetMenuButton;
		}

		public void PlayCollectAnimation()
		{
			mAnim_MennuCollect.Play("Anim_MenuCollect");
		}

		public void PlayCollectSubAnimation()
		{
			mAnim_MennuCollect.Play("Anim_MenuCollect_Sub");
		}

		public void OnDepthAdjust()
		{
			if (mTargetMenuButton != null)
			{
				NGUITools.AdjustDepth(mTargetMenuButton.gameObject, 10);
			}
		}

		public void OnFinishedCollectAnimation()
		{
			Sequence s = DOTween.Sequence().SetId(this);
			Tween t = mTargetMenuButton.transform.DOScale(new Vector3(1.8f, 1.8f), 0.2f).SetId(this);
			Tween t2 = mTargetMenuButton.transform.DOScale(new Vector3(1.7f, 1.7f), 0.1f).SetId(this);
			s.Append(t);
			s.Append(t2);
			if (mOnFinishedCollectAnimationListener != null)
			{
				mOnFinishedCollectAnimationListener();
			}
		}

		public void SetOnFinishedCollectAnimationListener(Action onFinishedCollectAnimationListener)
		{
			mOnFinishedCollectAnimationListener = onFinishedCollectAnimationListener;
		}

		private void OnDestroy()
		{
			mAnim_MennuCollect = null;
			mOnFinishedCollectAnimationListener = null;
			mTargetMenuButton = null;
		}
	}
}
