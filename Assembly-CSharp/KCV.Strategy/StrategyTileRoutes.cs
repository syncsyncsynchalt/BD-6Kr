using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTileRoutes : MonoBehaviour
	{
		private GameObject[] TileRoutes;

		private bool[] isEnable;

		private void Awake()
		{
			TileRoutes = new GameObject[4];
			for (int i = 0; i < TileRoutes.Length; i++)
			{
				TileRoutes[i] = base.transform.FindChild("TileRoute" + (i + 1)).gameObject;
			}
			isEnable = new bool[4];
		}

		public void Init(List<int> OpenTileIDs)
		{
			RouteUpdate(OpenTileIDs, 1, 13, 15, 0f);
			RouteUpdate(OpenTileIDs, 2, 12, 17, 0f);
			RouteUpdate(OpenTileIDs, 3, 6, 17, 0f);
			RouteUpdate(OpenTileIDs, 4, 14, 16, 0f);
		}

		public void UpdateTileRouteState(List<int> OpenTileIDs)
		{
			RouteUpdate(OpenTileIDs, 1, 13, 15, 0.4f);
			RouteUpdate(OpenTileIDs, 2, 12, 17, 0.4f);
			RouteUpdate(OpenTileIDs, 3, 6, 17, 0.4f);
			RouteUpdate(OpenTileIDs, 4, 14, 16, 0.4f);
		}

		public void HideRoute()
		{
			for (int i = 0; i < TileRoutes.Length; i++)
			{
				TweenAlpha.Begin(TileRoutes[i], 0.2f, 0f);
			}
		}

		public void ShowRoute()
		{
			for (int i = 0; i < TileRoutes.Length; i++)
			{
				if (isEnable[i])
				{
					TweenAlpha.Begin(TileRoutes[i], 0.2f, 1f);
				}
			}
		}

		public List<int> CreateOpenTileIDs()
		{
			List<int> list = new List<int>();
			for (int i = 1; i <= 17; i++)
			{
				if (StrategyTopTaskManager.GetLogicManager().Area[i].IsOpen())
				{
					list.Add(i);
				}
			}
			return list;
		}

		private void RouteUpdate(List<int> OpenTileIDs, int RouteNo, int checkID1, int checkID2, float duration)
		{
			if (OpenTileIDs.Exists((int x) => x == checkID1) && OpenTileIDs.Exists((int x) => x == checkID2))
			{
				TweenAlpha.Begin(TileRoutes[RouteNo - 1], duration, 1f);
				isEnable[RouteNo - 1] = true;
			}
			else
			{
				TweenAlpha.Begin(TileRoutes[RouteNo - 1], duration, 0f);
				isEnable[RouteNo - 1] = false;
			}
		}
	}
}
