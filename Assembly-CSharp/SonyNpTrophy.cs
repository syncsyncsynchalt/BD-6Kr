using Sony.NP;
using UnityEngine;

public class SonyNpTrophy : IScreen
{
	private MenuLayout menuTrophies;

	private int nextTrophyIndex = 1;

	private Trophies.GameInfo gameInfo;

	private Texture2D trophyIcon;

	private Texture2D trophyGroupIcon;

	public SonyNpTrophy()
	{
		Initialize();
	}

	public MenuLayout GetMenu()
	{
		return menuTrophies;
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
			Trophies.GetLastError(out result);
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
		MenuTrophies(stack);
	}

	public void Initialize()
	{
		menuTrophies = new MenuLayout(this, 450, 34);
		Trophies.OnGotGameInfo += OnTrophyGotGameInfo;
		Trophies.OnGotGroupInfo += OnTrophyGotGroupInfo;
		Trophies.OnGotTrophyInfo += OnTrophyGotTrophyInfo;
		Trophies.OnGotProgress += OnTrophyGotProgress;
		Trophies.OnAwardedTrophy += OnSomeEvent;
		Trophies.OnAwardTrophyFailed += OnSomeEvent;
		Trophies.OnAlreadyAwardedTrophy += OnSomeEvent;
		Trophies.OnUnlockedPlatinum += OnSomeEvent;
	}

	public void MenuTrophies(MenuStack menuStack)
	{
		menuTrophies.Update();
		bool trophiesAreAvailable = Trophies.TrophiesAreAvailable;
		if (menuTrophies.AddItem("Game Info", trophiesAreAvailable))
		{
			DumpGameInfo();
		}
		if (menuTrophies.AddItem("Group Info", trophiesAreAvailable && !Trophies.RequestGroupInfoIsBusy()))
		{
			ErrorHandler(Trophies.RequestGroupInfo());
		}
		if (menuTrophies.AddItem("Trophy Info", trophiesAreAvailable && !Trophies.RequestTrophyInfoIsBusy()))
		{
			ErrorHandler(Trophies.RequestTrophyInfo());
		}
		if (menuTrophies.AddItem("Trophy Progress", trophiesAreAvailable && !Trophies.RequestTrophyProgressIsBusy()))
		{
			ErrorHandler(Trophies.RequestTrophyProgress());
		}
		if (menuTrophies.AddItem("Award Trophy", trophiesAreAvailable) && ErrorHandler(Trophies.AwardTrophy(nextTrophyIndex)) == ErrorCode.NP_OK)
		{
			nextTrophyIndex++;
			if (nextTrophyIndex == gameInfo.numTrophies)
			{
				nextTrophyIndex = 1;
			}
		}
		if (menuTrophies.AddItem("Award All Trophies", trophiesAreAvailable))
		{
			for (int i = 1; i < gameInfo.numTrophies; i++)
			{
				ErrorHandler(Trophies.AwardTrophy(i));
			}
		}
		if (menuTrophies.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void DumpGameInfo()
	{
		OnScreenLog.Add("title: " + gameInfo.title);
		OnScreenLog.Add("desc: " + gameInfo.description);
		OnScreenLog.Add("numTrophies: " + gameInfo.numTrophies);
		OnScreenLog.Add("numGroups: " + gameInfo.numGroups);
		OnScreenLog.Add("numBronze: " + gameInfo.numBronze);
		OnScreenLog.Add("numSilver: " + gameInfo.numSilver);
		OnScreenLog.Add("numGold: " + gameInfo.numGold);
		OnScreenLog.Add("numPlatinum: " + gameInfo.numPlatinum);
	}

	private void OnTrophyGotGameInfo(Messages.PluginMessage msg)
	{
		gameInfo = Trophies.GetCachedGameInfo();
		DumpGameInfo();
	}

	private void OnTrophyGotGroupInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Group List!");
		Trophies.GroupDetails[] cachedGroupDetails = Trophies.GetCachedGroupDetails();
		Trophies.GroupData[] cachedGroupData = Trophies.GetCachedGroupData();
		OnScreenLog.Add("Groups: " + cachedGroupDetails.Length);
		for (int i = 0; i < cachedGroupDetails.Length; i++)
		{
			if (cachedGroupDetails[i].hasIcon && trophyGroupIcon == null)
			{
				trophyGroupIcon = cachedGroupDetails[i].icon;
				OnScreenLog.Add("Found icon: " + trophyGroupIcon.width + ", " + trophyGroupIcon.height);
			}
			OnScreenLog.Add(" " + i + ": " + cachedGroupDetails[i].groupId + ", " + cachedGroupDetails[i].title + ", " + cachedGroupDetails[i].description + ", " + cachedGroupDetails[i].numTrophies + ", " + cachedGroupDetails[i].numPlatinum + ", " + cachedGroupDetails[i].numGold + ", " + cachedGroupDetails[i].numSilver + ", " + cachedGroupDetails[i].numBronze);
			OnScreenLog.Add(" " + i + ": " + cachedGroupData[i].groupId + ", " + cachedGroupData[i].unlockedTrophies + ", " + cachedGroupData[i].unlockedPlatinum + ", " + cachedGroupData[i].unlockedGold + ", " + cachedGroupData[i].unlockedSilver + ", " + cachedGroupData[i].unlockedBronze + ", " + cachedGroupData[i].progressPercentage + cachedGroupData[i].userId.ToString("X"));
		}
	}

	private void OnTrophyGotTrophyInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Trophy List!");
		Trophies.TrophyDetails[] cachedTrophyDetails = Trophies.GetCachedTrophyDetails();
		Trophies.TrophyData[] cachedTrophyData = Trophies.GetCachedTrophyData();
		OnScreenLog.Add("Trophies: " + cachedTrophyDetails.Length);
		for (int i = 0; i < cachedTrophyDetails.Length; i++)
		{
			if (cachedTrophyData[i].hasIcon && trophyIcon == null)
			{
				trophyIcon = cachedTrophyData[i].icon;
				OnScreenLog.Add("Found icon: " + trophyIcon.width + ", " + trophyIcon.height);
			}
			OnScreenLog.Add(" " + i + ": " + cachedTrophyDetails[i].name + ", " + cachedTrophyDetails[i].trophyId + ", " + cachedTrophyDetails[i].trophyGrade + ", " + cachedTrophyDetails[i].groupId + ", " + cachedTrophyDetails[i].hidden + ", " + cachedTrophyData[i].unlocked + ", " + cachedTrophyData[i].timestamp + ", " + cachedTrophyData[i].userId.ToString("X"));
		}
	}

	private void OnTrophyGotProgress(Messages.PluginMessage msg)
	{
		Trophies.TrophyProgress cachedTrophyProgress = Trophies.GetCachedTrophyProgress();
		OnScreenLog.Add("Progress for userId: 0x" + cachedTrophyProgress.userId.ToString("X"));
		OnScreenLog.Add("progressPercentage: " + cachedTrophyProgress.progressPercentage);
		OnScreenLog.Add("unlockedTrophies: " + cachedTrophyProgress.unlockedTrophies);
		OnScreenLog.Add("unlockedPlatinum: " + cachedTrophyProgress.unlockedPlatinum);
		OnScreenLog.Add("unlockedGold: " + cachedTrophyProgress.unlockedGold);
		OnScreenLog.Add("unlockedSilver: " + cachedTrophyProgress.unlockedSilver);
		OnScreenLog.Add("unlockedBronze: " + cachedTrophyProgress.unlockedBronze);
	}
}
