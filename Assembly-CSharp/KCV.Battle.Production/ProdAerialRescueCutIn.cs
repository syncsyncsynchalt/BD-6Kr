using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialRescueCutIn : MonoBehaviour
	{
		public enum RescueShipType
		{
			Defender,
			Protector
		}

		public List<UIBattleShip> _listBattleShip;

		private List<Vector3> _camTargetPos;

		private Action _actCallback;

		public Vector3 calcDefenderCamStartPos
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
				Vector3 spPointOfGaze = _listBattleShip[0].spPointOfGaze;
				float x = spPointOfGaze.x;
				Vector3 spPointOfGaze2 = _listBattleShip[0].spPointOfGaze;
				return new Vector3(x, spPointOfGaze2.y, 0f);
			}
		}

		public void _init()
		{
			_listBattleShip = new List<UIBattleShip>();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _actCallback);
			Mem.DelListSafe(ref _listBattleShip);
			Mem.DelListSafe(ref _camTargetPos);
		}

		public void initShipList()
		{
			if (_listBattleShip != null)
			{
				_listBattleShip.Clear();
			}
			_listBattleShip = null;
		}

		public void AddShipList(UIBattleShip defenderShip, UIBattleShip protecterShip)
		{
			if (_listBattleShip != null)
			{
				initShipList();
				_init();
			}
			_listBattleShip.Add(defenderShip);
			_listBattleShip.Add(protecterShip);
			_listBattleShip[0].standingPositionType = StandingPositionType.Advance;
		}

		public void setFieldCamera()
		{
			Vector3 calcDefenderCamStartPo = calcDefenderCamStartPos;
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			friendFieldCamera.motionBlur.enabled = true;
			friendFieldCamera.LookAt(_listBattleShip[0].spPointOfGaze);
			friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
		}

		public void Play(Action callBack)
		{
			_actCallback = callBack;
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.friendFieldCamera;
			setFieldCamera();
			_camTargetPos = calcCloseUpCamPos(fieldCam.transform.position);
			fieldCam.transform.MoveTo(_camTargetPos[0], 0.6f, iTween.EaseType.linear, delegate
			{
				fieldCam.motionBlur.enabled = false;
				PlayProtectDefender();
			});
		}

		public void PlayProtectDefender()
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.friendFieldCamera;
			fieldCam.transform.MoveTo(_camTargetPos[1], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1], iTween.EaseType.linear, null);
			Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate
			{
				fieldCam.transform.iTweenStop();
				Vector3 target = calcProtecterPos(_camTargetPos[3]);
				_listBattleShip[1].transform.positionZ(target.z);
				_listBattleShip[1].transform.MoveTo(target, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0] * 1.2f, iTween.EaseType.easeOutSine, null);
				fieldCam.transform.MoveTo(_camTargetPos[2], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0], iTween.EaseType.linear, delegate
				{
					fieldCam.transform.MoveTo(_camTargetPos[3], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1], iTween.EaseType.linear, delegate
					{
						_listBattleShip[0].standingPositionType = StandingPositionType.OneRow;
						_listBattleShip[1].standingPositionType = StandingPositionType.OneRow;
						_actCallback();
					});
				});
			});
		}

		protected void setProtecterLayer()
		{
			_listBattleShip[1].layer = Generics.Layers.FocusDim;
		}

		public Vector3 calcCamTargetPos(bool isPointOfGaze)
		{
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? _listBattleShip[0].spPointOfGaze : _listBattleShip[0].pointOfGaze, friendFieldCamera.eyePosition) * 10f;
			Vector3 result;
			if (isPointOfGaze)
			{
				Vector3 pointOfGaze = _listBattleShip[0].pointOfGaze;
				float x = pointOfGaze.x + vector.x;
				Vector3 pointOfGaze2 = _listBattleShip[0].pointOfGaze;
				float y = pointOfGaze2.y;
				Vector3 pointOfGaze3 = _listBattleShip[0].pointOfGaze;
				result = new Vector3(x, y, pointOfGaze3.z + vector.z);
			}
			else
			{
				Vector3 spPointOfGaze = _listBattleShip[0].spPointOfGaze;
				float x2 = spPointOfGaze.x + vector.x;
				Vector3 spPointOfGaze2 = _listBattleShip[0].spPointOfGaze;
				float y2 = spPointOfGaze2.y;
				Vector3 spPointOfGaze3 = _listBattleShip[0].spPointOfGaze;
				result = new Vector3(x2, y2, spPointOfGaze3.z + vector.z);
			}
			return result;
		}

		protected virtual Vector3 calcProtecterPos(Vector3 close4)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			Vector3 vector = Vector3.Lerp(_listBattleShip[0].spPointOfGaze, close4, 0.58f);
			Vector3 position = _listBattleShip[0].transform.position;
			float x = position.x;
			Vector3 spPointOfGaze = _listBattleShip[0].spPointOfGaze;
			float num = x - spPointOfGaze.x;
			Vector3 position2 = _listBattleShip[1].transform.position;
			float x2 = position2.x;
			Vector3 spPointOfGaze2 = _listBattleShip[1].spPointOfGaze;
			float num2 = num - (x2 - spPointOfGaze2.x);
			Vector3 position3 = _listBattleShip[0].transform.position;
			Vector3 seaLevelPos = battleField.seaLevelPos;
			position3.y = seaLevelPos.y;
			position3.z = vector.z;
			return position3;
		}

		public List<Vector3> calcCloseUpCamPos(Vector3 from)
		{
			Vector3 vector = calcCamTargetPos(isPointOfGaze: false);
			Vector3 item = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[0]);
			Vector3 item2 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[1]);
			Vector3 spPointOfGaze = _listBattleShip[1].spPointOfGaze;
			item.y = spPointOfGaze.y;
			Vector3 spPointOfGaze2 = _listBattleShip[1].spPointOfGaze;
			item2.y = spPointOfGaze2.y;
			Vector3 position = _listBattleShip[0].transform.position;
			float x = position.x;
			Vector3 spPointOfGaze3 = _listBattleShip[0].spPointOfGaze;
			float num = x - spPointOfGaze3.x;
			Vector3 position2 = _listBattleShip[1].transform.position;
			float x2 = position2.x;
			Vector3 spPointOfGaze4 = _listBattleShip[1].spPointOfGaze;
			float num2 = num - (x2 - spPointOfGaze4.x);
			item.x += num2;
			item2.x += num2;
			List<Vector3> list = new List<Vector3>();
			list.Add(Vector3.Lerp(from, vector, 0.98f));
			list.Add(vector);
			list.Add(item);
			list.Add(item2);
			return list;
		}

		public static ProdAerialRescueCutIn Instantiate(ProdAerialRescueCutIn prefab, Transform parent)
		{
			ProdAerialRescueCutIn prodAerialRescueCutIn = UnityEngine.Object.Instantiate(prefab);
			prodAerialRescueCutIn.transform.parent = parent;
			prodAerialRescueCutIn.transform.localPosition = Vector3.zero;
			prodAerialRescueCutIn.transform.localScale = Vector3.one;
			prodAerialRescueCutIn._init();
			return prodAerialRescueCutIn;
		}
	}
}
