using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Messages
	{
		public enum MessageType
		{
			kNPToolKit_NotSet,
			kNPToolKit_Log,
			kNPToolKit_LogWarning,
			kNPToolKit_LogError,
			kNPToolKit_NPInitialized,
			kNPToolKit_ConnectionUp,
			kNPToolKit_ConnectionDown,
			kNPToolKit_SignedIn,
			kNPToolKit_SignedOut,
			kNPToolKit_SignInError,
			kNPToolKit_SysEvent,
			kNPToolKit_SysResume,
			kNPToolKit_SysNpMessageArrived,
			kNPToolKit_SysStorePurchase,
			kNPToolKit_SysStoreRedemption,
			kNPToolKit_GotUserProfile,
			kNPToolKit_GotRemoteUserNpID,
			kNPToolKit_GotRemoteUserProfile,
			kNPToolKit_UserProfileError,
			kNPToolKit_NetInfoGotBandwidth,
			kNPToolKit_NetInfoGotBasic,
			kNPToolKit_NetInfoError,
			kNPToolKit_FriendsListUpdated,
			kNPToolkit_FriendsPresenceUpdated,
			kNPToolKit_GotFriendsList,
			kNPToolKit_FriendsListError,
			kNPToolKit_PresenceSet,
			kNPToolKit_PresenceError,
			kNPToolKit_TrophySetSetupSuccess,
			kNPToolKit_TrophySetSetupCancelled,
			kNPToolKit_TrophySetSetupAborted,
			kNPToolKit_TrophySetSetupFail,
			kNPToolKit_TrophyCacheReady,
			kNPToolKit_TrophyGotGameInfo,
			kNPToolKit_TrophyGotGroupInfo,
			kNPToolKit_TrophyGotTrophyInfo,
			kNPToolKit_TrophyGotProgress,
			kNPToolKit_TrophyUnlocked,
			kNPToolKit_TrophyUnlockFailed,
			kNPToolKit_TrophyUnlockedAlready,
			kNPToolKit_TrophyUnlockedPlatinum,
			kNPToolKit_TrophyError,
			kNPToolKit_RankingCacheRegistered,
			kNPToolKit_RankingNewBestScore,
			kNPToolKit_RankingNotBestScore,
			kNPToolKit_RankingGotOwnRank,
			kNPToolKit_RankingGotFriendRank,
			kNPToolKit_RankingGotRankList,
			kNPToolKit_RankingError,
			kNPToolKit_MatchingCreatedSession,
			kNPToolKit_MatchingFoundSessions,
			kNPToolKit_MatchingJoinedSession,
			kNPToolKit_MatchingJoinInvalidSession,
			kNPToolKit_MatchingUpdatedSession,
			kNPToolKit_MatchingLeftSession,
			kNPToolKit_MatchingRoomDestroyed,
			kNPToolKit_MatchingKickedOut,
			kNPToolKit_MatchingError,
			kNPToolKit_WordFilterNotCensored,
			kNPToolKit_WordFilterCensored,
			kNPToolKit_WordFilterSanitized,
			kNPToolKit_WordFilterError,
			kNPToolKit_MessagingSent,
			kNPToolKit_MessagingCanceled,
			kNPToolKit_MessagingNotSent,
			kNPToolKit_MessagingNotSentFreqTooHigh,
			kNPToolKit_MessagingSessionInviteRetrieved,
			kNPToolKit_MessagingCustomInviteRetrieved,
			kNPToolKit_MessagingDataMessageRetrieved,
			kNPToolKit_MessagingInGameDataMessageRetrieved,
			kNPToolKit_MessagingSessionInviteReceived,
			kNPToolKit_MessagingSessionInviteAccepted,
			kNPToolKit_MessagingError,
			kNPToolKit_CommerceSessionCreated,
			kNPToolKit_CommerceSessionAborted,
			kNPToolKit_CommerceGotCategoryInfo,
			kNPToolKit_CommerceGotProductList,
			kNPToolKit_CommerceGotProductInfo,
			kNPToolKit_CommerceGotEntitlementList,
			kNPToolKit_CommerceConsumedEntitlement,
			kNPToolKit_CommerceError,
			kNPToolKit_CommerceCheckoutStarted,
			kNPToolKit_CommerceCheckoutFinished,
			kNPToolKit_CommerceProductBrowseStarted,
			kNPToolKit_CommerceProductBrowseSuccess,
			kNPToolKit_CommerceProductBrowseAborted,
			kNPToolKit_CommerceProductBrowseFinished,
			kNPToolKit_CommerceCategoryBrowseStarted,
			kNPToolKit_CommerceCategoryBrowseFinished,
			kNPToolKit_CommerceVoucherInputStarted,
			kNPToolKit_CommerceVoucherInputFinished,
			kNPToolKit_CommerceDownloadListStarted,
			kNPToolKit_CommerceDownloadListFinished,
			kNPToolKit_TUSDataSet,
			kNPToolKit_TUSDataReceived,
			kNPToolKit_TUSVariablesSet,
			kNPToolKit_TUSVariablesModified,
			kNPToolKit_TUSVariablesReceived,
			kNPToolKit_TSSDataReceived,
			kNPToolKit_TSSNoData,
			kNPToolKit_TusTssError,
			kNPToolKit_DlgFriendsClosed,
			kNPToolKit_DlgSharedPlayHistoryClosed,
			kNPToolKit_DlgProfileClosed,
			kNPToolKit_DlgCommerceClosed,
			kNPToolKit_FacebookDialogStarted,
			kNPToolKit_FacebookDialogFinished,
			kNPToolKit_FacebookMessagePosted,
			kNPToolKit_FacebookMessagePostFailed,
			kNPToolKit_TwitterDialogStarted,
			kNPToolKit_TwitterDialogFinished,
			kNPToolKit_TwitterDialogCanceled,
			kNPToolKit_TwitterMessagePosted,
			kNPToolKit_TwitterMessagePostFailed,
			kNPToolKit_TicketingGotTicket,
			kNPToolKit_TicketingError,
			kNPToolKit_CheckPlusResult,
			kNPToolKit_AccountLanguageResult,
			kNPToolKit_ParentalControlResult,
			kNPToolKit_END
		}

		public delegate void EventHandler(PluginMessage msg);

		public struct PluginMessage
		{
			public MessageType type;

			public int userId;

			public int dataSize;

			public IntPtr data;

			public string Text
			{
				get
				{
					switch (type)
					{
					case MessageType.kNPToolKit_Log:
					case MessageType.kNPToolKit_LogWarning:
					case MessageType.kNPToolKit_LogError:
						return Marshal.PtrToStringAnsi(data);
					default:
						return "no text";
					}
				}
			}

			public int Int
			{
				get
				{
					MessageType messageType = type;
					if (messageType == MessageType.kNPToolKit_SysEvent)
					{
						return (int)data;
					}
					return 0;
				}
			}

			public override string ToString()
			{
				string text = "Event:" + type.ToString();
				if (userId != 0)
				{
					text = text + " userId:0x" + userId.ToString("X8");
				}
				return text;
			}
		}

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxHasMessage();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxGetFirstMessage(out PluginMessage msg);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxRemoveFirstMessage();

		public static bool HasMessage()
		{
			return PrxHasMessage();
		}

		public static void RemoveFirstMessage()
		{
			PrxRemoveFirstMessage();
		}

		public static void GetFirstMessage(out PluginMessage msg)
		{
			PrxGetFirstMessage(out msg);
		}
	}
}
