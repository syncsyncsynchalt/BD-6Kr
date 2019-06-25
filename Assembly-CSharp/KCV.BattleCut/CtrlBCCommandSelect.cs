using Common.Enum;
using local.managers;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlBCCommandSelect : MonoBehaviour
	{
		public enum CtrlMode
		{
			Surface,
			Command
		}

		[SerializeField]
		private Transform _prefabUICommandUnitSelect;

		[SerializeField]
		private UICommandSurfaceList _uiCommandSurfaceList;

		[SerializeField]
		private ProdBCBattle.FleetInfos _uiEnemyFleetInfos;

		private bool _isInputPossible;

		private CommandPhaseModel _clsCommandModel;

		private UICommandUnitSelect _uiCommandUnitSelect;

		private UIPanel _uiPanel;

		private CtrlMode _iCtrlModel;

		private StatementMachine _clsState;

		private List<BattleCommand> _listInvalidCommands;

		private Action _actOnFinished;

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static CtrlBCCommandSelect Instantiate(CtrlBCCommandSelect prefab, Transform parent, CommandPhaseModel model)
		{
			CtrlBCCommandSelect ctrlBCCommandSelect = UnityEngine.Object.Instantiate(prefab);
			ctrlBCCommandSelect.transform.parent = parent;
			ctrlBCCommandSelect.transform.localPositionZero();
			ctrlBCCommandSelect.transform.localScaleOne();
			ctrlBCCommandSelect.Init(model);
			return ctrlBCCommandSelect;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiCommandSurfaceList);
			Mem.Del(ref _clsCommandModel);
			Mem.Del(ref _actOnFinished);
		}

		private bool Init(CommandPhaseModel model)
		{
			_clsState = new StatementMachine();
			_listInvalidCommands = GetInvalidCommands(model.GetSelectableCommands());
			_clsCommandModel = model;
			_iCtrlModel = CtrlMode.Surface;
			_isInputPossible = false;
			_uiCommandSurfaceList.Init(model.GetPresetCommand(), OnSelectedSurface, OnStartBattle);
			_uiCommandSurfaceList.isColliderEnabled = false;
			_uiCommandUnitSelect = UICommandUnitSelect.Instantiate(((Component)_prefabUICommandUnitSelect).GetComponent<UICommandUnitSelect>(), base.transform, model.GetSelectableCommands(), OnDecideUnitSelect, OnCancelUnitSelect);
			InitEnemyFleetInfos();
			panel.alpha = 0f;
			panel.widgetsAreStatic = true;
			return true;
		}

		private void InitEnemyFleetInfos()
		{
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			new BattleData();
			IEnumerable<ShipModel_BattleAll> source = from x in battleManager.Ships_e
				where x != null
				select x;
			int maxHP = (from x in source
				select x.HpStart).Sum();
			int num = (from x in source
				select x.HpPhaseStart).Sum();
			_uiEnemyFleetInfos.circleGauge.SetHPGauge(maxHP, num, num);
			for (int i = 0; i < 6; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = battleManager.Ships_e[i];
				_uiEnemyFleetInfos.hpBars.Add(((Component)_uiEnemyFleetInfos.shipHPBarAnchor.Find("EnemyHPBar" + (i + 1))).GetComponent<BtlCut_HPBar>());
				if (shipModel_BattleAll != null)
				{
					_uiEnemyFleetInfos.hpBars[i].SetHpBar(shipModel_BattleAll);
				}
				else
				{
					_uiEnemyFleetInfos.hpBars[i].Hide();
				}
			}
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

		public void Play(Action onFinished)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInCommand(_iCtrlModel);
			navigation.Show(Defines.PHASE_FADE_TIME, null);
			_actOnFinished = onFinished;
			BattleCutManager.SetTitleText(BattleCutPhase.Command);
			Show(delegate
			{
				_clsState.AddState(InitSurfaceSelect, UpdateSurfaceSelect);
			});
		}

		public bool Run()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return false;
		}

		private bool InitSurfaceSelect(object data)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInCommand(CtrlMode.Surface);
			_isInputPossible = true;
			_uiCommandSurfaceList.isColliderEnabled = true;
			return false;
		}

		private bool UpdateSurfaceSelect(object data)
		{
			if (!_isInputPossible)
			{
				return false;
			}
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.UP))
			{
				_uiCommandSurfaceList.Prev();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.DOWN))
			{
				_uiCommandSurfaceList.Next();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				_uiCommandSurfaceList.RemoveUnit();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.SHIKAKU))
			{
				_uiCommandSurfaceList.RemoveUnitAll();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				_uiCommandSurfaceList.OnSelectSurface();
			}
			return false;
		}

		private bool InitUnitSelect(object data)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInCommand(CtrlMode.Command);
			_isInputPossible = true;
			return false;
		}

		private bool UpdateUnitSelect(object data)
		{
			if (!_isInputPossible)
			{
				return false;
			}
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.UP))
			{
				_uiCommandUnitSelect.Prev();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.DOWN))
			{
				_uiCommandUnitSelect.Next();
			}
			else
			{
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					return _uiCommandUnitSelect.OnDecide();
				}
				if (keyControl.GetDown(KeyControl.KeyName.BATU))
				{
					return _uiCommandUnitSelect.OnCancel();
				}
			}
			return false;
		}

		private void Show(Action onFinished)
		{
			panel.widgetsAreStatic = false;
			panel.transform.LTCancel();
			panel.transform.LTValue(panel.alpha, 1f, Defines.PHASE_FADE_TIME).setDelay(0.5f).setEase(LeanTweenType.linear)
				.setOnUpdate(delegate(float x)
				{
					panel.alpha = x;
				})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		private void Hide(Action onFinished)
		{
			panel.transform.LTCancel();
			panel.transform.LTValue(panel.alpha, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					panel.widgetsAreStatic = true;
					Dlg.Call(ref onFinished);
				});
		}

		private void OnSelectedSurface(UICommandLabelButton selectedButton)
		{
			_clsState.Clear();
			_isInputPossible = false;
			BattleCommand iCommand = (!_listInvalidCommands.Contains(selectedButton.battleCommand)) ? selectedButton.battleCommand : BattleCommand.Sekkin;
			_uiCommandUnitSelect.Show(iCommand, delegate
			{
				_clsState.AddState(InitUnitSelect, UpdateUnitSelect);
			});
		}

		private void OnDecideUnitSelect(BattleCommand iCommand)
		{
			_uiCommandSurfaceList.selectedSurface.SetCommand(iCommand);
			_clsState.Clear();
			_uiCommandUnitSelect.Hide(delegate
			{
				_clsState.AddState(InitSurfaceSelect, UpdateSurfaceSelect);
			});
		}

		private void OnCancelUnitSelect()
		{
			_clsState.Clear();
			_uiCommandUnitSelect.Hide(delegate
			{
				_clsState.AddState(InitSurfaceSelect, UpdateSurfaceSelect);
			});
		}

		private bool OnStartBattle(List<BattleCommand> commands)
		{
			if (!_clsCommandModel.SetCommand(commands))
			{
				return false;
			}
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			_clsState.Clear();
			_isInputPossible = false;
			Hide(delegate
			{
				Dlg.Call(ref _actOnFinished);
			});
			return true;
		}
	}
}
