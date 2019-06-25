using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleAerialCombatSecond : BaseBattleTask
	{
		private KoukuuModel _clsKoukuu1;

		private KoukuuModel _clsKoukuu2;

		private ProdAerialSecondCutIn _prodAerialCutinP;

		private ProdAerialCombatP1 _prodAerialCombatP1;

		private ProdAerialCombatP2 _prodAerialCombatP2;

		private ProdAerialTouchPlane _prodAerialTouchPlane;

		private ProdAerialCombatP1 _prodAerialSecondP1;

		private ProdAerialCombatP2 _prodAerialSecondP2;

		private Dictionary<int, UIBattleShip> friendBattleship;

		private Dictionary<int, UIBattleShip> enemyBattleship;

		protected override bool Init()
		{
			_clsKoukuu1 = BattleTaskManager.GetBattleManager().GetKoukuuData();
			_clsKoukuu2 = BattleTaskManager.GetBattleManager().GetKoukuuData2();
			if (_clsKoukuu1 == null)
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombatSecond));
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
			_clsKoukuu1 = null;
			_clsKoukuu2 = null;
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
			if (_prodAerialSecondP1 != null)
			{
				_prodAerialSecondP1.gameObject.Discard();
			}
			_prodAerialSecondP1 = null;
			if (_prodAerialSecondP2 != null)
			{
				_prodAerialSecondP2.gameObject.Discard();
			}
			_prodAerialSecondP2 = null;
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
			return ChkChangePhase(BattlePhase.AerialCombatSecond);
		}

		private bool _initAerialCombatCutIn(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			_prodAerialCutinP = ProdAerialSecondCutIn.Instantiate(PrefabFile.Load<ProdAerialSecondCutIn>(PrefabFileInfos.BattleProdAerialSecondCutIn), _clsKoukuu1, cutInCamera.transform);
			_prodAerialCombatP1 = ProdAerialCombatP1.Instantiate(PrefabFile.Load<ProdAerialCombatP1>(PrefabFileInfos.BattleProdAerialCombatP1), _clsKoukuu1, cutInCamera.transform.parent, _prodAerialCutinP._chkCutInType());
			_prodAerialCombatP1.gameObject.SetActive(false);
			_prodAerialCombatP2 = ProdAerialCombatP2.Instantiate(PrefabFile.Load<ProdAerialCombatP2>(PrefabFileInfos.BattleProdAerialCombatP2), _clsKoukuu1, cutInCamera.transform);
			_prodAerialCombatP2.CreateHpGauge(FleetType.Friend);
			_prodAerialCombatP2.gameObject.SetActive(false);
			_prodAerialCutinP.Play(_onAerialCombatCutInFinished);
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
			if (_clsKoukuu1.existStage3())
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
			Object.Destroy(_prodAerialCombatP1.gameObject);
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.enabled = true;
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(isSplit: false);
			SlotitemModel_Battle touchPlane = _clsKoukuu1.GetTouchPlane(is_friend: true);
			SlotitemModel_Battle touchPlane2 = _clsKoukuu1.GetTouchPlane(is_friend: false);
			_prodAerialTouchPlane = ProdAerialTouchPlane.Instantiate(Resources.Load<ProdAerialTouchPlane>("Prefabs/Battle/Production/AerialCombat/ProdAerialTouchPlane"), cutInCamera2.transform);
			_prodAerialTouchPlane.transform.localPosition = Vector3.zero;
			_prodAerialTouchPlane.Init(touchPlane, touchPlane2);
			_prodAerialCombatP2.Play(_onAircraftCombatFinished, dicFriendBattleShips, dicEnemyBattleShips);
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
			cutInEffectCamera.glowEffect.enabled = true;
			PlayProdDamage(_clsKoukuu1, delegate
			{
				_clsState.AddState(_initAerialSecondCutIn, _updateAerialSecondCutIn);
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

		private bool _initAerialSecondCutIn(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			_prodAerialSecondP1 = ProdAerialCombatP1.Instantiate(iType: (_clsKoukuu2.GetCaptainShip(is_friend: true) != null && _clsKoukuu2.GetCaptainShip(is_friend: false) != null) ? CutInType.Both : ((_clsKoukuu2.GetCaptainShip(is_friend: true) == null) ? CutInType.EnemyOnly : CutInType.FriendOnly), prefab: PrefabFile.Load<ProdAerialCombatP1>(PrefabFileInfos.BattleProdAerialCombatP1), model: _clsKoukuu2, parent: cutInCamera.transform.parent);
			_prodAerialSecondP1.gameObject.SetActive(false);
			_prodAerialSecondP2 = ProdAerialCombatP2.Instantiate(PrefabFile.Load<ProdAerialCombatP2>(PrefabFileInfos.BattleProdAerialCombatP2), _clsKoukuu2, cutInCamera.transform);
			_prodAerialSecondP2.CreateHpGauge(FleetType.Friend);
			_prodAerialSecondP2.gameObject.SetActive(false);
			_onAerialSecondCutInFinished();
			return false;
		}

		private bool _updateAerialSecondCutIn(object data)
		{
			return true;
		}

		private void _onAerialSecondCutInFinished()
		{
			_clsState.AddState(_initAircraftSecond, _updateAircraftSecond);
		}

		private bool _initAircraftSecond(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			CutInType cutInType = (_clsKoukuu2.GetCaptainShip(is_friend: true) != null && _clsKoukuu2.GetCaptainShip(is_friend: false) != null) ? CutInType.Both : ((_clsKoukuu2.GetCaptainShip(is_friend: true) == null) ? CutInType.EnemyOnly : CutInType.FriendOnly);
			if (cutInType == CutInType.Both)
			{
				battleCameras.SetSplitCameras2D(isSplit: true);
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			else
			{
				if (cutInType == CutInType.FriendOnly)
				{
					cutInEffectCamera.isCulling = false;
				}
				battleCameras.SetSplitCameras2D(isSplit: false);
			}
			_prodAerialSecondP1.gameObject.SetActive(true);
			_prodAerialSecondP1.Play(_onAerialSecondPhase1Finished);
			return false;
		}

		private bool _updateAircraftSecond(object data)
		{
			return true;
		}

		private void _onAerialSecondPhase1Finished()
		{
			if (_clsKoukuu2.existStage3())
			{
				_clsState.AddState(_initAircraftSecondPhase2, _updateAircraftSecondPhase2);
				return;
			}
			Object.Destroy(_prodAerialSecondP1.gameObject);
			_onAircraftSecondFinished();
		}

		private bool _initAircraftSecondPhase2(object data)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInCamera cutInCamera2 = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			_prodAerialSecondP2.gameObject.SetActive(true);
			Object.Destroy(_prodAerialSecondP1.gameObject);
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.enabled = true;
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(isSplit: false);
			_prodAerialSecondP2.Play(_onAircraftSecondFinished, dicFriendBattleShips, dicEnemyBattleShips);
			return false;
		}

		private bool _updateAircraftSecondPhase2(object data)
		{
			return true;
		}

		private void _onAircraftSecondFinished()
		{
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(isSplit: false);
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.enabled = true;
			PlayProdDamage(_clsKoukuu2, delegate
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombatSecond));
			});
			if (_prodAerialSecondP2 != null)
			{
				Object.Destroy(_prodAerialSecondP2.gameObject);
			}
			if (_prodAerialTouchPlane != null)
			{
				_prodAerialTouchPlane.Hide();
			}
		}
	}
}
