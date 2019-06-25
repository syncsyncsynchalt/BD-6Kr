using local.models;
using UnityEngine;

namespace KCV.Supply
{
	public class UISupplyDeckShipBanner : UISupplyCommonShipBanner
	{
		[SerializeField]
		private CommonShipBanner _shipBanner;

		[SerializeField]
		private UITexture _shutterL;

		[SerializeField]
		private UITexture _shutterR;

		public void Init(Vector3 pos)
		{
			Init();
			base.transform.localPosition = pos;
		}

		public override void SetBanner(ShipModel ship, int idx)
		{
			base.SetBanner(ship, idx);
			if (base.enabled)
			{
				_shipBanner.SetActive(isActive: true);
				_shipBanner.SetShipData(ship);
			}
		}

		public override void SetEnabled(bool enabled)
		{
			base.SetEnabled(enabled);
			_shipBanner.SetActive(enabled);
			_shutterL.SetActive(!enabled);
			_shutterR.SetActive(!enabled);
		}

		public void OnClick()
		{
			if (IsSelectable() && SupplyMainManager.Instance.IsShipSelectableStatus())
			{
				SupplyMainManager.Instance.change_2_SHIP_SELECT(defaultFocus: true);
				SupplyMainManager.Instance._shipBannerContainer.UpdateCurrentItem(idx);
				SupplyMainManager.Instance._shipBannerContainer.SwitchCurrentSelected();
			}
		}
	}
}
