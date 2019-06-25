using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class CtrlRebellionOrganize : MonoBehaviour
	{
		public enum RebellionOrganizeMode
		{
			Main,
			Detail
		}

		public const float STATE_CHANGE_TIME = 0.2f;

		public static readonly LeanTweenType STATE_CHANGE_EASING = LeanTweenType.easeInSine;

		[SerializeField]
		private int _nBaseDepth;

		[SerializeField]
		private Transform _prefabUINavigation;

		[SerializeField]
		private Transform _prefabFleetSelector;

		[SerializeField]
		private Transform _prefabParticipatingFleetSelector;

		[SerializeField]
		private Transform _prefabHeader;

		[SerializeField]
		private UIRebellionFleetShipsList _uiFleetShipsList;

		private bool _isDecide;

		private RebellionOrganizeMode _iMode;

		private UIRebellionNavigation _uiNavigation;

		private UIRebellionFleetSelector _uiFleetSelector;

		private UIRebellionParticipatingFleetSelector _uiParticipatingFleetSelector;

		private UIRebellionHeader _uiHeader;

		private StatementMachine _clsState;

		private Action _actSortieStartCallback;

		private Action _actCalcelCallback;

		public RebellionOrganizeMode mode => _iMode;

		public int baseDepth
		{
			get
			{
				return _nBaseDepth;
			}
			set
			{
				if (_nBaseDepth != value)
				{
					_nBaseDepth = value;
					SortPanelDepth(_nBaseDepth);
				}
			}
		}

		public UIRebellionParticipatingFleetSelector participatingFleetSelector => _uiParticipatingFleetSelector;

		public static CtrlRebellionOrganize Instantiate(CtrlRebellionOrganize prefab, Transform parent, Action sortieStartAction, Action cancelAction)
		{
			CtrlRebellionOrganize ctrlRebellionOrganize = UnityEngine.Object.Instantiate(prefab);
			ctrlRebellionOrganize.transform.parent = parent;
			ctrlRebellionOrganize.transform.localPositionZero();
			ctrlRebellionOrganize.transform.localScaleZero();
			ctrlRebellionOrganize._actSortieStartCallback = sortieStartAction;
			ctrlRebellionOrganize._actCalcelCallback = cancelAction;
			ctrlRebellionOrganize.Setup();
			return ctrlRebellionOrganize;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabUINavigation);
			Mem.Del(ref _prefabFleetSelector);
			Mem.Del(ref _prefabParticipatingFleetSelector);
			Mem.Del(ref _uiNavigation);
			Mem.Del(ref _uiFleetSelector);
			Mem.Del(ref _uiParticipatingFleetSelector);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
			Mem.Del(ref _actSortieStartCallback);
			Mem.Del(ref _actCalcelCallback);
		}

		private bool Setup()
		{
			_isDecide = false;
			_iMode = RebellionOrganizeMode.Main;
			_clsState = new StatementMachine();
			Observable.FromCoroutine(InstantiateObjects).Subscribe(delegate
			{
				Init();
			}).AddTo(base.gameObject);
			return true;
		}

		private IEnumerator InstantiateObjects()
		{
			_uiNavigation = UIRebellionNavigation.Instantiate(((Component)_prefabUINavigation).GetComponent<UIRebellionNavigation>(), base.transform, _iMode);
			yield return null;
			_uiFleetSelector = UIRebellionFleetSelector.Instantiate(((Component)_prefabFleetSelector).GetComponent<UIRebellionFleetSelector>(), base.transform);
			yield return null;
			_uiParticipatingFleetSelector = UIRebellionParticipatingFleetSelector.Instantiate(((Component)_prefabParticipatingFleetSelector).GetComponent<UIRebellionParticipatingFleetSelector>(), base.transform);
			yield return StartCoroutine(_uiParticipatingFleetSelector.InstantiateObjects());
			yield return null;
			_uiHeader = UIRebellionHeader.Instantiate(((Component)_prefabHeader).GetComponent<UIRebellionHeader>(), base.transform);
			yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
		}

		public bool Init()
		{
			SortPanelDepth(_nBaseDepth);
			_uiParticipatingFleetSelector.Init(DecideParticipatingFleetInfo, DecideSortieStart);
			RebellionManager rebellionManager = StrategyTaskManager.GetStrategyRebellion().GetRebellionManager();
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < rebellionManager.Decks.Count; i++)
			{
				if (rebellionManager.Decks[i].IsValidSortie().Count == 0)
				{
					list.Add(rebellionManager.Decks[i]);
				}
			}
			_uiFleetSelector.Init(list, 0, DecideFleetSelector);
			_uiFleetSelector.rouletteSelector.controllable = false;
			KeyControl keycontrol = StrategyTaskManager.GetStrategyRebellion().keycontrol;
			keycontrol.reset();
			keycontrol.setMinMaxIndex(0, Enum.GetValues(typeof(RebellionFleetType)).Length);
			keycontrol.useDoubleIndex(0, _uiFleetSelector.fleetCnt - 1);
			base.transform.localScaleOne();
			Show();
			return true;
		}

		private void SortPanelDepth(int baseDepth)
		{
			_uiFleetSelector.panel.depth = baseDepth;
			_uiParticipatingFleetSelector.panel.depth = _uiFleetSelector.panel.depth + 1;
			_uiFleetShipsList.panel.depth = _uiParticipatingFleetSelector.panel.depth + 1;
			_uiNavigation.panel.depth = _uiFleetShipsList.panel.depth + 1;
			_uiHeader.panel.depth = _uiNavigation.panel.depth + 1;
		}

		private void Show()
		{
			_uiHeader.Show(null);
			_uiNavigation.Show(null);
			_uiParticipatingFleetSelector.Show(delegate
			{
				_clsState.AddState(InitMainProcess, UpdateMainProcess);
			});
		}

		public bool Run()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return true;
		}

		private bool InitMainProcess(object data)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", string.Empty);
			_uiNavigation.SetNavigation(RebellionOrganizeMode.Main);
			_uiFleetSelector.isColliderEnabled = true;
			_uiParticipatingFleetSelector.isColliderEnabled = true;
			return false;
		}

		private bool UpdateMainProcess(object data)
		{
			if (_isDecide)
			{
				return true;
			}
			KeyControl keycontrol = StrategyTaskManager.GetStrategyRebellion().keycontrol;
			if (keycontrol.GetDown(KeyControl.KeyName.UP))
			{
				_uiParticipatingFleetSelector.MovePrev();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.DOWN))
			{
				_uiParticipatingFleetSelector.MoveNext();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.LEFT))
			{
				_uiFleetSelector.rouletteSelector.MovePrev();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.RIGHT))
			{
				_uiFleetSelector.rouletteSelector.MoveNext();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.MARU))
			{
				_uiFleetSelector.rouletteSelector.Determine();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.BATU))
			{
				if (_actCalcelCallback != null)
				{
					_actCalcelCallback();
				}
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.SHIKAKU))
			{
				if (!_uiParticipatingFleetSelector.isSortieStartFocus)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
					_uiParticipatingFleetSelector.SetFleetInfo((RebellionFleetType)_uiParticipatingFleetSelector.nowIndex, null);
					_uiParticipatingFleetSelector.ChkSortieStartState();
				}
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.SANKAKU))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
				DebugUtils.Log("RebellionOrganizeCtrl", $"[{_uiFleetSelector.nowSelectedDeck.Id}({_uiFleetSelector.nowSelectedDeck.Name})]{((_uiFleetSelector.nowSelectedDeck.GetFlagShip() == null) ? string.Empty : _uiFleetSelector.nowSelectedDeck.GetFlagShip().Name)}");
				if (_uiFleetSelector.nowSelectedDeck.GetFlagShip() == null)
				{
					return false;
				}
				_uiParticipatingFleetSelector.isColliderEnabled = false;
				_uiFleetSelector.isColliderEnabled = false;
				_uiFleetSelector.rouletteSelector.controllable = false;
				_uiFleetShipsList.Init(_uiFleetSelector.nowSelectedDeck);
				ChangeState(RebellionOrganizeMode.Detail);
				return true;
			}
			return false;
		}

		private bool InitDetailProcess(object data)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", string.Empty);
			_uiNavigation.SetNavigation(RebellionOrganizeMode.Detail);
			return false;
		}

		private bool UpdateDetailProcess(object data)
		{
			KeyControl keycontrol = StrategyTaskManager.GetStrategyRebellion().keycontrol;
			if (keycontrol.GetDown(KeyControl.KeyName.MARU))
			{
				SetRebellionParticipatingFleet((RebellionFleetType)_uiParticipatingFleetSelector.nowIndex, _uiFleetSelector.nowSelectedDeck);
				ChangeState(RebellionOrganizeMode.Main);
				return true;
			}
			if (keycontrol.GetDown(KeyControl.KeyName.BATU))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				ChangeState(RebellionOrganizeMode.Main);
				return true;
			}
			if (keycontrol.GetDown(KeyControl.KeyName.SANKAKU))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
				ChangeState(RebellionOrganizeMode.Main);
				return true;
			}
			return false;
		}

		private bool SetRebellionParticipatingFleet(RebellionFleetType iType, DeckModel model)
		{
			if (isValidSetDeck(iType, model))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				_uiParticipatingFleetSelector.SetFleetInfo(iType, _uiFleetSelector.nowSelectedDeck);
				_uiParticipatingFleetSelector.ChkSortieStartState();
				return true;
			}
			return false;
		}

		private bool isValidSetDeck(RebellionFleetType iType, DeckModel model)
		{
			DebugUtils.Log("isValidSetDeck::" + iType);
			bool flag = !_uiParticipatingFleetSelector.IsAlreadySetFleet(_uiFleetSelector.nowSelectedDeck) && _uiFleetSelector.nowSelectedDeck.GetFlagShip() != null;
			RebellionManager rebellionManager = StrategyTaskManager.GetStrategyRebellion().GetRebellionManager();
			bool flag2 = false;
			List<IsGoCondition> list = null;
			switch (iType)
			{
			case RebellionFleetType.VanguardSupportFleet:
				list = rebellionManager.IsValidMissionSub(model.Id);
				break;
			case RebellionFleetType.DecisiveBattleSupportFleet:
				list = rebellionManager.IsValid_MissionMain(model.Id);
				break;
			}
			if (list == null || list.Count == 0)
			{
				flag2 = true;
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(list[0]));
			}
			return flag && flag2;
		}

		private void ChangeState(RebellionOrganizeMode iMode)
		{
			_iMode = iMode;
			switch (iMode)
			{
			case RebellionOrganizeMode.Main:
				_uiFleetSelector.ReqMode(RebellionOrganizeMode.Main, 0.2f, STATE_CHANGE_EASING);
				_uiHeader.Show(null);
				_uiParticipatingFleetSelector.Show(null);
				_uiFleetShipsList.Hide(delegate
				{
					_clsState.AddState(InitMainProcess, UpdateMainProcess);
				});
				break;
			case RebellionOrganizeMode.Detail:
				_uiFleetSelector.ReqMode(RebellionOrganizeMode.Detail, 0.2f, STATE_CHANGE_EASING);
				_uiHeader.Hide(null);
				_uiParticipatingFleetSelector.Hide(null);
				_uiFleetShipsList.Show(delegate
				{
					_clsState.AddState(InitDetailProcess, UpdateDetailProcess);
				});
				break;
			}
		}

		private void DecideFleetSelector(DeckModel model)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", $"[{model.Id}({model.Name})]{((model.GetFlagShip() == null) ? string.Empty : model.GetFlagShip().Name)}");
			if (_uiParticipatingFleetSelector.isSortieStartFocus)
			{
				DecideSortieStart();
			}
			else if (!SetRebellionParticipatingFleet((RebellionFleetType)_uiParticipatingFleetSelector.nowIndex, _uiFleetSelector.nowSelectedDeck))
			{
			}
		}

		private void DecideParticipatingFleetInfo(IRebellionOrganizeSelectObject selectObj)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", selectObj.button.name);
		}

		private void DecideSortieStart()
		{
			DebugUtils.Log("RebellionOrganizeCtrl", string.Empty);
			if (!_isDecide)
			{
				_isDecide = true;
				if (_actSortieStartCallback != null)
				{
					_actSortieStartCallback();
				}
			}
		}
	}
}
