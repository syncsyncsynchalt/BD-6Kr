using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkLobbyPlayer")]
	[DisallowMultipleComponent]
	public class NetworkLobbyPlayer : NetworkBehaviour
	{
		[SerializeField]
		public bool ShowLobbyGUI = true;

		private byte m_Slot;

		private bool m_ReadyToBegin;

		public byte slot
		{
			get
			{
				return m_Slot;
			}
			set
			{
				m_Slot = value;
			}
		}

		public bool readyToBegin
		{
			get
			{
				return m_ReadyToBegin;
			}
			set
			{
				m_ReadyToBegin = value;
			}
		}

		private void Start()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		public override void OnStartClient()
		{
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if ((bool)networkLobbyManager)
			{
				networkLobbyManager.lobbySlots[m_Slot] = this;
				m_ReadyToBegin = false;
				OnClientEnterLobby();
			}
			else
			{
				Debug.LogError("No Lobby for LobbyPlayer");
			}
		}

		public void SendReadyToBeginMessage()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyPlayer SendReadyToBeginMessage");
			}
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if ((bool)networkLobbyManager)
			{
				LobbyReadyToBeginMessage lobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();
				lobbyReadyToBeginMessage.slotId = (byte)base.playerControllerId;
				lobbyReadyToBeginMessage.readyState = true;
				networkLobbyManager.client.Send(43, lobbyReadyToBeginMessage);
			}
		}

		public void SendNotReadyToBeginMessage()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyPlayer SendReadyToBeginMessage");
			}
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if ((bool)networkLobbyManager)
			{
				LobbyReadyToBeginMessage lobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();
				lobbyReadyToBeginMessage.slotId = (byte)base.playerControllerId;
				lobbyReadyToBeginMessage.readyState = false;
				networkLobbyManager.client.Send(43, lobbyReadyToBeginMessage);
			}
		}

		public void SendSceneLoadedMessage()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyPlayer SendSceneLoadedMessage");
			}
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if ((bool)networkLobbyManager)
			{
				IntegerMessage msg = new IntegerMessage(base.playerControllerId);
				networkLobbyManager.client.Send(44, msg);
			}
		}

		private void OnLevelWasLoaded()
		{
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if ((!networkLobbyManager || !(Application.loadedLevelName == networkLobbyManager.lobbyScene)) && base.isLocalPlayer)
			{
				SendSceneLoadedMessage();
			}
		}

		public void RemovePlayer()
		{
			if (base.isLocalPlayer && !m_ReadyToBegin)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("NetworkLobbyPlayer RemovePlayer");
				}
				ClientScene.RemovePlayer(GetComponent<NetworkIdentity>().playerControllerId);
			}
		}

		public virtual void OnClientEnterLobby()
		{
		}

		public virtual void OnClientExitLobby()
		{
		}

		public virtual void OnClientReady(bool readyState)
		{
		}

		public override bool OnSerialize(NetworkWriter writer, bool initialState)
		{
			writer.WritePackedUInt32(1u);
			writer.Write(m_Slot);
			writer.Write(m_ReadyToBegin);
			return true;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (reader.ReadPackedUInt32() != 0)
			{
				m_Slot = reader.ReadByte();
				m_ReadyToBegin = reader.ReadBoolean();
			}
		}

		private void OnGUI()
		{
			if (!ShowLobbyGUI)
			{
				return;
			}
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if ((bool)networkLobbyManager && (!networkLobbyManager.showLobbyGUI || Application.loadedLevelName != networkLobbyManager.lobbyScene))
			{
				return;
			}
			Rect position = new Rect(100 + m_Slot * 100, 200f, 90f, 20f);
			if (base.isLocalPlayer)
			{
				GUI.Label(position, " [ You ]");
				if (m_ReadyToBegin)
				{
					position.y += 25f;
					if (GUI.Button(position, "Ready"))
					{
						SendNotReadyToBeginMessage();
					}
					return;
				}
				position.y += 25f;
				if (GUI.Button(position, "Not Ready"))
				{
					SendReadyToBeginMessage();
				}
				position.y += 25f;
				if (GUI.Button(position, "Remove"))
				{
					ClientScene.RemovePlayer(GetComponent<NetworkIdentity>().playerControllerId);
				}
			}
			else
			{
				GUI.Label(position, "Player [" + base.netId + "]");
				position.y += 25f;
				GUI.Label(position, "Ready [" + m_ReadyToBegin + "]");
			}
		}
	}
}
