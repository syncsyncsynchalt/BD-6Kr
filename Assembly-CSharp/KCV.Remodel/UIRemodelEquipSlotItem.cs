using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodelEquipSlotItem : MonoBehaviour
	{
		public enum ActionType
		{
			OnTouch
		}

		public delegate void UIRemodelEquipSlotItemAction(ActionType actionType, UIRemodelEquipSlotItem actionObject);

		private UIWidget mWidget;

		[SerializeField]
		private UISprite mSprite_TypeIcon;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_PlaneCount;

		[SerializeField]
		private UIButton mButton_Action;

		[SerializeField]
		private SlotItemLevelStar levelStar;

		[SerializeField]
		private UISprite mLock_Icon;

		[SerializeField]
		private UISprite PlaneSkill;

		private Vector3 PlaneNumPos_NoSkill = new Vector3(165f, 0f, 0f);

		private Vector3 PlaneNumPos_SkillPos = new Vector3(165f, -14f, 0f);

		private Transform ExSlotButtonFrame;

		private UIRemodelEquipSlotItemAction mUIRemodelEquipSlotItemAction;

		private SlotitemModel mModel;

		private bool mEnabled;

		private static readonly Vector3 ExSize = new Vector3(0.8f, 0.8f, 0.8f);

		public UIButton ExSlotButton
		{
			get;
			private set;
		}

		public int index
		{
			get;
			private set;
		}

		public bool isExSlot
		{
			get;
			private set;
		}

		public SlotitemModel GetModel()
		{
			return mModel;
		}

		private void Awake()
		{
			mWidget = GetComponent<UIWidget>();
		}

		private void OnAction(ActionType actionType, UIRemodelEquipSlotItem actionObject)
		{
			if (mUIRemodelEquipSlotItemAction != null)
			{
				mUIRemodelEquipSlotItemAction(actionType, actionObject);
			}
		}

		public void Initialize(int index, ShipModel shipModel)
		{
			SlotitemModel itemModel = shipModel.SlotitemList[index];
			Initialize(index, itemModel, shipModel);
		}

		public void Initialize(int index, SlotitemModel itemModel, ShipModel shipModel)
		{
			this.index = index;
			mModel = itemModel;
			bool flag = false;
			isExSlot = (shipModel.HasExSlot() && shipModel.SlotCount <= index);
			base.transform.SetActiveChildren(isActive: true);
			if (ExSlotButton != null)
			{
				ExSlotButton.transform.parent.SetActive(isActive: false);
			}
			if (itemModel != null)
			{
				mLabel_Name.text = mModel.Name;
				if (itemModel.IsLocked())
				{
					mLock_Icon.transform.localScale = Vector3.one;
				}
				else
				{
					mLock_Icon.transform.localScale = Vector3.zero;
				}
				mSprite_TypeIcon.spriteName = "icon_slot" + itemModel.Type4;
				flag = itemModel.IsPlane(dummy: true);
				if (flag)
				{
					mLabel_PlaneCount.text = shipModel.TousaiMax[index].ToString();
				}
			}
			else
			{
				mLabel_Name.text = "-";
				mLock_Icon.transform.localScale = Vector3.zero;
				mSprite_TypeIcon.spriteName = "icon_slot_notset";
			}
			mLabel_PlaneCount.SetActive(flag);
			SetPlaneSkill(itemModel);
			InitializeButtonColor(mButton_Action);
			levelStar.Init(itemModel);
			if (isExSlot)
			{
				ChangeExMode();
			}
			else
			{
				ChangeNormalMode();
			}
		}

		private void SetPlaneSkill(SlotitemModel item)
		{
			if (item != null && item.IsPlane())
			{
				int skillLevel = item.SkillLevel;
				if (skillLevel == 0)
				{
					PlaneSkill.SetActive(isActive: false);
					mLabel_PlaneCount.transform.localPosition = PlaneNumPos_NoSkill;
					return;
				}
				PlaneSkill.SetActive(isActive: true);
				PlaneSkill.spriteName = "skill_" + skillLevel;
				PlaneSkill.MakePixelPerfect();
				mLabel_PlaneCount.transform.localPosition = PlaneNumPos_SkillPos;
			}
			else
			{
				PlaneSkill.SetActive(isActive: false);
			}
		}

		public void InitExSlotButton(bool isEnable)
		{
			base.transform.SetActiveChildren(isActive: false);
			ChangeNormalMode();
			if (ExSlotButton == null)
			{
				GameObject gameObject = Util.InstantiatePrefab("Remodel/ExSlotBtn", base.gameObject);
				ExSlotButton = ((Component)gameObject.transform.FindChild("ExSlotButton")).GetComponent<UIButton>();
				ExSlotButtonFrame = gameObject.transform.FindChild("ExSlotButtonFrame");
			}
			ExSlotButton.transform.parent.SetActive(isActive: true);
			ExSlotButton.transform.SetActive(isActive: true);
			ExSlotButtonFrame.SetActive(isActive: false);
			isExSlot = true;
			ExSlotButton.isEnabled = isEnable;
			ExSlotButton.onClick.Add(Util.CreateEventDelegate(((Component)base.transform.parent.parent).GetComponent<UIRemodelEquipSlotItems>(), "OpenExSlotDialog", null));
			if (isEnable)
			{
				ExSlotButton.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			else
			{
				ExSlotButton.SetState(UIButtonColor.State.Disabled, immediate: true);
			}
		}

		public void UnSetSlotItem()
		{
			mModel = null;
			mLabel_Name.text = "-";
			mSprite_TypeIcon.spriteName = "icon_slot_notset";
		}

		private void InitializeButtonColor(UIButton target)
		{
			target.hover = Util.CursolColor;
			target.defaultColor = Color.white;
			target.pressed = Color.white;
			target.disabledColor = Color.white;
		}

		public void SetOnUIRemodelEquipSlotItemActionListener(UIRemodelEquipSlotItemAction action)
		{
			mUIRemodelEquipSlotItemAction = action;
		}

		public void OnClick()
		{
			if (mEnabled)
			{
				OnAction(ActionType.OnTouch, this);
			}
		}

		public void Hover()
		{
			if (isExSlot && ExSlotButton != null && ExSlotButton.isActiveAndEnabled)
			{
				if (ExSlotButton.isEnabled)
				{
					ExSlotButton.SetState(UIButtonColor.State.Hover, immediate: true);
				}
				ExSlotButtonFrame.SetActive(isActive: true);
			}
			else
			{
				mButton_Action.SetState(UIButtonColor.State.Hover, immediate: true);
				UISelectedObject.SelectedOneObjectBlink(mButton_Action.gameObject, value: true);
			}
		}

		public void RemoveHover()
		{
			if (isExSlot && ExSlotButton != null && ExSlotButton.isActiveAndEnabled)
			{
				if (ExSlotButton.isEnabled)
				{
					ExSlotButton.SetState(UIButtonColor.State.Normal, immediate: true);
				}
				ExSlotButtonFrame.SetActive(isActive: false);
			}
			else
			{
				mButton_Action.SetState(UIButtonColor.State.Normal, immediate: true);
				UISelectedObject.SelectedOneObjectBlink(mButton_Action.gameObject, value: false);
			}
		}

		public void Show()
		{
			mEnabled = true;
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			mEnabled = false;
			mModel = null;
			base.gameObject.SetActive(false);
			RemoveHover();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidget);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_TypeIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Name);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_PlaneCount);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Action);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLock_Icon);
			levelStar = null;
			mUIRemodelEquipSlotItemAction = null;
			mModel = null;
		}

		private void ChangeExMode()
		{
			base.transform.localScale = ExSize;
			base.transform.localPositionX(-40f);
			mLabel_Name.fontSize = 30;
		}

		private void ChangeNormalMode()
		{
			base.transform.localScale = Vector3.one;
			base.transform.localPositionX(0f);
			mLabel_Name.fontSize = 24;
		}
	}
}
