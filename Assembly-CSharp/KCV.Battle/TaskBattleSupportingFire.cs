using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models.battle;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleSupportingFire : BaseBattleTask
	{
		private IShienModel _clsShien;

		private ShienModel_Hou _clsShelling;

		private ShienModel_Air _clsAerial;

		private ShienModel_Rai _clsTorpedo;

		private ProdSupportCutIn _prodSupportCutIn;

		private ProdSupportShelling _prodSupportShelling;

		private ProdSupportTorpedoP1 _prodSupportTorpedoP1;

		private ProdSupportTorpedoP2 _prodSupportTorpedoP2;

		private ProdSupportAerialPhase1 _prodSupportAerialPhase1;

		private ProdSupportAerialPhase2 _prodSupportAerialPhase2;

		private PSTorpedoWake TorpedoParticle;

		private ParticleSystem SplashPar;

		protected override bool Init()
		{
			_clsShien = BattleTaskManager.GetBattleManager().GetShienData();
			_clsState = new StatementMachine();
			if (_clsShien == null)
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			}
			else
			{
				_clsState.AddState(_initSupportFleetAdmission, _updateSupportFleetAdmission);
			}
			return true;
		}

		protected override bool UnInit()
		{
			_clsShien = null;
			_clsState.Clear();
			if (_prodSupportCutIn != null)
			{
				_prodSupportCutIn.gameObject.Discard();
			}
			_prodSupportCutIn = null;
			if (_prodSupportShelling != null)
			{
				_prodSupportShelling.gameObject.Discard();
			}
			_prodSupportShelling = null;
			if (_prodSupportTorpedoP1 != null && _prodSupportTorpedoP1.transform != null)
			{
				Object.Destroy(_prodSupportTorpedoP1.transform.gameObject);
			}
			_prodSupportTorpedoP1 = null;
			if (_prodSupportTorpedoP2 != null && _prodSupportTorpedoP2.transform != null)
			{
				Object.Destroy(_prodSupportTorpedoP2.transform.gameObject);
			}
			_prodSupportTorpedoP2 = null;
			if (_prodSupportAerialPhase1 != null)
			{
				_prodSupportAerialPhase1.gameObject.Discard();
			}
			_prodSupportAerialPhase1 = null;
			if (_prodSupportAerialPhase2 != null)
			{
				_prodSupportAerialPhase2.gameObject.Discard();
			}
			_prodSupportAerialPhase2 = null;
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.SupportingFire);
		}

		private bool _initSupportFleetAdmission(object data)
		{
			BattleTaskManager.GetBattleField().ResetFleetAnchorPosition();
			BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(isDeploy: false);
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			Observable.FromCoroutine((IObserver<bool> observer) => CreateCutIn(observer)).Subscribe(delegate
			{
				_prodSupportCutIn.Play(delegate
				{
					_onSupportFleetAdmissionFinished();
				});
			});
			return false;
		}

		private IEnumerator CreateCutIn(IObserver<bool> observer)
		{
			BattleCameras cams = BattleTaskManager.GetBattleCameras();
			cams.SwitchMainCamera(FleetType.Friend);
			cams.InitEnemyFieldCameraDefault();
			_prodSupportCutIn = ProdSupportCutIn.Instantiate(parent: cams.cutInCamera.transform, prefab: ((Component)BattleTaskManager.GetPrefabFile().prefabProdSupportCutIn).GetComponent<ProdSupportCutIn>(), model: _clsShien);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateSupportFleetAdmission(object data)
		{
			return true;
		}

		private void _onSupportFleetAdmissionFinished()
		{
			if (_clsShien is ShienModel_Rai)
			{
				_clsTorpedo = (ShienModel_Rai)_clsShien;
				TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD);
				_clsState.AddState(_initSupportTorpedoPhase1, _updateSupportTorpedoPhase1);
			}
			else if (_clsShien is ShienModel_Hou)
			{
				_clsShelling = (ShienModel_Hou)_clsShien;
				_clsState.AddState(_initSupportShelling, _updateSupportShelling);
			}
			else if (_clsShien is ShienModel_Air)
			{
				_clsAerial = (ShienModel_Air)_clsShien;
				_clsState.AddState(_initSupportAerialPhase1, _updateSupportAerialPhase1);
			}
		}

		private bool _initSupportShelling(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => CreateShelling(observer)).Subscribe(delegate
			{
				_prodSupportShelling.Play(_onSupportShellingFinished);
			});
			return false;
		}

		private IEnumerator CreateShelling(IObserver<bool> observer)
		{
			_prodSupportShelling = ProdSupportShelling.Instantiate(Resources.Load<ProdSupportShelling>("Prefabs/Battle/Production/SupportingFire/ProdSupportShelling"), _clsShelling, BattleTaskManager.GetBattleCameras().cutInCamera.transform);
			yield return new WaitForEndOfFrame();
			_prodSupportShelling.CreateHpGauge(FleetType.Enemy);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateSupportShelling(object data)
		{
			return true;
		}

		private void _onSupportShellingFinished()
		{
			PlayProdDamage(_clsShelling, delegate
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			});
		}

		private bool _initSupportAerialPhase1(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => CreateAerialCombatCutIn(observer)).Subscribe(delegate
			{
				_prodSupportAerialPhase1.Play(_onSupportAerialFinishedPhase1);
			});
			return false;
		}

		private IEnumerator CreateAerialCombatCutIn(IObserver<bool> observer)
		{
			BattleCameras cams = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera inCam = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera efCam = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			BattleShips _battleShips = BattleTaskManager.GetBattleShips();
			_prodSupportAerialPhase1 = ProdSupportAerialPhase1.Instantiate(fShips: _battleShips.dicFriendBattleShips, eShips: _battleShips.dicEnemyBattleShips, prefab: Resources.Load<ProdSupportAerialPhase1>("Prefabs/Battle/Production/SupportingFire/ProdSupportAerialPhase1"), model: _clsAerial, parent: cams.cutInCamera.transform.parent);
			if (_clsAerial.GetPlanes(is_friend: true) != null && _clsAerial.GetPlanes(is_friend: false) != null)
			{
				inCam.isCulling = true;
				efCam.isCulling = true;
			}
			else if (_clsAerial.GetPlanes(is_friend: true) != null)
			{
				efCam.isCulling = false;
			}
			yield return new WaitForEndOfFrame();
			_prodSupportAerialPhase2 = ProdSupportAerialPhase2.Instantiate(Resources.Load<ProdSupportAerialPhase2>("Prefabs/Battle/Production/SupportingFire/ProdSupportAerialPhase2"), _clsAerial, inCam.transform);
			yield return new WaitForEndOfFrame();
			_prodSupportAerialPhase2.CreateHpGauge(FleetType.Enemy);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateSupportAerialPhase1(object data)
		{
			return true;
		}

		private void _onSupportAerialFinishedPhase1()
		{
			if (_clsAerial.existStage3())
			{
				_clsState.AddState(_initSupportAerialPhase2, _updateSupportAerialPhase2);
				return;
			}
			Object.Destroy(_prodSupportAerialPhase1.gameObject);
			_onSupportAerialFinishedPhase2();
		}

		private bool _initSupportAerialPhase2(object data)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInCamera cutInCamera2 = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			Dictionary<int, UIBattleShip> dicFriendBattleShip = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShip = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			Object.Destroy(_prodSupportAerialPhase1.gameObject);
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.enabled = true;
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(isSplit: false);
			_prodSupportAerialPhase2.Play(_onSupportAerialFinishedPhase2);
			return false;
		}

		private bool _updateSupportAerialPhase2(object data)
		{
			return true;
		}

		private void _onSupportAerialFinishedPhase2()
		{
			PlayProdDamage(_clsAerial, delegate
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			});
		}

		private bool _initSupportTorpedoPhase1(object data)
		{
			SplashPar = ParticleFile.Load<ParticleSystem>(ParticleFileInfos.BattlePSSplashTorpedo);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			Observable.FromCoroutine((IObserver<bool> observer) => CreateTorpedo1(observer)).Subscribe(delegate
			{
				_prodSupportTorpedoP1.Play(_onSupportTorpedoPhase1Finished);
			});
			return false;
		}

		private IEnumerator CreateTorpedo1(IObserver<bool> observer)
		{
			Transform torpedoStraight = Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabTorpedoStraightController, Vector3.zero, Quaternion.identity) as Transform;
			Transform trams3 = new GameObject().transform;
			trams3.name = "ProdSupportTorpedoPhase1";
			_prodSupportTorpedoP1 = new ProdSupportTorpedoP1(trams3, ((Component)torpedoStraight).GetComponent<TorpedoStraightController>());
			_prodSupportTorpedoP1.Initialize(_clsTorpedo, TorpedoParticle);
			yield return new WaitForEndOfFrame();
			Transform trams2 = new GameObject().transform;
			trams2.name = "ProdSupportTorpedoPhase2";
			_prodSupportTorpedoP2 = new ProdSupportTorpedoP2(trams2);
			_prodSupportTorpedoP2.Initialize(_clsTorpedo, TorpedoParticle);
			yield return new WaitForEndOfFrame();
			_prodSupportTorpedoP2.CreateHpGauge(FleetType.Enemy);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateSupportTorpedoPhase1(object data)
		{
			return _prodSupportTorpedoP1.Update();
		}

		private void _onSupportTorpedoPhase1Finished()
		{
			_clsState.AddState(_initSupportTorpedoPhase2, _updateSupportTorpedoPhase2);
		}

		private bool _initSupportTorpedoPhase2(object data)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors[CameraAnchorType.OneRowAnchor][FleetType.Friend].position;
			battleCameras.friendFieldCamera.transform.position = new Vector3(-38f, 8f, -74f);
			battleCameras.friendFieldCamera.transform.localRotation = Quaternion.Euler(new Vector3(9.5f, 137.5f, 0f));
			BattleTaskManager.GetBattleShips().SetBollboardTarget(isFriend: false, battleCameras.friendFieldCamera.transform);
			if (_prodSupportTorpedoP1 != null)
			{
				_prodSupportTorpedoP1.deleteTorpedoWake();
				_prodSupportTorpedoP1.OnSetDestroy();
			}
			_prodSupportTorpedoP1 = null;
			_prodSupportTorpedoP2.Play(_onSupportTorpedoPhase2Finished);
			return false;
		}

		private IEnumerator CreateTorpedo2(IObserver<bool> observer)
		{
			Transform trams = new GameObject().transform;
			trams.name = "ProdSupportTorpedoPhase2";
			_prodSupportTorpedoP2 = new ProdSupportTorpedoP2(trams);
			_prodSupportTorpedoP2.Initialize(_clsTorpedo, TorpedoParticle);
			yield return new WaitForEndOfFrame();
			_prodSupportTorpedoP2.CreateHpGauge(FleetType.Enemy);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateSupportTorpedoPhase2(object data)
		{
			if (_prodSupportTorpedoP2 == null)
			{
				return false;
			}
			return _prodSupportTorpedoP2.Update();
		}

		private void _onSupportTorpedoPhase2Finished()
		{
			PlayProdDamage(_clsTorpedo, delegate
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			});
			if (_prodSupportTorpedoP2 != null)
			{
				Object.Destroy(_prodSupportTorpedoP2.transform.gameObject);
			}
			_prodSupportTorpedoP2 = null;
		}
	}
}
