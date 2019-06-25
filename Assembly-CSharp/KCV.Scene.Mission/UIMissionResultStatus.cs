using KCV.Scene.Port;
using KCV.Scene.Strategy.Result;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Mission
{
	public class UIMissionResultStatus : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_DeckName;

		[SerializeField]
		private UIStrategyResultShipInfo[] mUIStrategyResultShipInfos;

		private MissionResultModel mMissionResultModel;

		public void Inititalize(MissionResultModel missionResultModel)
		{
			mMissionResultModel = missionResultModel;
			ShipModel[] ships = mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				mUIStrategyResultShipInfos[i].SetActive(isActive: false);
				mUIStrategyResultShipInfos[i].Initialize(i, shipModel, missionResultModel.GetShipExpInfo(shipModel.MemId));
			}
		}

		private void ChainAnimation(Action<Action> chainFrom, Action<Action> chainTo, Action onFinished)
		{
			chainFrom(delegate
			{
				chainTo(delegate
				{
					if (onFinished != null)
					{
						onFinished();
					}
				});
			});
		}

		private void ShowBanners(Action onFinished)
		{
			ShipModel[] ships = mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				mUIStrategyResultShipInfos[i].SetActive(isActive: true);
				if (i == ships.Length - 1)
				{
					mUIStrategyResultShipInfos[i].PlayShowBannerAnimation(onFinished);
				}
				else
				{
					mUIStrategyResultShipInfos[i].PlayShowBannerAnimation(null);
				}
			}
		}

		private void ShowStatuses(Action onFinished)
		{
			ShipModel[] ships = mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				if (i == ships.Length - 1)
				{
					mUIStrategyResultShipInfos[i].PlayShowStatusAnimation(onFinished);
				}
				else
				{
					mUIStrategyResultShipInfos[i].PlayShowStatusAnimation(null);
				}
			}
		}

		private void ShowExpUpdate(Action onFinished)
		{
			ShipModel[] ships = mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				if (i == ships.Length - 1)
				{
					mUIStrategyResultShipInfos[i].PlayExpAnimation(onFinished);
				}
				else
				{
					mUIStrategyResultShipInfos[i].PlayExpAnimation(null);
				}
			}
		}

		public void PlayShowBanners(Action onFinished)
		{
			ChainAnimation(ShowBanners, ShowStatuses, onFinished);
		}

		public void PlayShowBannersExp(Action onFinished)
		{
			ShowExpUpdate(onFinished);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_DeckName);
			if (mUIStrategyResultShipInfos != null)
			{
				for (int i = 0; i < mUIStrategyResultShipInfos.Length; i++)
				{
					mUIStrategyResultShipInfos[i] = null;
				}
			}
			mUIStrategyResultShipInfos = null;
			mMissionResultModel = null;
		}
	}
}
