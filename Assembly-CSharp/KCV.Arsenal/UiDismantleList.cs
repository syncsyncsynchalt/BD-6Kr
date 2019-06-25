using local.models;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiDismantleList : MonoBehaviour
	{
		[SerializeField]
		public ShipModel ship;

		[SerializeField]
		private UISprite _teamIcon;

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

		private int _shipNum;

		private bool isCheck;

		public bool IsCheckList()
		{
			return isCheck;
		}

		public void init()
		{
			_labelType = ((Component)base.transform.FindChild("LabelType")).GetComponent<UILabel>();
			_labelName = ((Component)base.transform.FindChild("LabelName")).GetComponent<UILabel>();
			_labelLv = ((Component)base.transform.FindChild("LabelLevel")).GetComponent<UILabel>();
			_check = ((Component)base.transform.FindChild("Check")).GetComponent<UISprite>();
			_checkBox = ((Component)base.transform.FindChild("CheckBox")).GetComponent<UISprite>();
			_check.alpha = 0f;
			isCheck = false;
		}

		public void setShip(int num, ShipModel _ship)
		{
			_shipNum = num;
			ship = _ship;
			_labelType.text = ship.ShipTypeName;
			_labelName.text = ship.Name;
			_labelLv.textInt = ship.Level;
		}

		public void setTeamIcon()
		{
			ShipModel[] shipList = TaskMainArsenalManager.arsenalManager.GetShipList();
			if (shipList[_shipNum].IsInDeck() == 0)
			{
				_teamIcon.alpha = 1f;
				_teamIcon.spriteName = "icon-d1_k";
			}
			else if (shipList[_shipNum].IsInDeck() == -1)
			{
				_teamIcon.alpha = 1f;
				_teamIcon.spriteName = "icon-d" + shipList[_shipNum].IsInDeck() + "_on";
			}
			else
			{
				_teamIcon.alpha = 0f;
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
