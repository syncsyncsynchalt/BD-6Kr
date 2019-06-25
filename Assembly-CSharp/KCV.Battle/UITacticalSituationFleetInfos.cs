using KCV.Battle.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[Serializable]
	public class UITacticalSituationFleetInfos : IDisposable
	{
		[SerializeField]
		private UIWidget _uiWidget;

		[SerializeField]
		private UILabel _uiFleetName;

		[SerializeField]
		private UITexture _uiLine;

		[SerializeField]
		private UIGrid _uiShipsAnchor;

		private FleetType _iFleetType;

		private List<UITacticalSituationShipBanner> _listShipBanners;

		public UIWidget widget => _uiWidget;

		public FleetType fleetType => _iFleetType;

		public void Dispose()
		{
			Mem.Del(ref _uiWidget);
			Mem.Del(ref _uiFleetName);
			Mem.Del(ref _uiLine);
			Mem.Del(ref _uiShipsAnchor);
			Mem.Del(ref _iFleetType);
			Mem.DelListSafe(ref _listShipBanners);
		}

		public bool Init(FleetType iType, string strFleetName, List<ShipModel_BattleAll> shipList, UITacticalSituationShipBanner prefab)
		{
			_iFleetType = iType;
			_uiFleetName.text = strFleetName;
			CreateShipBanners(shipList, prefab);
			return true;
		}

		private void CreateShipBanners(List<ShipModel_BattleAll> shipList, UITacticalSituationShipBanner prefab)
		{
			_listShipBanners = new List<UITacticalSituationShipBanner>();
			shipList.ForEach(delegate(ShipModel_BattleAll x)
			{
				if (x != null)
				{
					_listShipBanners.Add(UITacticalSituationShipBanner.Instantiate(prefab, _uiShipsAnchor.transform, x));
				}
			});
		}
	}
}
