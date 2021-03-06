public class SonyNpCloud : IScreen
{
	private MenuLayout menuCloud;

	private SonyNpCloudTUS tus;

	private SonyNpCloudTSS tss;

	public SonyNpCloud()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuCloud;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		MenuCloud(stack);
	}

	public void Initialize()
	{
		menuCloud = new MenuLayout(this, 550, 34);
		tus = new SonyNpCloudTUS();
		tss = new SonyNpCloudTSS();
	}

	public void MenuCloud(MenuStack stack)
	{
		menuCloud.Update();
		if (menuCloud.AddItem("Title User Storage"))
		{
			stack.PushMenu(tus.GetMenu());
		}
		if (menuCloud.AddItem("Title Small Storage"))
		{
			stack.PushMenu(tss.GetMenu());
		}
		if (menuCloud.AddBackIndex("Back"))
		{
			stack.PopMenu();
		}
	}
}
