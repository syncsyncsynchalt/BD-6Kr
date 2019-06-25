using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIRevampSlotItemScrollListChildNew : MonoBehaviour, UIScrollListItem<SlotitemModel, UIRevampSlotItemScrollListChildNew>
{
	private const int LEVEL_MAX = 10;

	private UIWidget mWidgetThis;

	[SerializeField]
	private UISprite mSprite_WeaponTypeIcon;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Level;

	[SerializeField]
	private UISprite mSprite_LevelMax;

	[SerializeField]
	private GameObject mLevel;

	[SerializeField]
	private Transform mTransform_BlinkObject;

	[SerializeField]
	private Transform mLock;

	private Action<UIRevampSlotItemScrollListChildNew> mOnTouchListener;

	private Transform mCachedTransform;

	private SlotitemModel mSlotItemModel;

	private int mRealIndex;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
	}

	public void Initialize(int realIndex, SlotitemModel model)
	{
		mRealIndex = realIndex;
		mSlotItemModel = model;
		mWidgetThis.alpha = 1f;
		if (model.Level == 10)
		{
			mLevel.SetActive(true);
			mSprite_LevelMax.SetActive(isActive: true);
		}
		else if (0 < model.Level && model.Level < 10)
		{
			mSprite_LevelMax.SetActive(isActive: false);
			mLevel.SetActive(true);
			mLabel_Level.text = model.Level.ToString();
		}
		else
		{
			mLevel.SetActive(false);
		}
		mLabel_Name.text = model.Name;
		mSprite_WeaponTypeIcon.spriteName = "icon_slot" + model.Type4;
		mLock.SetActive(model.IsLocked() ? true : false);
	}

	public void InitializeDefault(int realIndex)
	{
		mRealIndex = realIndex;
		mSlotItemModel = null;
		mWidgetThis.alpha = 1E-08f;
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
		return 90;
	}

	public void SetOnTouchListener(Action<UIRevampSlotItemScrollListChildNew> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void Touch()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(mTransform_BlinkObject.gameObject, value: true);
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
		UISelectedObject.SelectedOneObjectBlink(mTransform_BlinkObject.gameObject, value: false);
	}

	private void OnDestroy()
	{
		mWidgetThis = null;
		mSprite_WeaponTypeIcon = null;
		mLabel_Name = null;
		mLabel_Level = null;
		mSprite_LevelMax = null;
		mLevel = null;
		mTransform_BlinkObject = null;
		mOnTouchListener = null;
		mCachedTransform = null;
		mSlotItemModel = null;
	}
}
