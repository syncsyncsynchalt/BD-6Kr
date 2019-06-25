using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal sealed class LocalClient : NetworkClient
	{
		private struct InternalMsg
		{
			internal byte[] buffer;

			internal int channelId;
		}

		private const int k_InitialFreeMessagePoolSize = 64;

		private List<InternalMsg> m_InternalMsgs = new List<InternalMsg>();

		private List<InternalMsg> m_InternalMsgs2 = new List<InternalMsg>();

		private Stack<InternalMsg> m_FreeMessages;

		private NetworkServer m_LocalServer;

		private bool m_Connected;

		private NetworkMessage s_InternalMessage = new NetworkMessage();

		public override void Disconnect()
		{
			ClientScene.HandleClientDisconnect(m_Connection);
			if (m_Connected)
			{
				PostInternalMessage(33);
				m_Connected = false;
			}
			m_AsyncConnect = ConnectState.Disconnected;
		}

		internal void InternalConnectLocalServer()
		{
			if (m_FreeMessages == null)
			{
				m_FreeMessages = new Stack<InternalMsg>();
				for (int i = 0; i < 64; i++)
				{
					m_FreeMessages.Push(default(InternalMsg));
				}
			}
			m_LocalServer = NetworkServer.instance;
			m_Connection = new ULocalConnectionToServer(m_LocalServer);
			SetHandlers(m_Connection);
			m_Connection.connectionId = m_LocalServer.AddLocalClient(this);
			m_AsyncConnect = ConnectState.Connected;
			NetworkClient.SetActive(state: true);
			RegisterSystemHandlers(localClient: true);
			PostInternalMessage(32);
			m_Connected = true;
		}

		internal override void Update()
		{
			ProcessInternalMessages();
		}

		internal void AddLocalPlayer(PlayerController localPlayer)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Local client AddLocalPlayer " + localPlayer.gameObject.name + " conn=" + m_Connection.connectionId);
			}
			m_Connection.isReady = true;
			m_Connection.SetPlayerController(localPlayer);
			NetworkIdentity unetView = localPlayer.unetView;
			if (unetView != null)
			{
				ClientScene.SetLocalObject(unetView.netId, localPlayer.gameObject);
				unetView.SetConnectionToServer(m_Connection);
			}
			ClientScene.InternalAddPlayer(unetView, localPlayer.playerControllerId);
		}

		private void PostInternalMessage(byte[] buffer, int channelId)
		{
			InternalMsg item = (m_FreeMessages.Count != 0) ? m_FreeMessages.Pop() : default(InternalMsg);
			item.buffer = buffer;
			item.channelId = channelId;
			m_InternalMsgs.Add(item);
		}

		private void PostInternalMessage(short msgType)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.StartMessage(msgType);
			networkWriter.FinishMessage();
			PostInternalMessage(networkWriter.AsArray(), 0);
		}

		private void ProcessInternalMessages()
		{
			if (m_InternalMsgs.Count != 0)
			{
				List<InternalMsg> internalMsgs = m_InternalMsgs;
				m_InternalMsgs = m_InternalMsgs2;
				foreach (InternalMsg item in internalMsgs)
				{
					InternalMsg current = item;
					if (s_InternalMessage.reader == null)
					{
						s_InternalMessage.reader = new NetworkReader(current.buffer);
					}
					else
					{
						s_InternalMessage.reader.Replace(current.buffer);
					}
					s_InternalMessage.reader.ReadInt16();
					s_InternalMessage.channelId = current.channelId;
					s_InternalMessage.conn = base.connection;
					s_InternalMessage.msgType = s_InternalMessage.reader.ReadInt16();
					m_Connection.InvokeHandler(s_InternalMessage);
					m_FreeMessages.Push(current);
					base.connection.lastMessageTime = Time.time;
				}
				m_InternalMsgs = internalMsgs;
				m_InternalMsgs.Clear();
				foreach (InternalMsg item2 in m_InternalMsgs2)
				{
					m_InternalMsgs.Add(item2);
				}
				m_InternalMsgs2.Clear();
			}
		}

		internal void InvokeHandlerOnClient(short msgType, MessageBase msg, int channelId)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			networkWriter.StartMessage(msgType);
			msg.Serialize(networkWriter);
			networkWriter.FinishMessage();
			InvokeBytesOnClient(networkWriter.AsArray(), channelId);
		}

		internal void InvokeBytesOnClient(byte[] buffer, int channelId)
		{
			PostInternalMessage(buffer, channelId);
		}
	}
}
