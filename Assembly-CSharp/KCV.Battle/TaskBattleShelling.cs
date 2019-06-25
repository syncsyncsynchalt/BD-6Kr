using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleShelling : BaseBattleTask
	{
		private ProdShellingFormationJudge _prodShellingFormationJudge;

		private ProdShellingAttack _prodShellingAttack;

		private ProdShellingTorpedo _prodShellingTorpedo;

		private HougekiListModel _clsNowHougekiList;

		private RaigekiModel _clsNowRaigeki;

		private List<CmdActionPhaseModel> _listCmdActionList;

		private int _nCurrentShellingCnt;

		private bool _isFriendActionExit;

		private bool _isEnemyActionExit;

		private Action _actOnFleetAction;

		private int shellingCnt
		{
			get
			{
				if (_clsNowHougekiList == null)
				{
					return -1;
				}
				return _clsNowHougekiList.Count;
			}
		}

		private bool isNextAttack
		{
			get
			{
				if (shellingCnt == _nCurrentShellingCnt)
				{
					return false;
				}
				return true;
			}
		}

		private CmdActionPhaseModel currentCmdActionPhase => _listCmdActionList[_nCurrentShellingCnt];

		protected override bool Init()
		{
			if (!BattleTaskManager.GetBattleManager().IsExistHougekiPhase_Day())
			{
				ImmediateTermination();
				EndPhase(BattleUtils.NextPhase(BattlePhase.Shelling));
			}
			else
			{
				_listCmdActionList = BattleTaskManager.GetBattleManager().GetHougekiData_Day();
				_nCurrentShellingCnt = 0;
				_actOnFleetAction = null;
				_prodShellingFormationJudge = ProdShellingFormationJudge.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdShellingFormationJudge).GetComponent<ProdShellingFormationJudge>(), BattleTaskManager.GetBattleManager(), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
				_prodShellingAttack = new ProdShellingAttack();
				_clsState = new StatementMachine();
				_clsState.AddState(InitFormationJudge, UpdateFormationJudge);
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			_prodShellingFormationJudge = null;
			if (_prodShellingAttack != null)
			{
				_prodShellingAttack.Dispose();
			}
			_prodShellingAttack = null;
			_clsNowHougekiList = null;
			Mem.DelIDisposableSafe(ref _prodShellingTorpedo);
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.Shelling);
		}

		private bool InitFormationJudge(object data)
		{
			_prodShellingFormationJudge.Play(delegate
			{
				BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
				BattleTaskManager.GetBattleShips().SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
				BattleTaskManager.GetBattleShips().RadarDeployment(isDeploy: false);
			}, OnFormationJudgeFinished);
			return false;
		}

		private bool UpdateFormationJudge(object data)
		{
			return true;
		}

		private void OnFormationJudgeFinished()
		{
			_clsState.AddState(InitCommandBuffer, UpdateCommandBuffer);
		}

		private bool InitCommandBuffer(object data)
		{
			if (_nCurrentShellingCnt == _listCmdActionList.Count)
			{
				OnShellingPhaseFinished();
				return false;
			}
			_isFriendActionExit = false;
			_isEnemyActionExit = false;
			EffectModel effectModel = BattleTaskManager.GetBattleManager().GetEffectData(_nCurrentShellingCnt);
			if (effectModel != null)
			{
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer = ProdBattleCommandBuffer.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdBattleCommandBuffer).GetComponent<ProdBattleCommandBuffer>(), BattleTaskManager.GetStage(), effectModel, _nCurrentShellingCnt);
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer.Play(delegate
				{
					if (effectModel.Withdrawal)
					{
						BattleTaskManager.ReqPhase(BattlePhase.WithdrawalDecision);
					}
					else
					{
						CheckNextAction();
					}
				});
			}
			else
			{
				CheckNextAction();
			}
			return false;
		}

		private bool UpdateCommandBuffer(object data)
		{
			return true;
		}

		private void CheckNextAction()
		{
			if ((_isFriendActionExit && _isEnemyActionExit) || currentCmdActionPhase == null)
			{
				_nCurrentShellingCnt++;
				_clsState.AddState(InitCommandBuffer, UpdateCommandBuffer);
				return;
			}
			_actOnFleetAction = null;
			_clsNowHougekiList = null;
			_clsNowRaigeki = null;
			if (!_isFriendActionExit)
			{
				if (currentCmdActionPhase.Action_f != null)
				{
					if (currentCmdActionPhase.Action_f is HougekiListModel)
					{
						_actOnFleetAction = CheckNextAction;
						_clsNowHougekiList = (currentCmdActionPhase.Action_f as HougekiListModel);
						_clsState.AddState(InitShelling, UpdateShelling);
					}
					else
					{
						_actOnFleetAction = CheckNextAction;
						_clsNowRaigeki = (currentCmdActionPhase.Action_f as RaigekiModel);
						_clsState.AddState(InitTorpedo, UpdateTorpedo);
					}
					_isFriendActionExit = true;
				}
				else
				{
					_isFriendActionExit = true;
					CheckNextAction();
				}
			}
			else
			{
				if (_isEnemyActionExit)
				{
					return;
				}
				if (currentCmdActionPhase.Action_e != null)
				{
					if (currentCmdActionPhase.Action_e is HougekiListModel)
					{
						_actOnFleetAction = CheckNextAction;
						_clsNowHougekiList = (currentCmdActionPhase.Action_e as HougekiListModel);
						_clsState.AddState(InitShelling, UpdateShelling);
					}
					else
					{
						_actOnFleetAction = CheckNextAction;
						_clsNowRaigeki = (currentCmdActionPhase.Action_e as RaigekiModel);
						_clsState.AddState(InitTorpedo, UpdateTorpedo);
					}
					_isEnemyActionExit = true;
				}
				else
				{
					_isEnemyActionExit = true;
					CheckNextAction();
				}
			}
		}

		protected bool InitShelling(object data)
		{
			HougekiModel nextData = _clsNowHougekiList.GetNextData();
			if (nextData == null)
			{
				Dlg.Call(ref _actOnFleetAction);
			}
			else
			{
				_prodShellingAttack.Play(nextData, _nCurrentShellingCnt, isNextAttack, null);
				BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
			}
			return false;
		}

		protected bool UpdateShelling(object data)
		{
			if (_prodShellingAttack.isFinished)
			{
				_prodShellingAttack.Clear();
				_clsState.AddState(InitShelling, UpdateShelling);
				return true;
			}
			if (_prodShellingAttack != null)
			{
				_prodShellingAttack.Update();
			}
			return false;
		}

		protected void OnShellingFinished()
		{
			_clsState.AddState(InitCommandBuffer, UpdateCommandBuffer);
		}

		private bool InitTorpedo(object data)
		{
			_prodShellingTorpedo = new ProdShellingTorpedo(_clsNowRaigeki);
			_prodShellingTorpedo.Play(delegate
			{
				OnTorpedoTerminate();
			});
			return false;
		}

		private bool UpdateTorpedo(object data)
		{
			if (_prodShellingTorpedo != null)
			{
				_prodShellingTorpedo.Update();
			}
			return false;
		}

		private void OnTorpedoTerminate()
		{
			_clsState.Clear();
			PlayProdDamage(_clsNowRaigeki, delegate
			{
				UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
				circleHPGauge.transform.localScaleZero();
				Mem.DelIDisposableSafe(ref _prodShellingTorpedo);
				Dlg.Call(ref _actOnFleetAction);
			});
		}

		private void OnShellingPhaseFinished()
		{
			EndPhase(BattleUtils.NextPhase(BattlePhase.Shelling));
		}
	}
}
