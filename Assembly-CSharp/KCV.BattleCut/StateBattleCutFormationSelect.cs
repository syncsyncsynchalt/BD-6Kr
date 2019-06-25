using Common.Enum;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutFormationSelect : BaseBattleCutState
	{
		private ProdFormationSelect _prodFormationSelect;

		public override bool Init(object data)
		{
			Observable.FromCoroutine((IObserver<ProdFormationSelect> observer) => CreateFormationSelect(observer)).Subscribe(delegate(ProdFormationSelect x)
			{
				x.Play(delegate(BattleFormationKinds1 formation)
				{
					BattleCutManager.StartBattle(formation);
				});
			});
			return false;
		}

		public override bool Run(object data)
		{
			if (_prodFormationSelect != null)
			{
				_prodFormationSelect.Run();
			}
			return IsCheckPhase(BattleCutPhase.BattleCutPhase_ST);
		}

		public override bool Terminate(object data)
		{
			Object.Destroy(_prodFormationSelect.gameObject);
			_prodFormationSelect = null;
			return false;
		}

		private IEnumerator CreateFormationSelect(IObserver<ProdFormationSelect> observer)
		{
			_prodFormationSelect = ProdFormationSelect.Instantiate(((Component)BattleCutManager.GetPrefabFile().prefabProdFormation).GetComponent<ProdFormationSelect>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetMapManager().Deck);
			yield return null;
			observer.OnNext(_prodFormationSelect);
			observer.OnCompleted();
		}
	}
}
