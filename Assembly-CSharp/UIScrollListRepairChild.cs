using Common.Enum;
using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class UIScrollListRepairChild : MonoBehaviour, UIScrollListItem<ShipModel, UIScrollListRepairChild>
{
	[SerializeField]
	private UITexture mTexture_Flag;

	[SerializeField]
	private UISprite ShipTypeIcon;

	[SerializeField]
	private UILabel Label_name;

	[SerializeField]
	private UILabel Label_hp_all;

	[SerializeField]
	private UISprite HP_bar_grn;

	[SerializeField]
	private UILabel Label_lv;

	[SerializeField]
	private UISprite hakai;

	[SerializeField]
	private GameObject mBackground;

	private int mRealIndex;

	private ShipModel mShipModel;

	private UIWidget mWidgetThis;

	private Transform mTransform;

	private Action<UIScrollListRepairChild> mOnTouchListener;

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 1E-09f;
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		mRealIndex = realIndex;
		mShipModel = model;
		InitializeDeckFlag(model);
		ShipTypeIcon.spriteName = "ship" + model.ShipType;
		Label_name.text = model.Name;
		Label_hp_all.text = model.NowHp + "/" + model.MaxHp;
		HP_bar_grn.width = model.NowHp * 1580 / model.MaxHp / 10;
		HP_bar_grn.color = Util.HpGaugeColor2(model.MaxHp, model.NowHp);
		Label_lv.text = model.Level.ToString();
		if (model.IsInRepair())
		{
			hakai.spriteName = "icon-s_syufuku";
		}
		else if (model.DamageStatus == DamageState.Taiha)
		{
			hakai.spriteName = "icon-s_taiha";
		}
		else if (model.DamageStatus == DamageState.Tyuuha)
		{
			hakai.spriteName = "icon-s_chuha";
		}
		else if (model.DamageStatus == DamageState.Shouha)
		{
			hakai.spriteName = "icon-s_shoha";
		}
		else
		{
			hakai.spriteName = null;
		}
		if (model.IsInMission())
		{
			hakai.spriteName = "icon-s_ensei";
		}
		if (model.IsInActionEndDeck())
		{
			hakai.spriteName = "icon-s_done";
		}
		mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		mRealIndex = realIndex;
		mShipModel = null;
		mWidgetThis.alpha = 1E-09f;
	}

	public Transform GetTransform()
	{
		if (mTransform == null)
		{
			mTransform = base.transform;
		}
		return mTransform;
	}

	public ShipModel GetModel()
	{
		return mShipModel;
	}

	public int GetHeight()
	{
		return 62;
	}

	public void Touch()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}

	public void SetOnTouchListener(Action<UIScrollListRepairChild> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(mBackground, value: true);
	}

	public void RemoveHover()
	{
		UISelectedObject.SelectedOneObjectBlink(mBackground, value: false);
	}

	public int GetRealIndex()
	{
		return mRealIndex;
	}

	private void InitializeDeckFlag(ShipModel shipModel)
	{
		if (IsDeckInShip(shipModel))
		{
			DeckModelBase deck = shipModel.getDeck();
			bool isFlagShip = deck.GetFlagShip().MemId == shipModel.MemId;
			int id = deck.Id;
			if (deck.IsEscortDeckMyself())
			{
				InitializeEscortDeckFlag(id, isFlagShip);
			}
			else
			{
				InitializeNormalDeckFlag(id, isFlagShip);
			}
		}
		else if (shipModel.IsBling())
		{
			BlingDeckFlag();
		}
		else
		{
			RemoveDeckFlag();
		}
	}

	private bool IsDeckInShip(ShipModel shipModel)
	{
		DeckModelBase deck = shipModel.getDeck();
		return deck != null;
	}

	private void RemoveDeckFlag()
	{
		mTexture_Flag.SetDimensions(0, 0);
		mTexture_Flag.mainTexture = null;
	}

	private void BlingDeckFlag()
	{
		mTexture_Flag.mainTexture = (Resources.Load("Textures/repair/NewUI/icon-ss_kaiko") as Texture2D);
		mTexture_Flag.SetDimensions(70, 59);
	}

	private void InitializeNormalDeckFlag(int deckId, bool isFlagShip)
	{
		string empty = string.Empty;
		int w = 60;
		int h = 56;
		string str = (!isFlagShip) ? $"icon_deck{deckId}" : $"icon_deck{deckId}_fs";
		mTexture_Flag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + str) as Texture2D);
		mTexture_Flag.SetDimensions(w, h);
	}

	private void InitializeEscortDeckFlag(int deckId, bool isFlagShip)
	{
		string empty = string.Empty;
		int w = 56;
		int h = 64;
		string str = (!isFlagShip) ? "icon_guard" : "icon_guard_fs";
		mTexture_Flag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + str) as Texture2D);
		mTexture_Flag.SetDimensions(w, h);
	}
}
