using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAircraftAttack : BaseProdAttackShelling
	{
		protected override bool InitAttackerFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras[0];
			List<Vector3> camTargetPos = CalcCloseUpCamPos(fieldCam.transform.position, CalcCamTargetPos(isAttacker: true, isPointOfGaze: false), isProtect: false);
			base.alterWaveDirection = _listBattleShips[0].fleetType;
			GraAddDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_936);
			fieldCam.transform.LTMove(camTargetPos[0], BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE[0]).setOnComplete((Action)delegate
			{
				playShellingSlot(_clsHougekiModel.GetSlotitem(), BaseProdLine.AnimationName.ProdAircraftAttackLine, _listBattleShips[0].shipModel.IsFriend(), 0.033f);
				PlayShipAnimation(_listBattleShips[0], UIBattleShip.AnimationName.ProdShellingNormalAttack, 0.4f);
				fieldCam.motionBlur.enabled = false;
				fieldCam.transform.LTMove(camTargetPos[1], 1.2f).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE[1]).setOnComplete((Action)delegate
				{
					ChkAttackCntForNextPhase();
				});
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
			fieldCam.transform.LTMove(camTargetPos[0], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0]).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE[0]).setOnComplete((Action)delegate
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

		protected virtual void playShellingSlot(SlotitemModel_Battle model, BaseProdLine.AnimationName iName, bool isFriend, float delay)
		{
			if (model != null)
			{
				Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(delegate
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_048);
					ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
					prodShellingSlotLine.SetSlotData(model, isFriend);
					prodShellingSlotLine.Play(iName, isFriend, null);
				});
			}
		}
	}
}
