using Sony.NP;

public class SonyNpDialogs : IScreen
{
	private MenuLayout menu;

	public SonyNpDialogs()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menu;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	private ErrorCode ErrorHandler(ErrorCode errorCode)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Dialogs.GetLastError(out result);
			OnScreenLog.Add("Error: " + result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
		}
		return errorCode;
	}

	public void Process(MenuStack stack)
	{
		menu.Update();
		bool enabled = User.IsSignedInPSN && !Dialogs.IsDialogOpen;
		if (menu.AddItem("Friends Dialog", enabled))
		{
			ErrorHandler(Dialogs.FriendsList());
		}
		if (menu.AddItem("Shared History Dialog", enabled))
		{
			ErrorHandler(Dialogs.SharedPlayHistory());
		}
		if (menu.AddItem("Profile Dialog", enabled))
		{
			ErrorHandler(Dialogs.Profile(User.GetCachedUserProfile().npID));
		}
		if (menu.AddBackIndex("Back"))
		{
			stack.PopMenu();
		}
	}

	public void Initialize()
	{
		menu = new MenuLayout(this, 450, 34);
		Dialogs.OnDlgFriendsListClosed += OnFriendDialogClosed;
		Dialogs.OnDlgSharedPlayHistoryClosed += OnSharedPlayHistoryDialogClosed;
		Dialogs.OnDlgProfileClosed += OnProfileDialogClosed;
		Dialogs.OnDlgCommerceClosed += OnCommerceDialogClosed;
	}

	private void OnFriendDialogClosed(Messages.PluginMessage msg)
	{
		Dialogs.NpDialogReturn dialogResult = Dialogs.GetDialogResult();
		OnScreenLog.Add("Friends Dialog closed with result: " + dialogResult.result);
		if (dialogResult.result == Dialogs.EnumNpDlgResult.NP_DLG_OK)
		{
			Dialogs.Profile(dialogResult.npID);
		}
	}

	private void OnSharedPlayHistoryDialogClosed(Messages.PluginMessage msg)
	{
		Dialogs.NpDialogReturn dialogResult = Dialogs.GetDialogResult();
		OnScreenLog.Add("Shared play history dialog closed with result: " + dialogResult.result);
		if (dialogResult.result == Dialogs.EnumNpDlgResult.NP_DLG_OK)
		{
			Dialogs.Profile(dialogResult.npID);
		}
	}

	private void OnProfileDialogClosed(Messages.PluginMessage msg)
	{
		Dialogs.NpDialogReturn dialogResult = Dialogs.GetDialogResult();
		OnScreenLog.Add("Profile dialog closed with result: " + dialogResult.result);
	}

	private void OnCommerceDialogClosed(Messages.PluginMessage msg)
	{
		Dialogs.NpDialogReturn dialogResult = Dialogs.GetDialogResult();
		OnScreenLog.Add("Commerce dialog closed with result: " + dialogResult.result + " PlusAllowed:" + dialogResult.plusAllowed);
	}
}
