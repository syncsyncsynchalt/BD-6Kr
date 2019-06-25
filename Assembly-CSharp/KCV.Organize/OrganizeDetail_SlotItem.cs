using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_SlotItem : MonoBehaviour
	{
		[SerializeField]
		private UILabel Name;

		[SerializeField]
		private UISprite Icon;

		[SerializeField]
		private Transform PlusParent;

		[SerializeField]
		private UILabel PlusNum;

		[SerializeField]
		private Transform PlusBase;

		[SerializeField]
		private Transform PlusMax;

		[SerializeField]
		private Transform PlaneNumParent;

		[SerializeField]
		private UILabel PlaneNum;

		[SerializeField]
		private UISprite PlaneSkill;

		[SerializeField]
		private int SlotItemNo;

		private Vector3 PlaneNumPos_NoSkill = new Vector3(158f, 127f, 0f);

		private Vector3 PlaneNumPos_SkillPos = new Vector3(158f, 113f, 0f);

		private float DefaultPosY;

		public void Start()
		{
			Vector3 localPosition = base.transform.localPosition;
			DefaultPosY = localPosition.y;
		}

		private void OnDestroy()
		{
			Name = null;
			Icon = null;
			PlusParent = null;
			PlusNum = null;
			PlusBase = null;
			PlusMax = null;
			PlaneNumParent = null;
			PlaneNum = null;
			PlaneSkill = null;
		}

		public void SetSlotItem(SlotitemModel item, ShipModel ship, int index, bool isExtention)
		{
			DefaultPosY = index * -67;
			if (item != null)
			{
				Name.text = item.Name;
				Icon.spriteName = "icon_slot" + item.Type4;
				SetPlusIcon(item);
				SetPlaneNum(item, ship);
				SetPlaneSkill(item);
			}
			else
			{
				Name.text = "-";
				PlusParent.SetActive(isActive: false);
				PlaneNumParent.SetActive(isActive: false);
				PlaneSkill.SetActive(isActive: false);
				Icon.spriteName = string.Empty;
			}
			SetExtentionSlotMode(isExtention);
			base.gameObject.SetActive(true);
		}

		private void SetPlusIcon(SlotitemModel item)
		{
			if (item.Level <= 0)
			{
				PlusParent.SetActive(isActive: false);
			}
			else if (item.Level < 10)
			{
				PlusMax.SetActive(isActive: false);
				PlusNum.textInt = item.Level;
				PlusParent.SetActive(isActive: true);
			}
			else
			{
				PlusParent.SetActive(isActive: false);
			}
		}

		private void SetPlaneNum(SlotitemModel item, ShipModel ship)
		{
			if (item.IsPlane())
			{
				PlaneNum.text = ship.Tousai[SlotItemNo - 1].ToString();
				PlaneNumParent.SetActive(isActive: true);
			}
			else
			{
				PlaneNumParent.SetActive(isActive: false);
			}
		}

		private void SetPlaneSkill(SlotitemModel item)
		{
			if (item.IsPlane())
			{
				int skillLevel = item.SkillLevel;
				if (skillLevel == 0)
				{
					PlaneSkill.SetActive(isActive: false);
					PlaneNumParent.transform.localPosition = PlaneNumPos_NoSkill;
					return;
				}
				PlaneSkill.SetActive(isActive: true);
				PlaneSkill.spriteName = "skill_" + skillLevel;
				PlaneSkill.MakePixelPerfect();
				PlaneNumParent.transform.localPosition = PlaneNumPos_SkillPos;
			}
			else
			{
				PlaneSkill.SetActive(isActive: false);
			}
		}

		private void SetExtentionSlotMode(bool isExtention)
		{
			if (isExtention)
			{
				base.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
				base.transform.localPosition = new Vector3(-40f, DefaultPosY + 28f, 0f);
				Name.fontSize = 30;
			}
			else
			{
				base.transform.localScale = Vector3.one;
				base.transform.localPosition = new Vector3(0f, DefaultPosY, 0f);
				Name.fontSize = 24;
			}
		}
	}
}
