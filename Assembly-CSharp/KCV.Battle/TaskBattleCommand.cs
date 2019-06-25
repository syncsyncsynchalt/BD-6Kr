using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models.battle;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleCommand : BaseBattleTask
	{
		private ProdBattleCommandSelect _prodBattleCommandSelect;

		private RationModel _clsRationModel;

		private ProdCombatRation _prodCombatRation;

		public ProdBattleCommandSelect prodBattleCommandSelect => _prodBattleCommandSelect;

		protected override bool Init()
		{
			_prodBattleCommandSelect = ProdBattleCommandSelect.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdBattleCommandSelect).GetComponent<ProdBattleCommandSelect>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, BattleTaskManager.GetBattleManager().GetCommandPhaseModel());
			_clsRationModel = BattleTaskManager.GetBattleManager().GetRationModel();
			if (_clsRationModel != null)
			{
				_prodCombatRation = ProdCombatRation.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdCombatRation).GetComponent<ProdCombatRation>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsRationModel);
				_prodCombatRation.Play(OnCombatRationFinished);
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Register(delegate
				{
					Mem.DelComponentSafe(ref _prodCombatRation);
				});
			}
			else
			{
				OnCombatRationFinished();
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del(ref _clsRationModel);
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.Command);
		}

		private void OnCombatRationFinished()
		{
			_clsState = new StatementMachine();
			_clsState.AddState(InitCommandSelect, UpdateCommandSelect);
		}

		private bool InitCommandSelect(object data)
		{
			Observable.FromCoroutine(() => _prodBattleCommandSelect.PlayShowAnimation(delegate
			{
				_clsState.AddState(InitCommandBuffer, UpdateCommandBuffer);
			})).Subscribe();
			return false;
		}

		private bool UpdateCommandSelect(object data)
		{
			if (_prodBattleCommandSelect != null)
			{
				_prodBattleCommandSelect.Run();
				return false;
			}
			return true;
		}

		private bool InitCommandBuffer(object data)
		{
			EffectModel effectModel = BattleTaskManager.GetBattleManager().GetOpeningEffectData();
			if (effectModel != null)
			{
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer = ProdBattleCommandBuffer.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdBattleCommandBuffer).GetComponent<ProdBattleCommandBuffer>(), BattleTaskManager.GetStage(), effectModel, 0);
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer.Play(delegate
				{
					if (effectModel.Withdrawal)
					{
						OnCommandBufferFinished2Withdrawal();
					}
					else
					{
						OnCommandBufferFinished();
					}
				});
				_prodBattleCommandSelect.DiscardAfterFadeIn().setOnComplete((Action)delegate
				{
					Mem.DelComponentSafe(ref _prodBattleCommandSelect);
				});
			}
			else
			{
				OnCommandBufferFinished();
				Observable.TimerFrame(20, FrameCountType.EndOfFrame).Subscribe(delegate
				{
					_prodBattleCommandSelect.DiscardAfterFadeIn().setOnComplete((Action)delegate
					{
						Mem.DelComponentSafe(ref _prodBattleCommandSelect);
					});
				});
			}
			return false;
		}

		private bool UpdateCommandBuffer(object data)
		{
			return true;
		}

		private void OnCommandBufferFinished2Withdrawal()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			BattleTaskManager.ReqPhase(BattlePhase.WithdrawalDecision);
		}

		private void OnCommandBufferFinished()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			BattleTaskManager.ReqPhase(BattleUtils.NextPhase(BattlePhase.Command));
		}
	}
}
