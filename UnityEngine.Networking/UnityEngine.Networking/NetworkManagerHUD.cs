using System.ComponentModel;
using UnityEngine.Networking.Match;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class NetworkManagerHUD : MonoBehaviour
	{
		public NetworkManager manager;

		[SerializeField]
		public bool showGUI = true;

		[SerializeField]
		public int offsetX;

		[SerializeField]
		public int offsetY;

		private bool m_ShowServer;

		private void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		private void Update()
		{
			if (!showGUI)
			{
				return;
			}
			if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && manager.IsClientConnected() && Input.GetKeyDown(KeyCode.X))
			{
				manager.StopHost();
			}
		}

		private void OnGUI()
		{
			if (!showGUI)
			{
				return;
			}
			int num = 10 + offsetX;
			int num2 = 40 + offsetY;
			if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(num, num2, 200f, 20f), "LAN Host(H)"))
				{
					manager.StartHost();
				}
				num2 += 24;
				if (GUI.Button(new Rect(num, num2, 105f, 20f), "LAN Client(C)"))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(num + 100, num2, 95f, 20f), manager.networkAddress);
				num2 += 24;
				if (GUI.Button(new Rect(num, num2, 200f, 20f), "LAN Server Only(S)"))
				{
					manager.StartServer();
				}
				num2 += 24;
			}
			else
			{
				if (NetworkServer.active)
				{
					GUI.Label(new Rect(num, num2, 300f, 20f), "Server: port=" + manager.networkPort);
					num2 += 24;
				}
				if (manager.IsClientConnected())
				{
					GUI.Label(new Rect(num, num2, 300f, 20f), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
					num2 += 24;
				}
			}
			if (manager.IsClientConnected() && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(num, num2, 200f, 20f), "Client Ready"))
				{
					ClientScene.Ready(manager.client.connection);
					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				num2 += 24;
			}
			if (NetworkServer.active || manager.IsClientConnected())
			{
				if (GUI.Button(new Rect(num, num2, 200f, 20f), "Stop (X)"))
				{
					manager.StopHost();
				}
				num2 += 24;
			}
			if (NetworkServer.active || manager.IsClientConnected())
			{
				return;
			}
			num2 += 10;
			if (manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(num, num2, 200f, 20f), "Enable Match Maker (M)"))
				{
					manager.StartMatchMaker();
				}
				return;
			}
			if (manager.matchInfo == null)
			{
				if (manager.matches == null)
				{
					if (GUI.Button(new Rect(num, num2, 200f, 20f), "Create Internet Match"))
					{
						NetworkMatch matchMaker = manager.matchMaker;
						string matchName = manager.matchName;
						uint matchSize = manager.matchSize;
						string empty = string.Empty;
						NetworkManager networkManager = manager;
						matchMaker.CreateMatch(matchName, matchSize, matchAdvertise: true, empty, networkManager.OnMatchCreate);
					}
					num2 += 24;
					GUI.Label(new Rect(num, num2, 100f, 20f), "Room Name:");
					manager.matchName = GUI.TextField(new Rect(num + 100, num2, 100f, 20f), manager.matchName);
					num2 += 24;
					num2 += 10;
					if (GUI.Button(new Rect(num, num2, 200f, 20f), "Find Internet Match"))
					{
						NetworkMatch matchMaker2 = manager.matchMaker;
						string empty2 = string.Empty;
						NetworkManager networkManager2 = manager;
						matchMaker2.ListMatches(0, 20, empty2, networkManager2.OnMatchList);
					}
					num2 += 24;
				}
				else
				{
					foreach (MatchDesc match in manager.matches)
					{
						if (GUI.Button(new Rect(num, num2, 200f, 20f), "Join Match:" + match.name))
						{
							manager.matchName = match.name;
							manager.matchSize = (uint)match.currentSize;
							manager.matchMaker.JoinMatch(match.networkId, string.Empty, manager.OnMatchJoined);
						}
						num2 += 24;
					}
				}
			}
			if (GUI.Button(new Rect(num, num2, 200f, 20f), "Change MM server"))
			{
				m_ShowServer = !m_ShowServer;
			}
			if (m_ShowServer)
			{
				num2 += 24;
				if (GUI.Button(new Rect(num, num2, 100f, 20f), "Local"))
				{
					manager.SetMatchHost("localhost", 1337, https: false);
					m_ShowServer = false;
				}
				num2 += 24;
				if (GUI.Button(new Rect(num, num2, 100f, 20f), "Internet"))
				{
					manager.SetMatchHost("mm.unet.unity3d.com", 443, https: true);
					m_ShowServer = false;
				}
				num2 += 24;
				if (GUI.Button(new Rect(num, num2, 100f, 20f), "Staging"))
				{
					manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, https: true);
					m_ShowServer = false;
				}
			}
			num2 += 24;
			GUI.Label(new Rect(num, num2, 300f, 20f), "MM Uri: " + manager.matchMaker.baseUri);
			num2 += 24;
			if (GUI.Button(new Rect(num, num2, 200f, 20f), "Disable Match Maker"))
			{
				manager.StopMatchMaker();
			}
		}
	}
}
