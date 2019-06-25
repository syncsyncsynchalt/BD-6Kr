using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			None,
			TorpedoCutInFriend,
			TorpedoCutInEnemy,
			ProdTorpedoCutIn
		}

		[SerializeField]
		private List<UITexture> _listShipTex;

		private AnimationList _iList;

		private RaigekiModel _clsRaigeki;

		public static ProdTorpedoCutIn Instantiate(ProdTorpedoCutIn prefab, RaigekiModel model, Transform parent)
		{
			ProdTorpedoCutIn prodTorpedoCutIn = UnityEngine.Object.Instantiate(prefab);
			prodTorpedoCutIn.transform.parent = parent;
			prodTorpedoCutIn.transform.localPosition = Vector3.zero;
			prodTorpedoCutIn.transform.localScale = Vector3.one;
			prodTorpedoCutIn._clsRaigeki = model;
			prodTorpedoCutIn.init();
			prodTorpedoCutIn.setShipInfo();
			return prodTorpedoCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelListSafe(ref _listShipTex);
			Mem.Del(ref _iList);
			_clsRaigeki = null;
		}

		private void init()
		{
			_iList = AnimationList.None;
			if (_listShipTex == null)
			{
				_listShipTex = new List<UITexture>();
				foreach (int value in Enum.GetValues(typeof(FleetType)))
				{
					if (value != 2)
					{
						_listShipTex.Add(((Component)base.transform.FindChild($"{((FleetType)value).ToString()}Ship/Anchor/Object2D")).GetComponent<UITexture>());
					}
				}
			}
		}

		public bool Run()
		{
			return false;
		}

		private void setShipInfo()
		{
			ShipModel_Attacker torpedoCutInShip = getTorpedoCutInShip(_clsRaigeki, isFriend: true);
			if (torpedoCutInShip != null)
			{
				_listShipTex[0].mainTexture = ShipUtils.LoadTexture(torpedoCutInShip);
				_listShipTex[0].MakePixelPerfect();
				_listShipTex[0].transform.localPosition = Util.Poi2Vec(new ShipOffset(torpedoCutInShip.GetGraphicsMstId()).GetShipDisplayCenter(torpedoCutInShip.DamagedFlg));
			}
			ShipModel_Attacker torpedoCutInShip2 = getTorpedoCutInShip(_clsRaigeki, isFriend: false);
			if (torpedoCutInShip2 != null)
			{
				_listShipTex[1].mainTexture = ShipUtils.LoadTexture(torpedoCutInShip2);
				_listShipTex[1].MakePixelPerfect();
				_listShipTex[1].transform.localPosition = ShipUtils.GetShipOffsPos(torpedoCutInShip2, MstShipGraphColumn.CutIn);
			}
			_iList = getAnimationList(torpedoCutInShip, torpedoCutInShip2);
		}

		private void debugShipInfo()
		{
		}

		public override void Play(Action callback)
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			base.transform.localScale = Vector3.one;
			_actCallback = callback;
			GetComponent<UIPanel>().widgetsAreStatic = false;
			if (_iList == AnimationList.None)
			{
				onAnimationFinishedAfterDiscard();
				return;
			}
			base.Play(_iList, callback);
			if (_iList == AnimationList.ProdTorpedoCutIn || _iList == AnimationList.TorpedoCutInFriend)
			{
				ShipUtils.PlayTorpedoVoice(getTorpedoCutInShip(_clsRaigeki, isFriend: true));
			}
			else if (_iList != AnimationList.TorpedoCutInEnemy)
			{
			}
		}

		private void DebugPlay()
		{
			_iList = AnimationList.ProdTorpedoCutIn;
			_actCallback = null;
			GetComponent<Animation>().Stop();
			base.Play(_iList, null);
			debugShipInfo();
			if (_iList == AnimationList.ProdTorpedoCutIn || _iList == AnimationList.TorpedoCutInFriend)
			{
				ShipUtils.PlayTorpedoVoice(getTorpedoCutInShip(_clsRaigeki, isFriend: true));
			}
			else if (_iList != AnimationList.TorpedoCutInEnemy)
			{
			}
		}

		private AnimationList getAnimationList(ShipModel_Battle friendShip, ShipModel_Battle enemyShip)
		{
			if (friendShip == null && enemyShip == null)
			{
				return AnimationList.None;
			}
			if (friendShip != null && enemyShip == null)
			{
				return AnimationList.TorpedoCutInFriend;
			}
			if (friendShip == null && enemyShip != null)
			{
				return AnimationList.TorpedoCutInEnemy;
			}
			return AnimationList.ProdTorpedoCutIn;
		}

		private ShipModel_Attacker getTorpedoCutInShip(RaigekiModel model, bool isFriend)
		{
			List<ShipModel_Attacker> attackers = model.GetAttackers(isFriend);
			if (attackers == null)
			{
				return null;
			}
			using (List<ShipModel_Attacker>.Enumerator enumerator = attackers.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		private void startMotionBlur()
		{
		}

		private void endMotionBlur()
		{
		}

		private void onPlaySeAnime(int seNo)
		{
			switch (seNo)
			{
			case 0:
			{
				SEFIleInfos info = SEFIleInfos.BattleAdmission;
				base._playSE(info);
				break;
			}
			case 1:
			{
				SEFIleInfos info = SEFIleInfos.BattleNightMessage;
				base._playSE(info);
				break;
			}
			}
		}

		private void onInitBackground()
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.ResetFleetAnchorPosition();
			BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SwitchMainCamera(FleetType.Friend);
			battleCameras.InitEnemyFieldCameraDefault();
			BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
			BattleFieldCamera enemyFieldCamera = battleCameras.enemyFieldCamera;
			battleCameras.isFieldDimCameraEnabled = false;
			friendFieldCamera.ResetMotionBlur();
			friendFieldCamera.clearFlags = CameraClearFlags.Skybox;
			friendFieldCamera.cullingMask = battleCameras.GetDefaultLayers();
			enemyFieldCamera.cullingMask = battleCameras.GetEnemyCamSplitLayers();
			battleCameras.SetVerticalSplitCameras(isSplit: true);
			friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			enemyFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors[CameraAnchorType.OneRowAnchor][FleetType.Friend].position;
			friendFieldCamera.transform.position = new Vector3(-51f, 8f, 90f);
			friendFieldCamera.transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 70f, 0f));
			Vector3 position2 = battleField.dicCameraAnchors[CameraAnchorType.OneRowAnchor][FleetType.Enemy].position;
			enemyFieldCamera.transform.position = new Vector3(-51f, 8f, -90f);
			enemyFieldCamera.transform.rotation = Quaternion.Euler(new Vector3(10.5f, 111f, 0f));
			battleField.isEnemySeaLevelActive = true;
			battleField.AlterWaveDirection(FleetType.Friend, FleetType.Friend);
			battleField.AlterWaveDirection(FleetType.Enemy, FleetType.Enemy);
			BattleShips battleShips2 = BattleTaskManager.GetBattleShips();
			battleShips2.RadarDeployment(isDeploy: false);
			battleShips2.SetBollboardTarget(isFriend: false, enemyFieldCamera.transform);
			battleShips2.SetTorpedoSalvoWakeAngle(isSet: true);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = ((Component)cutInEffectCamera.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 1f;
			}
			BattleTaskManager.GetBattleCameras().fieldDimCamera.maskAlpha = 0f;
			foreach (UIBattleShip value in BattleTaskManager.GetBattleShips().dicFriendBattleShips.Values)
			{
				value.billboard.billboardTarget = BattleTaskManager.GetBattleCameras().friendFieldCamera.transform;
			}
			foreach (UIBattleShip value2 in BattleTaskManager.GetBattleShips().dicEnemyBattleShips.Values)
			{
				value2.billboard.billboardTarget = BattleTaskManager.GetBattleCameras().enemyFieldCamera.transform;
			}
		}
	}
}
