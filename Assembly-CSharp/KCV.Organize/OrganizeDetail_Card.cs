using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Card : MonoBehaviour
	{
		[SerializeField]
		private UITexture ShipCard;

		[SerializeField]
		private UISprite StateIcon;

		[SerializeField]
		private UISprite FatigueIcon;

		[SerializeField]
		private UISprite FatigueMask;

		[SerializeField]
		private Transform Ring;

		public void SetShipCard(ShipModel ship)
		{
			BaseShipCard component = GetComponent<BaseShipCard>();
			component.Init(ship, ShipCard);
			component.UpdateFatigue(ship.ConditionState, FatigueIcon, FatigueMask);
			component.UpdateStateIcon(StateIcon);
			if (ship.IsMarriage())
			{
				Ring.SetActive(isActive: true);
			}
			else
			{
				Ring.SetActive(isActive: false);
			}
		}

		public void Release()
		{
			Resources.UnloadAsset(ShipCard.mainTexture);
		}

		private void OnDestroy()
		{
			ShipCard = null;
			StateIcon = null;
			FatigueIcon = null;
			FatigueMask = null;
			Ring = null;
		}
	}
}
