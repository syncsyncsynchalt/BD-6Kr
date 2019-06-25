using local.managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexTileManager : MonoBehaviour
	{
		private UIWidget Widget;

		private TweenPosition TweenPos;

		[SerializeField]
		private UISprite FocusObject;

		public StrategyHexTile RebellionTile;

		public StrategyHexTile[] Tiles
		{
			get;
			private set;
		}

		public int OpenAreaNum
		{
			get;
			private set;
		}

		public StrategyHexTile FocusTile
		{
			get;
			private set;
		}

		private void Awake()
		{
			Tiles = new StrategyHexTile[18];
			for (int i = 1; i < Tiles.Length; i++)
			{
				Tiles[i] = ((Component)base.transform.FindChild("Tile" + i.ToString("D2"))).GetComponent<StrategyHexTile>();
			}
			Widget = GetComponent<UIWidget>();
			TweenPos = GetComponent<TweenPosition>();
		}

		public void Init()
		{
			int currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
		}

		public void setAreaModels(StrategyMapManager strategyManager)
		{
			for (int i = 1; i < Tiles.Length; i++)
			{
				Tiles[i].setAreaModel(strategyManager.Area[i]);
			}
			updateTilesColor();
			UpdateAllAreaDockIcons();
		}

		public IEnumerator StartTilesPopUp(int[] newOpenAreas, Action<bool> CallBack)
		{
			List<StrategyHexTile> PopUpTiles = new List<StrategyHexTile>();
			List<int> PopUpTilesID = new List<int>();
			int OpenTileNum = 0;
			List<int> newOpenAreaList = null;
			if (newOpenAreas != null)
			{
				newOpenAreaList = newOpenAreas.ToList();
				OpenTileNum = newOpenAreaList.Count;
			}
			int PopUpTileNum = 0;
			for (int j = 1; j < Tiles.Length; j++)
			{
				if (isPopUpTile(newOpenAreaList, j, ref PopUpTileNum))
				{
					PopUpTiles.Add(Tiles[j]);
					PopUpTilesID.Add(j);
				}
				else if (newOpenAreaList == null || newOpenAreaList.Exists((int x) => x != j))
				{
					Tiles[j].setActivePositionAnimation(isActive: false);
				}
			}
			OpenAreaNum = PopUpTileNum + OpenTileNum;
			for (int i = 0; i < PopUpTiles.Count; i++)
			{
				PopUpTiles[i].SetActive(isActive: true);
				Action<bool> callBack = (i != PopUpTiles.Count - 1) ? null : CallBack;
				float delay = 0.3f / (float)PopUpTiles.Count * (float)(i + 1);
				PopUpTiles[i].PlayPopUpAnimation(callBack, delay);
			}
			StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.UpdateTileRouteState(PopUpTilesID);
			yield return null;
		}

		private bool isPopUpTile(List<int> newOpenAreaList, int targetNo, ref int OpenCount)
		{
			if (Tiles[targetNo].isOpen)
			{
				OpenCount++;
			}
			if (Tiles[targetNo].isRebellionTile)
			{
				RebellionTile = Tiles[targetNo];
			}
			if (newOpenAreaList == null)
			{
				return Tiles[targetNo].isOpen || Tiles[targetNo].isRebellionTile;
			}
			return (Tiles[targetNo].isOpen || Tiles[targetNo].isRebellionTile) && !newOpenAreaList.Contains(targetNo);
		}

		public void setFocusObject(int areaID)
		{
			FocusObject.transform.position = FocusTile.transform.position;
			FocusObject.SetActive(isActive: true);
		}

		private void setFocusTile(int areaID)
		{
			FocusTile = Tiles[areaID];
		}

		public void changeFocus(int areaID)
		{
			setFocusTile(areaID);
			setFocusObject(areaID);
			updateTilesColor();
			for (int i = 1; i < Tiles.Length; i++)
			{
				Tiles[i].isFocus = false;
			}
			Tiles[areaID].isFocus = true;
		}

		public void setMovable(List<int> movableAreaID)
		{
			for (int i = 1; i < movableAreaID.Count; i++)
			{
				Tiles[i].isMovable = movableAreaID.Contains(i);
			}
		}

		public void clearMovable()
		{
			for (int i = 1; i < Tiles.Length; i++)
			{
				Tiles[i].isMovable = false;
			}
		}

		public void updateTilesColor()
		{
			for (int i = 1; i < Tiles.Length; i++)
			{
				if (Tiles[i].isOpen)
				{
					Tiles[i].setTileColor();
				}
			}
		}

		public void SetVisible(bool isVisible)
		{
			float alpha = isVisible ? 1 : 0;
			TweenAlpha.Begin(Widget.gameObject, 0.2f, alpha);
			FocusObject.SetActive(isVisible);
			FocusObject.GetComponent<TweenScale>().ResetToBeginning();
			FocusObject.GetComponent<TweenAlpha>().ResetToBeginning();
		}

		public void setVisibleFocusObject(bool isVisible)
		{
			FocusObject.enabled = isVisible;
		}

		public void setActivePositionAnimations(bool isActive)
		{
			TweenPos.enabled = isActive;
		}

		public void ChangeTileColorMove(List<int> areaIDs)
		{
			int i;
			for (i = 1; i < Tiles.Length; i++)
			{
				if (areaIDs != null && areaIDs.Exists((int x) => x == i))
				{
					Tiles[i].ChangeMoveTileColor();
				}
				else
				{
					Tiles[i].ClearTileColor();
				}
			}
		}

		public bool isExistRebellionTargetTile()
		{
			return Tiles.Any((StrategyHexTile x) => x != null && x.isColorChanged);
		}

		public int GetColorChangedTileID()
		{
			return Tiles.First((StrategyHexTile x) => x != null && x.isColorChanged).areaID;
		}

		public void UpdateAllAreaDockIcons()
		{
			StrategyHexTile[] tiles = Tiles;
			foreach (StrategyHexTile strategyHexTile in tiles)
			{
				if (strategyHexTile != null)
				{
					strategyHexTile.UpdateDockIcons();
				}
			}
		}

		public void SetVisibleAllAreaDockIcons(bool isVisible)
		{
			StrategyHexTile[] tiles = Tiles;
			foreach (StrategyHexTile strategyHexTile in tiles)
			{
				if (strategyHexTile != null)
				{
					strategyHexTile.SetVisibleDockIcons(isVisible);
				}
			}
		}
	}
}
