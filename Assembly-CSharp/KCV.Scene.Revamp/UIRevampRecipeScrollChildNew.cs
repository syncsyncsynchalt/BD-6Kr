using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRevampRecipeScrollChildNew : MonoBehaviour, UIScrollListItem<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>
	{
		private RevampRecipeScrollUIModel mRevampRecipeScrollUIModel;

		private int mRealIndex;

		private UIWidget mWidgetThis;

		[SerializeField]
		private Transform mBlinkObject;

		[SerializeField]
		private UITexture mTexture_WeaponThumbnail;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

		[SerializeField]
		private UILabel mLabel_WeaponName;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_DevKit;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private UILabel mLabel_RevampKit;

		[SerializeField]
		private UIButton mButton_Select;

		[SerializeField]
		private UISprite mSprite_ButtonState;

		private Transform mCachedTransform;

		private Action<UIRevampRecipeScrollChildNew> mOnTouchListener;

		public float alpha
		{
			get
			{
				if (mWidgetThis != null)
				{
					return mWidgetThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (mWidgetThis != null)
				{
					mWidgetThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
		}

		public void Initialize(int realIndex, RevampRecipeScrollUIModel model)
		{
			mRealIndex = realIndex;
			mRevampRecipeScrollUIModel = model;
			RevampRecipeModel model2 = mRevampRecipeScrollUIModel.Model;
			SlotitemModel_Mst slotitem = model2.Slotitem;
			mTexture_WeaponThumbnail.mainTexture = (Resources.Load("Textures/SlotItems/" + slotitem.MstId + "/2") as Texture);
			mLabel_WeaponName.text = slotitem.Name;
			mSprite_WeaponTypeIcon.spriteName = "icon_slot" + slotitem.Type4;
			mLabel_Fuel.text = model2.Fuel.ToString();
			mLabel_Steel.text = model2.Steel.ToString();
			mLabel_DevKit.text = model2.DevKit.ToString();
			mLabel_Ammo.text = model2.Ammo.ToString();
			mLabel_Bauxite.text = model2.Baux.ToString();
			mLabel_RevampKit.text = model2.RevKit.ToString();
			mWidgetThis.alpha = 1f;
			if (mRevampRecipeScrollUIModel.Clickable)
			{
				mSprite_ButtonState.spriteName = "btn_select";
			}
			else
			{
				mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void Touch()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}

		public void InitializeDefault(int realIndex)
		{
			mWidgetThis.alpha = 1E-08f;
			mRealIndex = realIndex;
			mRevampRecipeScrollUIModel = null;
		}

		public int GetRealIndex()
		{
			return mRealIndex;
		}

		public RevampRecipeScrollUIModel GetModel()
		{
			return mRevampRecipeScrollUIModel;
		}

		public int GetHeight()
		{
			return 140;
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(mBlinkObject.gameObject, value: true);
			if (mRevampRecipeScrollUIModel.Clickable)
			{
				mSprite_ButtonState.spriteName = "btn_select_on";
			}
			else
			{
				mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}

		public Transform GetTransform()
		{
			if (mCachedTransform == null)
			{
				mCachedTransform = base.transform;
			}
			return mCachedTransform;
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(mBlinkObject.gameObject, value: false);
			if (mRevampRecipeScrollUIModel != null && mRevampRecipeScrollUIModel.Clickable)
			{
				mSprite_ButtonState.spriteName = "btn_select";
			}
			else
			{
				mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}

		internal void Hide()
		{
			mWidgetThis.alpha = 1E-11f;
		}

		internal void Show()
		{
			mWidgetThis.alpha = 1f;
		}

		public void SetOnTouchListener(Action<UIRevampRecipeScrollChildNew> onTouchListener)
		{
			mOnTouchListener = onTouchListener;
		}

		private void OnDestroy()
		{
			mRevampRecipeScrollUIModel = null;
			mWidgetThis = null;
			mBlinkObject = null;
			mTexture_WeaponThumbnail = null;
			mSprite_WeaponTypeIcon = null;
			mLabel_WeaponName = null;
			mLabel_Fuel = null;
			mLabel_Ammo = null;
			mLabel_Steel = null;
			mLabel_DevKit = null;
			mLabel_Bauxite = null;
			mLabel_RevampKit = null;
			mButton_Select = null;
			mSprite_ButtonState = null;
			mCachedTransform = null;
			mOnTouchListener = null;
		}
	}
}
