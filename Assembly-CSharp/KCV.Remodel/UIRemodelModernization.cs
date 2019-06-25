using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	[RequireComponent(typeof(UIPanel))]
	public class UIRemodelModernization : MonoBehaviour, UIRemodelView, IBannerResourceManage
	{
		private const float ANIMATION_DURATION = 0.2f;

		private UIPanel mPanelThis;

		[SerializeField]
		private UIButton mButton_Start;

		[SerializeField]
		private UIRemodeModernzationTargetShip[] mUIRemodeModernzationTargetShip_TargetShips;

		[SerializeField]
		private UIButton mButton_TouchBack;

		[SerializeField]
		private ButtonLightTexture btnLight;

		private Vector3 showPos = new Vector3(315f, 0f, 0f);

		private Vector3 hidePos = new Vector3(900f, 0f, 0f);

		private UIRemodeModernzationTargetShip mCurrentFocusTargetShipSlot;

		private UIRemodeModernzationTargetShip _BeforeTargetSlot;

		private KeyControl mKeyController;

		private ShipModel mModernzationShipModel;

		private int _CursorDown;

		private bool isShown;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			base.transform.localPosition = hidePos;
			_BeforeTargetSlot = mUIRemodeModernzationTargetShip_TargetShips[0];
			mButton_TouchBack.SetActive(isActive: false);
			UIRemodeModernzationTargetShip[] array = mUIRemodeModernzationTargetShip_TargetShips;
			foreach (UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip in array)
			{
				uIRemodeModernzationTargetShip.SetOnUIRemodeModernzationTargetShipActionListener(UIRemodeModernzationTargetShipAction);
			}
			_CursorDown = 0;
		}

		public void Initialize(KeyControl keyController, ShipModel shipModel)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			mKeyController = keyController;
			mModernzationShipModel = shipModel;
			UIRemodeModernzationTargetShip[] array = mUIRemodeModernzationTargetShip_TargetShips;
			foreach (UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip in array)
			{
				uIRemodeModernzationTargetShip.UnSet();
			}
			UpdateStartButtonEnabled();
		}

		private void Update()
		{
			if (mKeyController == null || !base.enabled || !isShown)
			{
				return;
			}
			if (mKeyController.IsUpDown())
			{
				if (mCurrentFocusTargetShipSlot == null)
				{
					ChangeFocusSlot(mUIRemodeModernzationTargetShip_TargetShips[mUIRemodeModernzationTargetShip_TargetShips.Length - 1]);
					mButton_Start.SetState(UIButtonColor.State.Normal, immediate: true);
					return;
				}
				int num = Array.IndexOf(mUIRemodeModernzationTargetShip_TargetShips, mCurrentFocusTargetShipSlot);
				int num2 = num - 1;
				if (0 <= num2)
				{
					ChangeFocusSlot(mUIRemodeModernzationTargetShip_TargetShips[num2]);
				}
			}
			else if (mKeyController.IsDownDown() || _CursorDown != 0)
			{
				do
				{
					if (!(mCurrentFocusTargetShipSlot == null))
					{
						int num3 = Array.IndexOf(mUIRemodeModernzationTargetShip_TargetShips, mCurrentFocusTargetShipSlot);
						int num4 = num3 + 1;
						if (num4 < mUIRemodeModernzationTargetShip_TargetShips.Length)
						{
							ChangeFocusSlot(mUIRemodeModernzationTargetShip_TargetShips[num4], (_CursorDown != 0) ? true : false);
						}
						else if (num4 == mUIRemodeModernzationTargetShip_TargetShips.Length && CanModernize())
						{
							ChangeFocusSlot(null, (_CursorDown != 0) ? true : false);
							mButton_Start.SetState(UIButtonColor.State.Hover, immediate: true);
						}
					}
				}
				while (--_CursorDown > 0);
				_CursorDown = 0;
			}
			else if (mKeyController.IsMaruDown())
			{
				if (mCurrentFocusTargetShipSlot != null)
				{
					Forward4Select();
				}
				else
				{
					Forward4Confirm();
				}
			}
			else if (mKeyController.IsBatuDown())
			{
				Back();
			}
		}

		private void Forward4Select()
		{
			if (isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Forward2KindaikaKaishuSozaiSentaku(GetSetShipModels());
			}
		}

		private void Forward4Confirm()
		{
			if (isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Forward2KindaikaKaishuKakunin(GetModernizationShipModel(), GetSetShipModels());
			}
		}

		private void Back()
		{
			if (isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UnSetAll();
				RemoveFocus();
				UserInterfaceRemodelManager.instance.Back2ModeSelect();
			}
		}

		public void SwitchChildEnabled()
		{
			mButton_TouchBack.SetActive(base.enabled);
			UIRemodeModernzationTargetShip[] array = mUIRemodeModernzationTargetShip_TargetShips;
			foreach (UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip in array)
			{
				uIRemodeModernzationTargetShip.SetEnabled(base.enabled);
			}
		}

		public ShipModel GetModernizationShipModel()
		{
			return mModernzationShipModel;
		}

		public List<ShipModel> GetSetShipModels()
		{
			List<ShipModel> list = new List<ShipModel>();
			UIRemodeModernzationTargetShip[] array = mUIRemodeModernzationTargetShip_TargetShips;
			foreach (UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip in array)
			{
				if (uIRemodeModernzationTargetShip.GetSlotInShip() != null)
				{
					list.Add(uIRemodeModernzationTargetShip.GetSlotInShip());
				}
			}
			return list;
		}

		public void UnSetAll()
		{
			UIRemodeModernzationTargetShip[] array = mUIRemodeModernzationTargetShip_TargetShips;
			foreach (UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip in array)
			{
				uIRemodeModernzationTargetShip.UnSet();
			}
			mButton_Start.SetState(UIButtonColor.State.Normal, immediate: true);
			UpdateStartButtonEnabled();
		}

		private void UIRemodeModernzationTargetShipAction(UIRemodeModernzationTargetShip.ActionType actionType, UIRemodeModernzationTargetShip calledObject)
		{
			if (mKeyController != null && actionType == UIRemodeModernzationTargetShip.ActionType.OnTouch)
			{
				ChangeFocusSlot(calledObject);
				Forward4Select();
			}
		}

		private void ChangeFocusSlot(UIRemodeModernzationTargetShip target)
		{
			ChangeFocusSlot(target, mute: false);
		}

		private void ChangeFocusSlot(UIRemodeModernzationTargetShip target, bool mute)
		{
			if (mCurrentFocusTargetShipSlot != null)
			{
				mCurrentFocusTargetShipSlot.RemoveHover();
			}
			mCurrentFocusTargetShipSlot = target;
			if (mCurrentFocusTargetShipSlot != null)
			{
				mCurrentFocusTargetShipSlot.Hover();
			}
			if (target != _BeforeTargetSlot)
			{
				if (!mute)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				_BeforeTargetSlot = target;
			}
		}

		public UIRemodeModernzationTargetShip GetFocusSlot()
		{
			return mCurrentFocusTargetShipSlot;
		}

		public void Hide(bool animation)
		{
			base.enabled = false;
			SwitchChildEnabled();
			_BeforeTargetSlot = mUIRemodeModernzationTargetShip_TargetShips[0];
			isShown = false;
			if (animation)
			{
				if (UserInterfaceRemodelManager.instance.status != ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
				{
					RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.2f, delegate
					{
						if (!isShown)
						{
							base.gameObject.SetActive(false);
						}
					});
				}
			}
			else
			{
				base.transform.localPosition = hidePos;
				base.gameObject.SetActive(false);
			}
		}

		public void Show()
		{
			Show(animation: true);
		}

		public void Show(bool animation)
		{
			UIRemodeModernzationTargetShip[] array = mUIRemodeModernzationTargetShip_TargetShips;
			foreach (UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip in array)
			{
				uIRemodeModernzationTargetShip.Refresh();
			}
			base.gameObject.SetActive(true);
			base.enabled = true;
			SwitchChildEnabled();
			isShown = true;
			if (animation)
			{
				mPanelThis.widgetsAreStatic = true;
				RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.2f, delegate
				{
					mPanelThis.widgetsAreStatic = false;
				});
			}
			else
			{
				base.transform.localPosition = showPos;
			}
			if (mCurrentFocusTargetShipSlot != null)
			{
				mCurrentFocusTargetShipSlot.Hover();
			}
			UpdateStartButtonEnabled();
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void OnTouchStart()
		{
			if (UserInterfaceRemodelManager.instance.status == ScreenStatus.MODE_KINDAIKA_KAISHU)
			{
				Forward4Confirm();
			}
		}

		public void SetCurrentFocusToShip(ShipModel shipModel)
		{
			mCurrentFocusTargetShipSlot.Initialize(shipModel);
			UpdateStartButtonEnabled();
		}

		public void RefreshList()
		{
			List<ShipModel> setShipModels = GetSetShipModels();
			UnSetAll();
			int num = 0;
			foreach (ShipModel item in setShipModels)
			{
				mUIRemodeModernzationTargetShip_TargetShips[num++].Initialize(item);
			}
			_CursorDown = num;
			UpdateStartButtonEnabled();
		}

		private void UpdateStartButtonEnabled()
		{
			mButton_Start.enabled = (GetSetShipModels().Count > 0 && CanModernize());
			if (mButton_Start.enabled)
			{
				btnLight.PlayAnim();
			}
			else
			{
				btnLight.StopAnim();
			}
			mButton_Start.SetState((!mButton_Start.enabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, immediate: true);
		}

		public void InitFocus()
		{
			if (mUIRemodeModernzationTargetShip_TargetShips.Length > 0)
			{
				ChangeFocusSlot(mUIRemodeModernzationTargetShip_TargetShips[0]);
			}
		}

		public void RemoveFocus()
		{
			ChangeFocusSlot(null, mute: true);
		}

		public void OnTouchBackArea()
		{
			Back();
		}

		private bool CanModernize()
		{
			return UserInterfaceRemodelManager.instance.mRemodelManager.IsValidPowerUp(GetSetShipModels());
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Start);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_TouchBack);
			if (mUIRemodeModernzationTargetShip_TargetShips != null)
			{
				for (int i = 0; i < mUIRemodeModernzationTargetShip_TargetShips.Length; i++)
				{
					mUIRemodeModernzationTargetShip_TargetShips[i] = null;
				}
			}
			mUIRemodeModernzationTargetShip_TargetShips = null;
			btnLight = null;
			mCurrentFocusTargetShipSlot = null;
			_BeforeTargetSlot = null;
			mKeyController = null;
			mModernzationShipModel = null;
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodeModernzationTargetShip[] array = mUIRemodeModernzationTargetShip_TargetShips;
			foreach (UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip in array)
			{
				CommonShipBanner banner = uIRemodeModernzationTargetShip.GetBanner();
				list.Add(banner);
			}
			return list.ToArray();
		}
	}
}
