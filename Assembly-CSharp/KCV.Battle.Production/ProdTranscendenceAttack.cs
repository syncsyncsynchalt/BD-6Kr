using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTranscendenceAttack : BaseProdAttackShelling
	{
		private ProdTranscendenceCutIn _prodTranscendenceCutIn;

		public ProdTranscendenceAttack()
		{
			_prodTranscendenceCutIn = ProdTranscendenceCutIn.Instantiate(PrefabFile.Load<ProdTranscendenceCutIn>(PrefabFileInfos.BattleProdTranscendenceCutIn), BattleTaskManager.GetBattleCameras().cutInEffectCamera.transform);
		}

		protected override void OnDispose()
		{
			if (_prodTranscendenceCutIn != null)
			{
				UnityEngine.Object.Destroy(_prodTranscendenceCutIn.gameObject);
			}
			_prodTranscendenceCutIn = null;
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
					BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					cutInEffectCamera.isCulling = false;
					_prodTranscendenceCutIn.transform.localScaleZero();
				});
				_prodTranscendenceCutIn.SetShellingData(_clsHougekiModel);
				_prodTranscendenceCutIn.Play(delegate
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

		private ProdTranscendenceCutIn.AnimationList getTranscendenceAttackAnimation(BattleAttackKind iKind)
		{
			switch (iKind)
			{
			case BattleAttackKind.Syu_Syu_Syu:
				return ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3;
			case BattleAttackKind.Rai_Rai:
				return ProdTranscendenceCutIn.AnimationList.ProdTATorpedox2;
			default:
				return ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3;
			}
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
