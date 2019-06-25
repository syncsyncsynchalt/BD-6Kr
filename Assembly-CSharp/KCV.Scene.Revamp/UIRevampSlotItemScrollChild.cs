using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemScrollChild : UIScrollListChild<SlotitemModel>
	{
		private const int LEVEL_MAX = 10;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UISprite mSprite_LevelMax;

		[SerializeField]
		private GameObject mLevel;

		protected override void InitializeChildContents(SlotitemModel model, bool clickable)
		{
			if (model != null)
			{
				if (model.Level == 10)
				{
					mSprite_LevelMax.SetActive(isActive: true);
				}
				else if (0 < model.Level && model.Level < 10)
				{
					mSprite_LevelMax.SetActive(isActive: false);
					mLevel.SetActive(true);
					mLabel_Level.text = model.Level.ToString();
				}
				else
				{
					mLevel.SetActive(false);
				}
				mLabel_Name.text = model.Name;
				mSprite_WeaponTypeIcon.spriteName = "icon_slot" + model.Type4;
			}
			else
			{
				mLabel_Name.text = "-";
				mSprite_WeaponTypeIcon.spriteName = "icon_slot_notset";
			}
		}
	}
}
