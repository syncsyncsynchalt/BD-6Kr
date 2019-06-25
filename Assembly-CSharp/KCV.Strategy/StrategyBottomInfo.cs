using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyBottomInfo : MonoBehaviour
	{
		[Serializable]
		public class DeckInfo
		{
			public UILabel DeckName;

			public GameObject ParentObject;

			public void UpdateDeckInfo(bool isShort = false)
			{
				DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
				ShipModel shipModel = currentDeck?.GetFlagShip();
				DeckName.supportEncoding = false;
				if (shipModel == null)
				{
					DeckName.text = string.Empty;
				}
				else if (isShort)
				{
					DeckName.text = "旗艦 " + shipModel.ShipTypeName + " " + shipModel.Name;
				}
				else
				{
					DeckName.text = currentDeck.Name + "旗艦\n" + shipModel.ShipTypeName + " " + shipModel.Name;
				}
			}

			public string getDeckStateString(DeckModel deck)
			{
				string result = string.Empty;
				switch (deck.MissionState)
				{
				case MissionStates.NONE:
					result = "行動可能";
					break;
				case MissionStates.RUNNING:
					result = "遠征中 残り" + deck.MissionRemainingTurns + "日";
					break;
				case MissionStates.STOP:
					result = "遠征帰還中";
					break;
				}
				if (deck.IsActionEnd())
				{
					result = "行動終了";
				}
				return result;
			}
		}

		[SerializeField]
		private DeckInfo deckInfo;

		[SerializeField]
		private StrategyHaveMaterials haveMaterial;

		[SerializeField]
		private TweenPosition TweenPos;

		public void UpdateBottomPanel(StrategyInfoManager.Mode nowMode, bool isUpdateMaterial = true)
		{
			switch (nowMode)
			{
			case StrategyInfoManager.Mode.AreaInfo:
				if (isUpdateMaterial)
				{
					haveMaterial.UpdateFooterMaterials();
				}
				deckInfo.UpdateDeckInfo();
				break;
			case StrategyInfoManager.Mode.DeckInfo:
				deckInfo.UpdateDeckInfo();
				break;
			}
		}

		public void UpdateDeckInfo(bool isShort)
		{
			deckInfo.UpdateDeckInfo(isShort);
		}

		public void Enter(StrategyInfoManager.Mode nowMode)
		{
			UpdateBottomPanel(nowMode);
			TweenPos.PlayForward();
		}

		public void Exit()
		{
			TweenPos.PlayReverse();
		}

		public void ChangeMode(StrategyInfoManager.Mode nowMode)
		{
			switch (nowMode)
			{
			case StrategyInfoManager.Mode.AreaInfo:
				haveMaterial.ParentObject.SetActive(true);
				deckInfo.ParentObject.SetActive(true);
				break;
			case StrategyInfoManager.Mode.DeckInfo:
				haveMaterial.ParentObject.SetActive(true);
				deckInfo.ParentObject.SetActive(false);
				break;
			}
		}
	}
}
