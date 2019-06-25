using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using local.utils;
using System;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleAdvancingWithdrawalDC : BaseBattleTask
	{
		private ProdAdvancingWithDrawalDC _prodAdvancingWithDrawalDC;

		private AsyncOperation _async;

		private Action<ShipRecoveryType> _actOnGotoSortieMap;

		public TaskBattleAdvancingWithdrawalDC(Action<ShipRecoveryType> onGotoSortieMap)
		{
			_actOnGotoSortieMap = onGotoSortieMap;
		}

		protected override void Dispose(bool isDisposing)
		{
			Mem.Del(ref _prodAdvancingWithDrawalDC);
			Mem.Del(ref _actOnGotoSortieMap);
		}

		protected override bool Init()
		{
			BattleTaskManager.GetPrefabFile().battleShutter.Init(BaseShutter.ShutterMode.Close);
			if (BattleTaskManager.GetBattleManager().IsPractice)
			{
				_async = Application.LoadLevelAsync(Generics.Scene.Strategy.ToString());
				_async.allowSceneActivation = true;
			}
			else
			{
				_prodAdvancingWithDrawalDC = ProdAdvancingWithDrawalDC.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabProdAdvancingWithDrawalDC).GetComponent<ProdAdvancingWithDrawalDC>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, BattleTaskManager.GetRootType());
				_prodAdvancingWithDrawalDC.Play(DecideAdvancinsWithDrawalBtn);
			}
			return true;
		}

		protected override bool UnInit()
		{
			_prodAdvancingWithDrawalDC = null;
			return true;
		}

		protected override bool Update()
		{
			return _prodAdvancingWithDrawalDC.Run();
		}

		private void DecideAdvancinsWithDrawalBtn(UIHexButton btn)
		{
			if (BattleTaskManager.IsSortieBattle())
			{
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = ((btn.index != 0) ? Generics.Scene.SortieAreaMap : Generics.Scene.Strategy);
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = ((SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene == Generics.Scene.SortieAreaMap) ? true : false);
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					if (btn.index == 1)
					{
						TrophyUtil.Unlock_At_GoNext();
						RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.Personnel));
						Dlg.Call(ref _actOnGotoSortieMap, ShipRecoveryType.Personnel);
					}
					else if (btn.index == 2)
					{
						TrophyUtil.Unlock_At_GoNext();
						RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.Goddes));
						Dlg.Call(ref _actOnGotoSortieMap, ShipRecoveryType.Goddes);
					}
					else if (btn.index == 3)
					{
						MapManager mapManager = SortieBattleTaskManager.GetMapManager();
						mapManager.ChangeCurrentDeck();
						RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(mapManager, ShipRecoveryType.None));
						Dlg.Call(ref _actOnGotoSortieMap, ShipRecoveryType.None);
					}
					else
					{
						RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					}
				});
			}
		}
	}
}
