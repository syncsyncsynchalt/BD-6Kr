using KCV.View.Scroll;
using local.models;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemScrollParent : UIScrollListParent<SlotitemModel, UIRevampSlotItemScrollChild>
	{
		private RevampRecipeModel mRevampRecipeModel;

		public void Initialize(RevampRecipeModel recipeModel, SlotitemModel[] slotItemModels)
		{
			mRevampRecipeModel = recipeModel;
			base.Initialize(slotItemModels);
		}

		public RevampRecipeModel GetRevampRecipeModel()
		{
			return mRevampRecipeModel;
		}
	}
}
