using KCV.Battle.Utils;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdObservedShellingAttack : BaseProdAttackShelling
	{
		private ProdObservedShellingCutIn _prodObservedShellingCutIn;

		public ProdObservedShellingAttack()
		{
			_prodObservedShellingCutIn = ProdObservedShellingCutIn.Instantiate(PrefabFile.Load<ProdObservedShellingCutIn>(PrefabFileInfos.BattleProdObservedShellingCutIn), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
		}

		protected override void OnDispose()
		{
			if (_prodObservedShellingCutIn != null)
			{
				UnityEngine.Object.Destroy(_prodObservedShellingCutIn.gameObject);
			}
			_prodObservedShellingCutIn = null;
			base.OnDispose();
		}

		protected override bool InitAttackerFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras[0];
			List<Vector3> camTargetPos = CalcCloseUpCamPos(fieldCam.transform.position, CalcCamTargetPos(isAttacker: true, isPointOfGaze: true), isProtect: false);
			base.alterWaveDirection = _listBattleShips[0].fleetType;
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_936);
			GraAddDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]);
			fieldCam.transform.LTMove(camTargetPos[0], BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE[0]).setOnComplete((Action)delegate
			{
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Register(delegate
				{
					_prodObservedShellingCutIn.transform.localScaleZero();
				});
				_prodObservedShellingCutIn.SetObservedShelling(_clsHougekiModel);
				_prodObservedShellingCutIn.Play(delegate
				{
					ChkAttackCntForNextPhase();
				});
				fieldCam.transform.LTMove(camTargetPos[1], BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[1]).setEase(LeanTweenType.linear);
			});
			return false;
		}

		protected override void OnCameraRotateStart()
		{
			PostProcessCutIn();
		}

		protected override bool InitDefenderFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras[0];
			BattleFieldDimCamera fieldDimCamera = battleCameras.fieldDimCamera;
			fieldDimCamera.SetMaskPlaneAlpha(0f);
			Vector3 calcDefenderCamStartPos = CalcDefenderCamStartPos;
			SetFieldCamera(isAttacker: false, calcDefenderCamStartPos, _listBattleShips[1].spPointOfGaze);
			List<Vector3> camTargetPos = CalcCloseUpCamPos(fieldCam.transform.position, CalcCamTargetPos(isAttacker: false, isPointOfGaze: false), base.isProtect);
			base.alterWaveDirection = _listBattleShips[1].fleetType;
			GraAddDimCameraMaskAlpha((!_isSkipAttack) ? BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0] : 0f);
			fieldCam.transform.LTMove(camTargetPos[0], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0]).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE[0]).setOnStart(delegate
			{
				PostProcessCutIn();
			})
				.setOnComplete((Action)delegate
				{
					fieldCam.motionBlur.enabled = false;
					if (base.isProtect)
					{
						PlayProtectDefender(camTargetPos);
					}
					else
					{
						PlayDefenderEffect(_listBattleShips[1], _listBattleShips[1].pointOfGaze, fieldCam, 0.5f);
						ChkDamagedStateFmAnticipating(camTargetPos[1]);
					}
				});
			return false;
		}

		private void PostProcessCutIn()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			if (observerAction.Count != 0)
			{
				observerAction.Execute();
			}
		}
	}
}
