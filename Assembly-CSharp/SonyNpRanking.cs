using Sony.NP;
using System;

public class SonyNpRanking : IScreen
{
	private MenuLayout rankingMenu;

	private ulong currentScore;

	private int rankBoardID;

	private int LastRankDisplayed;

	private int LastRankingMaxCount = 999;

	public SonyNpRanking()
	{
		Initialize();
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		MenuRanking(stack);
	}

	public MenuLayout GetMenu()
	{
		return rankingMenu;
	}

	public void Initialize()
	{
		rankingMenu = new MenuLayout(this, 450, 34);
		Ranking.OnCacheRegistered += OnSomeEvent;
		Ranking.OnRegisteredNewBestScore += OnRegisteredNewBestScore;
		Ranking.OnNotBestScore += OnSomeEvent;
		Ranking.OnGotOwnRank += OnRankingGotOwnRank;
		Ranking.OnGotFriendRank += OnRankingGotFriendRank;
		Ranking.OnGotRankList += OnRankingGotRankList;
		Ranking.OnRankingError += OnRankingError;
	}

	private ErrorCode ErrorHandler(ErrorCode errorCode = ErrorCode.NP_ERR_FAILED)
	{
		if (errorCode != 0)
		{
			ResultCode result = default(ResultCode);
			Ranking.GetLastError(out result);
			if (result.lastError != 0)
			{
				OnScreenLog.Add("Error: " + result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
				return result.lastError;
			}
		}
		return errorCode;
	}

	public void MenuRanking(MenuStack menuStack)
	{
		bool isSignedInPSN = User.IsSignedInPSN;
		rankingMenu.Update();
		if (rankingMenu.AddItem("Register Score", isSignedInPSN && !Ranking.RegisterScoreIsBusy()))
		{
			OnScreenLog.Add("Registering score: " + currentScore);
			ErrorHandler(Ranking.RegisterScore(rankBoardID, currentScore, "Insert comment here"));
			currentScore++;
		}
		if (rankingMenu.AddItem("Register score & data", isSignedInPSN && !Ranking.RegisterScoreIsBusy()))
		{
			OnScreenLog.Add("Registering score: " + currentScore);
			byte[] array = new byte[64];
			for (byte b = 0; b < 64; b = (byte)(b + 1))
			{
				array[b] = b;
			}
			ErrorHandler(Ranking.RegisterScoreWithData(rankBoardID, currentScore, "Insert comment here", array));
			currentScore++;
		}
		if (rankingMenu.AddItem("Own Rank", isSignedInPSN && !Ranking.RefreshOwnRankIsBusy()))
		{
			ErrorHandler(Ranking.RefreshOwnRank(rankBoardID));
		}
		if (rankingMenu.AddItem("Friend Rank", isSignedInPSN && !Ranking.RefreshFriendRankIsBusy()))
		{
			ErrorHandler(Ranking.RefreshFriendRank(rankBoardID));
		}
		if (rankingMenu.AddItem("Rank List", isSignedInPSN && !Ranking.RefreshRankListIsBusy()))
		{
			int num = LastRankDisplayed + 1;
			int count = Math.Min(10, LastRankingMaxCount - num + 1);
			ErrorHandler(Ranking.RefreshRankList(rankBoardID, num, count));
		}
		if (rankingMenu.AddBackIndex("Back"))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnRegisteredNewBestScore(Messages.PluginMessage msg)
	{
		Ranking.Rank ownRank = Ranking.GetOwnRank();
		OnScreenLog.Add("New best score...");
		OnScreenLog.Add("rank #" + ownRank.rank + ", provisional rank #" + ownRank.provisional + ", online id:" + ownRank.onlineId + ", score:" + ownRank.score + ", comment:" + ownRank.comment);
	}

	private void LogRank(Ranking.Rank rank)
	{
		long num = 10L;
		DateTime dateTime = new DateTime((long)rank.recordDate * num);
		OnScreenLog.Add("#" + rank.rank + " (provisionally #" + rank.provisional + "), online id:" + rank.onlineId + ", score:" + rank.score + ", comment:" + rank.comment + ", recorded on:" + dateTime.ToString());
		if (rank.gameInfoSize <= 0)
		{
			return;
		}
		int num2 = 0;
		string str = string.Empty;
		byte[] gameInfoData = rank.gameInfoData;
		foreach (byte b in gameInfoData)
		{
			str = str + b.ToString() + ",";
			if (num2++ > 8)
			{
				break;
			}
		}
		str += "...";
		OnScreenLog.Add("  dataSize: " + rank.gameInfoSize + ", data: " + str);
	}

	private void OnRankingGotOwnRank(Messages.PluginMessage msg)
	{
		Ranking.Rank ownRank = Ranking.GetOwnRank();
		OnScreenLog.Add("Own rank...");
		if (ownRank.rank > 0)
		{
			LogRank(ownRank);
		}
		else
		{
			OnScreenLog.Add("rank #: Not Ranked, " + ownRank.onlineId);
		}
	}

	private void OnRankingGotFriendRank(Messages.PluginMessage msg)
	{
		Ranking.Rank[] friendRanks = Ranking.GetFriendRanks();
		OnScreenLog.Add("Friend ranks...");
		for (int i = 0; i < friendRanks.Length; i++)
		{
			LogRank(friendRanks[i]);
		}
	}

	private void OnRankingGotRankList(Messages.PluginMessage msg)
	{
		Ranking.Rank[] rankList = Ranking.GetRankList();
		OnScreenLog.Add("Ranks...");
		OnScreenLog.Add("Showing " + rankList[0].serialRank + "-> " + (rankList[0].serialRank + rankList.Length - 1) + " out of " + Ranking.GetRanksCountOnServer());
		for (int i = 0; i < rankList.Length; i++)
		{
			LogRank(rankList[i]);
		}
		LastRankDisplayed = rankList[0].serialRank + rankList.Length - 1;
		LastRankingMaxCount = Ranking.GetRanksCountOnServer();
		Console.WriteLine("LastRankDisplayed:" + LastRankDisplayed + " LastRankingMaxCount:" + LastRankingMaxCount);
		if (LastRankDisplayed >= LastRankingMaxCount)
		{
			LastRankDisplayed = 0;
		}
	}

	private void OnRankingError(Messages.PluginMessage msg)
	{
		ErrorHandler();
	}
}
