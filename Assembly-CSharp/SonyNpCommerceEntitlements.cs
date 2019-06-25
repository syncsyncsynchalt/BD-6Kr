using Sony.NP;

public class SonyNpCommerceEntitlements : IScreen
{
	private MenuLayout menu;

	public SonyNpCommerceEntitlements()
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
		Commerce.OnGotEntitlementList += OnGotEntitlementList;
		Commerce.OnConsumedEntitlement += OnConsumedEntitlement;
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
		if (menu.AddItem("Get Entitlement List", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestEntitlementList());
		}
		if (menu.AddItem("Consume Entitlement", enabled))
		{
			Commerce.CommerceEntitlement[] entitlementList = Commerce.GetEntitlementList();
			if (entitlementList.Length > 0)
			{
				SonyNpCommerce.ErrorHandler(Commerce.ConsumeEntitlement(entitlementList[0].id, entitlementList[0].remainingCount));
			}
		}
		if (menu.AddBackIndex("Back"))
		{
			stack.PopMenu();
		}
	}

	private void OnGotEntitlementList(Messages.PluginMessage msg)
	{
		Commerce.CommerceEntitlement[] entitlementList = Commerce.GetEntitlementList();
		OnScreenLog.Add("Got Entitlement List, ");
		if (entitlementList.Length > 0)
		{
			Commerce.CommerceEntitlement[] array = entitlementList;
			for (int i = 0; i < array.Length; i++)
			{
				Commerce.CommerceEntitlement commerceEntitlement = array[i];
				OnScreenLog.Add(" " + commerceEntitlement.id + " rc: " + commerceEntitlement.remainingCount + " cc: " + commerceEntitlement.consumedCount + " type: " + commerceEntitlement.type);
			}
		}
		else
		{
			OnScreenLog.Add("You do not have any entitlements.");
		}
	}

	private void OnConsumedEntitlement(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Consumed Entitlement");
	}
}
