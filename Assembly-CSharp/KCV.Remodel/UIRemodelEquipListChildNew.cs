using KCV.Scene.Port;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodelEquipListChildNew : MonoBehaviour, UIScrollListItem<SlotitemModel, UIRemodelEquipListChildNew>
	{
		private const int HIDE_DEPTH = -100;

		private UIWidget mWidgetThis;

		[SerializeField]
		private UILabel ItemName;

		[SerializeField]
		private UISprite ItemIcon;

		[SerializeField]
		private UISprite LockedIcon;

		[SerializeField]
		private UILabel Rare;

		[SerializeField]
		private Transform mTransform_Background;

		[SerializeField]
		private SlotItemLevelStar levelStar;

		[SerializeField]
		private UISprite PlaneSkill;

		private Action<UIRemodelEquipListChildNew> mOnTouchListener;

		private Transform mTransformCache;

		private SlotitemModel mSlotItemModel;

		private bool locked;

		private string originalSpriteName;

		private int mRealIndex;

		protected void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 0f;
			originalSpriteName = LockedIcon.spriteName;
		}

		public void SwitchLockedIcon(bool Change)
		{
			if (Change)
			{
				UserInterfaceRemodelManager.instance.mRemodelManager.SlotLock(mSlotItemModel.MemId);
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

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref ItemName);
			UserInterfacePortManager.ReleaseUtils.Release(ref ItemIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref LockedIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref Rare);
			mTransform_Background = null;
			levelStar = null;
			mOnTouchListener = null;
			mTransformCache = null;
			mSlotItemModel = null;
			PlaneSkill = null;
		}

		public void Initialize(int realIndex, SlotitemModel slotitemModel)
		{
			mRealIndex = realIndex;
			mSlotItemModel = slotitemModel;
			ItemName.text = mSlotItemModel.Name;
			Rare.text = Util.RareToString(mSlotItemModel.Rare);
			string str = mSlotItemModel.Type4.ToString();
			ItemIcon.spriteName = "icon_slot" + str;
			levelStar.Init(mSlotItemModel);
			SetPlaneSkill(mSlotItemModel);
			locked = mSlotItemModel.IsLocked();
			SetLockedIconVisible(locked);
			mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault(int realIndex)
		{
			mSlotItemModel = null;
			mRealIndex = realIndex;
			mWidgetThis.alpha = 1E-07f;
		}

		public int GetRealIndex()
		{
			return mRealIndex;
		}

		public SlotitemModel GetModel()
		{
			return mSlotItemModel;
		}

		public int GetHeight()
		{
			return 75;
		}

		public void SetOnTouchListener(Action<UIRemodelEquipListChildNew> onTouchListener)
		{
			mOnTouchListener = onTouchListener;
		}

		public void OnClickItem()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}

		public void OnClickLock()
		{
			SwitchLockedIcon(Change: true);
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(mTransform_Background.gameObject, value: true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(mTransform_Background.gameObject, value: false);
		}

		public Transform GetTransform()
		{
			if (mTransformCache == null)
			{
				mTransformCache = base.transform;
			}
			return mTransformCache;
		}

		private void SetPlaneSkill(SlotitemModel item)
		{
			if (item != null && item.IsPlane())
			{
				int skillLevel = item.SkillLevel;
				if (skillLevel == 0)
				{
					PlaneSkill.SetActive(isActive: false);
					return;
				}
				PlaneSkill.SetActive(isActive: true);
				PlaneSkill.spriteName = "skill_" + skillLevel;
				PlaneSkill.MakePixelPerfect();
			}
			else
			{
				PlaneSkill.SetActive(isActive: false);
			}
		}
	}
}
