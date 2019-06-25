using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_SlotItemManager : MonoBehaviour
	{
		[SerializeField]
		private OrganizeDetail_SlotItem[] SlotItem;

		private UIGrid grid;

		public void SetSlotItems(ShipModel ship)
		{
			bool flag = ship.HasExSlot();
			for (int i = 0; i < SlotItem.Length; i++)
			{
				if (i < ship.SlotitemList.Count)
				{
					SlotItem[i].SetSlotItem(ship.SlotitemList[i], ship, i, isExtention: false);
				}
				else if (flag)
				{
					flag = false;
					SlotItem[i].SetSlotItem(ship.SlotitemEx, ship, i, isExtention: true);
				}
				else
				{
					SlotItem[i].SetActive(isActive: false);
				}
			}
		}

		private void OnDestroy()
		{
			SlotItem = null;
		}
	}
}
