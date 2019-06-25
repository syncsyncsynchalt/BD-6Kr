using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Animation))]
	public class BaseProdBuffer : MonoBehaviour
	{
		protected enum AnimationList
		{
			ProdBufferAntiSubmarine,
			ProdBufferAssault,
			ProdBufferAviation,
			ProdBufferAvoidance,
			ProdBufferClose,
			ProdBufferShelling,
			ProdBufferTorpedoSalvo,
			ProdBufferUnifiedFire,
			ProdBufferWithdrawal
		}

		protected Vector3 _vCameraPos = Vector3.zero;

		protected Vector3 _vFleetPos = Vector3.zero;

		protected Bezier _clsCameraBezier;

		protected Bezier _clsFleetBezier;

		protected Transform _traFleetAnchorFriend;

		protected Transform _traFleetAnchorEnemy;

		protected Action _actOnPlayBufferEffect;

		protected Action _actOnCalcInitLineRotation;

		protected Action _actOnPlayLineAnimation;

		protected Action _actOnNextFocusShipAnimation;

		[SerializeField]
		protected Animation _anim;

		[SerializeField]
		[Range(0f, 4f)]
		protected int _nBufferCnt;

		[Range(0f, 1f)]
		[SerializeField]
		protected float _fProgress;

		[Header("[Main Camera Properties]")]
		[SerializeField]
		protected Vector3 _vStartCameraPivot = Vector3.zero;

		[SerializeField]
		protected Vector3 _vMidCameraPivot = Vector3.zero;

		[SerializeField]
		protected List<Vector3> _listEndCameraPivot;

		[SerializeField]
		protected Vector3 _vCameraPog = Vector3.zero;

		[SerializeField]
		protected float _fFov = 30f;

		[SerializeField]
		[Header("[Friend Fleet Properties]")]
		protected Vector3 _vStartFleetPivot = Vector3.zero;

		[SerializeField]
		protected Vector3 _vMidFleetPivot = Vector3.zero;

		[SerializeField]
		protected Vector3 _vEndFleetPivot = Vector3.zero;

		[SerializeField]
		protected float _fFleetRotation;

		[SerializeField]
		[Range(0f, 1f)]
		protected float _fFleetMoveProgress;

		[SerializeField]
		[Header("[Enemy Fleet Properties]")]
		protected List<Vector3> _listEnemyFleetPivot;

		protected new Animation animation => this.GetComponentThis(ref _anim);

		protected virtual void OnDestroy()
		{
			Mem.Del(ref _vCameraPos);
			Mem.Del(ref _vFleetPos);
			Mem.Del(ref _clsCameraBezier);
			Mem.Del(ref _clsFleetBezier);
			Mem.Del(ref _traFleetAnchorFriend);
			Mem.Del(ref _traFleetAnchorEnemy);
			Mem.Del(ref _actOnPlayBufferEffect);
			Mem.Del(ref _actOnCalcInitLineRotation);
			Mem.Del(ref _actOnPlayLineAnimation);
			Mem.Del(ref _actOnNextFocusShipAnimation);
			Mem.Del(ref _nBufferCnt);
			Mem.Del(ref _anim);
			Mem.Del(ref _fProgress);
			Mem.Del(ref _vStartCameraPivot);
			Mem.Del(ref _vMidCameraPivot);
			Mem.DelListSafe(ref _listEndCameraPivot);
			Mem.Del(ref _vCameraPog);
			Mem.Del(ref _fFov);
			Mem.Del(ref _vStartFleetPivot);
			Mem.Del(ref _vMidFleetPivot);
			Mem.Del(ref _vEndFleetPivot);
			Mem.Del(ref _fFleetRotation);
			Mem.Del(ref _fFleetMoveProgress);
			Mem.DelListSafe(ref _listEnemyFleetPivot);
		}

		protected virtual void LateUpdate()
		{
			if (animation.isPlaying)
			{
				_vCameraPos = _clsCameraBezier.Interpolate(_fProgress);
				_vFleetPos = _clsFleetBezier.Interpolate(_fProgress);
				if (Application.isPlaying)
				{
					BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[1];
					battleFieldCamera.transform.position = _vCameraPos;
					battleFieldCamera.fieldOfView = _fFov;
					_vCameraPog = Vector3.Lerp(_vFleetPos, _listEnemyFleetPivot[_nBufferCnt], 0.5f);
					battleFieldCamera.pointOfGaze = _vCameraPog;
					_traFleetAnchorFriend.position = _vFleetPos;
					_traFleetAnchorEnemy.position = _listEnemyFleetPivot[_nBufferCnt];
					_traFleetAnchorFriend.rotation = Quaternion.Euler(new Vector3(0f, _fFleetRotation, 0f));
				}
			}
		}

		public virtual UniRx.IObservable<bool> Play(Action onPlayBufferEffect, Action onCalcInitLineRotation, Action onPlayLineAnimation, Action onNextFocusShipAnimation, int nBufferCnt)
		{
			_nBufferCnt = Mathe.MinMax2(nBufferCnt, 0, 4);
			_clsCameraBezier = new Bezier(Bezier.BezierType.Quadratic, _vStartCameraPivot, _listEndCameraPivot[_nBufferCnt], _vMidCameraPivot, Vector3.zero);
			_clsFleetBezier = new Bezier(Bezier.BezierType.Quadratic, _vStartFleetPivot, _vEndFleetPivot, _vMidFleetPivot, Vector3.zero);
			BattleField battleField = BattleTaskManager.GetBattleField();
			_traFleetAnchorFriend = battleField.dicFleetAnchor[FleetType.Friend];
			_traFleetAnchorEnemy = battleField.dicFleetAnchor[FleetType.Enemy];
			_traFleetAnchorEnemy.transform.localScale = Vector3.one * 0.8f;
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SetVerticalSplitCameras(isSplit: false);
			battleCameras.fieldDimCamera.maskAlpha = 0f;
			battleCameras.SwitchMainCamera(FleetType.Enemy);
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[1];
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.FixChasing);
			battleFieldCamera.eyePosition = _clsCameraBezier.Interpolate(0f);
			battleFieldCamera.pointOfGaze = Vector3.Lerp(_clsFleetBezier.Interpolate(0f), _listEnemyFleetPivot[_nBufferCnt], 0.5f);
			battleFieldCamera.transform.LookAt(battleFieldCamera.pointOfGaze);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(battleFieldCamera.transform);
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = ((Component)cutInEffectCamera.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 0f;
			}
			_actOnPlayBufferEffect = onPlayBufferEffect;
			_actOnNextFocusShipAnimation = onNextFocusShipAnimation;
			Observable.NextFrame().Subscribe(delegate
			{
				Dlg.Call(ref onCalcInitLineRotation);
				Dlg.Call(ref onPlayLineAnimation);
			});
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => AnimationObserver(observer));
		}

		protected virtual IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		protected virtual void PlayBufferEffect()
		{
			Dlg.Call(ref _actOnPlayBufferEffect);
		}

		protected virtual void PlayNextFocusShipAnimation()
		{
			Dlg.Call(ref _actOnNextFocusShipAnimation);
		}
	}
}
