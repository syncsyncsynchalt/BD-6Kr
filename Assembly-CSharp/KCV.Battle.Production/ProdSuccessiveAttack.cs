using KCV.Battle.Utils;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSuccessiveAttack : BaseProdAttackShelling
	{
		protected override bool InitAttackerFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras[0];
			SetDimCamera(isAttacker: true, fieldCam.transform);
			SetFieldCamera(isAttacker: true, CalcCamPos(isAttacker: true, isPointOfGaze: false), _listBattleShips[0].spPointOfGaze);
			List<Vector3> camTargetPos = CalcCloseUpCamPos(fieldCam.transform.position, CalcCamTargetPos(isAttacker: true, isPointOfGaze: false), isProtect: false);
			base.alterWaveDirection = _listBattleShips[0].fleetType;
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_936);
			GraAddDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]);
			fieldCam.transform.LTMove(camTargetPos[0], BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]).setEase(LeanTweenType.easeInQuad).setOnComplete((Action)delegate
			{
				PlayShellingSlot(_clsHougekiModel.GetSlotitems(), BaseProdLine.AnimationName.ProdSuccessiveLine, _listBattleShips[0].shipModel.IsFriend(), 0.033f);
				PlayShipAnimation(_listBattleShips[0], UIBattleShip.AnimationName.ProdShellingNormalAttack, 0.4f);
				fieldCam.motionBlur.enabled = false;
				fieldCam.transform.LTMove(camTargetPos[1], BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[1]).setEase(LeanTweenType.linear).setOnComplete((Action)delegate
				{
					ChkAttackCntForNextPhase();
				});
			});
			return false;
		}

        protected override bool InitRotateFocus(object data)
        {
            BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
            BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
            battleFieldCamera.motionBlur.enabled = false;
            this.GraSubDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]);
            this.RotateFocusTowardsTarget2RotateFieldCam(this._listBattleShips[1].spPointOfGaze);
            this.RotateFocusTowardTarget2MoveFieldCam(this._listBattleShips[1].spPointOfGaze, delegate
            {
                this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitDefenderFocus), new StatementMachine.StatementMachineUpdate(this.UpdateDefenderFocus));
            });
            return false;
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
			if (base.isSkipAttack)
			{
				base.alterWaveDirection = _listBattleShips[1].fleetType;
			}
			GraAddDimCameraMaskAlpha((!_isSkipAttack) ? BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0] : 0f);
			fieldCam.transform.LTMove(camTargetPos[0], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0]).setEase(LeanTweenType.easeInQuad).setOnComplete((Action)delegate
			{
				fieldCam.motionBlur.enabled = false;
				if (base.isProtect)
				{
					PlayProtectDefender(camTargetPos);
				}
				else
				{
					PlayDefenderEffect(_listBattleShips[1], _listBattleShips[1].pointOfGaze, fieldCam, 0.3f);
					ChkDamagedStateFmAnticipating(camTargetPos[1]);
				}
			});
			return false;
		}
	}
}
