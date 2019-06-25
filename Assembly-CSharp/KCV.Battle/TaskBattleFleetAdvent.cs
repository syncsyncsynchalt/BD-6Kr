using KCV.Battle.Utils;
using Librarys.Cameras;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleFleetAdvent : BaseBattleTask
	{
		private Dictionary<FleetType, ParticleSystem> _dicPSClouds;

		protected override bool Init()
		{
			if (!BattleTaskManager.GetBattleShips().isInitialize)
			{
				return false;
			}
			_dicPSClouds = BattleTaskManager.GetBattleField().dicParticleClouds;
			Vector3 position = BattleTaskManager.GetBattleField().dicFleetAnchor[FleetType.Friend].position;
			position.y = 20f;
			((Component)_dicPSClouds[FleetType.Friend]).transform.position = position;
			position = BattleTaskManager.GetBattleField().dicFleetAnchor[FleetType.Enemy].position;
			position.y = 20f;
			((Component)_dicPSClouds[FleetType.Enemy]).transform.position = position;
			_clsState = new StatementMachine();
			_clsState.AddState(InitFriendFleetAdvent, UpdateFriendFleetAdvent);
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			foreach (KeyValuePair<FleetType, ParticleSystem> dicPSCloud in _dicPSClouds)
			{
				dicPSCloud.Value.Stop();
				((Component)dicPSCloud.Value).SetActive(isActive: false);
			}
			_dicPSClouds = null;
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.FleetAdvent);
		}

		private bool InitFriendFleetAdvent(object data)
		{
			((Component)_dicPSClouds[FleetType.Friend]).SetActive(isActive: true);
			_dicPSClouds[FleetType.Friend].Play();
			BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			UIBattleShip uIBattleShip = BattleTaskManager.GetBattleShips().dicFriendBattleShips[0];
			Vector3 position = BattleTaskManager.GetBattleField().dicFleetAnchor[FleetType.Friend].position;
			Vector3 pointOfGaze = uIBattleShip.pointOfGaze;
			position.y = pointOfGaze.y;
			ShipUtils.PlayBattleStartVoice(uIBattleShip.shipModel);
			cam.ReqViewMode(CameraActor.ViewMode.RotateAroundObject);
			cam.SetRotateAroundObjectCamera(position, BattleDefines.FLEET_ADVENT_START_CAM_POS[0], -10f);
			List<float> rotDst = CalcCloseUpCamDist(cam.rotateDistance, 30f);
			cam.transform.LTValue(cam.rotateDistance, rotDst[0], 1f).setEase(BattleDefines.FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE).setOnUpdate(delegate(float x)
			{
				cam.rotateDistance = x;
			})
				.setOnComplete((Action)delegate
				{
					cam.transform.LTValue(cam.rotateDistance, rotDst[1], 1f).setEase(BattleDefines.FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE).setOnUpdate(delegate(float x)
					{
						cam.rotateDistance = x;
					})
						.setOnComplete((Action)delegate
						{
							EndPhase(BattleUtils.NextPhase(BattlePhase.FleetAdvent));
						});
				});
			return false;
		}

		private bool UpdateFriendFleetAdvent(object data)
		{
			return true;
		}

		private List<float> CalcCloseUpCamDist(float from, float to)
		{
			List<float> list = new List<float>();
			list.Add(Mathe.Lerp(from, to, 0.95f));
			list.Add(to);
			return list;
		}
	}
}
