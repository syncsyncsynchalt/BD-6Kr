using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleFlagshipWreck : BaseBattleTask
	{
		private ProdFlagshipWreck _prodFlagshipWreck;

		protected override bool Init()
		{
			base.Init();
			_clsState = new StatementMachine();
			BattleTaskManager.GetPrefabFile().battleShutter.Init(BaseShutter.ShutterMode.Close);
			_clsState.AddState(_initFlagshipWreck, _updateFlagshipWreck);
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			if (_prodFlagshipWreck != null)
			{
				_prodFlagshipWreck.Discard();
			}
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return ChkChangePhase(BattlePhase.FlagshipWreck);
		}

		private bool _initFlagshipWreck(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => CreateFlagshipWreck(observer)).Subscribe(delegate
			{
				_prodFlagshipWreck.Play(_onFlagshipWreckFinished);
			});
			return false;
		}

		private IEnumerator CreateFlagshipWreck(IObserver<bool> observer)
		{
			_prodFlagshipWreck = ProdFlagshipWreck.Instantiate(parent: BattleTaskManager.GetBattleCameras().cutInCamera.transform, prefab: ((Component)BattleTaskManager.GetPrefabFile().prefabProdFlagshipWreck).GetComponent<ProdFlagshipWreck>(), flagShip: BattleTaskManager.GetBattleManager().Ships_f[0], deck: SortieBattleTaskManager.GetMapManager().Deck, input: BattleTaskManager.GetKeyControl(), isBattleCut: false);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateFlagshipWreck(object data)
		{
			return _prodFlagshipWreck.Run();
		}

		private void _onFlagshipWreckFinished()
		{
			if (BattleTaskManager.IsSortieBattle() && SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataFlagshipWreck(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
		}
	}
}
