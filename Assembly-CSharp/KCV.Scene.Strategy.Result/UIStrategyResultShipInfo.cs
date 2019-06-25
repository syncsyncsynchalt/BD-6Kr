using DG.Tweening;
using KCV.Utils;
using local.models;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Strategy.Result
{
	public class UIStrategyResultShipInfo : MonoBehaviour
	{
		private UIWidget mWidget_Banner;

		[SerializeField]
		private UIWidget mWidget_Status;

		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UITexture mTexture_GaugeExp;

		[SerializeField]
		private UITexture mTexture_LevelUp;

		private ShipModel mShipModel;

		private ShipExpModel mShipExpModel;

		private int mSlotIndex;

		private void Awake()
		{
			mWidget_Banner = GetComponent<UIWidget>();
			mWidget_Banner.alpha = 0f;
		}

		private void Start()
		{
			mTexture_LevelUp.SetActive(isActive: false);
			mWidget_Status.alpha = 0f;
		}

		public void Initialize(int slotIndex, ShipModel shipModel, ShipExpModel shipExpModel)
		{
			mSlotIndex = slotIndex;
			mShipModel = shipModel;
			mShipExpModel = shipExpModel;
			mLabel_Level.text = shipExpModel.LevelBefore.ToString();
			mCommonShipBanner.SetShipData(mShipModel);
		}

		public void PlayShowBannerAnimation(Action onFinished)
		{
			Vector3 localPosition = base.transform.localPosition;
			Vector3 localPosition2 = base.transform.localPosition;
			float x = localPosition2.x;
			Vector3 localPosition3 = base.transform.localPosition;
			Vector3 localPosition4 = new Vector3(x, localPosition3.y - 20f);
			base.transform.localPosition = localPosition4;
			float delay = (float)mSlotIndex * 0.1f;
			float duration = 0.5f;
			base.transform.DOLocalMove(localPosition, duration).SetDelay(delay);
			DOVirtual.Float(0f, 1f, duration, delegate(float alpha)
			{
				mWidget_Banner.alpha = alpha;
			}).SetDelay(delay).OnComplete(delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
			});
		}

		public void PlayShowStatusAnimation(Action onFinished)
		{
			Vector3 localPosition = mWidget_Status.transform.localPosition;
			Vector3 localPosition2 = new Vector3(localPosition.x + 20f, localPosition.y);
			mWidget_Status.transform.localPosition = localPosition2;
			float delay = (float)mSlotIndex * 0.1f;
			float duration = 0.5f;
			mWidget_Status.transform.DOLocalMove(localPosition, duration).SetDelay(delay);
			DOVirtual.Float(0f, 1f, duration, delegate(float alpha)
			{
				mWidget_Status.alpha = alpha;
			}).SetDelay(delay).OnComplete(delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
			});
		}

		public void PlayExpAnimation(Action onFinished)
		{
			float delay = 1.3f;
			float num = 0.4f;
			float maxGaugeTextureWidth = 90f;
			mTexture_GaugeExp.width = (int)(maxGaugeTextureWidth * ((float)mShipExpModel.ExpRateBefore * 0.01f));
			int num2 = mShipExpModel.ExpRateAfter.Sum();
			int currentLevel = mShipExpModel.LevelBefore;
			DOVirtual.Float(mShipExpModel.ExpRateBefore, num2, (float)(num2 / 90) * num, delegate(float exp)
			{
				mTexture_GaugeExp.width = (int)(maxGaugeTextureWidth * (exp % 100f * 0.01f));
				int num3 = (int)(exp / 100f);
				if (currentLevel != num3 + mShipExpModel.LevelBefore)
				{
					currentLevel = num3 + mShipExpModel.LevelBefore;
					mLabel_Level.text = currentLevel.ToString();
				}
			}).SetDelay(delay).OnComplete(delegate
			{
				if (mShipExpModel.LevelBefore != mShipExpModel.LevelAfter)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_058);
					PlayLevelUpAnimation();
				}
				if (onFinished != null)
				{
					onFinished();
				}
			});
		}

		private void PlayLevelUpAnimation()
		{
			mTexture_LevelUp.SetActive(isActive: true);
			Sequence sequence = DOTween.Sequence().SetId(this);
			TweenCallback action = delegate
			{
				mTexture_LevelUp.transform.localPosition = new Vector3(225f, -15f);
			};
			Tween t = mTexture_LevelUp.transform.DOLocalMove(new Vector3(225f, -10f), 0.15f);
			Tween t2 = mTexture_LevelUp.transform.DOLocalMove(new Vector3(225f, -15f), 0.15f);
			Tween t3 = DOVirtual.Float(1f, 0f, 0.5f, delegate(float alpha)
			{
				mTexture_LevelUp.alpha = alpha;
			}).OnComplete(delegate
			{
				mTexture_LevelUp.SetActive(isActive: false);
			});
			sequence.OnPlay(action);
			sequence.Append(t);
			sequence.Append(t2);
			sequence.AppendInterval(0.5f);
			sequence.Append(t3);
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mWidget_Banner = null;
			mWidget_Status = null;
			mCommonShipBanner = null;
			mLabel_Level = null;
			mTexture_GaugeExp = null;
			mTexture_LevelUp = null;
			mShipModel = null;
			mShipExpModel = null;
		}
	}
}
