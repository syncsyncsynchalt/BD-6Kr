using KCV.Production;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIDutyRewardExchangeItem : MonoBehaviour
	{
		[SerializeField]
		private ProdRevampReceiveItem mPrefab_ProdRevampReceiveItem;

		public void Initialize(Reward_Exchange_Slotitem exchangeReward, Action onFinished)
		{
			Initialize(exchangeReward.ItemFrom, exchangeReward.ItemTo, exchangeReward.IsCosumedTojoin(), onFinished);
		}

		private void Initialize(IReward_Slotitem from, IReward_Slotitem to, bool isCosumedToJoin, Action onFinished)
		{
			ProdRevampReceiveItem.Instantiate(mPrefab_ProdRevampReceiveItem, base.gameObject.transform, from, to, 500, isCosumedToJoin, new KeyControl()).Play(onFinished);
		}
	}
}
