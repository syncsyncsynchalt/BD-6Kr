using local.models;

namespace KCV.View.Scroll
{
	public class RemodelListShip
	{
		public enum ListItemOption
		{
			Model,
			Option
		}

		public ListItemOption Option
		{
			get;
			private set;
		}

		public ShipModel shipModel
		{
			get;
			private set;
		}

		public RemodelListShip(ListItemOption option, ShipModel model)
		{
			Option = option;
			shipModel = model;
		}
	}
}
