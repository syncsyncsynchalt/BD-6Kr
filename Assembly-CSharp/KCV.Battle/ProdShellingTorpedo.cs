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
	public class ProdShellingTorpedo : IDisposable
	{
		public Transform prefabProdTorpedoCutIn;

		public Transform prefabTorpedoStraightController;

		public Transform prefabProdTorpedoResucueCutIn;

		public Transform prefabProdTorpedoStraight;

		private StatementMachine _clsState;

		private RaigekiModel _clsRaigeki;

		private ProdTorpedoCutIn _prodTorpedoCutIn;

		private GameObject _prodTorpedoCutInTexture;

		private ProdTorpedoSalvoPhase2 _prodTorpedoSalvoPhase2;

		private ProdTorpedoSalvoPhase3 _prodTorpedoSalvoPhase3;

		private PSTorpedoWake TorpedoParticle;

		private UITexture CenterLine;

		private bool _isPlaying;

		private Action _actOnFinished;

		public ProdTorpedoSalvoPhase2 prodTorpedoSalvoPhase2 => _prodTorpedoSalvoPhase2;

		public bool isPlaying => _isPlaying;

		public ProdShellingTorpedo(RaigekiModel model)
		{
			_clsState = new StatementMachine();
			_isPlaying = false;
			_actOnFinished = null;
			_clsRaigeki = model;
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
		}

		public void Dispose()
		{
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
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
			if (_clsRaigeki != null)
			{
				_clsRaigeki = null;
			}
			TorpedoParticle = null;
		}

		public void Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
		}

		public void Play(Action onFinished)
		{
			_isPlaying = true;
			_actOnFinished = onFinished;
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
			_clsState.AddState(_initTorpedoCutInNInjection, _updateTorpedoCutInNInjection);
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
			cam.motionBlur.enabled = false;
			UITexture centerLine = ((Component)cam.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			if (centerLine != null)
			{
				centerLine.alpha = 0f;
			}
			_prodTorpedoCutIn = ProdTorpedoCutIn.Instantiate((!(prefabProdTorpedoCutIn != null)) ? Resources.Load<ProdTorpedoCutIn>("Prefabs/Battle/Production/Torpedo/ProdTorpedoCutIn") : ((Component)prefabProdTorpedoCutIn).GetComponent<ProdTorpedoCutIn>(), _clsRaigeki, cam.transform);
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			yield return new WaitForEndOfFrame();
			prefabProdTorpedoStraight = (UnityEngine.Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabTorpedoStraightController, Vector3.zero, Quaternion.identity) as Transform);
			Transform trams3 = new GameObject().transform;
			trams3.name = "ProdTorpedoSalvoPhase2";
			_prodTorpedoSalvoPhase2 = new ProdTorpedoSalvoPhase2(trams3, ((Component)prefabProdTorpedoStraight).GetComponent<TorpedoStraightController>());
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

		private bool _updateTorpedoCutInNInjection(object data)
		{
			return true;
		}

		private void _onTorpedoCutInNInjectionFinished()
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
			BattleTaskManager.GetBattleShips().SetBollboardTarget(isFriend: false, battleCameras.enemyFieldCamera.transform);
			_prodTorpedoSalvoPhase3.Play(_onTorpedoExplosionFinished);
			return false;
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
			if (_prodTorpedoSalvoPhase3 != null)
			{
				UnityEngine.Object.Destroy(_prodTorpedoSalvoPhase3.transform.gameObject);
			}
			_prodTorpedoSalvoPhase3 = null;
			Dlg.Call(ref _actOnFinished);
		}
	}
}
