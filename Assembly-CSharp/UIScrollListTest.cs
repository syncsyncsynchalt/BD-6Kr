using KCV;
using KCV.Resource;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System.Collections;
using UnityEngine;

public class UIScrollListTest : UIScrollList<ShipModel, UIScrollListChildTest>
{
	public static ShipBannerManager sShipBannerManager;

	[SerializeField]
	private ShipBannerManager mShipBannerManager;

	[SerializeField]
	private UILabel mLabel_Scrollisec;

	[SerializeField]
	private UILabel mLabel_ShipName;

	private KeyControl mKeyController;

	protected override void OnAwake()
	{
		sShipBannerManager = mShipBannerManager;
	}

	protected override void OnStart()
	{
		PortManager portManager = new PortManager(1);
		ShipModel[] array = portManager.UserInfo.__GetShipList__().ToArray();
		mShipBannerManager.Initialize(array);
		mKeyController = new KeyControl();
		string text = string.Empty;
		ShipModel[] array2 = array;
		foreach (ShipModel shipModel in array2)
		{
			text += shipModel.Name;
		}
		mLabel_ShipName.text = text;
		Initialize(array);
		HeadFocus();
		StartControl();
	}

	private IEnumerator OnStartCoroutine()
	{
		while (Application.isPlaying)
		{
			Debug.Log(GetListState().ToString());
			yield return new WaitForEndOfFrame();
		}
	}

	protected override void OnUpdate()
	{
		if (mKeyController != null && base.mState == ListState.Waiting)
		{
			mKeyController.Update();
			if (mKeyController.keyState[12].down)
			{
				NextFocus();
			}
			else if (mKeyController.keyState[8].down)
			{
				PrevFocus();
			}
			else if (mKeyController.keyState[14].down)
			{
				PrevPageOrHeadFocus();
			}
			else if (mKeyController.keyState[10].down)
			{
				NextPageOrTailFocus();
			}
			else if (mKeyController.keyState[1].down)
			{
				Select();
			}
			else if (mKeyController.keyState[0].down)
			{
				mKeyController = null;
				StartCoroutine(MoveToPort());
			}
			else if (mKeyController.keyState[4].down | mKeyController.keyState[4].press)
			{
				mScrollCheckLevel -= 1f;
				mLabel_Scrollisec.text = mScrollCheckLevel.ToString();
			}
			else if (mKeyController.keyState[5].down | mKeyController.keyState[5].press)
			{
				mScrollCheckLevel += 1f;
				mLabel_Scrollisec.text = mScrollCheckLevel.ToString();
			}
		}
	}

	private IEnumerator MoveToPort()
	{
		AsyncOperation asyncOperation = Application.LoadLevelAsync("Strategy");
		while (true)
		{
			if (CommonPopupDialog.Instance != null)
			{
				CommonPopupDialog.Instance.StartPopup("MoveToStrategy:" + asyncOperation.progress);
			}
			yield return null;
		}
	}

	protected override void OnChangedFocusView(UIScrollListChildTest focusToView)
	{
		if (!(focusToView == null))
		{
			string mes = focusToView.GetModel().Name + ":" + (focusToView.GetRealIndex() + 1).ToString() + "/" + mModels.Length.ToString();
			if (CommonPopupDialog.Instance != null)
			{
				CommonPopupDialog.Instance.StartPopup(mes);
			}
		}
	}

	protected override void OnSelect(UIScrollListChildTest view)
	{
		if (!(view == null))
		{
		}
	}
}
