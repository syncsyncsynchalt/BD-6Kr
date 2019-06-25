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
	public class TaskBattleTorpedoSalvo : BaseBattleTask
	{
		private Transform prefabProdTorpedoStraight;

		private RaigekiModel _clsRaigeki;

		private ProdTorpedoCutIn _prodTorpedoCutIn;

		private GameObject _prodTorpedoCutInTexture;

		private ProdTorpedoSalvoPhase2 _prodTorpedoSalvoPhase2;

		private ProdTorpedoSalvoPhase3 _prodTorpedoSalvoPhase3;

		private PSTorpedoWake TorpedoParticle;

		private UITexture CenterLine;

		protected override bool Init()
		{
			base.Init();
			_clsRaigeki = BattleTaskManager.GetBattleManager().GetRaigekiData();
			if (_clsRaigeki == null)
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.TorpedoSalvo));
			}
			else
			{
				_clsState = new StatementMachine();
				BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
				TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
				_clsState.AddState(_initTorpedoCutInNInjection, _updateTorpedoCutInNInjection);
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
			if (prefabProdTorpedoStraight != null)
			{
				UnityEngine.Object.Destroy(prefabProdTorpedoStraight.gameObject);
			}
			prefabProdTorpedoStraight = null;
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
			return ChkChangePhase(BattlePhase.TorpedoSalvo);
		}

		private bool _initTorpedoCutInNInjection(object data)
		{
			BattleTaskManager.GetBattleShips().SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			Observable.FromCoroutine((UniRx.IObserver<bool> observer) => CreateTorpedoCutIn(observer)).Subscribe(delegate
			{
				_prodTorpedoCutIn.Play(delegate
				{
					_onTorpedoCutInNInjectionFinished();
				});
			});
			return false;
		}

		private IEnumerator CreateTorpedoCutIn(UniRx.IObserver<bool> observer)
		{
			BattleCutInEffectCamera cam = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture centerLine = ((Component)cam.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			if (centerLine != null)
			{
				centerLine.alpha = 0f;
			}
			_prodTorpedoCutIn = ProdTorpedoCutIn.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdTorpedoCutIn).GetComponent<ProdTorpedoCutIn>(), _clsRaigeki, cam.transform);
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			yield return new WaitForEndOfFrame();
			prefabProdTorpedoStraight = (UnityEngine.Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabTorpedoStraightController, Vector3.zero, Quaternion.identity) as Transform);
			Transform transP4 = new GameObject().transform;
			transP4.name = "ProdTorpedoSalvoPhase2";
			_prodTorpedoSalvoPhase2 = new ProdTorpedoSalvoPhase2(transP4, ((Component)prefabProdTorpedoStraight).GetComponent<TorpedoStraightController>());
			_prodTorpedoSalvoPhase2.Initialize(_clsRaigeki, TorpedoParticle, centerLine);
			yield return new WaitForEndOfFrame();
			Transform transP3 = new GameObject().transform;
			transP3.name = "ProdTorpedoSalvoPhase3";
			_prodTorpedoSalvoPhase3 = new ProdTorpedoSalvoPhase3(transP3);
			_prodTorpedoSalvoPhase3.Initialize(_clsRaigeki, TorpedoParticle);
			yield return new WaitForEndOfFrame();
			_prodTorpedoSalvoPhase3.SetHpGauge();
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateTorpedoCutInNInjection(object data)
		{
			return true;
		}

		private void _onTorpedoCutInNInjectionFinished()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			CenterLine = ((Component)cutInEffectCamera.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			CenterLine.alpha = 1f;
			_clsState.AddState(_initTorpedoCloseUp, _updateTorpedoCloseUp);
		}

		private bool _initTorpedoCloseUp(object data)
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			_prodTorpedoSalvoPhase2.Play(_onTorpedoCloseUpFinished);
			return false;
		}

		private IEnumerator CreateTorpedoSalvoPhase2(UniRx.IObserver<bool> observer)
		{
			prefabProdTorpedoStraight = (UnityEngine.Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabTorpedoStraightController, Vector3.zero, Quaternion.identity) as Transform);
			yield return new WaitForEndOfFrame();
			Transform trams = new GameObject().transform;
			trams.name = "ProdTorpedoSalvoPhase2";
			_prodTorpedoSalvoPhase2 = new ProdTorpedoSalvoPhase2(trams, ((Component)prefabProdTorpedoStraight).GetComponent<TorpedoStraightController>());
			_prodTorpedoSalvoPhase2.Initialize(_clsRaigeki, TorpedoParticle, CenterLine);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateTorpedoCloseUp(object data)
		{
			if (_prodTorpedoSalvoPhase2 == null)
			{
				return false;
			}
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
				_prodTorpedoSalvoPhase2.OnSetDestroy();
			}
			_prodTorpedoSalvoPhase2 = null;
			if (prefabProdTorpedoStraight != null)
			{
				UnityEngine.Object.Destroy(prefabProdTorpedoStraight.gameObject);
			}
			prefabProdTorpedoStraight = null;
			battleCameras.SetVerticalSplitCameras(isSplit: true);
			battleCameras.friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			battleCameras.enemyFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors[CameraAnchorType.OneRowAnchor][FleetType.Friend].position;
			battleCameras.friendFieldCamera.transform.position = new Vector3(-51f, 8f, 90f);
			battleCameras.friendFieldCamera.transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 70f, 0f));
			Vector3 position2 = battleField.dicCameraAnchors[CameraAnchorType.OneRowAnchor][FleetType.Enemy].position;
			battleCameras.enemyFieldCamera.transform.position = new Vector3(-51f, 8f, -90f);
			battleCameras.enemyFieldCamera.transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 111f, 0f));
			_prodTorpedoSalvoPhase3.Play(_onTorpedoExplosionFinished);
			return false;
		}

		private IEnumerator CreateTorpedoSalvoPhase3(UniRx.IObserver<bool> observer)
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
			if (_prodTorpedoSalvoPhase3 == null)
			{
				return false;
			}
			return _prodTorpedoSalvoPhase3.Update();
		}

		private void _onTorpedoExplosionFinished()
		{
			UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
			circleHPGauge.transform.localScaleZero();
			PlayProdDamage(_clsRaigeki, delegate
			{
				BattleTaskManager.GetBattleCameras().cutInEffectCamera.isCulling = true;
				Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
				{
					ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
					observerAction.Register(delegate
					{
						BattleTaskManager.GetTorpedoHpGauges().Hide();
					});
					EndPhase(BattleUtils.NextPhase(BattlePhase.TorpedoSalvo));
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
