using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleWithdrawalDecision : BaseBattleTask
	{
		private bool _isNightCombat;

		private Dictionary<FleetType, Vector3> _dicSplitCameraPos;

		private Dictionary<FleetType, Quaternion> _dicSplitCameraRot;

		private ProdWithdrawalDecisionSelection _prodWithdrawalDecisionSelection;

		protected override bool Init()
		{
			_isNightCombat = BattleTaskManager.GetBattleManager().HasNightBattle();
			if (!_isNightCombat)
			{
				EndPhase(BattlePhase.Result);
				return true;
			}
			_clsState = new StatementMachine();
			float num = 72f;
			_dicSplitCameraPos = new Dictionary<FleetType, Vector3>();
			_dicSplitCameraPos.Add(FleetType.Friend, new Vector3(0f, 4f, num));
			_dicSplitCameraPos.Add(FleetType.Enemy, new Vector3(0f, 4f, 0f - num));
			_dicSplitCameraRot = new Dictionary<FleetType, Quaternion>();
			_dicSplitCameraRot.Add(FleetType.Friend, Quaternion.Euler(Vector3.zero));
			_dicSplitCameraRot.Add(FleetType.Enemy, Quaternion.Euler(Vector3.up * 180f));
			_prodWithdrawalDecisionSelection = ProdWithdrawalDecisionSelection.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdWithdrawalDecisionSelection).GetComponent<ProdWithdrawalDecisionSelection>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
			_clsState.AddState(InitWithdrawalSelection, UpdateWithdrawalSelection);
			return true;
		}

		protected override bool UnInit()
		{
			_isNightCombat = false;
			if (_dicSplitCameraPos != null)
			{
				_dicSplitCameraPos.Clear();
			}
			if (_dicSplitCameraRot != null)
			{
				_dicSplitCameraRot.Clear();
			}
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.WithdrawalDecision);
		}

		private bool InitWithdrawalSelection(object data)
		{
			_prodWithdrawalDecisionSelection.Play(delegate
			{
				BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				BattleField battleField = BattleTaskManager.GetBattleField();
				battleField.ResetFleetAnchorPosition();
				battleField.enemySeaLevel.SetActive(isActive: true);
				battleField.ReqTimeZone(KCV.Battle.Utils.TimeZone.Night, BattleTaskManager.GetSkyType());
				battleField.AlterWaveDirection(FleetType.Friend, FleetType.Friend);
				battleField.AlterWaveDirection(FleetType.Enemy, FleetType.Enemy);
				BattleShips battleShips = BattleTaskManager.GetBattleShips();
				battleShips.RadarDeployment(isDeploy: false);
				battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
				battleShips.SetStandingPosition(StandingPositionType.OneRow);
				battleShips.SetLayer(Generics.Layers.ShipGirl);
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				battleCameras.InitEnemyFieldCameraDefault();
				battleCameras.SetVerticalSplitCameras(isSplit: false);
				if (!battleCameras.isSplit)
				{
					battleCameras.SetSplitCameras(isSplit: true);
				}
				battleCameras.ResetFieldCamSettings(FleetType.Friend);
				battleCameras.ResetFieldCamSettings(FleetType.Enemy);
				battleCameras.fieldDimCameraEnabled(isEnabled: false);
				BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.Fix);
				battleFieldCamera.SetFixCamera(_dicSplitCameraPos[FleetType.Friend], _dicSplitCameraRot[FleetType.Friend]);
				battleFieldCamera.cullingMask = battleCameras.GetDefaultLayers();
				battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[1];
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.Fix);
				battleFieldCamera.SetFixCamera(_dicSplitCameraPos[FleetType.Enemy], _dicSplitCameraRot[FleetType.Enemy]);
				battleFieldCamera.cullingMask = battleCameras.GetEnemyCamSplitLayers();
				BattleShips battleShips2 = BattleTaskManager.GetBattleShips();
				battleShips2.SetBollboardTarget(isFriend: true, battleCameras.fieldCameras[0].transform);
				battleShips2.SetBollboardTarget(isFriend: false, battleCameras.fieldCameras[1].transform);
				battleShips2.SetTorpedoSalvoWakeAngle(isSet: false);
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				cutInEffectCamera.isCulling = false;
				UITexture component = ((Component)cutInEffectCamera.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
				if (component != null)
				{
					component.alpha = 0f;
				}
				BattleTaskManager.GetTorpedoHpGauges().SetDestroy();
			}, OnDecideWithdrawalButton);
			return false;
		}

		private bool UpdateWithdrawalSelection(object data)
		{
			if (_prodWithdrawalDecisionSelection != null)
			{
				return _prodWithdrawalDecisionSelection.Run();
			}
			return false;
		}

		private void OnDecideWithdrawalButton(UIHexButtonEx btn)
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				Mem.DelComponentSafe(ref _prodWithdrawalDecisionSelection);
			});
			Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
			{
				if (btn.index == 0)
				{
					EndPhase(BattlePhase.Result);
				}
				else if (!BattleTaskManager.GetIsSameBGM())
				{
					KCV.Utils.SoundUtils.StopFadeBGM(0.2f, delegate
					{
						BattleTaskManager.GetBattleManager().StartDayToNightBattle();
						EndPhase(BattlePhase.NightCombat);
					});
				}
				else
				{
					BattleTaskManager.GetBattleManager().StartDayToNightBattle();
					EndPhase(BattlePhase.NightCombat);
				}
			});
		}
	}
}
