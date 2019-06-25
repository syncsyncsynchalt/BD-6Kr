using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkIdentity")]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public sealed class NetworkIdentity : MonoBehaviour
	{
		[SerializeField]
		private NetworkSceneId m_SceneId;

		[SerializeField]
		private NetworkHash128 m_AssetId;

		[SerializeField]
		private bool m_ServerOnly;

		[SerializeField]
		private bool m_LocalPlayerAuthority;

		private bool m_IsClient;

		private bool m_IsServer;

		private bool m_HasAuthority;

		private NetworkInstanceId m_NetId;

		private bool m_IsLocalPlayer;

		private NetworkConnection m_ConnectionToServer;

		private NetworkConnection m_ConnectionToClient;

		private short m_PlayerId = -1;

		private NetworkBehaviour[] m_NetworkBehaviours;

		private HashSet<int> m_ObserverConnections;

		private List<NetworkConnection> m_Observers;

		private NetworkConnection m_ClientAuthorityOwner;

		private static uint s_NextNetworkId = 1u;

		private static NetworkWriter s_UpdateWriter = new NetworkWriter();

		public bool isClient => m_IsClient;

		public bool isServer
		{
			get
			{
				if (!m_IsServer)
				{
					return false;
				}
				return NetworkServer.active && m_IsServer;
			}
		}

		public bool hasAuthority => m_HasAuthority;

		public NetworkInstanceId netId => m_NetId;

		public NetworkSceneId sceneId => m_SceneId;

		public bool serverOnly
		{
			get
			{
				return m_ServerOnly;
			}
			set
			{
				m_ServerOnly = value;
			}
		}

		public bool localPlayerAuthority
		{
			get
			{
				return m_LocalPlayerAuthority;
			}
			set
			{
				m_LocalPlayerAuthority = value;
			}
		}

		public NetworkConnection clientAuthorityOwner => m_ClientAuthorityOwner;

		public NetworkHash128 assetId => m_AssetId;

		public bool isLocalPlayer => m_IsLocalPlayer;

		public short playerControllerId => m_PlayerId;

		public NetworkConnection connectionToServer => m_ConnectionToServer;

		public NetworkConnection connectionToClient => m_ConnectionToClient;

		public ReadOnlyCollection<NetworkConnection> observers
		{
			get
			{
				if (m_Observers == null)
				{
					return null;
				}
				return new ReadOnlyCollection<NetworkConnection>(m_Observers);
			}
		}

		internal void SetDynamicAssetId(NetworkHash128 newAssetId)
		{
			if (!m_AssetId.IsValid() || m_AssetId.Equals(newAssetId))
			{
				m_AssetId = newAssetId;
			}
			else if (LogFilter.logWarn)
			{
				Debug.LogWarning("SetDynamicAssetId object already has an assetId <" + m_AssetId + ">");
			}
		}

		internal void SetClientOwner(NetworkConnection conn)
		{
			if (m_ClientAuthorityOwner != null && LogFilter.logError)
			{
				Debug.LogError("SetClientOwner m_ClientAuthorityOwner already set!");
			}
			m_ClientAuthorityOwner = conn;
			m_ClientAuthorityOwner.AddOwnedObject(this);
		}

		internal void ClearClientOwner()
		{
			m_ClientAuthorityOwner = null;
		}

		internal void ForceAuthority(bool authority)
		{
			m_HasAuthority = authority;
			if (authority)
			{
				OnStartAuthority();
			}
			else
			{
				OnStopAuthority();
			}
		}

		internal static NetworkInstanceId GetNextNetworkId()
		{
			uint value = s_NextNetworkId;
			s_NextNetworkId++;
			return new NetworkInstanceId(value);
		}

		private void CacheBehaviours()
		{
			if (m_NetworkBehaviours == null)
			{
				m_NetworkBehaviours = GetComponents<NetworkBehaviour>();
			}
		}

		internal void SetNetworkInstanceId(NetworkInstanceId newNetId)
		{
			m_NetId = newNetId;
		}

		public void ForceSceneId(int newSceneId)
		{
			m_SceneId = new NetworkSceneId((uint)newSceneId);
		}

		internal void UpdateClientServer(bool isClientFlag, bool isServerFlag)
		{
			m_IsClient |= isClientFlag;
			m_IsServer |= isServerFlag;
		}

		internal void SetNoServer()
		{
			m_IsServer = false;
			SetNetworkInstanceId(NetworkInstanceId.Zero);
		}

		internal void SetNotLocalPlayer()
		{
			m_IsLocalPlayer = false;
			m_HasAuthority = false;
		}

		internal void RemoveObserverInternal(NetworkConnection conn)
		{
			if (m_Observers != null)
			{
				m_Observers.Remove(conn);
				m_ObserverConnections.Remove(conn.connectionId);
			}
		}

		private void OnDestroy()
		{
			if (m_IsServer)
			{
				NetworkServer.Destroy(base.gameObject);
			}
		}

		internal void OnStartServer()
		{
			if (m_IsServer)
			{
				return;
			}
			m_IsServer = true;
			if (m_LocalPlayerAuthority)
			{
				m_HasAuthority = false;
			}
			else
			{
				m_HasAuthority = true;
			}
			m_Observers = new List<NetworkConnection>();
			m_ObserverConnections = new HashSet<int>();
			CacheBehaviours();
			if (netId.IsEmpty())
			{
				m_NetId = GetNextNetworkId();
				if (LogFilter.logDev)
				{
					Debug.Log("OnStartServer " + base.gameObject + " GUID:" + netId);
				}
				NetworkServer.instance.SetLocalObjectOnServer(netId, base.gameObject);
				for (int i = 0; i < m_NetworkBehaviours.Length; i++)
				{
					NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
					try
					{
						networkBehaviour.OnStartServer();
					}
					catch (Exception ex)
					{
						Debug.LogError("Exception in OnStartServer:" + ex.Message + " " + ex.StackTrace);
					}
				}
				if (NetworkClient.active && NetworkServer.localClientActive)
				{
					ClientScene.SetLocalObject(netId, base.gameObject);
					OnStartClient();
				}
				if (hasAuthority)
				{
					OnStartAuthority();
				}
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Object has non-zero netId " + netId + " for " + base.gameObject + " !!1");
			}
		}

		internal void OnStartClient()
		{
			if (!m_IsClient)
			{
				m_IsClient = true;
			}
			CacheBehaviours();
			if (LogFilter.logDev)
			{
				Debug.Log("OnStartClient " + base.gameObject + " GUID:" + netId + " localPlayerAuthority:" + localPlayerAuthority);
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.PreStartClient();
					networkBehaviour.OnStartClient();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnStartClient:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal void OnStartAuthority()
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.OnStartAuthority();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnStartAuthority:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal void OnStopAuthority()
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.OnStopAuthority();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnStopAuthority:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal void OnSetLocalVisibility(bool vis)
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.OnSetLocalVisibility(vis);
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnSetLocalVisibility:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal bool OnCheckObserver(NetworkConnection conn)
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					if (!networkBehaviour.OnCheckObserver(conn))
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnCheckObserver:" + ex.Message + " " + ex.StackTrace);
				}
			}
			return true;
		}

		internal void UNetSerializeAllVars(NetworkWriter writer)
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				networkBehaviour.OnSerialize(writer, initialState: true);
			}
		}

		internal void HandleSyncEvent(int cmdHash, NetworkReader reader)
		{
			if (base.gameObject == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SyncEvent [" + NetworkBehaviour.GetCmdHashHandlerName(cmdHash) + "] received for deleted object " + netId);
				}
				return;
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				if (networkBehaviour.InvokeSyncEvent(cmdHash, reader))
				{
					break;
				}
			}
		}

		internal void HandleClientAuthority(bool authority)
		{
			if (!localPlayerAuthority)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleClientAuthority " + base.gameObject + " does not have localPlayerAuthority");
				}
			}
			else
			{
				ForceAuthority(authority);
			}
		}

		internal void HandleSyncList(int cmdHash, NetworkReader reader)
		{
			if (base.gameObject == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SyncList [" + NetworkBehaviour.GetCmdHashHandlerName(cmdHash) + "] received for deleted object " + netId);
				}
				return;
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				if (networkBehaviour.InvokeSyncList(cmdHash, reader))
				{
					break;
				}
			}
		}

		internal void HandleCommand(int cmdHash, NetworkReader reader)
		{
			if (base.gameObject == null)
			{
				string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Command [" + cmdHashHandlerName + "] received for deleted object [netId=" + netId + "]");
				}
				return;
			}
			bool flag = false;
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				if (networkBehaviour.InvokeCommand(cmdHash, reader))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				string cmdHashHandlerName2 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logError)
				{
					Debug.LogError("Found no receiver for incoming command [" + cmdHashHandlerName2 + "] on " + base.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "].");
				}
			}
		}

		internal void HandleRPC(int cmdHash, NetworkReader reader)
		{
			if (base.gameObject == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ClientRpc [" + NetworkBehaviour.GetCmdHashHandlerName(cmdHash) + "] received for deleted object " + netId);
				}
				return;
			}
			if (m_NetworkBehaviours.Length == 0)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("No receiver found for ClientRpc [" + NetworkBehaviour.GetCmdHashHandlerName(cmdHash) + "]. Does the script with the function inherit NetworkBehaviour?");
				}
				return;
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				if (networkBehaviour.InvokeRPC(cmdHash, reader))
				{
					return;
				}
			}
			string text = NetworkBehaviour.GetInvoker(cmdHash);
			if (text == null)
			{
				text = "[unknown:" + cmdHash + "]";
			}
			if (LogFilter.logWarn)
			{
				Debug.LogWarning("Failed to invoke RPC " + text + "(" + cmdHash + ") on netID " + netId);
			}
			NetworkBehaviour.DumpInvokers();
		}

		internal void UNetUpdate()
		{
			uint num = 0u;
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				int dirtyChannel = networkBehaviour.GetDirtyChannel();
				if (dirtyChannel != -1)
				{
					num = (uint)((int)num | (1 << dirtyChannel));
				}
			}
			if (num == 0)
			{
				return;
			}
			for (int j = 0; j < NetworkServer.numChannels; j++)
			{
				if (((int)num & (1 << j)) == 0)
				{
					continue;
				}
				s_UpdateWriter.StartMessage(8);
				s_UpdateWriter.Write(netId);
				bool flag = false;
				for (int k = 0; k < m_NetworkBehaviours.Length; k++)
				{
					short position = s_UpdateWriter.Position;
					NetworkBehaviour networkBehaviour2 = m_NetworkBehaviours[k];
					if (networkBehaviour2.GetDirtyChannel() != j)
					{
						networkBehaviour2.OnSerialize(s_UpdateWriter, initialState: false);
						continue;
					}
					if (networkBehaviour2.OnSerialize(s_UpdateWriter, initialState: false))
					{
						networkBehaviour2.ClearAllDirtyBits();
						flag = true;
					}
					if (s_UpdateWriter.Position - position > NetworkServer.maxPacketSize)
					{
						Debug.LogWarning("Large state update of " + (s_UpdateWriter.Position - position) + " bytes for netId:" + netId + " from script:" + networkBehaviour2);
					}
				}
				if (flag)
				{
					s_UpdateWriter.FinishMessage();
					NetworkServer.SendWriterToReady(base.gameObject, s_UpdateWriter, j);
				}
			}
		}

		internal void OnUpdateVars(NetworkReader reader, bool initialState)
		{
			if (initialState && m_NetworkBehaviours == null)
			{
				m_NetworkBehaviours = GetComponents<NetworkBehaviour>();
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				networkBehaviour.OnDeserialize(reader, initialState);
			}
		}

		internal void SetLocalPlayer(short localPlayerControllerId)
		{
			m_IsLocalPlayer = true;
			m_PlayerId = localPlayerControllerId;
			if (localPlayerAuthority)
			{
				m_HasAuthority = true;
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				networkBehaviour.OnStartLocalPlayer();
				if (localPlayerAuthority)
				{
					networkBehaviour.OnStartAuthority();
				}
			}
		}

		internal void SetConnectionToServer(NetworkConnection conn)
		{
			m_ConnectionToServer = conn;
		}

		internal void SetConnectionToClient(NetworkConnection conn, short newPlayerControllerId)
		{
			m_PlayerId = newPlayerControllerId;
			m_ConnectionToClient = conn;
		}

		internal void OnNetworkDestroy()
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				networkBehaviour.OnNetworkDestroy();
			}
			m_IsServer = false;
		}

		internal void ClearObservers()
		{
			if (m_Observers != null)
			{
				int count = m_Observers.Count;
				for (int i = 0; i < count; i++)
				{
					NetworkConnection networkConnection = m_Observers[i];
					networkConnection.RemoveFromVisList(this, isDestroyed: true);
				}
				m_Observers.Clear();
				m_ObserverConnections.Clear();
			}
		}

		internal void AddObserver(NetworkConnection conn)
		{
			if (m_Observers == null)
			{
				return;
			}
			if (m_ObserverConnections.Contains(conn.connectionId))
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("Duplicate observer " + conn.address + " added for " + base.gameObject);
				}
				return;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("Added observer " + conn.address + " added for " + base.gameObject);
			}
			m_Observers.Add(conn);
			m_ObserverConnections.Add(conn.connectionId);
			conn.AddToVisList(this);
		}

		internal void RemoveObserver(NetworkConnection conn)
		{
			if (m_Observers != null)
			{
				m_Observers.Remove(conn);
				m_ObserverConnections.Remove(conn.connectionId);
				conn.RemoveFromVisList(this, isDestroyed: false);
			}
		}

		public void RebuildObservers(bool initialize)
		{
			if (m_Observers == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			HashSet<NetworkConnection> hashSet = new HashSet<NetworkConnection>();
			HashSet<NetworkConnection> hashSet2 = new HashSet<NetworkConnection>(m_Observers);
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				flag2 |= networkBehaviour.OnRebuildObservers(hashSet, initialize);
			}
			if (!flag2)
			{
				if (initialize)
				{
					foreach (NetworkConnection connection in NetworkServer.connections)
					{
						if (connection != null && connection.isReady)
						{
							AddObserver(connection);
						}
					}
					foreach (NetworkConnection localConnection in NetworkServer.localConnections)
					{
						if (localConnection != null && localConnection.isReady)
						{
							AddObserver(localConnection);
						}
					}
				}
				return;
			}
			foreach (NetworkConnection item in hashSet)
			{
				if (item != null)
				{
					if (!item.isReady)
					{
						if (LogFilter.logWarn)
						{
							Debug.LogWarning("Observer is not ready for " + base.gameObject + " " + item);
						}
					}
					else if (initialize || !hashSet2.Contains(item))
					{
						item.AddToVisList(this);
						if (LogFilter.logDebug)
						{
							Debug.Log("New Observer for " + base.gameObject + " " + item);
						}
						flag = true;
					}
				}
			}
			foreach (NetworkConnection item2 in hashSet2)
			{
				if (!hashSet.Contains(item2))
				{
					item2.RemoveFromVisList(this, isDestroyed: false);
					if (LogFilter.logDebug)
					{
						Debug.Log("Removed Observer for " + base.gameObject + " " + item2);
					}
					flag = true;
				}
			}
			if (initialize)
			{
				foreach (NetworkConnection localConnection2 in NetworkServer.localConnections)
				{
					if (!hashSet.Contains(localConnection2))
					{
						OnSetLocalVisibility(vis: false);
					}
				}
			}
			if (flag)
			{
				m_Observers = new List<NetworkConnection>(hashSet);
				m_ObserverConnections.Clear();
				foreach (NetworkConnection observer in m_Observers)
				{
					m_ObserverConnections.Add(observer.connectionId);
				}
			}
		}

		public bool RemoveClientAuthority(NetworkConnection conn)
		{
			if (!isServer)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority can only be call on the server for spawned objects.");
				}
				return false;
			}
			if (connectionToClient != null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority cannot remove authority for a player object");
				}
				return false;
			}
			if (m_ClientAuthorityOwner == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority for " + base.gameObject + " has no clientAuthority owner.");
				}
				return false;
			}
			if (m_ClientAuthorityOwner != conn)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority for " + base.gameObject + " has different owner.");
				}
				return false;
			}
			m_ClientAuthorityOwner.RemoveOwnedObject(this);
			m_ClientAuthorityOwner = null;
			ForceAuthority(authority: true);
			ClientAuthorityMessage clientAuthorityMessage = new ClientAuthorityMessage();
			clientAuthorityMessage.netId = netId;
			clientAuthorityMessage.authority = false;
			conn.Send(15, clientAuthorityMessage);
			return true;
		}

		public bool AssignClientAuthority(NetworkConnection conn)
		{
			if (!isServer)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority can only be call on the server for spawned objects.");
				}
				return false;
			}
			if (!localPlayerAuthority)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority can only be used for NetworkIdentity component with LocalPlayerAuthority set.");
				}
				return false;
			}
			if (m_ClientAuthorityOwner != null && conn != m_ClientAuthorityOwner)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority for " + base.gameObject + " already has an owner. Use RemoveClientAuthority() first.");
				}
				return false;
			}
			if (conn == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority for " + base.gameObject + " owner cannot be null. Use RemoveClientAuthority() instead.");
				}
				return false;
			}
			m_ClientAuthorityOwner = conn;
			m_ClientAuthorityOwner.AddOwnedObject(this);
			ForceAuthority(authority: false);
			ClientAuthorityMessage clientAuthorityMessage = new ClientAuthorityMessage();
			clientAuthorityMessage.netId = netId;
			clientAuthorityMessage.authority = true;
			conn.Send(15, clientAuthorityMessage);
			return true;
		}

		internal static void UNetStaticUpdate()
		{
			NetworkServer.Update();
			NetworkClient.UpdateClients();
			NetworkManager.UpdateScene();
		}
	}
}
