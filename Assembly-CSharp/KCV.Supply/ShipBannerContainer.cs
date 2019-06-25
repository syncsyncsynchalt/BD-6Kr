using Common.Enum;
using KCV.Utils;
using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Supply
{
	public class ShipBannerContainer : MonoBehaviour
	{
		private const int MAX_SHIP_PER_DECK = 6;

		[SerializeField]
		private UISupplyDeckShipBanner[] _shipBanner;

		private int currentIdx;

		private Vector3 showPos = new Vector3(0f, 0f);

		private Vector3 hidePos = new Vector3(-1000f, 0f);

		private DeckModel deck;

		private int shipCount => deck.GetShipCount();

		public void Init()
		{
			for (int i = 0; i < 6; i++)
			{
				_shipBanner[i].SafeGetTweenAlpha(0f, 1f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				_shipBanner[i].Init(new Vector3(-162f, 106f - (float)i * 63f, 0f));
			}
		}

		public void SelectLengthwise(bool isUp)
		{
			if (UpdateCurrentItem((!isUp) ? (currentIdx + 1) : (currentIdx - 1)))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void InitDeck(DeckModel deck)
		{
			this.deck = deck;
			for (int i = 0; i < 6; i++)
			{
				if (_shipBanner[i] != null)
				{
					_shipBanner[i].SetBanner((i >= shipCount) ? null : deck.GetShip(i), i);
				}
			}
			UpdateCurrentItem(0);
		}

		public bool UpdateCurrentItem(int newIdx)
		{
			if (newIdx < 0 || newIdx >= shipCount)
			{
				return false;
			}
			currentIdx = newIdx;
			for (int i = 0; i < 6; i++)
			{
				if (_shipBanner[i] != null)
				{
					_shipBanner[i].Hover(i == currentIdx);
				}
			}
			return true;
		}

		private void RemoveAllHover()
		{
			for (int i = 0; i < 6; i++)
			{
				_shipBanner[i].Hover(enabled: false);
			}
		}

		public void SwitchCurrentSelected()
		{
			if (_shipBanner[currentIdx].IsSelectable())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				_shipBanner[currentIdx].SwitchSelected();
				SupplyMainManager.Instance.UpdateRightPain();
			}
		}

		public void SwitchAllSelected()
		{
			bool flag = false;
			SupplyMainManager.Instance.SupplyManager.ClickCheckBoxAll();
			for (int i = 0; i < shipCount; i++)
			{
				if (_shipBanner[i].enabled)
				{
					flag = true;
					_shipBanner[i].Select(SupplyMainManager.Instance.SupplyManager.CheckBoxStates[i] == CheckBoxStatus.ON);
				}
			}
			if (flag)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			SupplyMainManager.Instance.UpdateRightPain();
		}

		public List<ShipModel> getSeletedModelList()
		{
			List<ShipModel> list = new List<ShipModel>();
			for (int i = 0; i < shipCount; i++)
			{
				if (_shipBanner[i].selected)
				{
					list.Add(_shipBanner[i].Ship);
				}
			}
			return list;
		}

		public void Show(bool animation)
		{
			base.transform.localPosition = ((!animation) ? showPos : showPos);
		}

		public void Hide(bool animation)
		{
			SupplyMainManager.Instance.SetControllDone(enabled: false);
			base.transform.localPosition = ((!animation) ? hidePos : hidePos);
		}

		public void SetFocus(bool focused)
		{
			SupplyMainManager.Instance.SetControllDone(focused);
			RemoveAllHover();
			if (focused)
			{
				_shipBanner[currentIdx].Hover(enabled: true);
			}
		}

		public void ProcessRecoveryAnimation()
		{
			for (int i = 0; i < shipCount; i++)
			{
				if (_shipBanner[i].selected)
				{
					_shipBanner[i].ProcessRecoveryAnimation();
				}
			}
		}
	}
}
