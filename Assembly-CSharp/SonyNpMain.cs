using Sony.NP;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.PSVita;

public class SonyNpMain : MonoBehaviour, IScreen
{
	private struct Avatar
	{
		public string url;

		public bool pendingDownload;

		public Texture2D texture;

		public GameObject gameObject;

		public Avatar(GameObject gameObject)
		{
			this.gameObject = gameObject;
			url = string.Empty;
			pendingDownload = false;
			texture = null;
		}
	}

	public struct SharedSessionData
	{
		public int id;

		public string text;

		public int item1;

		public int item2;

		public byte[] WriteToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(id);
			binaryWriter.Write(text);
			binaryWriter.Write(item1);
			binaryWriter.Write(item2);
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public void ReadFromBuffer(byte[] buffer)
		{
			MemoryStream input = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(input);
			id = binaryReader.ReadInt32();
			text = binaryReader.ReadString();
			item1 = binaryReader.ReadInt32();
			item2 = binaryReader.ReadInt32();
			binaryReader.Close();
		}
	}

	private MenuStack menuStack;

	private MenuLayout menuMain;

	private bool npReady;

	private SonyNpUser user;

	private SonyNpFriends friends;

	private SonyNpTrophy trophies;

	private SonyNpRanking ranking;

	private SonyNpSession sessions;

	private int sendCount;

	private float sendingInterval = 1f;

	private SonyNpMessaging messaging;

	private SonyNpCloud cloudStorage;

	private SonyNpUtilities utilities;

	private SonyNpCommerce commerce;

	private SonyNpRequests requests;

	private static Avatar[] avatars = new Avatar[2];

	public static Texture2D avatarTexture = null;

	private void Start()
	{
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Invalid comparison between Unknown and I4
		avatars[0] = new Avatar(GameObject.Find("UserAvatar"));
		avatars[1] = new Avatar(GameObject.Find("RemoteUserAvatar"));
		menuMain = new MenuLayout(this, 500, 34);
		menuStack = new MenuStack();
		menuStack.SetMenu(menuMain);
		Main.OnNPInitialized += OnNPInitialized;
		OnScreenLog.Add("Initializing NP");
		Main.enableInternalLogging = true;
		Main.OnLog += OnLog;
		Main.OnLogWarning += OnLogWarning;
		Main.OnLogError += OnLogError;
		int kNpToolkitCreate_CacheTrophyIcons = Main.kNpToolkitCreate_CacheTrophyIcons;
		Main.Initialize(kNpToolkitCreate_CacheTrophyIcons);
		string sessionImage = Application.streamingAssetsPath + "/PSP2SessionImage.jpg";
		Main.SetSessionImage(sessionImage);
		Sony.NP.System.OnConnectionUp += OnSomeEvent;
		Sony.NP.System.OnConnectionDown += OnConnectionDown;
		Sony.NP.System.OnSysResume += OnSomeEvent;
		Sony.NP.System.OnSysNpMessageArrived += OnSomeEvent;
		Sony.NP.System.OnSysStorePurchase += OnSomeEvent;
		Sony.NP.System.OnSysStoreRedemption += OnSomeEvent;
		Sony.NP.System.OnSysEvent += OnSomeEvent;
		Messaging.OnSessionInviteMessageRetrieved += OnMessagingSessionInviteRetrieved;
		Messaging.OnMessageSessionInviteReceived += OnMessagingSessionInviteReceived;
		Messaging.OnMessageSessionInviteAccepted += OnMessagingSessionInviteAccepted;
		User.OnSignedIn += OnSignedIn;
		User.OnSignedOut += OnSomeEvent;
		User.OnSignInError += OnSignInError;
		user = new SonyNpUser();
		friends = new SonyNpFriends();
		trophies = new SonyNpTrophy();
		ranking = new SonyNpRanking();
		sessions = new SonyNpSession();
		messaging = new SonyNpMessaging();
		commerce = new SonyNpCommerce();
		cloudStorage = new SonyNpCloud();
		utilities = new SonyNpUtilities();
		Utility.SkuFlags skuFlags = Utility.skuFlags;
		if ((int)skuFlags == 1)
		{
			OnScreenLog.Add("Trial Mode, purchase the full app to get extra features.");
		}
	}

