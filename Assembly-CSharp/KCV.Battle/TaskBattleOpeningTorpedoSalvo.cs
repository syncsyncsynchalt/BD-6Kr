using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models.battle;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleOpeningTorpedoSalvo : BaseBattleTask
	{
		private RaigekiModel _clsRaigeki;

		private ProdTorpedoCutIn _prodTorpedoCutIn;

		private GameObject _prodTorpedoCutInTexture;

		private ProdTorpedoSalvoPhase2 _prodTorpedoSalvoPhase2;

		private ProdTorpedoSalvoPhase3 _prodTorpedoSalvoPhase3;

		private PSTorpedoWake TorpedoParticle;

		private UITexture CenterLine;

		protected override bool Init()
		{
			_clsRaigeki = BattleTaskManager.GetBattleManager().GetKaimakuData();
			if (_clsRaigeki == null)
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.OpeningTorpedoSalvo));
			}
			else
			{
				_clsState = new StatementMachine();
				_clsState.AddState(_initTorpedoCutInInjection, _updateTorpedoCutInInjection);
				TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
			}
			return true;
		}

		protected override bool UnInit()
		{
			if (_prodTorpedoCutIn != null)
			{
				_prodTorpedoCutIn.gameObject.Discard();
			}
			_prodTorpedoCutIn = null;
			if (_prodTorpedoSalvoPhase2 != null)
			{
				UnityEngine.Object.Destroy(_prodTorpedoSalvoPhase2.transform.gameObject);
			}
			_prodTorpedoSalvoPhase2 = null;
			if (_prodTorpedoSalvoPhase3 != null)
			{
				UnityEngine.Object.Destroy(_prodTorpedoSalvoPhase3.transform.gameObject);
			}
			_prodTorpedoSalvoPhase3 = null;
			base.UnInit();
			if (_clsRaigeki != null)
			{
				_clsRaigeki = null;
			}
			TorpedoParticle = null;
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.OpeningTorpedoSalvo);
		}

		private bool _initTorpedoCutInInjection(object data)
		{
			BattleTaskManager.GetBattleShips().SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			Observable.FromCoroutine((UniRx.IObserver<bool> observer) => CreateTorpedoCutIn(observer)).Subscribe(delegate
			{
				_prodTorpedoCutIn.Play(delegate
				{
					_onTorpedoCutInInjectionFinished();
				});
			});
			return false;
		}

		private IEnumerator CreateTorpedoCutIn(UniRx.IObserver<bool> observer)
		{
			BattleCutInEffectCamera cam = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cam.isCulling = true;
			cam.motionBlur.enabled = true;
			UITexture centerLine = ((Component)cam.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			if (centerLine != null)
			{
				centerLine.alpha = 0f;
			}
			_prodTorpedoCutIn = ProdTorpedoCutIn.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdOpeningTorpedoCutIn).GetComponent<ProdTorpedoCutIn>(), _clsRaigeki, cam.transform);
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			yield return new WaitForEndOfFrame();
			Transform torpedoStraight = UnityEngine.Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabTorpedoStraightController, Vector3.zero, Quaternion.identity) as Transform;
			Transform trams3 = new GameObject().transform;
			trams3.name = "ProdTorpedoSalvoPhase2";
			_prodTorpedoSalvoPhase2 = new ProdTorpedoSalvoPhase2(trams3, ((Component)torpedoStraight).GetComponent<TorpedoStraightController>());
			_prodTorpedoSalvoPhase2.Initialize(_clsRaigeki, TorpedoParticle, centerLine);
			yield return new WaitForEndOfFrame();
			Transform trams2 = new GameObject().transform;
			trams2.name = "ProdTorpedoSalvoPhase3";
			_prodTorpedoSalvoPhase3 = new ProdTorpedoSalvoPhase3(trams2);
			_prodTorpedoSalvoPhase3.Initialize(_clsRaigeki, TorpedoParticle);
			yield return new WaitForEndOfFrame();
			_prodTorpedoSalvoPhase3.SetHpGauge();
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateTorpedoCutInInjection(object data)
		{
			return true;
		}

		private void _onTorpedoCutInInjectionFinished()
		{
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			CenterLine = ((Component)cutInEffectCamera.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			CenterLine.alpha = 1f;
			cutInEffectCamera.motionBlur.enabled = false;
			_clsState.AddState(_initTorpedoCloseUp, _updateTorpedoCloseUp);
		}

		private bool _initTorpedoCloseUp(object data)
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			_prodTorpedoSalvoPhase2.Play(_onTorpedoCloseUpFinished);
			return false;
		}

		private IEnumerator CreateTorpedoPhase2(UniRx.IObserver<bool> observer)
		{
			Transform torpedoStraight = UnityEngine.Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabTorpedoStraightController, Vector3.zero, Quaternion.identity) as Transform;
			Transform trams = new GameObject().transform;
			trams.name = "ProdTorpedoSalvoPhase2";
			_prodTorpedoSalvoPhase2 = new ProdTorpedoSalvoPhase2(trams, ((Component)torpedoStraight).GetComponent<TorpedoStraightController>());
			_prodTorpedoSalvoPhase2.Initialize(_clsRaigeki, TorpedoParticle, CenterLine);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateTorpedoCloseUp(object data)
		{
			return _prodTorpedoSalvoPhase2.Run();
		}

		private void _onTorpedoCloseUpFinished()
		{
			_clsState.AddState(_initTorpedoExplosion, _updateTorpedoExplosion);
		}

		private bool _initTorpedoExplosion(object data)
		{
			CenterLine.alpha = 1f;
			BattleField battleField = BattleTaskManager.GetBattleField();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			if (_prodTorpedoSalvoPhase2 != null)
			{
				_prodTorpedoSalvoPhase2.deleteTorpedoWake();
				UnityEngine.Object.Destroy(_prodTorpedoSalvoPhase2.transform.gameObject);
			}
			_prodTorpedoSalvoPhase2 = null;
			battleCameras.SetVerticalSplitCameras(isSplit: true);
			battleCameras.friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			battleCameras.enemyFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors[CameraAnchorType.OneRowAnchor][FleetType.Friend].position;
			battleCameras.friendFieldCamera.transform.position = new Vector3(-51f, 8f, 90f);
			battleCameras.friendFieldCamera.transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 70f, 0f));
			Vector3 position2 = battleField.dicCameraAnchors[CameraAnchorType.OneRowAnchor][FleetType.Enemy].position;
			battleCameras.enemyFieldCamera.transform.position = new Vector3(-51f, 8f, -90f);
			battleCameras.enemyFieldCamera.transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 111f, 0f));
			BattleTaskManager.GetBattleShips().SetBollboardTarget(isFriend: false, battleCameras.enemyFieldCamera.transform);
			_prodTorpedoSalvoPhase3.Play(_onTorpedoExplosionFinished);
			return false;
		}

		private IEnumerator CreateTorpedoPhase3(UniRx.IObserver<bool> observer)
		{
			Transform trams = new GameObject().transform;
			trams.name = "ProdTorpedoSalvoPhase3";
			_prodTorpedoSalvoPhase3 = new ProdTorpedoSalvoPhase3(trams);
			_prodTorpedoSalvoPhase3.Initialize(_clsRaigeki, TorpedoParticle);
			yield return new WaitForEndOfFrame();
			_prodTorpedoSalvoPhase3.SetHpGauge();
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateTorpedoExplosion(object data)
		{
			return _prodTorpedoSalvoPhase3.Update();
		}

		private void _onTorpedoExplosionFinished()
		{
			BattleCutInEffectCamera efcam = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
			circleHPGauge.transform.localScaleZero();
			PlayProdDamage(_clsRaigeki, delegate
			{
				efcam.isCulling = true;
				Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
				{
					ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
					observerAction.Register(delegate
					{
						BattleTaskManager.GetTorpedoHpGauges().Hide();
					});
					EndPhase(BattleUtils.NextPhase(BattlePhase.OpeningTorpedoSalvo));
				});
			});
			if (_prodTorpedoSalvoPhase3 != null)
			{
				UnityEngine.Object.Destroy(_prodTorpedoSalvoPhase3.transform.gameObject);
			}
			_prodTorpedoSalvoPhase3 = null;
		}
	}
}
