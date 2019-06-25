using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Networking
{
	public class NetworkConnection : IDisposable
	{
		public class PacketStat
		{
			public short msgType;

			public int count;

			public int bytes;

			public override string ToString()
			{
				return MsgType.MsgTypeToString(msgType) + ": count=" + count + " bytes=" + bytes;
			}
		}

		private const int k_MaxMessageLogSize = 150;

		private ChannelBuffer[] m_Channels;

		private List<PlayerController> m_PlayerControllers = new List<PlayerController>();

		private NetworkMessage m_NetMsg = new NetworkMessage();

		private HashSet<NetworkIdentity> m_VisList = new HashSet<NetworkIdentity>();

		private NetworkWriter m_Writer;

		private Dictionary<short, NetworkMessageDelegate> m_MessageHandlersDict;

		private NetworkMessageHandlers m_MessageHandlers;

		private HashSet<NetworkInstanceId> m_ClientOwnedObjects;

		private NetworkMessage m_MessageInfo = new NetworkMessage();

		public int hostId = -1;

		public int connectionId = -1;

		public bool isReady;

		public string address;

		public float lastMessageTime;

		public bool logNetworkMessages;

		private Dictionary<short, PacketStat> m_PacketStats = new Dictionary<short, PacketStat>();

		private bool m_Disposed;

		internal HashSet<NetworkIdentity> visList => m_VisList;

		public List<PlayerController> playerControllers => m_PlayerControllers;

		public HashSet<NetworkInstanceId> clientOwnedObjects => m_ClientOwnedObjects;

		internal Dictionary<short, PacketStat> packetStats => m_PacketStats;

		public NetworkConnection()
		{
			m_Writer = new NetworkWriter();
		}

		public virtual void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
		{
			m_Writer = new NetworkWriter();
			address = networkAddress;
			hostId = networkHostId;
			connectionId = networkConnectionId;
			int channelCount = hostTopology.DefaultConfig.ChannelCount;
			int packetSize = hostTopology.DefaultConfig.PacketSize;
			m_Channels = new ChannelBuffer[channelCount];
			for (int i = 0; i < channelCount; i++)
			{
				ChannelQOS channelQOS = hostTopology.DefaultConfig.Channels[i];
				int bufferSize = packetSize;
				if (channelQOS.QOS == QosType.ReliableFragmented || channelQOS.QOS == QosType.UnreliableFragmented)
				{
					bufferSize = hostTopology.DefaultConfig.FragmentSize * 128;
				}
				m_Channels[i] = new ChannelBuffer(this, bufferSize, (byte)i, IsReliableQoS(channelQOS.QOS));
			}
		}

		~NetworkConnection()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!m_Disposed && m_Channels != null)
			{
				for (int i = 0; i < m_Channels.Length; i++)
				{
					m_Channels[i].Dispose();
				}
			}
			m_Channels = null;
			if (m_ClientOwnedObjects != null)
			{
				foreach (NetworkInstanceId clientOwnedObject in m_ClientOwnedObjects)
				{
					GameObject gameObject = NetworkServer.FindLocalObject(clientOwnedObject);
					if (gameObject != null)
					{
						gameObject.GetComponent<NetworkIdentity>().ClearClientOwner();
					}
				}
			}
			m_ClientOwnedObjects = null;
			m_Disposed = true;
		}

		private static bool IsReliableQoS(QosType qos)
		{
			return qos == QosType.Reliable || qos == QosType.ReliableFragmented || qos == QosType.ReliableSequenced || qos == QosType.ReliableStateUpdate;
		}

		public bool SetChannelOption(int channelId, ChannelOption option, int value)
		{
			if (m_Channels == null)
			{
				return false;
			}
			if (channelId < 0 || channelId >= m_Channels.Length)
			{
				return false;
			}
			return m_Channels[channelId].SetOption(option, value);
		}

		public void Disconnect()
		{
			address = string.Empty;
			isReady = false;
			ClientScene.HandleClientDisconnect(this);
			if (hostId != -1)
			{
				NetworkTransport.Disconnect(hostId, connectionId, out byte _);
				RemoveObservers();
			}
		}

		internal void SetHandlers(NetworkMessageHandlers handlers)
		{
			m_MessageHandlers = handlers;
			m_MessageHandlersDict = handlers.GetHandlers();
		}

		public bool InvokeHandlerNoData(short msgType)
		{
			return InvokeHandler(msgType, null, 0);
		}

		public bool InvokeHandler(short msgType, NetworkReader reader, int channelId)
		{
			if (m_MessageHandlersDict.ContainsKey(msgType))
			{
				m_MessageInfo.msgType = msgType;
				m_MessageInfo.conn = this;
				m_MessageInfo.reader = reader;
				m_MessageInfo.channelId = channelId;
				NetworkMessageDelegate networkMessageDelegate = m_MessageHandlersDict[msgType];
				if (networkMessageDelegate == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkConnection InvokeHandler no handler for " + msgType);
					}
					return false;
				}
				networkMessageDelegate(m_MessageInfo);
				return true;
			}
			return false;
		}

		public bool InvokeHandler(NetworkMessage netMsg)
		{
			if (m_MessageHandlersDict.ContainsKey(netMsg.msgType))
			{
				NetworkMessageDelegate networkMessageDelegate = m_MessageHandlersDict[netMsg.msgType];
				networkMessageDelegate(netMsg);
				return true;
			}
			return false;
		}

		public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			m_MessageHandlers.RegisterHandler(msgType, handler);
		}

		public void UnregisterHandler(short msgType)
		{
			m_MessageHandlers.UnregisterHandler(msgType);
		}

		internal void SetPlayerController(PlayerController player)
		{
			while (player.playerControllerId >= m_PlayerControllers.Count)
			{
				m_PlayerControllers.Add(new PlayerController());
			}
			m_PlayerControllers[player.playerControllerId] = player;
		}

		internal void RemovePlayerController(short playerControllerId)
		{
			for (int num = m_PlayerControllers.Count; num >= 0; num--)
			{
				if (playerControllerId == num && playerControllerId == m_PlayerControllers[num].playerControllerId)
				{
					m_PlayerControllers[num] = new PlayerController();
					return;
				}
			}
			if (LogFilter.logError)
			{
				Debug.LogError("RemovePlayer player at playerControllerId " + playerControllerId + " not found");
			}
		}

		internal bool GetPlayerController(short playerControllerId, out PlayerController playerController)
		{
			playerController = null;
			if (playerControllers.Count > 0)
			{
				for (int i = 0; i < playerControllers.Count; i++)
				{
					if (playerControllers[i].IsValid && playerControllers[i].playerControllerId == playerControllerId)
					{
						playerController = playerControllers[i];
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public void FlushChannels()
		{
			if (m_Channels != null)
			{
				ChannelBuffer[] channels = m_Channels;
				foreach (ChannelBuffer channelBuffer in channels)
				{
					channelBuffer.CheckInternalBuffer();
				}
			}
		}

		public void SetMaxDelay(float seconds)
		{
			if (m_Channels != null)
			{
				ChannelBuffer[] channels = m_Channels;
				foreach (ChannelBuffer channelBuffer in channels)
				{
					channelBuffer.maxDelay = seconds;
				}
			}
		}

		public virtual bool Send(short msgType, MessageBase msg)
		{
			return SendByChannel(msgType, msg, 0);
		}

		public virtual bool SendUnreliable(short msgType, MessageBase msg)
		{
			return SendByChannel(msgType, msg, 1);
		}

		public virtual bool SendByChannel(short msgType, MessageBase msg, int channelId)
		{
			m_Writer.StartMessage(msgType);
			msg.Serialize(m_Writer);
			m_Writer.FinishMessage();
			return SendWriter(m_Writer, channelId);
		}

		public virtual bool SendBytes(byte[] bytes, int numBytes, int channelId)
		{
			if (logNetworkMessages)
			{
				LogSend(bytes);
			}
			return CheckChannel(channelId) && m_Channels[channelId].SendBytes(bytes, numBytes);
		}

		public virtual bool SendWriter(NetworkWriter writer, int channelId)
		{
			if (logNetworkMessages)
			{
				LogSend(writer.ToArray());
			}
			return CheckChannel(channelId) && m_Channels[channelId].SendWriter(writer);
		}

		private void LogSend(byte[] bytes)
		{
			NetworkReader networkReader = new NetworkReader(bytes);
			ushort num = networkReader.ReadUInt16();
			ushort num2 = networkReader.ReadUInt16();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 4; i < 4 + num; i++)
			{
				stringBuilder.AppendFormat("{0:X2}", bytes[i]);
				if (i > 150)
				{
					break;
				}
			}
			Debug.Log("ConnectionSend con:" + connectionId + " bytes:" + num + " msgId:" + num2 + " " + stringBuilder);
		}

		private bool CheckChannel(int channelId)
		{
			if (m_Channels == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Channels not initialized sending on id '" + channelId);
				}
				return false;
			}
			if (channelId < 0 || channelId >= m_Channels.Length)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Invalid channel when sending buffered data, '" + channelId + "'. Current channel count is " + m_Channels.Length);
				}
				return false;
			}
			return true;
		}

		public void ResetStats()
		{
		}

		protected void HandleBytes(byte[] buffer, int receivedSize, int channelId)
		{
			NetworkReader reader = new NetworkReader(buffer);
			HandleReader(reader, receivedSize, channelId);
		}

		protected void HandleReader(NetworkReader reader, int receivedSize, int channelId)
		{
			short num2;
			while (true)
			{
				if (reader.Position >= receivedSize)
				{
					return;
				}
				ushort num = reader.ReadUInt16();
				num2 = reader.ReadInt16();
				byte[] array = reader.ReadBytes(num);
				NetworkReader reader2 = new NetworkReader(array);
				if (logNetworkMessages)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < num; i++)
					{
						stringBuilder.AppendFormat("{0:X2}", array[i]);
						if (i > 150)
						{
							break;
						}
					}
					Debug.Log("ConnectionRecv con:" + connectionId + " bytes:" + num + " msgId:" + num2 + " " + stringBuilder);
				}
				NetworkMessageDelegate networkMessageDelegate = null;
				if (m_MessageHandlersDict.ContainsKey(num2))
				{
					networkMessageDelegate = m_MessageHandlersDict[num2];
				}
				if (networkMessageDelegate == null)
				{
					break;
				}
				m_NetMsg.msgType = num2;
				m_NetMsg.reader = reader2;
				m_NetMsg.conn = this;
				m_NetMsg.channelId = channelId;
				networkMessageDelegate(m_NetMsg);
				lastMessageTime = Time.time;
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Unknown message ID " + num2 + " connId:" + connectionId);
			}
		}

		public virtual void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
		{
			numMsgs = 0;
			numBufferedMsgs = 0;
			numBytes = 0;
			lastBufferedPerSecond = 0;
			ChannelBuffer[] channels = m_Channels;
			foreach (ChannelBuffer channelBuffer in channels)
			{
				numMsgs += channelBuffer.numMsgsOut;
				numBufferedMsgs += channelBuffer.numBufferedMsgsOut;
				numBytes += channelBuffer.numBytesOut;
				lastBufferedPerSecond += channelBuffer.lastBufferedPerSecond;
			}
		}

		public virtual void GetStatsIn(out int numMsgs, out int numBytes)
		{
			numMsgs = 0;
			numBytes = 0;
			ChannelBuffer[] channels = m_Channels;
			foreach (ChannelBuffer channelBuffer in channels)
			{
				numMsgs += channelBuffer.numMsgsIn;
				numBytes += channelBuffer.numBytesIn;
			}
		}

		public override string ToString()
		{
			return $"hostId: {hostId} connectionId: {connectionId} isReady: {isReady} channel count: {((m_Channels != null) ? m_Channels.Length : 0)}";
		}

		internal void AddToVisList(NetworkIdentity uv)
		{
			m_VisList.Add(uv);
			NetworkServer.ShowForConnection(uv, this);
		}

		internal void RemoveFromVisList(NetworkIdentity uv, bool isDestroyed)
		{
			m_VisList.Remove(uv);
			if (!isDestroyed)
			{
				NetworkServer.HideForConnection(uv, this);
			}
		}

		internal void RemoveObservers()
		{
			foreach (NetworkIdentity vis in m_VisList)
			{
				vis.RemoveObserverInternal(this);
			}
			m_VisList.Clear();
		}

		public virtual void TransportRecieve(byte[] bytes, int numBytes, int channelId)
		{
			HandleBytes(bytes, numBytes, channelId);
		}

		public virtual bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
		{
			return NetworkTransport.Send(hostId, connectionId, channelId, bytes, numBytes, out error);
		}

		internal void AddOwnedObject(NetworkIdentity obj)
		{
			if (m_ClientOwnedObjects == null)
			{
				m_ClientOwnedObjects = new HashSet<NetworkInstanceId>();
			}
			m_ClientOwnedObjects.Add(obj.netId);
		}

		internal void RemoveOwnedObject(NetworkIdentity obj)
		{
			if (m_ClientOwnedObjects != null)
			{
				m_ClientOwnedObjects.Remove(obj.netId);
			}
		}
	}
}
