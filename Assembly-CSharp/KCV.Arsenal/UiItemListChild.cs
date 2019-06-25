using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiItemListChild : UIScrollListChild<SlotitemModel>
	{
		public SlotitemModel Slotitem;

		[SerializeField]
		private UILabel _labelRea;

		[SerializeField]
		private UILabel _labelReaStars;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UISprite _checkBox;

		[SerializeField]
		private UISprite _SlotItem;

		[SerializeField]
		private UISprite _LockIcon;

		private int _itemNum;

		private bool _isCheck;

		public bool IsCheckList()
		{
			return _isCheck;
		}

		protected override void InitializeChildContents(SlotitemModel model, bool clickable)
		{
			Slotitem = model;
			Refresh();
			setItem();
		}

		public void Refresh()
		{
			_check.alpha = 0f;
			_isCheck = false;
		}

		public void setItem()
		{
			string text = string.Empty;
			for (int i = 0; i <= ((Slotitem.Rare >= 5) ? 4 : Slotitem.Rare); i++)
			{
				text += "★";
			}
			_labelRea.text = Util.RareToString(Slotitem.Rare);
			_labelReaStars.text = text;
			_labelName.text = Slotitem.Name;
			_SlotItem.spriteName = "icon_slot" + Slotitem.Type4;
			_LockIcon.spriteName = ((!Slotitem.IsLocked()) ? null : "lock_on");
			if (TaskMainArsenalManager.arsenalManager.IsSelected(Slotitem.MemId))
			{
				UpdateListSet(enabled: true);
			}
			else
			{
				UpdateListSet(enabled: false);
			}
		}

		public void UpdateListSelect(bool enabled)
		{
			_checkBox.spriteName = ((!enabled) ? "check_bg" : "check_bg_on");
		}

		public void UpdateListSet(bool enabled)
		{
			_check.alpha = ((!enabled) ? 0f : 1f);
			_isCheck = enabled;
		}
	}
}
