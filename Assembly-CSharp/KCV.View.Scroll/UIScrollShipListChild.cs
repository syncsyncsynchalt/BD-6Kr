using local.models;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class UIScrollShipListChild : UIScrollListChild<ShipModel>
	{
		[SerializeField]
		private UILabel mLabel_ShipName;

		[SerializeField]
		private UITexture mTexture_Ship;

		protected override void InitializeChildContents(ShipModel model, bool clickable)
		{
			mLabel_ShipName.text = model.Name;
			mTexture_Ship.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, 1);
			mLabel_ShipName.text = "SortLevel::" + base.SortIndex;
		}
	}
}
