using Common.Enum;
using DG.Tweening;
using KCV;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionWithTankerConfirmPopUp : MonoBehaviour
{
	public enum ActionType
	{
		StartMission,
		NotStartMission,
		Shown,
		Hiden,
		ShowDetail
	}

	public enum CheckType
	{
		CallTankerCountUp,
		CallTankerCountDown,
		CanStartCheck
	}

	public delegate void UIMissionWithTankerConfirmPopUpAction(ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject);

	public delegate bool UIMissionWithTankerConfirmPopUpCheck(CheckType actionType, UIMissionWithTankerConfirmPopUp calledObject);

	[SerializeField]
	private UIButtonManager mButtonManager;

	[SerializeField]
	private UILabel mLabel_RequireDay;

	[SerializeField]
	private UILabel mLabel_RequireTanker;

	[SerializeField]
	private UILabel mLabel_SettingTankerValue;

	[SerializeField]
	private UILabel mLabel_HasTankerCount;

	[SerializeField]
	private UIButton mButton_Positive;

	[SerializeField]
	private UIButton mButton_Negative;

	[SerializeField]
	private UIButton mButton_Description;

	[SerializeField]
	private UIButton mButton_CountUp;

	[SerializeField]
	private UIButton mButton_CountDown;

	[SerializeField]
	private UIMissionShipBanner[] mUIMissionShipBanners;

	[SerializeField]
	private UILabel mLabel_Message;

	private UIButton mFocusButton;

	private KeyControl mKeyController;

	private UIMissionWithTankerConfirmPopUpAction mUIMissionWithTankerConfirmPopUpAction;

	private Coroutine mInitializeCoroutine;

	private int mHasTankerCount;

	private UIButton[] mSelectableButtons;

	private bool isGoCondition;

	public int SettingTankerCount
	{
		get;
		private set;
	}

	public DeckModel MissionStartDeckModel
	{
		get;
		private set;
	}

	public MissionModel MissionStartTargetModel
	{
		get;
		private set;
	}

	public bool Opend
	{
		get;
		private set;
	}

	public void Initialize(DeckModel deckModel, MissionModel missionModel, int hasTankerCount)
	{
		mLabel_Message.alpha = 0.01f;
		MissionStartTargetModel = missionModel;
		MissionStartDeckModel = deckModel;
		mHasTankerCount = hasTankerCount;
		List<UIButton> list = new List<UIButton>();
		list.Add(mButton_Negative);
		if (missionModel.TankerMinCount <= hasTankerCount)
		{
			list.Add(mButton_Positive);
		}
		list.Add(mButton_Description);
		mSelectableButtons = list.ToArray();
		mButtonManager.IndexChangeAct = delegate
		{
			UIButton uIButton = mButtonManager.GetFocusableButtons()[mButtonManager.nowForcusIndex];
			if (0 <= Array.IndexOf(mSelectableButtons, uIButton))
			{
				ChangeFocusButton(uIButton);
			}
		};
		if (mInitializeCoroutine != null)
		{
			StopCoroutine(mInitializeCoroutine);
			mInitializeCoroutine = null;
		}
		mInitializeCoroutine = StartCoroutine(InitailizeCoroutine(deckModel, missionModel, hasTankerCount, delegate
		{
			mInitializeCoroutine = null;
			CallBackAction(ActionType.Shown, this);
			Opend = true;
		}));
	}

	private IEnumerator InitailizeCoroutine(DeckModel deckModel, MissionModel missionModel, int hasTankerCount, Action callBack)
	{
		float delayStart = Time.deltaTime;
		mLabel_RequireDay.text = missionModel.Turn.ToString();
		mLabel_RequireTanker.text = missionModel.TankerCount.ToString();
		mLabel_HasTankerCount.text = hasTankerCount.ToString();
		ShipModel[] ships = deckModel.GetShips();
		if (missionModel.TankerMinCount <= hasTankerCount)
		{
			SettingTankerCount = missionModel.TankerMinCount;
			UpdateSettingTankerCountLabel(SettingTankerCount, isPoor: false);
		}
		else
		{
			int num = hasTankerCount - missionModel.TankerMinCount;
			UpdateSettingTankerCountLabel(SettingTankerCount, isPoor: true);
			mButton_Positive.isEnabled = false;
			mButton_CountUp.isEnabled = false;
			mButton_CountDown.isEnabled = false;
		}
		List<IsGoCondition> sortieCondition = new MissionManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID).IsValidMissionStart(MissionStartDeckModel.Id, MissionStartTargetModel.Id, SettingTankerCount);
		bool conditionOk = sortieCondition.Count <= 0;
		bool otherDeckInMission = missionModel.Deck != null;
		if (conditionOk && !otherDeckInMission)
		{
			mButton_Positive.isEnabled = true;
			mLabel_Message.text = string.Empty;
			mLabel_Message.color = Color.black;
			mLabel_Message.alpha = 1E-09f;
			mLabel_Message.text = "この艦隊で遠征しますか？";
		}
		else
		{
			mButton_Positive.isEnabled = false;
			mLabel_Message.text = string.Empty;
			mLabel_Message.color = Color.red;
			mLabel_Message.alpha = 1E-09f;
			mLabel_Message.text = GoConditionToString(sortieCondition[sortieCondition.Count - 1]);
		}
		isGoCondition = conditionOk;
		yield return new WaitForEndOfFrame();
		ChangeFocusButton(mButton_Negative);
		for (int i = 0; i < ships.Length; i++)
		{
			mUIMissionShipBanners[i].Initialize(i + 1, ships[i]);
			mUIMissionShipBanners[i].Show();
			UIUtil.AnimationOnFocus(mUIMissionShipBanners[i].transform, null);
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForEndOfFrame();
		Vector3 localPosition = mLabel_Message.transform.localPosition;
		float x = localPosition.x - 50f;
		Vector3 localPosition2 = mLabel_Message.transform.localPosition;
		float y = localPosition2.y;
		Vector3 localPosition3 = mLabel_Message.transform.localPosition;
		Vector3 from = new Vector3(x, y, localPosition3.z);
		Vector3 to = mLabel_Message.transform.localPosition;
		mLabel_Message.transform.localPosition = from;
		mLabel_Message.transform.DOLocalMove(to, 0.3f);
		DOVirtual.DelayedCall(Time.deltaTime - delayStart, delegate
		{
			this.mLabel_Message.transform.DOLocalMove(to, 0.3f);
		});
		mLabel_Message.alpha = 1f;
		callBack?.Invoke();
	}

	public string GoConditionToString(IsGoCondition condition)
	{
		switch (condition)
		{
		case IsGoCondition.AnotherArea:
			return "艦隊は他の海域にいます";
		case IsGoCondition.ActionEndDeck:
			return "行動終了している艦隊です";
		case IsGoCondition.Mission:
			return "艦隊は遠征中です";
		case IsGoCondition.Deck1:
			return "第一艦隊です";
		case IsGoCondition.HasBling:
			return "回航艦を含んでいます";
		case IsGoCondition.HasRepair:
			return "艦隊は入渠中の艦を含んでいます";
		case IsGoCondition.FlagShipTaiha:
			return "旗艦が大破しています";
		case IsGoCondition.ReqFullSupply:
			return "燃料/弾薬が最大の必要があります";
		case IsGoCondition.NeedSupply:
			return "燃料/弾薬が0の艦を含んでいます(補給が必要です)";
		case IsGoCondition.ConditionRed:
			return "疲労度-赤の艦を含んでいます";
		case IsGoCondition.Tanker:
			return "輸送船が不足しています";
		case IsGoCondition.NecessaryStype:
			return "特定の艦種が必要です";
		case IsGoCondition.InvalidOrganization:
			return "艦隊の編成は条件を満たしていません";
		case IsGoCondition.OtherDeckMissionRunning:
			return "既に他の艦隊が遠征しています";
		default:
			return string.Empty;
		}
	}

	public KeyControl GetKeyController()
	{
		if (mKeyController == null)
		{
			mKeyController = new KeyControl(0, 2);
			mKeyController.setChangeValue(0f, 1f, 0f, -1f);
		}
		return mKeyController;
	}

	private void Update()
	{
		if (mKeyController == null)
		{
			return;
		}
		if (mKeyController.keyState[14].down)
		{
			int num = Array.IndexOf(mSelectableButtons, mFocusButton);
			int num2 = num - 1;
			if (0 > num2)
			{
				return;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			UIButton uIButton = mSelectableButtons[num2];
			if (uIButton.isEnabled)
			{
				ChangeFocusButton(uIButton);
				return;
			}
			int num3 = num2 - 1;
			if (0 <= num3)
			{
				UIButton uIButton2 = mSelectableButtons[num3];
				if (uIButton2.isEnabled)
				{
					ChangeFocusButton(uIButton2);
				}
			}
		}
		else if (mKeyController.keyState[10].down)
		{
			int num4 = Array.IndexOf(mSelectableButtons, mFocusButton);
			int num5 = num4 + 1;
			if (num5 >= mSelectableButtons.Length)
			{
				return;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			UIButton uIButton3 = mSelectableButtons[num5];
			if (uIButton3.isEnabled)
			{
				ChangeFocusButton(uIButton3);
				return;
			}
			int num6 = num5 + 1;
			if (num6 < mSelectableButtons.Length)
			{
				UIButton uIButton4 = mSelectableButtons[num6];
				if (uIButton4.isEnabled)
				{
					ChangeFocusButton(uIButton4);
				}
			}
		}
		else if (mKeyController.keyState[1].down)
		{
			mFocusButton.SendMessage("OnClick");
		}
		else if (mKeyController.keyState[0].down)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			mButton_Negative.SendMessage("OnClick");
		}
		else if (mKeyController.keyState[8].down)
		{
			mButton_CountUp.SendMessage("OnClick");
		}
		else if (mKeyController.keyState[12].down)
		{
			mButton_CountDown.SendMessage("OnClick");
		}
	}

	public void OnClickCountUpTanker()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		CountUpTanker();
	}

	public void OnClickCountDownTanker()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		CountDownTanker();
	}

	public void OnClickPositiveButton()
	{
		if (isGoCondition)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			CallBackAction(ActionType.StartMission, this);
		}
	}

	public void OnClickNegativeButton()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		CallBackAction(ActionType.NotStartMission, this);
	}

	public void OnClickDescriptionButton()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		CallBackAction(ActionType.ShowDetail, this);
	}

	public void SetOnUIMissionWithTankerConfirmPopUpAction(UIMissionWithTankerConfirmPopUpAction action)
	{
		mUIMissionWithTankerConfirmPopUpAction = action;
	}

	private void CountUpTanker()
	{
		UIUtil.AnimationOnFocus(mButton_CountUp.transform, null);
		if (SettingTankerCount + 1 <= MissionStartTargetModel.TankerMaxCount && SettingTankerCount + 1 <= mHasTankerCount)
		{
			SettingTankerCount++;
			UpdateSettingTankerCountLabel(SettingTankerCount, isPoor: false);
		}
	}

	private void CountDownTanker()
	{
		UIUtil.AnimationOnFocus(mButton_CountDown.transform, null);
		if (MissionStartTargetModel.TankerMinCount <= SettingTankerCount - 1)
		{
			SettingTankerCount--;
			UpdateSettingTankerCountLabel(SettingTankerCount, isPoor: false);
		}
	}

	private void UpdateSettingTankerCountLabel(int value, bool isPoor)
	{
		mLabel_SettingTankerValue.text = value.ToString();
		if (isPoor)
		{
			mLabel_SettingTankerValue.color = Color.red;
		}
	}

	private void ChangeFocusButton(UIButton target)
	{
		if (mFocusButton != null && mFocusButton.isEnabled)
		{
			mFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
		}
		mFocusButton = target;
		if (mFocusButton != null && mFocusButton.isEnabled)
		{
			mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
		}
	}

	private void CallBackAction(ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject)
	{
		if (mUIMissionWithTankerConfirmPopUpAction != null)
		{
			mUIMissionWithTankerConfirmPopUpAction(actionType, calledObject);
		}
	}

	private void OnDestroy()
	{
		mButtonManager = null;
		mLabel_RequireDay = null;
		mLabel_RequireTanker = null;
		mLabel_SettingTankerValue = null;
		mLabel_HasTankerCount = null;
		mButton_Positive = null;
		mButton_Negative = null;
		mButton_Description = null;
		mButton_CountUp = null;
		mButton_CountDown = null;
		mLabel_Message = null;
		mFocusButton = null;
		mUIMissionWithTankerConfirmPopUpAction = null;
		mInitializeCoroutine = null;
		mSelectableButtons = null;
		mUIMissionShipBanners = null;
	}
}
