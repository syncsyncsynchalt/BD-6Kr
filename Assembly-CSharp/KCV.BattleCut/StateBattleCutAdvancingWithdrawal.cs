using Common.Enum;
using KCV.Battle.Utils;
using local.managers;
using Server_Models;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutAdvancingWithdrawal : BaseBattleCutState
	{
		private ProdBCAdvancingWithdrawal _prodBCAdvancingWithdrawal;

		public override bool Init(object data)
		{
			Observable.FromCoroutine((IObserver<ProdBCAdvancingWithdrawal> observer) => CreateAdvancingWithdrawal(observer)).Subscribe(delegate(ProdBCAdvancingWithdrawal x)
			{
				x.Play(OnDecideAdvancingWithdrawal);
			});
			return false;
		}

		public override bool Run(object data)
		{
			if (_prodBCAdvancingWithdrawal != null)
			{
				_prodBCAdvancingWithdrawal.Run();
			}
			return IsCheckPhase(BattleCutPhase.AdvancingWithdrawal);
		}

		public override bool Terminate(object data)
		{
			return false;
		}

		private IEnumerator CreateAdvancingWithdrawal(IObserver<ProdBCAdvancingWithdrawal> observer)
		{
			_prodBCAdvancingWithdrawal = ProdBCAdvancingWithdrawal.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdBCAdvancingWithdrawal).GetComponent<ProdBCAdvancingWithdrawal>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleType());
			yield return null;
			observer.OnNext(_prodBCAdvancingWithdrawal);
			observer.OnCompleted();
		}

		private void OnDecideAdvancingWithdrawal(AdvancingWithdrawalType iType)
		{
			switch (iType)
			{
			case AdvancingWithdrawalType.Withdrawal:
				if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: true);
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						Mst_DataManager.Instance.PurgeUIBattleMaster();
						RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawal(BattleCutManager.GetMapManager(), ShipRecoveryType.None));
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					});
				}
				break;
			case AdvancingWithdrawalType.Advance:
				BattleCutManager.EndBattleCut(ShipRecoveryType.None);
				break;
			case AdvancingWithdrawalType.AdvancePrimary:
			{
				MapManager mapManager = BattleCutManager.GetMapManager();
				mapManager.ChangeCurrentDeck();
				BattleCutManager.EndBattleCut(ShipRecoveryType.None);
				break;
			}
			}
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
			SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
			Object.Destroy(_prodBCAdvancingWithdrawal.gameObject);
			Mem.Del(ref _prodBCAdvancingWithdrawal);
		}
	}
}
