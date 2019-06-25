using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIStoreList : UIScrollListChild<FurnitureModel>
	{
		public FurnitureModel StoreItem;

		private UILabel _labelName;

		private UILabel _labelPrice;

		private UILabel _labelSoldOut;

		private bool isCheck;

		public bool IsCheckList()
		{
			return isCheck;
		}

		protected override void InitializeChildContents(FurnitureModel model, bool clickable)
		{
			StoreItem = model;
			init();
			setList();
		}

		public void init()
		{
			_labelName = ((Component)base.transform.FindChild("Label_name")).GetComponent<UILabel>();
			_labelPrice = ((Component)base.transform.FindChild("FCoin")).GetComponent<UILabel>();
			_labelSoldOut = ((Component)base.transform.FindChild("SoldOut")).GetComponent<UILabel>();
			isCheck = false;
		}

		public void setList()
		{
			_labelName.text = StoreItem.Name;
			_labelPrice.textInt = StoreItem.Price;
			_labelPrice.SetActive(!StoreItem.IsPossession());
			_labelSoldOut.SetActive(StoreItem.IsPossession());
		}
	}
}
