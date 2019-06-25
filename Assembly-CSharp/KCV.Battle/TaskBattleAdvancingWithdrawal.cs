using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleAdvancingWithdrawal : BaseBattleTask
	{
		private ProdAdvancingWithDrawalSelect _prodAdvancingWithDrawalSelect;

		private AsyncOperation _async;

		private Action<ShipRecoveryType> _actOnGotoSortieMap;

		public TaskBattleAdvancingWithdrawal(Action<ShipRecoveryType> onGotoSortieMap)
		{
			_actOnGotoSortieMap = onGotoSortieMap;
		}

		protected override void Dispose(bool isDisposing)
		{
			Mem.Del(ref _prodAdvancingWithDrawalSelect);
			Mem.Del(ref _actOnGotoSortieMap);
			base.Dispose(isDisposing);
		}

		protected override bool Init()
		{
			if (BattleTaskManager.GetBattleManager().IsPractice)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					ImmediateTermination();
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
			else
			{
				BattleTaskManager.GetPrefabFile().battleShutter.ReqMode(BaseShutter.ShutterMode.Close, delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
					{
						BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
						cutInEffectCamera.blur.enabled = false;
						_prodAdvancingWithDrawalSelect = ProdAdvancingWithDrawalSelect.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdAdvancingWithDrawalSelect).GetComponent<ProdAdvancingWithDrawalSelect>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, BattleTaskManager.GetRootType());
						_prodAdvancingWithDrawalSelect.Play(DecideAdvancinsWithDrawalBtn);
					});
				});
			}
			return true;
		}

		protected override bool UnInit()
		{
			_prodAdvancingWithDrawalSelect = null;
			return true;
		}

		protected override bool Update()
		{
			if (_prodAdvancingWithDrawalSelect != null)
			{
				return _prodAdvancingWithDrawalSelect.Run();
			}
			return ChkChangePhase(BattlePhase.AdvancingWithdrawal);
		}

		private void DecideAdvancinsWithDrawalBtn(UIHexButtonEx btn)
		{
			if (btn.index == 2)
			{
				MapManager mapManager = SortieBattleTaskManager.GetMapManager();
				mapManager.ChangeCurrentDeck();
			}
			if (BattleTaskManager.IsSortieBattle() && SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawal(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					if (btn.index == 0)
					{
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					}
					else
					{
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = true;
						Dlg.Call(ref _actOnGotoSortieMap, ShipRecoveryType.None);
					}
				});
			}
		}
	}
}
