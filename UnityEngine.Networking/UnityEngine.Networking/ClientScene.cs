using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	public class ClientScene
	{
		private struct PendingOwner
		{
			public NetworkInstanceId netId;

			public short playerControllerId;
		}

		private static List<PlayerController> s_LocalPlayers = new List<PlayerController>();

		private static NetworkConnection s_ReadyConnection;

		private static Dictionary<NetworkSceneId, NetworkIdentity> s_SpawnableObjects;

		private static bool s_IsReady;

		private static bool s_IsSpawnFinished;

		private static NetworkScene s_NetworkScene = new NetworkScene();

		private static ObjectSpawnSceneMessage s_ObjectSpawnSceneMessage = new ObjectSpawnSceneMessage();

		private static ObjectSpawnFinishedMessage s_ObjectSpawnFinishedMessage = new ObjectSpawnFinishedMessage();

		private static ObjectDestroyMessage s_ObjectDestroyMessage = new ObjectDestroyMessage();

		private static ObjectSpawnMessage s_ObjectSpawnMessage = new ObjectSpawnMessage();

		private static OwnerMessage s_OwnerMessage = new OwnerMessage();

		private static ClientAuthorityMessage s_ClientAuthorityMessage = new ClientAuthorityMessage();

		private static List<PendingOwner> s_PendingOwnerIds = new List<PendingOwner>();

		public static List<PlayerController> localPlayers => s_LocalPlayers;

		public static bool ready => s_IsReady;

		public static NetworkConnection readyConnection => s_ReadyConnection;

		public static Dictionary<NetworkInstanceId, NetworkIdentity> objects => s_NetworkScene.localObjects;

		public static Dictionary<NetworkHash128, GameObject> prefabs => NetworkScene.guidToPrefab;

		public static Dictionary<NetworkSceneId, NetworkIdentity> spawnableObjects => s_SpawnableObjects;

		internal static void SetNotReady()
		{
			s_IsReady = false;
		}

		internal static void Shutdown()
		{
			s_NetworkScene.Shutdown();
			s_LocalPlayers = new List<PlayerController>();
			s_PendingOwnerIds = new List<PendingOwner>();
			s_SpawnableObjects = null;
			s_ReadyConnection = null;
			s_IsReady = false;
			s_IsSpawnFinished = false;
			NetworkTransport.Shutdown();
			NetworkTransport.Init();
		}

		internal static bool GetPlayerController(short playerControllerId, out PlayerController player)
		{
			player = null;
			if (playerControllerId >= localPlayers.Count)
			{
				if (LogFilter.logWarn)
				{
					Debug.Log("ClientScene::GetPlayer: no local player found for: " + playerControllerId);
				}
				return false;
			}
			if (localPlayers[playerControllerId] == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ClientScene::GetPlayer: local player is null for: " + playerControllerId);
				}
				return false;
			}
			player = localPlayers[playerControllerId];
			return player.gameObject != null;
		}

		internal static void InternalAddPlayer(NetworkIdentity view, short playerControllerId)
		{
			if (LogFilter.logDebug)
			{
				Debug.LogWarning("ClientScene::InternalAddPlayer: playerControllerId : " + playerControllerId);
			}
			if (playerControllerId >= s_LocalPlayers.Count)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ClientScene::InternalAddPlayer: playerControllerId higher than expected: " + playerControllerId);
				}
				while (playerControllerId >= s_LocalPlayers.Count)
				{
					s_LocalPlayers.Add(new PlayerController());
				}
			}
			PlayerController playerController = new PlayerController();
			playerController.gameObject = view.gameObject;
			playerController.playerControllerId = playerControllerId;
			playerController.unetView = view;
			PlayerController playerController2 = playerController;
			s_LocalPlayers[playerControllerId] = playerController2;
			s_ReadyConnection.SetPlayerController(playerController2);
		}

		public static bool AddPlayer(short playerControllerId)
		{
			return AddPlayer(null, playerControllerId);
		}

		public static bool AddPlayer(NetworkConnection readyConn, short playerControllerId)
		{
			return AddPlayer(readyConn, playerControllerId, null);
		}

		public static bool AddPlayer(NetworkConnection readyConn, short playerControllerId, MessageBase extraMessage)
		{
			if (playerControllerId < 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " is negative");
				}
				return false;
			}
			if (playerControllerId > 32)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " is too high, max is " + 32);
				}
				return false;
			}
			if (playerControllerId > 16 && LogFilter.logWarn)
			{
				Debug.LogWarning("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " is unusually high");
			}
			while (playerControllerId >= s_LocalPlayers.Count)
			{
				s_LocalPlayers.Add(new PlayerController());
			}
			if (readyConn == null)
			{
				if (!s_IsReady)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Must call AddPlayer() with a connection the first time to become ready.");
					}
					return false;
				}
			}
			else
			{
				s_IsReady = true;
				s_ReadyConnection = readyConn;
			}
			PlayerController playerController;
			if (s_ReadyConnection.GetPlayerController(playerControllerId, out playerController) && playerController.IsValid && playerController.gameObject != null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " already in use.");
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::AddPlayer() for ID " + playerControllerId + " called with connection [" + s_ReadyConnection + "]");
			}
			AddPlayerMessage addPlayerMessage = new AddPlayerMessage();
			addPlayerMessage.playerControllerId = playerControllerId;
			if (extraMessage != null)
			{
				NetworkWriter networkWriter = new NetworkWriter();
				extraMessage.Serialize(networkWriter);
				addPlayerMessage.msgData = networkWriter.ToArray();
				addPlayerMessage.msgSize = networkWriter.Position;
			}
			s_ReadyConnection.Send(37, addPlayerMessage);
			return true;
		}

		public static bool RemovePlayer(short playerControllerId)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::RemovePlayer() for ID " + playerControllerId + " called with connection [" + s_ReadyConnection + "]");
			}
			if (s_ReadyConnection.GetPlayerController(playerControllerId, out PlayerController playerController))
			{
				RemovePlayerMessage removePlayerMessage = new RemovePlayerMessage();
				removePlayerMessage.playerControllerId = playerControllerId;
				s_ReadyConnection.Send(38, removePlayerMessage);
				s_ReadyConnection.RemovePlayerController(playerControllerId);
				s_LocalPlayers[playerControllerId] = new PlayerController();
				Object.Destroy(playerController.gameObject);
				return true;
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Failed to find player ID " + playerControllerId);
			}
			return false;
		}

		public static bool Ready(NetworkConnection conn)
		{
			if (s_IsReady)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("A connection has already been set as ready. There can only be one.");
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::Ready() called with connection [" + conn + "]");
			}
			if (conn != null)
			{
				ReadyMessage msg = new ReadyMessage();
				conn.Send(35, msg);
				s_IsReady = true;
				s_ReadyConnection = conn;
				s_ReadyConnection.isReady = true;
				return true;
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Ready() called with invalid connection object: conn=null");
			}
			return false;
		}

		public static NetworkClient ConnectLocalServer()
		{
			LocalClient localClient = new LocalClient();
			NetworkServer.instance.ActivateLocalClientScene();
			localClient.InternalConnectLocalServer();
			return localClient;
		}

		internal static void HandleClientDisconnect(NetworkConnection conn)
		{
			if (s_ReadyConnection == conn && s_IsReady)
			{
				s_IsReady = false;
				s_ReadyConnection = null;
			}
		}

		internal static void PrepareToSpawnSceneObjects()
		{
			s_SpawnableObjects = new Dictionary<NetworkSceneId, NetworkIdentity>();
			NetworkIdentity[] array = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
			foreach (NetworkIdentity networkIdentity in array)
			{
				if (!networkIdentity.gameObject.activeSelf && networkIdentity.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity.gameObject.hideFlags != HideFlags.HideAndDontSave && !networkIdentity.sceneId.IsEmpty())
				{
					s_SpawnableObjects[networkIdentity.sceneId] = networkIdentity;
					if (LogFilter.logDebug)
					{
						Debug.Log("ClientScene::PrepareSpawnObjects sceneId:" + networkIdentity.sceneId);
					}
				}
			}
		}

		internal static NetworkIdentity SpawnSceneObject(NetworkSceneId sceneId)
		{
			if (s_SpawnableObjects.ContainsKey(sceneId))
			{
				NetworkIdentity result = s_SpawnableObjects[sceneId];
				s_SpawnableObjects.Remove(sceneId);
				return result;
			}
			return null;
		}

		internal static void RegisterSystemHandlers(NetworkClient client, bool localClient)
		{
			if (localClient)
			{
				client.RegisterHandlerSafe(1, OnLocalClientObjectDestroy);
				client.RegisterHandlerSafe(13, OnLocalClientObjectHide);
				client.RegisterHandlerSafe(3, OnLocalClientObjectSpawn);
				client.RegisterHandlerSafe(10, OnLocalClientObjectSpawnScene);
				client.RegisterHandlerSafe(15, OnClientAuthority);
			}
			else
			{
				client.RegisterHandlerSafe(3, OnObjectSpawn);
				client.RegisterHandlerSafe(10, OnObjectSpawnScene);
				client.RegisterHandlerSafe(12, OnObjectSpawnFinished);
				client.RegisterHandlerSafe(1, OnObjectDestroy);
				client.RegisterHandlerSafe(13, OnObjectDestroy);
				client.RegisterHandlerSafe(8, OnUpdateVarsMessage);
				client.RegisterHandlerSafe(4, OnOwnerMessage);
				client.RegisterHandlerSafe(9, OnSyncListMessage);
				client.RegisterHandlerSafe(40, NetworkAnimator.OnAnimationClientMessage);
				client.RegisterHandlerSafe(41, NetworkAnimator.OnAnimationParametersClientMessage);
				client.RegisterHandlerSafe(15, OnClientAuthority);
			}
			client.RegisterHandlerSafe(2, OnRPCMessage);
			client.RegisterHandlerSafe(7, OnSyncEventMessage);
			client.RegisterHandlerSafe(42, NetworkAnimator.OnAnimationTriggerClientMessage);
		}

		internal static string GetStringForAssetId(NetworkHash128 assetId)
		{
			if (NetworkScene.GetPrefab(assetId, out GameObject prefab))
			{
				return prefab.name;
			}
			if (NetworkScene.GetSpawnHandler(assetId, out SpawnDelegate handler))
			{
				return handler.Method.Name;
			}
			return "unknown";
		}

		public static void RegisterPrefab(GameObject prefab)
		{
			NetworkScene.RegisterPrefab(prefab);
		}

		public static void RegisterPrefab(GameObject prefab, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			NetworkScene.RegisterPrefab(prefab, spawnHandler, unspawnHandler);
		}

		public static void UnregisterPrefab(GameObject prefab)
		{
			NetworkScene.UnregisterPrefab(prefab);
		}

		public static void RegisterSpawnHandler(NetworkHash128 assetId, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			NetworkScene.RegisterSpawnHandler(assetId, spawnHandler, unspawnHandler);
		}

		public static void UnregisterSpawnHandler(NetworkHash128 assetId)
		{
			NetworkScene.UnregisterSpawnHandler(assetId);
		}

		public static void ClearSpawners()
		{
			NetworkScene.ClearSpawners();
		}

		public static void DestroyAllClientObjects()
		{
			s_NetworkScene.DestroyAllClientObjects();
		}

		public static void SetLocalObject(NetworkInstanceId netId, GameObject obj)
		{
			s_NetworkScene.SetLocalObject(netId, obj, s_IsSpawnFinished, isServer: false);
		}

		public static GameObject FindLocalObject(NetworkInstanceId netId)
		{
			return s_NetworkScene.FindLocalObject(netId);
		}

		private static void ApplySpawnPayload(NetworkIdentity uv, Vector3 position, byte[] payload, NetworkInstanceId netId, GameObject newGameObject)
		{
			uv.transform.position = position;
			if (payload != null && payload.Length > 0)
			{
				NetworkReader reader = new NetworkReader(payload);
				uv.OnUpdateVars(reader, initialState: true);
			}
			if (!(newGameObject == null))
			{
				newGameObject.SetActive(value: true);
				uv.SetNetworkInstanceId(netId);
				SetLocalObject(netId, newGameObject);
				if (s_IsSpawnFinished)
				{
					uv.OnStartClient();
					CheckForOwner(uv);
				}
			}
		}

		private static void OnObjectSpawn(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectSpawnMessage);
			if (!s_ObjectSpawnMessage.assetId.IsValid())
			{
				if (LogFilter.logError)
				{
					Debug.LogError("OnObjSpawn netId: " + s_ObjectSpawnMessage.netId + " has invalid asset Id");
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Client spawn handler instantiating [netId:" + s_ObjectSpawnMessage.netId + " asset ID:" + s_ObjectSpawnMessage.assetId + " pos:" + s_ObjectSpawnMessage.position + "]");
			}
			GameObject prefab;
			SpawnDelegate handler;
			if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnMessage.netId, out NetworkIdentity uv))
			{
				ApplySpawnPayload(uv, s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.payload, s_ObjectSpawnMessage.netId, null);
			}
			else if (NetworkScene.GetPrefab(s_ObjectSpawnMessage.assetId, out prefab))
			{
				GameObject gameObject = (GameObject)Object.Instantiate(prefab, s_ObjectSpawnMessage.position, Quaternion.identity);
				uv = gameObject.GetComponent<NetworkIdentity>();
				if (uv == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Client object spawned for " + s_ObjectSpawnMessage.assetId + " does not have a NetworkIdentity");
					}
				}
				else
				{
					ApplySpawnPayload(uv, s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.payload, s_ObjectSpawnMessage.netId, gameObject);
				}
			}
			else if (NetworkScene.GetSpawnHandler(s_ObjectSpawnMessage.assetId, out handler))
			{
				GameObject gameObject2 = handler(s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.assetId);
				if (gameObject2 == null)
				{
					if (LogFilter.logWarn)
					{
						Debug.LogWarning("Client spawn handler for " + s_ObjectSpawnMessage.assetId + " returned null");
					}
					return;
				}
				uv = gameObject2.GetComponent<NetworkIdentity>();
				if (uv == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Client object spawned for " + s_ObjectSpawnMessage.assetId + " does not have a network identity");
					}
				}
				else
				{
					uv.SetDynamicAssetId(s_ObjectSpawnMessage.assetId);
					ApplySpawnPayload(uv, s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.payload, s_ObjectSpawnMessage.netId, gameObject2);
				}
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Failed to spawn server object, assetId=" + s_ObjectSpawnMessage.assetId + " netId=" + s_ObjectSpawnMessage.netId);
			}
		}

		private static void OnObjectSpawnScene(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectSpawnSceneMessage);
			if (LogFilter.logDebug)
			{
				Debug.Log("Client spawn scene handler instantiating [netId:" + s_ObjectSpawnSceneMessage.netId + " sceneId:" + s_ObjectSpawnSceneMessage.sceneId + " pos:" + s_ObjectSpawnSceneMessage.position);
			}
			if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnSceneMessage.netId, out NetworkIdentity uv))
			{
				ApplySpawnPayload(uv, s_ObjectSpawnSceneMessage.position, s_ObjectSpawnSceneMessage.payload, s_ObjectSpawnSceneMessage.netId, uv.gameObject);
				return;
			}
			NetworkIdentity networkIdentity = SpawnSceneObject(s_ObjectSpawnSceneMessage.sceneId);
			if (networkIdentity == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Spawn scene object not found for " + s_ObjectSpawnSceneMessage.sceneId);
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Client spawn for [netId:" + s_ObjectSpawnSceneMessage.netId + "] [sceneId:" + s_ObjectSpawnSceneMessage.sceneId + "] obj:" + networkIdentity.gameObject.name);
			}
			ApplySpawnPayload(networkIdentity, s_ObjectSpawnSceneMessage.position, s_ObjectSpawnSceneMessage.payload, s_ObjectSpawnSceneMessage.netId, networkIdentity.gameObject);
		}

		private static void OnObjectSpawnFinished(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectSpawnFinishedMessage);
			if (LogFilter.logDebug)
			{
				Debug.Log("SpawnFinished:" + s_ObjectSpawnFinishedMessage.state);
			}
			if (s_ObjectSpawnFinishedMessage.state == 0)
			{
				PrepareToSpawnSceneObjects();
				s_IsSpawnFinished = false;
			}
			else
			{
				foreach (NetworkIdentity value in objects.Values)
				{
					if (!value.isClient)
					{
						value.OnStartClient();
						CheckForOwner(value);
					}
				}
				s_IsSpawnFinished = true;
			}
		}

		private static void OnObjectDestroy(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectDestroyMessage);
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnObjDestroy netId:" + s_ObjectDestroyMessage.netId);
			}
			if (s_NetworkScene.GetNetworkIdentity(s_ObjectDestroyMessage.netId, out NetworkIdentity uv))
			{
				uv.OnNetworkDestroy();
				if (!NetworkScene.InvokeUnSpawnHandler(uv.assetId, uv.gameObject))
				{
					if (uv.sceneId.IsEmpty())
					{
						Object.Destroy(uv.gameObject);
					}
					else
					{
						uv.gameObject.SetActive(value: false);
						s_SpawnableObjects[uv.sceneId] = uv;
					}
				}
				s_NetworkScene.RemoveLocalObject(s_ObjectDestroyMessage.netId);
			}
			else if (LogFilter.logDebug)
			{
				Debug.LogWarning("Did not find target for destroy message for " + s_ObjectDestroyMessage.netId);
			}
		}

		private static void OnLocalClientObjectDestroy(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectDestroyMessage);
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnLocalObjectObjDestroy netId:" + s_ObjectDestroyMessage.netId);
			}
			s_NetworkScene.RemoveLocalObject(s_ObjectDestroyMessage.netId);
		}

		private static void OnLocalClientObjectHide(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectDestroyMessage);
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnLocalObjectObjHide netId:" + s_ObjectDestroyMessage.netId);
			}
			if (s_NetworkScene.GetNetworkIdentity(s_ObjectDestroyMessage.netId, out NetworkIdentity uv))
			{
				uv.OnSetLocalVisibility(vis: false);
			}
		}

		private static void OnLocalClientObjectSpawn(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectSpawnMessage);
			if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnMessage.netId, out NetworkIdentity uv))
			{
				uv.OnSetLocalVisibility(vis: true);
			}
		}

		private static void OnLocalClientObjectSpawnScene(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ObjectSpawnSceneMessage);
			if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnSceneMessage.netId, out NetworkIdentity uv))
			{
				uv.OnSetLocalVisibility(vis: true);
			}
		}

		private static void OnUpdateVarsMessage(NetworkMessage netMsg)
		{
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			if (LogFilter.logDev)
			{
				Debug.Log("ClientScene::OnUpdateVarsMessage " + networkInstanceId + " channel:" + netMsg.channelId);
			}
			if (s_NetworkScene.GetNetworkIdentity(networkInstanceId, out NetworkIdentity uv))
			{
				uv.OnUpdateVars(netMsg.reader, initialState: false);
			}
			else if (LogFilter.logWarn)
			{
				Debug.LogWarning("Did not find target for sync message for " + networkInstanceId);
			}
		}

		private static void OnRPCMessage(NetworkMessage netMsg)
		{
			int num = (int)netMsg.reader.ReadPackedUInt32();
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnRPCMessage hash:" + num + " netId:" + networkInstanceId);
			}
			if (s_NetworkScene.GetNetworkIdentity(networkInstanceId, out NetworkIdentity uv))
			{
				uv.HandleRPC(num, netMsg.reader);
			}
			else if (LogFilter.logWarn)
			{
				Debug.LogWarning("Did not find target for RPC message for " + networkInstanceId);
			}
		}

		private static void OnSyncEventMessage(NetworkMessage netMsg)
		{
			int cmdHash = (int)netMsg.reader.ReadPackedUInt32();
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnSyncEventMessage " + networkInstanceId);
			}
			if (s_NetworkScene.GetNetworkIdentity(networkInstanceId, out NetworkIdentity uv))
			{
				uv.HandleSyncEvent(cmdHash, netMsg.reader);
			}
			else if (LogFilter.logWarn)
			{
				Debug.LogWarning("Did not find target for SyncEvent message for " + networkInstanceId);
			}
		}

		private static void OnSyncListMessage(NetworkMessage netMsg)
		{
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			int cmdHash = (int)netMsg.reader.ReadPackedUInt32();
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnSyncListMessage " + networkInstanceId);
			}
			if (s_NetworkScene.GetNetworkIdentity(networkInstanceId, out NetworkIdentity uv))
			{
				uv.HandleSyncList(cmdHash, netMsg.reader);
			}
			else if (LogFilter.logWarn)
			{
				Debug.LogWarning("Did not find target for SyncList message for " + networkInstanceId);
			}
		}

		private static void OnClientAuthority(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_ClientAuthorityMessage);
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnClientAuthority for  connectionId=" + netMsg.conn.connectionId + " netId: " + s_ClientAuthorityMessage.netId);
			}
			if (s_NetworkScene.GetNetworkIdentity(s_ClientAuthorityMessage.netId, out NetworkIdentity uv))
			{
				uv.HandleClientAuthority(s_ClientAuthorityMessage.authority);
			}
		}

		private static void OnOwnerMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_OwnerMessage);
			if (LogFilter.logDebug)
			{
				Debug.Log("ClientScene::OnOwnerMessage - connectionId=" + netMsg.conn.connectionId + " netId: " + s_OwnerMessage.netId);
			}
			if (netMsg.conn.GetPlayerController(s_OwnerMessage.playerControllerId, out PlayerController playerController))
			{
				playerController.unetView.SetNotLocalPlayer();
			}
			if (s_NetworkScene.GetNetworkIdentity(s_OwnerMessage.netId, out NetworkIdentity uv))
			{
				uv.SetConnectionToServer(netMsg.conn);
				uv.SetLocalPlayer(s_OwnerMessage.playerControllerId);
				InternalAddPlayer(uv, s_OwnerMessage.playerControllerId);
			}
			else
			{
				PendingOwner pendingOwner = default(PendingOwner);
				pendingOwner.netId = s_OwnerMessage.netId;
				pendingOwner.playerControllerId = s_OwnerMessage.playerControllerId;
				PendingOwner item = pendingOwner;
				s_PendingOwnerIds.Add(item);
			}
		}

		private static void CheckForOwner(NetworkIdentity uv)
		{
			int num = 0;
			PendingOwner pendingOwner;
			while (true)
			{
				if (num < s_PendingOwnerIds.Count)
				{
					pendingOwner = s_PendingOwnerIds[num];
					if (pendingOwner.netId == uv.netId)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			uv.SetConnectionToServer(s_ReadyConnection);
			uv.SetLocalPlayer(pendingOwner.playerControllerId);
			if (LogFilter.logDev)
			{
				Debug.Log("ClientScene::OnOwnerMessage - player=" + uv.gameObject.name);
			}
			if (s_ReadyConnection.connectionId < 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Owner message received on a local client.");
				}
			}
			else
			{
				InternalAddPlayer(uv, pendingOwner.playerControllerId);
				s_PendingOwnerIds.RemoveAt(num);
			}
		}
	}
}
