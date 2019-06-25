using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using Server_Models;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutFlagshipWreck : BaseBattleCutState
	{
		private ProdFlagshipWreck _prodFlagshipWreck;

		private AsyncOperation _async;

		public override bool Init(object data)
		{
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
			_prodFlagshipWreck = ProdFlagshipWreck.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdFlagshipWreck).GetComponent<ProdFlagshipWreck>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleManager().Ships_f[0], BattleCutManager.GetMapManager().Deck, BattleCutManager.GetKeyControl(), isBattleCut: true);
			_prodFlagshipWreck.Play(delegate
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: true);
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
					SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
					Mst_DataManager.Instance.PurgeUIBattleMaster();
					SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
					RetentionData.SetData(BattleUtils.GetRetentionDataFlagshipWreck(BattleCutManager.GetMapManager(), ShipRecoveryType.None));
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			});
			return false;
		}

		public override bool Terminate(object data)
		{
			if (_prodFlagshipWreck != null && _prodFlagshipWreck.gameObject != null)
			{
				Object.Destroy(_prodFlagshipWreck.gameObject);
			}
			_prodFlagshipWreck = null;
			return false;
		}

		public override bool Run(object data)
		{
			_prodFlagshipWreck.Run();
			return IsCheckPhase(BattleCutPhase.FlagshipWreck);
		}
	}
}
