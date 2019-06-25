using KCV.Remodel;
using KCV.Utils;
using local.models;
using System.Collections;
using UnityEngine;

namespace KCV.View.Scroll
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class UIRemodelEquipListChild : UIScrollListChildNew<SlotitemModel, UIRemodelEquipListChild>
	{
		private const int HIDE_DEPTH = -100;

		[SerializeField]
		private UILabel ItemName;

		[SerializeField]
		private UISprite ItemIcon;

		[SerializeField]
		private UISprite LockedIcon;

		[SerializeField]
		private UILabel Rare;

		[SerializeField]
		private SlotItemLevelStar levelStar;

		private bool locked;

		private string originalSpriteName;

		private SlotitemModel _sm;

		protected new void Awake()
		{
			base.Awake();
			originalSpriteName = LockedIcon.spriteName;
		}

		protected override IEnumerator InitializeCoroutine(SlotitemModel slotitemModel)
		{
			_sm = slotitemModel;
			base.InitializeCoroutine(_sm);
			ItemName.text = _sm.Name;
			Rare.text = Util.RareToString(_sm.Rare);
			string slotItemNo = _sm.Type4.ToString();
			ItemIcon.spriteName = "icon_slot" + slotItemNo;
			levelStar.Init(_sm);
			locked = _sm.IsLocked();
			SetLockedIconVisible(locked);
			yield return null;
		}

		public void OnClick2()
		{
			SwitchLockedIcon(Change: true);
		}

		public void SwitchLockedIcon(bool Change)
		{
			if (Change)
			{
				UserInterfaceRemodelManager.instance.mRemodelManager.SlotLock(_sm.MemId);
			}
			locked = !locked;
			SetLockedIconVisible(locked);
			if (locked)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_005);
			}
			else
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_006);
			}
		}

		private void SetLockedIconVisible(bool visible)
		{
			LockedIcon.spriteName = ((!visible) ? "lock_weapon" : "lock_weapon_open");
		}

		protected override void OnCallDestroy()
		{
			ItemName = null;
			ItemIcon = null;
			Rare = null;
			levelStar = null;
			LockedIcon = null;
		}
	}
}
