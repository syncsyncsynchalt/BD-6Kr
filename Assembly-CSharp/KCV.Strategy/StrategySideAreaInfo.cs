using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySideAreaInfo : MonoBehaviour
	{
		[SerializeField]
		private UILabel TankerNum;

		[SerializeField]
		private StrategySideEscortDeck escortDeck;

		[SerializeField]
		private UILabel[] TurnGetMaterialNums;

		[SerializeField]
		private UIWidget myUIWidget;

		public void UpdateSideAreaPanel()
		{
			MapAreaModel focusAreaModel = StrategyTopTaskManager.Instance.GetAreaMng().FocusAreaModel;
			int countNoMove = focusAreaModel.GetTankerCount().GetCountNoMove();
			int countMove = focusAreaModel.GetTankerCount().GetCountMove();
			setTankerCount(countNoMove, countMove);
			escortDeck.UpdateEscortDeck(focusAreaModel.GetEscortDeck());
			setMaterialNums(focusAreaModel, countNoMove);
		}

		private void setTankerCount(int tankerCount, int moveTankerCount)
		{
			if (moveTankerCount == 0)
			{
				TankerNum.text = "× " + tankerCount;
			}
			else
			{
				TankerNum.text = "× " + tankerCount + " + " + moveTankerCount;
			}
		}

		private void setMaterialNums(MapAreaModel areaModel, int tankerCount)
		{
			TurnGetMaterialNums[0].text = "× " + areaModel.GetResources(tankerCount)[enumMaterialCategory.Fuel];
			TurnGetMaterialNums[1].text = "× " + areaModel.GetResources(tankerCount)[enumMaterialCategory.Bull];
			TurnGetMaterialNums[2].text = "× " + areaModel.GetResources(tankerCount)[enumMaterialCategory.Steel];
			TurnGetMaterialNums[3].text = "× " + areaModel.GetResources(tankerCount)[enumMaterialCategory.Bauxite];
		}

		public void EnterAreaInfoPanel(float delay)
		{
			UpdateSideAreaPanel();
			TweenAlpha.Begin(base.gameObject, 0.2f, 1f).delay = delay;
		}

		public void ExitAreaInfoPanel()
		{
			TweenAlpha.Begin(base.gameObject, 0.2f, 0f).delay = 0f;
		}

		public void setVisible(bool isVisible)
		{
			if (isVisible)
			{
				myUIWidget.alpha = 1f;
			}
			else
			{
				myUIWidget.alpha = 0f;
			}
		}
	}
}
