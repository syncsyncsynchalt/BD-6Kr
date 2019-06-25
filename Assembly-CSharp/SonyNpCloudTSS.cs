using Sony.NP;

public class SonyNpCloudTSS : IScreen
{
	private MenuLayout menuTss;

	public SonyNpCloudTSS()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuTss;
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
			TusTss.GetLastError(out result);
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
		MenuTss(stack);
	}

	public void Initialize()
	{
		menuTss = new MenuLayout(this, 550, 34);
		TusTss.OnTssDataRecieved += OnGotTssData;
		TusTss.OnTssNoData += OnSomeEvent;
		TusTss.OnTusTssError += OnTusTssError;
	}

	public void MenuTss(MenuStack menuStack)
	{
		menuTss.Update();
		bool enabled = User.IsSignedInPSN && !TusTss.IsTssBusy();
		if (menuTss.AddItem("TSS Request Data", enabled))
		{
			ErrorHandler(TusTss.RequestTssData());
		}
		if (menuTss.AddItem("TSS Request Data from slot", enabled))
		{
			int slot = 1;
			ErrorHandler(TusTss.RequestTssDataFromSlot(slot));
		}
		if (menuTss.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnTusTssError(Messages.PluginMessage msg)
	{
		ErrorHandler();
	}

	private void OnGotTssData(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got TSS Data");
		byte[] tssData = TusTss.GetTssData();
		OnScreenLog.Add(" Data size: " + tssData.Length);
		string text = string.Empty;
		for (int i = 0; i < 16 && i < tssData.Length; i++)
		{
			text = text + tssData[i].ToString() + ", ";
		}
		OnScreenLog.Add(" Data: " + text);
	}
}
