using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportTorpedoP1
	{
		public enum StateType
		{
			None,
			TorpedoShot,
			TorpedoMove,
			End
		}

		public StateType _stateType;

		private Action _actCallback;

		private ShienModel_Rai _clsTorpedo;

		private PSTorpedoWake _torpedoWake;

		private BattleFieldCamera _fieldCam;

		private List<PSTorpedoWake> _listTorpedoWake;

		private TorpedoStraightController _straightController;

		private bool _isPlaying;

		private float _fPhaseTime;

		private float _fStraightTargetGazeHeight;

		private Vector3 _vecStraightBegin;

		private Vector3 _vecStraightTarget;

		public Transform transform;

		public ProdSupportTorpedoP1(Transform obj, TorpedoStraightController torpedoStraightController)
		{
			transform = obj;
			_straightController = torpedoStraightController;
		}

		public bool Initialize(ShienModel_Rai model, PSTorpedoWake trupedoWake)
		{
			_fieldCam = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_fieldCam.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			_clsTorpedo = model;
			_torpedoWake = trupedoWake;
			_fPhaseTime = 0f;
			_stateType = StateType.None;
			return true;
		}

		public void OnSetDestroy()
		{
			if (_straightController != null)
			{
				UnityEngine.Object.Destroy(_straightController.gameObject);
			}
			Mem.Del(ref _straightController);
			Mem.Del(ref _stateType);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsTorpedo);
			Mem.Del(ref _torpedoWake);
			Mem.Del(ref _fieldCam);
			Mem.Del(ref _vecStraightBegin);
			Mem.Del(ref _vecStraightTarget);
			Mem.DelListSafe(ref _listTorpedoWake);
			if (transform != null)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			Mem.Del(ref transform);
		}

		public bool Update()
		{
			if (_isPlaying && _stateType == StateType.End)
			{
				_onTorpedoAttackFinished();
				_stateType = StateType.None;
				return true;
			}
			return false;
		}

		private void _setState(StateType state)
		{
			_stateType = state;
			_fPhaseTime = 0f;
		}

		public void Play(Action callBack)
		{
			_isPlaying = true;
			_actCallback = callBack;
			_playOnesTorpedo();
			_setState(StateType.TorpedoMove);
			BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(isSet: false);
			_straightController.ReferenceCameraTransform = _fieldCam.transform;
			_straightController.BeginPivot = _vecStraightBegin;
			_straightController.TargetPivot = _vecStraightTarget;
			_straightController._params.gazeHeight = _fStraightTargetGazeHeight;
			_straightController.FlyingFinish2F.Take(1).Subscribe(delegate
			{
				_straightController._isAnimating = false;
				_setState(StateType.End);
			});
			_straightController.PlayAnimation().Subscribe(delegate
			{
			}).AddTo(_straightController.gameObject);
		}

		private void _playOnesTorpedo()
		{
			_fieldCam.motionBlur.enabled = true;
			BattleTaskManager.GetBattleCameras().SetVerticalSplitCameras(isSplit: false);
			_createTorpedoWakeOnes();
			foreach (UIBattleShip value in BattleTaskManager.GetBattleShips().dicEnemyBattleShips.Values)
			{
				value.billboard.billboardTarget = _fieldCam.transform;
			}
		}

		private void _setTopCamera(float _x)
		{
			_fieldCam.transform.position = new Vector3(_x, 20f, 95f);
			_fieldCam.transform.rotation = Quaternion.Euler(new Vector3(90f, -180f, 0f));
			_vecStraightBegin = _fieldCam.transform.position;
		}

		private void _createTorpedoWakeOnes()
		{
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			bool isTC = false;
			for (int i = 0; i < _clsTorpedo.ShienShips.Length; i++)
			{
				if (_clsTorpedo.ShienShips[i] != null && _clsTorpedo.ShienShips[i].ShipType == 4)
				{
					isTC = true;
				}
			}
			_listTorpedoWake = new List<PSTorpedoWake>();
			List<ShipModel_Defender> defenders = _clsTorpedo.GetDefenders(is_friend: false);
			Vector3 injection = default(Vector3);
			for (int j = 0; j < defenders.Count; j++)
			{
				DamageModel attackDamage = _clsTorpedo.GetAttackDamage(defenders[j].TmpId);
				Vector3 vecStraightTarget = (!attackDamage.GetProtectEffect()) ? dicEnemyBattleShips[j].transform.position : dicEnemyBattleShips[0].transform.position;
				float x = vecStraightTarget.x;
				float y = vecStraightTarget.y;
				Vector3 position = dicFriendBattleShips[0].transform.position;
				injection = new Vector3(x, y, position.z - 1f);
				Vector3 target = (attackDamage.GetHitState() != 0) ? new Vector3(vecStraightTarget.x, vecStraightTarget.y, vecStraightTarget.z + 20f) : new Vector3(vecStraightTarget.x - 3f, vecStraightTarget.y, vecStraightTarget.z + 20f);
				_listTorpedoWake.Add(_createTorpedo(isOnes: true, injection, target, 2.65f, isDet: false, isMiss: false));
				_setTopCamera(injection.x);
				_vecStraightTarget = vecStraightTarget;
				Vector3 pointOfGaze = dicEnemyBattleShips[j].pointOfGaze;
				_fStraightTargetGazeHeight = pointOfGaze.y;
			}
			foreach (PSTorpedoWake item in _listTorpedoWake)
			{
				item.Injection(iTween.EaseType.linear, isPlaySE: true, isTC, delegate
				{
				});
			}
		}

		private PSTorpedoWake _createTorpedo(bool isOnes, Vector3 injection, Vector3 target, float _time, bool isDet, bool isMiss)
		{
			return PSTorpedoWake.Instantiate((!isOnes) ? _torpedoWake : ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD), BattleTaskManager.GetBattleField().transform, new Vector3(injection.x, injection.y + 1f, injection.z), new Vector3(target.x, target.y + 1f, target.z), 0, _time, isDet, isMiss);
		}

		public void deleteTorpedoWake()
		{
			if (_listTorpedoWake != null)
			{
				foreach (PSTorpedoWake item in _listTorpedoWake)
				{
					UnityEngine.Object.Destroy(item);
				}
				_listTorpedoWake.Clear();
			}
			_listTorpedoWake = null;
		}

		private void _onTorpedoAttackFinished()
		{
			deleteTorpedoWake();
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
