using Common.Enum;
using KCV.Battle.Utils;
using local.utils;
using Server_Models;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutAdvancingWithdrawalDC : BaseBattleCutState
	{
		private ProdBCAdvancingWithdrawalDC _prodBCAdvancingWithdrawalDC;

		public override bool Init(object data)
		{
			Observable.FromCoroutine((IObserver<ProdBCAdvancingWithdrawalDC> observer) => CreateBCAdvancingWithdrawalDC(observer)).Subscribe(delegate(ProdBCAdvancingWithdrawalDC x)
			{
				x.Play(OnDecideAdvancingWithdrawal);
			});
			return false;
		}

		public override bool Terminate(object data)
		{
			return base.Terminate(data);
		}

		public override bool Run(object data)
		{
			if (_prodBCAdvancingWithdrawalDC != null)
			{
				_prodBCAdvancingWithdrawalDC.Run();
			}
			return false;
		}

		private IEnumerator CreateBCAdvancingWithdrawalDC(IObserver<ProdBCAdvancingWithdrawalDC> observer)
		{
			_prodBCAdvancingWithdrawalDC = ProdBCAdvancingWithdrawalDC.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdBCAdvancingWithdrawalDC).GetComponent<ProdBCAdvancingWithdrawalDC>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleManager().Ships_f[0], BattleCutManager.GetBattleType());
			yield return null;
			observer.OnNext(_prodBCAdvancingWithdrawalDC);
			observer.OnCompleted();
		}

		private void OnDecideAdvancingWithdrawal(AdvancingWithdrawalDCType iType, ShipRecoveryType iRecoveryType)
		{
			RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(BattleCutManager.GetMapManager(), iRecoveryType));
			switch (iType)
			{
			case AdvancingWithdrawalDCType.Withdrawal:
				if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: true);
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						Mst_DataManager.Instance.PurgeUIBattleMaster();
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					});
				}
				break;
			case AdvancingWithdrawalDCType.Youin:
			case AdvancingWithdrawalDCType.Megami:
				TrophyUtil.Unlock_At_GoNext();
				BattleCutManager.EndBattleCut(iRecoveryType);
				break;
			case AdvancingWithdrawalDCType.AdvancePrimary:
				BattleCutManager.GetMapManager().ChangeCurrentDeck();
				BattleCutManager.EndBattleCut(iRecoveryType);
				break;
			}
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
			SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
			Object.Destroy(_prodBCAdvancingWithdrawalDC.gameObject);
			Mem.Del(ref _prodBCAdvancingWithdrawalDC);
		}
	}
}
