using Common.Enum;
using DG.Tweening;
using KCV.Display;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Supply
{
	public class SupplyMainManager : MonoBehaviour, CommonDeckSwitchHandler
	{
		public enum ScreenStatus
		{
			SHIP_SELECT,
			RIGHT_PAIN_WORK,
			SUPPLY_PROCESS,
			SHIP_RECOVERY_ANIMATION
		}

		public SupplyManager SupplyManager;

		[SerializeField]
		private Texture[] mPreloads_Texture;

		[SerializeField]
		private UIShipSortButton mShipSortButton;

		[SerializeField]
		public ShipBannerContainer _shipBannerContainer;

		[SerializeField]
		public OtherShipListScrollNew _otherListParent;

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		[SerializeField]
		private UILabel _deckName;

		[SerializeField]
		private UITexture _bauxiteMsgSuccess;

		[SerializeField]
		private UITexture _bauxiteMsgIncomplete;

		[SerializeField]
		private Transform _mTrans_TurnEndStamp;

		[SerializeField]
		private SupplyRightPane _rightPane;

		[SerializeField]
		private CommonDeckSwitchManager _commonDeckSwitchManager;

		private bool _isControllDone;

		private DeckModel[] _decks;

		private DeckModel _currentDeck;

		private ScreenStatus _status;

		public static SupplyMainManager Instance
		{
			get;
			private set;
		}

		public KeyControl KeyController
		{
			get;
			private set;
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			mUIDisplaySwipeEventRegion.SetOnSwipeListener(OnSwipeListener);
			Instance = this;
			_isControllDone = false;
			KeyController = new KeyControl();
			UpdateSupplyManager();
			_deckName.supportEncoding = false;
			SupplyManager.InitForOther();
			SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(SupplyManager);
			_decks = SupplyManager.MapArea.GetDecks();
			yield return new WaitForEndOfFrame();
			_shipBannerContainer.Init();
			_otherListParent.Initialize(KeyController, SupplyManager.Ships);
			_rightPane.Init();
			_commonDeckSwitchManager.Init(SupplyManager, (from x in _decks
				where !x.HasBling()
				select x).ToArray(), this, KeyController, otherEnabled: true, SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			yield return StartCoroutine(Util.WaitEndOfFrames(3));
			if (SingletonMonoBehaviour<PortObjectManager>.Instance != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
				{
					SoundUtils.PlaySceneBGM(BGMFileInfos.PortTools);
				});
			}
		}

		public bool isNowDeckIsOther()
		{
			return _commonDeckSwitchManager.currentDeck == null;
		}

		public bool isNowRightFocus()
		{
			return _status == ScreenStatus.RIGHT_PAIN_WORK;
		}

		private void OnSwipeListener(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if (actionType != UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				return;
			}
			float num = 0.1f;
			if (!(num < Math.Abs(movePercentageX)))
			{
				return;
			}
			if (0f < movePercentageX)
			{
				if (_status == ScreenStatus.SHIP_SELECT)
				{
					_commonDeckSwitchManager.ChangePrevDeck();
				}
			}
			else if (_status == ScreenStatus.SHIP_SELECT)
			{
				_commonDeckSwitchManager.ChangeNextDeck();
			}
		}

		public void UpdateSupplyManager()
		{
			SupplyManager = new SupplyManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
		}

		public KeyControl GetKeyController()
		{
			if (KeyController != null)
			{
				KeyController.Update();
			}
			return KeyController;
		}

		public void SetControllDone(bool enabled)
		{
			_isControllDone = enabled;
		}

		public void Update()
		{
			if (GetKeyController() == null || _isControllDone)
			{
				return;
			}
			if (KeyController.IsUpDown())
			{
				switch (_status)
				{
				case ScreenStatus.SHIP_SELECT:
					_shipBannerContainer.SelectLengthwise(isUp: true);
					break;
				case ScreenStatus.RIGHT_PAIN_WORK:
					_rightPane.SelectButtonLengthwise(isUp: true);
					break;
				}
			}
			if (KeyController.IsDownDown())
			{
				switch (_status)
				{
				case ScreenStatus.SHIP_SELECT:
					_shipBannerContainer.SelectLengthwise(isUp: false);
					break;
				case ScreenStatus.RIGHT_PAIN_WORK:
					_rightPane.SelectButtonLengthwise(isUp: false);
					break;
				}
			}
			if (KeyController.IsLeftDown())
			{
				ScreenStatus status = _status;
				if (status != 0 && status == ScreenStatus.RIGHT_PAIN_WORK)
				{
					_rightPane.SelectButtonHorizontal(isLeft: true);
				}
			}
			if (KeyController.IsRightDown())
			{
				switch (_status)
				{
				case ScreenStatus.SHIP_SELECT:
					if (change_2_RIGHT_PAIN_WORK(defaultFocus: true))
					{
						_otherListParent.LockControl();
					}
					break;
				case ScreenStatus.RIGHT_PAIN_WORK:
					_rightPane.SelectButtonHorizontal(isLeft: false);
					break;
				}
			}
			else if (KeyController.IsMaruDown())
			{
				switch (_status)
				{
				case ScreenStatus.SHIP_SELECT:
					_shipBannerContainer.SwitchCurrentSelected();
					break;
				case ScreenStatus.RIGHT_PAIN_WORK:
					_rightPane.DisideButton();
					break;
				}
			}
			else if (KeyController.IsBatuDown())
			{
				switch (_status)
				{
				case ScreenStatus.SHIP_SELECT:
					backPortTop();
					break;
				case ScreenStatus.RIGHT_PAIN_WORK:
					change_2_SHIP_SELECT(defaultFocus: true);
					break;
				}
			}
			else if (KeyController.IsShikakuDown())
			{
				switch (_status)
				{
				case ScreenStatus.SHIP_SELECT:
					if (isNotOther())
					{
						_shipBannerContainer.SwitchAllSelected();
						if (change_2_RIGHT_PAIN_WORK(defaultFocus: true))
						{
							_otherListParent.LockControl();
						}
					}
					break;
				case ScreenStatus.RIGHT_PAIN_WORK:
					if (isNotOther())
					{
						change_2_SHIP_SELECT(defaultFocus: false);
						_shipBannerContainer.SwitchAllSelected();
					}
					break;
				}
			}
			else if (KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
		}

		private bool change_2_RIGHT_PAIN_WORK(bool defaultFocus)
		{
			if (_status != 0)
			{
				return false;
			}
			if (!_rightPane.IsFocusable())
			{
				return false;
			}
			SetShipSelectFocus(focused: false);
			if (defaultFocus)
			{
				_rightPane.setFocus();
			}
			_status = ScreenStatus.RIGHT_PAIN_WORK;
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			return true;
		}

		public void change_2_SHIP_SELECT(bool defaultFocus)
		{
			if (_status == ScreenStatus.RIGHT_PAIN_WORK || _status == ScreenStatus.SHIP_RECOVERY_ANIMATION)
			{
				_status = ScreenStatus.SHIP_SELECT;
				SetShipSelectFocus(focused: true);
				_rightPane.LostFocus();
				if (!isNotOther())
				{
					_otherListParent.StartControl();
				}
			}
		}

		public void change_2_SUPPLY_PROCESS(SupplyType supplyType)
		{
			if (_status != 0 && _status != ScreenStatus.RIGHT_PAIN_WORK)
			{
				return;
			}
			_status = ScreenStatus.SUPPLY_PROCESS;
			_otherListParent.LockControl();
			_commonDeckSwitchManager.keyControlEnable = false;
			if (supplyType == SupplyType.All)
			{
				ShipModel model;
				if (isNotOther() && _currentDeck.GetShipCount() == getSelectedShipList().Count)
				{
					int mstId = _currentDeck.GetFlagShip().MstId;
					model = _currentDeck.GetFlagShip();
				}
				else
				{
					List<ShipModel> selectedShipList = getSelectedShipList();
					int index = UnityEngine.Random.Range(0, selectedShipList.Count);
					int mstId2 = selectedShipList[index].MstId;
					model = selectedShipList[index];
				}
				ShipUtils.PlayShipVoice(model, 27);
			}
			SetShipSelectFocus(focused: false);
			_rightPane.LostFocus();
			_rightPane.SetEnabled(enabled: false);
			_rightPane.DoWindowOpenAnimation(supplyType);
			Supply(supplyType);
		}

		public void change_2_SHIP_RECOVERY_ANIMATION()
		{
			if (_status == ScreenStatus.SUPPLY_PROCESS)
			{
				_status = ScreenStatus.SHIP_RECOVERY_ANIMATION;
				if (isNotOther())
				{
					_shipBannerContainer.ProcessRecoveryAnimation();
				}
				else
				{
					_otherListParent.ProcessRecoveryAnimation();
				}
				_rightPane.DoWindowCloseAnimation();
			}
		}

		public void ProcessSupplyFinished()
		{
			if (_status == ScreenStatus.SHIP_RECOVERY_ANIMATION)
			{
				_rightPane.SetEnabled(enabled: true);
				change_2_SHIP_SELECT(defaultFocus: true);
				OnDeckChange(_currentDeck);
				if (!isNotOther() && _otherListParent.GetShipCount() == 0)
				{
					_commonDeckSwitchManager.Init(SupplyManager, _decks, this, KeyController, otherEnabled: true);
				}
				_commonDeckSwitchManager.keyControlEnable = true;
			}
		}

		public bool IsShipSelectableStatus()
		{
			return _status == ScreenStatus.SHIP_SELECT || _status == ScreenStatus.RIGHT_PAIN_WORK;
		}

		private void backPortTop()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void OnDeckChange(DeckModel deck)
		{
			_currentDeck = deck;
			if (_currentDeck != null && _currentDeck.IsActionEnd())
			{
				_mTrans_TurnEndStamp.SetActive(isActive: true);
				_mTrans_TurnEndStamp.DOKill();
				_mTrans_TurnEndStamp.DOLocalRotate(new Vector3(0f, 0f, 300f), 0f, RotateMode.FastBeyond360);
				_mTrans_TurnEndStamp.DOLocalRotate(new Vector3(0f, 0f, 360f), 0.8f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce);
			}
			else
			{
				_mTrans_TurnEndStamp.SetActive(isActive: false);
			}
			if (isNotOther())
			{
				mShipSortButton.SetActive(isActive: false);
				SupplyManager.InitForDeck(_currentDeck.Id);
				_deckName.text = deck.Name;
				Transform transform = _mTrans_TurnEndStamp.transform;
				Vector3 localPosition = _deckName.transform.localPosition;
				float x = localPosition.x;
				Vector2 printedSize = _deckName.printedSize;
				transform.localPositionX(x + printedSize.x + 20f);
				_shipBannerContainer.Show(animation: true);
				_shipBannerContainer.InitDeck(deck);
				_otherListParent.Hide();
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = _currentDeck;
			}
			else
			{
				mShipSortButton.SetActive(isActive: true);
				SupplyManager.InitForOther();
				_deckName.text = "その他";
				_shipBannerContainer.Hide(animation: true);
				_otherListParent.MemoryNextFocus();
				_otherListParent.Initialize(KeyController, SupplyManager.Ships);
				_otherListParent.Show();
				_otherListParent.StartState();
			}
			UpdateRightPain();
			if (_status == ScreenStatus.RIGHT_PAIN_WORK)
			{
				change_2_SHIP_SELECT(defaultFocus: true);
			}
		}

		private List<ShipModel> getSelectedShipList()
		{
			return (!isNotOther()) ? _otherListParent.getSeletedModelList() : _shipBannerContainer.getSeletedModelList();
		}

		public void UpdateRightPain()
		{
			_rightPane.Refresh();
		}

		private void SetShipSelectFocus(bool focused)
		{
			if (isNotOther())
			{
				_shipBannerContainer.SetFocus(focused);
			}
		}

		private bool isNotOther()
		{
			return (_currentDeck != null) ? true : false;
		}

		private void Supply(SupplyType supplyType)
		{
			if (SupplyManager.Supply(supplyType, out bool use_baux))
			{
				if (use_baux)
				{
					animateBauxite(_bauxiteMsgSuccess);
				}
			}
			else if (use_baux)
			{
				animateBauxite(_bauxiteMsgIncomplete);
			}
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(SupplyManager);
			Instance.UpdateSupplyManager();
		}

		public bool IsDeckSelectable(int index, DeckModel deck)
		{
			if (deck == null)
			{
				return _otherListParent.GetShipCount() > 0;
			}
			return deck.GetShipCount() > 0;
		}

		private void animateBauxite(UITexture texture)
		{
			texture.GetComponent<Animation>().Play("SupplyBauxiteMessage");
		}

		private void OnDestroy()
		{
			for (int i = 0; i < mPreloads_Texture.Length; i++)
			{
				mPreloads_Texture[i] = null;
			}
			mPreloads_Texture = null;
			Instance = null;
			SupplyManager = null;
			KeyController = null;
			_shipBannerContainer = null;
			_otherListParent = null;
			_deckName = null;
			_bauxiteMsgSuccess = null;
			_bauxiteMsgIncomplete = null;
			_mTrans_TurnEndStamp = null;
			_rightPane = null;
			_commonDeckSwitchManager = null;
			_decks = null;
			_currentDeck = null;
		}
	}
}
