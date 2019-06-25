using KCV.Battle.Utils;
using local.models;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class UISlotItemHexButton : UIHexButton
	{
		private static readonly float HEX_BUTTON_SIZE = 390f;

		private UITexture _uiSlotItem;

		protected override void Awake()
		{
			base.Awake();
			Util.FindParentToChild(ref _uiSlotItem, base.transform, "SlotItem");
			_uiSlotItem.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[4];
			_uiSlotItem.alpha = 0.01f;
			_uiHexBtn.localSize = new Vector2(HEX_BUTTON_SIZE, HEX_BUTTON_SIZE);
			_uiHexBtn.alpha = 0.01f;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiSlotItem);
		}

		public override void SetDepth(int nDepth)
		{
			_uiHexBtn.depth = nDepth;
			_uiSlotItem.depth = nDepth + 1;
		}

		public void SetSlotItem(SlotitemModel_Battle model)
		{
			SlotItemUtils.LoadTexture(ref _uiSlotItem, model);
		}
	}
}
