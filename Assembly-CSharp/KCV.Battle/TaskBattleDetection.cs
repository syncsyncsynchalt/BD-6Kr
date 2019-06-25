using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using LT.Tweening;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleDetection : BaseBattleTask
	{
		private SakutekiModel _clsSakuteki;

		private ParticleSystem _psDetectionRipple;

		private ProdDetectionCutIn _prodDetectionCutIn;

		private ProdDetectionResultCutIn _prodDetectionResultCutIn;

		private ProdDetectionResultCutIn.AnimationList _iResult;

		private Tuple<Vector3, float> _tpFocusPoint;

		protected override bool Init()
		{
			_clsSakuteki = BattleTaskManager.GetBattleManager().GetSakutekiData();
			if (_clsSakuteki == null || !BattleTaskManager.GetBattleManager().IsExistSakutekiData())
			{
				ImmediateTermination();
				EndPhase(BattleUtils.NextPhase(BattlePhase.Detection));
				return true;
			}
			_clsState = new StatementMachine();
			_clsState.AddState(InitMoveCameraTo2D, UpdateMoveCameraTo2D);
			Transform transform = BattleTaskManager.GetBattleCameras().cutInCamera.transform;
			_prodDetectionCutIn = ProdDetectionCutIn.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdDetectionCutIn).GetComponent<ProdDetectionCutIn>(), transform, _clsSakuteki);
			_prodDetectionResultCutIn = ProdDetectionResultCutIn.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdDetectionResultCutIn).GetComponent<ProdDetectionResultCutIn>(), transform, _clsSakuteki);
			_iResult = _prodDetectionResultCutIn.detectionResult;
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del(ref _clsSakuteki);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
			Mem.DelComponentSafe<ParticleSystem>(ref _psDetectionRipple);
			Mem.DelComponentSafe(ref _prodDetectionCutIn);
			Mem.Del(ref _tpFocusPoint);
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.Detection);
		}

		private bool InitMoveCameraTo2D(object data)
		{
			BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			ProdDetectionStartCutIn pdsc = ProdDetectionStartCutIn.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdDetectionStartCutIn).GetComponent<ProdDetectionStartCutIn>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
			ShipModel_Battle detectionPrimaryShip = ShipUtils.GetDetectionPrimaryShip(_clsSakuteki.planes_f, isFriend: true);
			UIBattleShip uIBattleShip = (detectionPrimaryShip == null) ? battleShips.flagShipFriend : battleShips.dicFriendBattleShips[detectionPrimaryShip.Index];
			Vector3 vector = Mathe.NormalizeDirection(uIBattleShip.pointOfGaze, Vector3.zero) * 30f;
			Vector3 pointOfGaze = uIBattleShip.pointOfGaze;
			float x = pointOfGaze.x;
			Vector3 pointOfGaze2 = uIBattleShip.pointOfGaze;
			float y = pointOfGaze2.y;
			Vector3 pointOfGaze3 = uIBattleShip.pointOfGaze;
			Vector3 fixChasingCamera = new Vector3(x, y, pointOfGaze3.z + vector.z);
			cam.pointOfGaze = uIBattleShip.pointOfGaze;
			cam.ReqViewMode(CameraActor.ViewMode.FixChasing);
			cam.SetFixChasingCamera(fixChasingCamera);
			Vector3 pointOfGaze4 = uIBattleShip.pointOfGaze;
			float x2 = pointOfGaze4.x;
			Vector3 pointOfGaze5 = uIBattleShip.pointOfGaze;
			Vector3 endCamPos = new Vector3(x2, 50f, pointOfGaze5.z + vector.z * 6f);
			Transform transform = uIBattleShip.transform;
			Vector3 position = BattleTaskManager.GetBattleShips().dicFriendBattleShips[0].transform.position;
			_psDetectionRipple = Util.Instantiate(ParticleFile.Load(ParticleFileInfos.BattlePSDetectionRipple)).GetComponent<ParticleSystem>();
			((Component)_psDetectionRipple).transform.parent = transform;
			((Component)_psDetectionRipple).transform.position = new Vector3(position.x, position.y + 0.01f, position.z);
			_psDetectionRipple.Play();
			pdsc.Play().Subscribe(delegate
			{
				cam.transform.LTMove(endCamPos, 1.95f).setEase(LeanTweenType.easeInOutCubic);
				Mem.DelComponentSafe(ref pdsc);
			});
			return false;
		}

		private bool UpdateMoveCameraTo2D(object data)
		{
			ProdCloud prodCloud = BattleTaskManager.GetPrefabFile().prodCloud;
			if (!prodCloud.isPlaying)
			{
				BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
				Vector3 eyePosition = battleFieldCamera.eyePosition;
				float y = eyePosition.y;
				Vector3 vector = Vector3.Lerp(Vector3.zero, Vector3.up * 50f, 0.15f);
				if (y >= vector.y)
				{
					prodCloud.Play(ProdCloud.AnimationList.ProdCloudOut, delegate
					{
						if (_prodDetectionCutIn.isAircraft)
						{
							_prodDetectionCutIn.Play(delegate
							{
								if (_iResult == ProdDetectionResultCutIn.AnimationList.DetectionSucces || _iResult == ProdDetectionResultCutIn.AnimationList.DetectionLost)
								{
									_clsState.AddState(InitDetectionResultCutIn, UpdateDetectionResultCutIn);
								}
								else
								{
									_clsState.AddState(InitEnemyFleetFocus, UpdateEnemyFleetFocus);
								}
							}, null);
						}
						else if (_iResult == ProdDetectionResultCutIn.AnimationList.DetectionSucces)
						{
							_clsState.AddState(InitDetectionResultCutIn, UpdateDetectionResultCutIn);
						}
						else
						{
							_clsState.AddState(InitEnemyFleetFocus, UpdateEnemyFleetFocus);
							InitCameraSettingsForEnemyFocus();
						}
					}, delegate
					{
						if (_prodDetectionCutIn.isAircraft || (!_prodDetectionCutIn.isAircraft && _iResult == ProdDetectionResultCutIn.AnimationList.DetectionSucces))
						{
							InitCameraSettingsForEnemyFocus();
						}
					});
					return true;
				}
			}
			return false;
		}

		private void InitCameraSettingsForEnemyFocus()
		{
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			battleFieldCamera.transform.LTCancel();
			Vector3 vector = battleFieldCamera.pointOfGaze = BattleTaskManager.GetBattleShips().flagShipEnemy.pointOfGaze;
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.FixChasing);
			Vector3 fixChasingCamera = CalcCameraFleetFocusPos(_iResult);
			battleFieldCamera.SetFixChasingCamera(fixChasingCamera);
		}

		private bool InitDetectionResultCutIn(object data)
		{
			SetEnemyShipsDrawType(_iResult);
			_prodDetectionResultCutIn.Play(OnDetectionResultCutInFinished, null);
			return false;
		}

		private bool UpdateDetectionResultCutIn(object data)
		{
			return true;
		}

		private void OnDetectionResultCutInFinished()
		{
			_clsState.AddState(InitEnemyFleetFocus, UpdateEnemyFleetFocus);
		}

		private bool InitEnemyFleetFocus(object data)
		{
			SetEnemyShipsDrawType(_iResult);
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			Vector3 pointOfGaze = BattleTaskManager.GetBattleShips().flagShipEnemy.pointOfGaze;
			Vector3 vector = Vector3.Lerp(battleFieldCamera.eyePosition, pointOfGaze, 0.3f);
			battleFieldCamera.transform.LTMove(vector, 2.7f).setEase(LeanTweenType.linear);
			_tpFocusPoint = new Tuple<Vector3, float>(vector, Vector3.Distance(Vector3.Lerp(battleFieldCamera.eyePosition, vector, 0.7f), vector));
			ProdCloud prodCloud = BattleTaskManager.GetPrefabFile().prodCloud;
			prodCloud.Play(GetFleetFocusAnim(_iResult), null, null);
			return false;
		}

		private bool UpdateEnemyFleetFocus(object data)
		{
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			if (Vector3.Distance(battleFieldCamera.eyePosition, _tpFocusPoint.Item1) <= _tpFocusPoint.Item2)
			{
				EndPhase(BattleUtils.NextPhase(BattlePhase.Detection));
				return true;
			}
			return false;
		}

		private Vector3 CalcCameraFleetFocusPos(ProdDetectionResultCutIn.AnimationList iList)
		{
			Vector3 result = Vector3.zero;
			switch (iList)
			{
			case ProdDetectionResultCutIn.AnimationList.DetectionLost:
			case ProdDetectionResultCutIn.AnimationList.DetectionNotFound:
			case ProdDetectionResultCutIn.AnimationList.DetectionSucces:
			{
				Vector3 pointOfGaze = BattleTaskManager.GetBattleShips().flagShipEnemy.pointOfGaze;
				result = BattleDefines.FLEET_ADVENT_START_CAM_POS[1];
				result.y = pointOfGaze.y;
				break;
			}
			}
			return result;
		}

		private ProdCloud.AnimationList GetFleetFocusAnim(ProdDetectionResultCutIn.AnimationList iList)
		{
			ProdCloud.AnimationList result = ProdCloud.AnimationList.ProdCloudIn;
			switch (iList)
			{
			case ProdDetectionResultCutIn.AnimationList.DetectionSucces:
				result = ProdCloud.AnimationList.ProdCloudIn;
				break;
			case ProdDetectionResultCutIn.AnimationList.DetectionLost:
			case ProdDetectionResultCutIn.AnimationList.DetectionNotFound:
				result = ProdCloud.AnimationList.ProdCloudInNotFound;
				break;
			}
			return result;
		}

		private void SetEnemyShipsDrawType(ProdDetectionResultCutIn.AnimationList iList)
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			switch (iList)
			{
			case ProdDetectionResultCutIn.AnimationList.DetectionSucces:
				battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
				break;
			case ProdDetectionResultCutIn.AnimationList.DetectionLost:
			case ProdDetectionResultCutIn.AnimationList.DetectionNotFound:
				battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Silhouette);
				break;
			}
		}
	}
}
