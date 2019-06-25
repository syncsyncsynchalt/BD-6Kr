using KCV.View;
using local.models;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemSummary : BaseUISummary<SlotitemModel>
	{
		private const int LEVEL_MAX = 10;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UISprite mSprite_LevelMax;

		[SerializeField]
		private UIButton mButton_Action;

		[SerializeField]
		private GameObject mLevel;

		public override void Initialize(int index, SlotitemModel model)
		{
			base.Initialize(index, model);
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
		}

		public override void Hover()
		{
			base.Hover();
			mButton_Action.SetState(UIButtonColor.State.Hover, immediate: true);
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			mButton_Action.SetState(UIButtonColor.State.Normal, immediate: true);
		}
	}
}
