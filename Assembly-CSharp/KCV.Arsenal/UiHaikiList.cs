using local.models;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiHaikiList : MonoBehaviour
	{
		public SlotitemModel item;

		[SerializeField]
		private UILabel _labelRea;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UISprite _checkBox;

		private int _itemNum;

		private bool isCheck;

		public bool IsCheckList()
		{
			return isCheck;
		}

		public void init()
		{
			_labelRea = ((Component)base.transform.FindChild("LabelRea")).GetComponent<UILabel>();
			_labelName = ((Component)base.transform.FindChild("LabelName")).GetComponent<UILabel>();
			_check = ((Component)base.transform.FindChild("Check")).GetComponent<UISprite>();
			_checkBox = ((Component)base.transform.FindChild("CheckBox")).GetComponent<UISprite>();
			_check.alpha = 0f;
			isCheck = false;
		}

		public void setItem(int num, SlotitemModel item)
		{
			_itemNum = num;
			this.item = item;
			_labelRea.textInt = this.item.Rare;
			_labelName.text = this.item.Name;
			if (TaskMainArsenalManager.arsenalManager.IsSelected(this.item.MemId))
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
