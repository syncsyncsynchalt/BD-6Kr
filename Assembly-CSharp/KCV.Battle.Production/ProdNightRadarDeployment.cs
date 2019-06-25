using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation))]
	[RequireComponent(typeof(UIPanel))]
	public class ProdNightRadarDeployment : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			[Range(0f, 1f)]
			public float progress;

			[Header("[Camera Properties]")]
			public Vector3 cameraStartPivot;

			public Vector3 cameraMidPivot;

			public Vector3 cameraEndPivot;

			[Range(0f, 1f)]
			public float cameraMoveProgress;

			[Header("[Fleet Properties]")]
			public Vector3 friendFleetPivot;

			public Vector3 enemyFleetPivot;

			public void Dispose()
			{
				Mem.Del(ref progress);
				Mem.Del(ref cameraStartPivot);
				Mem.Del(ref cameraMidPivot);
				Mem.Del(ref cameraEndPivot);
				Mem.Del(ref cameraMoveProgress);
				Mem.Del(ref friendFleetPivot);
				Mem.Del(ref enemyFleetPivot);
			}
		}

		[SerializeField]
		private Transform _prefabProdNightMessage;

		[SerializeField]
		private UITexture _uiOverlay;

		[Header("[Animation Properties]")]
		[SerializeField]
		private Params _strParams = default(Params);

		private Bezier _clsCameraMoveBezier;

		private Vector3 _vCameraPos;

		private Animation _anim;

		private UIPanel _uiPanel;

		private Subject<bool> _subMessage = new Subject<bool>();

		private Transform _traFriendFleet;

		private Transform _traEnemyFleet;

		private ProdNightMessage _prodNightMessage;

		private bool _isRadarDeployment;

		private new Animation animation => this.GetComponentThis(ref _anim);

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdNightRadarDeployment Instantiate(ProdNightRadarDeployment prefab, Transform parent)
		{
			ProdNightRadarDeployment prodNightRadarDeployment = UnityEngine.Object.Instantiate(prefab);
			prodNightRadarDeployment.transform.parent = parent;
			prodNightRadarDeployment.transform.localScaleOne();
			prodNightRadarDeployment.transform.localPositionZero();
			prodNightRadarDeployment.Init();
			return prodNightRadarDeployment;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabProdNightMessage);
			Mem.Del(ref _uiOverlay);
			Mem.DelIDisposableSafe(ref _strParams);
			Mem.Del(ref _clsCameraMoveBezier);
			Mem.Del(ref _vCameraPos);
			Mem.Del(ref _anim);
			Mem.Del(ref _uiPanel);
			_subMessage.OnCompleted();
			Mem.Del(ref _subMessage);
			Mem.Del(ref _traFriendFleet);
			Mem.Del(ref _traEnemyFleet);
			Mem.DelComponentSafe(ref _prodNightMessage);
			Mem.Del(ref _isRadarDeployment);
		}

		private void LateUpdate()
		{
			if (animation.isPlaying)
			{
				_traEnemyFleet.position = _strParams.enemyFleetPivot;
				_traFriendFleet.position = _strParams.friendFleetPivot;
				_vCameraPos = Vector3.Lerp(_strParams.cameraStartPivot, _strParams.cameraEndPivot, Mathe.MinMax2F01(_strParams.cameraMoveProgress));
				BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras[0];
				battleFieldCamera.transform.position = _vCameraPos;
			}
		}

		private bool Init()
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			_prodNightMessage = ProdNightMessage.Instantiate(((Component)_prefabProdNightMessage).GetComponent<ProdNightMessage>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
			_prodNightMessage.panel.widgetsAreStatic = true;
			_subMessage.Take(1).Subscribe(delegate
			{
				_prodNightMessage.panel.widgetsAreStatic = false;
				_prodNightMessage.Play(null);
			}).AddTo(base.gameObject);
			panel.depth = _prodNightMessage.panel.depth - 1;
			_traFriendFleet = battleField.dicFleetAnchor[FleetType.Friend];
			_traEnemyFleet = battleField.dicFleetAnchor[FleetType.Enemy];
			_clsCameraMoveBezier = new Bezier(Bezier.BezierType.Quadratic, _strParams.cameraStartPivot, _strParams.cameraEndPivot, _strParams.cameraMidPivot, Vector3.zero);
			RadarDeployment();
			return true;
		}

		private void RadarDeployment()
		{
			if (!_isRadarDeployment)
			{
				_isRadarDeployment = true;
				BattleShips battleShips = BattleTaskManager.GetBattleShips();
				battleShips.RadarDeployment(isDeploy: true);
			}
		}

		public void RadarObjectConvergence()
		{
			BattleTaskManager.GetBattleCameras().fieldCameras[0].glowEffect.enabled = false;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(isDeploy: false);
		}

		public UniRx.IObservable<bool> Play()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetStandingPosition(StandingPositionType.CommandBuffer);
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SetVerticalSplitCameras(isSplit: false);
			battleCameras.fieldDimCamera.maskAlpha = 0f;
			battleCameras.SwitchMainCamera(FleetType.Friend);
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras[0];
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.FixChasing);
			battleFieldCamera.pointOfGaze = Vector3.Lerp(BattleTaskManager.GetBattleField().dicFleetAnchor[FleetType.Friend].position, BattleTaskManager.GetBattleField().dicFleetAnchor[FleetType.Enemy].position, 0.8f);
			battleFieldCamera.vignetting.enabled = true;
			battleFieldCamera.glowEffect.enabled = true;
			battleShips.SetBollboardTarget(null);
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = ((Component)cutInEffectCamera.transform.FindChild("TorpedoLine/OverlayLine")).GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 0f;
			}
			Observable.NextFrame().Subscribe(delegate
			{
				CalcInitLineRotation();
				PlayLineAnimation();
			});
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => AnimationObserver(observer));
		}

		private void CalcInitLineRotation()
		{
			BattleField field = BattleTaskManager.GetBattleField();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor[FleetType.Enemy]);
			});
			battleShips.bufferShipCirlce[1].ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor[FleetType.Friend]);
			});
		}

		private void PlayLineAnimation()
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
			battleShips.bufferShipCirlce[1].ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
		}

		private IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			if (animation.isPlaying)
			{
				observer.OnNext(value: true);
				observer.OnCompleted();
				yield break;
			}
			animation.Play();

            throw new NotImplementedException("‚È‚É‚±‚ê");
            //yield return Observable.Timer(TimeSpan.FromSeconds(animation.get_Item("ProdNightRadarDeployment").length)).StartAsCoroutine();
            yield return Observable.Timer(TimeSpan.FromSeconds(0)).StartAsCoroutine();


            observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private void PlayNightMessage()
		{
			_subMessage.OnNext(value: true);
		}
	}
}
