using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyUIMapManager : MonoBehaviour
	{
		[SerializeField]
		private StrategyHexTileManager tileManager;

		[SerializeField]
		private StrategyShipManager shipIconManager;

		public StrategyHexTileManager TileManager
		{
			get
			{
				return tileManager;
			}
			private set
			{
				tileManager = value;
			}
		}

		public StrategyShipManager ShipIconManager
		{
			get
			{
				return shipIconManager;
			}
			private set
			{
				shipIconManager = value;
			}
		}
	}
}
