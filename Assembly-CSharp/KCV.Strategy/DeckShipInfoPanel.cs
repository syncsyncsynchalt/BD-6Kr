using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Strategy
{
	public class DeckShipInfoPanel : MonoBehaviour
	{
		private UIPanel panel;

		[SerializeField]
		private CommonShipBanner[] ShipBanners;

		[SerializeField]
		private UILabel DeckName;

		[SerializeField]
		private UILabel[] HPLabels;

		[SerializeField]
		private UILabel[] NameLabels;

		[SerializeField]
		private CommonShipSupplyState[] shipSupplyStates;

		[SerializeField]
		private GameObject BlackPanel;

		public bool isOpen;

		private float Cliping;

		private void Start()
		{
			panel = GetComponent<UIPanel>();
			panel.SetRect(0f, 0f, 0f, 1f);
			panel.alpha = 0f;
			Cliping = 0f;
			isOpen = false;
			DeckName.supportEncoding = false;
		}

		private void Update()
		{
		}

		public void OpenPanel()
		{
			isOpen = true;
			BlackPanel.SafeGetTweenAlpha(0f, 1f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			ChangeDeck();
			panel.alpha = 1f;
			panel.SetRect(0f, 0f, 0f, 1f);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", Cliping, "to", 1, "time", 0.2f, "onupdate", "UpdateHandler"));
			SoundUtils.PlaySE(SEFIleInfos.SE_002);
		}

		public void ClosePanel()
		{
			isOpen = false;
			BlackPanel.SafeGetTweenAlpha(1f, 0f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			TweenAlpha.Begin(base.gameObject, 0.2f, 0f);
			Cliping = 0f;
			for (int i = 0; i < ShipBanners.Length; i++)
			{
				if (ShipBanners[i].isActiveAndEnabled)
				{
					ShipBanners[i].StopParticle();
				}
			}
			SoundUtils.PlaySE(SEFIleInfos.SE_003);
		}

		public void ChangeDeck()
		{
			int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			ShipModel[] ships = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(id).GetShips();
			DeckName.text = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Name;
			for (int i = 0; i < 6; i++)
			{
				if (i < ships.Length)
				{
					ShipBanners[i].transform.parent.SetActive(isActive: true);
					ShipBanners[i].SetShipData(ships[i]);
					HPLabels[i].text = ships[i].NowHp + " / " + ships[i].MaxHp;
					NameLabels[i].text = ships[i].Name;
					shipSupplyStates[i].setSupplyState(ships[i]);
				}
				else
				{
					ShipBanners[i].transform.parent.SetActive(isActive: false);
				}
			}
		}

		private void UpdateHandler(float value)
		{
			panel.SetRect(0f, 0f, 960f * value, 544f * value);
			Cliping = value;
		}
	}
}
