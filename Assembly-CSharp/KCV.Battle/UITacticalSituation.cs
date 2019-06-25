using KCV.Battle.Utils;
using local.managers;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UITacticalSituation : MonoBehaviour
	{
		[Serializable]
		private struct AnimParams
		{
			public float showhideTime;

			public LeanTweenType showhideEase;
		}

		[Serializable]
		private class UIFrame : IDisposable
		{
			[SerializeField]
			private UIWidget _uiWidget;

			[SerializeField]
			private UILabel _uiLabel;

			[SerializeField]
			private UITexture _uiSeparator;

			public void Dispose()
			{
				Mem.Del(ref _uiWidget);
				Mem.Del(ref _uiLabel);
				Mem.Del(ref _uiSeparator);
			}
		}

		[SerializeField]
		private Transform _prefabUITacticalSituationShipBanner;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiBlur;

		[SerializeField]
		private UIFrame _uiFrame;

		[SerializeField]
		private List<UITacticalSituationFleetInfos> _listFleetInfos;

		[SerializeField]
		[Header("[Animation Parameter]")]
		private AnimParams _strAnimParams;

		private UIPanel _uiPanel;

		private Action _actOnBack;

		private bool _isInputPossible;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static UITacticalSituation Instantiate(UITacticalSituation prefab, Transform parent, BattleManager manager)
		{
			UITacticalSituation uITacticalSituation = UnityEngine.Object.Instantiate(prefab);
			uITacticalSituation.transform.parent = parent;
			uITacticalSituation.transform.localScaleOne();
			uITacticalSituation.transform.localPositionZero();
			uITacticalSituation.VirtualCtor(manager);
			return uITacticalSituation;
		}

		private void VirtualCtor(BattleManager manager)
		{
			_isInputPossible = false;
			panel.alpha = 0f;
			InitFleetsInfos(manager);
			panel.widgetsAreStatic = true;
		}

		private void InitFleetsInfos(BattleManager manager)
		{
			foreach (int value in Enum.GetValues(typeof(FleetType)))
			{
				if (value != 2)
				{
					_listFleetInfos[value].Init((FleetType)value, (value != 0) ? "敵艦隊" : "味方艦隊", (value != 0) ? manager.Ships_e.ToList() : manager.Ships_f.ToList(), ((Component)_prefabUITacticalSituationShipBanner).GetComponent<UITacticalSituationShipBanner>());
				}
			}
			Mem.Del(ref _prefabUITacticalSituationShipBanner);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabUITacticalSituationShipBanner);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiBlur);
			Mem.DelIDisposableSafe(ref _uiFrame);
			if (_listFleetInfos != null)
			{
				_listFleetInfos.ForEach(delegate(UITacticalSituationFleetInfos x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe(ref _listFleetInfos);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _actOnBack);
			Mem.Del(ref _isInputPossible);
		}

		public bool Init(Action onBack)
		{
			_actOnBack = onBack;
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (_isInputPossible && keyControl.GetDown(KeyControl.KeyName.SANKAKU))
			{
				Hide();
				return true;
			}
			return false;
		}

		public void Show(Action onFinished)
		{
			panel.widgetsAreStatic = false;
			panel.transform.LTCancel();
			panel.transform.LTValue(panel.alpha, 1f, _strAnimParams.showhideTime).setEase(_strAnimParams.showhideEase).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					_isInputPossible = true;
					Dlg.Call(ref onFinished);
				});
		}

		private void Hide()
		{
			_isInputPossible = false;
			panel.transform.LTCancel();
			panel.transform.LTValue(panel.alpha, 0f, _strAnimParams.showhideTime).setEase(_strAnimParams.showhideEase).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					panel.widgetsAreStatic = true;
					Dlg.Call(ref _actOnBack);
				});
		}
	}
}
