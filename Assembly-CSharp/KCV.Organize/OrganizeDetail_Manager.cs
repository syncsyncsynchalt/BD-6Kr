using local.managers;
using local.models;
using System.Collections;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Manager : MonoBehaviour
	{
		[SerializeField]
		private OrganizeDetail_Card card;

		[SerializeField]
		private OrganizeDetail_Status status;

		[SerializeField]
		private OrganizeDetail_StatusMaxIcons statusMaxIcons;

		[SerializeField]
		private OrganizeDetail_Paramerter parameter;

		[SerializeField]
		private OrganizeDetail_SlotItemManager slotItem;

		[SerializeField]
		private DialogAnimation DialogAnim;

		[SerializeField]
		private UIButtonMessage BackButton;

		public bool isShow;

		public OrganizeDetail_Buttons buttons;

		private BoxCollider2D MaskBg;

		public bool Init()
		{
			Util.FindParentToChild(ref card, base.transform, "CardPanel");
			Util.FindParentToChild(ref status, base.transform, "StatusPanel");
			Util.FindParentToChild(ref statusMaxIcons, base.transform, "StatusMaxIcons");
			Util.FindParentToChild(ref parameter, base.transform, "ParamaterPanel");
			Util.FindParentToChild(ref slotItem, base.transform, "SlotItemPanel");
			if (DialogAnim == null)
			{
				DialogAnim = ((Component)base.transform).GetComponent<DialogAnimation>();
			}
			Util.FindParentToChild(ref buttons, base.transform, "ButtonPanel");
			Util.FindParentToChild(ref MaskBg, base.transform, "MaskBg");
			MaskBg.size = Vector2.right * 1060f + Vector2.up * 544f;
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref card);
			Mem.Del(ref status);
			Mem.Del(ref statusMaxIcons);
			Mem.Del(ref parameter);
			Mem.Del(ref slotItem);
			Mem.Del(ref DialogAnim);
			Mem.Del(ref BackButton);
			Mem.Del(ref buttons);
			Mem.Del(ref MaskBg);
		}

		public void SetDetailPanel(ShipModel ship, bool isFirstDitail, int SelectDeckId, IOrganizeManager manager, int ShipIndex, MonoBehaviour CallBackTarget)
		{
			card.SetShipCard(ship);
			status.SetStatus(ship);
			statusMaxIcons.SetMaxIcons(ship);
			parameter.SetParams(ship);
			slotItem.SetSlotItems(ship);
			if (isFirstDitail)
			{
				buttons.SetDeckShipDetailButtons(ship, manager, CallBackTarget);
			}
			else
			{
				buttons.SetListShipDetailButtons(ship, SelectDeckId, manager, ShipIndex, CallBackTarget);
			}
		}

		public void SetBackButton(GameObject target, string FunctionName)
		{
			BackButton.target = target;
			BackButton.functionName = FunctionName;
		}

		public void Open()
		{
			((Component)base.transform).GetComponent<UIPanel>().alpha = 1f;
			DialogAnim.FadeIn(0f);
			isShow = true;
			DialogAnim.CloseAction = null;
		}

		public void Close()
		{
			DialogAnim.CloseAction = delegate
			{
				((Component)base.transform).GetComponent<UIPanel>().alpha = 0f;
				card.Release();
			};
			DialogAnim.FadeOut();
			isShow = false;
		}

		public IEnumerator CloseAndDestroy()
		{
			bool isFinished = false;
			DialogAnim.CloseAction = delegate
			{
                isFinished = true;
			};
			DialogAnim.FadeOut();
			isShow = false;
			while (!isFinished)
			{
				yield return null;
			}
			if (!isShow)
			{
				((Component)base.transform).GetComponent<UIPanel>().alpha = 0f;
				card.Release();
				Object.Destroy(base.gameObject);
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
