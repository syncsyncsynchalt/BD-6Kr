using Sony.NP;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.PSVita;

public class SonyNpCloudTUS : IScreen
{
	private enum TUSDataRequestType
	{
		None,
		SaveRawData,
		LoadRawData,
		SavePlayerPrefs,
		LoadPlayerPrefs
	}

	private const int kTUS_DataSlot_RawData = 1;

	private const int kTUS_DataSlot_PlayerPrefs = 3;

	private MenuLayout menuTus;

	private string virtualUserOnlineID = "_ERGVirtualUser1";

	private TUSDataRequestType m_TUSDataRequestType;

	public SonyNpCloudTUS()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuTus;
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
		MenuTus(stack);
	}

	public void Initialize()
	{
		menuTus = new MenuLayout(this, 550, 34);
		TusTss.OnTusDataSet += OnSetTusData;
		TusTss.OnTusDataRecieved += OnGotTusData;
		TusTss.OnTusVariablesSet += OnSetTusVariables;
		TusTss.OnTusVariablesModified += OnModifiedTusVariables;
		TusTss.OnTusVariablesRecieved += OnGotTusVariables;
		TusTss.OnTusTssError += OnTusTssError;
	}

	public void MenuTus(MenuStack menuStack)
	{
		menuTus.Update();
		bool enabled = User.IsSignedInPSN && !TusTss.IsTusBusy();
		if (menuTus.AddItem("TUS Set Data", enabled))
		{
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = (byte)(3 - i);
			}
			OnScreenLog.Add(" Data size: " + array.Length);
			string text = string.Empty;
			for (int j = 0; j < 16 && j < array.Length; j++)
			{
				text = text + array[j].ToString() + ", ";
			}
			OnScreenLog.Add(" Data: " + text);
			m_TUSDataRequestType = TUSDataRequestType.SaveRawData;
			ErrorHandler(TusTss.SetTusData(1, array));
		}
		if (menuTus.AddItem("TUS Request Data", enabled))
		{
			m_TUSDataRequestType = TUSDataRequestType.LoadRawData;
			ErrorHandler(TusTss.RequestTusData(1));
		}
		if (menuTus.AddItem("TUS Save PlayerPrefs", enabled))
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.SetInt("keyA", 1);
			PlayerPrefs.SetString("keyB", "Hello");
			PlayerPrefs.SetInt("keyC", 3);
			PlayerPrefs.SetInt("keyD", 4);
			byte[] data = PSVitaPlayerPrefs.SaveToByteArray();
			m_TUSDataRequestType = TUSDataRequestType.SavePlayerPrefs;
			ErrorHandler(TusTss.SetTusData(3, data));
		}
		if (menuTus.AddItem("TUS Load PlayerPrefs", enabled))
		{
			m_TUSDataRequestType = TUSDataRequestType.LoadPlayerPrefs;
			ErrorHandler(TusTss.RequestTusData(3));
		}
		if (menuTus.AddItem("TUS Set Variables", enabled))
		{
			ErrorHandler(TusTss.SetTusVariables(new TusTss.TusVariable[4]
			{
				new TusTss.TusVariable(1, 110L),
				new TusTss.TusVariable(2, 220L),
				new TusTss.TusVariable(3, 330L),
				new TusTss.TusVariable(4, 440L)
			}));
		}
		if (menuTus.AddItem("TUS Request Variables", enabled))
		{
			int[] slotIDs = new int[4]
			{
				1,
				2,
				3,
				4
			};
			ErrorHandler(TusTss.RequestTusVariables(slotIDs));
		}
		if (menuTus.AddItem("TUS Set Variables VU", enabled))
		{
			ErrorHandler(TusTss.SetTusVariablesForVirtualUser(variables: new TusTss.TusVariable[1]
			{
				new TusTss.TusVariable(5, 12345L)
			}, onlineID: virtualUserOnlineID));
		}
		if (menuTus.AddItem("TUS Request Variables VU", enabled))
		{
			int[] slotIDs2 = new int[1]
			{
				5
			};
			ErrorHandler(TusTss.RequestTusVariablesForVirtualUser(virtualUserOnlineID, slotIDs2));
		}
		if (menuTus.AddItem("TUS Modify Variables VU", enabled))
		{
			ErrorHandler(TusTss.ModifyTusVariablesForVirtualUser(variables: new TusTss.TusVariable[1]
			{
				new TusTss.TusVariable(5, 1L)
			}, onlineID: virtualUserOnlineID));
		}
		if (menuTus.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnTusTssError(Messages.PluginMessage msg)
	{
		ErrorHandler();
	}

	private void OnSetTusData(Messages.PluginMessage msg)
	{
		switch (m_TUSDataRequestType)
		{
		case TUSDataRequestType.LoadRawData:
			break;
		case TUSDataRequestType.SavePlayerPrefs:
			OnScreenLog.Add("Sent PlayerPrefs to TUS");
			break;
		case TUSDataRequestType.SaveRawData:
			OnScreenLog.Add("Sent data to TUS");
			break;
		}
	}

	private void OnGotTusData(Messages.PluginMessage msg)
	{
		switch (m_TUSDataRequestType)
		{
		case TUSDataRequestType.SavePlayerPrefs:
			break;
		case TUSDataRequestType.LoadPlayerPrefs:
		{
			OnScreenLog.Add("Got PlayerPrefs from TUS...");
			byte[] tusData = TusTss.GetTusData();
			PSVitaPlayerPrefs.LoadFromByteArray(tusData);
			OnScreenLog.Add(" keyA = " + PlayerPrefs.GetInt("keyA"));
			OnScreenLog.Add(" keyB = " + PlayerPrefs.GetString("keyB"));
			OnScreenLog.Add(" keyC = " + PlayerPrefs.GetInt("keyC"));
			OnScreenLog.Add(" keyD = " + PlayerPrefs.GetInt("keyD"));
			break;
		}
		case TUSDataRequestType.LoadRawData:
		{
			OnScreenLog.Add("Got TUS Data");
			byte[] tusData = TusTss.GetTusData();
			OnScreenLog.Add(" Data size: " + tusData.Length);
			string text = string.Empty;
			for (int i = 0; i < 16 && i < tusData.Length; i++)
			{
				text = text + tusData[i].ToString() + ", ";
			}
			OnScreenLog.Add(" Data: " + text);
			break;
		}
		}
	}

	private void OnSetTusVariables(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Sent TUS variables");
	}

	private void OnGotTusVariables(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got TUS Variables");
		TusTss.TusRetrievedVariable[] tusVariables = TusTss.GetTusVariables();
		for (int i = 0; i < tusVariables.Length; i++)
		{
			string @string = Encoding.Default.GetString(tusVariables[i].ownerNpID);
			string string2 = Encoding.Default.GetString(tusVariables[i].lastChangeAuthorNpID);
			DateTime dateTime = new DateTime(tusVariables[i].lastChangedDate, DateTimeKind.Utc);
			OnScreenLog.Add(" HasData: " + tusVariables[i].hasData);
			OnScreenLog.Add(" Value: " + tusVariables[i].variable);
			OnScreenLog.Add(" OwnerNpID: " + @string);
			OnScreenLog.Add(" lastChangeNpID: " + string2);
			OnScreenLog.Add(" lastChangeTime: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		}
	}

	private void OnModifiedTusVariables(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Modified TUS Variables");
		TusTss.TusRetrievedVariable[] tusVariables = TusTss.GetTusVariables();
		for (int i = 0; i < tusVariables.Length; i++)
		{
			string @string = Encoding.Default.GetString(tusVariables[i].ownerNpID);
			string string2 = Encoding.Default.GetString(tusVariables[i].lastChangeAuthorNpID);
			DateTime dateTime = new DateTime(tusVariables[i].lastChangedDate, DateTimeKind.Utc);
			OnScreenLog.Add(" HasData: " + tusVariables[i].hasData);
			OnScreenLog.Add(" Value: " + tusVariables[i].variable);
			OnScreenLog.Add(" OwnerNpID: " + @string);
			OnScreenLog.Add(" lastChangeNpID: " + string2);
			OnScreenLog.Add(" lastChangeTime: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		}
	}
}
