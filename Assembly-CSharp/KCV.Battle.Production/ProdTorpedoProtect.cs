using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoProtect : MonoBehaviour
	{
		public enum RescueShipType
		{
			Defender,
			Protector
		}

		public List<UIBattleShip> _listBattleShipF;

		public List<UIBattleShip> _listBattleShipE;

		private bool[] _isProtect;

		private List<Vector3> _camTargetPosF;

		private List<Vector3> _camTargetPosE;

		private Action _actCallback;

		public Vector3 calcDefenderCamStartPosF
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
				Vector3 spPointOfGaze = _listBattleShipF[0].spPointOfGaze;
				float x = spPointOfGaze.x;
				Vector3 spPointOfGaze2 = _listBattleShipF[0].spPointOfGaze;
				return new Vector3(x, spPointOfGaze2.y, 0f);
			}
		}

		public Vector3 calcDefenderCamStartPosE
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
				Vector3 spPointOfGaze = _listBattleShipE[0].spPointOfGaze;
				float x = spPointOfGaze.x;
				Vector3 spPointOfGaze2 = _listBattleShipE[0].spPointOfGaze;
				return new Vector3(x, spPointOfGaze2.y, 0f);
			}
		}

		public void _init()
		{
			_isProtect = new bool[2];
			_listBattleShipF = new List<UIBattleShip>();
			_listBattleShipE = new List<UIBattleShip>();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _actCallback);
			Mem.Del(ref _isProtect);
			Mem.DelListSafe(ref _listBattleShipF);
			Mem.DelListSafe(ref _listBattleShipE);
			Mem.DelListSafe(ref _camTargetPosF);
			Mem.DelListSafe(ref _camTargetPosE);
		}

		public void initShipList(FleetType type)
		{
			if (type == FleetType.Friend)
			{
				if (_listBattleShipF != null)
				{
					_listBattleShipF.Clear();
					_listBattleShipF = new List<UIBattleShip>();
					_isProtect[0] = false;
				}
			}
			else if (_listBattleShipE != null)
			{
				_listBattleShipE.Clear();
				_listBattleShipE = new List<UIBattleShip>();
				_isProtect[1] = false;
			}
		}

		public void AddShipList(UIBattleShip defenderShip, UIBattleShip protecterShip, FleetType type)
		{
			initShipList(type);
			if (type == FleetType.Friend)
			{
				_isProtect[0] = true;
				_listBattleShipF.Add(defenderShip);
				_listBattleShipF.Add(protecterShip);
				_listBattleShipF[0].standingPositionType = StandingPositionType.Advance;
			}
			else
			{
				_isProtect[1] = true;
				_listBattleShipE.Add(defenderShip);
				_listBattleShipE.Add(protecterShip);
				_listBattleShipE[0].standingPositionType = StandingPositionType.Advance;
			}
		}

		public void setFieldCamera(BattleFieldCamera camF, BattleFieldCamera camE)
		{
			if (_isProtect[0])
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // Vector3 calcDefenderCamStartPosF = calcDefenderCamStartPosF;

				camF.motionBlur.enabled = true;
				camF.LookAt(_listBattleShipF[0].spPointOfGaze);
				camF.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			}
			if (_isProtect[1])
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // Vector3 calcDefenderCamStartPosE = calcDefenderCamStartPosE;

				camE.motionBlur.enabled = true;
				camE.LookAt(_listBattleShipE[0].spPointOfGaze);
				camE.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			}
		}

		public void Play(Action callBack)
		{
			_actCallback = callBack;
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCamF = battleCameras.friendFieldCamera;
			BattleFieldCamera fieldCamE = battleCameras.enemyFieldCamera;
			setFieldCamera(fieldCamF, fieldCamE);
			if (_isProtect[0])
			{
				_camTargetPosF = calcCloseUpCamPos(fieldCamF.transform.position, FleetType.Friend);
				fieldCamF.transform.MoveTo(_camTargetPosF[0], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0], iTween.EaseType.linear, delegate
				{
					fieldCamF.motionBlur.enabled = false;
					PlayProtectDefender(fieldCamF, FleetType.Friend);
				});
			}
			if (_isProtect[1])
			{
				_camTargetPosE = calcCloseUpCamPos(fieldCamE.transform.position, FleetType.Enemy);
				fieldCamE.transform.MoveTo(_camTargetPosE[0], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0], iTween.EaseType.linear, delegate
				{
					fieldCamE.motionBlur.enabled = false;
					PlayProtectDefender(fieldCamE, FleetType.Enemy);
				});
			}
		}

		public void PlayProtectDefender(BattleFieldCamera cam, FleetType type)
		{
			BattleTaskManager.GetBattleCameras();
			if (type == FleetType.Friend)
			{
				cam.transform.MoveTo(_camTargetPosF[1], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1], iTween.EaseType.linear, null);
				Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate
				{
					cam.transform.iTweenStop();
					Vector3 target2 = calcProtecterPos(_camTargetPosF[3], FleetType.Friend);
					_listBattleShipF[1].transform.positionZ(target2.z);
					_listBattleShipF[1].transform.MoveTo(target2, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0] * 1.2f, iTween.EaseType.easeOutSine, null);
					cam.transform.MoveTo(_camTargetPosF[2], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0], iTween.EaseType.linear, delegate
					{
						cam.transform.MoveTo(_camTargetPosF[3], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1], iTween.EaseType.linear, delegate
						{
							_listBattleShipF.ForEach(delegate(UIBattleShip x)
							{
								x.standingPositionType = StandingPositionType.OneRow;
							});
							_actCallback();
						});
					});
				});
			}
			else
			{
				cam.transform.MoveTo(_camTargetPosE[1], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1], iTween.EaseType.linear, null);
				Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate
				{
					cam.transform.iTweenStop();
					Vector3 target = calcProtecterPos(_camTargetPosE[3], FleetType.Enemy);
					_listBattleShipE[1].transform.positionZ(target.z);
					_listBattleShipE[1].transform.MoveTo(target, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0] * 1.2f, iTween.EaseType.easeOutSine, null);
					cam.transform.MoveTo(_camTargetPosE[2], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[0], iTween.EaseType.linear, delegate
					{
						cam.transform.MoveTo(_camTargetPosE[3], BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[1], iTween.EaseType.linear, delegate
						{
							_listBattleShipE.ForEach(delegate(UIBattleShip x)
							{
								x.standingPositionType = StandingPositionType.OneRow;
							});
							_actCallback();
						});
					});
				});
			}
		}

		protected void setProtecterLayer()
		{
			if (_isProtect[0])
			{
				_listBattleShipF[1].layer = Generics.Layers.FocusDim;
			}
			if (_isProtect[1])
			{
				_listBattleShipE[1].layer = Generics.Layers.FocusDim;
			}
		}

		public Vector3 calcCamTargetPos(bool isPointOfGaze, FleetType type)
		{
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			BattleFieldCamera friendFieldCamera2 = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			if (type == FleetType.Friend)
			{
				Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? _listBattleShipF[0].spPointOfGaze : _listBattleShipF[0].pointOfGaze, friendFieldCamera2.eyePosition) * 10f;
				Vector3 result;
				if (isPointOfGaze)
				{
					Vector3 pointOfGaze = _listBattleShipF[0].pointOfGaze;
					float x = pointOfGaze.x + vector.x;
					Vector3 pointOfGaze2 = _listBattleShipF[0].pointOfGaze;
					float y = pointOfGaze2.y;
					Vector3 pointOfGaze3 = _listBattleShipF[0].pointOfGaze;
					result = new Vector3(x, y, pointOfGaze3.z + vector.z);
				}
				else
				{
					Vector3 spPointOfGaze = _listBattleShipF[0].spPointOfGaze;
					float x2 = spPointOfGaze.x + vector.x;
					Vector3 spPointOfGaze2 = _listBattleShipF[0].spPointOfGaze;
					float y2 = spPointOfGaze2.y;
					Vector3 spPointOfGaze3 = _listBattleShipF[0].spPointOfGaze;
					result = new Vector3(x2, y2, spPointOfGaze3.z + vector.z);
				}
				return result;
			}
			Vector3 vector2 = Mathe.NormalizeDirection((!isPointOfGaze) ? _listBattleShipE[0].spPointOfGaze : _listBattleShipE[0].pointOfGaze, friendFieldCamera.eyePosition) * 10f;
			Vector3 result2;
			if (isPointOfGaze)
			{
				Vector3 pointOfGaze4 = _listBattleShipE[0].pointOfGaze;
				float x3 = pointOfGaze4.x + vector2.x;
				Vector3 pointOfGaze5 = _listBattleShipE[0].pointOfGaze;
				float y3 = pointOfGaze5.y;
				Vector3 pointOfGaze6 = _listBattleShipE[0].pointOfGaze;
				result2 = new Vector3(x3, y3, pointOfGaze6.z + vector2.z);
			}
			else
			{
				Vector3 spPointOfGaze4 = _listBattleShipE[0].spPointOfGaze;
				float x4 = spPointOfGaze4.x + vector2.x;
				Vector3 spPointOfGaze5 = _listBattleShipE[0].spPointOfGaze;
				float y4 = spPointOfGaze5.y;
				Vector3 spPointOfGaze6 = _listBattleShipE[0].spPointOfGaze;
				result2 = new Vector3(x4, y4, spPointOfGaze6.z + vector2.z);
			}
			return result2;
		}

		protected virtual Vector3 calcProtecterPos(Vector3 close4, FleetType type)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			if (type == FleetType.Friend)
			{
				Vector3 vector = Vector3.Lerp(_listBattleShipF[0].spPointOfGaze, close4, 0.58f);
				Vector3 position = _listBattleShipF[0].transform.position;
				float x = position.x;
				Vector3 spPointOfGaze = _listBattleShipF[0].spPointOfGaze;
				float num = x - spPointOfGaze.x;
				Vector3 position2 = _listBattleShipF[1].transform.position;
				float x2 = position2.x;
				Vector3 spPointOfGaze2 = _listBattleShipF[1].spPointOfGaze;
				float num3 = num - (x2 - spPointOfGaze2.x);
				Vector3 position3 = _listBattleShipF[0].transform.position;
				Vector3 seaLevelPos = battleField.seaLevelPos;
				position3.y = seaLevelPos.y;
				position3.z = vector.z + 1f;
				return position3;
			}
			Vector3 vector2 = Vector3.Lerp(_listBattleShipE[0].spPointOfGaze, close4, 0.58f);
			Vector3 position4 = _listBattleShipE[0].transform.position;
			float x3 = position4.x;
			Vector3 spPointOfGaze3 = _listBattleShipE[0].spPointOfGaze;
			float num2 = x3 - spPointOfGaze3.x;
			Vector3 position5 = _listBattleShipE[1].transform.position;
			float x4 = position5.x;
			Vector3 spPointOfGaze4 = _listBattleShipE[1].spPointOfGaze;
			float num4 = num2 - (x4 - spPointOfGaze4.x);
			Vector3 position6 = _listBattleShipE[0].transform.position;
			Vector3 seaLevelPos2 = battleField.seaLevelPos;
			position6.y = seaLevelPos2.y;
			position6.z = vector2.z - 1f;
			return position6;
		}

		public List<Vector3> calcCloseUpCamPos(Vector3 from, FleetType type)
		{
			Vector3 vector = calcCamTargetPos(isPointOfGaze: false, type);
			List<Vector3> list;
			List<Vector3> list2;
			if (type == FleetType.Friend)
			{
				Vector3 item = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[0]);
				Vector3 item2 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[1]);
				Vector3 spPointOfGaze = _listBattleShipF[1].spPointOfGaze;
				item.y = spPointOfGaze.y;
				Vector3 spPointOfGaze2 = _listBattleShipF[1].spPointOfGaze;
				item2.y = spPointOfGaze2.y;
				Vector3 position = _listBattleShipF[0].transform.position;
				float x = position.x;
				Vector3 spPointOfGaze3 = _listBattleShipF[0].spPointOfGaze;
				float num = x - spPointOfGaze3.x;
				Vector3 position2 = _listBattleShipF[1].transform.position;
				float x2 = position2.x;
				Vector3 spPointOfGaze4 = _listBattleShipF[1].spPointOfGaze;
				float num2 = num - (x2 - spPointOfGaze4.x);
				item.x += num2;
				item2.x += num2;
				list = new List<Vector3>();
				list.Add(Vector3.Lerp(from, vector, 0.98f));
				list.Add(vector);
				list.Add(item);
				list.Add(item2);
				list2 = list;
				for (int i = 0; i < list2.Count; i++)
				{
					List<Vector3> list3 = list2;
					int index = i;
					Vector3 vector2 = list2[i];
					float x3 = vector2.x - 2f;
					Vector3 vector3 = list2[i];
					float y = vector3.y;
					Vector3 vector4 = list2[i];
					list3[index] = new Vector3(x3, y, vector4.z - 8f);
				}
				return list2;
			}
			Vector3 item3 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[0]);
			Vector3 item4 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE[1]);
			Vector3 spPointOfGaze5 = _listBattleShipE[1].spPointOfGaze;
			item3.y = spPointOfGaze5.y;
			Vector3 spPointOfGaze6 = _listBattleShipE[1].spPointOfGaze;
			item4.y = spPointOfGaze6.y;
			Vector3 position3 = _listBattleShipE[0].transform.position;
			float x4 = position3.x;
			Vector3 spPointOfGaze7 = _listBattleShipE[0].spPointOfGaze;
			float num3 = x4 - spPointOfGaze7.x;
			Vector3 position4 = _listBattleShipE[1].transform.position;
			float x5 = position4.x;
			Vector3 spPointOfGaze8 = _listBattleShipE[1].spPointOfGaze;
			float num4 = num3 - (x5 - spPointOfGaze8.x);
			item3.x += num4;
			item4.x += num4;
			list = new List<Vector3>();
			list.Add(Vector3.Lerp(from, vector, 0.98f));
			list.Add(vector);
			list.Add(item3);
			list.Add(item4);
			list2 = list;
			for (int j = 0; j < list2.Count; j++)
			{
				List<Vector3> list4 = list2;
				int index2 = j;
				Vector3 vector5 = list2[j];
				float x6 = vector5.x;
				Vector3 vector6 = list2[j];
				float y2 = vector6.y;
				Vector3 vector7 = list2[j];
				list4[index2] = new Vector3(x6, y2, vector7.z + 8f);
			}
			return list2;
		}

		public static ProdTorpedoProtect Instantiate(ProdTorpedoProtect prefab, Transform parent)
		{
			ProdTorpedoProtect prodTorpedoProtect = UnityEngine.Object.Instantiate(prefab);
			prodTorpedoProtect.transform.parent = parent;
			prodTorpedoProtect.transform.localPosition = Vector3.zero;
			prodTorpedoProtect.transform.localScale = Vector3.one;
			prodTorpedoProtect._init();
			return prodTorpedoProtect;
		}
	}
}
