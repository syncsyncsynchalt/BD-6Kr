using KCV;
using KCV.Dialog;
using KCV.Mission.Header;
using KCV.Strategy;
using KCV.View.PopUp.Mission;
using KCV.View.Scroll;
using KCV.View.Scroll.Mission;
using local.managers;
using local.models;
using System;
using System.Collections;
using UnityEngine;

public class UserInterfaceMissionManager : MonoBehaviour
{
	[SerializeField]
	private UIMissionScrollListParent mUIMissionScrollListParent;

	[SerializeField]
	private UIMissionWithTankerDescriptionPopUp mPrefab_UIMissionWithTankerDescriptionPopUp;

	[SerializeField]
	private UIMissionWithTankerConfirmPopUp mPrefab_UIMissionWithTankerConfirmPopUp;

	[SerializeField]
	private UIMissionStateChangedCutin mPrefab_UIMissionStateChangedCutIn;

	[SerializeField]
	private ModalCamera mModalCamera;

	[SerializeField]
	private UIMissionHeader mUIMissionHeader;

	private int mAreaId;

	private int mDeckId;

	private MissionManager mMissionManager;

	private KeyControl mFocusKeyController;

	private UIMissionWithTankerConfirmPopUp ConfirmPopUp;

	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		mAreaId = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
		mDeckId = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
		mMissionManager = new MissionManager(mAreaId);
		mUIMissionHeader.Initialize(mMissionManager, BackToStrategy);
		mUIMissionScrollListParent.Initialize(mMissionManager.GetMissions());
		mUIMissionScrollListParent.SetOnUIScrollListParentAction(delegate(ActionType actionType, UIScrollListParent<MissionModel, UIMissionScrollListChild> calledObject, UIScrollListChild<MissionModel> actionChild)
		{
			this.UIScrollListParentAction(actionType, (UIMissionScrollListParent)calledObject, (UIMissionScrollListChild)actionChild);
		});
		KeyControl nextFocusKeyController = mUIMissionScrollListParent.GetKeyController();
		ChangeFocusKeyController(nextFocusKeyController);
		yield return new WaitForEndOfFrame();
		SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(null);
		PortObjectManager.SceneChangeAct = delegate
		{
			UnityEngine.Object.Destroy(this.gameObject);
		};
	}

	private void Update()
	{
		if (mFocusKeyController != null)
		{
			if (mFocusKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			mFocusKeyController.Update();
		}
	}

	private void UIScrollListParentAction(ActionType actionType, UIMissionScrollListParent calledObject, UIMissionScrollListChild actionChild)
	{
		KeyControl nextFocusKeyController = null;
		switch (actionType)
		{
		case ActionType.OnChangeFocus:
			break;
		case ActionType.OnChangeFirstFocus:
			break;
		case ActionType.OnButtonSelect:
		case ActionType.OnTouch:
			mUIMissionScrollListParent.EnableTouchControl = false;
			actionChild.Hover();
			mModalCamera.Show();
			ShowMissionWithTankerConfirmPopUp(actionChild.Model);
			ChangeFocusKeyController(nextFocusKeyController);
			break;
		case ActionType.OnBack:
			BackToStrategy();
			break;
		}
	}

	private void BackToStrategy()
	{
		mFocusKeyController = null;
		UnityEngine.Object.Destroy(base.gameObject);
		StrategyTaskManager.SceneCallBack();
		PortObjectManager.SceneChangeAct = null;
	}

	private void ShowMissionWithTankerConfirmPopUp(MissionModel missionModel)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		mUIMissionScrollListParent.EnableTouchControl = false;
		UIMissionWithTankerConfirmPopUp component = Util.Instantiate(mPrefab_UIMissionWithTankerConfirmPopUp.gameObject, mModalCamera.gameObject).GetComponent<UIMissionWithTankerConfirmPopUp>();
		component.SetOnUIMissionWithTankerConfirmPopUpAction(UIMissionWithTankerConfirmPopUpAction);
		ChangeFocusKeyController(null);
		component.Initialize(mMissionManager.UserInfo.GetDeck(mDeckId), missionModel, mMissionManager.TankerCount);
	}

	private void UIMissionWithTankerConfirmPopUpAction(UIMissionWithTankerConfirmPopUp.ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject)
	{
		ConfirmPopUp = calledObject;
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (actionType)
		{
		case UIMissionWithTankerConfirmPopUp.ActionType.Hiden:
			break;
		case UIMissionWithTankerConfirmPopUp.ActionType.Shown:
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			KeyControl keyController = calledObject.GetKeyController();
			ChangeFocusKeyController(keyController);
			break;
		}
		case UIMissionWithTankerConfirmPopUp.ActionType.NotStartMission:
			if (calledObject.Opend)
			{
				UnityEngine.Object.Destroy(calledObject.gameObject);
				mModalCamera.Close();
				mUIMissionScrollListParent.EnableTouchControl = true;
				ChangeFocusKeyController(mUIMissionScrollListParent.GetKeyController());
			}
			break;
		case UIMissionWithTankerConfirmPopUp.ActionType.StartMission:
			if (calledObject.Opend)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				UnityEngine.Object.Destroy(calledObject.gameObject);
				mModalCamera.Close();
				ChangeFocusKeyController(null);
				if (mMissionManager.IsValidMissionStart(calledObject.MissionStartDeckModel.Id, calledObject.MissionStartTargetModel.Id, calledObject.SettingTankerCount).Count == 0)
				{
					mMissionManager.MissionStart(calledObject.MissionStartDeckModel.Id, calledObject.MissionStartTargetModel.Id, calledObject.SettingTankerCount);
					ShowCutin(mMissionManager.UserInfo.GetDeck(mDeckId), delegate
					{
						BackToStrategy();
					});
				}
			}
			break;
		case UIMissionWithTankerConfirmPopUp.ActionType.ShowDetail:
			if (calledObject.Opend)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				ShowMissionWithTankerDescriptionPopUp(calledObject.MissionStartTargetModel);
			}
			break;
		}
	}

	private void OnDestroy()
	{
		mMissionManager = null;
		mFocusKeyController = null;
		ConfirmPopUp = null;
		mUIMissionScrollListParent = null;
		mPrefab_UIMissionWithTankerDescriptionPopUp = null;
		mPrefab_UIMissionWithTankerConfirmPopUp = null;
		mPrefab_UIMissionStateChangedCutIn = null;
		mModalCamera = null;
		mUIMissionHeader = null;
	}

	private void ShowCutin(DeckModel deckModel, Action onFinishedCallBack)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		UIMissionStateChangedCutin component = Util.Instantiate(mPrefab_UIMissionStateChangedCutIn.gameObject, mModalCamera.gameObject).GetComponent<UIMissionStateChangedCutin>();
		component.Initialize(deckModel);
		component.PlayStartCutin(onFinishedCallBack);
	}

	private void ShowMissionWithTankerDescriptionPopUp(MissionModel model)
	{
		mUIMissionScrollListParent.EnableTouchControl = false;
		UIMissionWithTankerDescriptionPopUp component = Util.Instantiate(mPrefab_UIMissionWithTankerDescriptionPopUp.gameObject, mModalCamera.gameObject).GetComponent<UIMissionWithTankerDescriptionPopUp>();
		mModalCamera.Show();
		component.Initialize(model);
		component.SetOnUIMissionWithTankerDescriptionPopUpAction(UIMissionWithTankerDescriptionPopUpAction);
		component.Show();
		ChangeFocusKeyController(component.GetKeyController());
	}

	private void UIMissionWithTankerDescriptionPopUpAction(UIMissionWithTankerDescriptionPopUp.ActionType actionType, UIMissionWithTankerDescriptionPopUp calledObject)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (actionType)
		{
		case UIMissionWithTankerDescriptionPopUp.ActionType.Shown:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			TweenAlpha.Begin(ConfirmPopUp.gameObject, 0.2f, 0f);
			break;
		case UIMissionWithTankerDescriptionPopUp.ActionType.Hiden:
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			KeyControl keyController = ConfirmPopUp.GetKeyController();
			ChangeFocusKeyController(keyController);
			UnityEngine.Object.Destroy(calledObject.gameObject);
			mModalCamera.Close();
			TweenAlpha.Begin(ConfirmPopUp.gameObject, 0f, 1f);
			break;
		}
		}
	}

	private void ChangeFocusKeyController(KeyControl nextFocusKeyController)
	{
		if (mFocusKeyController != null)
		{
			mFocusKeyController.firstUpdate = true;
			mFocusKeyController.ClearKeyAll();
		}
		mFocusKeyController = nextFocusKeyController;
		if (mFocusKeyController != null)
		{
			mFocusKeyController.firstUpdate = true;
			mFocusKeyController.ClearKeyAll();
		}
	}
}
