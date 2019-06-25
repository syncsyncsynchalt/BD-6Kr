using Sony.NP;
using UnityEngine;

public class SonyNpSession : IScreen
{
	private MenuLayout menuSession;

	private MenuLayout menuInSessionHosting;

	private MenuLayout menuInSessionClient;

	private bool matchingIsReady;

	private int gameDetails = 100;

	private int cartype;

	private int serverPort = 25001;

	private int serverMaxConnections = 32;

	private int appVersion = 200;

	public string sessionPassword = "password";

	public bool sendingData;

	private Matching.Session[] availableSessions;

	private Matching.SessionMemberInfo? host;

	private Matching.SessionMemberInfo? myself;

	private Matching.SessionMemberInfo? connected;

	private Matching.FlagSessionCreate SignallingType = Matching.FlagSessionCreate.CREATE_SIGNALING_MESH_SESSION;

	public SonyNpSession()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuSession;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	private ErrorCode ErrorHandler(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Matching.GetLastError(out result);
			if (result.lastError != 0)
			{
				OnScreenLog.Add("Error: " + result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
				return result.lastError;
			}
		}
		return errorCode;
	}

	public void Process(MenuStack stack)
	{
		MenuSession(stack);
	}

	public void Initialize()
	{
		menuSession = new MenuLayout(this, 550, 34);
		menuInSessionHosting = new MenuLayout(this, 450, 34);
		menuInSessionClient = new MenuLayout(this, 450, 34);
		Matching.OnCreatedSession += OnMatchingCreatedSession;
		Matching.OnFoundSessions += OnMatchingFoundSessions;
		Matching.OnJoinedSession += OnMatchingJoinedSession;
		Matching.OnJoinInvalidSession += OnMatchingJoinInvalidSession;
		Matching.OnUpdatedSession += OnMatchingUpdatedSession;
		Matching.OnLeftSession += OnMatchingLeftSession;
		Matching.OnSessionDestroyed += OnMatchingSessionDestroyed;
		Matching.OnKickedOut += OnMatchingKickedOut;
		Matching.OnSessionError += OnSessionError;
		Matching.ClearAttributeDefinitions();
		Matching.AddAttributeDefinitionInt("LEVEL", Matching.EnumAttributeType.SESSION_SEARCH_ATTRIBUTE);
		Matching.AddAttributeDefinitionBin("RACE_TRACK", Matching.EnumAttributeType.SESSION_EXTERNAL_ATTRIBUTE, Matching.EnumAttributeMaxSize.SESSION_ATTRIBUTE_MAX_SIZE_12);
		Matching.AddAttributeDefinitionBin("CAR_TYPE", Matching.EnumAttributeType.SESSION_MEMBER_ATTRIBUTE, Matching.EnumAttributeMaxSize.SESSION_ATTRIBUTE_MAX_SIZE_28);
		Matching.AddAttributeDefinitionInt("GAME_DETAILS", Matching.EnumAttributeType.SESSION_INTERNAL_ATTRIBUTE);
		Matching.AddAttributeDefinitionInt("APP_VERSION", Matching.EnumAttributeType.SESSION_SEARCH_ATTRIBUTE);
		Matching.AddAttributeDefinitionBin("TEST_BIN_SEARCH", Matching.EnumAttributeType.SESSION_SEARCH_ATTRIBUTE, Matching.EnumAttributeMaxSize.SESSION_ATTRIBUTE_MAX_SIZE_60);
		Matching.AddAttributeDefinitionBin("PASSWORD", Matching.EnumAttributeType.SESSION_INTERNAL_ATTRIBUTE, Matching.EnumAttributeMaxSize.SESSION_ATTRIBUTE_MAX_SIZE_12);
		ErrorHandler(Matching.RegisterAttributeDefinitions());
	}

