using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIToggle))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(UIWidget))]
	[RequireComponent(typeof(UIButton))]
	public class UIRebellionParticipatingFleetInfo : MonoBehaviour, IRebellionOrganizeSelectObject
	{
		[Serializable]
		private class Delta
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiBackground;

			[SerializeField]
			private UISprite _uiForeground;

			[SerializeField]
			private float _fFlasingTime = 1f;

			private bool _isFlasing;

			public Transform transform => _tra;

			public bool isFlasing
			{
				get
				{
					return _isFlasing;
				}
				set
				{
					if (value)
					{
						_isFlasing = true;
						_uiForeground.transform.LTCancel();
						_uiForeground.transform.LTValue(0f, 1f, _fFlasingTime).setEase(LeanTweenType.easeInSine).setLoopPingPong()
							.setOnUpdate(delegate(float x)
							{
								_uiForeground.alpha = x;
							});
					}
					else
					{
						_isFlasing = value;
						_uiForeground.transform.LTCancel();
						_uiForeground.transform.LTValue(_uiForeground.alpha, 0f, 0.2f).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
						{
							_uiForeground.alpha = x;
						});
					}
				}
			}

			public bool Init()
			{
				_isFlasing = false;
				_uiForeground.alpha = 0f;
				_uiForeground.transform.LTCancel();
				return true;
			}

			public bool UnInit()
			{
				_uiForeground.transform.LTCancel();
				Mem.Del(ref _tra);
				Mem.Del(ref _uiBackground);
				Mem.Del(ref _uiForeground);
				Mem.Del(ref _fFlasingTime);
				return true;
			}
		}

		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private UITexture _uiShipBanner;

		[SerializeField]
		private UISprite _uiFleetNum;

		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private UIButton _uiButton;

		[SerializeField]
		private UIToggle _uiToggle;

		[SerializeField]
		private Delta _clsDelta;

		private RebellionFleetType _iType;

		private DeckModel _clsDeckMode;

		private UIWidget _uiWidget;

		public RebellionFleetType type => _iType;

		public DeckModel deckModel => _clsDeckMode;

		public int index
		{
			get;
			private set;
		}

		public UIButton button
		{
			get
			{
				return _uiButton;
			}
			private set
			{
				_uiButton = value;
			}
		}

		public UIToggle toggle
		{
			get
			{
				return _uiToggle;
			}
			private set
			{
				_uiToggle = value;
			}
		}

		public DelDicideRebellionOrganizeSelectBtn delDicideRebellionOrganizeSelectBtn
		{
			get;
			private set;
		}

		public bool isFlagShipExists
		{
			get
			{
				if (deckModel == null)
				{
					return false;
				}
				return (deckModel.GetFlagShip() != null) ? true : false;
			}
		}

		public UIWidget widget => this.GetComponentThis(ref _uiWidget);

		public static UIRebellionParticipatingFleetInfo Instantiate(UIRebellionParticipatingFleetInfo prefab, Transform parent, Vector3 pos)
		{
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo = UnityEngine.Object.Instantiate(prefab);
			uIRebellionParticipatingFleetInfo.transform.parent = parent;
			uIRebellionParticipatingFleetInfo.transform.localScaleOne();
			uIRebellionParticipatingFleetInfo.transform.localPosition = pos;
			uIRebellionParticipatingFleetInfo.Setup();
			return uIRebellionParticipatingFleetInfo;
		}

		private bool Setup()
		{
			if (_uiBackground == null)
			{
				Util.FindParentToChild(ref _uiBackground, base.transform, "Background");
			}
			if (_uiShipBanner == null)
			{
				Util.FindParentToChild(ref _uiShipBanner, base.transform, "Banner");
			}
			if (_uiFleetNum == null)
			{
				Util.FindParentToChild(ref _uiFleetNum, base.transform, "FleetNum");
			}
			if (_uiLabel == null)
			{
				Util.FindParentToChild(ref _uiLabel, base.transform, "Label");
			}
			if (button == null)
			{
				GetComponent<UIButton>();
			}
			_uiShipBanner.localSize = ResourceManager.SHIP_TEXTURE_SIZE[1];
			_uiButton.onClick = Util.CreateEventDelegateList(this, "Decide", null);
			_clsDelta.Init();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiShipBanner);
			Mem.Del(ref _uiFleetNum);
			Mem.Del(ref _uiLabel);
			Mem.Del(ref _uiButton);
			Mem.Del(ref _uiToggle);
			_clsDelta.UnInit();
			Mem.Del(ref _clsDelta);
			Mem.Del(ref _iType);
			Mem.Del(ref _clsDelta);
			Mem.Del(ref _uiWidget);
		}

		public bool Init(RebellionFleetType iType, DelDicideRebellionOrganizeSelectBtn decideDelegate)
		{
			_iType = iType;
			_uiLabel.text = GetLabelString(iType);
			index = (int)iType;
			SetFleetInfo(null);
			delDicideRebellionOrganizeSelectBtn = decideDelegate;
			_clsDelta.isFlasing = false;
			SetScale(iType);
			return true;
		}

		private void SetScale(RebellionFleetType iType)
		{
			base.transform.localScale = ((iType != RebellionFleetType.DecisiveBattleSupportFleet && iType != RebellionFleetType.VanguardSupportFleet) ? Vector3.one : (Vector3.one * 0.87f));
			_clsDelta.transform.localScale = ((iType != RebellionFleetType.DecisiveBattleSupportFleet && iType != RebellionFleetType.VanguardSupportFleet) ? Vector3.one : (Vector3.one * 1.13f));
			_uiLabel.transform.localScale = ((iType != RebellionFleetType.DecisiveBattleSupportFleet && iType != RebellionFleetType.VanguardSupportFleet) ? Vector3.one : (Vector3.one * 1.13f));
		}

		public void SetFleetInfo(DeckModel model)
		{
			_clsDeckMode = model;
			if (model == null)
			{
				_uiShipBanner.mainTexture = null;
				_uiFleetNum.spriteName = string.Empty;
				_clsDelta.isFlasing = false;
			}
			else
			{
				ShipModel flagShip = model.GetFlagShip();
				_uiShipBanner.mainTexture = ShipUtils.LoadBannerTexture(flagShip);
				_uiFleetNum.spriteName = $"icon_deck{model.Id}";
				_clsDelta.isFlasing = true;
			}
		}

		private string GetLabelString(RebellionFleetType iType)
		{
			switch (iType)
			{
			case RebellionFleetType.VanguardFleet:
				return "前衛艦隊";
			case RebellionFleetType.VanguardSupportFleet:
				return "前衛支援艦隊";
			case RebellionFleetType.DecisiveBattlePrimaryFleet:
				return "決戦主力艦隊";
			case RebellionFleetType.DecisiveBattleSupportFleet:
				return "決戦支援艦隊";
			default:
				return string.Empty;
			}
		}

		public void Decide()
		{
			DebugUtils.Log("UIRebellionFleetInfo", base.gameObject.name);
			if (delDicideRebellionOrganizeSelectBtn != null)
			{
				delDicideRebellionOrganizeSelectBtn(this);
			}
		}
	}
}
