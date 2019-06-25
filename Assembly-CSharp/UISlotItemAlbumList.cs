using KCV;
using KCV.Display;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UISlotItemAlbumList : MonoBehaviour
{
	private const int PAGE_IN_ITEM_COUNT = 10;

	[SerializeField]
	private UITexture mTexture_Focus;

	[SerializeField]
	private UISlotItemAlbumListItem[] mUISlotItemAlbumListItems;

	[SerializeField]
	private UILabel mLabel_PageCurrent;

	[SerializeField]
	private UILabel mLabel_PageTotal;

	[SerializeField]
	private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

	private int mCurrentPageIndex;

	private UISlotItemAlbumListItem mCurrentFocusListItem;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private Action<IAlbumModel> mOnSelectedListItemListener;

	private Action mOnBackListener;

	private IEnumerator mChangePageCoroutine;

	private void Awake()
	{
		UISlotItemAlbumListItem[] array = mUISlotItemAlbumListItems;
		foreach (UISlotItemAlbumListItem uISlotItemAlbumListItem in array)
		{
			uISlotItemAlbumListItem.SetOnSelectedListener(OnSelectedListItemListener);
		}
		mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(SwipeJudgeDelegate);
	}

	private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
	{
		if (mKeyController != null && actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
		{
			if (0.3f < movePercentageX)
			{
				PrevPage();
			}
			else if (movePercentageX < -0.3f)
			{
				NextPage();
			}
		}
	}

	private void Update()
	{
		if (mKeyController == null)
		{
			return;
		}
		if (mKeyController.IsRDown())
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}
		else if (mKeyController.IsRSLeftDown())
		{
			PrevPage();
		}
		else if (mKeyController.IsRSRightDown())
		{
			NextPage();
		}
		else if (mKeyController.keyState[0].down)
		{
			OnBack();
		}
		else if (mKeyController.keyState[1].down)
		{
			OnSelectedListItemListener(mCurrentFocusListItem);
		}
		else if (mKeyController.keyState[14].down)
		{
			int num = Array.IndexOf(mUISlotItemAlbumListItems, mCurrentFocusListItem);
			int num2 = num - 1;
			if (0 <= num2)
			{
				UISlotItemAlbumListItem target = mUISlotItemAlbumListItems[num2];
				ChangeFocus(target, needSe: true);
			}
		}
		else if (mKeyController.keyState[10].down)
		{
			int num3 = Array.IndexOf(mUISlotItemAlbumListItems, mCurrentFocusListItem);
			int num4 = num3 + 1;
			if (num4 < mUISlotItemAlbumListItems.Length)
			{
				UISlotItemAlbumListItem target2 = mUISlotItemAlbumListItems[num4];
				ChangeFocus(target2, needSe: true);
			}
		}
		else if (mKeyController.keyState[8].down)
		{
			int num5 = Array.IndexOf(mUISlotItemAlbumListItems, mCurrentFocusListItem);
			int num6 = num5 - 5;
			if (0 <= num6)
			{
				UISlotItemAlbumListItem target3 = mUISlotItemAlbumListItems[num6];
				ChangeFocus(target3, needSe: true);
			}
		}
		else if (mKeyController.keyState[12].down)
		{
			int num7 = Array.IndexOf(mUISlotItemAlbumListItems, mCurrentFocusListItem);
			int num8 = num7 + 5;
			if (num8 < mUISlotItemAlbumListItems.Length)
			{
				UISlotItemAlbumListItem target4 = mUISlotItemAlbumListItems[num8];
				ChangeFocus(target4, needSe: true);
			}
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void Initialize(IAlbumModel[] albumModels)
	{
		mCurrentPageIndex = 0;
		mAlbumModels = albumModels;
	}

	public void SetOnSelectedListItemListener(Action<IAlbumModel> onSelectedListItemListener)
	{
		mOnSelectedListItemListener = onSelectedListItemListener;
	}

	public void SetOnBackListener(Action onBackListener)
	{
		mOnBackListener = onBackListener;
	}

	public void StartState()
	{
		ChangePage(0, needSe: false);
		ChangeFocus(mUISlotItemAlbumListItems[0], needSe: false);
	}

	private void ChangeFocus(UISlotItemAlbumListItem target, bool needSe)
	{
		if (mCurrentFocusListItem != null && !mCurrentFocusListItem.Equals(target))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
		mCurrentFocusListItem = target;
		if (mCurrentFocusListItem != null)
		{
			UISelectedObject.SelectedOneObjectBlink(mTexture_Focus.gameObject, value: true);
			mTexture_Focus.transform.localPosition = mCurrentFocusListItem.transform.localPosition;
		}
		else
		{
			UISelectedObject.SelectedOneObjectBlink(mTexture_Focus.gameObject, value: false);
		}
	}

	private void NextPage()
	{
		int pageIndex = mCurrentPageIndex + 1;
		if (HasPageAt(pageIndex))
		{
			ChangePage(pageIndex, needSe: true);
			ChangeFocus(mUISlotItemAlbumListItems[0], needSe: false);
		}
	}

	private void PrevPage()
	{
		int pageIndex = mCurrentPageIndex - 1;
		if (HasPageAt(pageIndex))
		{
			ChangePage(pageIndex, needSe: true);
			ChangeFocus(mUISlotItemAlbumListItems[0], needSe: false);
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchEvent()
	{
		OnBack();
	}

	private void OnBack()
	{
		if (mOnBackListener != null)
		{
			mOnBackListener();
		}
	}

	private bool HasPageAt(int pageIndex)
	{
		if (0 <= pageIndex)
		{
			if (pageIndex < mAlbumModels.Length / 10 + ((mAlbumModels.Length % 10 != 0) ? 1 : 0))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private void ChangePage(int pageIndex, bool needSe)
	{
		if (mChangePageCoroutine != null)
		{
			StopCoroutine(mChangePageCoroutine);
		}
		mChangePageCoroutine = ChangePageCoroutine(pageIndex);
		if (needSe)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_002);
		}
		StartCoroutine(mChangePageCoroutine);
	}

	private IEnumerator ChangePageCoroutine(int pageIndex)
	{
		IAlbumModel[] nextPageAlbumModels = mAlbumModels.Skip(pageIndex * 10).Take(10).ToArray();
		mCurrentPageIndex = pageIndex;
		mLabel_PageCurrent.text = (mCurrentPageIndex + 1).ToString();
		mLabel_PageTotal.text = (mAlbumModels.Length / 10 + ((mAlbumModels.Length % 10 != 0) ? 1 : 0)).ToString();
		for (int listItemIndex2 = 0; listItemIndex2 < mUISlotItemAlbumListItems.Length; listItemIndex2++)
		{
			UISlotItemAlbumListItem targetListItem = mUISlotItemAlbumListItems[listItemIndex2];
			targetListItem.Hide();
		}
		for (int listItemIndex = 0; listItemIndex < mUISlotItemAlbumListItems.Length; listItemIndex++)
		{
			UISlotItemAlbumListItem targetListItem2 = mUISlotItemAlbumListItems[listItemIndex];
			if (listItemIndex < nextPageAlbumModels.Length)
			{
				IAlbumModel albumModel = nextPageAlbumModels[listItemIndex];
				IEnumerator childInitializeCoroutine = targetListItem2.GenerateInitializeCoroutine(albumModel);
				yield return StartCoroutine(childInitializeCoroutine);
				targetListItem2.Show();
			}
			else
			{
				targetListItem2.InitializeDefailt();
			}
		}
	}

	private void OnSelectedListItemListener(UISlotItemAlbumListItem calledObject)
	{
		IAlbumModel albumModel = calledObject.GetAlbumModel();
		bool flag = albumModel != null;
		if (mOnSelectedListItemListener != null && flag)
		{
			ChangeFocus(calledObject, needSe: false);
			mOnSelectedListItemListener(albumModel);
		}
	}

	public void ResumeState()
	{
	}

	private void OnDestroy()
	{
		mTexture_Focus = null;
		mUISlotItemAlbumListItems = null;
		mLabel_PageCurrent = null;
		mLabel_PageTotal = null;
		mUIDisplaySwipeEventRegion = null;
		mCurrentFocusListItem = null;
		mKeyController = null;
		mAlbumModels = null;
		mOnSelectedListItemListener = null;
		mOnBackListener = null;
	}
}