	public void MenuSession(MenuStack menuStack)
	{
		bool isSignedInPSN = User.IsSignedInPSN;
		bool inSession = Matching.InSession;
		if (!matchingIsReady && isSignedInPSN)
		{
			matchingIsReady = true;
		}
		if (inSession)
		{
			MenuInSession(menuStack);
		}
		else
		{
			MenuSetupSession(menuStack);
		}
	}

	public void MenuSetupSession(MenuStack menuStack)
	{
		bool isSignedInPSN = User.IsSignedInPSN;
		bool inSession = Matching.InSession;
		bool sessionIsBusy = Matching.SessionIsBusy;
		menuSession.Update();
		if (menuSession.AddItem("Create & Join Session", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Creating session...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.name = "APP_VERSION";
			sessionAttribute.intValue = appVersion;
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.name = "PASSWORD";
			sessionAttribute.binValue = "NO";
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.name = "CAR_TYPE";
			sessionAttribute.binValue = "CATMOB";
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.name = "LEVEL";
			sessionAttribute.intValue = 1;
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.name = "RACE_TRACK";
			sessionAttribute.binValue = "TURKEY";
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.name = "GAME_DETAILS";
			sessionAttribute.intValue = gameDetails;
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.name = "TEST_BIN_SEARCH";
			sessionAttribute.binValue = "BIN_VALUE";
			Matching.AddSessionAttribute(sessionAttribute);
			string name = "Test Session";
			int serverID = 0;
			int worldID = 0;
			int numSlots = 8;
			string empty = string.Empty;
			string ps4SessionStatus = "Toolkit Sample Session";
			ErrorHandler(Matching.CreateSession(name, serverID, worldID, numSlots, empty, SignallingType, Matching.EnumSessionType.SESSION_TYPE_PUBLIC, ps4SessionStatus));
		}
		if (menuSession.AddItem("Create & Join Private Session", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Creating private session... password is required");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.name = "APP_VERSION";
			sessionAttribute2.intValue = appVersion;
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.name = "PASSWORD";
			sessionAttribute2.binValue = "YES";
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.name = "CAR_TYPE";
			sessionAttribute2.binValue = "CATMOB";
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.name = "LEVEL";
			sessionAttribute2.intValue = 1;
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.name = "RACE_TRACK";
			sessionAttribute2.binValue = "TURKEY";
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.name = "GAME_DETAILS";
			sessionAttribute2.intValue = gameDetails;
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.name = "TEST_BIN_SEARCH";
			sessionAttribute2.binValue = "BIN_VALUE";
			Matching.AddSessionAttribute(sessionAttribute2);
			string name2 = "Test Session";
			int serverID2 = 0;
			int worldID2 = 0;
			int numSlots2 = 8;
			string password = sessionPassword;
			string ps4SessionStatus2 = "Toolkit Sample Session";
			ErrorHandler(Matching.CreateSession(name2, serverID2, worldID2, numSlots2, password, SignallingType | Matching.FlagSessionCreate.CREATE_PASSWORD_SESSION, Matching.EnumSessionType.SESSION_TYPE_PRIVATE, ps4SessionStatus2));
		}
		if (menuSession.AddItem("Create & Join Friend Session", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Creating Friend session...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.name = "APP_VERSION";
			sessionAttribute3.intValue = appVersion;
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.name = "PASSWORD";
			sessionAttribute3.binValue = "YES";
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.name = "CAR_TYPE";
			sessionAttribute3.binValue = "CATMOB";
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.name = "LEVEL";
			sessionAttribute3.intValue = 1;
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.name = "RACE_TRACK";
			sessionAttribute3.binValue = "TURKEY";
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.name = "GAME_DETAILS";
			sessionAttribute3.intValue = gameDetails;
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.name = "TEST_BIN_SEARCH";
			sessionAttribute3.binValue = "BIN_VALUE";
			Matching.AddSessionAttribute(sessionAttribute3);
			string name3 = "Test Session";
			int serverID3 = 0;
			int worldID3 = 0;
			int numSlots3 = 8;
			int friendSlots = 8;
			string password2 = sessionPassword;
			string ps4SessionStatus3 = "Toolkit Sample Session";
			ErrorHandler(Matching.CreateFriendsSession(name3, serverID3, worldID3, numSlots3, friendSlots, password2, SignallingType | Matching.FlagSessionCreate.CREATE_PASSWORD_SESSION, ps4SessionStatus3));
		}
		if (menuSession.AddItem("Find Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute4 = new Matching.SessionAttribute();
			sessionAttribute4.name = "APP_VERSION";
			sessionAttribute4.intValue = appVersion;
			sessionAttribute4.searchOperator = Matching.EnumSearchOperators.MATCHING_OPERATOR_EQ;
			Matching.AddSessionAttribute(sessionAttribute4);
			int serverID4 = 0;
			int worldID4 = 0;
			ErrorHandler(Matching.FindSession(serverID4, worldID4));
		}
		if (menuSession.AddItem("Find Sessions (bin search)", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute5 = new Matching.SessionAttribute();
			sessionAttribute5.name = "TEST_BIN_SEARCH";
			sessionAttribute5.binValue = "BIN_VALUE";
			sessionAttribute5.searchOperator = Matching.EnumSearchOperators.MATCHING_OPERATOR_EQ;
			Matching.AddSessionAttribute(sessionAttribute5);
			int serverID5 = 0;
			int worldID5 = 0;
			ErrorHandler(Matching.FindSession(serverID5, worldID5, Matching.FlagSessionSearch.SEARCH_REGIONAL_SESSIONS));
		}
		if (menuSession.AddItem("Find Friend Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding friend sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute6 = new Matching.SessionAttribute();
			sessionAttribute6.name = "APP_VERSION";
			sessionAttribute6.intValue = appVersion;
			sessionAttribute6.searchOperator = Matching.EnumSearchOperators.MATCHING_OPERATOR_EQ;
			Matching.AddSessionAttribute(sessionAttribute6);
			int serverID6 = 0;
			int worldID6 = 0;
			ErrorHandler(Matching.FindSession(serverID6, worldID6, Matching.FlagSessionSearch.SEARCH_FRIENDS_SESSIONS));
		}
		if (menuSession.AddItem("Find Regional Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding friend sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute7 = new Matching.SessionAttribute();
			sessionAttribute7.name = "APP_VERSION";
			sessionAttribute7.intValue = appVersion;
			sessionAttribute7.searchOperator = Matching.EnumSearchOperators.MATCHING_OPERATOR_EQ;
			Matching.AddSessionAttribute(sessionAttribute7);
			int serverID7 = 0;
			int worldID7 = 0;
			ErrorHandler(Matching.FindSession(serverID7, worldID7, Matching.FlagSessionSearch.SEARCH_REGIONAL_SESSIONS));
		}
		if (menuSession.AddItem("Find Random Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding sessions in a random order...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute8 = new Matching.SessionAttribute();
			sessionAttribute8.name = "APP_VERSION";
			sessionAttribute8.intValue = appVersion;
			sessionAttribute8.searchOperator = Matching.EnumSearchOperators.MATCHING_OPERATOR_EQ;
			Matching.AddSessionAttribute(sessionAttribute8);
			int serverID8 = 0;
			int worldID8 = 0;
			ErrorHandler(Matching.FindSession(serverID8, worldID8, Matching.FlagSessionSearch.SEARCH_RANDOM_SESSIONS));
		}
		bool flag = availableSessions != null && availableSessions.Length > 0;
		if (menuSession.AddItem("Join 1st Found Session", isSignedInPSN && flag && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Joining PSN session: " + availableSessions[0].sessionInfo.sessionName);
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute9 = new Matching.SessionAttribute();
			sessionAttribute9.name = "CAR_TYPE";
			sessionAttribute9.binValue = "CATMOB";
			Matching.AddSessionAttribute(sessionAttribute9);
			ErrorHandler(Matching.JoinSession(availableSessions[0].sessionInfo.sessionID, sessionPassword));
		}
		if (menuSession.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	public void MenuInSession(MenuStack menuStack)
	{
		bool isSignedInPSN = User.IsSignedInPSN;
		bool inSession = Matching.InSession;
		bool sessionIsBusy = Matching.SessionIsBusy;
		bool isHost = Matching.IsHost;
		MenuLayout menuLayout = (!isHost) ? menuInSessionClient : menuInSessionHosting;
		menuLayout.Update();
		if (isHost && menuLayout.AddItem("Modify Session", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Modifying session...");
			Matching.ClearModifySessionAttributes();
			gameDetails += 100;
			Matching.ModifySessionAttribute modifySessionAttribute = new Matching.ModifySessionAttribute();
			modifySessionAttribute.name = "GAME_DETAILS";
			modifySessionAttribute.intValue = gameDetails;
			Matching.AddModifySessionAttribute(modifySessionAttribute);
			ErrorHandler(Matching.ModifySession(Matching.EnumAttributeType.SESSION_INTERNAL_ATTRIBUTE));
		}
		if (menuLayout.AddItem("Modify Member Attribute", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Modifying Member Attribute...");
			Matching.ClearModifySessionAttributes();
			Matching.ModifySessionAttribute modifySessionAttribute2 = new Matching.ModifySessionAttribute();
			modifySessionAttribute2.name = "CAR_TYPE";
			cartype++;
			if (cartype > 3)
			{
				cartype = 0;
			}
			switch (cartype)
			{
			case 0:
				modifySessionAttribute2.binValue = "CATMOB";
				break;
			case 1:
				modifySessionAttribute2.binValue = "CARTYPE1";
				break;
			case 2:
				modifySessionAttribute2.binValue = "CARTYPE2";
				break;
			case 3:
				modifySessionAttribute2.binValue = "CARTYPE3";
				break;
			}
			modifySessionAttribute2.intValue = gameDetails;
			Matching.AddModifySessionAttribute(modifySessionAttribute2);
			ErrorHandler(Matching.ModifySession(Matching.EnumAttributeType.SESSION_MEMBER_ATTRIBUTE));
		}
		if (!sendingData)
		{
			if (menuLayout.AddItem("Start Sending Data", isSignedInPSN && inSession && !sessionIsBusy))
			{
				sendingData = true;
			}
		}
		else if (menuLayout.AddItem("Stop Sending Data", isSignedInPSN && inSession && !sessionIsBusy))
		{
			sendingData = false;
		}
		if (menuLayout.AddItem("Leave Session", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Leaving session...");
			ErrorHandler(Matching.LeaveSession());
		}
		if (menuLayout.AddItem("List session members", isSignedInPSN && inSession && !sessionIsBusy))
		{
			Matching.Session session = Matching.GetSession();
			Matching.SessionMemberInfo[] members = session.members;
			for (int i = 0; i < members.Length; i++)
			{
				Matching.SessionMemberInfo sessionMemberInfo = members[i];
				string msg = i + "/memberId:" + sessionMemberInfo.memberId + "/memberFlag:" + sessionMemberInfo.memberFlag + "/addr:" + sessionMemberInfo.addr + "/natType:" + sessionMemberInfo.natType + "/port:" + sessionMemberInfo.port;
				OnScreenLog.Add(msg);
			}
		}
		if (menuLayout.AddItem("Invite Friend", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Invite Friend...");
			ErrorHandler(Matching.InviteToSession("Invite Test", 8));
		}
		if (menuLayout.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
		if (Matching.IsHost)
		{
			NetworkPlayer[] connections = Network.connections;
			GUI.Label(new Rect(Screen.width - 200, Screen.height - 200, 200f, 64f), connections.Length.ToString());
		}
	}

	private Matching.SessionMemberInfo? FindHostMember(Matching.Session session)
	{
		Matching.SessionMemberInfo[] members = session.members;
		for (int i = 0; i < members.Length; i++)
		{
			if ((members[i].memberFlag & Matching.FlagMemberType.MEMBER_OWNER) != 0)
			{
				return members[i];
			}
		}
		return null;
	}

	private Matching.SessionMemberInfo? FindSelfMember(Matching.Session session)
	{
		Matching.SessionMemberInfo[] members = session.members;
		for (int i = 0; i < members.Length; i++)
		{
			if ((members[i].memberFlag & Matching.FlagMemberType.MEMBER_MYSELF) != 0)
			{
				return members[i];
			}
		}
		return null;
	}

	private bool InitializeHostAndSelf(Matching.Session session)
	{
		host = FindHostMember(session);
		Matching.SessionMemberInfo? sessionMemberInfo = host;
		if (!sessionMemberInfo.HasValue)
		{
			OnScreenLog.Add("Host member not found!");
			return false;
		}
		myself = FindSelfMember(session);
		Matching.SessionMemberInfo? sessionMemberInfo2 = myself;
		if (!sessionMemberInfo2.HasValue)
		{
			OnScreenLog.Add("Self member not found!");
			return false;
		}
		return true;
	}

	private void OnMatchingFoundSessions(Messages.PluginMessage msg)
	{
		Matching.Session[] foundSessionList = Matching.GetFoundSessionList();
		OnScreenLog.Add("Found " + foundSessionList.Length + " sessions");
		for (int i = 0; i < foundSessionList.Length; i++)
		{
			DumpSessionInfo(foundSessionList[i]);
		}
		availableSessions = foundSessionList;
	}

	private string IntIPToIPString(int ip)
	{
		int num = ip & 0xFF;
		int num2 = (ip >> 8) & 0xFF;
		int num3 = (ip >> 16) & 0xFF;
		int num4 = (ip >> 24) & 0xFF;
		return num.ToString() + "." + num2.ToString() + "." + num3.ToString() + "." + num4.ToString();
	}

	private void DumpSessionInfo(Matching.Session session)
	{
		Matching.SessionInfo sessionInfo = session.sessionInfo;
		Matching.SessionAttributeInfo[] sessionAttributes = session.sessionAttributes;
		Matching.SessionMemberInfo[] members = session.members;
		OnScreenLog.Add("session: " + sessionInfo.sessionName + ", " + sessionInfo.numMembers + ", " + sessionInfo.maxMembers + ", " + sessionInfo.openSlots + ", " + sessionInfo.reservedSlots + ", " + sessionInfo.worldId + ", " + sessionInfo.roomId);
		for (int i = 0; i < sessionAttributes.Length; i++)
		{
			string text = " Attribute " + i + ": " + sessionAttributes[i].attributeName;
			switch (sessionAttributes[i].attributeValueType)
			{
			case Matching.EnumAttributeValueType.SESSION_ATTRIBUTE_VALUE_INT:
				text = text + " = " + sessionAttributes[i].attributeIntValue;
				break;
			case Matching.EnumAttributeValueType.SESSION_ATTRIBUTE_VALUE_BINARY:
				text = text + " = " + sessionAttributes[i].attributeBinValue;
				break;
			default:
				text += ", has bad value type";
				break;
			}
			text = text + ", " + sessionAttributes[i].attributeType;
			OnScreenLog.Add(text);
		}
		if (members == null)
		{
			return;
		}
		for (int j = 0; j < members.Length; j++)
		{
			OnScreenLog.Add(" Member " + j + ": " + members[j].npOnlineID + ", Type: " + members[j].memberFlag);
			if (members[j].addr != 0)
			{
				OnScreenLog.Add("  IP: " + IntIPToIPString(members[j].addr) + " port " + members[j].port + " 0x" + members[j].port.ToString("X"));
			}
			else
			{
				OnScreenLog.Add("  IP: unknown ");
			}
			sessionAttributes = session.memberAttributes[j];
			if (sessionAttributes.Length == 0)
			{
				OnScreenLog.Add("  No Member Attributes");
			}
			for (int k = 0; k < sessionAttributes.Length; k++)
			{
				string text2 = "  Attribute " + k + ": " + sessionAttributes[k].attributeName;
				switch (sessionAttributes[k].attributeValueType)
				{
				case Matching.EnumAttributeValueType.SESSION_ATTRIBUTE_VALUE_INT:
					text2 = text2 + " = " + sessionAttributes[k].attributeIntValue;
					break;
				case Matching.EnumAttributeValueType.SESSION_ATTRIBUTE_VALUE_BINARY:
					text2 = text2 + " = " + sessionAttributes[k].attributeBinValue;
					break;
				default:
					text2 += ", has bad value type";
					break;
				}
				OnScreenLog.Add(text2);
			}
		}
	}

	private void OnMatchingCreatedSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Created session...");
		Matching.Session session = Matching.GetSession();
		DumpSessionInfo(session);
		if (!InitializeHostAndSelf(session))
		{
			OnScreenLog.Add("ERROR: Expected members not found!");
		}
		NetworkConnectionError networkConnectionError = Network.InitializeServer(serverMaxConnections, serverPort, false);
		if (networkConnectionError != 0)
		{
			OnScreenLog.Add("Server err: " + networkConnectionError);
		}
		else
		{
			OnScreenLog.Add("Started Server");
		}
	}

	private void OnMatchingJoinedSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Joined PSN matching session... waiting on session info in OnMatchingUpdatedSession()");
	}

	private void OnMatchingUpdatedSession(Messages.PluginMessage msg)
	{
		Matching.GetSessionInformationPtr();
		OnScreenLog.Add("Session info updated...");
		Matching.Session session = Matching.GetSession();
		DumpSessionInfo(session);
		if (!InitializeHostAndSelf(session))
		{
			OnScreenLog.Add("ERROR: Expected members not found!");
		}
		if (Matching.IsHost)
		{
			return;
		}
		Matching.SessionMemberInfo? sessionMemberInfo = connected;
		if (sessionMemberInfo.HasValue)
		{
			return;
		}
		Matching.SessionMemberInfo value = host.Value;
		if (value.addr == 0)
		{
			OnScreenLog.Add("Unable to retrieve host IP address");
			ErrorHandler(Matching.LeaveSession());
			return;
		}
		Matching.SessionMemberInfo value2 = host.Value;
		string text = IntIPToIPString(value2.addr);
		object[] obj = new object[6]
		{
			"Connecting to ",
			text,
			":",
			serverPort,
			" using signalling port:",
			null
		};
		Matching.SessionMemberInfo value3 = host.Value;
		obj[5] = value3.port;
		OnScreenLog.Add(string.Concat(obj));
		NetworkConnectionError networkConnectionError = Network.Connect(text, serverPort);
		if (networkConnectionError != 0)
		{
			OnScreenLog.Add("Connection failed: " + networkConnectionError);
			return;
		}
		OnScreenLog.Add("Connected to host " + text + " : " + serverPort);
		connected = host;
	}

	private void OnMatchingJoinInvalidSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Failed to join session...");
		OnScreenLog.Add(" Session search results may be stale.");
	}

	private void OnMatchingLeftSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Left the session");
		Network.Disconnect(1);
		host = null;
		connected = null;
		myself = null;
	}

	private void OnMatchingSessionDestroyed(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Session Destroyed");
		Network.Disconnect(1);
		host = null;
		connected = null;
		myself = null;
	}

	private void OnMatchingKickedOut(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Kicked out of session");
		Network.Disconnect(1);
		host = null;
		connected = null;
		myself = null;
	}

	private void OnSessionError(Messages.PluginMessage msg)
	{
		ErrorHandler();
	}
}
