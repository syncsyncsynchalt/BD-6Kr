using UnityEngine;

namespace UniRx
{
	public class ObservableMonoBehaviour : TypedMonoBehaviour
	{
		private bool calledAwake;

		private Subject<Unit> awake;

		private Subject<Unit> fixedUpdate;

		private Subject<Unit> lateUpdate;

		private Subject<int> onAnimatorIK;

		private Subject<Unit> onAnimatorMove;

		private Subject<bool> onApplicationFocus;

		private Subject<bool> onApplicationPause;

		private Subject<Unit> onApplicationQuit;

		private Subject<Tuple<float[], int>> onAudioFilterRead;

		private Subject<Unit> onBecameInvisible;

		private Subject<Unit> onBecameVisible;

		private Subject<Collision> onCollisionEnter;

		private Subject<Collision2D> onCollisionEnter2D;

		private Subject<Collision> onCollisionExit;

		private Subject<Collision2D> onCollisionExit2D;

		private Subject<Collision> onCollisionStay;

		private Subject<Collision2D> onCollisionStay2D;

		private Subject<Unit> onConnectedToServer;

		private Subject<ControllerColliderHit> onControllerColliderHit;

		private bool calledDestroy;

		private Subject<Unit> onDestroy;

		private Subject<Unit> onDisable;

		private Subject<Unit> onDrawGizmos;

		private Subject<Unit> onDrawGizmosSelected;

		private Subject<Unit> onEnable;

		private Subject<float> onJointBreak;

		private Subject<int> onLevelWasLoaded;

		private Subject<Unit> onMouseDown;

		private Subject<Unit> onMouseDrag;

		private Subject<Unit> onMouseEnter;

		private Subject<Unit> onMouseExit;

		private Subject<Unit> onMouseOver;

		private Subject<Unit> onMouseUp;

		private Subject<Unit> onMouseUpAsButton;

		private Subject<GameObject> onParticleCollision;

		private Subject<Unit> onPostRender;

		private Subject<Unit> onPreCull;

		private Subject<Unit> onPreRender;

		private Subject<Tuple<RenderTexture, RenderTexture>> onRenderImage;

		private Subject<Unit> onRenderObject;

		private Subject<Unit> onServerInitialized;

		private Subject<Collider> onTriggerEnter;

		private Subject<Collider2D> onTriggerEnter2D;

		private Subject<Collider> onTriggerExit;

		private Subject<Collider2D> onTriggerExit2D;

		private Subject<Collider> onTriggerStay;

		private Subject<Collider2D> onTriggerStay2D;

		private Subject<Unit> onValidate;

		private Subject<Unit> onWillRenderObject;

		private Subject<Unit> reset;

		private bool calledStart;

		private Subject<Unit> start;

		private Subject<Unit> update;

		private Subject<NetworkDisconnection> onDisconnectedFromServer;

		private Subject<NetworkConnectionError> onFailedToConnect;

		private Subject<NetworkConnectionError> onFailedToConnectToMasterServer;

		private Subject<MasterServerEvent> onMasterServerEvent;

		private Subject<NetworkMessageInfo> onNetworkInstantiate;

		private Subject<NetworkPlayer> onPlayerConnected;

		private Subject<NetworkPlayer> onPlayerDisconnected;

		private Subject<Tuple<BitStream, NetworkMessageInfo>> onSerializeNetworkView;

		public override void Awake()
		{
			calledAwake = true;
			if (awake != null)
			{
				awake.OnNext(Unit.Default);
				awake.OnCompleted();
			}
		}

		public IObservable<Unit> AwakeAsObservable()
		{
			if (calledAwake)
			{
				return Observable.Return(Unit.Default);
			}
			return awake ?? (awake = new Subject<Unit>());
		}

