using KCV.Utils;
using local.managers;
using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Buttons : MonoBehaviour
	{
		[SerializeField]
		protected UIButton LeftButton;

		[SerializeField]
		protected UIButton RightButton;

		public LockSwitch LockSwitch;

		[SerializeField]
		protected UIButtonMessage BackBG;

		protected ShipModel ship;

		[SerializeField]
		protected UIButtonManager buttonManager;

		protected bool isDeckShipDetail;

		public void SetDeckShipDetailButtons(ShipModel ship, IOrganizeManager manager = null, MonoBehaviour CallBackTarget = null)
		{
			isDeckShipDetail = true;
			this.ship = ship;
			bool flag = manager?.IsValidShip(ship.MemId) ?? IsValidShip();
			LeftButton.SetActive(isActive: true);
			setChangeButton(LeftButton, flag, CallBackTarget);
			setUnsetButton(RightButton, manager, CallBackTarget);
			buttonManager.nowForcusButton = ((!flag) ? RightButton : LeftButton);
			LockSwitch.SetActive(isActive: false);
			if (ship.IsBling() && ship.IsInDeck() != -1)
			{
				buttonManager.setFocus(1);
			}
			GameObject backBG = (!(CallBackTarget == null)) ? CallBackTarget.gameObject : null;
			setBackBG(backBG);
		}

		public void SetListShipDetailButtons(ShipModel ship, int deckId, IOrganizeManager manager = null, int ShipIndex = 0, MonoBehaviour CallBackTarget = null)
		{
			isDeckShipDetail = false;
			this.ship = ship;
			bool flag = manager?.IsValidChange(deckId, ShipIndex, ship.MemId) ?? IsValidChange(deckId, ShipIndex, ship.MemId);
			LockSwitch.setIcon(ship);
			LeftButton.SetActive(isActive: false);
			setChangeButton(RightButton, flag, CallBackTarget);
			buttonManager.nowForcusButton = ((!flag) ? null : RightButton);
			LockSwitch.SetActive(isActive: true);
			GameObject backBG = (!(CallBackTarget == null)) ? CallBackTarget.gameObject : null;
			setBackBG(backBG);
		}

		protected void setChangeButton(UIButton button, bool isValidChange, MonoBehaviour Target = null)
		{
			button.normalSprite = "btn_set";
			button.hoverSprite = "btn_set_on";
			button.pressedSprite = "btn_set_on";
			button.disabledSprite = "btn_set_off";
			button.isEnabled = isValidChange;
			if (isValidChange)
			{
				if (isDeckShipDetail)
				{
					MonoBehaviour target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetDetailTask();
					button.onClick = Util.CreateEventDelegateList(target, "SetBtnEL", null);
				}
				else
				{
					MonoBehaviour target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetListDetailTask();
					button.onClick = Util.CreateEventDelegateList(target, "ChangeButtonEL", null);
				}
				button.SetState(UIButtonColor.State.Hover, immediate: false);
			}
		}

		protected virtual void setUnsetButton(UIButton button, IOrganizeManager manager = null, MonoBehaviour Target = null)
		{
			button.normalSprite = "btn_reset";
			button.hoverSprite = "btn_reset_on";
			button.pressedSprite = "btn_reset_on";
			button.disabledSprite = "btn_reset_off";
			MonoBehaviour target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetDetailTask();
			button.onClick = Util.CreateEventDelegateList(target, "ResetBtnEL", null);
			button.isEnabled = (manager?.IsValidUnset(ship.MemId) ?? IsValidUnset());
			button.SetState((!button.isEnabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, immediate: true);
		}

		protected virtual void setBackBG(GameObject Target = null)
		{
			if (isDeckShipDetail)
			{
				GameObject target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetDetailTask().gameObject;
				BackBG.target = target;
				BackBG.functionName = "BackMaskEL";
				BackBG.trigger = UIButtonMessage.Trigger.OnClick;
			}
			else
			{
				GameObject target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetListDetailTask().gameObject;
				BackBG.target = target;
				BackBG.functionName = "BackDataEL";
				BackBG.trigger = UIButtonMessage.Trigger.OnClick;
			}
		}

		public virtual void UpdateButton(bool isLeft, OrganizeManager manager = null)
		{
			if ((manager?.IsValidShip(ship.MemId) ?? IsValidShip()) && (manager?.IsValidUnset(ship.MemId) ?? IsValidUnset()))
			{
				if (isLeft && buttonManager.nowForcusButton != LeftButton)
				{
					RightButton.SetState(UIButtonColor.State.Normal, immediate: true);
					LeftButton.SetState(UIButtonColor.State.Hover, immediate: true);
					buttonManager.nowForcusButton = LeftButton;
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				else if (!isLeft && buttonManager.nowForcusButton != RightButton)
				{
					RightButton.SetState(UIButtonColor.State.Hover, immediate: true);
					LeftButton.SetState(UIButtonColor.State.Normal, immediate: true);
					buttonManager.nowForcusButton = RightButton;
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
		}

		public virtual void Decide()
		{
			buttonManager.Decide();
		}

		private bool IsValidShip()
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidShip(ship.MemId);
		}

		private bool IsValidChange(int deck_id, int selected_index, int ship_mem_id)
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidChange(deck_id, selected_index, ship_mem_id);
		}

		private bool IsValidUnset()
		{
			Debug.Log(ship.MemId);
			Debug.Log(ship.Name);
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidUnset(ship.MemId);
		}
	}
}
