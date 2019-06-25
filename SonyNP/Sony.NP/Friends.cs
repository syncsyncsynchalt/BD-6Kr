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
            public EnumNpOnlineStatus npOnlineStatus;

            public int npPresenceSdkVersion;

            public EnumNpPresenceType npPresenceType;

            public int npPresenceDataSize;

            public byte[] npID
            {
                get
                {
                    return new byte[] { };
                }
            }

            public string npOnlineID => "";

            public string npPresenceTitle => "";

            public string npPresenceStatus => "";

            public string npComment => "";

            public byte[] npPresenceData
            {
                get
                {
                    return new byte[] { };
                }
            }
        }

        public static event Messages.EventHandler OnFriendsListUpdated;

        public static event Messages.EventHandler OnFriendsPresenceUpdated;

        public static event Messages.EventHandler OnGotFriendsList;

        public static event Messages.EventHandler OnFriendsListError;

        public static bool GetLastError(out ResultCode result)
        {
            result = new ResultCode();
            return false;
        }

        public static ErrorCode RequestFriendsList()
        {
            return new ErrorCode();
        }

        public static bool FriendsListIsBusy()
        {
            return false;
        }

        public static Friend[] GetCachedFriendsList()
        {
            return new Friend[] { };
        }

        public static bool ProcessMessage(Messages.PluginMessage msg)
        {
            return false;
        }
    }
}
