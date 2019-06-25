using Sony.NP;

public class SonyNpCommerceInGameStore : IScreen
{
	private MenuLayout menu;

	private bool sessionCreated;

	private string testCategoryID = "ED1633-NPXB01864_00-WEAPS_01";

	private string testProductID = "ED1633-NPXB01864_00-A000010000000000";

	private string[] testProductSkuIDs = new string[2]
	{
		"ED1633-NPXB01864_00-A000010000000000-E001",
		"ED1633-NPXB01864_00-A000020000000000-E001"
	};

	public SonyNpCommerceInGameStore()
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
		Commerce.OnSessionCreated += OnSessionCreated;
		Commerce.OnSessionAborted += OnSomeEvent;
		Commerce.OnGotCategoryInfo += OnGotCategoryInfo;
		Commerce.OnGotProductList += OnGotProductList;
		Commerce.OnGotProductInfo += OnGotProductInfo;
		Commerce.OnCheckoutStarted += OnSomeEvent;
		Commerce.OnCheckoutFinished += OnSomeEvent;
	}

	public void CreateSession()
	{
		SonyNpCommerce.ErrorHandler(Commerce.CreateSession());
	}

	public void OnEnter()
	{
		CreateSession();
		Commerce.ShowStoreIcon(Commerce.StoreIconPosition.Center);
	}

	public void OnExit()
	{
		Commerce.HideStoreIcon();
	}

	public void Process(MenuStack stack)
	{
		bool enabled = User.IsSignedInPSN && sessionCreated && !Commerce.IsBusy();
		menu.Update();
		if (menu.AddItem("Category Info", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestCategoryInfo(string.Empty));
		}
		if (menu.AddItem("Product List", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestProductList(testCategoryID));
		}
		if (menu.AddItem("Product Info", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestDetailedProductInfo(testProductID));
		}
		if (menu.AddItem("Browse Product", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.BrowseProduct(testProductID));
		}
		if (menu.AddItem("Checkout", enabled))
		{
			Commerce.GetProductList();
			SonyNpCommerce.ErrorHandler(Commerce.Checkout(testProductSkuIDs));
		}
		if (menu.AddItem("Redeem Voucher", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.VoucherInput());
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

	private void OnSessionCreated(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Commerce Session Created");
		sessionCreated = true;
	}

	private void OnGotCategoryInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Category Info");
		Commerce.CommerceCategoryInfo categoryInfo = Commerce.GetCategoryInfo();
		OnScreenLog.Add("Category Id: " + categoryInfo.categoryId);
		OnScreenLog.Add("Category Name: " + categoryInfo.categoryName);
		OnScreenLog.Add("Category num products: " + categoryInfo.countOfProducts);
		OnScreenLog.Add("Category num sub categories: " + categoryInfo.countOfSubCategories);
		for (int i = 0; i < categoryInfo.countOfSubCategories; i++)
		{
			Commerce.CommerceCategoryInfo subCategoryInfo = Commerce.GetSubCategoryInfo(i);
			OnScreenLog.Add("SubCategory Id: " + subCategoryInfo.categoryId);
			OnScreenLog.Add("SubCategory Name: " + subCategoryInfo.categoryName);
			if (i == 0)
			{
				SonyNpCommerce.ErrorHandler(Commerce.RequestCategoryInfo(subCategoryInfo.categoryId));
			}
		}
	}

	private void OnGotProductList(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Product List");
		Commerce.CommerceProductInfo[] productList = Commerce.GetProductList();
		Commerce.CommerceProductInfo[] array = productList;
		for (int i = 0; i < array.Length; i++)
		{
			Commerce.CommerceProductInfo commerceProductInfo = array[i];
			OnScreenLog.Add("Product: " + commerceProductInfo.productName + " - " + commerceProductInfo.price);
		}
	}

	private void OnGotProductInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Detailed Product Info");
		Commerce.CommerceProductInfoDetailed detailedProductInfo = Commerce.GetDetailedProductInfo();
		OnScreenLog.Add("Product: " + detailedProductInfo.productName + " - " + detailedProductInfo.price);
		OnScreenLog.Add("Long desc: " + detailedProductInfo.longDescription);
	}
}
