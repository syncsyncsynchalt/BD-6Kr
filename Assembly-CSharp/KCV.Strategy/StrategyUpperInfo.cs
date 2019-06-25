using Common.Struct;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyUpperInfo : MonoBehaviour
	{
		private const float RotateSpeed = -10f;

		[SerializeField]
		private UILabel Year;

		[SerializeField]
		private UILabel Month;

		[SerializeField]
		private UILabel Day;

		[SerializeField]
		private UILabel MovingTankerNum;

		[SerializeField]
		private UILabel TankerNum;

		[SerializeField]
		private TweenPosition TweenPos;

		[SerializeField]
		private UISprite UpperIconLabel;

		public void Update()
		{
			UpperIconLabel.gameObject.transform.Rotate(0f, 0f, -10f * Time.deltaTime, Space.World);
		}

		public void UpdateUpperInfo()
		{
			UpdateMovingTankerNum();
			UpdateNonMoveTankerNum();
			UpdateDayLabel();
		}

		public void Enter()
		{
			UpdateUpperInfo();
			TweenPos.PlayForward();
		}

		public void Exit()
		{
			TweenPos.PlayReverse();
		}

		private void UpdateMovingTankerNum()
		{
			MovingTankerNum.textInt = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountMove();
		}

		private void UpdateNonMoveTankerNum()
		{
			TankerNum.textInt = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountNoMove();
		}

		private void UpdateDayLabel()
		{
			TurnString datetimeString = StrategyTopTaskManager.GetLogicManager().DatetimeString;
			Year.text = datetimeString.Year + "の年";
			Month.text = datetimeString.Month;
			Day.text = datetimeString.Day;
		}
	}
}
