using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIInteriorList : UIScrollListChild<FurnitureModel>
	{
		public FurnitureModel Interior;

		private UILabel _labelName;

		private UISprite _spEquip;

		private bool isCheck;

		public bool IsCheckList()
		{
			return isCheck;
		}

		private void Start()
		{
		}

		protected override void InitializeChildContents(FurnitureModel model, bool clickable)
		{
			Interior = model;
			init();
			setList();
		}

		public void init()
		{
			_labelName = ((Component)base.transform.FindChild("Label_name")).GetComponent<UILabel>();
			_spEquip = ((Component)base.transform.FindChild("Equip")).GetComponent<UISprite>();
			isCheck = false;
		}

		public void setList()
		{
			_labelName.text = Interior.Name;
		}

		private void Update()
		{
		}
	}
}
