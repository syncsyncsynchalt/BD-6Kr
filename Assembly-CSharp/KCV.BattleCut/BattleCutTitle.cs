using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class BattleCutTitle : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiPhaseTitle;

		private UIPanel _uiPanel;

		private Dictionary<BattleCutPhase, string> _dicPhaseTitle;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static BattleCutTitle Instantiate(BattleCutTitle prefab, Transform parent, Vector3 pos)
		{
			return UnityEngine.Object.Instantiate(prefab);
		}

		private void Awake()
		{
			_dicPhaseTitle = new Dictionary<BattleCutPhase, string>();
			_dicPhaseTitle.Add(BattleCutPhase.BattleCutPhase_ST, "Formation");
			_dicPhaseTitle.Add(BattleCutPhase.Command, "Command");
			_dicPhaseTitle.Add(BattleCutPhase.DayBattle, "Battle");
			_dicPhaseTitle.Add(BattleCutPhase.Battle_End, "Battle End");
			_dicPhaseTitle.Add(BattleCutPhase.WithdrawalDecision, "Decision");
			_dicPhaseTitle.Add(BattleCutPhase.Judge, "Result");
			_dicPhaseTitle.Add(BattleCutPhase.NightBattle, "NightBattle");
			_dicPhaseTitle.Add(BattleCutPhase.Result, "Result");
			_dicPhaseTitle.Add(BattleCutPhase.AdvancingWithdrawal, "Decision");
			panel.widgetsAreStatic = true;
		}

		private void OnDestroy()
		{
			_uiPhaseTitle.transform.LTCancel();
			_uiPhaseTitle = null;
			_uiPanel = null;
			Mem.DelDictionarySafe(ref _dicPhaseTitle);
		}

		public void SetPhaseText(BattleCutPhase iPhase)
		{
			if (_dicPhaseTitle.ContainsKey(iPhase) && (_uiPhaseTitle.text == null || !(_uiPhaseTitle.text == _dicPhaseTitle[iPhase])))
			{
				panel.widgetsAreStatic = false;
				float fadeTime = 0.2f;
				_uiPhaseTitle.transform.LTCancel();
				_uiPhaseTitle.transform.LTValue(_uiPhaseTitle.alpha, 0f, fadeTime).setOnUpdate(delegate(float x)
				{
					_uiPhaseTitle.alpha = x;
				}).setEase(LeanTweenType.linear)
					.setOnComplete((Action)delegate
					{
						_uiPhaseTitle.text = _dicPhaseTitle[iPhase];
						_uiPhaseTitle.transform.LTValue(_uiPhaseTitle.alpha, 0.5f, fadeTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
						{
							_uiPhaseTitle.alpha = x;
						})
							.setOnComplete((Action)delegate
							{
								panel.widgetsAreStatic = true;
							});
					});
			}
		}
	}
}
