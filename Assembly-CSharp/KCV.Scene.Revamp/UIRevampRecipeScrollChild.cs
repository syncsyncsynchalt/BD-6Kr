using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeScrollChild : UIScrollListChild<RevampRecipeModel>
	{
		[SerializeField]
		private UITexture mTexture_WeaponThumbnail;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

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

		[SerializeField]
		private UIButton mButton_Select;

		[SerializeField]
		private UISprite mSprite_ButtonState;

		protected override void InitializeChildContents(RevampRecipeModel model, bool clickable)
		{
			StopBlink();
			if (model != null)
			{
				mTexture_WeaponThumbnail.mainTexture = (Resources.Load("Textures/SlotItems/" + model.Slotitem.MstId + "/2") as Texture);
				mLabel_WeaponName.text = model.Slotitem.Name;
				mSprite_WeaponTypeIcon.spriteName = "icon_slot" + model.Slotitem.Type4;
				mLabel_Fuel.text = model.Fuel.ToString();
				mLabel_Steel.text = model.Steel.ToString();
				mLabel_DevKit.text = model.DevKit.ToString();
				mLabel_Ammo.text = model.Ammo.ToString();
				mLabel_Bauxite.text = model.Baux.ToString();
				mLabel_RevampKit.text = model.RevKit.ToString();
				if (clickable)
				{
					mSprite_ButtonState.spriteName = "btn_select";
					mButton_Action.isEnabled = true;
					mButton_Select.isEnabled = true;
				}
				else
				{
					mSprite_ButtonState.spriteName = "btn_select_off";
					mButton_Action.isEnabled = false;
					mButton_Select.SetEnableCollider2D(enabled: false);
				}
			}
			else
			{
				mLabel_WeaponName.text = "-";
				mSprite_WeaponTypeIcon.spriteName = "icon_slot_notset";
			}
		}

		public override void Hover()
		{
			base.Hover();
			if (base.mIsClickable)
			{
				mSprite_ButtonState.spriteName = "btn_select_on";
			}
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			if (base.mIsClickable)
			{
				mSprite_ButtonState.spriteName = "btn_select";
			}
			else
			{
				mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}
	}
}