		public override void FixedUpdate()
		{
			if (fixedUpdate != null)
			{
				fixedUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> FixedUpdateAsObservable()
		{
			return fixedUpdate ?? (fixedUpdate = new Subject<Unit>());
		}

		public override void LateUpdate()
		{
			if (lateUpdate != null)
			{
				lateUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> LateUpdateAsObservable()
		{
			return lateUpdate ?? (lateUpdate = new Subject<Unit>());
		}

		public override void OnAnimatorIK(int layerIndex)
		{
			if (onAnimatorIK != null)
			{
				onAnimatorIK.OnNext(layerIndex);
			}
		}

		public IObservable<int> OnAnimatorIKAsObservable()
		{
			return onAnimatorIK ?? (onAnimatorIK = new Subject<int>());
		}

		public override void OnAnimatorMove()
		{
			if (onAnimatorMove != null)
			{
				onAnimatorMove.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnAnimatorMoveAsObservable()
		{
			return onAnimatorMove ?? (onAnimatorMove = new Subject<Unit>());
		}

		public override void OnApplicationFocus(bool focus)
		{
			if (onApplicationFocus != null)
			{
				onApplicationFocus.OnNext(focus);
			}
		}

		public IObservable<bool> OnApplicationFocusAsObservable()
		{
			return onApplicationFocus ?? (onApplicationFocus = new Subject<bool>());
		}

		public override void OnApplicationPause(bool pause)
		{
			if (onApplicationPause != null)
			{
				onApplicationPause.OnNext(pause);
			}
		}

		public IObservable<bool> OnApplicationPauseAsObservable()
		{
			return onApplicationPause ?? (onApplicationPause = new Subject<bool>());
		}

		public override void OnApplicationQuit()
		{
			if (onApplicationQuit != null)
			{
				onApplicationQuit.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnApplicationQuitAsObservable()
		{
			return onApplicationQuit ?? (onApplicationQuit = new Subject<Unit>());
		}

		public override void OnAudioFilterRead(float[] data, int channels)
		{
			if (onAudioFilterRead != null)
			{
				onAudioFilterRead.OnNext(Tuple.Create(data, channels));
			}
		}

		public IObservable<Tuple<float[], int>> OnAudioFilterReadAsObservable()
		{
			return onAudioFilterRead ?? (onAudioFilterRead = new Subject<Tuple<float[], int>>());
		}

		public override void OnBecameInvisible()
		{
			if (onBecameInvisible != null)
			{
				onBecameInvisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameInvisibleAsObservable()
		{
			return onBecameInvisible ?? (onBecameInvisible = new Subject<Unit>());
		}

		public override void OnBecameVisible()
		{
			if (onBecameVisible != null)
			{
				onBecameVisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameVisibleAsObservable()
		{
			return onBecameVisible ?? (onBecameVisible = new Subject<Unit>());
		}

		public override void OnCollisionEnter(Collision collision)
		{
			if (onCollisionEnter != null)
			{
				onCollisionEnter.OnNext(collision);
			}
		}

		public IObservable<Collision> OnCollisionEnterAsObservable()
		{
			return onCollisionEnter ?? (onCollisionEnter = new Subject<Collision>());
		}

		public override void OnCollisionEnter2D(Collision2D coll)
		{
			if (onCollisionEnter2D != null)
			{
				onCollisionEnter2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionEnter2DAsObservable()
		{
			return onCollisionEnter2D ?? (onCollisionEnter2D = new Subject<Collision2D>());
		}

		public override void OnCollisionExit(Collision collisionInfo)
		{
			if (onCollisionExit != null)
			{
				onCollisionExit.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionExitAsObservable()
		{
			return onCollisionExit ?? (onCollisionExit = new Subject<Collision>());
		}

		public override void OnCollisionExit2D(Collision2D coll)
		{
			if (onCollisionExit2D != null)
			{
				onCollisionExit2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionExit2DAsObservable()
		{
			return onCollisionExit2D ?? (onCollisionExit2D = new Subject<Collision2D>());
		}

		public override void OnCollisionStay(Collision collisionInfo)
		{
			if (onCollisionStay != null)
			{
				onCollisionStay.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionStayAsObservable()
		{
			return onCollisionStay ?? (onCollisionStay = new Subject<Collision>());
		}

		public override void OnCollisionStay2D(Collision2D coll)
		{
			if (onCollisionStay2D != null)
			{
				onCollisionStay2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionStay2DAsObservable()
		{
			return onCollisionStay2D ?? (onCollisionStay2D = new Subject<Collision2D>());
		}

		public override void OnConnectedToServer()
		{
			if (onConnectedToServer != null)
			{
				onConnectedToServer.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnConnectedToServerAsObservable()
		{
			return onConnectedToServer ?? (onConnectedToServer = new Subject<Unit>());
		}

		public override void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (onControllerColliderHit != null)
			{
				onControllerColliderHit.OnNext(hit);
			}
		}

		public IObservable<ControllerColliderHit> OnControllerColliderHitAsObservable()
		{
			return onControllerColliderHit ?? (onControllerColliderHit = new Subject<ControllerColliderHit>());
		}

		public override void OnDestroy()
		{
			calledDestroy = true;
			if (onDestroy != null)
			{
				onDestroy.OnNext(Unit.Default);
				onDestroy.OnCompleted();
			}
		}

		public IObservable<Unit> OnDestroyAsObservable()
		{
			if (this == null)
			{
				return Observable.Return(Unit.Default);
			}
			if (calledDestroy)
			{
				return Observable.Return(Unit.Default);
			}
			return onDestroy ?? (onDestroy = new Subject<Unit>());
		}

		public override void OnDisable()
		{
			if (onDisable != null)
			{
				onDisable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDisableAsObservable()
		{
			return onDisable ?? (onDisable = new Subject<Unit>());
		}

		public override void OnDrawGizmos()
		{
			if (onDrawGizmos != null)
			{
				onDrawGizmos.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDrawGizmosAsObservable()
		{
			return onDrawGizmos ?? (onDrawGizmos = new Subject<Unit>());
		}

		public override void OnDrawGizmosSelected()
		{
			if (onDrawGizmosSelected != null)
			{
				onDrawGizmosSelected.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDrawGizmosSelectedAsObservable()
		{
			return onDrawGizmosSelected ?? (onDrawGizmosSelected = new Subject<Unit>());
		}

		public override void OnEnable()
		{
			if (onEnable != null)
			{
				onEnable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnEnableAsObservable()
		{
			return onEnable ?? (onEnable = new Subject<Unit>());
		}

		public override void OnJointBreak(float breakForce)
		{
			if (onJointBreak != null)
			{
				onJointBreak.OnNext(breakForce);
			}
		}

		public IObservable<float> OnJointBreakAsObservable()
		{
			return onJointBreak ?? (onJointBreak = new Subject<float>());
		}

		public override void OnLevelWasLoaded(int level)
		{
			if (onLevelWasLoaded != null)
			{
				onLevelWasLoaded.OnNext(level);
			}
		}

		public IObservable<int> OnLevelWasLoadedAsObservable()
		{
			return onLevelWasLoaded ?? (onLevelWasLoaded = new Subject<int>());
		}

		public override void OnMouseDown()
		{
			if (onMouseDown != null)
			{
				onMouseDown.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDownAsObservable()
		{
			return onMouseDown ?? (onMouseDown = new Subject<Unit>());
		}

		public override void OnMouseDrag()
		{
			if (onMouseDrag != null)
			{
				onMouseDrag.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDragAsObservable()
		{
			return onMouseDrag ?? (onMouseDrag = new Subject<Unit>());
		}

		public override void OnMouseEnter()
		{
			if (onMouseEnter != null)
			{
				onMouseEnter.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseEnterAsObservable()
		{
			return onMouseEnter ?? (onMouseEnter = new Subject<Unit>());
		}

		public override void OnMouseExit()
		{
			if (onMouseExit != null)
			{
				onMouseExit.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseExitAsObservable()
		{
			return onMouseExit ?? (onMouseExit = new Subject<Unit>());
		}

		public override void OnMouseOver()
		{
			if (onMouseOver != null)
			{
				onMouseOver.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseOverAsObservable()
		{
			return onMouseOver ?? (onMouseOver = new Subject<Unit>());
		}

		public override void OnMouseUp()
		{
			if (onMouseUp != null)
			{
				onMouseUp.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsObservable()
		{
			return onMouseUp ?? (onMouseUp = new Subject<Unit>());
		}

		public override void OnMouseUpAsButton()
		{
			if (onMouseUpAsButton != null)
			{
				onMouseUpAsButton.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsButtonAsObservable()
		{
			return onMouseUpAsButton ?? (onMouseUpAsButton = new Subject<Unit>());
		}

		public override void OnParticleCollision(GameObject other)
		{
			if (onParticleCollision != null)
			{
				onParticleCollision.OnNext(other);
			}
		}

		public IObservable<GameObject> OnParticleCollisionAsObservable()
		{
			return onParticleCollision ?? (onParticleCollision = new Subject<GameObject>());
		}

		public override void OnPostRender()
		{
			if (onPostRender != null)
			{
				onPostRender.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnPostRenderAsObservable()
		{
			return onPostRender ?? (onPostRender = new Subject<Unit>());
		}

		public override void OnPreCull()
		{
			if (onPreCull != null)
			{
				onPreCull.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnPreCullAsObservable()
		{
			return onPreCull ?? (onPreCull = new Subject<Unit>());
		}

		public override void OnPreRender()
		{
			if (onPreRender != null)
			{
				onPreRender.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnPreRenderAsObservable()
		{
			return onPreRender ?? (onPreRender = new Subject<Unit>());
		}

		public override void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (onRenderImage != null)
			{
				onRenderImage.OnNext(Tuple.Create(src, dest));
			}
		}

		public IObservable<Tuple<RenderTexture, RenderTexture>> OnRenderImageAsObservable()
		{
			return onRenderImage ?? (onRenderImage = new Subject<Tuple<RenderTexture, RenderTexture>>());
		}

		public override void OnRenderObject()
		{
			if (onRenderObject != null)
			{
				onRenderObject.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnRenderObjectAsObservable()
		{
			return onRenderObject ?? (onRenderObject = new Subject<Unit>());
		}

		public override void OnServerInitialized()
		{
			if (onServerInitialized != null)
			{
				onServerInitialized.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnServerInitializedAsObservable()
		{
			return onServerInitialized ?? (onServerInitialized = new Subject<Unit>());
		}

		public override void OnTriggerEnter(Collider other)
		{
			if (onTriggerEnter != null)
			{
				onTriggerEnter.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerEnterAsObservable()
		{
			return onTriggerEnter ?? (onTriggerEnter = new Subject<Collider>());
		}

		public override void OnTriggerEnter2D(Collider2D other)
		{
			if (onTriggerEnter2D != null)
			{
				onTriggerEnter2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerEnter2DAsObservable()
		{
			return onTriggerEnter2D ?? (onTriggerEnter2D = new Subject<Collider2D>());
		}

		public override void OnTriggerExit(Collider other)
		{
			if (onTriggerExit != null)
			{
				onTriggerExit.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerExitAsObservable()
		{
			return onTriggerExit ?? (onTriggerExit = new Subject<Collider>());
		}

		public override void OnTriggerExit2D(Collider2D other)
		{
			if (onTriggerExit2D != null)
			{
				onTriggerExit2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerExit2DAsObservable()
		{
			return onTriggerExit2D ?? (onTriggerExit2D = new Subject<Collider2D>());
		}

		public override void OnTriggerStay(Collider other)
		{
			if (onTriggerStay != null)
			{
				onTriggerStay.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerStayAsObservable()
		{
			return onTriggerStay ?? (onTriggerStay = new Subject<Collider>());
		}

		public override void OnTriggerStay2D(Collider2D other)
		{
			if (onTriggerStay2D != null)
			{
				onTriggerStay2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerStay2DAsObservable()
		{
			return onTriggerStay2D ?? (onTriggerStay2D = new Subject<Collider2D>());
		}

		public override void OnValidate()
		{
			if (onValidate != null)
			{
				onValidate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnValidateAsObservable()
		{
			return onValidate ?? (onValidate = new Subject<Unit>());
		}

		public override void OnWillRenderObject()
		{
			if (onWillRenderObject != null)
			{
				onWillRenderObject.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnWillRenderObjectAsObservable()
		{
			return onWillRenderObject ?? (onWillRenderObject = new Subject<Unit>());
		}

		public override void Reset()
		{
			if (reset != null)
			{
				reset.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> ResetAsObservable()
		{
			return reset ?? (reset = new Subject<Unit>());
		}

		public override void Start()
		{
			calledStart = true;
			if (start != null)
			{
				start.OnNext(Unit.Default);
				start.OnCompleted();
			}
		}

		public IObservable<Unit> StartAsObservable()
		{
			if (calledStart)
			{
				return Observable.Return(Unit.Default);
			}
			return start ?? (start = new Subject<Unit>());
		}

		public override void Update()
		{
			if (update != null)
			{
				update.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> UpdateAsObservable()
		{
			return update ?? (update = new Subject<Unit>());
		}

		public override void OnDisconnectedFromServer(NetworkDisconnection info)
		{
			if (onDisconnectedFromServer != null)
			{
				onDisconnectedFromServer.OnNext(info);
			}
		}

		public IObservable<NetworkDisconnection> OnDisconnectedFromServerAsObservable()
		{
			return onDisconnectedFromServer ?? (onDisconnectedFromServer = new Subject<NetworkDisconnection>());
		}

		public override void OnFailedToConnect(NetworkConnectionError error)
		{
			if (onFailedToConnect != null)
			{
				onFailedToConnect.OnNext(error);
			}
		}

		public IObservable<NetworkConnectionError> OnFailedToConnectAsObservable()
		{
			return onFailedToConnect ?? (onFailedToConnect = new Subject<NetworkConnectionError>());
		}

		public override void OnFailedToConnectToMasterServer(NetworkConnectionError info)
		{
			if (onFailedToConnectToMasterServer != null)
			{
				onFailedToConnectToMasterServer.OnNext(info);
			}
		}

		public IObservable<NetworkConnectionError> OnFailedToConnectToMasterServerAsObservable()
		{
			return onFailedToConnectToMasterServer ?? (onFailedToConnectToMasterServer = new Subject<NetworkConnectionError>());
		}

		public override void OnMasterServerEvent(MasterServerEvent msEvent)
		{
			if (onMasterServerEvent != null)
			{
				onMasterServerEvent.OnNext(msEvent);
			}
		}

		public IObservable<MasterServerEvent> OnMasterServerEventAsObservable()
		{
			return onMasterServerEvent ?? (onMasterServerEvent = new Subject<MasterServerEvent>());
		}

		public override void OnNetworkInstantiate(NetworkMessageInfo info)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (onNetworkInstantiate != null)
			{
				onNetworkInstantiate.OnNext(info);
			}
		}

		public IObservable<NetworkMessageInfo> OnNetworkInstantiateAsObservable()
		{
			return onNetworkInstantiate ?? (onNetworkInstantiate = new Subject<NetworkMessageInfo>());
		}

		public override void OnPlayerConnected(NetworkPlayer player)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (onPlayerConnected != null)
			{
				onPlayerConnected.OnNext(player);
			}
		}

		public IObservable<NetworkPlayer> OnPlayerConnectedAsObservable()
		{
			return onPlayerConnected ?? (onPlayerConnected = new Subject<NetworkPlayer>());
		}

		public override void OnPlayerDisconnected(NetworkPlayer player)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (onPlayerDisconnected != null)
			{
				onPlayerDisconnected.OnNext(player);
			}
		}

		public IObservable<NetworkPlayer> OnPlayerDisconnectedAsObservable()
		{
			return onPlayerDisconnected ?? (onPlayerDisconnected = new Subject<NetworkPlayer>());
		}

		public override void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (onSerializeNetworkView != null)
			{
				onSerializeNetworkView.OnNext(Tuple.Create<BitStream, NetworkMessageInfo>(stream, info));
			}
		}

		public IObservable<Tuple<BitStream, NetworkMessageInfo>> OnSerializeNetworkViewAsObservable()
		{
			return onSerializeNetworkView ?? (onSerializeNetworkView = new Subject<Tuple<BitStream, NetworkMessageInfo>>());
		}
	}
}
