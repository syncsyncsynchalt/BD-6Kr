using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelCurrentSlot : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.2f;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UISprite ItemIcon;

		[SerializeField]
		private UISprite mLock_Icon;

		private SlotitemModel slotItem;

		private Vector3 showPos = new Vector3(-240f, -170f);

		private Vector3 hidePos = new Vector3(-840f, -170f);

		private void Awake()
		{
			labelName.text = string.Empty;
			ItemIcon.spriteName = string.Empty;
			slotItem = null;
			base.transform.localPosition = hidePos;
		}

		public void Init(SlotitemModel slotItem)
		{
			this.slotItem = slotItem;
			if (slotItem != null)
			{
				labelName.text = slotItem.Name;
				if (slotItem.IsLocked())
				{
					mLock_Icon.transform.localScale = Vector3.one;
				}
				else
				{
					mLock_Icon.transform.localScale = Vector3.zero;
				}
				ItemIcon.spriteName = "icon_slot" + slotItem.Type4.ToString();
			}
			else
			{
				mLock_Icon.transform.localScale = Vector3.zero;
			}
		}

		public void Show()
		{
			Show(animation: true);
			if (slotItem != null)
			{
				base.gameObject.SetActive(true);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		public void Show(bool animation)
		{
			base.gameObject.SetActive(true);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.2f);
			}
			else
			{
				base.transform.localPosition = showPos;
			}
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.2f, delegate
				{
					base.gameObject.SetActive(false);
				});
				return;
			}
			base.transform.localPosition = hidePos;
			base.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref labelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref ItemIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLock_Icon);
			slotItem = null;
		}
	}
}
