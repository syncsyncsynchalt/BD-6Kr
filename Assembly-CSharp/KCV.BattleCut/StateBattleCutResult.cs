using KCV.Battle.Utils;
using System;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutResult : BaseBattleCutState
	{
		private ProdBCResult _prodBCResult;

		public override bool Init(object data)
		{
			_prodBCResult = ProdBCResult.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdResult).GetComponent<ProdBCResult>(), BattleCutManager.GetSharedPlase());
			_prodBCResult.StartAnimation(OnResultAnimFinished);
			return false;
		}

		public override bool Run(object data)
		{
			_prodBCResult.Run();
			return IsCheckPhase(BattleCutPhase.Result);
		}

		public override bool Terminate(object data)
		{
			UnityEngine.Object.Destroy(_prodBCResult.gameObject);
			Mem.Del(ref _prodBCResult);
			return false;
		}

		private void OnResultAnimFinished()
		{
			if (BattleCutManager.GetBattleManager().IsPractice)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
				SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
				{
					BattleCutManager.EndBattleCut();
				});
			}
			else
			{
				BattleCutManager.ReqPhase(BattleCutPhase.ClearReward);
			}
		}
	}
}
