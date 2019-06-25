using Sony.NP;
using System.Text;

public class SonyNpUser : IScreen
{
	private MenuLayout menuUser;

	private string remoteOnlineID = "Q-ZLqkCtBK-GB-EN";

	private byte[] remoteNpID;

	private string[] sUserColors = new string[4]
	{
		"BLUE",
		"RED",
		"GREEN",
		"PINK"
	};

	public SonyNpUser()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuUser;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		MenuUser(stack);
	}

	public void Initialize()
	{
		menuUser = new MenuLayout(this, 560, 34);
		User.OnGotUserProfile += OnUserGotProfile;
		User.OnGotRemoteUserNpID += OnGotRemoteUserNpID;
		User.OnGotRemoteUserProfile += OnGotRemoteUserProfile;
		User.OnUserProfileError += OnUserProfileError;
	}

	public void MenuUser(MenuStack menuStack)
	{
		bool isSignedInPSN = User.IsSignedInPSN;
		menuUser.Update();
		if (menuUser.AddItem("Get My Profile", !User.IsUserProfileBusy) && User.RequestUserProfile() != 0)
		{
			ResultCode result = default(ResultCode);
			User.GetLastUserProfileError(out result);
			OnScreenLog.Add(result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
		}
		if (menuUser.AddItem("Get Remote Profile (onlineID)", isSignedInPSN && !User.IsUserProfileBusy) && User.RequestRemoteUserProfileForOnlineID(remoteOnlineID) != 0)
		{
			ResultCode result2 = default(ResultCode);
			User.GetLastUserProfileError(out result2);
			OnScreenLog.Add(result2.className + ": " + result2.lastError + ", sce error 0x" + result2.lastErrorSCE.ToString("X8"));
		}
		if (menuUser.AddItem("Get Remote NpID", isSignedInPSN && !User.IsUserProfileBusy) && User.RequestRemoteUserNpID(remoteOnlineID) != 0)
		{
			ResultCode result3 = default(ResultCode);
			User.GetLastUserProfileError(out result3);
			OnScreenLog.Add(result3.className + ": " + result3.lastError + ", sce error 0x" + result3.lastErrorSCE.ToString("X8"));
		}
		if (menuUser.AddItem("Get Remote Profile (npID)", remoteNpID != null && isSignedInPSN && !User.IsUserProfileBusy) && User.RequestRemoteUserProfileForNpID(remoteNpID) != 0)
		{
			ResultCode result4 = default(ResultCode);
			User.GetLastUserProfileError(out result4);
			OnScreenLog.Add(result4.className + ": " + result4.lastError + ", sce error 0x" + result4.lastErrorSCE.ToString("X8"));
		}
		if (menuUser.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnUserGotProfile(Messages.PluginMessage msg)
	{
		User.UserProfile cachedUserProfile = User.GetCachedUserProfile();
		OnScreenLog.Add(msg.ToString());
		OnScreenLog.Add(" OnlineID: " + cachedUserProfile.onlineID);
		string @string = Encoding.Default.GetString(cachedUserProfile.npID);
		OnScreenLog.Add(" NpID: " + @string);
		OnScreenLog.Add(" Avatar URL: " + cachedUserProfile.avatarURL);
		OnScreenLog.Add(" Country Code: " + cachedUserProfile.countryCode);
		OnScreenLog.Add(" Language: " + cachedUserProfile.language);
		OnScreenLog.Add(" Age: " + cachedUserProfile.age);
		OnScreenLog.Add(" Chat Restrict: " + cachedUserProfile.chatRestricted);
		OnScreenLog.Add(" Content Restrict: " + cachedUserProfile.contentRestricted);
		SonyNpMain.SetAvatarURL(cachedUserProfile.avatarURL, 0);
	}

	private void OnGotRemoteUserNpID(Messages.PluginMessage msg)
	{
		remoteNpID = User.GetCachedRemoteUserNpID();
		string @string = Encoding.Default.GetString(remoteNpID);
		OnScreenLog.Add("Got Remote User NpID: " + @string);
	}

	private void OnGotRemoteUserProfile(Messages.PluginMessage msg)
	{
		User.RemoteUserProfile cachedRemoteUserProfile = User.GetCachedRemoteUserProfile();
		OnScreenLog.Add("Got Remote User Profile");
		OnScreenLog.Add(" OnlineID: " + cachedRemoteUserProfile.onlineID);
		string @string = Encoding.Default.GetString(cachedRemoteUserProfile.npID);
		OnScreenLog.Add(" NpID: " + @string);
		OnScreenLog.Add(" Avatar URL: " + cachedRemoteUserProfile.avatarURL);
		OnScreenLog.Add(" Country Code: " + cachedRemoteUserProfile.countryCode);
		OnScreenLog.Add(" Language: " + cachedRemoteUserProfile.language);
		SonyNpMain.SetAvatarURL(cachedRemoteUserProfile.avatarURL, 1);
	}

	private void OnUserProfileError(Messages.PluginMessage msg)
	{
		ResultCode result = default(ResultCode);
		User.GetLastUserProfileError(out result);
		OnScreenLog.Add(result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
	}
}
