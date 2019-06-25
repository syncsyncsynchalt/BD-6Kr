using local.utils;
using System;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBattleEnd : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiEndLabel;

		[SerializeField]
		private TweenAlpha _taAlpha;

		public static ProdBattleEnd Instantiate(ProdBattleEnd prefab, Transform parent)
		{
			ProdBattleEnd prodBattleEnd = UnityEngine.Object.Instantiate(prefab);
			prodBattleEnd.transform.parent = parent;
			prodBattleEnd.transform.localPositionZero();
			prodBattleEnd.transform.localScaleOne();
			return prodBattleEnd;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiEndLabel);
			Mem.Del(ref _taAlpha);
		}

		public void Play(Action callback)
		{
			TrophyUtil.Unlock_At_SCutBattle();
			_taAlpha.PlayForward();
			_taAlpha.SetOnFinished(delegate
			{
				if (callback != null)
				{
					callback();
				}
				Observable.NextFrame().Subscribe(delegate
				{
					UnityEngine.Object.Destroy(base.gameObject);
				});
			});
		}
	}
}
