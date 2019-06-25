using KCV.InteriorStore;
using KCV.View.Scroll;
using local.models;

public class UIStoreListScroll : UIScrollListParent<FurnitureModel, UIStoreList>
{
	public new void Refresh(FurnitureModel[] furnitureModels)
	{
		base.Refresh(furnitureModels);
	}

	public UIStoreList[] GetChild()
	{
		return Views;
	}

	public FurnitureModel[] GetModels()
	{
		return Models;
	}
}
