using KCV;
using KCV.Display;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[SelectionBase]
public class UIShipAlbumList : MonoBehaviour
{
	private const int PAGE_IN_ITEM_COUNT = 10;

	[SerializeField]
	private UITexture mTexture_Focus;

	[SerializeField]
	private UIShipAlbumListItem[] mUIShipAlbumListItems;

	[SerializeField]
	private UILabel mLabel_PageCurrent;

	[SerializeField]
	private UILabel mLabel_PageTotal;

	[SerializeField]
	private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

	private int mCurrentPageIndex;

	private UIShipAlbumListItem mCurrentFocusListItem;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private Action<IAlbumModel> mOnSelectedListItemListener;

	private Action mOnBackListener;

	private IEnumerator mChangePageCoroutine;

	private void Awake()
	{
		UIShipAlbumListItem[] array = mUIShipAlbumListItems;
		foreach (UIShipAlbumListItem uIShipAlbumListItem in array)
		{
			uIShipAlbumListItem.SetOnSelectedListener(OnSelectedListItemListener);
		}
		mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(SwipeJudgeDelegate);
	}

	private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
	{
		if (mKeyController != null && actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
		{
			if (0.2f < movePercentageX)
			{
				PrevPage();
			}
			else if (movePercentageX < -0.1f)
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
			int num = Array.IndexOf(mUIShipAlbumListItems, mCurrentFocusListItem);
			int num2 = num - 1;
			if (0 <= num2)
			{
				UIShipAlbumListItem target = mUIShipAlbumListItems[num2];
				ChangeFocus(target, needSe: true);
			}
		}
		else if (mKeyController.keyState[10].down)
		{
			int num3 = Array.IndexOf(mUIShipAlbumListItems, mCurrentFocusListItem);
			int num4 = num3 + 1;
			if (num4 < mUIShipAlbumListItems.Length)
			{
				UIShipAlbumListItem target2 = mUIShipAlbumListItems[num4];
				ChangeFocus(target2, needSe: true);
			}
		}
		else if (mKeyController.keyState[8].down)
		{
			int num5 = Array.IndexOf(mUIShipAlbumListItems, mCurrentFocusListItem);
			int num6 = num5 - 5;
			if (0 <= num6)
			{
				UIShipAlbumListItem target3 = mUIShipAlbumListItems[num6];
				ChangeFocus(target3, needSe: true);
			}
		}
		else if (mKeyController.keyState[12].down)
		{
			int num7 = Array.IndexOf(mUIShipAlbumListItems, mCurrentFocusListItem);
			int num8 = num7 + 5;
			if (num8 < mUIShipAlbumListItems.Length)
			{
				UIShipAlbumListItem target4 = mUIShipAlbumListItems[num8];
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
		ChangeFocus(mUIShipAlbumListItems[0], needSe: false);
	}

	private void ChangeFocus(UIShipAlbumListItem target, bool needSe)
	{
		if (mCurrentFocusListItem != null && !mCurrentFocusListItem.Equals(target) && needSe)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
		mCurrentFocusListItem = target;
		if (mCurrentFocusListItem != null)
		{
			mTexture_Focus.transform.localPosition = mCurrentFocusListItem.transform.localPosition;
			UISelectedObject.SelectedOneObjectBlink(mTexture_Focus.gameObject, value: true);
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
			ChangeFocus(mUIShipAlbumListItems[0], needSe: false);
		}
	}

	private void PrevPage()
	{
		int pageIndex = mCurrentPageIndex - 1;
		if (HasPageAt(pageIndex))
		{
			ChangePage(pageIndex, needSe: true);
			ChangeFocus(mUIShipAlbumListItems[0], needSe: false);
		}
	}

	[Obsolete("Inspector上で選択して使用します.")]
	public void OnTouchBack()
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

	private void OnDestroy()
	{
		mTexture_Focus = null;
		mUIShipAlbumListItems = null;
		mLabel_PageCurrent = null;
		mLabel_PageTotal = null;
		mUIDisplaySwipeEventRegion = null;
		mCurrentFocusListItem = null;
		mKeyController = null;
		mAlbumModels = null;
		mOnSelectedListItemListener = null;
		mOnBackListener = null;
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
		for (int listItemIndex2 = 0; listItemIndex2 < mUIShipAlbumListItems.Length; listItemIndex2++)
		{
			UIShipAlbumListItem targetListItem = mUIShipAlbumListItems[listItemIndex2];
			if (listItemIndex2 < nextPageAlbumModels.Length)
			{
				targetListItem.Hide();
			}
		}
		for (int listItemIndex = 0; listItemIndex < mUIShipAlbumListItems.Length; listItemIndex++)
		{
			UIShipAlbumListItem targetListItem2 = mUIShipAlbumListItems[listItemIndex];
			if (listItemIndex < nextPageAlbumModels.Length)
			{
				IAlbumModel albumModel = nextPageAlbumModels[listItemIndex];
				IEnumerator childInitializeCoroutine = targetListItem2.GenerateInitializeCoroutine(albumModel);
				yield return StartCoroutine(childInitializeCoroutine);
				targetListItem2.Show();
			}
		}
	}

	private void OnSelectedListItemListener(UIShipAlbumListItem calledObject)
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
}