	public static void SetAvatarURL(string url, int index)
	{
		avatars[index].url = url;
		avatars[index].pendingDownload = true;
	}

	private IEnumerator DownloadAvatar(int index)
	{
		OnScreenLog.Add(" Downloading avatar image");
		avatars[index].gameObject.GetComponent<GUITexture>().texture = null;
		avatars[index].texture = new Texture2D(4, 4, TextureFormat.DXT1, mipmap: false);
		WWW www = new WWW(avatars[index].url);
		yield return www;
		www.LoadImageIntoTexture(avatars[index].texture);
		if (www.bytesDownloaded == 0)
		{
			OnScreenLog.Add(" Error: " + www.error);
		}
		else
		{
			avatars[index].texture.Apply(updateMipmaps: true, makeNoLongerReadable: true);
			Console.WriteLine("w " + avatars[index].texture.width + ", h " + avatars[index].texture.height + ", f " + avatars[index].texture.format);
			if (avatars[index].texture != null)
			{
				avatars[index].gameObject.GetComponent<GUITexture>().texture = avatars[index].texture;
			}
		}
		OnScreenLog.Add(" Done");
	}

	private void Update()
	{
		Main.Update();
		if (sessions.sendingData)
		{
			sendingInterval -= Time.deltaTime;
			if (sendingInterval <= 0f)
			{
				SendSessionData();
				sendingInterval = 1f;
			}
		}
		for (int i = 0; i < avatars.Length; i++)
		{
			if (avatars[i].pendingDownload)
			{
				avatars[i].pendingDownload = false;
				StartCoroutine(DownloadAvatar(i));
			}
		}
	}

	private void OnNPInitialized(Messages.PluginMessage msg)
	{
		npReady = true;
	}

	private void MenuMain()
	{
		menuMain.Update();
		bool isSignedInPSN = User.IsSignedInPSN;
		if (!npReady)
		{
			return;
		}
		if (!isSignedInPSN && menuMain.AddItem("Sign In To PSN", npReady))
		{
			OnScreenLog.Add("Begin sign in");
			User.SignIn();
		}
		if (menuMain.AddItem("Trophies"))
		{
			menuStack.PushMenu(trophies.GetMenu());
		}
		if (menuMain.AddItem("User"))
		{
			menuStack.PushMenu(user.GetMenu());
		}
		if (menuMain.AddItem("Utilities, Dialogs & Auth"))
		{
			menuStack.PushMenu(utilities.GetMenu());
		}
		if (isSignedInPSN)
		{
			if (menuMain.AddItem("Friends & SNS", isSignedInPSN))
			{
				menuStack.PushMenu(friends.GetMenu());
			}
			if (menuMain.AddItem("Ranking", isSignedInPSN))
			{
				menuStack.PushMenu(ranking.GetMenu());
			}
			if (menuMain.AddItem("Matching", isSignedInPSN))
			{
				menuStack.PushMenu(sessions.GetMenu());
			}
			if (menuMain.AddItem("Messaging", isSignedInPSN))
			{
				menuStack.PushMenu(messaging.GetMenu());
			}
			if (menuMain.AddItem("Cloud Storage (TUS/TSS)", isSignedInPSN))
			{
				menuStack.PushMenu(cloudStorage.GetMenu());
			}
			if (menuMain.AddItem("Commerce", isSignedInPSN))
			{
				menuStack.PushMenu(commerce.GetMenu());
			}
		}
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		MenuMain();
	}

	private void OnGUI()
	{
		MenuLayout menu = menuStack.GetMenu();
		menu.GetOwner().Process(menuStack);
	}

	private void OnLog(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(msg.Text);
	}

	private void OnLogWarning(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("WARNING: " + msg.Text);
	}

	private void OnLogError(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("ERROR?: " + msg.Text);
	}

