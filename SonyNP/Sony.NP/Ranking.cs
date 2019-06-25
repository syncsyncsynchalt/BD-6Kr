using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Ranking
	{
		public struct Rank
		{
			private IntPtr _onlineId;

			public int PcId;

			public int serialRank;

			public int rank;

			public int highestRank;

			public bool hasGameData;

			public ulong score;

			public ulong recordDate;

			private IntPtr _comment;

			public int gameInfoSize;

			private IntPtr _gameInfoData;

			public int boardId;

			public int provisional;

			public string onlineId => Marshal.PtrToStringAnsi(_onlineId);

			public string comment => Marshal.PtrToStringAnsi(_comment);

			public byte[] gameInfoData
			{
				get
				{
					if (gameInfoSize > 0)
					{
						byte[] array = new byte[gameInfoSize];
						Marshal.Copy(_gameInfoData, array, 0, gameInfoSize);
						return array;
					}
					return null;
				}
			}
		}

		public static event Messages.EventHandler OnCacheRegistered;

		public static event Messages.EventHandler OnRegisteredNewBestScore;

		public static event Messages.EventHandler OnNotBestScore;

		public static event Messages.EventHandler OnGotOwnRank;

		public static event Messages.EventHandler OnGotFriendRank;

		public static event Messages.EventHandler OnGotRankList;

		public static event Messages.EventHandler OnRankingError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxRankingGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxRankingGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRankingRegisterCache(int boardLineCount, int writeLineCount, bool friendCache, int rangeLineCount);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxRankingRegisterScoreIsBusy();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxRankingRegisterScore(int boardID, ulong score, string comment);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxRankingRegisterScoreWithData(int boardID, ulong score, string comment, byte[] data, int dataSize);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxRankingRefreshOwnRankIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRankingRefreshOwnRank(int boardID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRankingGetOwnRank(out Rank rank);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxRankingRefreshFriendRankIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRankingRefreshFriendRank(int boardID);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxRankingLockFriendRankList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxRankingUnlockFriendRankList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxRankingGetFriendRankCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRankingGetFriendRank(int index, out Rank rank);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxRankingRefreshRankListIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRankingRefreshRankList(int boardID, int firstIndex, int count);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxRankingLockRankList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxRankingUnlockRankList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxRankingGetRankListCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRankingGetRank(int index, out Rank rank);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxRankingGetTotalRankCount();

		public static ErrorCode RegisterCache(int boardLineCount, int writeLineCount, bool friendCache, int rangeLineCount)
		{
			return PrxRankingRegisterCache(boardLineCount, writeLineCount, friendCache, rangeLineCount);
		}

		public static bool RegisterScoreIsBusy()
		{
			return PrxRankingRegisterScoreIsBusy();
		}

		public static ErrorCode RegisterScore(int boardID, ulong score, string comment)
		{
			return PrxRankingRegisterScore(boardID, score, comment);
		}

		public static ErrorCode RegisterScoreWithData(int boardID, ulong score, string comment, byte[] data)
		{
			return PrxRankingRegisterScoreWithData(boardID, score, comment, data, data.Length);
		}

		public static bool RefreshOwnRankIsBusy()
		{
			return PrxRankingRefreshOwnRankIsBusy();
		}

		public static ErrorCode RefreshOwnRank(int boardID)
		{
			return PrxRankingRefreshOwnRank(boardID);
		}

		public static Rank GetOwnRank()
		{
			Rank rank = default(Rank);
			PrxRankingGetOwnRank(out rank);
			return rank;
		}

		public static bool RefreshFriendRankIsBusy()
		{
			return PrxRankingRefreshFriendRankIsBusy();
		}

		public static ErrorCode RefreshFriendRank(int boardID)
		{
			return PrxRankingRefreshFriendRank(boardID);
		}

		public static Rank[] GetFriendRanks()
		{
			PrxRankingLockFriendRankList();
			Rank[] array = new Rank[PrxRankingGetFriendRankCount()];
			for (int i = 0; i < PrxRankingGetFriendRankCount(); i++)
			{
				PrxRankingGetFriendRank(i, out array[i]);
			}
			PrxRankingUnlockFriendRankList();
			return array;
		}

		public static bool RefreshRankListIsBusy()
		{
			return PrxRankingRefreshRankListIsBusy();
		}

		public static ErrorCode RefreshRankList(int boardID, int firstIndex, int count)
		{
			return PrxRankingRefreshRankList(boardID, firstIndex, count);
		}

		public static Rank[] GetRankList()
		{
			PrxRankingLockRankList();
			Rank[] array = new Rank[PrxRankingGetRankListCount()];
			for (int i = 0; i < PrxRankingGetRankListCount(); i++)
			{
				PrxRankingGetRank(i, out array[i]);
			}
			PrxRankingUnlockRankList();
			return array;
		}

		public static int GetRanksCountOnServer()
		{
			return PrxRankingGetTotalRankCount();
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_RankingCacheRegistered:
				if (Ranking.OnCacheRegistered != null)
				{
					Ranking.OnCacheRegistered(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_RankingNewBestScore:
				if (Ranking.OnRegisteredNewBestScore != null)
				{
					Ranking.OnRegisteredNewBestScore(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_RankingNotBestScore:
				if (Ranking.OnNotBestScore != null)
				{
					Ranking.OnNotBestScore(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_RankingGotOwnRank:
				if (Ranking.OnGotOwnRank != null)
				{
					Ranking.OnGotOwnRank(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_RankingGotFriendRank:
				if (Ranking.OnGotFriendRank != null)
				{
					Ranking.OnGotFriendRank(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_RankingGotRankList:
				if (Ranking.OnGotRankList != null)
				{
					Ranking.OnGotRankList(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_RankingError:
				if (Ranking.OnRankingError != null)
				{
					Ranking.OnRankingError(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
