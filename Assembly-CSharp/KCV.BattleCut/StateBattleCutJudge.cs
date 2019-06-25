using KCV.Battle.Production;
using KCV.Battle.Utils;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutJudge : BaseBattleCutState
	{
		private ProdWinRankJudge _prodWinRunkJudge;

		public override bool Init(object data)
		{
			BattleCutManager.SetTitleText(BattleCutPhase.Battle_End);
			ProdBattleEnd prodBattleEnd = ProdBattleEnd.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdBattleEnd).GetComponent<ProdBattleEnd>(), BattleCutManager.GetSharedPlase());
			prodBattleEnd.Play(delegate
			{
				BattleDefines.SOUND_KEEP.BGMVolume = SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM;
				SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume * 0.5f;
				SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume * 0.5f;
				_prodWinRunkJudge = ProdWinRankJudge.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdWinRunkJudge).GetComponent<ProdWinRankJudge>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleManager().GetBattleResult(), isBattleCut: true);
				Observable.FromCoroutine(_prodWinRunkJudge.StartBattleJudge).Subscribe(delegate
				{
					BattleCutManager.ReqPhase(BattleCutPhase.Result);
				});
			});
			return false;
		}

		public override bool Run(object data)
		{
			return IsCheckPhase(BattleCutPhase.Judge);
		}

		public override bool Terminate(object data)
		{
			Object.Destroy(_prodWinRunkJudge.gameObject);
			_prodWinRunkJudge = null;
			return false;
		}
	}
}
