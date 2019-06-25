using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class TaskRebellionOrganize : SceneTaskMono
	{
		[SerializeField]
		private Transform prefabRebellionOrganizeCtrl;

		private CtrlRebellionOrganize _ctrlRebellionOrganize;

		[SerializeField]
		private CommonDialog strategyDialog;

		[SerializeField]
		private YesNoButton CancelDialogButton;

		public CtrlRebellionOrganize ctrlRebellionOrganize => _ctrlRebellionOrganize;

		private void OnDestroy()
		{
			prefabRebellionOrganizeCtrl = null;
			_ctrlRebellionOrganize = null;
			strategyDialog = null;
			CancelDialogButton = null;
		}

		protected override bool Init()
		{
			DebugUtils.Log("TaskRebellionOrganize", string.Empty);
			_ctrlRebellionOrganize = CtrlRebellionOrganize.Instantiate(((Component)prefabRebellionOrganizeCtrl).GetComponent<CtrlRebellionOrganize>(), StrategyTaskManager.GetOverView(), DecideSortieStart, DecideCancel);
			return true;
		}

		protected override bool UnInit()
		{
			DebugUtils.Log("TaskRebellionOrganize", string.Empty);
			if (_ctrlRebellionOrganize != null && _ctrlRebellionOrganize.gameObject != null)
			{
				_ctrlRebellionOrganize.gameObject.Discard();
			}
			_ctrlRebellionOrganize = null;
			return true;
		}

		protected override bool Run()
		{
			_ctrlRebellionOrganize.Run();
			return StrategyUtils.ChkStateRebellionTaskIsRun(StrategyRebellionTaskManagerMode.Organize);
		}

		private void DecideSortieStart()
		{
			DebugUtils.Log("TaskRebellionOrganize", string.Empty);
			RebellionManager rebellionManager = StrategyTaskManager.GetStrategyRebellion().GetRebellionManager();
			List<UIRebellionParticipatingFleetInfo> participatingFleetList = _ctrlRebellionOrganize.participatingFleetSelector.participatingFleetList;
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.VanguardFleet);
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo2 = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.DecisiveBattlePrimaryFleet);
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo3 = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.VanguardSupportFleet);
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo4 = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.DecisiveBattleSupportFleet);
			int[] array = new int[4]
			{
				(!(uIRebellionParticipatingFleetInfo == null)) ? uIRebellionParticipatingFleetInfo.deckModel.Id : (-1),
				(!(uIRebellionParticipatingFleetInfo2 == null)) ? uIRebellionParticipatingFleetInfo2.deckModel.Id : (-1),
				(!(uIRebellionParticipatingFleetInfo3 == null)) ? uIRebellionParticipatingFleetInfo3.deckModel.Id : (-1),
				(!(uIRebellionParticipatingFleetInfo4 == null)) ? uIRebellionParticipatingFleetInfo4.deckModel.Id : (-1)
			};
			bool flag = rebellionManager.IsGoRebellion(array[0], array[1], array[2], array[3]);
			List<IsGoCondition> list = null;
			List<IsGoCondition> list2 = null;
			if (array[2] != -1)
			{
				list = rebellionManager.IsValidMissionSub(array[2]);
			}
			if (array[3] != -1)
			{
				list2 = rebellionManager.IsValid_MissionMain(array[3]);
			}
			bool flag2 = list == null || (list != null && list.Count == 0);
			bool flag3 = list2 == null || (list2 != null && list2.Count == 0);
			if (flag && flag2 && flag3)
			{
				RebellionMapManager rebellionMapManager = rebellionManager.GoRebellion(array[0], array[1], array[2], array[3]);
				MapModel map = rebellionMapManager.Map;
				Hashtable hashtable = new Hashtable();
				hashtable.Add("rebellionMapManager", rebellionMapManager);
				hashtable.Add("rootType", 0);
				hashtable.Add("shipRecoveryType", ShipRecoveryType.None);
				hashtable.Add("escape", false);
				RetentionData.SetData(hashtable);
				Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
				SingletonMonoBehaviour<AppInformation>.Instance.prevStrategyDecks = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks();
				StartCoroutine(PlayTransition(map, uIRebellionParticipatingFleetInfo2.deckModel));
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void DecideCancel()
		{
			strategyDialog.isUseDefaultKeyController = false;
			strategyDialog.OpenDialog(3);
			CancelDialogButton.SetOnSelectPositiveListener(CancelOrganize);
			CancelDialogButton.SetOnSelectNegativeListener(delegate
			{
				strategyDialog.CloseDialog();
			});
			CancelDialogButton.SetKeyController(new KeyControl());
		}

		private void CancelOrganize()
		{
			strategyDialog.CloseDialog();
			strategyDialog.setCloseAction(delegate
			{
				DebugUtils.Log("TaskRebellionOrganize", string.Empty);
				Close();
				StartCoroutine(StrategyTaskManager.GetStrategyRebellion().taskRebellionEvent.NonDeckLose());
			});
		}

		private IEnumerator PlayTransition(MapModel mapModel, DeckModel deck)
		{
			Camera cam = GameObject.Find("TopCamera").GetComponent<Camera>();
			AsyncOperation asyncOpe = Application.LoadLevelAsync(Generics.Scene.SortieAreaMap.ToString());
			asyncOpe.allowSceneActivation = false;
			yield return null;
			MapTransitionCutManager mtcm = Util.Instantiate(Resources.Load("Prefabs/StrategyPrefab/MapSelect/MapTransitionCut"), base.transform.root.Find("Map Root").gameObject).GetComponent<MapTransitionCutManager>();
			mtcm.transform.localPosition = cam.transform.localPosition + new Vector3(-26.4f, -43f, 496.4f);
			mtcm.Initialize(mapModel, asyncOpe);
			ShipUtils.PlayShipVoice(deck.GetFlagShip(), 14);
		}
	}
}
