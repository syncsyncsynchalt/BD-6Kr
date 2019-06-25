using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyUIModel : MonoBehaviour
	{
		[SerializeField]
		private StrategyUIMapManager uiMapManager;

		[SerializeField]
		private StrategyInfoManager infoManager;

		[SerializeField]
		private StrategyAreaManager areaManager;

		[SerializeField]
		private StrategyCamera mapCamera;

		[SerializeField]
		private Transform overView;

		[SerializeField]
		private Camera overCamera;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private StrategyShipCharacter character;

		[SerializeField]
		private UIHowToStrategy howToStrategy;

		public StrategyUIMapManager UIMapManager
		{
			get
			{
				return uiMapManager;
			}
			private set
			{
				uiMapManager = value;
			}
		}

		public StrategyInfoManager InfoManager => infoManager;

		public StrategyAreaManager AreaManager => areaManager;

		public StrategyCamera MapCamera => mapCamera;

		public Transform OverView => overView;

		public Camera OverCamera => overCamera;

		public CommonDialog CommonDialog => commonDialog;

		public StrategyShipCharacter Character => character;

		public UIHowToStrategy HowToStrategy => howToStrategy;

		private void OnDestroy()
		{
			uiMapManager = null;
			infoManager = null;
			areaManager = null;
			mapCamera = null;
			overView = null;
			overCamera = null;
			commonDialog = null;
			character = null;
			howToStrategy = null;
		}
	}
}
