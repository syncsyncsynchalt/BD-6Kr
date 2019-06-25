using local.models;

namespace KCV.Interior
{
	public class UIInteriorFurnitureChangeScrollListChildModelNew
	{
		private int mDeckId;

		private FurnitureModel mModel;

		public UIInteriorFurnitureChangeScrollListChildModelNew(int deckId, FurnitureModel model)
		{
			mDeckId = deckId;
			mModel = model;
		}

		public string GetName()
		{
			return mModel.Name;
		}

		public int GetDeckId()
		{
			return mDeckId;
		}

		public bool IsConfiguredInDeck()
		{
			return mModel.GetSettingFlg(mDeckId);
		}

		public FurnitureModel GetFurnitureModel()
		{
			return mModel;
		}
	}
}
