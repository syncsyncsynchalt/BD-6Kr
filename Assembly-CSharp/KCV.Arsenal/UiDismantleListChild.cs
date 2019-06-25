using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiDismantleListChild : UIScrollListChild<ShipModel>
	{
		public ShipModel ship;

		[SerializeField]
		private UILabel _labelType;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UILabel _labelLv;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UISprite _checkBox;

		[SerializeField]
		private UISprite _ShipType;

		[SerializeField]
		private UISprite _LockType;

		private int _shipNum;

		private bool isCheck;

		public bool IsCheckList()
		{
			return isCheck;
		}

		protected override void InitializeChildContents(ShipModel model, bool clickable)
		{
			ship = model;
			refresh();
			setShip(model);
		}

		public void refresh()
		{
			_check.alpha = 0f;
			isCheck = false;
		}

		public void setShip(ShipModel _ship)
		{
			ship = _ship;
			_ShipType.spriteName = "ship" + ship.ShipType;
			_labelName.text = ship.Name;
			_labelLv.textInt = ship.Level;
			if (ship.IsLocked())
			{
				_LockType.spriteName = "lock_ship";
			}
			else if (ship.HasLocked())
			{
				_LockType.spriteName = "lock_on";
			}
			else
			{
				_LockType.spriteName = null;
			}
		}

		public void UpdateListSelect(bool enabled)
		{
			if (enabled)
			{
				_checkBox.spriteName = "check_bg_on";
			}
			else
			{
				_checkBox.spriteName = "check_bg";
			}
		}

		public void UpdateListSet(bool enabled)
		{
			if (enabled)
			{
				_check.alpha = 1f;
				isCheck = true;
			}
			else
			{
				_check.alpha = 0f;
				isCheck = false;
			}
		}
	}
}
