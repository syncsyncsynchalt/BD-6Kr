using DG.Tweening;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Strategy
{
	public class UIStageConfirm : MonoBehaviour
	{
		[SerializeField]
		private UIButton mButton_GoStage;

		[SerializeField]
		private Transform mTransfrom_GoStageDisabled;

		[SerializeField]
		private UILabel mLabel_OperationTitle;

		[SerializeField]
		private UILabel mLabel_OperationDetail;

		[SerializeField]
		private UISprite[] mSprites_Reward;

		[SerializeField]
		private Vector3 mVector3_ShowPosition;

		[SerializeField]
		private Vector3 mVector3_HidePosition;

		[SerializeField]
		private GoConditionInfo ConditionInfo;

		private Tween mTweenShowHide;

		public MapModel mMapModel
		{
			get;
			private set;
		}

		public bool Shown
		{
			get;
			private set;
		}

		public void Initialize(MapModel mapModel)
		{
			mMapModel = mapModel;
			mLabel_OperationTitle.text = mMapModel.Opetext;
			mLabel_OperationDetail.text = mMapModel.Infotext;
			if (mMapModel.Map_Possible)
			{
				mButton_GoStage.transform.SetActive(isActive: true);
				mTransfrom_GoStageDisabled.SetActive(isActive: false);
			}
			else
			{
				mButton_GoStage.transform.SetActive(isActive: false);
				mTransfrom_GoStageDisabled.SetActive(isActive: true);
			}
			int[] rewardItemIds = mapModel.GetRewardItemIds();
			UISprite[] array = mSprites_Reward;
			foreach (UISprite component in array)
			{
				component.SetActive(isActive: false);
			}
			for (int j = 0; j < rewardItemIds.Length; j++)
			{
				mSprites_Reward[j].SetActive(isActive: true);
				mSprites_Reward[j].spriteName = $"item_{rewardItemIds[j]}";
			}
		}

		public void Show()
		{
			Shown = true;
			base.transform.SetActive(isActive: true);
			if (mTweenShowHide != null)
			{
				mTweenShowHide.Kill();
				mTweenShowHide = null;
			}
			ConditionInfo.Initialize(mMapModel);
			this.DelayActionFrame(1, delegate
			{
				mTweenShowHide = DOTween.Sequence().Append(base.transform.DOLocalMove(mVector3_ShowPosition, 0.3f)).Join(mButton_GoStage.transform.DOScale(new Vector3(1f, 1f), 0.2f).SetEase(Ease.OutBack))
					.SetId(this);
			});
		}

		public void Hide()
		{
			Shown = false;
			if (mTweenShowHide != null)
			{
				mTweenShowHide.Kill();
				mTweenShowHide = null;
			}
			mTweenShowHide = DOTween.Sequence().Append(base.transform.DOLocalMove(mVector3_HidePosition, 0.2f).OnComplete(delegate
			{
				base.transform.SetActive(isActive: false);
			})).Join(mButton_GoStage.transform.DOScale(new Vector3(0.1f, 0.1f), 0.3f))
				.SetId(this);
		}

		public void ClickAnimation(Action onFinished)
		{
			mButton_GoStage.transform.DOScale(new Vector3(0.8f, 0.8f, 0f), 0.15f).SetEase(Ease.InCirc).OnComplete(delegate
			{
				mButton_GoStage.transform.DOScale(new Vector3(1f, 1f, 0f), 0.15f).SetEase(Ease.OutCirc).OnComplete(delegate
				{
					if (onFinished != null)
					{
						onFinished();
					}
				});
			})
				.PlayForward();
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mButton_GoStage = null;
			mTransfrom_GoStageDisabled = null;
			mLabel_OperationTitle = null;
			mLabel_OperationDetail = null;
			mSprites_Reward = null;
			ConditionInfo = null;
		}
	}
}
