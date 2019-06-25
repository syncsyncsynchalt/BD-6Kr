using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutBattle : BaseBattleCutState
	{
		private ProdBCBattle _prodBCBattle;

		private bool _isNightCombat;

		public ProdBCBattle prodBCBattle => _prodBCBattle;

		public bool isNightCombat
		{
			get
			{
				return _isNightCombat;
			}
			set
			{
				_isNightCombat = value;
			}
		}

		public override bool Init(object data)
		{
			Observable.FromCoroutine((UniRx.IObserver<ProdBCBattle> observer) => CreateBCBattle(observer)).Subscribe(delegate(ProdBCBattle x)
			{
				Action callback = (!isNightCombat) ? ((Action)delegate
				{
					BattleCutManager.ReqPhase(BattleCutPhase.WithdrawalDecision);
				}) : ((Action)delegate
				{
					BattleCutManager.ReqPhase(BattleCutPhase.Judge);
				});
				x.Play(_isNightCombat, callback);
			});
			return false;
		}

		public override bool Run(object data)
		{
			return IsCheckPhase(BattleCutPhase.DayBattle);
		}

		public override bool Terminate(object data)
		{
			return false;
		}

		private IEnumerator CreateBCBattle(UniRx.IObserver<ProdBCBattle> observer)
		{
			if (_prodBCBattle == null)
			{
				_prodBCBattle = ProdBCBattle.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdBCBattle).GetComponent<ProdBCBattle>(), BattleCutManager.GetSharedPlase());
			}
			yield return null;
			observer.OnNext(_prodBCBattle);
			observer.OnCompleted();
		}
	}
}
