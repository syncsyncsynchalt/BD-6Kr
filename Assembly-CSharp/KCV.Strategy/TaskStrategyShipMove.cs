using KCV.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyShipMove : SceneTaskMono
	{
		private StrategyTopTaskManager sttm;

		private TaskStrategySailSelect TaskSailSelect;

		private StrategyShipManager shipIconManager;

		private int moveDeckID;

		private int sailID;

		private Vector3 NextTilePos;

		private int currentAreaID;

		public AnimationCurve bound;

		private StrategyInfoManager.Mode prevMode;

		protected override void Start()
		{
			sttm = StrategyTaskManager.GetStrategyTop();
			TaskSailSelect = StrategyTopTaskManager.GetSailSelect();
			shipIconManager = StrategyTopTaskManager.Instance.ShipIconManager;
		}

		protected override bool Init()
		{
			Debug.Log("+++ TaskStrategyShipMove +++");
			KeyControlManager.Instance.KeyController = StrategyAreaManager.sailKeyController;
			currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			prevMode = StrategyTopTaskManager.Instance.GetInfoMng().NowInfoMode;
			StrategyTopTaskManager.Instance.GetInfoMng().SetSidePanelMode(StrategyInfoManager.Mode.AreaInfo);
			StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.3f);
			List<int> neighboringAreaIDs = StrategyTopTaskManager.GetLogicManager().Area[currentAreaID].NeighboringAreaIDs;
			StrategyTopTaskManager.Instance.TileManager.ChangeTileColorMove(neighboringAreaIDs);
			return true;
		}

		protected override bool Run()
		{
			StrategyAreaManager.sailKeyController.Update();
			sailID = StrategyAreaManager.sailKeyController.Index;
			if (StrategyAreaManager.sailKeyController.IsChangeIndex)
			{
				StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(sailID);
				StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(sailID);
			}
			else
			{
				if (StrategyAreaManager.sailKeyController.keyState[1].down)
				{
					return OnMoveDeside();
				}
				if (StrategyAreaManager.sailKeyController.keyState[0].down)
				{
					OnMoveCancel();
					return false;
				}
				if (StrategyAreaManager.sailKeyController.keyState[5].down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
				}
			}
			return true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		public bool OnMoveDeside()
		{
			if (StrategyTopTaskManager.GetLogicManager().Area[currentAreaID].NeighboringAreaIDs.Exists((int x) => x == StrategyAreaManager.sailKeyController.Index) && !shipIconManager.isShipMoving)
			{
				StrategyTopTaskManager.Instance.TileManager.ChangeTileColorMove(null);
				MoveStart();
				SoundUtils.PlaySE(SEFIleInfos.StrategyShipMove);
				return false;
			}
			return true;
		}

		public void OnMoveCancel()
		{
			StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: true, null);
			StrategyTopTaskManager.Instance.GetInfoMng().ExitInfoPanel();
			StrategyTopTaskManager.Instance.TileManager.ChangeTileColorMove(null);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			Close();
		}

		private void MoveStart()
		{
			Debug.Log("MoveStart" + Time.realtimeSinceStartup);
			StrategyTopTaskManager.GetSailSelect().isEnableCharacterEnter = true;
			moveDeckID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			StrategyTopTaskManager.GetLogicManager().Move(moveDeckID, sailID);
			StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(sailID);
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckAreaModel = StrategyTopTaskManager.GetLogicManager().Area[sailID];
			shipIconManager.sortAreaShipIcon(currentAreaID, isMoveCharacter: false, isUpdateOrganizeMessage: false);
			shipIconManager.sortAreaShipIcon(sailID, isMoveCharacter: false, isUpdateOrganizeMessage: true);
			returnPrevInfoMode();
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
			Debug.Log("MoveEnd" + Time.realtimeSinceStartup);
		}

		private void returnPrevInfoMode()
		{
			StrategyTopTaskManager.Instance.GetInfoMng().SetSidePanelMode(prevMode);
			StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.3f);
		}
	}
}
