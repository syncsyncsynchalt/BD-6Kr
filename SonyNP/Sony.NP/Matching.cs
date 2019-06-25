using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sony.NP
{
    public class Matching
    {
        public enum EnumSessionType
        {
            SESSION_TYPE_DEFAULT = 0,
            SESSION_TYPE_PUBLIC = 4,
            SESSION_TYPE_PRIVATE = 8
        }

        [Flags]
        public enum FlagSessionCreate
        {
            CREATE_DEFAULT = 0x0,
            CREATE_SIGNALING_MESH_SESSION = 0x4,
            CREATE_PASSWORD_SESSION = 0x10,
            CREATE_ALLOW_BLOCK_LIST_SESSION = 0x20,
            CREATE_HOST_MIGRATION_SESSION = 0x40,
            CREATE_NAT_RESTRICTED_SESSION = 0x80
        }

        [Flags]
        public enum FlagSessionSearch
        {
            SEARCH_FRIENDS_SESSIONS = 0x400,
            SEARCH_REGIONAL_SESSIONS = 0x1000,
            SEARCH_RECENTLY_MET_SESSIONS = 0x4000,
            SEARCH_RANDOM_SESSIONS = 0x40000,
            SEARCH_NAT_RESTRICTED_SESSIONS = 0x100000
        }

        public enum EnumAttributeType
        {
            SESSION_SEARCH_ATTRIBUTE = 2,
            SESSION_EXTERNAL_ATTRIBUTE = 4,
            SESSION_INTERNAL_ATTRIBUTE = 8,
            SESSION_MEMBER_ATTRIBUTE = 0x10
        }

        public enum EnumAttributeValueType
        {
            SESSION_ATTRIBUTE_VALUE_INT = 2,
            SESSION_ATTRIBUTE_VALUE_BINARY = 4
        }

        public enum EnumAttributeMaxSize
        {
            SESSION_ATTRIBUTE_MAX_SIZE_12 = 2,
            SESSION_ATTRIBUTE_MAX_SIZE_28 = 4,
            SESSION_ATTRIBUTE_MAX_SIZE_60 = 8,
            SESSION_ATTRIBUTE_MAX_SIZE_124 = 0x10,
            SESSION_ATTRIBUTE_MAX_SIZE_252 = 0x20
        }

        public enum EnumSearchOperators
        {
            MATCHING_OPERATOR_INVALID,
            MATCHING_OPERATOR_EQ,
            MATCHING_OPERATOR_NE,
            MATCHING_OPERATOR_LT,
            MATCHING_OPERATOR_LE,
            MATCHING_OPERATOR_GT,
            MATCHING_OPERATOR_GE
        }

        [Flags]
        public enum FlagMemberType
        {
            MEMBER_OWNER = 0x2,
            MEMBER_MYSELF = 0x4
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SessionAttribute
        {
            public int intValue;

            public EnumSearchOperators searchOperator;

            public string name
            {
                set
                {
                }
            }

            public string binValue
            {
                set
                {
                }
            }

            ~SessionAttribute()
            {
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class ModifySessionAttribute
        {
            public int intValue;

            public string name
            {
                set
                {
                }
            }

            public string binValue
            {
                set
                {
                }
            }

            ~ModifySessionAttribute()
            {
            }
        }

        public struct Session
        {
            public SessionInfo sessionInfo;

            public SessionAttributeInfo[] sessionAttributes;

            public SessionMemberInfo[] members;

            public List<SessionAttributeInfo[]> memberAttributes;
        }

        public struct SessionInfo
        {
            public int sessionID;

            public int maxMembers;

            public int numMembers;

            public int numSessionAttributes;

            public int reservedSlots;

            public int openSlots;

            public int worldId;

            public int serverId;

            public int matchingContext;

            public ulong roomId;
            public string sessionName => "";
        }

        public struct SessionAttributeInfo
        {
            public EnumAttributeType attributeType;

            public EnumSearchOperators searchOperator;

            public EnumAttributeMaxSize maxSize;

            public EnumAttributeValueType attributeValueType;

            public int attributeIntValue;

            public string attributeName => "";

            public string attributeBinValue => "";
        }

        public struct SessionMemberInfo
        {
            public int memberId;

            public int natType;

            public FlagMemberType memberFlag;

            public ulong joinDate;

            public int addr;

            public int port;

            public byte[] npID
            {
                get
                {
                    return new byte[] { };
                }
            }

            public string npOnlineID => "";
        }

        private static bool hosting = false;

        public static bool InSession => false;

        public static bool IsHost => false;

        public static bool SessionIsBusy => false;

        public static event Messages.EventHandler OnCreatedSession;

        public static event Messages.EventHandler OnFoundSessions;

        public static event Messages.EventHandler OnJoinedSession;

        public static event Messages.EventHandler OnJoinInvalidSession;

        public static event Messages.EventHandler OnUpdatedSession;

        public static event Messages.EventHandler OnLeftSession;

        public static event Messages.EventHandler OnSessionDestroyed;

        public static event Messages.EventHandler OnKickedOut;

        public static event Messages.EventHandler OnSessionError;

        public static bool GetLastError(out ResultCode result)
        {
            result = new ResultCode();
            return false;
        }

        public static ErrorCode ClearAttributeDefinitions()
        {
            return new ErrorCode();
        }

        public static ErrorCode AddAttributeDefinitionInt(string name, EnumAttributeType type)
        {
            return new ErrorCode();
        }

        public static ErrorCode AddAttributeDefinitionBin(string name, EnumAttributeType type, EnumAttributeMaxSize maxSize)
        {
            return new ErrorCode();
        }

        public static ErrorCode RegisterAttributeDefinitions()
        {
            return new ErrorCode();
        }

        public static ErrorCode ClearSessionAttributes()
        {
            return new ErrorCode();
        }

        public static ErrorCode AddSessionAttribute(SessionAttribute sessionAttribute)
        {
            return new ErrorCode();
        }

        public static ErrorCode CreateSession(string name, int serverID, int worldID, int numSlots, string password, FlagSessionCreate creationFlags, EnumSessionType sessionType, string ps4SessionStatus)
        {
            return new ErrorCode();
        }

        public static ErrorCode CreateFriendsSession(string name, int serverID, int worldID, int numSlots, int friendSlots, string password, FlagSessionCreate creationFlags, string ps4SessionStatus)
        {
            return new ErrorCode();
        }

        public static ErrorCode JoinSession(int sessionID, string password)
        {
            return new ErrorCode();
        }

        public static ErrorCode JoinInvitedSession(string password)
        {
            return new ErrorCode();
        }

        public static ErrorCode JoinInvitedSession()
        {
            return new ErrorCode();
        }

        public static ErrorCode ClearModifySessionAttributes()
        {
            return new ErrorCode();
        }

        public static ErrorCode AddModifySessionAttribute(ModifySessionAttribute sessionAttribute)
        {
            return new ErrorCode();
        }

        public static ErrorCode ModifySession(EnumAttributeType attributeType)
        {
            return new ErrorCode();
        }

        public static Session GetSession()
        {
            return new Session();
        }

        public static IntPtr GetSessionInformationPtr()
        {
            return new IntPtr();
        }

        public static ErrorCode LeaveSession()
        {
            return new ErrorCode();
        }

        public static ErrorCode FindSession(int serverID, int worldID)
        {
            return new ErrorCode();
        }

        public static ErrorCode FindSession(int serverID, int worldID, FlagSessionSearch flags)
        {
            return new ErrorCode();
        }

        public static ErrorCode FindSessionFriends(int serverID, int worldID)
        {
            return new ErrorCode();
        }

        public static ErrorCode FindSessionRegional(int serverID, int worldID)
        {
            return new ErrorCode();
        }

        public static Session[] GetFoundSessionList()
        {
            return new Session[] { };
        }

        public static ErrorCode InviteToSession(string text, int npIDCount)
        {
            return new ErrorCode();
        }

        public static ErrorCode GetSessionInviteSessionAttribute(string attributeName, out SessionAttributeInfo info)
        {
            info = new SessionAttributeInfo();
            return new ErrorCode();
        }

        public static bool ProcessMessage(Messages.PluginMessage msg)
        {
            return false;
        }
    }
}
