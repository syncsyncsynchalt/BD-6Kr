using KCV.Battle.Utils;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAntiGroundAttack : BaseProdAttackShelling
	{
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
				PlayShipAnimation(_listBattleShips[0], UIBattleShip.AnimationName.ProdShellingNormalAttack, 0.4f);
				fieldCam.motionBlur.enabled = false;
				fieldCam.transform.LTMove(camTargetPos[1], BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[1]).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE[1]).setOnComplete((Action)delegate
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

		protected override void PlayDefenderCritical(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.CriticalDamage, isTorpedo: false);
			ParticleSystem explosionAntiGround = BattleTaskManager.GetParticleFile().explosionAntiGround;
			((Component)explosionAntiGround).transform.parent = base.particleParent;
			((Component)explosionAntiGround).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)explosionAntiGround).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 0.9f);
			((Component)explosionAntiGround).SetActive(isActive: true);
			explosionAntiGround.Play();
			PlayHpGaugeDamage(ship, base.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
			PlayDamageVoice(ship, _clsHougekiModel.Defender.DamageEventAfter);
		}

		protected override void PlayDefenderNormal(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem explosionAntiGround = BattleTaskManager.GetParticleFile().explosionAntiGround;
			((Component)explosionAntiGround).transform.parent = base.particleParent;
			((Component)explosionAntiGround).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)explosionAntiGround).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 0.9f);
			((Component)explosionAntiGround).SetActive(isActive: true);
			explosionAntiGround.Play();
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.NomalDamage, isTorpedo: false);
			PlayDamageVoice(ship, _clsHougekiModel.Defender.DamageEventAfter);
			PlayHpGaugeDamage(ship, base.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected override void PlayDefenderGard(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem explosionB3WhiteSmoke = BattleTaskManager.GetParticleFile().explosionB3WhiteSmoke;
			((Component)explosionB3WhiteSmoke).transform.parent = base.particleParent;
			((Component)explosionB3WhiteSmoke).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)explosionB3WhiteSmoke).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 0.9f);
			((Component)explosionB3WhiteSmoke).SetActive(isActive: true);
			explosionB3WhiteSmoke.Play();
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.Gard, isTorpedo: false);
			PlayDamageVoice(ship, _clsHougekiModel.Defender.DamageEventAfter);
			PlayHpGaugeDamage(ship, base.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected override void PlayDefenderMiss(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.Miss, isTorpedo: false);
			ParticleSystem splashMiss = BattleTaskManager.GetParticleFile().splashMiss;
			((Component)splashMiss).transform.parent = base.particleParent;
			((Component)splashMiss).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)splashMiss).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 5f);
			((Component)splashMiss).transform.positionY();
			((Component)splashMiss).SetActive(isActive: true);
			splashMiss.Play();
			PlayHpGaugeDamage(ship, base.hitState);
		}
	}
}
