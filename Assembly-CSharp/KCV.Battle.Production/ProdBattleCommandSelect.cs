using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBattleCommandSelect : MonoBehaviour
	{
		[SerializeField]
		private UICommandBox _uiCommandBox;

		[SerializeField]
		private UICommandUnitList _uiCommandUnitList;

		[SerializeField]
		private UIPanel _uiOverlay;

		[SerializeField]
		private UITexture _uiBlur;

		private Action _actOnFinished;

		private CommandPhaseModel _clsCommandModel;

		private InputMode _iInputMode;

		private BattleCommandMode _iCommandMode;

		private List<BattleCommand> _listInvalidCommands;

		private StatementMachine _clsState;

		public UICommandBox commandBox => _uiCommandBox;

		public UICommandUnitList commandUnitList => _uiCommandUnitList;

		public InputMode inputMode
		{
			get
			{
				return _iInputMode;
			}
			private set
			{
				_iInputMode = value;
			}
		}

		public BattleCommandMode commandMode
		{
			get
			{
				return _iCommandMode;
			}
			private set
			{
				_iCommandMode = value;
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.SetNavigationInCommand(_iCommandMode);
			}
		}

		public static ProdBattleCommandSelect Instantiate(ProdBattleCommandSelect prefab, Transform parent, CommandPhaseModel model)
		{
			ProdBattleCommandSelect prodBattleCommandSelect = UnityEngine.Object.Instantiate(prefab);
			prodBattleCommandSelect.transform.parent = parent;
			prodBattleCommandSelect.transform.localScaleOne();
			prodBattleCommandSelect.transform.localPositionZero();
			prodBattleCommandSelect.Init(model);
			return prodBattleCommandSelect;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiCommandBox);
			Mem.Del(ref _uiCommandUnitList);
			Mem.Del(ref _uiOverlay);
			Mem.Del(ref _uiBlur);
			Mem.Del(ref _actOnFinished);
			Mem.Del(ref _clsCommandModel);
			Mem.Del(ref _iInputMode);
			Mem.Del(ref _iCommandMode);
			Mem.DelListSafe(ref _listInvalidCommands);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
		}

		private bool Init(CommandPhaseModel model)
		{
			_listInvalidCommands = GetInvalidCommands(model.GetSelectableCommands());
			_clsCommandModel = model;
			_iInputMode = InputMode.Key;
			_uiCommandBox.Init(model, OnStartBattle);
			_uiCommandBox.isColliderEnabled = false;
			_uiCommandUnitList.Init(model, OnUnitListDnDRelease);
			_uiCommandUnitList.isColliderEnabled = false;
			_uiOverlay.alpha = 0f;
			_uiBlur.enabled = false;
			commandMode = BattleCommandMode.SurfaceBox;
			_clsState = new StatementMachine();
			return true;
		}

		private void InitCommandBackground()
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			BattleTaskManager.GetPrefabFile().DisposeProdCloud();
			battleField.dicFleetAnchor[FleetType.Friend].position = Vector3.forward * 100f;
			battleField.dicFleetAnchor[FleetType.Enemy].position = Vector3.back * 100f;
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.RotateAroundObject);
			battleFieldCamera.SetRotateAroundObjectCamera(battleField.fieldCenter.position, 200f, -9.5f);
			battleFieldCamera.transform.LTMoveY(15.51957f, 0.01f).setEase(LeanTweenType.easeOutQuart);
			_uiBlur.enabled = true;
			battleShips.RadarDeployment(isDeploy: true);
		}

		private List<BattleCommand> GetInvalidCommands(HashSet<BattleCommand> validCommands)
		{
			List<BattleCommand> list = new List<BattleCommand>();
			foreach (int value in Enum.GetValues(typeof(BattleCommand)))
			{
				if (value != -1 && !validCommands.Contains((BattleCommand)value))
				{
					list.Add((BattleCommand)value);
				}
			}
			return list;
		}

		public IEnumerator PlayShowAnimation(Action onFinished)
		{
			UIBattleNavigation uibn = BattleTaskManager.GetPrefabFile().battleNavigation;
			uibn.SetNavigationInCommand(commandMode);
			_actOnFinished = onFinished;
			yield return new WaitForSeconds(0.2f);
			bool wait2 = false;
			FadeOutOverlay().setOnComplete((Action)delegate
			{
                wait2 = true;
			});
			while (!wait2)
			{
				yield return new WaitForEndOfFrame();
			}
			ObserverActionQueue oaq = BattleTaskManager.GetObserverAction();
			oaq.Executions();
			wait2 = false;
			InitCommandBackground();
			FadeInOverlay().setOnComplete((Action)delegate
			{
                wait2 = true;
			});
			while (!wait2)
			{
				yield return new WaitForEndOfFrame();
			}
			_uiCommandUnitList.Show().setOnComplete((Action)delegate
			{
				TutorialModel tutorial = BattleTaskManager.GetBattleManager().UserInfo.Tutorial;
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.BattleCommand, null, delegate
				{
                    uibn.Show();
					this._clsState.AddState(this.InitKeyMode, this.UpdateKeyMode);
					this._uiCommandBox.isColliderEnabled = true;
					this._uiCommandBox.ChkAllSurfaceSet();
					this._uiCommandUnitList.isColliderEnabled = true;
				});
			});
			Observable.FromCoroutine(_uiCommandBox.PlayShowAnimation).Subscribe();
		}

		public LTDescr DiscardAfterFadeIn()
		{
			_uiCommandBox.panel.alpha = 0f;
			_uiCommandUnitList.panel.alpha = 0f;
			return FadeInOverlay();
		}

		private LTDescr FadeInOverlay()
		{
			_uiOverlay.transform.LTCancel();
			return _uiOverlay.transform.LTValue(_uiOverlay.alpha, 0f, 1.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				_uiOverlay.alpha = x;
			});
		}

		private LTDescr FadeOutOverlay()
		{
			_uiOverlay.transform.LTCancel();
			return _uiOverlay.transform.LTValue(_uiOverlay.alpha, 1f, 0.35f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				_uiOverlay.alpha = x;
			});
		}

		public bool Run()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return true;
		}

		private bool InitKeyMode(object data)
		{
			inputMode = InputMode.Key;
			commandMode = BattleCommandMode.SurfaceBox;
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = Generics.Layers.CutIn;
			_uiCommandBox.SetBattleStartButtonLayer();
			_uiCommandBox.FocusSurfaceMagnify();
			return false;
		}

		private bool UpdateKeyMode(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			for (int i = 0; i < _uiCommandUnitList.listCommandUnits.Count; i++)
			{
				_uiCommandUnitList.listCommandUnits[i].ResetPosition();
			}
			if (Input.touchCount != 0 || Input.GetMouseButtonDown(0))
			{
				_clsState.AddState(InitTouchMode, UpdateTouchMode);
				return true;
			}
			switch (commandMode)
			{
			case BattleCommandMode.SurfaceBox:
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					_uiCommandBox.Prev();
					break;
				}
				if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					_uiCommandBox.Next();
					break;
				}
				if (keyControl.GetDown(KeyControl.KeyName.BATU))
				{
					_uiCommandBox.RemoveCommandUnit2FocusSurface();
					break;
				}
				if (keyControl.GetDown(KeyControl.KeyName.SHIKAKU))
				{
					_uiCommandBox.RemoveCommandUnitAll();
					return false;
				}
				if (!keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					break;
				}
				return OnKeyModeDecideSurface();
			case BattleCommandMode.UnitList:
				if (keyControl.GetDown(KeyControl.KeyName.UP))
				{
					_uiCommandUnitList.PrevColumn();
					break;
				}
				if (keyControl.GetDown(KeyControl.KeyName.DOWN))
				{
					_uiCommandUnitList.NextColumn();
					break;
				}
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					_uiCommandUnitList.PrevLine();
					break;
				}
				if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					_uiCommandUnitList.NextLine();
					break;
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					return OnKeyModeDecideUnit();
				}
				if (!keyControl.GetDown(KeyControl.KeyName.BATU))
				{
					break;
				}
				return OnKeyModeCancelUnit();
			}
			return false;
		}

		private bool OnKeyModeDecideSurface()
		{
			if (_uiCommandBox.isSelectBattleStart)
			{
				return _uiCommandBox.DecideStartBattle();
			}
			_uiCommandUnitList.Active2FocusUnit2(_uiCommandBox.focusSurface, _listInvalidCommands);
			commandMode = BattleCommandMode.UnitList;
			return false;
		}

		private bool OnKeyModeDecideUnit()
		{
			if (!_uiCommandUnitList.focusUnitIcon.isValid)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				return false;
			}
			_uiCommandBox.AbsodedUnitIcon2FocusSurface();
			_uiCommandBox.SetFocusUnitIcon2FocusSurface();
			_uiCommandUnitList.Reset2Unit();
			_uiCommandUnitList.ActiveAll2Unit(isActive: false);
			_uiCommandBox.FocusSurfaceMagnify();
			commandMode = BattleCommandMode.SurfaceBox;
			return false;
		}

		private bool OnKeyModeCancelUnit()
		{
			_uiCommandUnitList.ActiveAll2Unit(isActive: false);
			commandMode = BattleCommandMode.SurfaceBox;
			return false;
		}

		private bool InitTouchMode(object data)
		{
			inputMode = InputMode.Touch;
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			_uiCommandBox.ReductionAll();
			_uiCommandUnitList.ActiveAll2Unit(isActive: false);
			_uiCommandUnitList.isColliderEnabled = true;
			return false;
		}

		private bool UpdateTouchMode(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (keyControl.IsAnyKey)
			{
				return OnTouchModeFinished();
			}
			return false;
		}

		private bool OnTouchModeFinished()
		{
			_uiCommandUnitList.Reset2Unit();
			_clsState.AddState(InitKeyMode, UpdateKeyMode);
			return true;
		}

		private void OnUnitListDnDRelease()
		{
			List<UICommandSurface> listCommandSurfaces = _uiCommandBox.listCommandSurfaces;
			listCommandSurfaces.FindAll((UICommandSurface x) => !x.isAbsorded).ForEach(delegate(UICommandSurface x)
			{
				x.Reduction();
			});
			listCommandSurfaces.ForEach(delegate(UICommandSurface x)
			{
				x.isAbsorded = false;
			});
		}

		private bool OnStartBattle(List<BattleCommand> commands)
		{
			if (!_clsCommandModel.SetCommand(commands))
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				return false;
			}
			_clsState.Clear();
			_uiCommandUnitList.isColliderEnabled = false;
			Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
			{
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.Hide();
				FadeOutOverlay().setOnComplete((Action)delegate
				{
					_uiBlur.enabled = false;
					Dlg.Call(ref _actOnFinished);
				});
			});
			return true;
		}
	}
}
