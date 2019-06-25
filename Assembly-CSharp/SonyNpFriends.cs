using Sony.NP;
using System.Text;
using UnityEngine;

public class SonyNpFriends : IScreen
{
	private MenuLayout menuFriends;

	public SonyNpFriends()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuFriends;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	private ErrorCode ErrorHandlerFriends(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Friends.GetLastError(out result);
			if (result.lastError != 0)
			{
				OnScreenLog.Add("Error: " + result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
				return result.lastError;
			}
		}
		return errorCode;
	}

	private ErrorCode ErrorHandlerPresence(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			User.GetLastPresenceError(out result);
			if (result.lastError != 0)
			{
				OnScreenLog.Add("Error: " + result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
				return result.lastError;
			}
		}
		return errorCode;
	}

	private ErrorCode ErrorHandlerTwitter(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Twitter.GetLastError(out result);
			if (result.lastError != 0)
			{
				OnScreenLog.Add("Error: " + result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
				return result.lastError;
			}
		}
		return errorCode;
	}

	private ErrorCode ErrorHandlerFacebook(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Facebook.GetLastError(out result);
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
		MenuFriends(stack);
	}

	public void Initialize()
	{
		menuFriends = new MenuLayout(this, 450, 34);
		Friends.OnFriendsListUpdated += OnFriendsListUpdated;
		Friends.OnFriendsPresenceUpdated += OnFriendsListUpdated;
		Friends.OnGotFriendsList += OnFriendsGotList;
		Friends.OnFriendsListError += OnFriendsListError;
		User.OnPresenceSet += OnSomeEvent;
		User.OnPresenceError += OnPresenceError;
		Facebook.OnFacebookDialogStarted += OnSomeEvent;
		Facebook.OnFacebookDialogFinished += OnSomeEvent;
		Facebook.OnFacebookMessagePosted += OnSomeEvent;
		Facebook.OnFacebookMessagePostFailed += OnFacebookMessagePostFailed;
		Twitter.OnTwitterDialogStarted += OnSomeEvent;
		Twitter.OnTwitterDialogCanceled += OnSomeEvent;
		Twitter.OnTwitterDialogFinished += OnSomeEvent;
		Twitter.OnTwitterMessagePosted += OnSomeEvent;
		Twitter.OnTwitterMessagePostFailed += OnTwitterMessagePostFailed;
	}

	public void MenuFriends(MenuStack menuStack)
	{
		menuFriends.Update();
		if (menuFriends.AddItem("Friends", User.IsSignedInPSN && !Friends.FriendsListIsBusy()))
		{
			ErrorHandlerFriends(Friends.RequestFriendsList());
		}
		if (menuFriends.AddItem("Set Presence", User.IsSignedInPSN && !User.OnlinePresenceIsBusy()))
		{
			ErrorHandlerPresence(User.SetOnlinePresence("Testing UnityNpToolkit"));
		}
		if (menuFriends.AddItem("Clear Presence", User.IsSignedInPSN && !User.OnlinePresenceIsBusy()))
		{
			ErrorHandlerPresence(User.SetOnlinePresence(string.Empty));
		}
		if (menuFriends.AddItem("Post On Facebook", User.IsSignedInPSN && !Facebook.IsBusy()))
		{
			Facebook.PostFacebook message = default(Facebook.PostFacebook);
			message.appID = 701792156521339L;
			message.userText = "I'm testing Unity's facebook integration !";
			message.photoURL = "http://uk.playstation.com/media/RZXT_744/159/PlayStationNetworkFeaturedImage.jpg";
			message.photoTitle = "Title";
			message.photoCaption = "This is the caption";
			message.photoDescription = "This is the description";
			message.actionLinkName = "Go To Unity3d.com";
			message.actionLinkURL = "http://unity3d.com/";
			ErrorHandlerFacebook(Facebook.PostMessage(message));
		}
		if (menuFriends.AddItem("Post On Twitter", User.IsSignedInPSN && !Twitter.IsBusy()))
		{
			Twitter.PostTwitter message2 = default(Twitter.PostTwitter);
			message2.userText = "I'm testing Unity's Twitter integration !";
			message2.imagePath = Application.streamingAssetsPath + "/TweetUnity.png";
			message2.forbidAttachPhoto = false;
			message2.disableEditTweetMsg = true;
			message2.forbidOnlyImageTweet = false;
			message2.forbidNoImageTweet = false;
			message2.disableChangeImage = false;
			message2.limitToScreenShot = true;
			ErrorHandlerTwitter(Twitter.PostMessage(message2));
		}
		if (menuFriends.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnFacebookMessagePostFailed(Messages.PluginMessage msg)
	{
		ErrorHandlerFacebook();
	}

	private void OnTwitterMessagePostFailed(Messages.PluginMessage msg)
	{
		ErrorHandlerTwitter();
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnFriendsListUpdated(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Requesting Friends List for Event: " + msg.type);
		ErrorHandlerFriends(Friends.RequestFriendsList());
	}

	private string OnlinePresenceType(Friends.EnumNpPresenceType type)
	{
		switch (type)
		{
		case Friends.EnumNpPresenceType.IN_GAME_PRESENCE_TYPE_UNKNOWN:
			return "unknown";
		case Friends.EnumNpPresenceType.IN_GAME_PRESENCE_TYPE_NONE:
			return "none";
		case Friends.EnumNpPresenceType.IN_GAME_PRESENCE_TYPE_DEFAULT:
			return "default";
		case Friends.EnumNpPresenceType.IN_GAME_PRESENCE_TYPE_GAME_JOINING:
			return "joining";
		case Friends.EnumNpPresenceType.IN_GAME_PRESENCE_TYPE_GAME_JOINING_ONLY_FOR_PARTY:
			return "joining party";
		case Friends.EnumNpPresenceType.IN_GAME_PRESENCE_TYPE_JOIN_GAME_ACK:
			return "join ack";
		default:
			return "unknown";
		}
	}

	private string OnlineStatus(Friends.EnumNpOnlineStatus status)
	{
		switch (status)
		{
		case Friends.EnumNpOnlineStatus.ONLINE_STATUS_OFFLINE:
			return "offline";
		case Friends.EnumNpOnlineStatus.ONLINE_STATUS_AFK:
			return "afk";
		case Friends.EnumNpOnlineStatus.ONLINE_STATUS_ONLINE:
			return "online";
		default:
			return "unknown";
		}
	}

	private void OnFriendsGotList(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Friends List!");
		Friends.Friend[] cachedFriendsList = Friends.GetCachedFriendsList();
		Friends.Friend[] array = cachedFriendsList;
		for (int i = 0; i < array.Length; i++)
		{
			Friends.Friend friend = array[i];
			string @string = Encoding.Default.GetString(friend.npID);
			OnScreenLog.Add(friend.npOnlineID + ", np(" + @string + "), os(" + OnlineStatus(friend.npOnlineStatus) + "), pt(" + OnlinePresenceType(friend.npPresenceType) + "), prsc(" + friend.npPresenceTitle + ", " + friend.npPresenceStatus + ")," + friend.npComment);
		}
	}

	private void OnFriendsListError(Messages.PluginMessage msg)
	{
		ErrorHandlerFriends();
	}

	private void OnPresenceError(Messages.PluginMessage msg)
	{
		ErrorHandlerPresence();
	}
}
