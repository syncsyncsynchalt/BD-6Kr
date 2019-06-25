using Sony.NP;

public class SonyNpRequests : IScreen
{
	private MenuLayout menu;

	public SonyNpRequests()
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
			OnScreenLog.Add("Error: " + errorCode);
		}
		return errorCode;
	}

	public void Process(MenuStack stack)
	{
		menu.Update();
		if (menu.AddBackIndex("Back"))
		{
			stack.PopMenu();
		}
	}

	public void Initialize()
	{
		menu = new MenuLayout(this, 450, 34);
		Requests.OnCheckPlusResult += OnCheckPlusResult;
		Requests.OnAccountLanguageResult += OnAccountLanguageResult;
		Requests.OnParentalControlResult += OnParentalControlResult;
	}

	private void OnCheckPlusResult(Messages.PluginMessage msg)
	{
		Requests.GetCheckPlusResult(msg, out bool CheckPlusResult, out int userId);
		OnScreenLog.Add("OnPlusCheckResult  returned:" + CheckPlusResult + " userId :0x" + userId.ToString("X"));
	}

	private void OnAccountLanguageResult(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("OnAccountLanguageResult  AccountLanguage:" + Requests.GetAccountLanguageResult(msg) + " OnlineID: " + Requests.GetRequestOnlineId(msg));
	}

	private void OnParentalControlResult(Messages.PluginMessage msg)
	{
		Requests.GetParentalControlInfoResult(msg, out int Age, out bool chatRestriction, out bool ugcRestriction);
		OnScreenLog.Add("OnParentalControlResult  Age:" + Age + " chatRestriction:" + chatRestriction + " ugcRestriction:" + ugcRestriction + " OnlineID: " + Requests.GetRequestOnlineId(msg));
	}
}
