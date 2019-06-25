using Common.Enum;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyDockIcon : MonoBehaviour
	{
		private UISprite icon;

		private void Awake()
		{
			icon = GetComponent<UISprite>();
		}

		public void SetDockState(NdockStates state)
		{
			switch (state)
			{
			case NdockStates.EMPTY:
				icon.spriteName = "repair_blue";
				break;
			case NdockStates.RESTORE:
				icon.spriteName = "repair_green";
				break;
			case NdockStates.NOTUSE:
				icon.spriteName = "repair_none";
				break;
			}
		}
	}
}
