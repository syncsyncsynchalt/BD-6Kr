using KCV.View;
using local.models;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeSummary : BaseUISummary<RevampRecipeModel>
	{
		[SerializeField]
		private UITexture mTexture_WeaponThumbnail;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

		[SerializeField]
		private UIButton mButton_Select;

		[SerializeField]
		private UILabel mLabel_WeaponName;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_DevKit;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private UILabel mLabel_RevampKit;

		public bool Disabled
		{
			get;
			private set;
		}

		public override void Initialize(int index, RevampRecipeModel model)
		{
			base.Initialize(index, model);
			mLabel_WeaponName.text = model.Slotitem.Name;
			mLabel_Fuel.text = model.Fuel.ToString();
			mLabel_Steel.text = model.Steel.ToString();
			mLabel_DevKit.text = model.DevKit.ToString();
			mLabel_Ammo.text = model.Ammo.ToString();
			mLabel_Bauxite.text = model.Baux.ToString();
			mLabel_RevampKit.text = model.RevKit.ToString();
		}

		public override void Hover()
		{
			base.Hover();
			if (!Disabled)
			{
				mButton_Select.SetState(UIButtonColor.State.Hover, immediate: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(mButton_Select, value: true);
			}
			else
			{
				Debug.LogWarning("TODO:選択不可時のHover表示");
			}
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			if (!Disabled)
			{
				mButton_Select.SetState(UIButtonColor.State.Normal, immediate: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(mButton_Select, value: false);
			}
			else
			{
				Debug.LogWarning("TODO:選択不可時のRemoveHover表示");
			}
		}

		public void OnDisabled()
		{
			Disabled = true;
			mButton_Select.enabled = false;
			mButton_Select.SetState(UIButtonColor.State.Disabled, immediate: true);
		}
	}
}
