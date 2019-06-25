using local.models;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class RemodelShipListChild : UIScrollListChild<RemodelListShip>
	{
		[SerializeField]
		private UISprite ShipType;

		[SerializeField]
		private UILabel ShipName;

		[SerializeField]
		private UISprite[] ParamIcons;

		[SerializeField]
		private UILabel Level;

		protected override void InitializeChildContents(RemodelListShip ListItem, bool clickable)
		{
			if (ListItem.Option == RemodelListShip.ListItemOption.Option)
			{
				ShipName.text = "はずす";
				ShipType.spriteName = string.Empty;
				Level.text = string.Empty;
				UISprite[] paramIcons = ParamIcons;
				foreach (UISprite uISprite in paramIcons)
				{
					uISprite.enabled = false;
				}
			}
			else
			{
				ShipModel shipModel = ListItem.shipModel;
				ShipType.spriteName = "ship" + shipModel.ShipType;
				ShipName.text = shipModel.Name;
				Level.text = "LV " + shipModel.Level.ToString();
				ParamIcons[0].enabled = (shipModel.PowUpKaryoku > 0);
				ParamIcons[1].enabled = (shipModel.PowUpRaisou > 0);
				ParamIcons[2].enabled = (shipModel.PowUpSoukou > 0);
				ParamIcons[3].enabled = (shipModel.PowUpTaikuu > 0);
			}
		}
	}
}
