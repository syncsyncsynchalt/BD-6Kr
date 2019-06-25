using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Friends
	{
		public enum EnumNpOnlineStatus
		{
			ONLINE_STATUS_UNKNOWN,
			ONLINE_STATUS_OFFLINE,
			ONLINE_STATUS_AFK,
			ONLINE_STATUS_ONLINE
		}

		public enum EnumNpPresenceType
		{
			IN_GAME_PRESENCE_TYPE_UNKNOWN = -1,
			IN_GAME_PRESENCE_TYPE_NONE,
			IN_GAME_PRESENCE_TYPE_DEFAULT,
			IN_GAME_PRESENCE_TYPE_GAME_JOINING,
			IN_GAME_PRESENCE_TYPE_GAME_JOINING_ONLY_FOR_PARTY,
			IN_GAME_PRESENCE_TYPE_JOIN_GAME_ACK,
			IN_GAME_PRESENCE_TYPE_MAX
		}

		public struct Friend
		{
			private IntPtr _npID;

			private int npIDSize;

			private IntPtr _npOnlineID;

			public EnumNpOnlineStatus npOnlineStatus;

			private IntPtr _npPresenceTitle;

			public int npPresenceSdkVersion;

			public EnumNpPresenceType npPresenceType;

			private IntPtr _npPresenceStatus;

			private IntPtr _npComment;

			private IntPtr _npPresenceData;

			public int npPresenceDataSize;

			public byte[] npID
			{
				get
				{
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_npID, array, 0, npIDSize);
					return array;
				}
			}

			public string npOnlineID => Marshal.PtrToStringAnsi(_npOnlineID);

			public string npPresenceTitle => Marshal.PtrToStringAnsi(_npPresenceTitle);

			public string npPresenceStatus => Marshal.PtrToStringAnsi(_npPresenceStatus);

			public string npComment => Marshal.PtrToStringAnsi(_npComment);

			public byte[] npPresenceData
			{
				get
				{
					byte[] array = new byte[128];
					Marshal.Copy(_npPresenceData, array, 0, 128);
					return array;
				}
			}
		}

		private const int NP_ONLINEID_MAX_LENGTH = 16;

		private const int IN_GAME_PRESENCE_STATUS_SIZE_MAX = 192;

		private const int IN_GAME_PRESENCE_DATA_SIZE_MAX = 128;

		public static event Messages.EventHandler OnFriendsListUpdated;

		public static event Messages.EventHandler OnFriendsPresenceUpdated;

		public static event Messages.EventHandler OnGotFriendsList;

		public static event Messages.EventHandler OnFriendsListError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxFriendsListIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRefreshFriendsList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxLockFriendsList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxUnlockFriendsList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetFriendCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetFriend(int index, out Friend frnd);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxFriendsGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxFriendsGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		public static ErrorCode RequestFriendsList()
		{
			return PrxRefreshFriendsList();
		}

		public static bool FriendsListIsBusy()
		{
			return PrxFriendsListIsBusy();
		}

		public static Friend[] GetCachedFriendsList()
		{
			PrxLockFriendsList();
			Friend[] array = new Friend[PrxGetFriendCount()];
			for (int i = 0; i < PrxGetFriendCount(); i++)
			{
				PrxGetFriend(i, out array[i]);
			}
			PrxUnlockFriendsList();
			return array;
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_FriendsListUpdated:
				if (Friends.OnFriendsListUpdated != null)
				{
					Friends.OnFriendsListUpdated(msg);
				}
				return true;
			case Messages.MessageType.kNPToolkit_FriendsPresenceUpdated:
				if (Friends.OnFriendsPresenceUpdated != null)
				{
					Friends.OnFriendsPresenceUpdated(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_GotFriendsList:
				if (Friends.OnGotFriendsList != null)
				{
					Friends.OnGotFriendsList(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_FriendsListError:
				if (Friends.OnFriendsListError != null)
				{
					Friends.OnFriendsListError(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
