using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	public class NetworkClient
	{
		protected enum ConnectState
		{
			None,
			Resolving,
			Resolved,
			Connecting,
			Connected,
			Disconnected,
			Failed
		}

		private const int k_MaxEventsPerFrame = 500;

		private Type m_NetworkConnectionClass = typeof(NetworkConnection);

		private static List<NetworkClient> s_Clients = new List<NetworkClient>();

		private static bool s_IsActive;

		private HostTopology m_HostTopology;

		private bool m_UseSimulator;

		private int m_SimulatedLatency;

		private float m_PacketLoss;

		private string m_ServerIp = string.Empty;

		private int m_ServerPort;

		private int m_ClientId = -1;

		private int m_ClientConnectionId = -1;

		private int m_StatResetTime;

		private EndPoint m_RemoteEndPoint;

		private static PeerListMessage s_PeerListMessage = new PeerListMessage();

		private static CRCMessage s_CRCMessage = new CRCMessage();

		private NetworkMessageHandlers m_MessageHandlers = new NetworkMessageHandlers();

		protected NetworkConnection m_Connection;

		private byte[] m_MsgBuffer;

		private NetworkReader m_MsgReader;

		private PeerInfoMessage[] m_Peers;

		protected ConnectState m_AsyncConnect;

		private string m_RequestedServerHost = string.Empty;

		public static List<NetworkClient> allClients => s_Clients;

		public static bool active => s_IsActive;

		public string serverIp => m_ServerIp;

		public int serverPort => m_ServerPort;

		public NetworkConnection connection => m_Connection;

		public PeerInfoMessage[] peers => m_Peers;

		public Dictionary<short, NetworkMessageDelegate> handlers => m_MessageHandlers.GetHandlers();

		public int numChannels => m_HostTopology.DefaultConfig.ChannelCount;

		public HostTopology hostTopology => m_HostTopology;

		public bool isConnected => m_AsyncConnect == ConnectState.Connected;

		public Type networkConnectionClass => m_NetworkConnectionClass;

		public NetworkClient()
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Client created version " + Version.Current);
			}
			m_MsgBuffer = new byte[49152];
			m_MsgReader = new NetworkReader(m_MsgBuffer);
			AddClient(this);
		}

		internal void SetHandlers(NetworkConnection conn)
		{
			conn.SetHandlers(m_MessageHandlers);
		}

		public void SetNetworkConnectionClass<T>() where T : NetworkConnection
		{
			m_NetworkConnectionClass = typeof(T);
		}

		public bool Configure(ConnectionConfig config, int maxConnections)
		{
			HostTopology topology = new HostTopology(config, maxConnections);
			return Configure(topology);
		}

		public bool Configure(HostTopology topology)
		{
			m_HostTopology = topology;
			return true;
		}

		public void Connect(MatchInfo matchInfo)
		{
			PrepareForConnect();
			ConnectWithRelay(matchInfo);
		}

		public void ConnectWithSimulator(string serverIp, int serverPort, int latency, float packetLoss)
		{
			m_UseSimulator = true;
			m_SimulatedLatency = latency;
			m_PacketLoss = packetLoss;
			Connect(serverIp, serverPort);
		}

		private static bool IsValidIpV6(string address)
		{
			foreach (char c in address)
			{
				switch (c)
				{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case ':':
					continue;
				}
				if ((c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
				{
					continue;
				}
				return false;
			}
			return true;
		}

		public void Connect(string serverIp, int serverPort)
		{
			PrepareForConnect();
			if (LogFilter.logDebug)
			{
				Debug.Log("Client Connect: " + serverIp + ":" + serverPort);
			}
			m_ServerPort = serverPort;
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				m_ServerIp = serverIp;
				m_AsyncConnect = ConnectState.Resolved;
				return;
			}
			if (serverIp.Equals("127.0.0.1") || serverIp.Equals("localhost"))
			{
				m_ServerIp = "127.0.0.1";
				m_AsyncConnect = ConnectState.Resolved;
				return;
			}
			if (serverIp.IndexOf(":") != -1 && IsValidIpV6(serverIp))
			{
				m_ServerIp = serverIp;
				m_AsyncConnect = ConnectState.Resolved;
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Async DNS START:" + serverIp);
			}
			m_RequestedServerHost = serverIp;
			m_AsyncConnect = ConnectState.Resolving;
			Dns.BeginGetHostAddresses(serverIp, GetHostAddressesCallback, this);
		}

		public void Connect(EndPoint secureTunnelEndPoint)
		{
			PrepareForConnect();
			if (LogFilter.logDebug)
			{
				Debug.Log("Client Connect to remoteSockAddr");
			}
			if (secureTunnelEndPoint == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Connect failed: null endpoint passed in");
				}
				m_AsyncConnect = ConnectState.Failed;
				return;
			}
			if (secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetwork && secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetworkV6)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Connect failed: Endpoint AddressFamily must be either InterNetwork or InterNetworkV6");
				}
				m_AsyncConnect = ConnectState.Failed;
				return;
			}
			string fullName = secureTunnelEndPoint.GetType().FullName;
			if (fullName == "System.Net.IPEndPoint")
			{
				IPEndPoint iPEndPoint = (IPEndPoint)secureTunnelEndPoint;
				Connect(iPEndPoint.Address.ToString(), iPEndPoint.Port);
				return;
			}
			if (fullName != "UnityEngine.XboxOne.XboxOneEndPoint")
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Connect failed: invalid Endpoint (not IPEndPoint or XboxOneEndPoint)");
				}
				m_AsyncConnect = ConnectState.Failed;
				return;
			}
			byte error = 0;
			m_RemoteEndPoint = secureTunnelEndPoint;
			m_AsyncConnect = ConnectState.Connecting;
			try
			{
				m_ClientConnectionId = NetworkTransport.ConnectEndPoint(m_ClientId, m_RemoteEndPoint, 0, out error);
			}
			catch (Exception arg)
			{
				Debug.LogError("Connect failed: Exception when trying to connect to EndPoint: " + arg);
			}
			if (m_ClientConnectionId == 0 && LogFilter.logError)
			{
				Debug.LogError("Connect failed: Unable to connect to EndPoint (" + error + ")");
			}
			m_Connection = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
			m_Connection.SetHandlers(m_MessageHandlers);
			m_Connection.Initialize(m_ServerIp, m_ClientId, m_ClientConnectionId, m_HostTopology);
		}

		private void PrepareForConnect()
		{
			SetActive(state: true);
			RegisterSystemHandlers(localClient: false);
			if (m_HostTopology == null)
			{
				ConnectionConfig connectionConfig = new ConnectionConfig();
				connectionConfig.AddChannel(QosType.Reliable);
				connectionConfig.AddChannel(QosType.Unreliable);
				m_HostTopology = new HostTopology(connectionConfig, 8);
			}
			if (m_UseSimulator)
			{
				int num = m_SimulatedLatency / 3 - 1;
				if (num < 1)
				{
					num = 1;
				}
				int num2 = m_SimulatedLatency * 3;
				if (LogFilter.logDebug)
				{
					Debug.Log("AddHost Using Simulator " + num + "/" + num2);
				}
				m_ClientId = NetworkTransport.AddHostWithSimulator(m_HostTopology, num, num2, 0);
			}
			else
			{
				m_ClientId = NetworkTransport.AddHost(m_HostTopology, 0);
			}
		}

		internal static void GetHostAddressesCallback(IAsyncResult ar)
		{
			try
			{
				IPAddress[] array = Dns.EndGetHostAddresses(ar);
				NetworkClient networkClient = (NetworkClient)ar.AsyncState;
				if (array.Length == 0)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("DNS lookup failed for:" + networkClient.m_RequestedServerHost);
					}
					networkClient.m_AsyncConnect = ConnectState.Failed;
				}
				else
				{
					networkClient.m_ServerIp = array[0].ToString();
					networkClient.m_AsyncConnect = ConnectState.Resolved;
					if (LogFilter.logDebug)
					{
						Debug.Log("Async DNS Result:" + networkClient.m_ServerIp + " for " + networkClient.m_RequestedServerHost + ": " + networkClient.m_ServerIp);
					}
				}
			}
			catch (SocketException ex)
			{
				NetworkClient networkClient2 = (NetworkClient)ar.AsyncState;
				if (LogFilter.logError)
				{
					Debug.LogError("DNS resolution failed: " + ex.ErrorCode);
				}
				if (LogFilter.logDebug)
				{
					Debug.Log("Exception:" + ex);
				}
				networkClient2.m_AsyncConnect = ConnectState.Failed;
			}
		}

		internal void ContinueConnect()
		{
			byte error;
			if (m_UseSimulator)
			{
				int num = m_SimulatedLatency / 3;
				if (num < 1)
				{
					num = 1;
				}
				if (LogFilter.logDebug)
				{
					Debug.Log("Connect Using Simulator " + m_SimulatedLatency / 3 + "/" + m_SimulatedLatency);
				}
				ConnectionSimulatorConfig conf = new ConnectionSimulatorConfig(num, m_SimulatedLatency, num, m_SimulatedLatency, m_PacketLoss);
				m_ClientConnectionId = NetworkTransport.ConnectWithSimulator(m_ClientId, m_ServerIp, m_ServerPort, 0, out error, conf);
			}
			else
			{
				m_ClientConnectionId = NetworkTransport.Connect(m_ClientId, m_ServerIp, m_ServerPort, 0, out error);
			}
			m_Connection = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
			m_Connection.SetHandlers(m_MessageHandlers);
			m_Connection.Initialize(m_ServerIp, m_ClientId, m_ClientConnectionId, m_HostTopology);
		}

		private void ConnectWithRelay(MatchInfo info)
		{
			m_AsyncConnect = ConnectState.Connecting;
			Update();
			m_ClientConnectionId = NetworkTransport.ConnectToNetworkPeer(m_ClientId, info.address, info.port, 0, 0, info.networkId, Utility.GetSourceID(), info.nodeId, out byte error);
			m_Connection = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
			m_Connection.SetHandlers(m_MessageHandlers);
			m_Connection.Initialize(info.address, m_ClientId, m_ClientConnectionId, m_HostTopology);
			if (error != 0)
			{
				Debug.LogError("ConnectToNetworkPeer Error: " + error);
			}
		}

		public virtual void Disconnect()
		{
			m_AsyncConnect = ConnectState.Disconnected;
			ClientScene.HandleClientDisconnect(m_Connection);
			if (m_Connection != null)
			{
				m_Connection.Disconnect();
				m_Connection.Dispose();
				m_Connection = null;
				NetworkTransport.RemoveHost(m_ClientId);
			}
		}

		public bool Send(short msgType, MessageBase msg)
		{
			if (m_Connection != null)
			{
				if (m_AsyncConnect != ConnectState.Connected)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkClient Send when not connected to a server");
					}
					return false;
				}
				return m_Connection.Send(msgType, msg);
			}
			if (LogFilter.logError)
			{
				Debug.LogError("NetworkClient Send with no connection");
			}
			return false;
		}

		public bool SendWriter(NetworkWriter writer, int channelId)
		{
			if (m_Connection != null)
			{
				if (m_AsyncConnect != ConnectState.Connected)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkClient SendWriter when not connected to a server");
					}
					return false;
				}
				return m_Connection.SendWriter(writer, channelId);
			}
			if (LogFilter.logError)
			{
				Debug.LogError("NetworkClient SendWriter with no connection");
			}
			return false;
		}

		public bool SendBytes(byte[] data, int numBytes, int channelId)
		{
			if (m_Connection != null)
			{
				if (m_AsyncConnect != ConnectState.Connected)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkClient SendBytes when not connected to a server");
					}
					return false;
				}
				return m_Connection.SendBytes(data, numBytes, channelId);
			}
			if (LogFilter.logError)
			{
				Debug.LogError("NetworkClient SendBytes with no connection");
			}
			return false;
		}

		public bool SendUnreliable(short msgType, MessageBase msg)
		{
			if (m_Connection != null)
			{
				if (m_AsyncConnect != ConnectState.Connected)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkClient SendUnreliable when not connected to a server");
					}
					return false;
				}
				return m_Connection.SendUnreliable(msgType, msg);
			}
			if (LogFilter.logError)
			{
				Debug.LogError("NetworkClient SendUnreliable with no connection");
			}
			return false;
		}

		public bool SendByChannel(short msgType, MessageBase msg, int channelId)
		{
			if (m_Connection != null)
			{
				if (m_AsyncConnect != ConnectState.Connected)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkClient SendByChannel when not connected to a server");
					}
					return false;
				}
				return m_Connection.SendByChannel(msgType, msg, channelId);
			}
			if (LogFilter.logError)
			{
				Debug.LogError("NetworkClient SendByChannel with no connection");
			}
			return false;
		}

		public void SetMaxDelay(float seconds)
		{
			if (m_Connection == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SetMaxDelay failed, not connected.");
				}
			}
			else
			{
				m_Connection.SetMaxDelay(seconds);
			}
		}

		public void Shutdown()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("Shutting down client " + m_ClientId);
			}
			m_ClientId = -1;
			RemoveClient(this);
			if (s_Clients.Count == 0)
			{
				SetActive(state: false);
			}
		}

		internal virtual void Update()
		{
			if (m_ClientId == -1)
			{
				return;
			}
			switch (m_AsyncConnect)
			{
			case ConnectState.None:
			case ConnectState.Resolving:
			case ConnectState.Disconnected:
				return;
			case ConnectState.Failed:
				GenerateConnectError(11);
				m_AsyncConnect = ConnectState.Disconnected;
				return;
			case ConnectState.Resolved:
				m_AsyncConnect = ConnectState.Connecting;
				ContinueConnect();
				return;
			}
			if (m_Connection != null && (int)Time.time != m_StatResetTime)
			{
				m_Connection.ResetStats();
				m_StatResetTime = (int)Time.time;
			}
			NetworkEventType networkEventType;
			do
			{
				int num = 0;
				networkEventType = NetworkTransport.ReceiveFromHost(m_ClientId, out int _, out int channelId, m_MsgBuffer, (ushort)m_MsgBuffer.Length, out int receivedSize, out byte error);
				if (networkEventType != NetworkEventType.Nothing && LogFilter.logDev)
				{
					Debug.Log("Client event: host=" + m_ClientId + " event=" + networkEventType + " error=" + error);
				}
				switch (networkEventType)
				{
				case NetworkEventType.ConnectEvent:
					if (LogFilter.logDebug)
					{
						Debug.Log("Client connected");
					}
					if (error != 0)
					{
						GenerateConnectError(error);
						return;
					}
					m_AsyncConnect = ConnectState.Connected;
					m_Connection.InvokeHandlerNoData(32);
					break;
				case NetworkEventType.DataEvent:
					if (error != 0)
					{
						GenerateDataError(error);
						return;
					}
					m_MsgReader.SeekZero();
					m_Connection.TransportRecieve(m_MsgBuffer, receivedSize, channelId);
					break;
				case NetworkEventType.DisconnectEvent:
					if (LogFilter.logDebug)
					{
						Debug.Log("Client disconnected");
					}
					m_AsyncConnect = ConnectState.Disconnected;
					if (error != 0)
					{
						GenerateDisconnectError(error);
					}
					ClientScene.HandleClientDisconnect(m_Connection);
					m_Connection.InvokeHandlerNoData(33);
					break;
				default:
					if (LogFilter.logError)
					{
						Debug.LogError("Unknown network message type received: " + networkEventType);
					}
					break;
				case NetworkEventType.Nothing:
					break;
				}
				if (num + 1 >= 500)
				{
					if (LogFilter.logDebug)
					{
						Debug.Log("MaxEventsPerFrame hit (" + 500 + ")");
					}
					break;
				}
			}
			while (m_ClientId != -1 && networkEventType != NetworkEventType.Nothing);
			if (m_Connection != null && m_AsyncConnect == ConnectState.Connected)
			{
				m_Connection.FlushChannels();
			}
		}

		private void GenerateConnectError(int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Client Error Connect Error: " + error);
			}
			GenerateError(error);
		}

		private void GenerateDataError(int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Client Data Error: " + (NetworkError)error);
			}
			GenerateError(error);
		}

		private void GenerateDisconnectError(int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Client Disconnect Error: " + (NetworkError)error);
			}
			GenerateError(error);
		}

		private void GenerateError(int error)
		{
			NetworkMessageDelegate handler = m_MessageHandlers.GetHandler(34);
			if (handler == null)
			{
				handler = m_MessageHandlers.GetHandler(34);
			}
			if (handler != null)
			{
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.errorCode = error;
				byte[] buffer = new byte[200];
				NetworkWriter writer = new NetworkWriter(buffer);
				errorMessage.Serialize(writer);
				NetworkReader reader = new NetworkReader(buffer);
				NetworkMessage networkMessage = new NetworkMessage();
				networkMessage.msgType = 34;
				networkMessage.reader = reader;
				networkMessage.conn = m_Connection;
				networkMessage.channelId = 0;
				handler(networkMessage);
			}
		}

		public void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
		{
			numMsgs = 0;
			numBufferedMsgs = 0;
			numBytes = 0;
			lastBufferedPerSecond = 0;
			if (m_Connection != null)
			{
				m_Connection.GetStatsOut(out numMsgs, out numBufferedMsgs, out numBytes, out lastBufferedPerSecond);
			}
		}

		public void GetStatsIn(out int numMsgs, out int numBytes)
		{
			numMsgs = 0;
			numBytes = 0;
			if (m_Connection != null)
			{
				m_Connection.GetStatsIn(out numMsgs, out numBytes);
			}
		}

		public Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
		{
			if (m_Connection == null)
			{
				return null;
			}
			return m_Connection.packetStats;
		}

		public void ResetConnectionStats()
		{
			if (m_Connection != null)
			{
				m_Connection.ResetStats();
			}
		}

		public int GetRTT()
		{
			if (m_ClientId == -1)
			{
				return 0;
			}
			byte error;
			return NetworkTransport.GetCurrentRtt(m_ClientId, m_ClientConnectionId, out error);
		}

		internal void RegisterSystemHandlers(bool localClient)
		{
			RegisterHandlerSafe(11, OnPeerInfo);
			ClientScene.RegisterSystemHandlers(this, localClient);
			RegisterHandlerSafe(14, OnCRC);
		}

		private void OnPeerInfo(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("OnPeerInfo");
			}
			netMsg.ReadMessage(s_PeerListMessage);
			m_Peers = s_PeerListMessage.peers;
		}

		private void OnCRC(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_CRCMessage);
			NetworkCRC.Validate(s_CRCMessage.scripts, numChannels);
		}

		public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			m_MessageHandlers.RegisterHandler(msgType, handler);
		}

		public void RegisterHandlerSafe(short msgType, NetworkMessageDelegate handler)
		{
			m_MessageHandlers.RegisterHandlerSafe(msgType, handler);
		}

		public void UnregisterHandler(short msgType)
		{
			m_MessageHandlers.UnregisterHandler(msgType);
		}

		public static Dictionary<short, NetworkConnection.PacketStat> GetTotalConnectionStats()
		{
			Dictionary<short, NetworkConnection.PacketStat> dictionary = new Dictionary<short, NetworkConnection.PacketStat>();
			foreach (NetworkClient s_Client in s_Clients)
			{
				Dictionary<short, NetworkConnection.PacketStat> connectionStats = s_Client.GetConnectionStats();
				foreach (short key in connectionStats.Keys)
				{
					if (dictionary.ContainsKey(key))
					{
						NetworkConnection.PacketStat packetStat = dictionary[key];
						packetStat.count += connectionStats[key].count;
						packetStat.bytes += connectionStats[key].bytes;
						dictionary[key] = packetStat;
					}
					else
					{
						dictionary[key] = connectionStats[key];
					}
				}
			}
			return dictionary;
		}

		internal static void AddClient(NetworkClient client)
		{
			s_Clients.Add(client);
		}

		internal static void RemoveClient(NetworkClient client)
		{
			s_Clients.Remove(client);
		}

		internal static void UpdateClients()
		{
			for (int i = 0; i < s_Clients.Count; i++)
			{
				if (s_Clients[i] != null)
				{
					s_Clients[i].Update();
				}
				else
				{
					s_Clients.RemoveAt(i);
				}
			}
		}

		public static void ShutdownAll()
		{
			while (s_Clients.Count != 0)
			{
				s_Clients[0].Shutdown();
			}
			s_Clients = new List<NetworkClient>();
			s_IsActive = false;
			ClientScene.Shutdown();
		}

		internal static void SetActive(bool state)
		{
			if (!s_IsActive && state)
			{
				NetworkTransport.Init();
			}
			s_IsActive = state;
		}
	}
}
