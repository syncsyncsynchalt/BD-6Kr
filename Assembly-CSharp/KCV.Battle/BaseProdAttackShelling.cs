using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BaseProdAttackShelling : IDisposable
	{
		protected int _nCurrentAttackCnt;

		protected bool _isFinished;

		protected bool _isPlaying;

		protected bool _isNextAttack;

		protected bool _isSkipAttack;

		protected Action _actOnFinished;

		protected HougekiModel _clsHougekiModel;

		protected StatementMachine _clsState;

		protected List<UIBattleShip> _listBattleShips;

		protected Dictionary<FleetType, bool> _dicAttackFleet;

		public HougekiModel hougekiModel
		{
			get
			{
				return _clsHougekiModel;
			}
			private set
			{
				_clsHougekiModel = value;
			}
		}

		public int currentAttackCnt => _nCurrentAttackCnt;

		public bool isFinished => _isFinished;

		public bool isPlaying => _isPlaying;

		public bool isSkipAttack
		{
			get
			{
				if (_listBattleShips[0] == null)
				{
					return false;
				}
				return _isSkipAttack;
			}
		}

		protected bool isProtect
		{
			get
			{
				if (_clsHougekiModel == null)
				{
					return false;
				}
				return _clsHougekiModel.GetProtectEffect();
			}
		}

		protected Transform particleParent
		{
			get
			{
				BattleField battleField = BattleTaskManager.GetBattleField();
				if (_listBattleShips.Count == 0)
				{
					return battleField.dicFleetAnchor[FleetType.Friend];
				}
				return (!_listBattleShips[1].shipModel.IsFriend()) ? battleField.dicFleetAnchor[FleetType.Enemy] : battleField.dicFleetAnchor[FleetType.Friend];
			}
		}

		protected HitState hitState => BattleUtils.ConvertBattleHitState2HitState(_clsHougekiModel);

		protected StandingPositionType subjectStandingPosFmAnD
		{
			set
			{
				UIBattleShip uIBattleShip = _listBattleShips[0];
				_listBattleShips[1].standingPositionType = value;
				uIBattleShip.standingPositionType = value;
			}
		}

		protected Generics.Layers subjectShipLayerFmAnD
		{
			set
			{
				UIBattleShip uIBattleShip = _listBattleShips[0];
				_listBattleShips[1].layer = value;
				uIBattleShip.layer = value;
			}
		}

		protected FleetType alterWaveDirection
		{
			set
			{
				BattleTaskManager.GetBattleField().AlterWaveDirection(value);
			}
		}

		protected virtual Vector3 CalcAttackerCamStartPos
		{
			get
			{
				Vector3 spPointOfGaze = _listBattleShips[0].spPointOfGaze;
				float x = spPointOfGaze.x;
				Vector3 spPointOfGaze2 = _listBattleShips[0].spPointOfGaze;
				return new Vector3(x, spPointOfGaze2.y, 0f);
			}
		}

		protected virtual Vector3 CalcDefenderCamStartPos
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
				Vector3 result;
				if (isSkipAttack)
				{
					Vector3 spPointOfGaze = _listBattleShips[1].spPointOfGaze;
					float x = spPointOfGaze.x;
					Vector3 spPointOfGaze2 = _listBattleShips[1].spPointOfGaze;
					result = new Vector3(x, spPointOfGaze2.y, 0f);
				}
				else
				{
					Vector3 position = battleFieldCamera.transform.position;
					float x2 = position.x;
					Vector3 spPointOfGaze3 = _listBattleShips[1].spPointOfGaze;
					float y = spPointOfGaze3.y;
					Vector3 position2 = battleFieldCamera.transform.position;
					result = new Vector3(x2, y, position2.z);
				}
				return result;
			}
		}

		public BaseProdAttackShelling()
		{
			_nCurrentAttackCnt = 0;
			_isFinished = false;
			_isPlaying = false;
			_clsHougekiModel = null;
			_listBattleShips = new List<UIBattleShip>();
			_dicAttackFleet = new Dictionary<FleetType, bool>();
			_dicAttackFleet.Add(FleetType.Friend, value: false);
			_dicAttackFleet.Add(FleetType.Enemy, value: false);
			_clsState = new StatementMachine();
			_actOnFinished = null;
		}

		public void Dispose()
		{
			Mem.Del(ref _nCurrentAttackCnt);
			Mem.Del(ref _isFinished);
			Mem.Del(ref _isPlaying);
			Mem.Del(ref _actOnFinished);
			Mem.Del(ref _clsHougekiModel);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
			Mem.DelListSafe(ref _listBattleShips);
			Mem.DelDictionarySafe(ref _dicAttackFleet);
			OnDispose();
		}

		public virtual void Clear()
		{
			_clsHougekiModel = null;
			_actOnFinished = null;
			if (_listBattleShips != null)
			{
				_listBattleShips.Clear();
			}
			if (_clsState != null)
			{
				_clsState.Clear();
			}
		}

		public bool Reset()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				BattleTaskManager.GetPrefabFile().circleHPGauge.transform.localScaleZero();
			});
			return true;
		}

		protected virtual void OnDispose()
		{
		}

		protected virtual void OnCameraRotateStart()
		{
		}

		public virtual void PlayAttack(HougekiModel model, int nCurrentShellingCnt, bool isNextAttack, bool isSkipAttack, Action callback)
		{
			if (model == null)
			{
				Dlg.Call(ref callback);
			}
			BattleTaskManager.GetTorpedoHpGauges().Hide();
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			hougekiModel = model;
			_actOnFinished = callback;
			_isNextAttack = isNextAttack;
			_isSkipAttack = isSkipAttack;
			SetDirectionSubjects(hougekiModel);
			_nCurrentAttackCnt = nCurrentShellingCnt;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.ResetFleetAnchorPosition();
			CorFleetAnchorDifPosition();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SetVerticalSplitCameras(isSplit: false);
			BattleShips battleShips2 = BattleTaskManager.GetBattleShips();
			battleShips2.SetBollboardTarget(isFriend: true, battleCameras.fieldCameras[0].transform);
			battleShips2.SetBollboardTarget(isFriend: false, battleCameras.fieldCameras[1].transform);
			battleShips2.SetTorpedoSalvoWakeAngle(isSet: false);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = ((Component)cutInEffectCamera.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 0f;
			}
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
			battleFieldCamera.clearFlags = CameraClearFlags.Skybox;
			battleFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			battleFieldCamera.eyePosition = CalcAttackerCamStartPos;
			battleCameras.SwitchMainCamera(FleetType.Friend);
			BattleFieldCamera battleFieldCamera2 = battleCameras.fieldCameras[1];
			battleFieldCamera2.eyePosition = new Vector3(0f, 4f, 0f);
			battleFieldCamera2.eyeRotation = Quaternion.identity;
			battleFieldCamera2.fieldOfView = 30f;
			SetFieldCamera(isAttacker: true, CalcCamPos(isAttacker: true, isPointOfGaze: false), _listBattleShips[0].spPointOfGaze);
			SetDimCamera(isAttacker: true, battleFieldCamera.transform);
			subjectShipLayerFmAnD = Generics.Layers.FocusDim;
			subjectStandingPosFmAnD = StandingPositionType.Advance;
			BattleTaskManager.GetPrefabFile().circleHPGauge.transform.localScaleZero();
			_clsState.AddState(InitAttackerFocus, UpdateAttackerFocus);
		}

		protected virtual void SetDirectionSubjects(HougekiModel model)
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			_listBattleShips.Add((!model.Attacker.IsFriend()) ? battleShips.dicEnemyBattleShips[model.Attacker.Index] : battleShips.dicFriendBattleShips[model.Attacker.Index]);
			if (isProtect)
			{
				_listBattleShips.Add((!model.Defender.IsFriend()) ? battleShips.flagShipEnemy : battleShips.flagShipFriend);
				_listBattleShips.Add((!model.Defender.IsFriend()) ? battleShips.dicEnemyBattleShips[model.Defender.Index] : battleShips.dicFriendBattleShips[model.Defender.Index]);
			}
			else
			{
				_listBattleShips.Add((!model.Defender.IsFriend()) ? battleShips.dicEnemyBattleShips[model.Defender.Index] : battleShips.dicFriendBattleShips[model.Defender.Index]);
			}
			subjectStandingPosFmAnD = StandingPositionType.Advance;
		}

		protected void CorFleetAnchorDifPosition()
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			Vector3 to = (!_listBattleShips[0].shipModel.IsFriend()) ? _listBattleShips[1].transform.position : _listBattleShips[0].transform.position;
			Vector3 to2 = _listBattleShips[0].shipModel.IsFriend() ? _listBattleShips[1].transform.position : _listBattleShips[0].transform.position;
			Vector3 vector = Mathe.Direction(Vector3.zero, to);
			Vector3 vector2 = Mathe.Direction(Vector3.zero, to2);
			battleField.dicFleetAnchor[FleetType.Friend].transform.AddPosX(0f - vector.x);
			battleField.dicFleetAnchor[FleetType.Enemy].transform.AddPosX(0f - vector2.x);
		}

		protected void SetProtecterLayer()
		{
			_listBattleShips[2].layer = Generics.Layers.FocusDim;
		}

		public virtual bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return true;
		}

		protected virtual bool InitAttackerFocus(object data)
		{
			return false;
		}

		protected virtual bool UpdateAttackerFocus(object data)
		{
			return true;
		}

		protected virtual bool InitRotateFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
			battleFieldCamera.motionBlur.enabled = false;
			GraSubDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME[0]);
			RotateFocusTowardsTarget2RotateFieldCam(_listBattleShips[1].spPointOfGaze);
			RotateFocusTowardTarget2MoveFieldCam(_listBattleShips[1].spPointOfGaze, delegate
			{
				_clsState.AddState(InitDefenderFocus, UpdateDefenderFocus);
			});
			return false;
		}

		protected virtual bool UpdateRotateFocus(object data)
		{
			return true;
		}

		protected virtual bool InitDefenderFocus(object data)
		{
			return false;
		}

		protected virtual bool UpdateDefenderFocus(object data)
		{
			return true;
		}

		protected virtual bool InitDefenderFocusErx(object data)
		{
			return false;
		}

		protected virtual bool UpdateDefenderFocusErx(object data)
		{
			return true;
		}

		protected virtual Vector3 CalcCamPos(bool isAttacker, bool isPointOfGaze)
		{
			int index = (!isAttacker) ? 1 : 0;
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? _listBattleShips[index].spPointOfGaze : _listBattleShips[index].pointOfGaze, battleFieldCamera.eyePosition) * 50f;
			Vector3 result;
			if (isPointOfGaze)
			{
				Vector3 pointOfGaze = _listBattleShips[index].pointOfGaze;
				float x = pointOfGaze.x + vector.x;
				Vector3 pointOfGaze2 = _listBattleShips[index].pointOfGaze;
				float y = pointOfGaze2.y;
				Vector3 pointOfGaze3 = _listBattleShips[index].pointOfGaze;
				result = new Vector3(x, y, pointOfGaze3.z + vector.z);
			}
			else
			{
				Vector3 spPointOfGaze = _listBattleShips[index].spPointOfGaze;
				float x2 = spPointOfGaze.x + vector.x;
				Vector3 spPointOfGaze2 = _listBattleShips[index].spPointOfGaze;
				float y2 = spPointOfGaze2.y;
				Vector3 spPointOfGaze3 = _listBattleShips[index].spPointOfGaze;
				result = new Vector3(x2, y2, spPointOfGaze3.z + vector.z);
			}
			return result;
		}

		protected virtual Vector3 CalcCamTargetPos(bool isAttacker, bool isPointOfGaze)
		{
			int index = (!isAttacker) ? 1 : 0;
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? _listBattleShips[index].spPointOfGaze : _listBattleShips[index].pointOfGaze, battleFieldCamera.eyePosition) * 10f;
			Vector3 result;
			if (isPointOfGaze)
			{
				Vector3 pointOfGaze = _listBattleShips[index].pointOfGaze;
				float x = pointOfGaze.x + vector.x;
				Vector3 pointOfGaze2 = _listBattleShips[index].pointOfGaze;
				float y = pointOfGaze2.y;
				Vector3 pointOfGaze3 = _listBattleShips[index].pointOfGaze;
				result = new Vector3(x, y, pointOfGaze3.z + vector.z);
			}
			else
			{
				Vector3 spPointOfGaze = _listBattleShips[index].spPointOfGaze;
				float x2 = spPointOfGaze.x + vector.x;
				Vector3 spPointOfGaze2 = _listBattleShips[index].spPointOfGaze;
				float y2 = spPointOfGaze2.y;
				Vector3 spPointOfGaze3 = _listBattleShips[index].spPointOfGaze;
				result = new Vector3(x2, y2, spPointOfGaze3.z + vector.z);
			}
			return result;
		}

		protected virtual List<Vector3> CalcCloseUpCamPos(Vector3 from, Vector3 to, bool isProtect)
		{
			List<Vector3> list;
			if (!isProtect)
			{
				list = new List<Vector3>();
				list.Add(Vector3.Lerp(from, to, 0.98f));
				list.Add(to);
				return list;
			}
			Vector3 item = Vector3.Lerp(from, to, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[0]);
			Vector3 item2 = Vector3.Lerp(from, to, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[1]);
			Vector3 spPointOfGaze = _listBattleShips[2].spPointOfGaze;
			item.y = spPointOfGaze.y;
			Vector3 spPointOfGaze2 = _listBattleShips[2].spPointOfGaze;
			item2.y = spPointOfGaze2.y;
			Vector3 position = _listBattleShips[1].transform.position;
			float x = position.x;
			Vector3 spPointOfGaze3 = _listBattleShips[1].spPointOfGaze;
			float num = x - spPointOfGaze3.x;
			Vector3 position2 = _listBattleShips[2].transform.position;
			float x2 = position2.x;
			Vector3 spPointOfGaze4 = _listBattleShips[2].spPointOfGaze;
			float num2 = num - (x2 - spPointOfGaze4.x);
			item.x += num2;
			item2.x += num2;
			list = new List<Vector3>();
			list.Add(Vector3.Lerp(from, to, 0.98f));
			list.Add(to);
			list.Add(item);
			list.Add(item2);
			return list;
		}

		protected virtual Vector3 CalcProtecterPos(Vector3 close4)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			Vector3 vector = Vector3.Lerp(_listBattleShips[1].spPointOfGaze, close4, 0.58f);
			Vector3 position = _listBattleShips[1].transform.position;
			Vector3 seaLevelPos = battleField.seaLevelPos;
			position.y = seaLevelPos.y;
			position.z = vector.z;
			return position;
		}

		protected virtual void SetFieldCamera(bool isAttacker, Vector3 camPos, Vector3 lookPos)
		{
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			if (isAttacker)
			{
				battleFieldCamera.motionBlur.enabled = false;
				battleFieldCamera.motionBlur.blurAmount = 0.65f;
				battleFieldCamera.transform.position = camPos;
				battleFieldCamera.LookAt(lookPos);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
				battleFieldCamera.cullingMask = (Generics.Layers.FocusDim | Generics.Layers.UnRefrectEffects);
				battleFieldCamera.clearFlags = CameraClearFlags.Depth;
			}
			else
			{
				battleFieldCamera.motionBlur.enabled = false;
				battleFieldCamera.transform.position = camPos;
				battleFieldCamera.LookAt(lookPos);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			}
		}

		protected virtual void RotateFocusTowardsTarget2RotateFieldCam(Vector3 target)
		{
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
			{
				BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras[0];
				float num = (!_listBattleShips[0].shipModel.IsFriend()) ? (-180f) : 180f;
				Quaternion eyeRotation = cam.eyeRotation;
				float x = eyeRotation.x;
				float y = num;
				Quaternion eyeRotation2 = cam.eyeRotation;
				Vector3 vector = new Vector3(x, y, eyeRotation2.z);
				cam.transform.LTRotateAround(Vector3.up, num, 0.666f).setEase(LeanTweenType.easeInQuad).setOnComplete((Action)delegate
				{
					cam.LookAt(target);
					cam.ReqViewMode(CameraActor.ViewMode.FixChasing);
				});
			});
		}

		protected void RotateFocusTowardTarget2MoveFieldCam_version2(Vector3 target, Action callback)
		{
		}

		protected virtual void RotateFocusTowardTarget2MoveFieldCam(Vector3 target, Action callback)
		{
			BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			Vector3 vector = Vector3.Lerp(cam.eyePosition, target, 0.2f);
			vector.x = target.x;
			vector.y = target.y;
			cam.transform.LTMoveX(vector.x, 0.666f).setOnStart(delegate
			{
				OnCameraRotateStart();
			}).setEase(LeanTweenType.easeInQuad)
				.setOnUpdate(delegate(float x)
				{
					cam.transform.positionX(x);
				});
			cam.transform.LTMoveY(vector.y, 0.666f).setEase(LeanTweenType.easeInQuad).setOnUpdate(delegate(float x)
			{
				cam.transform.positionY(x);
			});
			cam.transform.LTMoveZ(vector.z, 1.1655f).setEase(LeanTweenType.easeInQuad).setOnUpdate(delegate(float x)
			{
				cam.transform.positionZ(x);
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref callback);
				});
		}

		protected virtual void SetDimCamera(bool isAttacker, Transform syncTarget)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldDimCamera fieldDimCamera = battleCameras.fieldDimCamera;
			if (isAttacker)
			{
				fieldDimCamera.syncTarget = syncTarget;
				fieldDimCamera.SyncTransform();
				fieldDimCamera.cullingMask = battleCameras.GetDefaultLayers();
				fieldDimCamera.maskAlpha = 0f;
				fieldDimCamera.isCulling = true;
				fieldDimCamera.isSync = true;
			}
		}

		protected virtual void GraAddDimCameraMaskAlpha(float time)
		{
			BattleFieldDimCamera fieldDimCamera = BattleTaskManager.GetBattleCameras().fieldDimCamera;
			if (time == 0f)
			{
				fieldDimCamera.maskAlpha = 0.75f;
			}
			else
			{
				fieldDimCamera.SetMaskPlaneAlpha(0f, 0.75f, time);
			}
		}

		protected virtual void GraSubDimCameraMaskAlpha(float time)
		{
			BattleFieldDimCamera fieldDimCamera = BattleTaskManager.GetBattleCameras().fieldDimCamera;
			fieldDimCamera.SetMaskPlaneAlpha(0.75f, 0f, time);
		}

		protected virtual void PlayShipAnimation(UIBattleShip ship, UIBattleShip.AnimationName iName, float delay)
		{
			Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(delegate
			{
				SoundUtils.PlayShellingSE(_listBattleShips[0].shipModel);
				ShipUtils.PlayShellingVoive(ship.shipModel);
				ship.PlayShipAnimation(iName);
			});
		}

		protected virtual void PlayShellingSlot(SlotitemModel_Battle model, BaseProdLine.AnimationName iName, bool isFriend, float delay)
		{
			if (model != null)
			{
				Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(delegate
				{
					ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
					prodShellingSlotLine.SetSlotData(model, isFriend);
					prodShellingSlotLine.Play(iName, isFriend, null);
				});
			}
		}

		protected virtual void PlayShellingSlot(SlotitemModel_Battle[] models, BaseProdLine.AnimationName iName, bool isFriend, float delay)
		{
			if (models != null)
			{
				Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(delegate
				{
					ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
					prodShellingSlotLine.SetSlotData(new List<SlotitemModel_Battle>(models), isFriend);
					prodShellingSlotLine.Play(iName, isFriend, null);
				});
			}
		}

		protected virtual void PlayProtectDefender(List<Vector3> camTargetPos)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras[0];
			fieldCam.transform.LTMove(camTargetPos[1], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1]).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE[1]);
			Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate
			{
				fieldCam.transform.LTCancel();
				SetProtecterLayer();
				Vector3 to = CalcProtecterPos(camTargetPos[3]);
				_listBattleShips[2].transform.positionZ(to.z);
				_listBattleShips[2].transform.LTMove(to, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0] * 1.2f).setEase(LeanTweenType.easeOutSine);
				fieldCam.transform.LTMove(camTargetPos[2], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0]).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE[0]).setOnComplete((Action)delegate
				{
					PlayDefenderEffect(_listBattleShips[2], _listBattleShips[2].pointOfGaze, fieldCam, 0.5f);
					ChkDamagedStateFmAnticipating(camTargetPos[3]);
				});
			});
		}

		protected void PlayDefenderEffect(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera, float delay)
		{
			Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(delegate
			{
				switch (hitState)
				{
				case HitState.CriticalDamage:
					PlayDefenderCritical(ship, defenderPos, fieldCamera);
					break;
				case HitState.NomalDamage:
					PlayDefenderNormal(ship, defenderPos, fieldCamera);
					break;
				case HitState.Gard:
					PlayDefenderGard(ship, defenderPos, fieldCamera);
					break;
				case HitState.Miss:
					PlayDefenderMiss(ship, defenderPos, fieldCamera);
					break;
				}
			});
		}

		protected virtual void PlayDefenderCritical(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			SoundUtils.PlayDamageSE(HitState.CriticalDamage, isTorpedo: false);
			ParticleSystem explosionB = BattleTaskManager.GetParticleFile().explosionB2;
			((Component)explosionB).transform.parent = particleParent;
			((Component)explosionB).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)explosionB).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 0.9f);
			((Component)explosionB).SetActive(isActive: true);
			explosionB.Play();
			PlayHpGaugeDamage(ship, hitState);
			fieldCamera.cameraShake.ShakeRot(null);
			PlayDamageVoice(ship, _clsHougekiModel.Defender.DamageEventAfter);
		}

		protected virtual void PlayDefenderNormal(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem explosionB = BattleTaskManager.GetParticleFile().explosionB2;
			((Component)explosionB).transform.parent = particleParent;
			((Component)explosionB).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)explosionB).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 0.9f);
			((Component)explosionB).SetActive(isActive: true);
			explosionB.Play();
			SoundUtils.PlayDamageSE(HitState.NomalDamage, isTorpedo: false);
			PlayDamageVoice(ship, _clsHougekiModel.Defender.DamageEventAfter);
			PlayHpGaugeDamage(ship, hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected virtual void PlayDefenderGard(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem explosionB3WhiteSmoke = BattleTaskManager.GetParticleFile().explosionB3WhiteSmoke;
			((Component)explosionB3WhiteSmoke).transform.parent = particleParent;
			((Component)explosionB3WhiteSmoke).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)explosionB3WhiteSmoke).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 0.9f);
			((Component)explosionB3WhiteSmoke).SetActive(isActive: true);
			explosionB3WhiteSmoke.Play();
			SoundUtils.PlayDamageSE(HitState.Gard, isTorpedo: false);
			PlayDamageVoice(ship, _clsHougekiModel.Defender.DamageEventAfter);
			PlayHpGaugeDamage(ship, hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected virtual void PlayDefenderMiss(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			SoundUtils.PlayDamageSE(HitState.Miss, isTorpedo: false);
			ParticleSystem splashMiss = BattleTaskManager.GetParticleFile().splashMiss;
			((Component)splashMiss).transform.parent = particleParent;
			((Component)splashMiss).SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), includeChildren: true);
			((Component)splashMiss).transform.position = Vector3.Lerp(fieldCamera.transform.position, defenderPos, 1f);
			((Component)splashMiss).transform.positionY();
			Transform transform = ((Component)splashMiss).transform;
			Vector3 localPosition = ((Component)splashMiss).transform.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = ((Component)splashMiss).transform.localPosition;
			transform.localPosition = new Vector3(x, localPosition2.y, (!ship.shipModel.IsFriend()) ? (-15f) : 15f);
			((Component)splashMiss).SetActive(isActive: true);
			splashMiss.Play();
			PlayHpGaugeDamage(ship, hitState);
		}

		protected void PlayDamageVoice(UIBattleShip ship, DamagedStates iStates)
		{
			if (iStates != DamagedStates.Tyuuha && iStates != DamagedStates.Taiha)
			{
				ShipUtils.PlayMildCaseVoice(_clsHougekiModel.Defender);
			}
		}

		protected virtual void PlayHpGaugeDamage(UIBattleShip ship, HitState iState)
		{
			BattleHitStatus status = BattleHitStatus.Normal;
			switch (iState)
			{
			case HitState.CriticalDamage:
				status = BattleHitStatus.Clitical;
				break;
			case HitState.Miss:
				status = BattleHitStatus.Miss;
				break;
			}
			if (_clsHougekiModel != null)
			{
				UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
				circleHPGauge.SetHPGauge(_clsHougekiModel.Defender.MaxHp, _clsHougekiModel.Defender.HpBefore, _clsHougekiModel.Defender.HpAfter, _clsHougekiModel.GetDamage(), status, _clsHougekiModel.Defender.IsFriend());
				Vector3 damagePosition = (!_clsHougekiModel.Defender.IsFriend()) ? new Vector3(280f, -125f, 0f) : new Vector3(-500f, -125f, 0f);
				circleHPGauge.SetDamagePosition(damagePosition);
				circleHPGauge.Play(delegate
				{
				});
			}
		}

		protected virtual void ChkAttackCntForNextPhase()
		{
			if (isSkipAttack)
			{
				_clsState.AddState(InitDefenderFocus, UpdateDefenderFocus);
			}
			else
			{
				_clsState.AddState(InitRotateFocus, UpdateRotateFocus);
			}
		}

		protected virtual void ChkDamagedStateFmAnticipating(Vector3 closeUpPos)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
			switch (_clsHougekiModel.Defender.DamageEventAfter)
			{
			case DamagedStates.Gekichin:
			case DamagedStates.Youin:
			case DamagedStates.Megami:
			{
				bool isFriend = _listBattleShips[1].shipModel.IsFriend();
				ShellingProdSubject index = (!isProtect) ? ShellingProdSubject.Defender : ShellingProdSubject.Protector;
				_listBattleShips[(int)index].PlayProdSinking(null);
				battleFieldCamera.transform.LTMove(closeUpPos, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[2]).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE[1]).setOnComplete((Action)delegate
				{
					if (!isFriend)
					{
						OnFinished();
					}
				});
				if (isFriend)
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
					{
						ProdSinking prodSinking = BattleTaskManager.GetPrefabFile().prodSinking;
						prodSinking.SetSinkingData(_clsHougekiModel.Defender);
						prodSinking.Play(delegate
						{
							BattleTaskManager.GetPrefabFile().circleHPGauge.transform.localScale = Vector3.zero;
						}, delegate
						{
						}, delegate
						{
							OnFinished();
						});
						BattleTaskManager.GetPrefabFile().circleHPGauge.transform.localScale = Vector3.zero;
					});
				}
				break;
			}
			case DamagedStates.Tyuuha:
			case DamagedStates.Taiha:
				battleFieldCamera.transform.LTMove(closeUpPos, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1]).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE[1]).setOnComplete((Action)delegate
				{
					ShellingProdSubject index2 = (!isProtect) ? ShellingProdSubject.Defender : ShellingProdSubject.Protector;
					if (_listBattleShips[(int)index2].shipModel.IsFriend())
					{
						DamagedStates damageEventAfter = _clsHougekiModel.Defender.DamageEventAfter;
						ProdDamageCutIn.DamageCutInType iType = (damageEventAfter == DamagedStates.Taiha) ? ProdDamageCutIn.DamageCutInType.Heavy : ProdDamageCutIn.DamageCutInType.Moderate;
						ProdDamageCutIn prodDamageCutIn = BattleTaskManager.GetPrefabFile().prodDamageCutIn;
						prodDamageCutIn.SetShipData(new List<ShipModel_Defender>
						{
							_clsHougekiModel.Defender
						}, iType);
						prodDamageCutIn.Play(iType, delegate
						{
							BattleTaskManager.GetPrefabFile().circleHPGauge.transform.localScale = Vector3.zero;
						}, delegate
						{
							BattleTaskManager.GetBattleShips().UpdateDamageAll(_clsHougekiModel);
							OnFinished();
						});
					}
					else
					{
						OnFinished();
					}
				});
				break;
			case DamagedStates.None:
			case DamagedStates.Shouha:
				battleFieldCamera.transform.LTMove(closeUpPos, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1]).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE[1]).setOnComplete((Action)delegate
				{
					OnFinished();
				});
				break;
			}
		}

		protected virtual void OnFinished()
		{
			if (_clsHougekiModel.Defender.DamageEventAfter == DamagedStates.Megami || _clsHougekiModel.Defender.DamageEventAfter == DamagedStates.Youin)
			{
				BattleTaskManager.GetBattleShips().Restored(_clsHougekiModel.Defender);
			}
			else
			{
				BattleTaskManager.GetBattleShips().UpdateDamageAll(_clsHougekiModel);
			}
			Reset();
			_isFinished = true;
			Dlg.Call(ref _actOnFinished);
		}
	}
}
