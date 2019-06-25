using local.managers;
using local.models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UIMapManager : MonoBehaviour
	{
		[SerializeField]
		private UISortieShip.Direction _iDirection = UISortieShip.Direction.Right;

		[SerializeField]
		private bool _isStartDirection;

		[SerializeField]
		private Transform _traRudder;

		[SerializeField]
		private Transform _traBackground;

		[SerializeField]
		[Button("SetBackgroundSize", "SetBackgroundSize", new object[]
		{

		})]
		private int _nSetBackgroundSize = 1;

		private UITexture _uiBackground;

		private UISortieShip _uiSortieShip;

		private UISortieMapCell _uiNowCell;

		private UISortieMapCell _uiNextCell;

		private List<UISortieMapCell> _listMapCell;

		private List<UISortieMapRoot> _listMapRoute;

		private Dictionary<int, Transform> _dicAirRecPoint;

		private WobblingIcons _clsWobblingIcons;

		public List<UISortieMapCell> cells
		{
			get
			{
				return _listMapCell;
			}
			private set
			{
				_listMapCell = value;
			}
		}

		public List<UISortieMapRoot> routes
		{
			get
			{
				return _listMapRoute;
			}
			private set
			{
				_listMapRoute = value;
			}
		}

		public Dictionary<int, Transform> airRecPoint
		{
			get
			{
				return _dicAirRecPoint;
			}
			private set
			{
				_dicAirRecPoint = value;
			}
		}

		public UISortieShip sortieShip
		{
			get
			{
				return _uiSortieShip;
			}
			private set
			{
				_uiSortieShip = value;
			}
		}

		public UISortieMapCell nowCell
		{
			get
			{
				return _uiNowCell;
			}
			private set
			{
				_uiNowCell = value;
			}
		}

		public UISortieMapCell nextCell
		{
			get
			{
				return _uiNextCell;
			}
			private set
			{
				_uiNextCell = value;
			}
		}

		public Transform rudder
		{
			get
			{
				return _traRudder;
			}
			set
			{
				if (value.name == "Rudder")
				{
					_traRudder = value;
				}
			}
		}

		public Transform background
		{
			get
			{
				return _traBackground;
			}
			set
			{
				if (value.name == "Background")
				{
					_traBackground = value;
				}
			}
		}

		public WobblingIcons wobblingIcons => _clsWobblingIcons;

		public static UIMapManager Instantiate(MapManager manager, UIMapManager prefab, Transform parent, UISortieShip sortieShip)
		{
			UIMapManager uIMapManager = (!(prefab != null)) ? Util.InstantiatePrefab($"SortieMap/AreaMap/Map{manager.Map.MstId}", parent.gameObject).GetComponent<UIMapManager>() : Object.Instantiate(prefab);
			uIMapManager.transform.parent = parent;
			uIMapManager.transform.localPositionZero();
			uIMapManager.transform.localScale = Vector3.one * 1.1f;
			uIMapManager.Init(manager, sortieShip);
			return uIMapManager;
		}

		private bool Init(MapManager manager, UISortieShip sortieShip)
		{
			if (_traBackground == null)
			{
				_traBackground = base.transform.FindChild("Background");
			}
			if (rudder == null)
			{
				rudder = base.transform.FindChild("Rudder");
			}
			MakeCellList(manager);
			MakeRouteList();
			MakeAirRecPoint(manager);
			MakeWobblingIconList(manager);
			this.sortieShip = UISortieShip.Instantiate(sortieShip, base.transform, _iDirection);
			UpdatePassedRoutesStates(manager);
			UpdateNowNNextCell(manager.NowCell, manager.NextCell);
			SetShipPosition();
			return true;
		}

		private void MakeCellList(MapManager manager)
		{
			cells = new List<UISortieMapCell>();
			cells.Add(null);
			for (int i = 1; i < base.transform.childCount; i++)
			{
				Transform transform = base.transform.FindChild("UISortieMapCell" + i);
				if (transform != null)
				{
					cells.Add(((Component)transform).GetComponent<UISortieMapCell>().Startup());
					cells[i].Init(manager.Cells[i]);
					continue;
				}
				break;
			}
		}

		private void MakeRouteList()
		{
			routes = new List<UISortieMapRoot>();
			routes.Add(null);
			for (int i = 1; i < base.transform.childCount; i++)
			{
				Transform transform = base.transform.FindChild($"Route{i}");
				if (transform != null)
				{
					routes.Add(((Component)transform).GetComponent<UISortieMapRoot>());
					routes[i].isPassed = false;
					continue;
				}
				break;
			}
		}

		private void MakeAirRecPoint(MapManager manager)
		{
			_dicAirRecPoint = new Dictionary<int, Transform>();
			manager.Cells.Skip(1).ForEach(delegate(CellModel x)
			{
				Transform transform = base.transform.FindChild($"AirRecPoint{x.CellNo}");
				if (transform != null)
				{
					_dicAirRecPoint.Add(x.CellNo, transform);
				}
			});
		}

		private void MakeWobblingIconList(MapManager manager)
		{
			_clsWobblingIcons = new WobblingIcons(manager, base.transform);
		}

		private void FixedUpdate()
		{
			if (_clsWobblingIcons.wobblingIcons != null && _clsWobblingIcons.wobblingIcons.Count != 0)
			{
				_clsWobblingIcons.FixedRun();
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _iDirection);
			Mem.Del(ref _isStartDirection);
			Mem.Del(ref _traRudder);
			Mem.Del(ref _traBackground);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiSortieShip);
			Mem.Del(ref _uiNowCell);
			Mem.Del(ref _uiNextCell);
			Mem.DelListSafe(ref _listMapCell);
			Mem.DelList(ref _listMapRoute);
			Mem.DelDictionarySafe(ref _dicAirRecPoint);
			Mem.DelIDisposableSafe(ref _clsWobblingIcons);
		}

		public void UpdatePassedRoutesStates(MapManager manager)
		{
			manager.Passed.ForEach(delegate(int x)
			{
				routes[x].isPassed = true;
			});
		}

		public void UpdateRouteState(int CellNo)
		{
			routes[CellNo].Passed(isPassed: true);
		}

		public void UpdateCellState(int CellNo, bool isPassed)
		{
			cells[CellNo].isPassedCell = isPassed;
		}

		public void SetShipPosition()
		{
			if (nowCell != null)
			{
				sortieShip.transform.localPosition = nowCell.transform.localPosition;
			}
			else
			{
				sortieShip.transform.position = _traRudder.position;
			}
		}

		public void InitAfterBattle()
		{
			if (nowCell != null)
			{
				nowCell.isPassedCell = true;
			}
			cells[nextCell.cellModel.CellNo].isPassedCell = true;
			routes[nextCell.cellModel.CellNo].isPassed = true;
			sortieShip.transform.localPosition = nextCell.transform.localPosition;
		}

		public void UpdateNowNNextCell(CellModel now, CellModel next)
		{
			SetNowCell(now.CellNo);
			SetNextCell(next.CellNo);
		}

		private void SetNowCell(int cellNo)
		{
			if (cells.Count <= cellNo)
			{
				cellNo = 1;
			}
			nowCell = cells[cellNo];
		}

		private void SetNextCell(int cellNo)
		{
			if (cells.Count <= cellNo)
			{
				cellNo = 1;
			}
			nextCell = cells[cellNo];
		}

		private void SetBackgroundSize()
		{
			((Component)_traBackground).GetComponent<UITexture>().localSize = Defines.SORTIEMAP_MAP_BG_SIZE;
		}
	}
}
