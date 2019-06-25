using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(iTweenPath))]
	public class ProdAircraftMove : MonoBehaviour
	{
		[SerializeField]
		private float _fLength = 0.4f;

		[SerializeField]
		private UISprite _uiAircraft;

		private UISortieShip.Direction _iStartDirection;

		private bool _isTurn;

		private bool _isScale;

		private iTweenPath _tpTweenPath;

		private Action _actOnFinished;

		private LTSpline _clsSpline;

		private float _fPositionPercent;

		private UIPanel _uiPanel;

		private iTweenPath tweenPath => this.GetComponentThis(ref _tpTweenPath);

		private UISprite uiAircraft => _uiAircraft;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdAircraftMove Instantiate(ProdAircraftMove prefab, Transform parent, int depth)
		{
			ProdAircraftMove prodAircraftMove = UnityEngine.Object.Instantiate(prefab);
			prodAircraftMove.transform.parent = parent;
			prodAircraftMove.transform.localScaleOne();
			prodAircraftMove.transform.localPositionZero();
			prodAircraftMove.Init(depth);
			return prodAircraftMove;
		}

		private bool Init(int nDepth)
		{
			if (!iTweenPath.paths.ContainsKey("SeachBossCell"))
			{
				GetComponent<iTweenPath>().enabled = false;
			}
			panel.depth = nDepth;
			_uiAircraft.transform.localPositionZero();
			_uiAircraft.transform.localScaleZero();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _fPositionPercent);
			Mem.Del(ref _fLength);
			Mem.Del(ref _isTurn);
			Mem.Del(ref _isScale);
			Mem.Del(ref _tpTweenPath);
			Mem.Del(ref _actOnFinished);
		}

		public void Move(Vector3 fromCell, Vector3 targetCell, Action onFinished)
		{
			SetAircraft(MapAirReconnaissanceKind.WarterPlane);
			tweenPath.nodes[0] = fromCell;
			tweenPath.nodes[2] = targetCell;
			_uiAircraft.transform.position = fromCell;
			_actOnFinished = onFinished;
			CalcRoutePoint();
			CalcDirection(fromCell, targetCell);
			_isTurn = false;
			_isScale = false;
			_uiAircraft.transform.localScaleZero();
			_uiAircraft.transform.LTScale(Vector2.one, 0.5f);
			base.transform.LTValue(0f, 1f, 3f).setOnUpdate(UpdateHandler).setOnComplete(OnCompleteHandler);
		}

		public void Move(Vector3 from, Vector3 to, MapAirReconnaissanceKind iKind, Action onFinished)
		{
			SetAircraft(iKind);
			tweenPath.nodes[0] = from;
			tweenPath.nodes[2] = to;
			_uiAircraft.transform.position = from;
			_actOnFinished = onFinished;
			CalcRoutePoint();
			CalcDirection(from, to);
			_isTurn = false;
			_isScale = false;
			_uiAircraft.transform.localScaleZero();
			_uiAircraft.transform.LTScale(Vector2.one, 0.5f);
			base.transform.LTValue(0f, 1f, 3f).setOnUpdate(UpdateHandler).setOnComplete(OnCompleteHandler);
		}

		private void UpdateHandler(float value)
		{
			_fPositionPercent = value;
			_uiAircraft.transform.position = iTween.PointOnPath(tweenPath.nodes.ToArray(), _fPositionPercent);
			if (0.3f < _fPositionPercent && !_isTurn)
			{
				_isTurn = true;
				float to = (_iStartDirection != UISortieShip.Direction.Right) ? 0f : (-180f);
				_uiAircraft.transform.LTRotateY(to, 0.8f);
			}
			if (0.8f < _fPositionPercent && !_isScale)
			{
				_isScale = true;
				_uiAircraft.transform.LTScale(Vector3.zero, 0.5f);
			}
		}

		private void OnCompleteHandler()
		{
			Dlg.Call(ref _actOnFinished);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void CalcRoutePoint()
		{
			Vector3[] array = MiscCalculateUtils.CalculateBezierPoint(tweenPath.nodes[0], tweenPath.nodes[2], _fLength);
			tweenPath.nodes[1] = array[0];
			tweenPath.nodes[3] = array[1];
			tweenPath.nodes[4] = tweenPath.nodes[0];
		}

		private void CalcDirection(Vector3 from, Vector3 to)
		{
			Vector3 vector = Mathe.Direction(from, to);
			switch (Math.Sign(vector.x))
			{
			case -1:
				_iStartDirection = UISortieShip.Direction.Left;
				_uiAircraft.transform.localEulerAnglesY(180f);
				break;
			case 1:
				_iStartDirection = UISortieShip.Direction.Right;
				_uiAircraft.transform.localEulerAnglesY(0f);
				break;
			}
		}

		private void SetAircraft(MapAirReconnaissanceKind iKind)
		{
			switch (iKind)
			{
			case MapAirReconnaissanceKind.WarterPlane:
				uiAircraft.spriteName = "icon_WaterPlane";
				uiAircraft.MakePixelPerfect();
				break;
			case MapAirReconnaissanceKind.LargePlane:
				uiAircraft.spriteName = "icon_LargePlane";
				uiAircraft.MakePixelPerfect();
				break;
			default:
				uiAircraft.spriteName = string.Empty;
				break;
			}
		}
	}
}
