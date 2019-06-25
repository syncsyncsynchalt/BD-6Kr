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
			private IntPtr _name;

			private IntPtr _binValue;

			public int intValue;

			public EnumSearchOperators searchOperator;

			public string name
			{
				set
				{
					_name = Marshal.StringToCoTaskMemAnsi(value);
				}
			}

			public string binValue
			{
				set
				{
					_binValue = Marshal.StringToCoTaskMemAnsi(value);
				}
			}

			~SessionAttribute()
			{
				Marshal.FreeCoTaskMem(_binValue);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class ModifySessionAttribute
		{
			private IntPtr _name;

			private IntPtr _binValue;

			public int intValue;

			public string name
			{
				set
				{
					_name = Marshal.StringToCoTaskMemAnsi(value);
				}
			}

			public string binValue
			{
				set
				{
					_binValue = Marshal.StringToCoTaskMemAnsi(value);
				}
			}

			~ModifySessionAttribute()
			{
				Marshal.FreeCoTaskMem(_binValue);
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
			private IntPtr _sessionName;

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

			public string sessionName => Marshal.PtrToStringAnsi(_sessionName);
		}

		public struct SessionAttributeInfo
		{
			private IntPtr _attribute;

			public EnumAttributeType attributeType;

			public EnumSearchOperators searchOperator;

			public EnumAttributeMaxSize maxSize;

			public EnumAttributeValueType attributeValueType;

			public int attributeIntValue;

			private IntPtr _attributeBinValue;

			public string attributeName => Marshal.PtrToStringAnsi(_attribute);

			public string attributeBinValue => Marshal.PtrToStringAnsi(_attributeBinValue);
		}

		public struct SessionMemberInfo
		{
			private IntPtr _npID;

			private int npIDSize;

			private IntPtr _npOnlineID;

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
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_npID, array, 0, npIDSize);
					return array;
				}
			}

			public string npOnlineID => Marshal.PtrToStringAnsi(_npOnlineID);
		}

		private static bool hosting = false;

		public static bool InSession => PrxSessionInSession();

		public static bool IsHost => hosting;

		public static bool SessionIsBusy => PrxSessionIsBusy();

		public static event Messages.EventHandler OnCreatedSession;

		public static event Messages.EventHandler OnFoundSessions;

		public static event Messages.EventHandler OnJoinedSession;

		public static event Messages.EventHandler OnJoinInvalidSession;

		public static event Messages.EventHandler OnUpdatedSession;

		public static event Messages.EventHandler OnLeftSession;

		public static event Messages.EventHandler OnSessionDestroyed;

		public static event Messages.EventHandler OnKickedOut;

		public static event Messages.EventHandler OnSessionError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxSessionIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxSessionGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxSessionGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxSessionInSession();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionClearAttributeDefinitions();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxSessionAddAttributeDefinition(string name, EnumAttributeType type, EnumAttributeValueType valueType, EnumAttributeMaxSize maxSize);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionRegisterAttributeDefinitions();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionClearAttributes();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionAddAttribute(SessionAttribute sessionAttribute);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxSessionCreateSession(string name, int serverID, int worldID, int numSlots, string password, FlagSessionCreate creationFlags, EnumSessionType sessionType, string ps4SessionStatus);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxSessionCreateFriendsSession(string name, int serverID, int worldID, int numSlots, int numFriendSlots, string password, FlagSessionCreate creationFlags, string ps4SessionStatus);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxSessionJoinSession(int sessionID, string password);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionJoinInvitedSession(string password);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionModifyClearAttributes();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionModifyAddAttribute(ModifySessionAttribute attribute);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionModifySession(EnumAttributeType attributeType);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetSessionInfo(out SessionInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxLockSessionAttributeList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxUnlockSessionAttributeList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetSessionAttributeListCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetSessionAttributeInfo(int index, out SessionAttributeInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxLockSessionMemberList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxUnlockSessionMemberList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetSessionMemberListCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetSessionMemberInfo(int index, out SessionMemberInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxLockSessionMemberAttributeList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxUnlockSessionMemberAttributeList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetSessionMemberAttributeListCount(int memberIndex);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetSessionMemberAttributeInfo(int memberIndex, int index, out SessionAttributeInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionLeaveSession();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionFind(int serverID, int worldID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionFindCustom(int serverID, int worldID, FlagSessionSearch flags);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionFindFriends(int serverID, int worldID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSessionFindRegional(int serverID, int worldID);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxLockFoundSessionList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxUnlockFoundSessionList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetFoundSessionListCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetFoundSessionInfo(int sessionIndex, out SessionInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxLockFoundSessionAttributeList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxUnlockFoundSessionAttributeList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetFoundSessionAttributeListCount(int sessionIndex);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetFoundSessionAttributeInfo(int sessionIndex, int attributeIndex, out SessionAttributeInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxLockFoundSessionMemberList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxUnlockFoundSessionMemberList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetFoundSessionMemberListCount(int sessionIndex);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxGetFoundSessionMemberInfo(int sessionIndex, int memberIndex, out SessionMemberInfo info);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxInviteToSession(string text, int ps4NpIDCount);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxGetSessionInviteSessionAttribute(string attributeName, out SessionAttributeInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern IntPtr PrxGetSessionInformationPtr();

		public static ErrorCode ClearAttributeDefinitions()
		{
			return PrxSessionClearAttributeDefinitions();
		}

		public static ErrorCode AddAttributeDefinitionInt(string name, EnumAttributeType type)
		{
			return PrxSessionAddAttributeDefinition(name, type, EnumAttributeValueType.SESSION_ATTRIBUTE_VALUE_INT, (EnumAttributeMaxSize)0);
		}

		public static ErrorCode AddAttributeDefinitionBin(string name, EnumAttributeType type, EnumAttributeMaxSize maxSize)
		{
			return PrxSessionAddAttributeDefinition(name, type, EnumAttributeValueType.SESSION_ATTRIBUTE_VALUE_BINARY, maxSize);
		}

		public static ErrorCode RegisterAttributeDefinitions()
		{
			return PrxSessionRegisterAttributeDefinitions();
		}

		public static ErrorCode ClearSessionAttributes()
		{
			return PrxSessionClearAttributes();
		}

		public static ErrorCode AddSessionAttribute(SessionAttribute sessionAttribute)
		{
			return PrxSessionAddAttribute(sessionAttribute);
		}

		public static ErrorCode CreateSession(string name, int serverID, int worldID, int numSlots, string password, FlagSessionCreate creationFlags, EnumSessionType sessionType, string ps4SessionStatus)
		{
			return PrxSessionCreateSession(name, serverID, worldID, numSlots, password, creationFlags, sessionType, ps4SessionStatus);
		}

		public static ErrorCode CreateFriendsSession(string name, int serverID, int worldID, int numSlots, int friendSlots, string password, FlagSessionCreate creationFlags, string ps4SessionStatus)
		{
			return PrxSessionCreateFriendsSession(name, serverID, worldID, numSlots, friendSlots, password, creationFlags, ps4SessionStatus);
		}

		public static ErrorCode JoinSession(int sessionID, string password)
		{
			return PrxSessionJoinSession(sessionID, password);
		}

		public static ErrorCode JoinInvitedSession(string password)
		{
			return PrxSessionJoinInvitedSession(password);
		}

		public static ErrorCode JoinInvitedSession()
		{
			return PrxSessionJoinInvitedSession("");
		}

		public static ErrorCode ClearModifySessionAttributes()
		{
			return PrxSessionModifyClearAttributes();
		}

		public static ErrorCode AddModifySessionAttribute(ModifySessionAttribute sessionAttribute)
		{
			return PrxSessionModifyAddAttribute(sessionAttribute);
		}

		public static ErrorCode ModifySession(EnumAttributeType attributeType)
		{
			return PrxSessionModifySession(attributeType);
		}

		public static Session GetSession()
		{
			Session result = default(Session);
			result.sessionInfo = default(SessionInfo);
			PrxGetSessionInfo(out result.sessionInfo);
			PrxLockSessionAttributeList();
			int num = PrxGetSessionAttributeListCount();
			result.sessionAttributes = new SessionAttributeInfo[num];
			for (int i = 0; i < num; i++)
			{
				PrxGetSessionAttributeInfo(i, out result.sessionAttributes[i]);
			}
			int num2 = PrxGetSessionMemberListCount();
			result.members = new SessionMemberInfo[num2];
			result.memberAttributes = new List<SessionAttributeInfo[]>();
			for (int j = 0; j < num2; j++)
			{
				PrxGetSessionMemberInfo(j, out result.members[j]);
				int num3 = PrxGetSessionMemberAttributeListCount(j);
				SessionAttributeInfo[] array = new SessionAttributeInfo[num3];
				for (int k = 0; k < num3; k++)
				{
					PrxGetSessionMemberAttributeInfo(j, k, out array[k]);
				}
				result.memberAttributes.Add(array);
			}
			PrxUnlockSessionAttributeList();
			return result;
		}

		public static IntPtr GetSessionInformationPtr()
		{
			return PrxGetSessionInformationPtr();
		}

		public static ErrorCode LeaveSession()
		{
			return PrxSessionLeaveSession();
		}

		public static ErrorCode FindSession(int serverID, int worldID)
		{
			return PrxSessionFind(serverID, worldID);
		}

		public static ErrorCode FindSession(int serverID, int worldID, FlagSessionSearch flags)
		{
			return PrxSessionFindCustom(serverID, worldID, flags);
		}

		public static ErrorCode FindSessionFriends(int serverID, int worldID)
		{
			return PrxSessionFindFriends(serverID, worldID);
		}

		public static ErrorCode FindSessionRegional(int serverID, int worldID)
		{
			return PrxSessionFindRegional(serverID, worldID);
		}

		public static Session[] GetFoundSessionList()
		{
			PrxLockFoundSessionList();
			int num = PrxGetFoundSessionListCount();
			Session[] array = new Session[num];
			for (int i = 0; i < num; i++)
			{
				array[i].sessionInfo = default(SessionInfo);
				PrxGetFoundSessionInfo(i, out array[i].sessionInfo);
				int num2 = PrxGetFoundSessionAttributeListCount(i);
				array[i].sessionAttributes = new SessionAttributeInfo[num2];
				for (int j = 0; j < num2; j++)
				{
					PrxGetFoundSessionAttributeInfo(i, j, out array[i].sessionAttributes[j]);
				}
			}
			PrxUnlockFoundSessionList();
			return array;
		}

		public static ErrorCode InviteToSession(string text, int npIDCount)
		{
			return PrxInviteToSession(text, npIDCount);
		}

		public static ErrorCode GetSessionInviteSessionAttribute(string attributeName, out SessionAttributeInfo info)
		{
			return PrxGetSessionInviteSessionAttribute(attributeName, out info);
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_MatchingCreatedSession:
				hosting = true;
				if (Matching.OnCreatedSession != null)
				{
					Matching.OnCreatedSession(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingFoundSessions:
				if (Matching.OnFoundSessions != null)
				{
					Matching.OnFoundSessions(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingJoinInvalidSession:
				if (Matching.OnJoinInvalidSession != null)
				{
					Matching.OnJoinInvalidSession(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingJoinedSession:
				hosting = false;
				if (Matching.OnJoinedSession != null)
				{
					Matching.OnJoinedSession(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingUpdatedSession:
				if (Matching.OnUpdatedSession != null)
				{
					Matching.OnUpdatedSession(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingLeftSession:
				hosting = false;
				if (Matching.OnLeftSession != null)
				{
					Matching.OnLeftSession(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingRoomDestroyed:
				hosting = false;
				if (Matching.OnSessionDestroyed != null)
				{
					Matching.OnSessionDestroyed(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingKickedOut:
				hosting = false;
				if (Matching.OnKickedOut != null)
				{
					Matching.OnKickedOut(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_MatchingError:
				if (Matching.OnSessionError != null)
				{
					Matching.OnSessionError(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
