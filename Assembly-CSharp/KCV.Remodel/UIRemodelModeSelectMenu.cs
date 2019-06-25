using KCV.PopupString;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelModeSelectMenu : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.3f;

		[SerializeField]
		private UIButton mButton_SlotItemChange;

		[SerializeField]
		private UIButton mButton_Modernization;

		[SerializeField]
		private UIButton mButton_Remodel;

		private UIButton[] mButtonsFocusable;

		private UIButton mCurrentButtonFocus;

		private Vector3 showPos = new Vector3(240f, 0f);

		private Vector3 hidePos = new Vector3(960f, 0f);

		private bool firstUpdateWhenShow = true;

		private KeyControl keyController;

		private bool validShip;

		private string validShipReason;

		private ShipModel ship;

		private UIButton _BeforeButton;

		private bool isShown;

		private void Awake()
		{
			base.transform.localPosition = hidePos;
			mButton_Modernization.OnEnableAndOnDisableChangeState = true;
			mButton_Remodel.OnEnableAndOnDisableChangeState = true;
			mButton_SlotItemChange.OnEnableAndOnDisableChangeState = true;
		}

		public void Init(KeyControl keyController, bool remodelable)
		{
			ChangeFocusButton(null);
			this.keyController = keyController;
			ship = UserInterfaceRemodelManager.instance.focusedShipModel;
			validShip = UserInterfaceRemodelManager.instance.IsValidShip();
			bool flag = UserInterfaceRemodelManager.instance.mRemodelManager.IsValidGradeUp(ship);
			List<UIButton> list = new List<UIButton>();
			list.Add(mButton_SlotItemChange);
			InitButton(list, mButton_SlotItemChange, enabled: true);
			InitButton(list, mButton_Modernization, validShip);
			InitButton(list, mButton_Remodel, validShip && flag);
			_BeforeButton = mButton_SlotItemChange;
			mButtonsFocusable = list.ToArray();
			ChangeFocusButton(mButton_SlotItemChange);
		}

		private void InitButton(List<UIButton> list, UIButton button, bool enabled)
		{
			button.hover = Color.white;
			button.defaultColor = Color.white;
			button.pressed = Color.white;
			button.disabledColor = Color.white;
			button.enabled = enabled;
			if (enabled)
			{
				list.Add(button);
				button.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			else
			{
				button.SetState(UIButtonColor.State.Disabled, immediate: true);
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.3f, delegate
			{
				isShown = true;
			});
			if (mCurrentButtonFocus != null)
			{
				ChangeFocusButton(mCurrentButtonFocus);
			}
			else
			{
				ChangeFocusButton(mButtonsFocusable[0]);
			}
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.3f, delegate
				{
					base.gameObject.SetActive(false);
				});
				return;
			}
			base.transform.localPosition = hidePos;
			base.gameObject.SetActive(false);
		}

		private void SwitchButton(int changeIndex)
		{
			int num = Array.IndexOf(mButtonsFocusable, mCurrentButtonFocus);
			int num2 = num + changeIndex;
			if (0 <= num2 && num2 < mButtonsFocusable.Length)
			{
				ChangeFocusButton(mButtonsFocusable[num2]);
			}
		}

		private void Update()
		{
			if (keyController == null || !isShown)
			{
				return;
			}
			if (keyController.IsLeftDown())
			{
				ChangeFocusButton(mButton_SlotItemChange);
			}
			else if (keyController.IsRightDown())
			{
				if (mButton_Modernization.enabled)
				{
					ChangeFocusButton(mButton_Modernization);
				}
			}
			else if (keyController.IsUpDown() && _BeforeButton != mButton_Modernization)
			{
				ChangeFocusButton(mButton_SlotItemChange);
			}
			else if (keyController.IsDownDown())
			{
				if (mButton_Remodel.enabled)
				{
					ChangeFocusButton(mButton_Remodel);
				}
			}
			else if (keyController.IsMaruDown())
			{
				if (mCurrentButtonFocus.Equals(mButton_SlotItemChange))
				{
					OnClickSlotItemChange();
				}
				else if (mCurrentButtonFocus.Equals(mButton_Modernization))
				{
					OnClickModernization();
				}
				else if (mCurrentButtonFocus.Equals(mButton_Remodel))
				{
					OnClickRemodel();
				}
			}
			else if (keyController.IsBatuDown())
			{
				Back();
			}
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchBack()
		{
			if (isShown)
			{
				Back();
			}
		}

		private void Back()
		{
			if (isShown)
			{
				UserInterfaceRemodelManager.instance.Back2ShipSelect();
			}
		}

		public bool IsValidSlotItemChange()
		{
			bool flag = !validShip || (ship.SlotitemList.Count == 0 && !ship.HasExSlot() && ship.Level < 30);
			return !flag;
		}

		public void PopUpFailOpenSummary()
		{
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCancel1);
			if (ship.IsInActionEndDeck())
			{
				validShipReason = Util.getPopupMessage(PopupMess.ActionEndShip);
			}
			else if (ship.SlotitemList.Count == 0)
			{
				validShipReason = Util.getPopupMessage(PopupMess.NoSlot);
			}
			else if (ship.IsInRepair())
			{
				validShipReason = Util.getPopupMessage(PopupMess.NowRepairing);
			}
			else if (ship.IsBling())
			{
				validShipReason = Util.getPopupMessage(PopupMess.NowBlinging);
			}
			else if (ship.IsInEscortDeck() != -1)
			{
				validShipReason = Util.getPopupMessage(PopupMess.InEscortShip);
			}
			else if (ship.IsInMission())
			{
				validShipReason = Util.getPopupMessage(PopupMess.InMissionShip);
			}
			else
			{
				validShipReason = string.Empty;
			}
			CommonPopupDialog.Instance.StartPopup(validShipReason);
		}

		public void OnClickSlotItemChange()
		{
			if (isShown)
			{
				if (!IsValidSlotItemChange())
				{
					PopUpFailOpenSummary();
					return;
				}
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				ChangeFocusButton(mButton_SlotItemChange);
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkou();
			}
		}

		public void OnClickModernization()
		{
			if (isShown)
			{
				ChangeFocusButton(mButton_Modernization);
				UserInterfaceRemodelManager.instance.Forward2KindaikaKaishu();
			}
		}

		public void OnClickRemodel()
		{
			if (isShown)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				ChangeFocusButton(mButton_Remodel);
				UserInterfaceRemodelManager.instance.Forward2Kaizo();
			}
		}

		private void ChangeFocusButton(UIButton target)
		{
			if (mCurrentButtonFocus != null)
			{
				mCurrentButtonFocus.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			mCurrentButtonFocus = target;
			if (mCurrentButtonFocus != null)
			{
				mCurrentButtonFocus.SetState(UIButtonColor.State.Hover, immediate: true);
			}
			if (mCurrentButtonFocus != _BeforeButton)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCursolMove);
				_BeforeButton = mCurrentButtonFocus;
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mButtonsFocusable);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_SlotItemChange);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Modernization);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Remodel);
			UserInterfacePortManager.ReleaseUtils.Release(ref mCurrentButtonFocus);
			UserInterfacePortManager.ReleaseUtils.Release(ref _BeforeButton);
			keyController = null;
			ship = null;
		}
	}
}
