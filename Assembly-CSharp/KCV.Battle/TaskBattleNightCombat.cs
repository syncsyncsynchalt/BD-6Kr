using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleNightCombat : BaseBattleTask
	{
		private ProdNightRadarDeployment _prodNightRadarDeployment;

		private NightCombatModel _clsNightCombat;

		private HougekiListModel _clsHougekiList;

		private ProdNightMessage _prodNightMessage;

		private ProdShellingAttack _prodShellingAttack;

		private SearchLightSceneController _ctrlSearchLight;

		private FlareBulletSceneController _ctrlFlareBullet;

		private int _nCurrentShellingCnt;

		private ProdAerialTouchPlane _prodAerialTouchPlane;

		private Vector3 _vCameraOriginPos;

		private int shellingCnt
		{
			get
			{
				if (_clsHougekiList == null)
				{
					return -1;
				}
				return _clsHougekiList.Count;
			}
		}

		private bool isNextAttack
		{
			get
			{
				if (shellingCnt == _nCurrentShellingCnt)
				{
					return false;
				}
				return true;
			}
		}

		protected override bool Init()
		{
			_clsNightCombat = BattleTaskManager.GetBattleManager().GetNightCombatData();
			_clsHougekiList = BattleTaskManager.GetBattleManager().GetHougekiList_Night();
			if (_clsHougekiList == null)
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.NightCombat));
				ImmediateTermination();
			}
			else
			{
				_nCurrentShellingCnt = 1;
				_clsState = new StatementMachine();
				_prodShellingAttack = new ProdShellingAttack();
				_vCameraOriginPos = BattleTaskManager.GetBattleCameras().fieldCameras[0].transform.position;
				if (!BattleTaskManager.GetIsSameBGM())
				{
					KCV.Utils.SoundUtils.SwitchBGM((BGMFileInfos)BattleTaskManager.GetBattleManager().GetBgmId());
				}
				_clsState.AddState(InitNightMessage, UpdateNightMessage);
				Transform transform = UnityEngine.Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabSearchLightSceneController, Vector3.zero, Quaternion.identity) as Transform;
				_ctrlSearchLight = ((Component)transform).GetComponent<SearchLightSceneController>();
				Transform transform2 = UnityEngine.Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabFlareBulletSceneController, Vector3.zero, Quaternion.identity) as Transform;
				_ctrlFlareBullet = ((Component)transform2).GetComponent<FlareBulletSceneController>();
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del(ref _clsNightCombat);
			Mem.Del(ref _clsHougekiList);
			Mem.Del(ref _prodNightMessage);
			if (_prodShellingAttack != null)
			{
				_prodShellingAttack.Dispose();
			}
			Mem.Del(ref _prodShellingAttack);
			if (_prodAerialTouchPlane != null)
			{
				UnityEngine.Object.Destroy(_prodAerialTouchPlane.gameObject);
			}
			Mem.Del(ref _prodAerialTouchPlane);
			if (_ctrlSearchLight != null)
			{
				UnityEngine.Object.Destroy(_ctrlSearchLight.gameObject);
			}
			Mem.Del(ref _ctrlSearchLight);
			if (_ctrlFlareBullet != null)
			{
				UnityEngine.Object.Destroy(_ctrlFlareBullet.gameObject);
			}
			Mem.Del(ref _ctrlFlareBullet);
			Mem.Del(ref _vCameraOriginPos);
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.NightCombat);
		}

		private bool InitNightMessage(object data)
		{
			_prodNightRadarDeployment = ProdNightRadarDeployment.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdNightRadarDeployment).GetComponent<ProdNightRadarDeployment>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
			_prodNightRadarDeployment.Play().Subscribe(delegate
			{
				OnNightMessageFinished();
			});
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.isEnemySeaLevelActive = false;
			ShipModel_Battle model = BattleTaskManager.GetBattleManager().Ships_f[0];
			KCV.Battle.Utils.ShipUtils.PlayStartNightCombatVoice(model);
			return false;
		}

		private bool UpdateNightMessage(object data)
		{
			return true;
		}

		private void OnNightMessageFinished()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			SlotitemModel_Battle touchPlane = _clsNightCombat.GetTouchPlane(is_friend: true);
			SlotitemModel_Battle touchPlane2 = _clsNightCombat.GetTouchPlane(is_friend: false);
			_prodAerialTouchPlane = ((!(cutInCamera.transform.GetComponentInChildren<ProdAerialTouchPlane>() != null)) ? ProdAerialTouchPlane.Instantiate(Resources.Load<ProdAerialTouchPlane>("Prefabs/Battle/Production/AerialCombat/ProdAerialTouchPlane"), cutInCamera.transform) : cutInCamera.transform.GetComponentInChildren<ProdAerialTouchPlane>());
			_prodAerialTouchPlane.transform.localPosition = Vector3.zero;
			_prodAerialTouchPlane.Init(touchPlane, touchPlane2);
			if (_clsNightCombat.GetRationData() != null)
			{
				ProdCombatRation pcr = ProdCombatRation.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdCombatRation).GetComponent<ProdCombatRation>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsNightCombat.GetRationData());
				pcr.SetOnStageReady(delegate
				{
					if (_prodNightRadarDeployment != null)
					{
						_prodNightRadarDeployment.RadarObjectConvergence();
					}
					Mem.DelComponentSafe(ref _prodNightRadarDeployment);
				}).Play(delegate
				{
					_clsState.AddState(InitSearchNFlare, UpdateSearchNFlare);
				});
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Register(delegate
				{
					Mem.DelComponentSafe(ref pcr);
				});
			}
			else
			{
				_clsState.AddState(InitSearchNFlare, UpdateSearchNFlare);
			}
		}

		private bool InitSearchNFlare(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
			battleFieldCamera.flareLayer.enabled = true;
			bool flag = (_clsNightCombat.GetSearchLightShip(is_friend: true) != null) ? true : false;
			bool flag2 = (_clsNightCombat.GetFlareShip(is_friend: true) != null) ? true : false;
			if (flag || flag2)
			{
				if (_prodNightRadarDeployment != null)
				{
					_prodNightRadarDeployment.RadarObjectConvergence();
				}
				Mem.DelComponentSafe(ref _prodNightRadarDeployment);
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				BattleTaskManager.GetBattleShips().SetStandingPosition(StandingPositionType.OneRow);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.Fix);
				battleFieldCamera.transform.position = _vCameraOriginPos;
				battleFieldCamera.transform.rotation = Quaternion.identity;
				ShipModel_BattleAll shipModel_BattleAll = (!flag) ? _clsNightCombat.GetFlareShip(is_friend: true) : _clsNightCombat.GetSearchLightShip(is_friend: true);
				if (shipModel_BattleAll != null)
				{
					BattleField battleField = BattleTaskManager.GetBattleField();
					UIBattleShip uIBattleShip = BattleTaskManager.GetBattleShips().dicFriendBattleShips[shipModel_BattleAll.Index];
					Vector3 position = uIBattleShip.transform.position;
					float x = 0f - position.x;
					battleField.dicFleetAnchor[FleetType.Friend].transform.AddPosX(x);
					battleFieldCamera.transform.AddPosX(x);
				}
			}
			SearchLight_FlareBullet_PlayAnimation().Subscribe(delegate
			{
				OnSearchNFlareFinished();
			});
			return false;
		}

		public UniRx.IObservable<int> SearchLight_FlareBullet_PlayAnimation()
		{
			return Observable.FromCoroutine((UniRx.IObserver<int> observer) => SearchLight_FlareBullet_Coroutine(observer));
		}

		private IEnumerator SearchLight_FlareBullet_Coroutine(UniRx.IObserver<int> observer)
		{
			BattleFieldCamera camera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			Vector3? searchPointOfGaze = GetSearchLightShipPointOfGaze();
			if (searchPointOfGaze.HasValue)
			{
				_ctrlSearchLight.ReferenceCameraTransform = camera.transform;
				_ctrlSearchLight.SearchLightCameraStartPivot = camera.transform.position;
				_ctrlSearchLight.SearchLightPivot = searchPointOfGaze.Value;
				yield return _ctrlSearchLight.PlayAnimation().StartAsCoroutine(delegate
				{
				}, delegate
				{
				});
			}
			Vector3? flarePointOfGaze = GetFlareBulletShipPointOfGaze();
			if (flarePointOfGaze.HasValue)
			{
				_ctrlFlareBullet.ReferenceCameraTransform = camera.transform;
				_ctrlFlareBullet.FlareBulletCameraStartPivot = camera.transform.position;
				_ctrlFlareBullet.FlareBulletFirePivot = flarePointOfGaze.Value;
				yield return _ctrlFlareBullet.PlayAnimation().StartAsCoroutine(delegate
				{
				}, delegate
				{
				});
			}
			observer.OnNext(0);
			observer.OnCompleted();
		}

		private static Vector3? GetShipPointOfGaze(ShipModel_Battle model)
		{
			if (model != null)
			{
				return BattleTaskManager.GetBattleShips().dicFriendBattleShips[model.Index].pointOfGaze;
			}
			return null;
		}

		private Vector3? GetSearchLightShipPointOfGaze()
		{
			return GetShipPointOfGaze(_clsNightCombat.GetSearchLightShip(is_friend: true));
		}

		private Vector3? GetFlareBulletShipPointOfGaze()
		{
			return GetShipPointOfGaze(_clsNightCombat.GetFlareShip(is_friend: true));
		}

		private bool UpdateSearchNFlare(object data)
		{
			return true;
		}

		private void OnSearchNFlareFinished()
		{
			_clsState.AddState(InitNightShelling, UpdateNightShelling);
		}

		private bool InitNightShelling(object data)
		{
			HougekiModel nextData = _clsHougekiList.GetNextData();
			if (nextData == null)
			{
				OnNightShellingFinished();
			}
			else
			{
				_prodShellingAttack.Play(nextData, _nCurrentShellingCnt, isNextAttack, null);
				if (_prodNightRadarDeployment != null)
				{
					_prodNightRadarDeployment.RadarObjectConvergence();
				}
				Mem.DelComponentSafe(ref _prodNightRadarDeployment);
			}
			return false;
		}

		private bool UpdateNightShelling(object data)
		{
			if (_prodShellingAttack.isFinished)
			{
				_prodShellingAttack.Clear();
				_nCurrentShellingCnt++;
				_clsState.AddState(InitNightShelling, UpdateNightShelling);
				return true;
			}
			if (_prodShellingAttack != null)
			{
				_prodShellingAttack.Update();
			}
			return false;
		}

		private void OnNightShellingFinished()
		{
			_clsState.AddState(InitGaugeDestruction, UpdateGaugeDestruction);
		}

		private bool InitGaugeDestruction(object data)
		{
			return false;
		}

		private bool UpdateGaugeDestruction(object data)
		{
			OnGaugeDestructionFinished();
			return true;
		}

		private void OnGaugeDestructionFinished()
		{
			_clsState.AddState(InitDeathCry, UpdateDeathCry);
		}

		private bool InitDeathCry(object data)
		{
			return false;
		}

		private bool UpdateDeathCry(object data)
		{
			OnDeathCryFinished();
			return true;
		}

		private void OnDeathCryFinished()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				if (_prodNightRadarDeployment != null)
				{
					_prodNightRadarDeployment.RadarObjectConvergence();
				}
				Mem.DelComponentSafe(ref _prodNightRadarDeployment);
			});
			Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.NightCombat));
			});
		}
	}
}
