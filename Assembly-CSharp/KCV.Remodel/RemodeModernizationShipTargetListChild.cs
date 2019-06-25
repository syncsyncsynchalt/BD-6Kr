using local.models;

namespace KCV.Remodel
{
	public class RemodeModernizationShipTargetListChild
	{
		public enum ListItemOption
		{
			Model,
			UnSet
		}

		public ListItemOption mOption
		{
			get;
			private set;
		}

		public ShipModel mShipModel
		{
			get;
			private set;
		}

		public int mType
		{
			get;
			private set;
		}

		public RemodeModernizationShipTargetListChild(ListItemOption option, ShipModel model)
		{
			mOption = option;
			mShipModel = model;
			if (model != null)
			{
				mType = ((0 < model.PowUpKaryoku) ? 16 : 0) + ((0 < model.PowUpRaisou) ? 8 : 0) + ((0 < model.PowUpSoukou) ? 4 : 0) + ((0 < model.PowUpTaikuu) ? 2 : 0) + ((0 < model.PowUpLucky) ? 1 : 0);
			}
			else
			{
				mType = 0;
			}
		}
	}
}
