using local.models;

namespace KCV.View.Scroll
{
	public class UIScrollShipListParent : UIScrollListParent<ShipModel, UIScrollShipListChild>
	{
		public new void Initialize(ShipModel[] models)
		{
			base.Initialize(models);
		}
	}
}
