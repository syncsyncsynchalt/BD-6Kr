using Sony.NP;
using System;

public class SonyNpUtilities : IScreen
{
	private MenuLayout menu;

	private SonyNpTicketing ticketing;

	private SonyNpDialogs dialogs;

	public SonyNpUtilities()
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

	private ErrorCode ErrorHandlerSystem(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Sony.NP.System.GetLastError(out result);
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
		MenuUtilities(stack);
	}

	public void Initialize()
	{
		menu = new MenuLayout(this, 450, 34);
		ticketing = new SonyNpTicketing();
		dialogs = new SonyNpDialogs();
		Sony.NP.System.OnGotBandwidth += OnSystemGotBandwidth;
		Sony.NP.System.OnGotNetInfo += OnSystemGotNetInfo;
		Sony.NP.System.OnNetInfoError += OnNetInfoError;
		WordFilter.OnCommentCensored += OnWordFilterCensored;
		WordFilter.OnCommentNotCensored += OnWordFilterNotCensored;
		WordFilter.OnCommentSanitized += OnWordFilterSanitized;
		WordFilter.OnWordFilterError += OnWordFilterError;
	}

	public void MenuUtilities(MenuStack menuStack)
	{
		menu.Update();
		if (menu.AddItem("Get Network Time", Sony.NP.System.IsConnected))
		{
			DateTime dateTime = new DateTime(Sony.NP.System.GetNetworkTime(), DateTimeKind.Utc);
			OnScreenLog.Add("networkTime: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		}
		if (menu.AddItem("Bandwidth", Sony.NP.System.IsConnected && !Sony.NP.System.RequestBandwidthInfoIsBusy()))
		{
			ErrorHandlerSystem(Sony.NP.System.RequestBandwidthInfo());
		}
		if (menu.AddItem("Net Info", !Sony.NP.System.RequestBandwidthInfoIsBusy()))
		{
			ErrorHandlerSystem(Sony.NP.System.RequestNetInfo());
		}
		if (menu.AddItem("Net Device Type"))
		{
			Sony.NP.System.NetDeviceType networkDeviceType = Sony.NP.System.GetNetworkDeviceType();
			OnScreenLog.Add("Network device: " + networkDeviceType);
		}
		if (User.IsSignedInPSN)
		{
			if (menu.AddItem("Dialogs"))
			{
				menuStack.PushMenu(dialogs.GetMenu());
			}
			if (menu.AddItem("Auth Ticketing"))
			{
				menuStack.PushMenu(ticketing.GetMenu());
			}
			if (menu.AddItem("Censor Bad Comment", Sony.NP.System.IsConnected && !WordFilter.IsBusy()))
			{
				WordFilter.CensorComment("Censor a shit comment");
			}
			if (menu.AddItem("Sanitize Bad Comment", Sony.NP.System.IsConnected && !WordFilter.IsBusy()))
			{
				WordFilter.SanitizeComment("Sanitize a shit comment");
			}
		}
		if (menu.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSystemGotBandwidth(Messages.PluginMessage msg)
	{
		Sony.NP.System.Bandwidth bandwidthInfo = Sony.NP.System.GetBandwidthInfo();
		OnScreenLog.Add("bandwidth download : " + bandwidthInfo.downloadBPS / 8192f + " KBs");
		OnScreenLog.Add("bandwidth upload : " + bandwidthInfo.uploadBPS / 8192f + " KBs");
	}

	private void OnSystemGotNetInfo(Messages.PluginMessage msg)
	{
		Sony.NP.System.NetInfoBasic netInfo = Sony.NP.System.GetNetInfo();
		OnScreenLog.Add("Got Net info");
		OnScreenLog.Add(" Connection status: " + netInfo.connectionStatus);
		OnScreenLog.Add(" IP address: " + netInfo.ipAddress);
		OnScreenLog.Add(" NAT type: " + netInfo.natType);
		OnScreenLog.Add(" NAT stun status: " + netInfo.natStunStatus);
		OnScreenLog.Add(" NAT mapped addr: 0x" + netInfo.natMappedAddr.ToString("X8"));
	}

	private void OnNetInfoError(Messages.PluginMessage msg)
	{
		ErrorHandlerSystem();
	}

	private void OnWordFilterCensored(Messages.PluginMessage msg)
	{
		WordFilter.FilteredComment result = WordFilter.GetResult();
		OnScreenLog.Add("Censored: changed=" + result.wasChanged + ", comment='" + result.comment + "'");
	}

	private void OnWordFilterNotCensored(Messages.PluginMessage msg)
	{
		WordFilter.FilteredComment result = WordFilter.GetResult();
		OnScreenLog.Add("Not censored: changed=" + result.wasChanged + ", comment='" + result.comment + "'");
	}

	private void OnWordFilterSanitized(Messages.PluginMessage msg)
	{
		WordFilter.FilteredComment result = WordFilter.GetResult();
		OnScreenLog.Add("Sanitized: changed=" + result.wasChanged + ", comment='" + result.comment + "'");
	}

	private void OnWordFilterError(Messages.PluginMessage msg)
	{
		ResultCode result = default(ResultCode);
		WordFilter.GetLastError(out result);
		OnScreenLog.Add(result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
	}
}
