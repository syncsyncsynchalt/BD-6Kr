using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using local.models.battle;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleResult : BaseBattleTask
	{
		private ProdVeteransReport _prodVeteransReport;

		private BattleResultModel _clsBattleResult;

		protected override bool Init()
		{
			BattleTaskManager.GetTorpedoHpGauges().SetDestroy();
			SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: true);
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				BattleTaskManager.GetBattleCameras().SetFieldCameraEnabled(isEnabled: false);
				KCV.Utils.SoundUtils.StopFadeBGM(0.25f, null);
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				BattleTaskManager.DestroyUnneccessaryObject2Result();
				Observable.FromCoroutine(BattleUtils.ClearMemory).Subscribe(delegate
				{
					_clsBattleResult = BattleTaskManager.GetBattleManager().GetBattleResult();
					BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					cutInEffectCamera.glowEffect.enabled = false;
					cutInEffectCamera.isCulling = true;
					_prodVeteransReport = ProdVeteransReport.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdVeteransReport).GetComponent<ProdVeteransReport>(), cutInEffectCamera.transform, _clsBattleResult);
					_clsState = new StatementMachine();
					_clsState.AddState(InitProdVeteransReport, UpdateProdVeteransReport);
				});
			});
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			_prodVeteransReport = null;
			_clsBattleResult = null;
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.Result);
		}

		private bool InitProdVeteransReport(object data)
		{
			Observable.FromCoroutine(() => _prodVeteransReport.CreateInstance((BattleTaskManager.GetBattleManager() is PracticeBattleManager) ? true : false)).Subscribe(delegate
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
				{
					_prodVeteransReport.PlayVeteransReport();
				});
			});
			return false;
		}

		private bool UpdateProdVeteransReport(object data)
		{
			if (!_prodVeteransReport.Run())
			{
				return false;
			}
			if (BattleTaskManager.GetBattleManager().IsPractice)
			{
				EndPhase(BattlePhase.AdvancingWithdrawal);
			}
			else
			{
				BattleTaskManager.ReqPhase(BattleUtils.NextPhase(BattlePhase.Result));
			}
			return true;
		}
	}
}
