using Common.Enum;
using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTileDockIcons : MonoBehaviour
	{
		[SerializeField]
		private StrategyDockIcon[] DockIcon;

		[Button("Debug", "Debug", new object[]
		{

		})]
		public int button;

		public void SetDockIcon(MapAreaModel areaModel)
		{
			List<NdockStates> nDockStateList = areaModel.GetNDockStateList();
			for (int i = 0; i < 4; i++)
			{
				if (i < nDockStateList.Count)
				{
					DockIcon[i].SetDockState(nDockStateList[i]);
				}
				else
				{
					DockIcon[i].SetActive(isActive: false);
				}
			}
		}

		public void Debug()
		{
			SetDockIcon(StrategyTopTaskManager.GetLogicManager().Area[1]);
		}
	}
}
