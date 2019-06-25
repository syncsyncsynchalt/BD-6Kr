using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySidePanel : MonoBehaviour
	{
		[SerializeField]
		private StrategySideAreaInfo sideAreaInfo;

		[SerializeField]
		private UITexture blackBG;

		private UIWidget AreaInfoWidget;

		private void Awake()
		{
			AreaInfoWidget = sideAreaInfo.GetComponent<UIWidget>();
			AreaInfoWidget.alpha = 0f;
		}

		public void Init(StrategyInfoManager.Mode nowMode)
		{
		}

		public void UpdateSidePanel(StrategyInfoManager.Mode nowMode)
		{
			switch (nowMode)
			{
			case StrategyInfoManager.Mode.AreaInfo:
				sideAreaInfo.UpdateSideAreaPanel();
				break;
			}
		}

		public void ChangeMode(StrategyInfoManager.Mode nowMode)
		{
			if (nowMode != 0 && nowMode == StrategyInfoManager.Mode.DeckInfo)
			{
				sideAreaInfo.ExitAreaInfoPanel();
			}
		}

		public void SetMode(StrategyInfoManager.Mode nowMode)
		{
			if (nowMode != 0 && nowMode == StrategyInfoManager.Mode.DeckInfo)
			{
				sideAreaInfo.setVisible(isVisible: false);
			}
		}

		public void Enter(StrategyInfoManager.Mode nowMode, float delay)
		{
			switch (nowMode)
			{
			case StrategyInfoManager.Mode.AreaInfo:
				sideAreaInfo.EnterAreaInfoPanel(delay);
				break;
			}
		}

		public void Exit(StrategyInfoManager.Mode nowMode)
		{
			switch (nowMode)
			{
			case StrategyInfoManager.Mode.AreaInfo:
				sideAreaInfo.ExitAreaInfoPanel();
				break;
			}
		}

		public void EnterBG()
		{
			if (!blackBG.enabled)
			{
				blackBG.enabled = true;
			}
			TweenAlpha.Begin(blackBG.gameObject, 0.2f, 0.5f);
		}

		public void ExitBG(bool isPanelOff = false)
		{
			TweenAlpha.Begin(blackBG.gameObject, 0.2f, 0f);
			if (isPanelOff)
			{
				blackBG.enabled = false;
			}
		}
	}
}
