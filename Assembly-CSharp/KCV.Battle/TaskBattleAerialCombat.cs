using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleAerialCombat : BaseBattleTask
	{
		private KoukuuModel _clsKoukuu;

		private ProdAerialCombatCutinP _prodAerialCutinP;

		private ProdAerialCombatP1 _prodAerialCombatP1;

		private ProdAerialCombatP2 _prodAerialCombatP2;

		private ProdAerialTouchPlane _prodAerialTouchPlane;

		protected override void Dispose(bool isDisposing)
		{
			Mem.Del(ref _clsKoukuu);
			Mem.Del(ref _prodAerialCutinP);
			Mem.Del(ref _prodAerialCombatP1);
			Mem.Del(ref _prodAerialCombatP2);
			Mem.Del(ref _prodAerialTouchPlane);
			base.Dispose(isDisposing);
		}

		protected override bool Init()
		{
			_clsKoukuu = BattleTaskManager.GetBattleManager().GetKoukuuData();
			if (_clsKoukuu == null)
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombat));
			}
			else if (BattleTaskManager.GetBattleManager().GetKoukuuData2() != null)
			{
				EndPhase(BattlePhase.AerialCombatSecond);
			}
			else
			{
				_clsState = new StatementMachine();
				_clsState.AddState(_initAerialCombatCutIn, _updateAerialCombatCutIn);
			}
			return true;
		}

		protected override bool UnInit()
		{
			_clsKoukuu = null;
			if (_prodAerialCutinP != null)
			{
				_prodAerialCutinP.gameObject.Discard();
			}
			_prodAerialCutinP = null;
			if (_prodAerialCombatP1 != null)
			{
				_prodAerialCombatP1.gameObject.Discard();
			}
			_prodAerialCombatP1 = null;
			if (_prodAerialCombatP2 != null)
			{
				_prodAerialCombatP2.gameObject.Discard();
			}
			_prodAerialCombatP2 = null;
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.AerialCombat);
		}

		private bool _initAerialCombatCutIn(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => CreateAerialCombatCutIn(observer)).Subscribe(delegate
			{
				_prodAerialCutinP.Play(_onAerialCombatCutInFinished);
			});
			return false;
		}

		private bool _updateAerialCombatCutIn(object data)
		{
			return true;
		}

		private void _onAerialCombatCutInFinished()
		{
			_clsState.AddState(_initAircraftCombat, _updateAircraftCombat);
		}

		private IEnumerator CreateAerialCombatCutIn(IObserver<bool> observer)
		{
			BattleCameras cams = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cam = cams.cutInCamera;
			cam.isCulling = true;
			_prodAerialCutinP = ProdAerialCombatCutinP.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdAerialCombatCutinP).GetComponent<ProdAerialCombatCutinP>(), _clsKoukuu, cam.transform);
			yield return new WaitForEndOfFrame();
			_prodAerialCombatP1 = ProdAerialCombatP1.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdAerialCombatP1).GetComponent<ProdAerialCombatP1>(), _clsKoukuu, cam.transform.parent, _prodAerialCutinP._chkCutInType());
			_prodAerialCombatP1.gameObject.SetActive(false);
			yield return new WaitForEndOfFrame();
			_prodAerialCombatP2 = ProdAerialCombatP2.Instantiate(PrefabFile.Load<ProdAerialCombatP2>(PrefabFileInfos.BattleProdAerialCombatP2), _clsKoukuu, cam.transform);
			_prodAerialCombatP2.gameObject.SetActive(false);
			yield return new WaitForEndOfFrame();
			_prodAerialCombatP2.CreateHpGauge(FleetType.Friend);
			yield return new WaitForEndOfFrame();
			_prodAerialTouchPlane = ProdAerialTouchPlane.Instantiate(Resources.Load<ProdAerialTouchPlane>("Prefabs/Battle/Production/AerialCombat/ProdAerialTouchPlane"), cam.transform);
			_prodAerialTouchPlane.transform.localPosition = Vector3.zero;
			_prodAerialTouchPlane.SetActive(isActive: false);
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _initAircraftCombat(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			if (_prodAerialCutinP._cutinPhaseCheck())
			{
				battleCameras.SetSplitCameras2D(isSplit: true);
			}
			if (_prodAerialCutinP._chkCutInType() == CutInType.Both)
			{
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			else if (_prodAerialCutinP._chkCutInType() == CutInType.FriendOnly)
			{
				cutInEffectCamera.isCulling = false;
			}
			Object.Destroy(_prodAerialCutinP.gameObject);
			_prodAerialCombatP1.gameObject.SetActive(true);
			_prodAerialCombatP1.Play(_onAerialCombatPhase1Finished);
			return false;
		}

		private bool _updateAircraftCombat(object data)
		{
			return true;
		}

		private void _onAerialCombatPhase1Finished()
		{
			if (_clsKoukuu.existStage3())
			{
				_clsState.AddState(_initAircraftCombatPhase2, _updateAircraftCombatPhase2);
				return;
			}
			Object.Destroy(_prodAerialCombatP1.gameObject);
			_onAircraftCombatFinished();
		}

		private bool _initAircraftCombatPhase2(object data)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInCamera cutInCamera2 = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			_prodAerialCombatP2.gameObject.SetActive(true);
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.enabled = true;
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(isSplit: false);
			SlotitemModel_Battle touchPlane = _clsKoukuu.GetTouchPlane(is_friend: true);
			SlotitemModel_Battle touchPlane2 = _clsKoukuu.GetTouchPlane(is_friend: false);
			_prodAerialTouchPlane.SetActive(isActive: true);
			_prodAerialTouchPlane.Init(touchPlane, touchPlane2);
			_prodAerialCombatP2.Play(_onAircraftCombatFinished, dicFriendBattleShips, dicEnemyBattleShips);
			Object.Destroy(_prodAerialCombatP1.gameObject);
			return false;
		}

		private bool _updateAircraftCombatPhase2(object data)
		{
			return true;
		}

		private void _onAircraftCombatFinished()
		{
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(isSplit: false);
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.enabled = false;
			PlayProdDamage(_clsKoukuu, delegate
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombat));
			});
			if (_prodAerialCombatP2 != null)
			{
				Object.Destroy(_prodAerialCombatP2.gameObject);
			}
			if (_prodAerialTouchPlane != null)
			{
				_prodAerialTouchPlane.Hide();
			}
		}
	}
}
