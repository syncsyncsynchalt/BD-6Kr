using local.models;
using UnityEngine;

namespace KCV
{
	public class CommonShipSupplyState : MonoBehaviour
	{
		public enum SupplyState
		{
			None,
			Green,
			Yellow,
			Red
		}

		[SerializeField]
		private UISprite FuelState;

		[SerializeField]
		private UISprite AmmoState;

		private SupplyState _iFuelState;

		private SupplyState _iAmmoState;

		public SupplyState fuelState => _iFuelState;

		public SupplyState ammoState => _iAmmoState;

		public bool isEitherSupplyNeeds => _iFuelState != SupplyState.Green || _iAmmoState != SupplyState.Green;

		private void OnDestroy()
		{
			Mem.Del(ref FuelState);
			Mem.Del(ref AmmoState);
			Mem.Del(ref _iFuelState);
			Mem.Del(ref _iAmmoState);
		}

		public void setSupplyState(ShipModel ship)
		{
			if (ship.AmmoRate >= 100.0)
			{
				AmmoState.spriteName = "icon_green";
				_iAmmoState = SupplyState.Green;
			}
			else if (ship.AmmoRate > 50.0 && ship.AmmoRate < 100.0)
			{
				AmmoState.spriteName = "icon_yellow";
				_iAmmoState = SupplyState.Yellow;
			}
			else
			{
				_iAmmoState = SupplyState.Red;
				AmmoState.spriteName = "icon_red";
			}
			if (ship.FuelRate >= 100.0)
			{
				FuelState.spriteName = "icon_green";
				_iFuelState = SupplyState.Green;
			}
			else if (ship.FuelRate > 50.0 && ship.FuelRate < 100.0)
			{
				FuelState.spriteName = "icon_yellow";
				_iFuelState = SupplyState.Yellow;
			}
			else
			{
				FuelState.spriteName = "icon_red";
				_iFuelState = SupplyState.Red;
			}
		}
	}
}
