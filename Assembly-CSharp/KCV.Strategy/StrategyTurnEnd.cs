using System;
using Common.Enum;
using KCV.Scene.Strategy;
using KCV.Strategy.Rebellion;
using KCV.Tutorial.Guide;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Common;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTurnEnd : SceneTaskMono
	{
		[SerializeField]
		private DayAnimation dayAnimation;

		[SerializeField]
		private GameObject ReturnMissionAnim;

		[SerializeField]
		private UserInterfaceStrategyResult mPrefab_UserInterfaceStrategyResult;

		private StrategyMapManager LogicMng;

		private EnemyActionPhaseResultModel enemyResult;

		private TurnResultModel TurnResult;

		private UserPreActionPhaseResultModel userPreAction;

		public bool isRebellion;

		private bool finished;

		public bool TurnEndFinish;

		private bool isDebug;

		private new void Awake()
		{
			TurnEndFinish = true;
		}

		public void TurnEnd()
		{
			if (Server_Common.Utils.IsTurnOver())
			{
				StrategyTopTaskManager.Instance.GameOver();
				return;
			}
			TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (tutorial.GetStep() == 7 && !tutorial.GetStepTutorialFlg(8))
			{
				tutorial.SetStepTutorialFlg(8);
				CommonPopupDialog.Instance.StartPopup("「タ\u30fcン終了」 達成");
				SoundUtils.PlaySE(SEFIleInfos.SE_012);
			}
			TurnEndFinish = false;
			isRebellion = false;
			StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.3f);
			StartCoroutine(TurnEndCoroutine());
			StrategyTopTaskManager.Instance.UIModel.Character.ResetTouchCount();
		}

		public void DebugTurnEnd()
		{
			TurnEndFinish = true;
			isDebug = true;
			isRebellion = false;
			StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			StrategyTopTaskManager.GetLogicManager().GetResult_UserActionPhase();
			StrategyTopTaskManager.GetLogicManager().GetResult_EnemyPreActionPhase();
			StrategyTopTaskManager.GetLogicManager().GetResult_EnemyActionPhase();
			TurnResult = StrategyTopTaskManager.GetLogicManager().GetResult_Turn();
			StrategyTopTaskManager.GetLogicManager().GetResult_UserPreActionPhase();
		}

		public void DebugTurnEndAuto()
		{
			TurnEndFinish = true;
			isDebug = true;
			isRebellion = false;
			StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			StrategyTopTaskManager.GetLogicManager().GetResult_UserActionPhase();
			StrategyTopTaskManager.GetLogicManager().GetResult_EnemyPreActionPhase();
			StrategyTopTaskManager.GetLogicManager().GetResult_EnemyActionPhase();
			TurnResult = StrategyTopTaskManager.GetLogicManager().GetResult_Turn();
			StrategyTopTaskManager.GetLogicManager().GetResult_UserPreActionPhase();
		}

		public IEnumerator TurnEndCoroutine()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			LogicMng = StrategyTopTaskManager.GetLogicManager();
			TutorialModel model = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			bool isFlagShipDamaged = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip()?.IsDamaged() ?? false;
			StrategyTopTaskManager.GetLogicManager().GetResult_UserActionPhase();
			StrategyTopTaskManager.GetLogicManager().GetResult_EnemyPreActionPhase();
			EnemyActionPhaseResultModel enemyResult = StrategyTopTaskManager.GetLogicManager().GetResult_EnemyActionPhase();
			TurnResult = StrategyTopTaskManager.GetLogicManager().GetResult_Turn();
			StrategyMapManager j = StrategyTopTaskManager.GetLogicManager();
			EnemyResult(enemyResult);
			if (TurnResult.RadingResult != null)
			{
				bool TutorialFinished3 = false;
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.Raider, delegate
				{
					SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
					{
                        TutorialFinished3 = true;
					};
				});
				while (!TutorialFinished3)
				{
					TutorialDialog t = SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog();
					if (t == null && !SingletonMonoBehaviour<TutorialGuideManager>.Instance.isRequest())
					{
						break;
					}
					yield return new WaitForEndOfFrame();
				}
				TileAnimationManager tam = GetComponent<TileAnimationManager>();
				bool isFirst = true;
				StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(isVisible: false);
				StrategyTopTaskManager.Instance.TileManager.SetVisibleAllAreaDockIcons(isVisible: false);
				for (int i = 0; i < TurnResult.RadingResult.Count; i++)
				{
					tam.Initialize(TurnResult.RadingResult[i], j.Area[TurnResult.RadingResult[i].AreaId], isFirst);
					isFirst = false;
					while (!tam.isFinished)
					{
						yield return new WaitForEndOfFrame();
					}
					tam.isFinished = false;
				}
				StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(isVisible: true);
				StrategyTopTaskManager.Instance.TileManager.SetVisibleAllAreaDockIcons(isVisible: true);
				bool isShow = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE;
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, isShow);
				StrategyTopTaskManager.Instance.UIModel.MapCamera.MoveToTargetTile(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID);
			}
			StartCoroutine(UserPreAction());
			dayAnimation.SetActive(isActive: true);
			StrategyTopTaskManager.Instance.GetInfoMng().updateUpperInfo();
			StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
			finished = false;
			yield return StartCoroutine(dayAnimation.StartDayAnimation(LogicMng, isDebug));
			yield return StartCoroutine(dayAnimation.StartMonthAnimation(LogicMng, userPreAction, isDebug));
			yield return StartCoroutine(dayAnimation.StartWeekAnimation(LogicMng, userPreAction, isDebug));
			yield return StartCoroutine(dayAnimation.StartSendChocoAnimation(LogicMng, userPreAction, isDebug));
			yield return StartCoroutine(dayAnimation.EndDayAnimation(LogicMng, isDebug));
			isDebug = false;
			dayAnimation.SetActive(isActive: false);
			StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.setShipIconsState();
			StrategyTopTaskManager.Instance.TileManager.updateTilesColor();
			if (StrategyTopTaskManager.Instance.TileManager.isExistRebellionTargetTile())
			{
				yield return StartCoroutine(RebellionTutorialGuide(model));
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() != null && isFlagShipDamaged != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip().IsDamaged())
			{
				StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter();
				StrategyTopTaskManager.Instance.UIModel.Character.setState(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			}
			bool TutorialFinished2 = !SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.ResourceRecovery, delegate
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
				{
                    TutorialFinished2 = true;
				};
			});
			while (!TutorialFinished2)
			{
				yield return new WaitForEndOfFrame();
			}
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: true);
			StrategyTopTaskManager.Instance.TileManager.UpdateAllAreaDockIcons();
			Close();
			if (model.GetStep() == 8 && !model.GetStepTutorialFlg(9))
			{
				StartCoroutine(StrategyTopTaskManager.Instance.TutorialCheck());
			}
			if (isRebellion)
			{
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
				}
				StrategyTaskManager.ReqMode(StrategyTaskManager.StrategyTaskManagerMode.Rebellion);
			}
			else
			{
				StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
			}
			yield return null;
		}

		private void EnemyResult(EnemyActionPhaseResultModel enemyResult)
		{
			if (StrategyRebellionTaskManager.RebellionForceDebug)
			{
				finished = true;
			}
			else if (StrategyTopTaskManager.GetLogicManager().GetRebellionAreaList().Count != 0)
			{
				isRebellion = true;
				StrategyRebellionTaskManager.checkRebellionArea();
			}
		}

		private IEnumerator UserPreAction()
		{
			userPreAction = StrategyTopTaskManager.GetLogicManager().GetResult_UserPreActionPhase();
			MissionResultModel[] missionResultModels = userPreAction.MissionResults;
			if (missionResultModels.Length != 0)
			{
				ShipUtils.PlayShipVoice(LogicMng.UserInfo.GetDeck(1).GetFlagShip(), 7);
				KeyControl managerKeyController = KeyControlManager.Instance.KeyController;
				for (int i = 0; i < missionResultModels.Length; i++)
				{
					finished = false;
					GameObject ReturnAnim = Util.InstantiateGameObject(ReturnMissionAnim, GameObject.Find("OverView").transform);
					UIMissionStateChangedCutin Anim = ReturnAnim.GetComponent<UIMissionStateChangedCutin>();
					Anim.Initialize(LogicMng.UserInfo.GetDeck(userPreAction.MissionResults[i].DeckID));
					Anim.PlayFinishedCutin(delegate
					{
                        throw new NotImplementedException("なにこれ");
                        // _003CUserPreAction_003Ec__Iterator186 _003CUserPreAction_003Ec__Iterator = this;


						UnityEngine.Object.Destroy(Anim.gameObject);
						UserInterfaceStrategyResult userInterfaceStrategyResult = UnityEngine.Object.Instantiate(this.mPrefab_UserInterfaceStrategyResult);
						userInterfaceStrategyResult.transform.positionX(5000f);
						MissionResultModel missionResultModel = missionResultModels[i];
						KeyControl keyController = new KeyControl();
						userInterfaceStrategyResult.Initialize(StrategyTopTaskManager.GetLogicManager(), missionResultModel, keyController, delegate
						{
							StrategyTopTaskManager.Instance.UIModel.Character.setState(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
							StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter();
							StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.setShipIconsState();
							userInterfaceStrategyResult.FadeOut(delegate
							{
                                UnityEngine.Object.Destroy(userInterfaceStrategyResult.gameObject);
                                finished = true;
							});
						});
						userInterfaceStrategyResult.Play();
					});
					while (!finished)
					{
						yield return null;
					}
				}
				KeyControlManager.Instance.KeyController = managerKeyController;
				bool isShow = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE;
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, isShow);
			}
			else
			{
				finished = true;
			}
			yield return null;
		}

		private IEnumerator RebellionTutorialGuide(TutorialModel model)
		{
			bool TutorialFinished = false;
			if (SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(model, TutorialGuideManager.TutorialID.RebellionPreparation))
			{
				int currentAreaID = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
				int TargetAreaID = StrategyTopTaskManager.Instance.TileManager.GetColorChangedTileID();
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null);
				StrategyTopTaskManager.GetSailSelect().isEnableCharacterEnter = false;
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
				}
				yield return StrategyTopTaskManager.Instance.UIModel.MapCamera.MoveToTargetTile(TargetAreaID);
				yield return new WaitForSeconds(0.5f);
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.RebellionPreparation, delegate
				{
					SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = delegate
					{
                        TutorialFinished = true;
					};
				});
				while (!TutorialFinished)
				{
					yield return new WaitForEndOfFrame();
				}
				StrategyTopTaskManager.GetSailSelect().isEnableCharacterEnter = true;
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null);
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
				}
				yield return StrategyTopTaskManager.Instance.UIModel.MapCamera.MoveToTargetTile(currentAreaID);
			}
		}

		private void OnDestroy()
		{
			ReturnMissionAnim = null;
			mPrefab_UserInterfaceStrategyResult = null;
			dayAnimation = null;
			ReturnMissionAnim = null;
			LogicMng = null;
			enemyResult = null;
			TurnResult = null;
			userPreAction = null;
		}
	}
}
