using Sony.NP;
using System;

public class SonyNpTicketing : IScreen
{
	private MenuLayout menuTicketing;

	private bool gotTicket;

	private Ticketing.Ticket ticket;

	public SonyNpTicketing()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuTicketing;
	}

	public void Initialize()
	{
		menuTicketing = new MenuLayout(this, 450, 34);
		Ticketing.OnGotTicket += OnGotTicket;
		Ticketing.OnError += OnError;
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
			Ticketing.GetLastError(out result);
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
		menuTicketing.Update();
		bool enabled = User.IsSignedInPSN && !Ticketing.IsBusy();
		if (menuTicketing.AddItem("Request Ticket", enabled))
		{
			ErrorHandler(Ticketing.RequestTicket());
		}
		if (menuTicketing.AddItem("Request Cached Ticket", enabled))
		{
			ErrorHandler(Ticketing.RequestCachedTicket());
		}
		if (menuTicketing.AddItem("Get Ticket Entitlements", gotTicket))
		{
			Ticketing.TicketEntitlement[] ticketEntitlements = Ticketing.GetTicketEntitlements(ticket);
			OnScreenLog.Add("Ticket contains " + ticketEntitlements.Length + " entitlements");
			for (int i = 0; i < ticketEntitlements.Length; i++)
			{
				OnScreenLog.Add("Entitlement " + i);
				OnScreenLog.Add(" " + ticketEntitlements[i].id + " rc: " + ticketEntitlements[i].remainingCount + " cc: " + ticketEntitlements[i].consumedCount + " type: " + ticketEntitlements[i].type);
			}
		}
		if (menuTicketing.AddBackIndex("Back"))
		{
			stack.PopMenu();
		}
	}

	private void OnGotTicket(Messages.PluginMessage msg)
	{
		ticket = Ticketing.GetTicket();
		gotTicket = true;
		OnScreenLog.Add("GotTicket");
		OnScreenLog.Add(" dataSize: " + ticket.dataSize);
		Ticketing.TicketInfo ticketInfo = Ticketing.GetTicketInfo(ticket);
		OnScreenLog.Add(" Issuer ID: " + ticketInfo.issuerID);
		DateTime dateTime = new DateTime(ticketInfo.issuedDate, DateTimeKind.Utc);
		OnScreenLog.Add(" Issue date: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		DateTime dateTime2 = new DateTime(ticketInfo.expireDate, DateTimeKind.Utc);
		OnScreenLog.Add(" Expire date: " + dateTime2.ToLongDateString() + " - " + dateTime2.ToLongTimeString());
		OnScreenLog.Add(" Account ID: 0x" + ticketInfo.subjectAccountID.ToString("X8"));
		OnScreenLog.Add(" Online ID: " + ticketInfo.subjectOnlineID);
		OnScreenLog.Add(" Service ID: " + ticketInfo.serviceID);
		OnScreenLog.Add(" Domain: " + ticketInfo.subjectDomain);
		OnScreenLog.Add(" Country Code: " + ticketInfo.countryCode);
		OnScreenLog.Add(" Language Code: " + ticketInfo.languageCode);
		OnScreenLog.Add(" Age: " + ticketInfo.subjectAge);
		OnScreenLog.Add(" Chat disabled: " + ticketInfo.chatDisabled);
		OnScreenLog.Add(" Content rating: " + ticketInfo.contentRating);
	}

	private void OnError(Messages.PluginMessage msg)
	{
		ErrorHandler();
	}
}
