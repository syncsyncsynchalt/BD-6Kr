using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreListChild : UIScrollListChild<FurnitureModel>
	{
		[SerializeField]
		private UILabel CoinValue;

		[SerializeField]
		private UILabel Name;

		[SerializeField]
		private UILabel Detail;

		[SerializeField]
		private UISprite[] Stars;

		[SerializeField]
		private UILabel SoldOut;

		[SerializeField]
		private UITexture texture;

		protected override void InitializeChildContents(FurnitureModel model, bool clickable)
		{
			CoinValue.textInt = model.Price;
			Name.text = model.Name;
			Detail.text = model.Description;
			SetStars(model.Rarity);
			SoldOut.enabled = model.IsPossession();
			mButton_Action.isEnabled = clickable;
			texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(model.Type, model.MstId);
		}

		private void SetStars(int num)
		{
			for (int i = 0; i < Stars.Length; i++)
			{
				Stars[i].SetActive(num > i);
			}
		}
	}
}
