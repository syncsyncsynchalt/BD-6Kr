using Sony.NP;
using System.IO;
using UnityEngine;

public class SonyNpMessaging : IScreen
{
	private class GameInviteData
	{
		public string taunt;

		public int level;

		public int score;

		public byte[] WriteToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream(16);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(taunt);
			binaryWriter.Write(level);
			binaryWriter.Write(score);
			binaryWriter.Close();
			return memoryStream.GetBuffer();
		}

		public void ReadFromBuffer(byte[] buffer)
		{
			MemoryStream input = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(input);
			taunt = binaryReader.ReadString();
			level = binaryReader.ReadInt32();
			score = binaryReader.ReadInt32();
			binaryReader.Close();
		}
	}

	private struct GameData
	{
		public string text;

		public int item1;

		public int item2;

		public byte[] WriteToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
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
			text = binaryReader.ReadString();
			item1 = binaryReader.ReadInt32();
			item2 = binaryReader.ReadInt32();
			binaryReader.Close();
		}
	}

	private MenuLayout menuMessaging;

	public SonyNpMessaging()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuMessaging;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		MenuMessaging(stack);
	}

	public void Initialize()
	{
		menuMessaging = new MenuLayout(this, 500, 34);
		Messaging.OnMessageSent += OnSomeEvent;
		Messaging.OnMessageNotSent += OnSomeEvent;
		Messaging.OnMessageCanceled += OnSomeEvent;
		Messaging.OnCustomDataMessageRetrieved += OnMessagingGotGameMessage;
		Messaging.OnCustomInviteMessageRetrieved += OnMessagingGotGameInvite;
		Messaging.OnInGameDataMessageRetrieved += OnMessagingGotInGameDataMessage;
		Messaging.OnMessageNotSentFreqTooHigh += OnSomeEvent;
		Messaging.OnMessageError += OnMessageError;
	}

	public void MenuMessaging(MenuStack menuStack)
	{
		menuMessaging.Update();
		if (menuMessaging.AddItem("Show Messages & Invites", User.IsSignedInPSN && !Messaging.IsBusy()))
		{
			Messaging.ShowRecievedDataMessageDialog();
		}
		if (menuMessaging.AddItem("Send Session Invite", User.IsSignedInPSN && Matching.InSession))
		{
			string text = "Join my session";
			int npIDCount = 8;
			Matching.InviteToSession(text, npIDCount);
		}
		if (menuMessaging.AddItem("Send Game Invite", User.IsSignedInPSN && !Messaging.IsBusy()))
		{
			GameInviteData gameInviteData = new GameInviteData();
			gameInviteData.taunt = "I got an awesome score, can you do better?";
			gameInviteData.level = 1;
			gameInviteData.score = 123456789;
			byte[] data = gameInviteData.WriteToBuffer();
			Messaging.MsgRequest msgRequest = new Messaging.MsgRequest();
			msgRequest.body = "Game invite";
			msgRequest.expireMinutes = 30;
			msgRequest.data = data;
			msgRequest.npIDCount = 8;
			string dataDescription = "Some data to test invite messages";
			string dataName = "Test data";
			msgRequest.dataDescription = dataDescription;
			msgRequest.dataName = dataName;
			msgRequest.iconPath = Application.streamingAssetsPath + "/PSP2SessionImage.jpg";
			Messaging.SendMessage(msgRequest);
		}
		if (menuMessaging.AddItem("Send Data Message", User.IsSignedInPSN && !Messaging.IsBusy()))
		{
			GameData gameData = default(GameData);
			gameData.text = "Here's some data";
			gameData.item1 = 2;
			gameData.item2 = 987654321;
			byte[] data2 = gameData.WriteToBuffer();
			Messaging.MsgRequest msgRequest2 = new Messaging.MsgRequest();
			msgRequest2.body = "Data message";
			msgRequest2.expireMinutes = 0;
			msgRequest2.data = data2;
			msgRequest2.npIDCount = 8;
			string dataDescription2 = "Some data to test messages";
			string dataName2 = "Test data";
			msgRequest2.dataDescription = dataDescription2;
			msgRequest2.dataName = dataName2;
			msgRequest2.iconPath = Application.streamingAssetsPath + "/PSP2SessionImage.jpg";
			Messaging.SendMessage(msgRequest2);
		}
		if (menuMessaging.AddItem("Send In Game Data (Session)", Matching.InSession && !Messaging.IsBusy()))
		{
			Matching.Session session = Matching.GetSession();
			Matching.SessionMemberInfo[] members = session.members;
			if (members == null)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < members.Length; i++)
			{
				if ((members[i].memberFlag & Matching.FlagMemberType.MEMBER_MYSELF) == 0)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				OnScreenLog.Add("Sending in game data message to " + members[num].npOnlineID);
				GameData gameData2 = default(GameData);
				gameData2.text = "Here's some data";
				gameData2.item1 = 2;
				gameData2.item2 = 987654321;
				byte[] data3 = gameData2.WriteToBuffer();
				Messaging.SendInGameDataMessage(members[num].npID, data3);
			}
			else
			{
				OnScreenLog.Add("No session member to send to.");
			}
		}
		if (menuMessaging.AddItem("Send In Game Message (Friend)", User.IsSignedInPSN && !Messaging.IsBusy()))
		{
			Friends.Friend[] cachedFriendsList = Friends.GetCachedFriendsList();
			if (cachedFriendsList.Length > 0)
			{
				int num2 = 0;
				if (num2 >= 0)
				{
					OnScreenLog.Add("Sending in game data message to " + cachedFriendsList[num2].npOnlineID);
					GameData gameData3 = default(GameData);
					gameData3.text = "Here's some data";
					gameData3.item1 = 2;
					gameData3.item2 = 987654321;
					byte[] data4 = gameData3.WriteToBuffer();
					Messaging.SendInGameDataMessage(cachedFriendsList[num2].npID, data4);
				}
				else
				{
					OnScreenLog.Add("No friends in this context.");
				}
			}
			else
			{
				OnScreenLog.Add("No friends cached.");
				OnScreenLog.Add("refresh the friends list then try again.");
			}
		}
		if (menuMessaging.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnMessagingGotGameInvite(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got game invite...");
		GameInviteData gameInviteData = new GameInviteData();
		byte[] gameInviteAttachment = Messaging.GetGameInviteAttachment();
		gameInviteData.ReadFromBuffer(gameInviteAttachment);
		OnScreenLog.Add(" taunt: " + gameInviteData.taunt);
		OnScreenLog.Add(" level: " + gameInviteData.level);
		OnScreenLog.Add(" score: " + gameInviteData.score);
	}

	private void OnMessagingGotGameMessage(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got message...");
		GameData gameData = default(GameData);
		byte[] messageAttachment = Messaging.GetMessageAttachment();
		gameData.ReadFromBuffer(messageAttachment);
		OnScreenLog.Add(" text: " + gameData.text);
		OnScreenLog.Add(" item1: " + gameData.item1);
		OnScreenLog.Add(" item2: " + gameData.item2);
	}

	private void OnMessagingGotInGameDataMessage(Messages.PluginMessage msg)
	{
		GameData gameData = default(GameData);
		OnScreenLog.Add("Got in-game data message...");
		while (Messaging.InGameDataMessagesRecieved())
		{
			Messaging.InGameDataMessage inGameDataMessage = Messaging.GetInGameDataMessage();
			gameData.ReadFromBuffer(inGameDataMessage.data);
			OnScreenLog.Add(" ID: " + inGameDataMessage.messageID + " text: " + gameData.text + " item1: " + gameData.item1 + " item2: " + gameData.item2);
		}
	}

	private void OnMessageError(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(" OnMessageError error code: " + Messaging.GetErrorFromMessage(msg).ToString("X"));
	}
}
