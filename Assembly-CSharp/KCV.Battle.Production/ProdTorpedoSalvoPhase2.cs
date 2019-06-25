using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoSalvoPhase2
	{
		private enum StateType
		{
			None,
			Line,
			TorpedoShot,
			TorpedoMove,
			End
		}

		private bool[] _isTC;

		private bool _isPlaying;

		private float _fPhaseTime;

		private float _straightTargetGazeY;

		private Vector3 _straightBegin;

		private Vector3 _straightTarget;

		private Action _actCallback;

		private Dictionary<FleetType, bool> _dicIsAttack;

		private UITexture _centerLine;

		private BattleFieldCamera _fieldCam;

		private PSTorpedoWake _torpedoWake;

		private RaigekiModel _clsTorpedo;

		private TorpedoStraightController _straightController;

		private PSTorpedoWake _onesTorpedoWake;

		private List<PSTorpedoWake> _listTorpedoWake;

		private StateType _stateType;

		public Transform transform;

		public ProdTorpedoSalvoPhase2(Transform obj, TorpedoStraightController torpedoStraightController)
		{
			transform = obj;
			_straightController = torpedoStraightController;
		}

		public bool Initialize(RaigekiModel model, PSTorpedoWake psTorpedo, UITexture line)
		{
			_fieldCam = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_isPlaying = false;
			_isTC = new bool[2];
			_clsTorpedo = model;
			_torpedoWake = psTorpedo;
			_centerLine = line;
			_fPhaseTime = 0f;
			_stateType = StateType.None;
			_dicIsAttack = new Dictionary<FleetType, bool>();
			_dicIsAttack.Add(FleetType.Friend, value: false);
			_dicIsAttack.Add(FleetType.Enemy, value: false);
			return true;
		}

		public void OnSetDestroy()
		{
			if (_straightController != null)
			{
				UnityEngine.Object.Destroy(_straightController.gameObject);
			}
			Mem.Del(ref _stateType);
			Mem.Del(ref _straightBegin);
			Mem.Del(ref _straightTarget);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _torpedoWake);
			Mem.Del(ref _straightController);
			Mem.Del(ref _onesTorpedoWake);
			Mem.DelListSafe(ref _listTorpedoWake);
			Mem.DelDictionarySafe(ref _dicIsAttack);
			_clsTorpedo = null;
			if (transform != null)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			Mem.Del(ref transform);
		}

		public bool Run()
		{
			if (_isPlaying)
			{
				if (_stateType == StateType.TorpedoShot)
				{
					_fPhaseTime += Time.deltaTime;
					if (_fPhaseTime > 2f)
					{
						_centerLine.alpha = 0f;
						_startZoomTorpedo();
						_setState(StateType.TorpedoMove);
						BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(isSet: false);
						_straightController.ReferenceCameraTransform = _fieldCam.transform;
						_straightController.BeginPivot = _straightBegin;
						_straightController.TargetPivot = _straightTarget;
						_straightController._params.gazeHeight = _straightTargetGazeY;
						_straightController.FlyingFinish2F.Take(1).Subscribe(delegate
						{
							_straightController._isAnimating = false;
							_setState(StateType.End);
						});
						_straightController.PlayAnimation().Subscribe(delegate
						{
						}).AddTo(_straightController.gameObject);
					}
				}
				else if (_stateType == StateType.End)
				{
					_onTorpedoAttackFinished();
					_stateType = StateType.None;
					return true;
				}
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
			_stateType = StateType.TorpedoShot;
			_fieldCam.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			_setAttack();
			Observable.FromCoroutine((UniRx.IObserver<bool> observer) => _createTorpedoWake(observer)).Subscribe(delegate
			{
				injectionTorpedo();
			});
		}

		private void _startZoomTorpedo()
		{
			if (_listTorpedoWake != null)
			{
				foreach (PSTorpedoWake item in _listTorpedoWake)
				{
					item.SetActive(isActive: false);
					UnityEngine.Object.Destroy(item);
				}
				_listTorpedoWake.Clear();
			}
			_listTorpedoWake = null;
			BattleTaskManager.GetBattleCameras().SetVerticalSplitCameras(isSplit: false);
			if (_dicIsAttack[FleetType.Friend])
			{
				_setTorpedoWakeZoom(isFriend: true);
			}
			else if (_dicIsAttack[FleetType.Enemy])
			{
				_setTorpedoWakeZoom(isFriend: false);
			}
		}

		private void _setCameraPosition(float _x)
		{
			if (_dicIsAttack[FleetType.Friend])
			{
				_fieldCam.transform.position = new Vector3(_x, 20f, 92f);
				_fieldCam.transform.rotation = Quaternion.Euler(new Vector3(90f, -180f, 0f));
			}
			else if (_dicIsAttack[FleetType.Enemy])
			{
				_fieldCam.transform.position = new Vector3(_x, 20f, -92f);
				_fieldCam.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
			}
			_straightBegin = _fieldCam.transform.position;
			BattleTaskManager.GetBattleCameras().fieldDimCamera.maskAlpha = 0f;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(isFriend: true, _fieldCam.transform);
			battleShips.SetBollboardTarget(isFriend: false, _fieldCam.transform);
		}

		private void _setAttack()
		{
			_dicIsAttack[FleetType.Friend] = false;
			_dicIsAttack[FleetType.Enemy] = false;
			List<ShipModel_Attacker> attackers = _clsTorpedo.GetAttackers(is_friend: true);
			if (attackers != null && attackers.Count > 0)
			{
				_dicIsAttack[FleetType.Friend] = true;
			}
			List<ShipModel_Attacker> attackers2 = _clsTorpedo.GetAttackers(is_friend: false);
			if (attackers2 != null && attackers2.Count > 0)
			{
				_dicIsAttack[FleetType.Enemy] = true;
			}
		}

		private IEnumerator _createTorpedoWake(UniRx.IObserver<bool> observer)
		{
			Dictionary<int, UIBattleShip> _dicFriendShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> _dicEnemyShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			_listTorpedoWake = new List<PSTorpedoWake>();
			_createTorpedo(FleetType.Friend, _dicFriendShips, _dicEnemyShips);
			yield return new WaitForEndOfFrame();
			_createTorpedo(FleetType.Enemy, _dicEnemyShips, _dicFriendShips);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private void _createTorpedo(FleetType type, Dictionary<int, UIBattleShip> dicAttackShip, Dictionary<int, UIBattleShip> dicDefenceShip)
		{
			if (!_dicIsAttack[type])
			{
				return;
			}
			for (int i = 0; i < dicAttackShip.Count; i++)
			{
				ShipModel_Battle shipModel = dicAttackShip[i].shipModel;
				ShipModel_Battle attackTo = _clsTorpedo.GetAttackTo(shipModel);
				if (shipModel != null && attackTo != null)
				{
					if (shipModel.ShipType == 4)
					{
						_isTC[(int)type] = true;
					}
					if (_listTorpedoWake != null)
					{
						_listTorpedoWake.Add(_instantiateTorpedo(dicAttackShip[i].transform.position, dicDefenceShip[attackTo.Index].transform.position, i, 8f, isDet: false, isMiss: false, isD: false));
					}
				}
			}
		}

		private void injectionTorpedo()
		{
			_straightController.DelayAction(0.5f, delegate
			{
				if (_listTorpedoWake != null)
				{
					foreach (PSTorpedoWake item in _listTorpedoWake)
					{
						item.Injection(iTween.EaseType.linear, isPlaySE: false, isTC: false, null);
					}
					if (_listTorpedoWake.Count > 0)
					{
						KCV.Utils.SoundUtils.PlaySE((!_isTC[0] && !_isTC[1]) ? SEFIleInfos.SE_904 : SEFIleInfos.SE_905);
					}
				}
			});
		}

		private void _setTorpedoWakeZoom(bool isFriend)
		{
			if (_createTorpedoWakeOnes(isFriend))
			{
				_onesTorpedoWake.Injection(iTween.EaseType.linear, isPlaySE: false, isTC: false, delegate
				{
				});
			}
		}

		private bool _createTorpedoWakeOnes(bool isFriend)
		{
			FleetType key = (!isFriend) ? FleetType.Enemy : FleetType.Friend;
			Dictionary<int, UIBattleShip> dictionary = (!isFriend) ? BattleTaskManager.GetBattleShips().dicEnemyBattleShips : BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dictionary2 = (!isFriend) ? BattleTaskManager.GetBattleShips().dicFriendBattleShips : BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			if (_dicIsAttack[key])
			{
				for (int i = 0; i < dictionary.Count; i++)
				{
					ShipModel_Battle shipModel = dictionary[i].shipModel;
					ShipModel_Battle attackTo = _clsTorpedo.GetAttackTo(shipModel);
					if (shipModel != null && attackTo != null)
					{
						Vector3 position = dictionary[i].transform.position;
						Vector3 vector;
						if (isFriend)
						{
							Vector3 position2 = dictionary2[attackTo.Index].transform.position;
							vector = new Vector3(position2.x, position.y, position.z - 1f);
						}
						else
						{
							Vector3 position3 = dictionary2[attackTo.Index].transform.position;
							vector = new Vector3(position3.x, position.y, position.z + 1f);
						}
						Vector3 injection = vector;
						RaigekiDamageModel attackDamage = _clsTorpedo.GetAttackDamage(attackTo.Index, (!isFriend) ? true : false);
						int key2 = (!attackDamage.GetProtectEffect(shipModel.TmpId)) ? attackTo.Index : 0;
						float num = (!isFriend) ? (-20f) : 20f;
						float num2 = (attackDamage.GetHitState(shipModel.TmpId) != 0) ? 0f : (-3f);
						Vector3 position4 = dictionary2[key2].transform.position;
						_onesTorpedoWake = _instantiateTorpedo(target: new Vector3(position4.x + num2, position4.y, position4.z + num), injection: injection, index: i, _time: 2.65f, isDet: false, isMiss: false, isD: true);
						_setCameraPosition(injection.x);
						_straightTarget = position4;
						Vector3 pointOfGaze = dictionary2[key2].pointOfGaze;
						_straightTargetGazeY = pointOfGaze.y;
						break;
					}
				}
			}
			return (_onesTorpedoWake != null) ? true : false;
		}

		private PSTorpedoWake _instantiateTorpedo(Vector3 injection, Vector3 target, int index, float _time, bool isDet, bool isMiss, bool isD)
		{
			return PSTorpedoWake.Instantiate(injectionVec: new Vector3(injection.x, injection.y + 1f, injection.z), target: new Vector3(target.x, target.y + 1f, target.z), prefab: (!isD) ? _torpedoWake : ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD), parent: BattleTaskManager.GetBattleField().transform, attacker: index, _time: _time, isDet: isDet, isMiss: isMiss);
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
				_listTorpedoWake = null;
			}
		}

		private void _onTorpedoAttackFinished()
		{
			deleteTorpedoWake();
			if (_onesTorpedoWake != null)
			{
				UnityEngine.Object.Destroy(_onesTorpedoWake);
			}
			_onesTorpedoWake = null;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
