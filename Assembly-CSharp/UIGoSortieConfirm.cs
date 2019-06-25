using Common.Struct;
using KCV;
using KCV.Strategy;
using local.models;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UIGoSortieConfirm : MonoBehaviour
{
	[SerializeField]
	private DeckStateViews deckStateViews;

	[SerializeField]
	private YesNoButton yesNoButton;

	[SerializeField]
	private UILabel Title;

	[SerializeField]
	private UITexture BGTex;

	[SerializeField]
	private UITexture DeckIconNo;

	[SerializeField]
	private UILabel AlertMessage;

	private static readonly Color32 AlertColor = new Color32(byte.MaxValue, 50, 50, byte.MaxValue);

	private static readonly Color32 WarningColor = new Color32(238, byte.MaxValue, 77, byte.MaxValue);

	private bool isInfoMode;

	private void OnDestroy()
	{
		deckStateViews = null;
		yesNoButton = null;
		Title = null;
		BGTex = null;
		DeckIconNo = null;
	}

	private void Awake()
	{
		isInfoMode = false;
	}

	private IEnumerator Start()
	{
		yield return null;
		base.transform.SetActive(isActive: false);
	}

	public void SetPushYesButton(Action act)
	{
		yesNoButton.SetOnSelectPositiveListener(act);
	}

	public void SetPushNoButton(Action act)
	{
		yesNoButton.SetOnSelectNegativeListener(act);
	}

	public void Initialize(DeckModel deckModel, bool isConfirm)
	{
		deckStateViews.Initialize(deckModel, !isConfirm);
		DeckIconNo.mainTexture = (Resources.Load("Textures/Common/DeckFlag/icon_deck" + deckModel.Id) as Texture2D);
		if (isConfirm)
		{
			setConfirmMode(deckModel);
		}
		else
		{
			setDeckInfoMode(deckModel);
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		yesNoButton.SetKeyController(keyController, isFocusLeft: false);
	}

	private void setConfirmMode(DeckModel deck)
	{
		base.transform.localPositionY(0f);
		yesNoButton.SetActive(isActive: true);
		Title.text = "この艦隊で出撃しますか？";
		BGTex.transform.localPositionY(0f);
		BGTex.height = 528;
		isInfoMode = false;
		AlertMessage.text = getAlertMessage(deck);
	}

	private void setDeckInfoMode(DeckModel deck)
	{
		base.transform.localPositionY(-17f);
		yesNoButton.SetActive(isActive: false);
		Title.text = deck.Name;
		Title.supportEncoding = false;
		BGTex.transform.localPositionY(20f);
		BGTex.height = 457;
		isInfoMode = true;
		AlertMessage.text = string.Empty;
	}

	private string getAlertMessage(DeckModel deck)
	{
		UserInfoModel userInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
		MemberMaxInfo memberMaxInfo = userInfo.ShipCountData();
		MemberMaxInfo memberMaxInfo2 = userInfo.SlotitemCountData();
		if (deck.GetShips().Any((ShipModel x) => x.IsTaiha()))
		{
			AlertMessage.color = AlertColor;
			return "※大破している艦娘がいます。（被弾により喪失の危険があります）";
		}
		if (memberMaxInfo.MaxCount <= memberMaxInfo.NowCount)
		{
			AlertMessage.color = AlertColor;
			return "※艦娘保有数が上限に達しているため、新しい艦娘との邂逅はできません。";
		}
		if (memberMaxInfo.MaxCount - 6 <= memberMaxInfo.NowCount)
		{
			AlertMessage.color = WarningColor;
			return "※艦娘保有数が上限に近いため、新しい艦娘と邂逅できない可能性があります。";
		}
		if (memberMaxInfo2.MaxCount <= memberMaxInfo2.NowCount)
		{
			AlertMessage.color = AlertColor;
			return "※装備保有数が上限に達しているため、新しい艦娘との邂逅はできません。";
		}
		if (memberMaxInfo2.MaxCount - 24 <= memberMaxInfo2.NowCount)
		{
			AlertMessage.color = WarningColor;
			return "※装備保有数が上限に近いため、新しい艦娘と邂逅できない可能性があります。";
		}
		if (deck.GetShips().Any((ShipModel x) => x.FuelRate < 100.0 || x.AmmoRate < 100.0))
		{
			AlertMessage.color = WarningColor;
			return "※十分な補給を受けていない艦娘がいます。（本来の戦闘力を発揮できません）";
		}
		return string.Empty;
	}

	private void Update()
	{
		if (isInfoMode && (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.S)))
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
		}
	}
}
