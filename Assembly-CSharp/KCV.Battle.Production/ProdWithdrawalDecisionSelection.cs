using KCV.Battle.Utils;
using KCV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdWithdrawalDecisionSelection : BaseAnimation
	{
		public enum Mode
		{
			Selection,
			TacticalSituation
		}

		[SerializeField]
		private Transform _prefabUITacticalSituation;

		[SerializeField]
		private List<UIWithdrawalButton> _listHexExBtns;

		private UITacticalSituation _uiTacticalSituation;

		private UIPanel _uiPanel;

		private Mode _iMode;

		private WithdrawalDecisionType _iSelectType;

		private bool _isDecide;

		private bool _isInputPossible;

		private DelDecideHexButtonEx _delDecideWithdrawalButton;

		private StatementMachine _clsState;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public bool isDecide => _isDecide;

		public bool isInputPossible => _isInputPossible;

		public static ProdWithdrawalDecisionSelection Instantiate(ProdWithdrawalDecisionSelection prefab, Transform parent)
		{
			ProdWithdrawalDecisionSelection prodWithdrawalDecisionSelection = UnityEngine.Object.Instantiate(prefab);
			prodWithdrawalDecisionSelection.transform.parent = parent;
			prodWithdrawalDecisionSelection.transform.localScaleZero();
			prodWithdrawalDecisionSelection.transform.localPositionZero();
			prodWithdrawalDecisionSelection.Init();
			return prodWithdrawalDecisionSelection;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _prefabUITacticalSituation);
			Mem.DelListSafe(ref _listHexExBtns);
			Mem.DelComponentSafe(ref _uiTacticalSituation);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _iMode);
			Mem.Del(ref _iSelectType);
			Mem.Del(ref _isDecide);
			Mem.Del(ref _isInputPossible);
			Mem.Del(ref _delDecideWithdrawalButton);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
		}

		public override bool Init()
		{
			base.Init();
			_iMode = Mode.Selection;
			_clsState = new StatementMachine();
			_isDecide = false;
			_isInputPossible = false;
			_delDecideWithdrawalButton = null;
			InitObjects();
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInWithdrawalDecision(_iMode);
			return true;
		}

		private bool InitObjects()
		{
			int cnt = 0;
			_listHexExBtns.ForEach(delegate(UIWithdrawalButton x)
			{
				x.Init(cnt, isColliderEnabled: false, 20, (Action)delegate
				{
					OnDeside((WithdrawalDecisionType)x.index);
				});
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", (WithdrawalDecisionType)x.index);
				cnt++;
			});
			return true;
		}

		public void Play(Action forceCallback, DelDecideHexButtonEx decideCallback)
		{
			base.Init();
			_actForceCallback = forceCallback;
			_delDecideWithdrawalButton = decideCallback;
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
			{
				ProdWithdrawalDecisionSelection prodWithdrawalDecisionSelection = this;
				BattleShutter shutter = BattleTaskManager.GetPrefabFile().battleShutter;
				shutter.ReqMode(BaseShutter.ShutterMode.Close, delegate
				{
					prodWithdrawalDecisionSelection.transform.localScaleOne();
					Observable.FromCoroutine(prodWithdrawalDecisionSelection.PlayForceCallback).Subscribe(delegate
					{
						shutter.ReqMode(BaseShutter.ShutterMode.Open, delegate
						{
						});
					});
				});
			});
		}

		public bool Run()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return false;
		}

		private IEnumerator PlayForceCallback()
		{
			UIBattleNavigation uibn = BattleTaskManager.GetPrefabFile().battleNavigation;
			uibn.SetNavigationInWithdrawalDecision(_iMode);
			Dlg.Call(ref _actForceCallback);
			_uiTacticalSituation = UITacticalSituation.Instantiate(((Component)_prefabUITacticalSituation).GetComponent<UITacticalSituation>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, BattleTaskManager.GetBattleManager());
			_uiTacticalSituation.Init(OnTacticalSituationBack);
			_uiTacticalSituation.panel.depth = panel.depth + 1;
			yield return StartCoroutine(BattleUtils.ClearMemory());
			_listHexExBtns.ForEach(delegate(UIWithdrawalButton x)
			{
				var _003CPlayForceCallback_003Ec__IteratorF = this;
				x.SetActive(isActive: true);
				x.Play(delegate
				{
					x.isFocus = ((x.index == 0) ? true : false);
					x.isColliderEnabled = true;
					if (x.index == 0)
					{
						_isInputPossible = true;
                        _clsState.AddState(InitWithdrawalSelection, UpdateWithdrawalSelection);
                        uibn.Show();
					}
				});
			});
			yield return null;
		}

		private bool InitWithdrawalSelection(object data)
		{
			_iMode = Mode.Selection;
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInWithdrawalDecision(_iMode);
			return false;
		}

		private bool UpdateWithdrawalSelection(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (!_isDecide && _isInputPossible)
			{
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					PreparaNext(isFoward: false);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					PreparaNext(isFoward: true);
				}
				else
				{
					if (keyControl.GetDown(KeyControl.KeyName.MARU))
					{
						_listHexExBtns[(int)_iSelectType].OnDecide();
						return true;
					}
					if (keyControl.GetDown(KeyControl.KeyName.SANKAKU))
					{
						return OnReqModeTacticalSituation();
					}
				}
			}
			return false;
		}

		private void PreparaNext(bool isFoward)
		{
			WithdrawalDecisionType iSelectType = _iSelectType;
			_iSelectType = (WithdrawalDecisionType)Mathe.NextElement((int)_iSelectType, 0, _listHexExBtns.Count - 1, isFoward);
			if (iSelectType != _iSelectType)
			{
				ChangeFocus(_iSelectType);
			}
		}

		private void ChangeFocus(WithdrawalDecisionType iType)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			_listHexExBtns.ForEach(delegate(UIWithdrawalButton x)
			{
				x.isFocus = ((x.index == (int)iType) ? true : false);
			});
		}

		private bool OnReqModeTacticalSituation()
		{
			_clsState.AddState(InitTacticalSituation, UpdateTacticalSituation);
			return true;
		}

		private bool InitTacticalSituation(object data)
		{
			_iMode = Mode.TacticalSituation;
			_uiTacticalSituation.Show(delegate
			{
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.SetNavigationInWithdrawalDecision(_iMode);
			});
			return false;
		}

		private bool UpdateTacticalSituation(object data)
		{
			if (_uiTacticalSituation != null)
			{
				_uiTacticalSituation.Run();
			}
			return false;
		}

		private void OnTacticalSituationBack()
		{
			_clsState.AddState(InitWithdrawalSelection, UpdateWithdrawalSelection);
		}

		private void OnActive(WithdrawalDecisionType iType)
		{
			if (_iSelectType != iType)
			{
				_iSelectType = iType;
				ChangeFocus(_iSelectType);
			}
		}

		private void OnDeside(WithdrawalDecisionType iType)
		{
			if (!_isDecide)
			{
				_isDecide = true;
				_isInputPossible = false;
				_listHexExBtns.ForEach(delegate(UIWithdrawalButton x)
				{
					x.toggle.enabled = false;
				});
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_036);
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.Hide(0.3f, null);
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
				{
					if (_delDecideWithdrawalButton != null)
					{
						_delDecideWithdrawalButton(_listHexExBtns[(int)iType]);
					}
				});
			}
		}
	}
}