	private void OnSignedIn(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(msg.ToString());
		ResultCode result = default(ResultCode);
		User.GetLastSignInError(out result);
		if (result.lastError == ErrorCode.NP_SIGNED_IN_FLIGHT_MODE)
		{
			OnScreenLog.Add("INFO: Signed in but flight mode is on");
		}
		else if (result.lastError != 0)
		{
			OnScreenLog.Add("Error: " + result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(msg.ToString());
	}

	private void OnConnectionDown(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Connection Down");
		ResultCode result = default(ResultCode);
		Sony.NP.System.GetLastConnectionError(out result);
		OnScreenLog.Add("Reason: " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
	}

	private void OnSignInError(Messages.PluginMessage msg)
	{
		ResultCode result = default(ResultCode);
		User.GetLastSignInError(out result);
		OnScreenLog.Add(result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
	}

	private IEnumerator DoJoinSessionFromInvite()
	{
		if (Matching.InSession)
		{
			OnScreenLog.Add("Leaving current session...");
			Matching.LeaveSession();
			while (Matching.SessionIsBusy)
			{
				yield return null;
			}
		}
		OnScreenLog.Add("Setting invited member attributes...");
		Matching.ClearSessionAttributes();
		Matching.AddSessionAttribute(new Matching.SessionAttribute
		{
			name = "CAR_TYPE",
			binValue = "CATMOB"
		});
		OnScreenLog.Add("Joining invited session...");
		if (Matching.GetSessionInviteSessionAttribute("PASSWORD", out Matching.SessionAttributeInfo passAttribute) == ErrorCode.NP_OK)
		{
			OnScreenLog.Add("Found PASSWORD attribute ..." + passAttribute.attributeBinValue);
			if (passAttribute.attributeBinValue == "YES")
			{
				OnScreenLog.Add("Session requires password...");
				Matching.JoinInvitedSession(sessions.sessionPassword);
			}
			else
			{
				OnScreenLog.Add("No password required...");
				Matching.JoinInvitedSession();
			}
		}
		else
		{
			Matching.JoinInvitedSession();
		}
		menuStack.SetMenu(menuMain);
		menuStack.PushMenu(sessions.GetMenu());
	}

	private void OnMessagingSessionInviteRetrieved(Messages.PluginMessage msg)
	{
		StartCoroutine("DoJoinSessionFromInvite");
	}

	private void OnMessagingSessionInviteReceived(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(" OnMessagingSessionInviteReceived ");
	}

	private void OnMessagingSessionInviteAccepted(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(" OnMessagingSessionInviteAccepted ");
	}

	private void OnServerInitialized(NetworkPlayer player)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		OnScreenLog.Add("Server Initialized: " + player.ipAddress + ":" + player.port);
		OnScreenLog.Add(" Network.isServer: " + Network.isServer);
		OnScreenLog.Add(" Network.isClient: " + Network.isClient);
		OnScreenLog.Add(" Network.peerType: " + Network.peerType);
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		OnScreenLog.Add("Player connected from " + player.ipAddress + ":" + player.port);
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		OnScreenLog.Add("Player disconnected " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	private void SendSessionData()
	{
		SharedSessionData sharedSessionData = default(SharedSessionData);
		sharedSessionData.id = sendCount++;
		sharedSessionData.text = "Here's some RPC data";
		sharedSessionData.item1 = 2;
		sharedSessionData.item2 = 987654321;
		byte[] array = sharedSessionData.WriteToBuffer();
		GetComponent<NetworkView>().RPC("RecieveSharedSessionData", RPCMode.Others, new object[1]
		{
			array
		});
	}

	[RPC]
	private void RecieveSharedSessionData(byte[] buffer)
	{
		SharedSessionData sharedSessionData = default(SharedSessionData);
		sharedSessionData.ReadFromBuffer(buffer);
		OnScreenLog.Add("RPC Rec: id " + sharedSessionData.id + " - " + sharedSessionData.text + " item1: " + sharedSessionData.item1 + " item2: " + sharedSessionData.item2);
	}

	private void OnConnectedToServer()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		OnScreenLog.Add("Connected to server...");
		OnScreenLog.Add(" Network.isServer: " + Network.isServer);
		OnScreenLog.Add(" Network.isClient: " + Network.isClient);
		OnScreenLog.Add(" Network.peerType: " + Network.peerType);
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		OnScreenLog.Add("Disconnected from server " + info);
		sessions.sendingData = false;
		sendCount = 0;
	}

	private void OnFailedToConnect(NetworkConnectionError error)
	{
		OnScreenLog.Add("Could not connect to server: " + error);
	}
}
