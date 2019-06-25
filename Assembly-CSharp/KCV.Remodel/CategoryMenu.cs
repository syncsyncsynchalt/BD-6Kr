using Common.Enum;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class CategoryMenu : MonoBehaviour, UIRemodelView
	{
		private const int REMOVE_TOGGLE_IDX = -1;

		[SerializeField]
		private UIButton backAreaButton;

		[SerializeField]
		private GameObject itemContainer;

		[SerializeField]
		private CategoryMenuItem removeItem;

		private int currentIdx;

		private KeyControl keyController;

		private List<CategoryMenuItem> items;

		private SlotitemCategory[] Categories = new SlotitemCategory[8]
		{
			SlotitemCategory.Syuhou,
			SlotitemCategory.Kanjouki,
			SlotitemCategory.Fukuhou,
			SlotitemCategory.Suijouki,
			SlotitemCategory.Kiju,
			SlotitemCategory.Dentan,
			SlotitemCategory.Gyorai,
			SlotitemCategory.Other
		};

		private ShipModel ship;

		private UIRemodelEquipSlotItem slotItem;

		private Vector3 showPos = new Vector3(235f, 0f, 0f);

		private Vector3 hidePos = new Vector3(1050f, 0f, 0f);

		private int _BeforeIdx;

		private bool isShown;

		private List<SlotitemCategory> selectableCategories
		{
			get
			{
				if (!slotItem.isExSlot)
				{
					return ship.GetEquipCategory();
				}
				List<SlotitemCategory> list = new List<SlotitemCategory>();
				list.Add(SlotitemCategory.Other);
				return list;
			}
		}

		public int group
		{
			get;
			private set;
		}

		private int currentRow => (int)Math.Ceiling((float)(currentIdx + 1) / 2f);

		private int maxRow => (int)Math.Ceiling((float)Categories.Length / 2f);

		private void Awake()
		{
			backAreaButton.SetActive(isActive: false);
			base.transform.localPosition = hidePos;
		}

		public void Init(KeyControl keyController, ShipModel ship, UIRemodelEquipSlotItem slotItem)
		{
			this.ship = ship;
			this.slotItem = slotItem;
			group = ((UnityEngine.Object)this).GetHashCode();
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.keyController = keyController;
			if (items == null)
			{
				items = new List<CategoryMenuItem>();
			}
			items.Clear();
			int num = 0;
			CategoryMenuItem[] componentsInChildren = itemContainer.transform.GetComponentsInChildren<CategoryMenuItem>();
			foreach (CategoryMenuItem categoryMenuItem in componentsInChildren)
			{
				categoryMenuItem.Init(this, num, selectableCategories.IndexOf(Categories[num++]) != -1);
				items.Add(categoryMenuItem);
			}
			for (int j = 0; j < items.Count; j++)
			{
				if (changeCurrentItem(j))
				{
					_BeforeIdx = j;
					break;
				}
			}
			bool flag = slotItem.GetModel() != null;
			if (flag)
			{
				removeItem.Init(this, -1, enabled: true);
			}
			removeItem.SetActive(flag);
		}

		private bool changeCurrentItem(int targetIdx)
		{
			if (targetIdx == -1)
			{
				currentIdx = -1;
				removeItem.Set(state: true);
				if (targetIdx != _BeforeIdx)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					_BeforeIdx = targetIdx;
				}
				return true;
			}
			if (IsSelectable(targetIdx))
			{
				currentIdx = targetIdx;
				items[currentIdx].Set(state: false);
				items[currentIdx].Set(state: true);
				if (targetIdx != _BeforeIdx)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					_BeforeIdx = targetIdx;
				}
				return true;
			}
			return false;
		}

		private void Update()
		{
			if (keyController == null || !base.enabled || !isShown)
			{
				return;
			}
			if (keyController.IsLeftDown())
			{
				int targetIdx = currentIdx - 1;
				if (currentIdx % 2 == 1)
				{
					changeCurrentItem(targetIdx);
				}
			}
			else if (keyController.IsRightDown())
			{
				int targetIdx2 = currentIdx + 1;
				if (currentIdx % 2 == 0)
				{
					changeCurrentItem(targetIdx2);
				}
			}
			else if (keyController.IsUpDown())
			{
				if (currentIdx == -1)
				{
					int num = Categories.Length - 1;
					while (num > 0 && !changeCurrentItem(num))
					{
						num--;
					}
				}
				else
				{
					int num2 = currentIdx - 2;
					while (num2 >= 0 && !changeCurrentItem(num2))
					{
						num2 -= 2;
					}
				}
			}
			else if (keyController.IsDownDown())
			{
				if (currentIdx == -1)
				{
					return;
				}
				bool flag = false;
				for (int i = currentIdx + 2; i < Categories.Length; i += 2)
				{
					if (flag = changeCurrentItem(i))
					{
						break;
					}
				}
				if (!flag && removeItem.gameObject.activeSelf)
				{
					changeCurrentItem(-1);
				}
			}
			else if (keyController.IsMaruDown())
			{
				if (currentIdx == -1)
				{
					ProcessRemove();
				}
				else
				{
					Forward();
				}
			}
			else if (keyController.IsBatuDown())
			{
				Back();
			}
		}

		public void OnTouchBack()
		{
			Back();
		}

		private void Forward()
		{
			if (isShown)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				Hide();
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouItemSelect(ship, Categories[currentIdx]);
			}
		}

		private void Back()
		{
			if (isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				Hide();
				UserInterfaceRemodelManager.instance.Back2SoubiHenkou();
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			backAreaButton.SetActive(isActive: true);
			base.enabled = true;
			RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.3f, delegate
			{
				isShown = true;
			});
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			backAreaButton.SetActive(isActive: false);
			base.enabled = false;
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

		public bool IsSelectable(int index)
		{
			return selectableCategories.IndexOf(Categories[index]) != -1;
		}

		public void OnItemClick(CategoryMenuItem child)
		{
			currentIdx = child.index;
			if (currentIdx == -1)
			{
				ProcessRemove();
			}
			else
			{
				Forward();
			}
		}

		public void ProcessRemove()
		{
			if (slotItem.isExSlot ? UserInterfaceRemodelManager.instance.mRemodelManager.IsValidUnsetSlotitemEx(ship.MemId) : UserInterfaceRemodelManager.instance.mRemodelManager.IsValidUnsetSlotitem(ship.MemId, slotItem.index))
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_009);
				int memId = ship.MemId;
				int index = slotItem.index;
				UserInterfaceRemodelManager.instance.mRemodelManager.GetSlotitemInfoToUnset(memId, index);
				if (!slotItem.isExSlot)
				{
					UserInterfaceRemodelManager.instance.mRemodelManager.UnsetSlotitem(memId, index);
				}
				else
				{
					UserInterfaceRemodelManager.instance.mRemodelManager.UnsetSlotitemEx(memId);
				}
				UserInterfaceRemodelManager.instance.Back2SoubiHenkou();
				Hide();
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref backAreaButton);
			UserInterfacePortManager.ReleaseUtils.Release(ref backAreaButton);
			UserInterfacePortManager.ReleaseUtils.Release(ref backAreaButton);
			if (items != null)
			{
				items.Clear();
			}
			items = null;
			itemContainer = null;
			removeItem = null;
			keyController = null;
			Categories = null;
			ship = null;
			slotItem = null;
		}
	}
}
