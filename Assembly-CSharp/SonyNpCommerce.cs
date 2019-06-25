using Sony.NP;
using System.IO;
using UnityEngine;
using UnityEngine.PSVita;

public class SonyNpCommerce : IScreen
{
	private MenuLayout menu;

	private SonyNpCommerceEntitlements entitlements;

	private SonyNpCommerceStore store;

	private SonyNpCommerceInGameStore inGameStore;

	public SonyNpCommerce()
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
		store = new SonyNpCommerceStore();
		inGameStore = new SonyNpCommerceInGameStore();
		entitlements = new SonyNpCommerceEntitlements();
		Commerce.OnError += OnCommerceError;
		Commerce.OnDownloadListStarted += OnSomeEvent;
		Commerce.OnDownloadListFinished += OnSomeEvent;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public static ErrorCode ErrorHandler(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Commerce.GetLastError(out result);
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
		bool enabled = User.IsSignedInPSN && !Commerce.IsBusy();
		menu.Update();
		if (menu.AddItem("Store", enabled))
		{
			stack.PushMenu(store.GetMenu());
		}
		if (menu.AddItem("In Game Store"))
		{
			stack.PushMenu(inGameStore.GetMenu());
		}
		if (menu.AddItem("Downloads"))
		{
			Commerce.DisplayDownloadList();
		}
		if (menu.AddItem("Entitlements"))
		{
			stack.PushMenu(entitlements.GetMenu());
		}
		if (menu.AddItem("Find Installed Content"))
		{
			EnumerateDRMContent();
		}
		if (menu.AddBackIndex("Back"))
		{
			stack.PopMenu();
		}
	}

	private int EnumerateDRMContentFiles(string contentDir)
	{
		int num = 0;
		PSVitaDRM.ContentOpen(contentDir);
		string text = "addcont0:" + contentDir;
		OnScreenLog.Add("Found content folder: " + text);
		string[] files = Directory.GetFiles(text);
		OnScreenLog.Add(" containing " + files.Length + " files");
		string[] array = files;
		foreach (string text2 in array)
		{
			OnScreenLog.Add("  " + text2);
			num++;
			if (text2.Contains(".unity3d"))
			{
				AssetBundle val = AssetBundle.CreateFromFile(text2);
				Object[] array2 = val.LoadAllAssets();
				OnScreenLog.Add("  Loaded " + array2.Length + " assets from asset bundle.");
				val.Unload(false);
			}
		}
		PSVitaDRM.ContentClose(contentDir);
		return num;
	}

	private void EnumerateDRMContent()
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		var val = default(PSVitaDRM.DrmContentFinder);
		val.dirHandle = -1;
		if (PSVitaDRM.ContentFinderOpen(ref val))
		{
			num += EnumerateDRMContentFiles(val.contentDir);
			while (PSVitaDRM.ContentFinderNext(ref val))
			{
				num += EnumerateDRMContentFiles(val.contentDir);
			}
			PSVitaDRM.ContentFinderClose(ref val);
		}
		OnScreenLog.Add("Found " + num + " files in installed DRM content");
	}

	private void OnCommerceError(Messages.PluginMessage msg)
	{
		ErrorHandler();
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}
}
