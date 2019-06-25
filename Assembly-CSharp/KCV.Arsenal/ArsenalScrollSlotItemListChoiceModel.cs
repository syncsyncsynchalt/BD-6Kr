using local.models;

namespace KCV.Arsenal
{
	public class ArsenalScrollSlotItemListChoiceModel
	{
		private SlotitemModel mSlotItemModel;

		public bool Selected
		{
			get;
			private set;
		}

		public ArsenalScrollSlotItemListChoiceModel(SlotitemModel slotItemModel, bool selected)
		{
			mSlotItemModel = slotItemModel;
			Selected = selected;
		}

		public SlotitemModel GetSlotItemModel()
		{
			return mSlotItemModel;
		}
	}
}
