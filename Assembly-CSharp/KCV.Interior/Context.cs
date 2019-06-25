using Common.Enum;
using local.models;

namespace KCV.Interior
{
	internal class Context
	{
		private FurnitureKinds mSelectedKind;

		private FurnitureModel mSelectedFurnitureModel;

		public FurnitureKinds CurrentCategory => mSelectedKind;

		public FurnitureModel SelectedFurniture => mSelectedFurnitureModel;

		public void SetSelectFurnitureKind(FurnitureKinds furnitureKind)
		{
			mSelectedKind = furnitureKind;
		}

		public void SetSelectedFurniture(FurnitureModel furnitureModel)
		{
			mSelectedFurnitureModel = furnitureModel;
		}
	}
}
