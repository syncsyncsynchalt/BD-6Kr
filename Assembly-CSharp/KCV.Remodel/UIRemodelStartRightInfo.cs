using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelStartRightInfo : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.3f;

		[SerializeField]
		private UITexture shipTypeMarkIcon;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UILabel labelLevel;

		[SerializeField]
		private UiStarManager stars;

		[SerializeField]
		private UIRemodelEquipSlotItem[] slots;

		private ShipModel ship;

		private Vector3 showPos = new Vector3(250f, 0f);

		private Vector3 hidePos = new Vector3(950f, 0f);

		private KeyControl mKeyController;

		private bool isShown;

		private void Awake()
		{
			base.transform.localPosition = hidePos;
			Show(animation: false);
		}

		public void Init(ShipModel ship)
		{
			this.ship = ship;
			labelName.text = ship.Name;
			labelLevel.text = ship.Level.ToString();
			stars.init(ship.Srate);
			shipTypeMarkIcon.mainTexture = ResourceManager.LoadShipTypeIcon(ship);
			UIRemodelEquipSlotItem[] array = slots;
			foreach (UIRemodelEquipSlotItem uIRemodelEquipSlotItem in array)
			{
				uIRemodelEquipSlotItem.Hide();
			}
			int j;
			for (j = 0; j < ship.SlotitemList.Count; j++)
			{
				slots[j].Initialize(j, ship);
				slots[j].SetOnUIRemodelEquipSlotItemActionListener(OnUIRemodelEquipSlotItemActionListener);
				slots[j].Show();
			}
			if (ship.HasExSlot())
			{
				slots[j].Initialize(j, ship.SlotitemEx, ship);
				slots[j].Show();
			}
		}

		public void Show()
		{
			Show(animation: true);
		}

		public void Show(bool animation)
		{
			if (animation)
			{
				TweenPosition tweenPosition = RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.3f, delegate
				{
					isShown = true;
				});
				tweenPosition.PlayForward();
			}
			else
			{
				isShown = true;
				base.transform.localPosition = showPos;
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

		private void OnUIRemodelEquipSlotItemActionListener(UIRemodelEquipSlotItem.ActionType actionType, UIRemodelEquipSlotItem actionObject)
		{
			if (isShown)
			{
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouShortCut(actionObject.GetModel());
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref shipTypeMarkIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelLevel);
			UserInterfacePortManager.ReleaseUtils.Release(ref shipTypeMarkIcon);
			if (slots != null)
			{
				for (int i = 0; i < slots.Length; i++)
				{
					slots[i] = null;
				}
			}
			slots = null;
			stars = null;
			ship = null;
			mKeyController = null;
		}
	}
}
