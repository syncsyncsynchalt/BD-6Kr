using Common.Enum;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UISortieShip : MonoBehaviour
	{
		public enum Direction
		{
			Left,
			Right,
			Same
		}

		private const int SPEED = 100;

		[SerializeField]
		private Transform _prefabProdExclamationPoint;

		[SerializeField]
		private Transform _prefabEventItem;

		[SerializeField]
		private Transform _prefabEventAircraftMove;

		[SerializeField]
		private Transform _prefabProdBalloon;

		[SerializeField]
		private Transform _prefabProdCommentBalloon;

		[SerializeField]
		private UISprite _uiShipSprite;

		[SerializeField]
		private UISprite _uiInputIcon;

		private Direction _iDirection = Direction.Right;

		private UIPanel _uiPanel;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static UISortieShip Instantiate(UISortieShip prefab, Transform parent, Direction iDirection)
		{
			UISortieShip uISortieShip = UnityEngine.Object.Instantiate(prefab);
			uISortieShip.transform.parent = parent;
			uISortieShip.transform.localPositionZero();
			uISortieShip.transform.localScaleOne();
			return uISortieShip.VirtualCtor(iDirection);
		}

		private UISortieShip VirtualCtor(Direction iDirection)
		{
			_uiInputIcon.alpha = 0f;
			_iDirection = iDirection;
			Vector3 euler = (_iDirection != 0) ? Vector3.zero : (Vector3.down * 180f);
			base.transform.localRotation = Quaternion.Euler(euler);
			return this;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabProdExclamationPoint);
			Mem.Del(ref _prefabEventItem);
			Mem.Del(ref _prefabEventAircraftMove);
			Mem.Del(ref _prefabProdBalloon);
			Mem.Del(ref _prefabProdCommentBalloon);
			Mem.Del(ref _uiShipSprite);
			Mem.Del(ref _uiInputIcon);
			Mem.Del(ref _uiPanel);
		}

		public void Move(UISortieMapCell NextCell, Action onFinishedAnimation)
		{
			Vector3 localPosition = NextCell.transform.localPosition;
			ChangeDirection(CalcDirection(base.transform.localPosition, localPosition));
			base.transform.LTMoveLocal(localPosition, Math.Abs(Vector3.Distance(base.transform.localPosition, localPosition)) / 100f).setEase(LeanTweenType.linear).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinishedAnimation);
			});
		}

		public void PlayGetMaterialOrItem(MapEventItemModel mapEventItemModel, Action onFinished)
		{
			ProdItem prodItem = ProdItem.Instantiate(((Component)_prefabEventItem).GetComponent<ProdItem>(), base.transform, mapEventItemModel);
			prodItem.PlayGetAnim(onFinished);
		}

		public void PlayLostMaterial(MapEventHappeningModel mapEventHappeningModel, Action onFinished)
		{
			ProdItem prodItem = ProdItem.Instantiate(((Component)_prefabEventItem).GetComponent<ProdItem>(), base.transform, mapEventHappeningModel);
			prodItem.PlayLostAnim(onFinished);
		}

		public UniRx.IObservable<bool> PlayExclamationPoint()
		{
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => PlayExclamationPointAnimationObserver(observer));
		}

		private IEnumerator PlayExclamationPointAnimationObserver(UniRx.IObserver<bool> observer)
		{
			GameObject anchor = new GameObject("ProdExclamationPointAnchor");
			anchor.transform.parent = base.transform;
			anchor.transform.localScaleOne();
			anchor.transform.localPosition = Vector3.up * 32f;
			ProdExclamationPoint pxp = ProdExclamationPoint.Instantiate(((Component)_prefabProdExclamationPoint).GetComponent<ProdExclamationPoint>(), anchor.transform);
			yield return pxp.Play().StartAsCoroutine();
			UnityEngine.Object.Destroy(anchor.gameObject);
			Mem.DelComponentSafe(ref pxp);
			observer.OnNext(value: true);
			observer.OnCompleted();
			yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
		}

		public void PlayDetectionAircraft(UISortieMapCell fromCell, UISortieMapCell toCell, Action onFinished)
		{
			ProdAircraftMove prodAircraftMove = ProdAircraftMove.Instantiate(((Component)_prefabEventAircraftMove).GetComponent<ProdAircraftMove>(), SortieMapTaskManager.GetUIMapManager().transform, panel.depth + 1);
			prodAircraftMove.Move(fromCell.transform.position, toCell.transform.position, onFinished);
		}

		public void PlayAirReconnaissance(MapAirReconnaissanceKind iKind, Transform from, Transform airRecPoint, Action onFinished)
		{
			switch (iKind)
			{
			case MapAirReconnaissanceKind.Impossible:
				Observable.Timer(TimeSpan.FromSeconds(1.2000000476837158)).Subscribe(delegate
				{
					Dlg.Call(ref onFinished);
				});
				break;
			case MapAirReconnaissanceKind.LargePlane:
			{
				ProdAircraftMove prodAircraftMove2 = ProdAircraftMove.Instantiate(((Component)_prefabEventAircraftMove).GetComponent<ProdAircraftMove>(), SortieMapTaskManager.GetUIMapManager().transform, panel.depth + 1);
				prodAircraftMove2.Move(from.position, airRecPoint.position, MapAirReconnaissanceKind.LargePlane, onFinished);
				break;
			}
			case MapAirReconnaissanceKind.WarterPlane:
			{
				ProdAircraftMove prodAircraftMove = ProdAircraftMove.Instantiate(((Component)_prefabEventAircraftMove).GetComponent<ProdAircraftMove>(), SortieMapTaskManager.GetUIMapManager().transform, panel.depth + 1);
				prodAircraftMove.Move(from.position, airRecPoint.position, MapAirReconnaissanceKind.WarterPlane, onFinished);
				break;
			}
			}
		}

		public void PlayBalloon(enumMapEventType iEventType, enumMapWarType iWarType, Action onFinished)
		{
			ProdBalloon balloon = ProdBalloon.Instantiate(((Component)_prefabProdBalloon).GetComponent<ProdBalloon>(), base.transform, _iDirection, iEventType, iWarType);
			balloon.depth = _uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				UnityEngine.Object.Destroy(balloon.gameObject);
				Mem.Del(ref balloon);
			});
		}

		public void PlayBalloon(MapEventItemModel model, Action onFinished)
		{
			ProdBalloon balloon = ProdBalloon.Instantiate(((Component)_prefabProdBalloon).GetComponent<ProdBalloon>(), base.transform, _iDirection, model);
			balloon.depth = _uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				UnityEngine.Object.Destroy(balloon.gameObject);
				Mem.Del(ref balloon);
			});
		}

		public void PlayBalloon(MapEventAirReconnaissanceModel eventAirRecModel, MapEventItemModel eventItemModel, Action onFinished)
		{
			if (eventAirRecModel.AircraftType == MapAirReconnaissanceKind.Impossible)
			{
				Dlg.Call(ref onFinished);
				return;
			}
			ProdBalloon balloon = ProdBalloon.Instantiate(((Component)_prefabProdBalloon).GetComponent<ProdBalloon>(), base.transform, _iDirection, eventAirRecModel, eventItemModel);
			balloon.depth = _uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				UnityEngine.Object.Destroy(balloon.gameObject);
				Mem.Del(ref balloon);
			});
		}

		public void PlayBalloon(MapCommentKind iKind, Action onFinished)
		{
			if (iKind == MapCommentKind.None)
			{
				Dlg.Call(ref onFinished);
				return;
			}
			ProdCommentBalloon balloon = ProdCommentBalloon.Instantiate(((Component)_prefabProdCommentBalloon).GetComponent<ProdCommentBalloon>(), base.transform, _iDirection, iKind);
			balloon.sprite.depth = _uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				UnityEngine.Object.Destroy(balloon.gameObject);
				Mem.Del(ref balloon);
			});
		}

		public void PlayBalloon(Action onFinished)
		{
			ProdCommentBalloon balloon = ProdCommentBalloon.Instantiate(((Component)_prefabProdCommentBalloon).GetComponent<ProdCommentBalloon>(), base.transform, _iDirection);
			balloon.sprite.depth = _uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				UnityEngine.Object.Destroy(balloon.gameObject);
				Mem.Del(ref balloon);
			});
		}

		public LTDescr ShowInputIcon()
		{
			return _uiInputIcon.transform.LTValue(_uiInputIcon.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiInputIcon.alpha = x;
			});
		}

		public LTDescr HideInputIcon()
		{
			return _uiInputIcon.transform.LTValue(_uiInputIcon.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiInputIcon.alpha = x;
			});
		}

		private Direction CalcDirection(Vector3 from, Vector3 to)
		{
			if (from.x < to.x)
			{
				return Direction.Right;
			}
			if (to.x < from.x)
			{
				return Direction.Left;
			}
			return Direction.Same;
		}

		private void ChangeDirection(Direction direction)
		{
			if (_iDirection != direction)
			{
				_iDirection = direction;
				float time = 0.5f;
				switch (_iDirection)
				{
				case Direction.Left:
					base.transform.LTRotateLocal(Vector3.down * 180f, time);
					break;
				case Direction.Right:
					base.transform.LTRotateLocal(Vector3.zero, time);
					break;
				}
			}
		}
	}
}
