using KCV.BattleCut;
using local.models;
using LT.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIWidget))]
	public class UIFleetInfos : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabUIShortCutHPBar;

		[SerializeField]
		private Transform _traAnchor;

		private List<BtlCut_HPBar> _listHPBar;

		private UIWidget _uiWidget;

		public UIWidget widget => this.GetComponentThis(ref _uiWidget);

		public static UIFleetInfos Instantiate(UIFleetInfos prefab, Transform parent, Vector3 pos, List<ShipModel_BattleAll> ships)
		{
			UIFleetInfos uIFleetInfos = Object.Instantiate(prefab);
			uIFleetInfos.transform.parent = parent;
			uIFleetInfos.transform.localScaleOne();
			uIFleetInfos.transform.localPosition = pos;
			uIFleetInfos.Init(ships);
			return uIFleetInfos;
		}

		public bool Init(List<ShipModel_BattleAll> ships)
		{
			_listHPBar = new List<BtlCut_HPBar>();
			ships.ForEach(delegate(ShipModel_BattleAll x)
			{
				if (x != null)
				{
					_listHPBar.Add(BtlCut_HPBar.Instantiate(((Component)_prefabUIShortCutHPBar).GetComponent<BtlCut_HPBar>(), _traAnchor, x, isAfter: true, BattleTaskManager.GetBattleManager()));
					_listHPBar[x.Index].SetHPLabelColor(Color.white);
				}
			});
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabUIShortCutHPBar);
			Mem.Del(ref _traAnchor);
			Mem.DelListSafe(ref _listHPBar);
			Mem.Del(ref _uiWidget);
		}

		public LTDescr Show()
		{
			return _uiWidget.transform.LTValue(_uiWidget.alpha, 1f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiWidget.alpha = x;
			});
		}

		public LTDescr Hide()
		{
			return _uiWidget.transform.LTValue(_uiWidget.alpha, 0f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiWidget.alpha = x;
			});
		}
	}
}
