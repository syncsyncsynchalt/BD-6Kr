using Sony.NP;

public class SonyNpCommerceStore : IScreen
{
	private MenuLayout menu;

	public SonyNpCommerceStore()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menu;
	}

	public void Initialize()
	{
		menu = new MenuLayout(this, 450, 34);
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		bool enabled = User.IsSignedInPSN && !Commerce.IsBusy();
		menu.Update();
		if (menu.AddItem("Browse Category", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.BrowseCategory(string.Empty));
		}
		if (menu.AddBackIndex("Back"))
		{
			stack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}
}
