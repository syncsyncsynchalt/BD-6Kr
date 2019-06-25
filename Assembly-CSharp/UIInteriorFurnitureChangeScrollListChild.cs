using KCV.View.Scroll;
using UnityEngine;

public class UIInteriorFurnitureChangeScrollListChild : UIScrollListChild<UIInteriorFurnitureChangeScrollListChildModel>
{
	[SerializeField]
	private Transform mEquipMark;

	[SerializeField]
	private UILabel mLabel_Name;

	protected override void InitializeChildContents(UIInteriorFurnitureChangeScrollListChildModel model, bool isClickable)
	{
		base.InitializeChildContents(model, isClickable);
		mLabel_Name.text = base.Model.GetName();
		if (model.IsConfiguredInDeck())
		{
			mEquipMark.SetActive(isActive: true);
		}
		else
		{
			mEquipMark.SetActive(isActive: false);
		}
	}
}
