using System;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
	public sealed class NetworkServer
	{
		private const int k_MaxEventsPerFrame = 500;

		private const int k_RemoveListInterval = 100;

		private static Type s_NetworkConnectionClass = typeof(NetworkConnection);

		private static bool s_Active;

		private static volatile NetworkServer s_Instance;

		private static object s_Sync = new Object();

		private static bool s_LocalClientActive;

		private static HashSet<int> s_ExternalConnections = new HashSet<int>();

		private int m_ServerId = -1;

		private int m_ServerPort = -1;

		private int m_RelaySlotId = -1;

		private NetworkMessageHandlers m_MessageHandlers = new NetworkMessageHandlers();

		private ConnectionArray m_Connections = new ConnectionArray();

		private static NetworkScene s_NetworkScene = new NetworkScene();

		private HostTopology m_HostTopology;

		private byte[] m_MsgBuffer;

		private bool m_SendPeerInfo = true;

		private bool m_UseWebSockets;

		private float m_MaxDelay = 0.1f;

		private List<LocalClient> m_LocalClients = new List<LocalClient>();

		private HashSet<NetworkInstanceId> m_RemoveList;

		private int m_RemoveListCount;

		internal static ushort maxPacketSize;

		private static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();

		public static Dictionary<short, NetworkMessageDelegate> handlers => instance.m_MessageHandlers.GetHandlers();

		public static List<NetworkConnection> connections => instance.m_Connections.connections;

		public static List<NetworkConnection> localConnections => instance.m_Connections.localConnections;

		public static Dictionary<NetworkInstanceId, NetworkIdentity> objects => s_NetworkScene.localObjects;

		public static bool useWebSockets
		{
			get
			{
				return instance.m_UseWebSockets;
			}
			set
			{
				instance.m_UseWebSockets = value;
			}
		}

		public static bool sendPeerInfo
		{
			get
			{
				return instance.m_SendPeerInfo;
			}
			set
			{
				instance.m_SendPeerInfo = value;
			}
		}

		public static Type networkConnectionClass => s_NetworkConnectionClass;

		internal static NetworkServer instance
		{
			get
			{
				if (s_Instance == null)
				{
					lock (s_Sync)
					{
						if (s_Instance == null)
						{
							s_Instance = new NetworkServer();
						}
					}
				}
				return s_Instance;
			}
		}

		public static bool active => s_Active;

		public static bool localClientActive => s_LocalClientActive;

		public static int numChannels => instance.m_HostTopology.DefaultConfig.ChannelCount;

		public static float maxDelay
		{
			get
			{
				return instance.m_MaxDelay;
			}
			set
			{
				instance.InternalSetMaxDelay(value);
			}
		}

		public static HostTopology hostTopology => instance.m_HostTopology;

		private NetworkServer()
		{
			NetworkTransport.Init();
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer Created version " + Version.Current);
			}
			m_MsgBuffer = new byte[49152];
			m_RemoveList = new HashSet<NetworkInstanceId>();
		}

		public static void SetNetworkConnectionClass<T>() where T : NetworkConnection
		{
			s_NetworkConnectionClass = typeof(T);
		}

		public static bool Configure(ConnectionConfig config, int maxConnections)
		{
			HostTopology topology = new HostTopology(config, maxConnections);
			return Configure(topology);
		}

		public static bool Configure(HostTopology topology)
		{
			instance.m_HostTopology = topology;
			return true;
		}

		public static void Reset()
		{
			NetworkTransport.Shutdown();
			NetworkTransport.Init();
			s_NetworkConnectionClass = typeof(NetworkConnection);
			s_Instance = null;
			s_Active = false;
			s_LocalClientActive = false;
			s_ExternalConnections = new HashSet<int>();
		}

		public static void Shutdown()
		{
			if (s_Instance != null)
			{
				s_Instance.InternalDisconnectAll();
				if (s_Instance.m_ServerId != -1)
				{
					NetworkTransport.RemoveHost(s_Instance.m_ServerId);
					s_Instance.m_ServerId = -1;
				}
				s_Instance = null;
			}
			s_ExternalConnections = new HashSet<int>();
			s_Active = false;
			s_LocalClientActive = false;
		}

		public static bool Listen(MatchInfo matchInfo, int listenPort)
		{
			if (!matchInfo.usingRelay)
			{
				return instance.InternalListen(null, listenPort);
			}
			instance.InternalListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId, listenPort);
			return true;
		}

		public static void ListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
		{
			instance.InternalListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId, 0);
		}

		internal void InternalListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId, int listenPort)
		{
			if (m_HostTopology == null)
			{
				ConnectionConfig connectionConfig = new ConnectionConfig();
				connectionConfig.AddChannel(QosType.Reliable);
				connectionConfig.AddChannel(QosType.Unreliable);
				m_HostTopology = new HostTopology(connectionConfig, 8);
			}
			m_ServerId = NetworkTransport.AddHost(m_HostTopology, listenPort);
			if (LogFilter.logDebug)
			{
				Debug.Log("Server Host Slot Id: " + m_ServerId);
			}
			Update();
			NetworkTransport.ConnectAsNetworkHost(m_ServerId, relayIp, relayPort, netGuid, sourceId, nodeId, out byte error);
			m_RelaySlotId = 0;
			if (LogFilter.logDebug)
			{
				Debug.Log("Relay Slot Id: " + m_RelaySlotId);
			}
			if (error != 0)
			{
				Debug.Log("ListenRelay Error: " + error);
			}
			s_Active = true;
			m_MessageHandlers.RegisterHandlerSafe(35, OnClientReadyMessage);
			m_MessageHandlers.RegisterHandlerSafe(5, OnCommandMessage);
			m_MessageHandlers.RegisterHandlerSafe(6, NetworkTransform.HandleTransform);
			m_MessageHandlers.RegisterHandlerSafe(16, NetworkTransformChild.HandleChildTransform);
			m_MessageHandlers.RegisterHandlerSafe(40, NetworkAnimator.OnAnimationServerMessage);
			m_MessageHandlers.RegisterHandlerSafe(41, NetworkAnimator.OnAnimationParametersServerMessage);
			m_MessageHandlers.RegisterHandlerSafe(42, NetworkAnimator.OnAnimationTriggerServerMessage);
		}

		public static bool Listen(int serverPort)
		{
			return instance.InternalListen(null, serverPort);
		}

		public static bool Listen(string ipAddress, int serverPort)
		{
			return instance.InternalListen(ipAddress, serverPort);
		}

		internal bool InternalListen(string ipAddress, int serverPort)
		{
			if (m_HostTopology == null)
			{
				ConnectionConfig connectionConfig = new ConnectionConfig();
				connectionConfig.AddChannel(QosType.Reliable);
				connectionConfig.AddChannel(QosType.Unreliable);
				m_HostTopology = new HostTopology(connectionConfig, 8);
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Server Listen. port: " + serverPort);
			}
			if (string.IsNullOrEmpty(ipAddress))
			{
				if (m_UseWebSockets)
				{
					m_ServerId = NetworkTransport.AddWebsocketHost(m_HostTopology, serverPort);
				}
				else
				{
					m_ServerId = NetworkTransport.AddHost(m_HostTopology, serverPort);
				}
			}
			else if (m_UseWebSockets)
			{
				m_ServerId = NetworkTransport.AddWebsocketHost(m_HostTopology, serverPort, ipAddress);
			}
			else
			{
				m_ServerId = NetworkTransport.AddHost(m_HostTopology, serverPort, ipAddress);
			}
			if (m_ServerId == -1)
			{
				return false;
			}
			m_ServerPort = serverPort;
			s_Active = true;
			maxPacketSize = hostTopology.DefaultConfig.PacketSize;
			m_MessageHandlers.RegisterHandlerSafe(35, OnClientReadyMessage);
			m_MessageHandlers.RegisterHandlerSafe(5, OnCommandMessage);
			m_MessageHandlers.RegisterHandlerSafe(6, NetworkTransform.HandleTransform);
			m_MessageHandlers.RegisterHandlerSafe(16, NetworkTransformChild.HandleChildTransform);
			m_MessageHandlers.RegisterHandlerSafe(38, OnRemovePlayerMessage);
			m_MessageHandlers.RegisterHandlerSafe(40, NetworkAnimator.OnAnimationServerMessage);
			m_MessageHandlers.RegisterHandlerSafe(41, NetworkAnimator.OnAnimationParametersServerMessage);
			m_MessageHandlers.RegisterHandlerSafe(42, NetworkAnimator.OnAnimationTriggerServerMessage);
			return true;
		}

		internal void InternalSetMaxDelay(float seconds)
		{
			for (int i = m_Connections.LocalIndex; i < m_Connections.Count; i++)
			{
				m_Connections.Get(i)?.SetMaxDelay(seconds);
			}
			m_MaxDelay = seconds;
		}

		internal int AddLocalClient(LocalClient localClient)
		{
			m_LocalClients.Add(localClient);
			ULocalConnectionToClient uLocalConnectionToClient = new ULocalConnectionToClient(localClient);
			uLocalConnectionToClient.SetHandlers(m_MessageHandlers);
			uLocalConnectionToClient.InvokeHandlerNoData(32);
			return m_Connections.AddLocal(uLocalConnectionToClient);
		}

		internal void SetLocalObjectOnServer(NetworkInstanceId netId, GameObject obj)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("SetLocalObjectOnServer " + netId + " " + obj);
			}
			s_NetworkScene.SetLocalObject(netId, obj, isClient: false, isServer: true);
		}

		internal void ActivateLocalClientScene()
		{
			if (!s_LocalClientActive)
			{
				s_LocalClientActive = true;
				foreach (NetworkIdentity value in objects.Values)
				{
					if (!value.isClient)
					{
						if (LogFilter.logDev)
						{
							Debug.Log("ActivateClientScene " + value.netId + " " + value.gameObject);
						}
						ClientScene.SetLocalObject(value.netId, value.gameObject);
						value.OnStartClient();
					}
				}
			}
		}

		public static bool SendToAll(short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToAll msgType:" + msgType);
			}
			ConnectionArray connections = instance.m_Connections;
			bool flag = true;
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections.Get(i);
				if (networkConnection != null)
				{
					flag &= networkConnection.Send(msgType, msg);
				}
			}
			return flag;
		}

		private static bool SendToObservers(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToObservers id:" + msgType);
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			if (component == null || component.observers == null)
			{
				return false;
			}
			int count = component.observers.Count;
			for (int i = 0; i < count; i++)
			{
				NetworkConnection networkConnection = component.observers[i];
				flag &= networkConnection.Send(msgType, msg);
			}
			return flag;
		}

		public static bool SendToReady(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToReady id:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = s_Instance.m_Connections.LocalIndex; i < s_Instance.m_Connections.Count; i++)
				{
					NetworkConnection networkConnection = s_Instance.m_Connections.Get(i);
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.Send(msgType, msg);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			if (component == null || component.observers == null)
			{
				return false;
			}
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.Send(msgType, msg);
				}
			}
			return flag;
		}

		public static void SendWriterToReady(GameObject contextObj, NetworkWriter writer, int channelId)
		{
			if (writer.AsArraySegment().Count > 32767)
			{
				throw new UnityException("NetworkWriter used buffer is too big!");
			}
			SendBytesToReady(contextObj, writer.AsArraySegment().Array, writer.AsArraySegment().Count, channelId);
		}

		public static void SendBytesToReady(GameObject contextObj, byte[] buffer, int numBytes, int channelId)
		{
			if (contextObj == null)
			{
				bool flag = true;
				for (int i = s_Instance.m_Connections.LocalIndex; i < s_Instance.m_Connections.Count; i++)
				{
					NetworkConnection networkConnection = s_Instance.m_Connections.Get(i);
					if (networkConnection != null && networkConnection.isReady && !networkConnection.SendBytes(buffer, numBytes, channelId))
					{
						flag = false;
					}
				}
				if (!flag && LogFilter.logWarn)
				{
					Debug.LogWarning("SendBytesToReady failed");
				}
			}
			else
			{
				NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
				try
				{
					bool flag2 = true;
					int count = component.observers.Count;
					for (int j = 0; j < count; j++)
					{
						NetworkConnection networkConnection2 = component.observers[j];
						if (networkConnection2.isReady && !networkConnection2.SendBytes(buffer, numBytes, channelId))
						{
							flag2 = false;
						}
					}
					if (!flag2 && LogFilter.logWarn)
					{
						Debug.LogWarning("SendBytesToReady failed for " + contextObj);
					}
				}
				catch (NullReferenceException)
				{
					if (LogFilter.logWarn)
					{
						Debug.LogWarning("SendBytesToReady object " + contextObj + " has not been spawned");
					}
				}
			}
		}

		public static void SendBytesToPlayer(GameObject player, byte[] buffer, int numBytes, int channelId)
		{
			ConnectionArray connections = instance.m_Connections;
			if (connections.ContainsPlayer(player, out NetworkConnection conn))
			{
				conn.SendBytes(buffer, numBytes, channelId);
			}
		}

		public static bool SendUnreliableToAll(short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendUnreliableToAll msgType:" + msgType);
			}
			ConnectionArray connections = instance.m_Connections;
			bool flag = true;
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections.Get(i);
				if (networkConnection != null)
				{
					flag &= networkConnection.SendUnreliable(msgType, msg);
				}
			}
			return flag;
		}

		public static bool SendUnreliableToReady(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendUnreliableToReady id:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = s_Instance.m_Connections.LocalIndex; i < s_Instance.m_Connections.Count; i++)
				{
					NetworkConnection networkConnection = s_Instance.m_Connections.Get(i);
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.SendUnreliable(msgType, msg);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.SendUnreliable(msgType, msg);
				}
			}
			return flag;
		}

		public static bool SendByChannelToAll(short msgType, MessageBase msg, int channelId)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendByChannelToAll id:" + msgType);
			}
			ConnectionArray connections = instance.m_Connections;
			bool flag = true;
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections.Get(i);
				if (networkConnection != null)
				{
					flag &= networkConnection.SendByChannel(msgType, msg, channelId);
				}
			}
			return flag;
		}

		public static bool SendByChannelToReady(GameObject contextObj, short msgType, MessageBase msg, int channelId)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendByChannelToReady msgType:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = s_Instance.m_Connections.LocalIndex; i < s_Instance.m_Connections.Count; i++)
				{
					NetworkConnection networkConnection = s_Instance.m_Connections.Get(i);
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.SendByChannel(msgType, msg, channelId);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.SendByChannel(msgType, msg, channelId);
				}
			}
			return flag;
		}

		public static void DisconnectAll()
		{
			instance.InternalDisconnectAll();
		}

		internal void InternalDisconnectAll()
		{
			for (int i = m_Connections.LocalIndex; i < m_Connections.Count; i++)
			{
				NetworkConnection networkConnection = m_Connections.Get(i);
				if (networkConnection != null)
				{
					networkConnection.Disconnect();
					networkConnection.Dispose();
				}
			}
			s_Active = false;
			s_LocalClientActive = false;
		}

		internal static void Update()
		{
			if (s_Instance != null)
			{
				s_Instance.InternalUpdate();
			}
		}

		internal void UpdateServerObjects()
		{
			foreach (NetworkIdentity value in objects.Values)
			{
				try
				{
					value.UNetUpdate();
				}
				catch (NullReferenceException)
				{
				}
			}
			if (m_RemoveListCount++ % 100 == 0)
			{
				CheckForNullObjects();
			}
		}

		private void CheckForNullObjects()
		{
			foreach (NetworkInstanceId key in objects.Keys)
			{
				NetworkIdentity networkIdentity = objects[key];
				if (networkIdentity == null || networkIdentity.gameObject == null)
				{
					m_RemoveList.Add(key);
				}
			}
			if (m_RemoveList.Count > 0)
			{
				foreach (NetworkInstanceId remove in m_RemoveList)
				{
					objects.Remove(remove);
				}
				m_RemoveList.Clear();
			}
		}

		internal void InternalUpdate()
		{
			if (m_ServerId == -1 || !NetworkTransport.IsStarted)
			{
				return;
			}
			int num = 0;
			byte error;
			NetworkEventType networkEventType;
			if (m_RelaySlotId != -1)
			{
				networkEventType = NetworkTransport.ReceiveRelayEventFromHost(m_ServerId, out error);
				if (networkEventType != NetworkEventType.Nothing && LogFilter.logDebug)
				{
					Debug.Log("NetGroup event:" + networkEventType);
				}
				if (networkEventType == NetworkEventType.ConnectEvent && LogFilter.logDebug)
				{
					Debug.Log("NetGroup server connected");
				}
				if (networkEventType == NetworkEventType.DisconnectEvent && LogFilter.logDebug)
				{
					Debug.Log("NetGroup server disconnected");
				}
			}
			do
			{
				networkEventType = NetworkTransport.ReceiveFromHost(m_ServerId, out int connectionId, out int channelId, m_MsgBuffer, (ushort)m_MsgBuffer.Length, out int receivedSize, out error);
				if (networkEventType != NetworkEventType.Nothing && LogFilter.logDev)
				{
					Debug.Log("Server event: host=" + m_ServerId + " event=" + networkEventType + " error=" + error);
				}
				switch (networkEventType)
				{
				case NetworkEventType.ConnectEvent:
				{
					if (LogFilter.logDebug)
					{
						Debug.Log("Server accepted client:" + connectionId);
					}
					if (error != 0)
					{
						GenerateConnectError(error);
						return;
					}
					NetworkTransport.GetConnectionInfo(m_ServerId, connectionId, out string address, out int _, out NetworkID _, out NodeID _, out byte _);
					NetworkConnection networkConnection2 = (NetworkConnection)Activator.CreateInstance(s_NetworkConnectionClass);
					networkConnection2.SetHandlers(m_MessageHandlers);
					networkConnection2.Initialize(address, m_ServerId, connectionId, m_HostTopology);
					networkConnection2.SetMaxDelay(m_MaxDelay);
					m_Connections.Add(connectionId, networkConnection2);
					networkConnection2.InvokeHandlerNoData(32);
					if (m_SendPeerInfo)
					{
						SendNetworkInfo(networkConnection2);
					}
					SendCrc(networkConnection2);
					break;
				}
				case NetworkEventType.DataEvent:
				{
					NetworkConnection networkConnection = m_Connections.Get(connectionId);
					if (error != 0)
					{
						GenerateDataError(networkConnection, error);
						return;
					}
					if (networkConnection != null)
					{
						networkConnection.TransportRecieve(m_MsgBuffer, receivedSize, channelId);
					}
					else if (LogFilter.logError)
					{
						Debug.LogError("Unknown connection data event?!?");
					}
					break;
				}
				case NetworkEventType.DisconnectEvent:
				{
					NetworkConnection @unsafe = m_Connections.GetUnsafe(connectionId);
					if (error != 0 && error != 6)
					{
						GenerateDisconnectError(@unsafe, error);
					}
					m_Connections.Remove(connectionId);
					if (@unsafe != null)
					{
						@unsafe.InvokeHandlerNoData(33);
						for (int i = 0; i < @unsafe.playerControllers.Count; i++)
						{
							if (@unsafe.playerControllers[i].gameObject != null && LogFilter.logWarn)
							{
								Debug.LogWarning("Player not destroyed when connection disconnected.");
							}
						}
						if (LogFilter.logDebug)
						{
							Debug.Log("Server lost client:" + connectionId);
						}
						@unsafe.RemoveObservers();
						@unsafe.Dispose();
					}
					else if (LogFilter.logDebug)
					{
						Debug.Log("Connection is null in disconnect event");
					}
					if (m_SendPeerInfo)
					{
						SendNetworkInfo(@unsafe);
					}
					break;
				}
				default:
					if (LogFilter.logError)
					{
						Debug.LogError("Unknown network message type received: " + networkEventType);
					}
					break;
				case NetworkEventType.Nothing:
					break;
				}
				if (++num >= 500)
				{
					if (LogFilter.logDebug)
					{
						Debug.Log("kMaxEventsPerFrame hit (" + 500 + ")");
					}
					break;
				}
			}
			while (networkEventType != NetworkEventType.Nothing);
			UpdateServerObjects();
			for (int j = m_Connections.LocalIndex; j < m_Connections.Count; j++)
			{
				m_Connections.Get(j)?.FlushChannels();
			}
		}

		private void GenerateConnectError(int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Connect Error: " + error);
			}
			GenerateError(null, error);
		}

		private void GenerateDataError(NetworkConnection conn, int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Data Error: " + (NetworkError)error);
			}
			GenerateError(conn, error);
		}

		private void GenerateDisconnectError(NetworkConnection conn, int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Disconnect Error: " + (NetworkError)error + " conn:[" + conn + "]:" + conn.connectionId);
			}
			GenerateError(conn, error);
		}

		private void GenerateError(NetworkConnection conn, int error)
		{
			NetworkMessageDelegate handler = m_MessageHandlers.GetHandler(34);
			if (handler != null)
			{
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.errorCode = error;
				NetworkWriter writer = new NetworkWriter();
				errorMessage.Serialize(writer);
				NetworkReader reader = new NetworkReader(writer);
				conn.InvokeHandler(34, reader, 0);
			}
		}

		public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			instance.m_MessageHandlers.RegisterHandler(msgType, handler);
		}

		public static void UnregisterHandler(short msgType)
		{
			instance.m_MessageHandlers.UnregisterHandler(msgType);
		}

		public static void ClearHandlers()
		{
			instance.m_MessageHandlers.ClearMessageHandlers();
		}

		public static void ClearSpawners()
		{
			NetworkScene.ClearSpawners();
		}

		public static void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
		{
			numMsgs = 0;
			numBufferedMsgs = 0;
			numBytes = 0;
			lastBufferedPerSecond = 0;
			ConnectionArray connections = instance.m_Connections;
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections.Get(i);
				if (networkConnection != null)
				{
					networkConnection.GetStatsOut(out int numMsgs2, out int numBufferedMsgs2, out int numBytes2, out int lastBufferedPerSecond2);
					numMsgs += numMsgs2;
					numBufferedMsgs += numBufferedMsgs2;
					numBytes += numBytes2;
					lastBufferedPerSecond += lastBufferedPerSecond2;
				}
			}
		}

		public static void GetStatsIn(out int numMsgs, out int numBytes)
		{
			numMsgs = 0;
			numBytes = 0;
			ConnectionArray connections = instance.m_Connections;
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections.Get(i);
				if (networkConnection != null)
				{
					networkConnection.GetStatsIn(out int numMsgs2, out int numBytes2);
					numMsgs += numMsgs2;
					numBytes += numBytes2;
				}
			}
		}

		public static void SendToClientOfPlayer(GameObject player, short msgType, MessageBase msg)
		{
			ConnectionArray connections = instance.m_Connections;
			if (connections.ContainsPlayer(player, out NetworkConnection conn))
			{
				conn.Send(msgType, msg);
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Failed to send message to player object '" + player.name + ", not found in connection list");
			}
		}

		public static void SendToClient(int connectionId, short msgType, MessageBase msg)
		{
			ConnectionArray connections = instance.m_Connections;
			NetworkConnection networkConnection = connections.Get(connectionId);
			if (networkConnection != null)
			{
				networkConnection.Send(msgType, msg);
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Failed to send message to connection ID '" + connectionId + ", not found in connection list");
			}
		}

		public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
		{
			if (GetNetworkIdentity(player, out NetworkIdentity view))
			{
				view.SetDynamicAssetId(assetId);
			}
			return instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
		}

		public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
		{
			return instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
		}

		public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
		{
			if (GetNetworkIdentity(player, out NetworkIdentity view))
			{
				view.SetDynamicAssetId(assetId);
			}
			return instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
		}

		public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
		{
			return instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
		}

		internal bool InternalAddPlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
		{
			if (!GetNetworkIdentity(playerGameObject, out NetworkIdentity view))
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject);
				}
				return false;
			}
			if (!CheckPlayerControllerIdForConnection(conn, playerControllerId))
			{
				return false;
			}
			PlayerController playerController = null;
			GameObject x = null;
			if (conn.GetPlayerController(playerControllerId, out playerController))
			{
				x = playerController.gameObject;
			}
			if (x != null)
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: player object already exists for playerControllerId of " + playerControllerId);
				}
				return false;
			}
			PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
			conn.SetPlayerController(playerController2);
			view.SetConnectionToClient(conn, playerController2.playerControllerId);
			SetClientReady(conn);
			if (SetupLocalPlayerForConnection(conn, view, playerController2))
			{
				return true;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Adding new playerGameObject object netId: " + playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + playerGameObject.GetComponent<NetworkIdentity>().assetId);
			}
			FinishPlayerForConnection(conn, view, playerGameObject);
			if (view.localPlayerAuthority)
			{
				view.SetClientOwner(conn);
			}
			return true;
		}

		private static bool CheckPlayerControllerIdForConnection(NetworkConnection conn, short playerControllerId)
		{
			if (playerControllerId < 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AddPlayer: playerControllerId of " + playerControllerId + " is negative");
				}
				return false;
			}
			if (playerControllerId > 32)
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: playerControllerId of " + playerControllerId + " is too high. max is " + 32);
				}
				return false;
			}
			if (playerControllerId > 16 && LogFilter.logWarn)
			{
				Debug.LogWarning("AddPlayer: playerControllerId of " + playerControllerId + " is unusually high");
			}
			return true;
		}

		private bool SetupLocalPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, PlayerController newPlayerController)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer SetupLocalPlayerForConnection netID:" + uv.netId);
			}
			ULocalConnectionToClient uLocalConnectionToClient = conn as ULocalConnectionToClient;
			if (uLocalConnectionToClient != null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer AddPlayer handling ULocalConnectionToClient");
				}
				if (uv.netId.IsEmpty())
				{
					uv.OnStartServer();
				}
				uv.RebuildObservers(initialize: true);
				SendSpawnMessage(uv, null);
				uLocalConnectionToClient.localClient.AddLocalPlayer(newPlayerController);
				uv.SetLocalPlayer(newPlayerController.playerControllerId);
				return true;
			}
			return false;
		}

		private static void FinishPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, GameObject playerGameObject)
		{
			if (uv.netId.IsEmpty())
			{
				Spawn(playerGameObject);
			}
			OwnerMessage ownerMessage = new OwnerMessage();
			ownerMessage.netId = uv.netId;
			ownerMessage.playerControllerId = uv.playerControllerId;
			conn.Send(4, ownerMessage);
		}

		internal bool InternalReplacePlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
		{
			if (!GetNetworkIdentity(playerGameObject, out NetworkIdentity view))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReplacePlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject);
				}
				return false;
			}
			if (!CheckPlayerControllerIdForConnection(conn, playerControllerId))
			{
				return false;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer ReplacePlayer");
			}
			if (conn.GetPlayerController(playerControllerId, out PlayerController playerController))
			{
				playerController.unetView.SetNotLocalPlayer();
			}
			PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
			conn.SetPlayerController(playerController2);
			view.SetConnectionToClient(conn, playerController2.playerControllerId);
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer ReplacePlayer setup local");
			}
			if (SetupLocalPlayerForConnection(conn, view, playerController2))
			{
				return true;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Replacing playerGameObject object netId: " + playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + playerGameObject.GetComponent<NetworkIdentity>().assetId);
			}
			FinishPlayerForConnection(conn, view, playerGameObject);
			if (view.localPlayerAuthority)
			{
				view.SetClientOwner(conn);
			}
			return true;
		}

		private static bool GetNetworkIdentity(GameObject go, out NetworkIdentity view)
		{
			view = go.GetComponent<NetworkIdentity>();
			if (view == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("UNET failure. GameObject doesn't have NetworkIdentity.");
				}
				return false;
			}
			return true;
		}

		public static void SetClientReady(NetworkConnection conn)
		{
			instance.SetClientReadyInternal(conn);
		}

		internal void SetClientReadyInternal(NetworkConnection conn)
		{
			if (conn.isReady)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("SetClientReady conn " + conn.connectionId + " already ready");
				}
				return;
			}
			if (conn.playerControllers.Count == 0 && LogFilter.logDebug)
			{
				Debug.LogWarning("Ready with no player object");
			}
			conn.isReady = true;
			ULocalConnectionToClient uLocalConnectionToClient = conn as ULocalConnectionToClient;
			if (uLocalConnectionToClient != null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer Ready handling ULocalConnectionToClient");
				}
				foreach (NetworkIdentity value in objects.Values)
				{
					if (value != null && value.gameObject != null)
					{
						if (value.OnCheckObserver(conn))
						{
							value.AddObserver(conn);
						}
						if (!value.isClient)
						{
							if (LogFilter.logDev)
							{
								Debug.Log("LocalClient.SetSpawnObject calling OnStartClient");
							}
							value.OnStartClient();
						}
					}
				}
			}
			else
			{
				ObjectSpawnFinishedMessage objectSpawnFinishedMessage = new ObjectSpawnFinishedMessage();
				objectSpawnFinishedMessage.state = 0u;
				conn.Send(12, objectSpawnFinishedMessage);
				foreach (NetworkIdentity value2 in objects.Values)
				{
					if (value2 == null)
					{
						if (LogFilter.logWarn)
						{
							Debug.LogWarning("Invalid object found in server local object list (null NetworkIdentity).");
						}
					}
					else
					{
						if (LogFilter.logDebug)
						{
							Debug.Log("Sending spawn message for current server objects name='" + value2.gameObject.name + "' netId=" + value2.netId);
						}
						if (value2.OnCheckObserver(conn))
						{
							value2.AddObserver(conn);
						}
					}
				}
				objectSpawnFinishedMessage.state = 1u;
				conn.Send(12, objectSpawnFinishedMessage);
			}
		}

		internal static void ShowForConnection(NetworkIdentity uv, NetworkConnection conn)
		{
			if (conn.isReady)
			{
				instance.SendSpawnMessage(uv, conn);
			}
		}

		internal static void HideForConnection(NetworkIdentity uv, NetworkConnection conn)
		{
			ObjectDestroyMessage objectDestroyMessage = new ObjectDestroyMessage();
			objectDestroyMessage.netId = uv.netId;
			conn.Send(13, objectDestroyMessage);
		}

		public static void SetAllClientsNotReady()
		{
			ConnectionArray connections = instance.m_Connections;
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections.Get(i);
				if (networkConnection != null)
				{
					SetClientNotReady(networkConnection);
				}
			}
		}

		public static void SetClientNotReady(NetworkConnection conn)
		{
			instance.InternalSetClientNotReady(conn);
		}

		internal void InternalSetClientNotReady(NetworkConnection conn)
		{
			if (conn.isReady)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("PlayerNotReady " + conn);
				}
				conn.isReady = false;
				conn.RemoveObservers();
				NotReadyMessage msg = new NotReadyMessage();
				conn.Send(36, msg);
			}
		}

		private static void OnClientReadyMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("Default handler for ready message from " + netMsg.conn);
			}
			SetClientReady(netMsg.conn);
		}

		private static void OnRemovePlayerMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_RemovePlayerMessage);
			PlayerController playerController = null;
			netMsg.conn.GetPlayerController(s_RemovePlayerMessage.playerControllerId, out playerController);
			if (playerController != null)
			{
				netMsg.conn.RemovePlayerController(s_RemovePlayerMessage.playerControllerId);
				Destroy(playerController.gameObject);
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Received remove player message but could not find the player ID: " + s_RemovePlayerMessage.playerControllerId);
			}
		}

		private static void OnCommandMessage(NetworkMessage netMsg)
		{
			int cmdHash = (int)netMsg.reader.ReadPackedUInt32();
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			GameObject gameObject = FindLocalObject(networkInstanceId);
			if (gameObject == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Instance not found when handling Command message [netId=" + networkInstanceId + "]");
				}
				return;
			}
			NetworkIdentity component = gameObject.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkIdentity deleted when handling Command message [netId=" + networkInstanceId + "]");
				}
				return;
			}
			bool flag = false;
			foreach (PlayerController playerController in netMsg.conn.playerControllers)
			{
				if (playerController.gameObject != null && playerController.gameObject.GetComponent<NetworkIdentity>().netId == component.netId)
				{
					flag = true;
					break;
				}
			}
			if (!flag && component.clientAuthorityOwner != netMsg.conn)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Command for object without authority [netId=" + networkInstanceId + "]");
				}
				return;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("OnCommandMessage for netId=" + networkInstanceId + " conn=" + netMsg.conn);
			}
			component.HandleCommand(cmdHash, netMsg.reader);
		}

		internal void SpawnObject(GameObject obj)
		{
			if (!active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("SpawnObject for " + obj + ", NetworkServer is not active. Cannot spawn objects without an active server.");
				}
				return;
			}
			if (!GetNetworkIdentity(obj, out NetworkIdentity view))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("SpawnObject " + obj + " has no NetworkIdentity. Please add a NetworkIdentity to " + obj);
				}
				return;
			}
			view.OnStartServer();
			if (LogFilter.logDebug)
			{
				Debug.Log("SpawnObject instance ID " + view.netId + " asset ID " + view.assetId);
			}
			view.RebuildObservers(initialize: true);
		}

		internal void SendSpawnMessage(NetworkIdentity uv, NetworkConnection conn)
		{
			if (uv.serverOnly)
			{
				return;
			}
			if (uv.sceneId.IsEmpty())
			{
				ObjectSpawnMessage objectSpawnMessage = new ObjectSpawnMessage();
				objectSpawnMessage.netId = uv.netId;
				objectSpawnMessage.assetId = uv.assetId;
				objectSpawnMessage.position = uv.transform.position;
				NetworkWriter networkWriter = new NetworkWriter();
				uv.UNetSerializeAllVars(networkWriter);
				if (networkWriter.Position > 0)
				{
					objectSpawnMessage.payload = networkWriter.ToArray();
				}
				if (conn != null)
				{
					conn.Send(3, objectSpawnMessage);
				}
				else
				{
					SendToReady(uv.gameObject, 3, objectSpawnMessage);
				}
			}
			else
			{
				ObjectSpawnSceneMessage objectSpawnSceneMessage = new ObjectSpawnSceneMessage();
				objectSpawnSceneMessage.netId = uv.netId;
				objectSpawnSceneMessage.sceneId = uv.sceneId;
				objectSpawnSceneMessage.position = uv.transform.position;
				NetworkWriter networkWriter2 = new NetworkWriter();
				uv.UNetSerializeAllVars(networkWriter2);
				if (networkWriter2.Position > 0)
				{
					objectSpawnSceneMessage.payload = networkWriter2.ToArray();
				}
				if (conn != null)
				{
					conn.Send(10, objectSpawnSceneMessage);
				}
				else
				{
					SendToReady(uv.gameObject, 3, objectSpawnSceneMessage);
				}
			}
		}

		public static void DestroyPlayersForConnection(NetworkConnection conn)
		{
			if (conn.playerControllers.Count == 0)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Empty player list given to NetworkServer.Destroy(), nothing to do.");
				}
				return;
			}
			if (conn.clientOwnedObjects != null)
			{
				HashSet<NetworkInstanceId> hashSet = new HashSet<NetworkInstanceId>(conn.clientOwnedObjects);
				foreach (NetworkInstanceId item in hashSet)
				{
					GameObject gameObject = FindLocalObject(item);
					if (gameObject != null)
					{
						DestroyObject(gameObject);
					}
				}
			}
			foreach (PlayerController playerController in conn.playerControllers)
			{
				if (playerController.IsValid)
				{
					if (!(playerController.unetView == null))
					{
						DestroyObject(playerController.unetView, destroyServerObject: true);
					}
					playerController.gameObject = null;
				}
			}
			conn.playerControllers.Clear();
		}

		private static void UnSpawnObject(GameObject obj)
		{
			NetworkIdentity view;
			if (obj == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer UnspawnObject is null");
				}
			}
			else if (GetNetworkIdentity(obj, out view))
			{
				UnSpawnObject(view);
			}
		}

		private static void UnSpawnObject(NetworkIdentity uv)
		{
			DestroyObject(uv, destroyServerObject: false);
		}

		private static void DestroyObject(GameObject obj)
		{
			NetworkIdentity view;
			if (obj == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer DestroyObject is null");
				}
			}
			else if (GetNetworkIdentity(obj, out view))
			{
				DestroyObject(view, destroyServerObject: true);
			}
		}

		private static void DestroyObject(NetworkIdentity uv, bool destroyServerObject)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("DestroyObject instance:" + uv.netId);
			}
			if (objects.ContainsKey(uv.netId))
			{
				objects.Remove(uv.netId);
			}
			if (uv.clientAuthorityOwner != null)
			{
				uv.clientAuthorityOwner.RemoveOwnedObject(uv);
			}
			ObjectDestroyMessage objectDestroyMessage = new ObjectDestroyMessage();
			objectDestroyMessage.netId = uv.netId;
			SendToObservers(uv.gameObject, 1, objectDestroyMessage);
			uv.ClearObservers();
			if (NetworkClient.active && s_LocalClientActive)
			{
				uv.OnNetworkDestroy();
				ClientScene.SetLocalObject(objectDestroyMessage.netId, null);
			}
			if (destroyServerObject)
			{
				Object.Destroy(uv.gameObject);
			}
			uv.SetNoServer();
		}

		public static void ClearLocalObjects()
		{
			objects.Clear();
		}

		public static void Spawn(GameObject obj)
		{
			instance.SpawnObject(obj);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, GameObject player)
		{
			NetworkIdentity component = player.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				Debug.LogError("SpawnWithClientAuthority player object has no NetworkIdentity");
				return false;
			}
			if (component.connectionToClient == null)
			{
				Debug.LogError("SpawnWithClientAuthority player object is not a player.");
				return false;
			}
			return SpawnWithClientAuthority(obj, component.connectionToClient);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, NetworkConnection conn)
		{
			Spawn(obj);
			NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
			if (component == null || !component.isServer)
			{
				return false;
			}
			return component.AssignClientAuthority(conn);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, NetworkHash128 assetId, NetworkConnection conn)
		{
			Spawn(obj, assetId);
			NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
			if (component == null || !component.isServer)
			{
				return false;
			}
			return component.AssignClientAuthority(conn);
		}

		public static void Spawn(GameObject obj, NetworkHash128 assetId)
		{
			if (GetNetworkIdentity(obj, out NetworkIdentity view))
			{
				view.SetDynamicAssetId(assetId);
			}
			instance.SpawnObject(obj);
		}

		public static void Destroy(GameObject obj)
		{
			DestroyObject(obj);
		}

		public static void UnSpawn(GameObject obj)
		{
			UnSpawnObject(obj);
		}

		internal bool InvokeBytes(ULocalConnectionToServer conn, byte[] buffer, int numBytes, int channelId)
		{
			NetworkReader networkReader = new NetworkReader(buffer);
			networkReader.ReadInt16();
			short msgType = networkReader.ReadInt16();
			NetworkMessageDelegate handler = m_MessageHandlers.GetHandler(msgType);
			if (handler != null)
			{
				NetworkConnection networkConnection = m_Connections.Get(conn.connectionId);
				if (networkConnection != null)
				{
					ULocalConnectionToClient uLocalConnectionToClient = (ULocalConnectionToClient)networkConnection;
					uLocalConnectionToClient.InvokeHandler(msgType, networkReader, channelId);
					return true;
				}
			}
			return false;
		}

		internal bool InvokeHandlerOnServer(ULocalConnectionToServer conn, short msgType, MessageBase msg, int channelId)
		{
			NetworkMessageDelegate handler = m_MessageHandlers.GetHandler(msgType);
			if (handler != null)
			{
				NetworkConnection networkConnection = m_Connections.Get(conn.connectionId);
				if (networkConnection != null)
				{
					ULocalConnectionToClient uLocalConnectionToClient = (ULocalConnectionToClient)networkConnection;
					NetworkWriter writer = new NetworkWriter();
					msg.Serialize(writer);
					NetworkReader reader = new NetworkReader(writer);
					uLocalConnectionToClient.InvokeHandler(msgType, reader, channelId);
					return true;
				}
				if (LogFilter.logError)
				{
					Debug.LogError("Local invoke: Failed to find local connection to invoke handler on [connectionId=" + conn.connectionId + "]");
				}
				return false;
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Local invoke: Failed to find message handler for message ID " + msgType);
			}
			return false;
		}

		public static GameObject FindLocalObject(NetworkInstanceId netId)
		{
			return s_NetworkScene.FindLocalObject(netId);
		}

		public static Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
		{
			ConnectionArray connections = instance.m_Connections;
			Dictionary<short, NetworkConnection.PacketStat> dictionary = new Dictionary<short, NetworkConnection.PacketStat>();
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections.Get(i);
				if (networkConnection != null)
				{
					foreach (short key in networkConnection.packetStats.Keys)
					{
						if (dictionary.ContainsKey(key))
						{
							NetworkConnection.PacketStat packetStat = dictionary[key];
							packetStat.count += networkConnection.packetStats[key].count;
							packetStat.bytes += networkConnection.packetStats[key].bytes;
							dictionary[key] = packetStat;
						}
						else
						{
							dictionary[key] = networkConnection.packetStats[key];
						}
					}
				}
			}
			return dictionary;
		}

		public static void ResetConnectionStats()
		{
			ConnectionArray connections = instance.m_Connections;
			for (int i = connections.LocalIndex; i < connections.Count; i++)
			{
				connections.Get(i)?.ResetStats();
			}
		}

		public static bool AddExternalConnection(NetworkConnection conn)
		{
			return instance.AddExternalConnectionInternal(conn);
		}

		private bool AddExternalConnectionInternal(NetworkConnection conn)
		{
			if (m_Connections.Get(conn.connectionId) != null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AddExternalConnection failed, already connection for id:" + conn.connectionId);
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("AddExternalConnection external connection " + conn.connectionId);
			}
			m_Connections.Add(conn.connectionId, conn);
			conn.SetHandlers(m_MessageHandlers);
			s_ExternalConnections.Add(conn.connectionId);
			return true;
		}

		public static void RemoveExternalConnection(int connectionId)
		{
			instance.RemoveExternalConnectionInternal(connectionId);
		}

		private bool RemoveExternalConnectionInternal(int connectionId)
		{
			if (!s_ExternalConnections.Contains(connectionId))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveExternalConnection failed, no connection for id:" + connectionId);
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("RemoveExternalConnection external connection " + connectionId);
			}
			m_Connections.Remove(connectionId);
			return true;
		}

		public static bool SpawnObjects()
		{
			if (active)
			{
				NetworkIdentity[] array = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
				NetworkIdentity[] array2 = array;
				foreach (NetworkIdentity networkIdentity in array2)
				{
					if (networkIdentity.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity.gameObject.hideFlags != HideFlags.HideAndDontSave && !networkIdentity.sceneId.IsEmpty())
					{
						if (LogFilter.logDebug)
						{
							Debug.Log("SpawnObjects sceneId:" + networkIdentity.sceneId + " name:" + networkIdentity.gameObject.name);
						}
						networkIdentity.gameObject.SetActive(value: true);
					}
				}
				NetworkIdentity[] array3 = array;
				foreach (NetworkIdentity networkIdentity2 in array3)
				{
					if (networkIdentity2.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity2.gameObject.hideFlags != HideFlags.HideAndDontSave && !networkIdentity2.sceneId.IsEmpty() && !networkIdentity2.isServer && !(networkIdentity2.gameObject == null))
					{
						Spawn(networkIdentity2.gameObject);
						networkIdentity2.ForceAuthority(authority: true);
					}
				}
			}
			return true;
		}

		private static void SendCrc(NetworkConnection targetConnection)
		{
			if (NetworkCRC.singleton != null && NetworkCRC.scriptCRCCheck)
			{
				CRCMessage cRCMessage = new CRCMessage();
				List<CRCMessageEntry> list = new List<CRCMessageEntry>();
				foreach (string key in NetworkCRC.singleton.scripts.Keys)
				{
					CRCMessageEntry item = default(CRCMessageEntry);
					item.name = key;
					item.channel = (byte)NetworkCRC.singleton.scripts[key];
					list.Add(item);
				}
				cRCMessage.scripts = list.ToArray();
				targetConnection.Send(14, cRCMessage);
			}
		}

		public void SendNetworkInfo(NetworkConnection targetConnection)
		{
			PeerListMessage peerListMessage = new PeerListMessage();
			List<PeerInfoMessage> list = new List<PeerInfoMessage>();
			for (int i = 0; i < m_Connections.Count; i++)
			{
				NetworkConnection networkConnection = m_Connections.Get(i);
				if (networkConnection != null)
				{
					PeerInfoMessage peerInfoMessage = new PeerInfoMessage();
					NetworkTransport.GetConnectionInfo(m_ServerId, networkConnection.connectionId, out string address, out int port, out NetworkID _, out NodeID _, out byte _);
					peerInfoMessage.connectionId = networkConnection.connectionId;
					peerInfoMessage.address = address;
					peerInfoMessage.port = port;
					peerInfoMessage.isHost = false;
					peerInfoMessage.isYou = (networkConnection == targetConnection);
					list.Add(peerInfoMessage);
				}
			}
			if (localClientActive)
			{
				PeerInfoMessage peerInfoMessage2 = new PeerInfoMessage();
				peerInfoMessage2.address = "HOST";
				peerInfoMessage2.port = m_ServerPort;
				peerInfoMessage2.connectionId = 0;
				peerInfoMessage2.isHost = true;
				peerInfoMessage2.isYou = false;
				list.Add(peerInfoMessage2);
			}
			peerListMessage.peers = list.ToArray();
			for (int j = 0; j < m_Connections.Count; j++)
			{
				m_Connections.Get(j)?.Send(11, peerListMessage);
			}
		}
	}
}
