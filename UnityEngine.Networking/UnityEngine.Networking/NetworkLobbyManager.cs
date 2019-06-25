using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkLobbyManager")]
	public class NetworkLobbyManager : NetworkManager
	{
		private struct PendingPlayer
		{
			public NetworkConnection conn;

			public GameObject lobbyPlayer;
		}

		[SerializeField]
		private bool m_ShowLobbyGUI = true;

		[SerializeField]
		private int m_MaxPlayers = 4;

		[SerializeField]
		private int m_MaxPlayersPerConnection = 1;

		[SerializeField]
		private int m_MinPlayers;

		[SerializeField]
		private NetworkLobbyPlayer m_LobbyPlayerPrefab;

		[SerializeField]
		private GameObject m_GamePlayerPrefab;

		[SerializeField]
		private string m_LobbyScene = string.Empty;

		[SerializeField]
		private string m_PlayScene = string.Empty;

		private List<PendingPlayer> m_PendingPlayers = new List<PendingPlayer>();

		public NetworkLobbyPlayer[] lobbySlots;

		private static LobbyReadyToBeginMessage s_ReadyToBeginMessage = new LobbyReadyToBeginMessage();

		private static IntegerMessage s_SceneLoadedMessage = new IntegerMessage();

		private static LobbyReadyToBeginMessage s_LobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();

		public bool showLobbyGUI
		{
			get
			{
				return m_ShowLobbyGUI;
			}
			set
			{
				m_ShowLobbyGUI = value;
			}
		}

		public int maxPlayers
		{
			get
			{
				return m_MaxPlayers;
			}
			set
			{
				m_MaxPlayers = value;
			}
		}

		public int maxPlayersPerConnection
		{
			get
			{
				return m_MaxPlayersPerConnection;
			}
			set
			{
				m_MaxPlayersPerConnection = value;
			}
		}

		public int minPlayers
		{
			get
			{
				return m_MinPlayers;
			}
			set
			{
				m_MinPlayers = value;
			}
		}

		public NetworkLobbyPlayer lobbyPlayerPrefab
		{
			get
			{
				return m_LobbyPlayerPrefab;
			}
			set
			{
				m_LobbyPlayerPrefab = value;
			}
		}

		public GameObject gamePlayerPrefab
		{
			get
			{
				return m_GamePlayerPrefab;
			}
			set
			{
				m_GamePlayerPrefab = value;
			}
		}

		public string lobbyScene
		{
			get
			{
				return m_LobbyScene;
			}
			set
			{
				m_LobbyScene = value;
				base.offlineScene = value;
			}
		}

		public string playScene
		{
			get
			{
				return m_PlayScene;
			}
			set
			{
				m_PlayScene = value;
			}
		}

		private void OnValidate()
		{
			if (m_MaxPlayers <= 0)
			{
				m_MaxPlayers = 1;
			}
			if (m_MaxPlayersPerConnection <= 0)
			{
				m_MaxPlayersPerConnection = 1;
			}
			if (m_MaxPlayersPerConnection > maxPlayers)
			{
				m_MaxPlayersPerConnection = maxPlayers;
			}
			if (m_MinPlayers < 0)
			{
				m_MinPlayers = 0;
			}
			if (m_MinPlayers > m_MaxPlayers)
			{
				m_MinPlayers = m_MaxPlayers;
			}
		}

		private byte FindSlot()
		{
			for (byte b = 0; b < maxPlayers; b = (byte)(b + 1))
			{
				if (lobbySlots[b] == null)
				{
					return b;
				}
			}
			return byte.MaxValue;
		}

		private void SceneLoadedForPlayer(NetworkConnection conn, GameObject lobbyPlayerGameObject)
		{
			NetworkLobbyPlayer component = lobbyPlayerGameObject.GetComponent<NetworkLobbyPlayer>();
			if (component == null)
			{
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobby SceneLoadedForPlayer scene:" + Application.loadedLevelName + " " + conn);
			}
			if (Application.loadedLevelName == m_LobbyScene)
			{
				PendingPlayer item = default(PendingPlayer);
				item.conn = conn;
				item.lobbyPlayer = lobbyPlayerGameObject;
				m_PendingPlayers.Add(item);
				return;
			}
			short playerControllerId = lobbyPlayerGameObject.GetComponent<NetworkIdentity>().playerControllerId;
			GameObject gameObject = OnLobbyServerCreateGamePlayer(conn, playerControllerId);
			if (gameObject == null)
			{
				Transform startPosition = GetStartPosition();
				gameObject = ((!(startPosition != null)) ? ((GameObject)Object.Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity)) : ((GameObject)Object.Instantiate(gamePlayerPrefab, startPosition.position, startPosition.rotation)));
			}
			if (OnLobbyServerSceneLoadedForPlayer(lobbyPlayerGameObject, gameObject))
			{
				NetworkServer.ReplacePlayerForConnection(conn, gameObject, playerControllerId);
			}
		}

		private static bool CheckConnectionIsReadyToBegin(NetworkConnection conn)
		{
			foreach (PlayerController playerController in conn.playerControllers)
			{
				if (playerController.IsValid)
				{
					NetworkLobbyPlayer component = playerController.gameObject.GetComponent<NetworkLobbyPlayer>();
					if (!component.readyToBegin)
					{
						return false;
					}
				}
			}
			return true;
		}

		public void CheckReadyToBegin()
		{
			if (!(Application.loadedLevelName != m_LobbyScene))
			{
				int num = 0;
				foreach (NetworkConnection connection in NetworkServer.connections)
				{
					if (connection != null)
					{
						if (!CheckConnectionIsReadyToBegin(connection))
						{
							return;
						}
						num++;
					}
				}
				foreach (NetworkConnection localConnection in NetworkServer.localConnections)
				{
					if (localConnection != null)
					{
						if (!CheckConnectionIsReadyToBegin(localConnection))
						{
							return;
						}
						num++;
					}
				}
				if (m_MinPlayers <= 0 || num >= m_MinPlayers)
				{
					m_PendingPlayers.Clear();
					OnLobbyServerPlayersReady();
				}
			}
		}

		public void ServerReturnToLobby()
		{
			if (!NetworkServer.active)
			{
				Debug.Log("ServerReturnToLobby called on client");
			}
			else
			{
				ServerChangeScene(m_LobbyScene);
			}
		}

		private void CallOnClientEnterLobby()
		{
			OnLobbyClientEnter();
			NetworkLobbyPlayer[] array = lobbySlots;
			foreach (NetworkLobbyPlayer networkLobbyPlayer in array)
			{
				if (!(networkLobbyPlayer == null))
				{
					networkLobbyPlayer.readyToBegin = false;
					networkLobbyPlayer.OnClientEnterLobby();
				}
			}
		}

		private void CallOnClientExitLobby()
		{
			OnLobbyClientExit();
			NetworkLobbyPlayer[] array = lobbySlots;
			foreach (NetworkLobbyPlayer networkLobbyPlayer in array)
			{
				if (!(networkLobbyPlayer == null))
				{
					networkLobbyPlayer.OnClientExitLobby();
				}
			}
		}

		public bool SendReturnToLobby()
		{
			if (client == null || !client.isConnected)
			{
				return false;
			}
			EmptyMessage msg = new EmptyMessage();
			client.Send(46, msg);
			return true;
		}

		public override void OnServerConnect(NetworkConnection conn)
		{
			if (base.numPlayers >= maxPlayers)
			{
				conn.Disconnect();
				return;
			}
			if (Application.loadedLevelName != m_LobbyScene)
			{
				conn.Disconnect();
				return;
			}
			base.OnServerConnect(conn);
			OnLobbyServerConnect(conn);
		}

		public override void OnServerDisconnect(NetworkConnection conn)
		{
			base.OnServerDisconnect(conn);
			OnLobbyServerDisconnect(conn);
		}

		public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
		{
			if (Application.loadedLevelName != m_LobbyScene)
			{
				return;
			}
			int num = 0;
			foreach (PlayerController playerController in conn.playerControllers)
			{
				if (playerController.IsValid)
				{
					num++;
				}
			}
			if (num >= maxPlayersPerConnection)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkLobbyManager no more players for this connection.");
				}
				EmptyMessage msg = new EmptyMessage();
				conn.Send(45, msg);
				return;
			}
			byte b = FindSlot();
			if (b == byte.MaxValue)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkLobbyManager no space for more players");
				}
				EmptyMessage msg2 = new EmptyMessage();
				conn.Send(45, msg2);
				return;
			}
			GameObject gameObject = OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
			if (gameObject == null)
			{
				gameObject = (GameObject)Object.Instantiate(lobbyPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
			}
			NetworkLobbyPlayer component = gameObject.GetComponent<NetworkLobbyPlayer>();
			component.slot = b;
			lobbySlots[b] = component;
			NetworkServer.AddPlayerForConnection(conn, gameObject, playerControllerId);
		}

		public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
		{
			short playerControllerId = player.playerControllerId;
			byte slot = player.gameObject.GetComponent<NetworkLobbyPlayer>().slot;
			lobbySlots[slot] = null;
			base.OnServerRemovePlayer(conn, player);
			NetworkLobbyPlayer[] array = lobbySlots;
			foreach (NetworkLobbyPlayer networkLobbyPlayer in array)
			{
				if (networkLobbyPlayer != null)
				{
					networkLobbyPlayer.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
					s_LobbyReadyToBeginMessage.slotId = networkLobbyPlayer.slot;
					s_LobbyReadyToBeginMessage.readyState = false;
					NetworkServer.SendToReady(null, 43, s_LobbyReadyToBeginMessage);
				}
			}
			OnLobbyServerPlayerRemoved(conn, playerControllerId);
		}

		public override void ServerChangeScene(string sceneName)
		{
			if (sceneName == m_LobbyScene)
			{
				NetworkLobbyPlayer[] array = lobbySlots;
				foreach (NetworkLobbyPlayer networkLobbyPlayer in array)
				{
					if (!(networkLobbyPlayer == null))
					{
						NetworkIdentity component = networkLobbyPlayer.GetComponent<NetworkIdentity>();
						if (component.connectionToClient.GetPlayerController(component.playerControllerId, out PlayerController playerController))
						{
							NetworkServer.Destroy(playerController.gameObject);
						}
						if (NetworkServer.active)
						{
							networkLobbyPlayer.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
							NetworkServer.ReplacePlayerForConnection(component.connectionToClient, networkLobbyPlayer.gameObject, component.playerControllerId);
						}
					}
				}
			}
			base.ServerChangeScene(sceneName);
		}

		public override void OnServerSceneChanged(string sceneName)
		{
			if (sceneName != m_LobbyScene)
			{
				foreach (PendingPlayer pendingPlayer in m_PendingPlayers)
				{
					PendingPlayer current = pendingPlayer;
					SceneLoadedForPlayer(current.conn, current.lobbyPlayer);
				}
				m_PendingPlayers.Clear();
			}
			OnLobbyServerSceneChanged(sceneName);
		}

		private void OnServerReadyToBeginMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager OnServerReadyToBeginMessage");
			}
			netMsg.ReadMessage(s_ReadyToBeginMessage);
			if (!netMsg.conn.GetPlayerController(s_ReadyToBeginMessage.slotId, out PlayerController playerController))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager OnServerReadyToBeginMessage invalid playerControllerId " + s_ReadyToBeginMessage.slotId);
				}
				return;
			}
			NetworkLobbyPlayer component = playerController.gameObject.GetComponent<NetworkLobbyPlayer>();
			component.readyToBegin = s_ReadyToBeginMessage.readyState;
			LobbyReadyToBeginMessage lobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();
			lobbyReadyToBeginMessage.slotId = component.slot;
			lobbyReadyToBeginMessage.readyState = s_ReadyToBeginMessage.readyState;
			NetworkServer.SendToReady(null, 43, lobbyReadyToBeginMessage);
			CheckReadyToBegin();
		}

		private void OnServerSceneLoadedMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager OnSceneLoadedMessage");
			}
			netMsg.ReadMessage(s_SceneLoadedMessage);
			if (!netMsg.conn.GetPlayerController((short)s_SceneLoadedMessage.value, out PlayerController playerController))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager OnServerSceneLoadedMessage invalid playerControllerId " + s_SceneLoadedMessage.value);
				}
			}
			else
			{
				SceneLoadedForPlayer(netMsg.conn, playerController.gameObject);
			}
		}

		private void OnServerReturnToLobbyMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager OnServerReturnToLobbyMessage");
			}
			ServerReturnToLobby();
		}

		public override void OnStartServer()
		{
			if (lobbySlots.Length == 0)
			{
				lobbySlots = new NetworkLobbyPlayer[maxPlayers];
			}
			NetworkServer.RegisterHandler(43, OnServerReadyToBeginMessage);
			NetworkServer.RegisterHandler(44, OnServerSceneLoadedMessage);
			NetworkServer.RegisterHandler(46, OnServerReturnToLobbyMessage);
			OnLobbyStartServer();
		}

		public override void OnStartHost()
		{
			OnLobbyStartHost();
		}

		public override void OnStopHost()
		{
			OnLobbyStopHost();
		}

		public override void OnStartClient(NetworkClient lobbyClient)
		{
			if (lobbySlots.Length == 0)
			{
				lobbySlots = new NetworkLobbyPlayer[maxPlayers];
			}
			if (m_LobbyPlayerPrefab == null || m_LobbyPlayerPrefab.gameObject == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager no LobbyPlayer prefab is registered. Please add a LobbyPlayer prefab.");
				}
			}
			else
			{
				ClientScene.RegisterPrefab(m_LobbyPlayerPrefab.gameObject);
			}
			if (m_GamePlayerPrefab == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager no GamePlayer prefab is registered. Please add a GamePlayer prefab.");
				}
			}
			else
			{
				ClientScene.RegisterPrefab(m_GamePlayerPrefab);
			}
			lobbyClient.RegisterHandler(43, OnClientReadyToBegin);
			lobbyClient.RegisterHandler(45, OnClientAddPlayerFailedMessage);
			OnLobbyStartClient(lobbyClient);
		}

		public override void OnClientConnect(NetworkConnection conn)
		{
			OnLobbyClientConnect(conn);
			CallOnClientEnterLobby();
			base.OnClientConnect(conn);
		}

		public override void OnClientDisconnect(NetworkConnection conn)
		{
			OnLobbyClientDisconnect(conn);
			base.OnClientDisconnect(conn);
		}

		public override void OnStopClient()
		{
			OnLobbyStopClient();
			CallOnClientExitLobby();
		}

		public override void OnClientSceneChanged(NetworkConnection conn)
		{
			if (Application.loadedLevelName == lobbyScene)
			{
				if (client.isConnected)
				{
					CallOnClientEnterLobby();
				}
			}
			else
			{
				CallOnClientExitLobby();
			}
			base.OnClientSceneChanged(conn);
			OnLobbyClientSceneChanged(conn);
		}

		private void OnClientReadyToBegin(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_LobbyReadyToBeginMessage);
			if (s_LobbyReadyToBeginMessage.slotId >= lobbySlots.Count())
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager OnClientReadyToBegin invalid lobby slot " + s_LobbyReadyToBeginMessage.slotId);
				}
				return;
			}
			NetworkLobbyPlayer networkLobbyPlayer = lobbySlots[s_LobbyReadyToBeginMessage.slotId];
			if (networkLobbyPlayer == null || networkLobbyPlayer.gameObject == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager OnClientReadyToBegin no player at lobby slot " + s_LobbyReadyToBeginMessage.slotId);
				}
			}
			else
			{
				networkLobbyPlayer.readyToBegin = s_LobbyReadyToBeginMessage.readyState;
				networkLobbyPlayer.OnClientReady(s_LobbyReadyToBeginMessage.readyState);
			}
		}

		private void OnClientAddPlayerFailedMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager Add Player failed.");
			}
			OnLobbyClientAddPlayerFailed();
		}

		public virtual void OnLobbyStartHost()
		{
		}

		public virtual void OnLobbyStopHost()
		{
		}

		public virtual void OnLobbyStartServer()
		{
		}

		public virtual void OnLobbyServerConnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyServerDisconnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyServerSceneChanged(string sceneName)
		{
		}

		public virtual GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
		{
			return null;
		}

		public virtual GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
		{
			return null;
		}

		public virtual void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
		{
		}

		public virtual bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
		{
			return true;
		}

		public virtual void OnLobbyServerPlayersReady()
		{
			ServerChangeScene(m_PlayScene);
		}

		public virtual void OnLobbyClientEnter()
		{
		}

		public virtual void OnLobbyClientExit()
		{
		}

		public virtual void OnLobbyClientConnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyClientDisconnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyStartClient(NetworkClient lobbyClient)
		{
		}

		public virtual void OnLobbyStopClient()
		{
		}

		public virtual void OnLobbyClientSceneChanged(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyClientAddPlayerFailed()
		{
		}

		private void OnGUI()
		{
			if (!showLobbyGUI || Application.loadedLevelName != m_LobbyScene)
			{
				return;
			}
			Rect position = new Rect(90f, 180f, 500f, 150f);
			GUI.Box(position, "Players:");
			if (NetworkClient.active)
			{
				Rect position2 = new Rect(100f, 300f, 120f, 20f);
				if (GUI.Button(position2, "Add Player"))
				{
					TryToAddPlayer();
				}
			}
		}

		public void TryToAddPlayer()
		{
			if (NetworkClient.active)
			{
				short num = -1;
				List<PlayerController> playerControllers = NetworkClient.allClients[0].connection.playerControllers;
				if (playerControllers.Count < maxPlayers)
				{
					num = (short)playerControllers.Count;
				}
				else
				{
					for (short num2 = 0; num2 < maxPlayers; num2 = (short)(num2 + 1))
					{
						if (!playerControllers[num2].IsValid)
						{
							num = num2;
							break;
						}
					}
				}
				if (LogFilter.logDebug)
				{
					Debug.Log("NetworkLobbyManager TryToAddPlayer controllerId " + num + " ready:" + ClientScene.ready);
				}
				if (num == -1)
				{
					if (LogFilter.logDebug)
					{
						Debug.Log("NetworkLobbyManager No Space!");
					}
				}
				else if (ClientScene.ready)
				{
					ClientScene.AddPlayer(num);
				}
				else
				{
					ClientScene.AddPlayer(NetworkClient.allClients[0].connection, num);
				}
			}
			else if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager NetworkClient not active!");
			}
		}
	}
}
