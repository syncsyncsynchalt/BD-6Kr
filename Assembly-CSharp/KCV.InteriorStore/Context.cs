using Common.Enum;
using local.models;

namespace KCV.InteriorStore
{
	internal class Context
	{
		public FurnitureKinds SelectedCategory
		{
			get;
			private set;
		}

		public FurnitureModel SelectedFurniture
		{
			get;
			private set;
		}

		public void SetSelectedCategory(FurnitureKinds furnitureKind)
		{
			SelectedCategory = furnitureKind;
		}

		public void SetSelectedFurniture(FurnitureModel furnitureModel)
		{
			SelectedFurniture = furnitureModel;
		}
	}
}
