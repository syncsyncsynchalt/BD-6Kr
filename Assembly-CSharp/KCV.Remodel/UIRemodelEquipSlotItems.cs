using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelEquipSlotItems : MonoBehaviour, UIRemodelView
	{
		[SerializeField]
		private UIButton touchBackArea;

		[SerializeField]
		private UIRemodelEquipSlotItem[] slots;

		[SerializeField]
		private GameObject parameters;

		[SerializeField]
		private CommonDialog ExSlotDialog;

		[SerializeField]
		private YesNoButton YesNoButton;

		[SerializeField]
		private UILabel ExSlotItemNum;

		private Vector3 showPos = new Vector3(300f, 0f);

		private Vector3 hidePos = new Vector3(780f, 0f);

		private ShipModel ship;

		private KeyControl keyController;

		private bool validShip;

		private bool validUnsetAll;

		private UIRemodelEquipSlotItem _BeforeItem;

		private bool isShown;

		public UIRemodelEquipSlotItem currentFocusItem
		{
			get;
			private set;
		}

		private void Awake()
		{
			base.transform.localPosition = hidePos;
			touchBackArea.SetActive(isActive: false);
			ExSlotDialog.SetActive(isActive: false);
		}

		public void Initialize(KeyControl keyController, ShipModel shipModel)
		{
			keyController.ClearKeyAll();
			keyController.firstUpdate = true;
			this.keyController = keyController;
			base.enabled = true;
			ship = shipModel;
			validShip = UserInterfaceRemodelManager.instance.IsValidShip();
			validUnsetAll = UserInterfaceRemodelManager.instance.mRemodelManager.IsValidUnsetAll(UserInterfaceRemodelManager.instance.focusedShipModel.MemId);
			UIRemodelEquipSlotItem[] array = slots;
			foreach (UIRemodelEquipSlotItem uIRemodelEquipSlotItem in array)
			{
				if (validShip)
				{
					uIRemodelEquipSlotItem.SetOnUIRemodelEquipSlotItemActionListener(UIRemodelEquipSlotItemAction);
				}
				else
				{
					uIRemodelEquipSlotItem.SetOnUIRemodelEquipSlotItemActionListener(null);
				}
				uIRemodelEquipSlotItem.Hide();
			}
			InitSlotItems(ship);
			ParameterType[] array2 = new ParameterType[12]
			{
				ParameterType.Taikyu,
				ParameterType.Karyoku,
				ParameterType.Soukou,
				ParameterType.Raisou,
				ParameterType.Kaihi,
				ParameterType.Taiku,
				ParameterType.Tous,
				ParameterType.Taisen,
				ParameterType.Soku,
				ParameterType.Sakuteki,
				ParameterType.Leng,
				ParameterType.Lucky
			};
			int[] array3 = new int[12]
			{
				shipModel.MaxHp,
				shipModel.Karyoku,
				shipModel.Soukou,
				shipModel.Raisou,
				shipModel.Kaihi,
				shipModel.Taiku,
				shipModel.TousaiMaxAll,
				shipModel.Taisen,
				shipModel.Soku,
				shipModel.Sakuteki,
				shipModel.Leng,
				shipModel.Lucky
			};
			for (int j = 0; j < array2.Length; j++)
			{
				UIRemodelParameter component = ((Component)parameters.transform.GetChild(j)).GetComponent<UIRemodelParameter>();
				component.Initialize(array2[j], array3[j]);
			}
			currentFocusItem = null;
			_BeforeItem = slots[0];
			Focus();
		}

		private void InitSlotItems(ShipModel shipModel)
		{
			int i;
			for (i = 0; i < ship.SlotitemList.Count; i++)
			{
				slots[i].Initialize(i, shipModel);
				slots[i].Show();
			}
			if (ship.HasExSlot())
			{
				slots[i].Initialize(i, ship.SlotitemEx, shipModel);
				slots[i].Show();
			}
			else if (ship.Level >= 30)
			{
				bool isEnable = UserInterfaceRemodelManager.instance.mRemodelManager.HokyoZousetsuNum > 0;
				slots[i].InitExSlotButton(isEnable);
				slots[i].Show();
			}
		}

		private void UIRemodelEquipSlotItemAction(UIRemodelEquipSlotItem.ActionType actionType, UIRemodelEquipSlotItem actionObject)
		{
			if (actionType == UIRemodelEquipSlotItem.ActionType.OnTouch)
			{
				if (actionObject == null)
				{
					Debug.Log("actionObject is null");
				}
				ChangeFocusItem(actionObject);
				forward();
			}
		}

		private void Focus()
		{
			if (validShip)
			{
				if (currentFocusItem == null)
				{
					FocusFirstItem();
				}
				else
				{
					currentFocusItem.Hover();
				}
			}
		}

		public void OnTouchBackArea()
		{
			back();
		}

		private void forward()
		{
			if (!isShown)
			{
				return;
			}
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
			if (currentFocusItem.isExSlot && !ship.HasExSlot())
			{
				if (currentFocusItem.ExSlotButton.state != UIButtonColor.State.Disabled)
				{
					OpenExSlotDialog();
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("アイテム「補強増設」を持っていません");
				}
			}
			else
			{
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouTypeSelect();
			}
		}

		private void back()
		{
			if (isShown)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Back2ModeSelect();
			}
		}

		private void UnsetAll()
		{
			UserInterfaceRemodelManager.instance.mRemodelManager.UnsetAll(ship.MemId);
			Initialize(keyController, ship);
		}

		private void ChangeFocusItem(UIRemodelEquipSlotItem target)
		{
			if (currentFocusItem != null)
			{
				currentFocusItem.RemoveHover();
			}
			currentFocusItem = target;
			if (currentFocusItem != null)
			{
				currentFocusItem.Hover();
			}
			if (target != _BeforeItem)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCursolMove);
				_BeforeItem = target;
			}
		}

		public void ChangeFocusItemFromModel(SlotitemModel slotItemModel)
		{
			if (slotItemModel != null)
			{
				int num = 0;
				while (true)
				{
					if (num < slots.Length)
					{
						if (slots[num].GetModel() != null && slots[num].GetModel().MemId == slotItemModel.MemId)
						{
							break;
						}
						num++;
						continue;
					}
					return;
				}
				ChangeFocusItem(slots[num]);
				return;
			}
			int num2 = 0;
			while (true)
			{
				if (num2 < slots.Length)
				{
					if (slots[num2].GetModel() == null)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			ChangeFocusItem(slots[num2]);
		}

		private void FocusFirstItem()
		{
			if (slots.Length > 0)
			{
				ChangeFocusItem(slots[0]);
			}
		}

		public UIRemodelEquipSlotItem GetCurrentItem()
		{
			return currentFocusItem;
		}

		public int GetCurrentSlotIndex()
		{
			return Array.IndexOf(slots, currentFocusItem);
		}

		private void Update()
		{
			if (keyController == null || !base.enabled || !isShown)
			{
				return;
			}
			if (validShip && keyController.IsUpDown())
			{
				int num = Array.IndexOf(slots, currentFocusItem);
				int num2 = num - 1;
				if (0 <= num2)
				{
					Debug.Log("PrevIndex::" + num2);
					ChangeFocusItem(slots[num2]);
				}
			}
			else if (validShip && keyController.IsDownDown())
			{
				int num3 = Array.IndexOf(slots, currentFocusItem);
				int num4 = num3 + 1;
				int num5 = (ship.Level >= 30) ? 1 : 0;
				if (num4 < ship.SlotitemList.Count + num5)
				{
					Debug.Log("NextIndex::" + num4);
					ChangeFocusItem(slots[num4]);
				}
			}
			else if (validShip && keyController.IsMaruDown())
			{
				forward();
			}
			else if (keyController.IsBatuDown())
			{
				back();
			}
			else if (validUnsetAll && keyController.IsShikakuDown())
			{
				UnsetAll();
				SoundUtils.PlayOneShotSE(SEFIleInfos.SE_009);
			}
		}

		public void Show()
		{
			Show(animation: true);
		}

		public void Show(bool animation)
		{
			base.gameObject.SetActive(true);
			base.enabled = true;
			Focus();
			touchBackArea.SetActive(isActive: true);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.3f, delegate
				{
					isShown = true;
				});
			}
			else
			{
				isShown = true;
				base.transform.localPosition = showPos;
			}
			UserInterfaceRemodelManager.instance.UpdateHeaderMaterial();
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			isShown = false;
			base.enabled = false;
			touchBackArea.SetActive(isActive: false);
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

		private void OpenExSlotDialog()
		{
			ExSlotDialog.SetActive(isActive: true);
			keyController.IsRun = false;
			keyController.ClearKeyAll();
			int hokyoZousetsuNum = UserInterfaceRemodelManager.instance.mRemodelManager.HokyoZousetsuNum;
			ExSlotDialog.isUseDefaultKeyController = false;
			ExSlotItemNum.text = hokyoZousetsuNum + "\u3000→\u3000" + (hokyoZousetsuNum - 1);
			ExSlotDialog.OpenDialog(0);
			ExSlotDialog.setCloseAction(delegate
			{
				keyController.IsRun = true;
			});
			YesNoButton.SetOnSelectPositiveListener(OpenExSlot);
			YesNoButton.SetOnSelectNegativeListener(CloseExSlotDialog);
			YesNoButton.SetKeyController(new KeyControl());
		}

		private void CloseExSlotDialog()
		{
			ExSlotDialog.CloseDialog();
		}

		private void OpenExSlot()
		{
			UserInterfaceRemodelManager.instance.mRemodelManager.OpenSlotEx(ship.MemId);
			InitSlotItems(ship);
			currentFocusItem.Hover();
			CloseExSlotDialog();
			StartCoroutine(PlayOpenAnimation());
		}

		private IEnumerator PlayOpenAnimation()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: true);
			App.OnlyController = new KeyControl();
			Animation anim = Util.InstantiatePrefab("Remodel/OpenInfos", base.gameObject).GetComponent<Animation>();
			anim.Play();
			while (anim.isPlaying)
			{
				Debug.Log("wait");
				yield return null;
			}
			App.OnlyController = null;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(isEnable: false);
			UnityEngine.Object.Destroy(((Component)anim).gameObject);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref touchBackArea);
			if (slots != null)
			{
				for (int i = 0; i < slots.Length; i++)
				{
					slots[i] = null;
				}
			}
			slots = null;
			parameters = null;
			currentFocusItem = null;
			ship = null;
			keyController = null;
			_BeforeItem = null;
			ExSlotDialog = null;
		}
	}
}
