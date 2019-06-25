using KCV.InteriorStore;
using KCV.View.Scroll;
using local.models;

public class UIListScroll : UIScrollListParent<FurnitureModel, UIInteriorList>
{
	public UIInteriorList[] GetChild()
	{
		return Views;
	}

	public FurnitureModel[] GetModels()
	{
		return Models;
	}

	public new void Initialize(FurnitureModel[] models)
	{
		this.SetActive(isActive: true);
		base.Initialize(models);
	}
}
