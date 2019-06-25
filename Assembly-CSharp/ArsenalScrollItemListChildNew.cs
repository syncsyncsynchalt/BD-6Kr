using KCV;
using KCV.Arsenal;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class ArsenalScrollItemListChildNew : MonoBehaviour, UIScrollListItem<ArsenalScrollSlotItemListChoiceModel, ArsenalScrollItemListChildNew>
{
	private ArsenalScrollSlotItemListChoiceModel mArsenalScrollItemListChoiceModel;

	private UIWidget mWidgetThis;

	[SerializeField]
	private UILabel mLabel_Rare;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UISprite mSprite_Check;

	[SerializeField]
	private UISprite mSprite_CheckBox;

	[SerializeField]
	private UISprite mSprite_SlotItem;

	[SerializeField]
	private UISprite mSprite_LockIcon;

	[SerializeField]
	private Transform mTransform_Background;

	private Transform mCachedTransform;

	private Action<ArsenalScrollItemListChildNew> mOnTouchListener;

	private int mRealIndex;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 1E-08f;
	}

	private void InitializeItemInfo(SlotitemModel slotItemModel)
	{
		mLabel_Rare.text = Util.RareToString(slotItemModel.Rare);
		mLabel_Name.text = slotItemModel.Name;
		mSprite_SlotItem.spriteName = "icon_slot" + slotItemModel.Type4;
		mSprite_LockIcon.spriteName = ((!slotItemModel.IsLocked()) ? null : "lock_on");
	}

	private void UpdateListSelect(bool enabled)
	{
		mSprite_CheckBox.spriteName = ((!enabled) ? "check_bg" : "check_bg_on");
	}

	public int GetRealIndex()
	{
		return mRealIndex;
	}

	public int GetHeight()
	{
		return 54;
	}

	public void SetOnTouchListener(Action<ArsenalScrollItemListChildNew> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(mTransform_Background.gameObject, value: true);
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
		UISelectedObject.SelectedOneObjectBlink(mTransform_Background.gameObject, value: false);
	}

	private void OnDestroy()
	{
		mArsenalScrollItemListChoiceModel = null;
		mLabel_Rare = null;
		mLabel_Name = null;
		mSprite_Check = null;
		mSprite_CheckBox = null;
		mSprite_SlotItem = null;
		mSprite_LockIcon = null;
		mTransform_Background = null;
		mCachedTransform = null;
		mOnTouchListener = null;
	}

	public void Initialize(int realIndex, ArsenalScrollSlotItemListChoiceModel model)
	{
		mRealIndex = realIndex;
		mArsenalScrollItemListChoiceModel = model;
		mSprite_Check.alpha = 1E-08f;
		InitializeItemInfo(mArsenalScrollItemListChoiceModel.GetSlotItemModel());
		UpdateSelectedView(mArsenalScrollItemListChoiceModel.Selected);
		mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		mRealIndex = realIndex;
		mArsenalScrollItemListChoiceModel = null;
		mWidgetThis.alpha = 1E-08f;
	}

	private void UpdateSelectedView(bool selected)
	{
		mSprite_Check.alpha = ((!selected) ? 1E-09f : 1f);
	}

	public ArsenalScrollSlotItemListChoiceModel GetModel()
	{
		return mArsenalScrollItemListChoiceModel;
	}

	public void Touch()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}
}
