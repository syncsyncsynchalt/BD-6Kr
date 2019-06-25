using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	public sealed class Network
	{
		public static string incomingPassword
		{
			get;
			set;
		}

		public static NetworkLogLevel logLevel
		{
			get;
			set;
		}

		public static NetworkPlayer[] connections
		{
			get;
		}

		public static NetworkPlayer player
		{
			get
			{
				NetworkPlayer result = default(NetworkPlayer);
				result.index = Internal_GetPlayer();
				return result;
			}
		}

		public static bool isClient
		{
			get;
		}

		public static bool isServer
		{
			get;
		}

		public static NetworkPeerType peerType
		{
			get;
		}

		public static float sendRate
		{
			get;
			set;
		}

		public static bool isMessageQueueRunning
		{
			get;
			set;
		}

		public static double time
		{
			get
			{
				Internal_GetTime(out double t);
				return t;
			}
		}

		public static int minimumAllocatableViewIDs
		{
			get;
			set;
		}

		[Obsolete("No longer needed. This is now explicitly set in the InitializeServer function call. It is implicitly set when calling Connect depending on if an IP/port combination is used (useNat=false) or a GUID is used(useNat=true).")]
		public static bool useNat
		{
			get;
			set;
		}

		public static string natFacilitatorIP
		{
			get;
			set;
		}

		public static int natFacilitatorPort
		{
			get;
			set;
		}

		public static string connectionTesterIP
		{
			get;
			set;
		}

		public static int connectionTesterPort
		{
			get;
			set;
		}

		public static int maxConnections
		{
			get;
			set;
		}

		public static string proxyIP
		{
			get;
			set;
		}

		public static int proxyPort
		{
			get;
			set;
		}

		public static bool useProxy
		{
			get;
			set;
		}

		public static string proxyPassword
		{
			get;
			set;
		}

		public static NetworkConnectionError InitializeServer(int connections, int listenPort, bool useNat) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static NetworkConnectionError Internal_InitializeServerDeprecated(int connections, int listenPort) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use the IntializeServer(connections, listenPort, useNat) function instead")]
		public static NetworkConnectionError InitializeServer(int connections, int listenPort)
		{
			return Internal_InitializeServerDeprecated(connections, listenPort);
		}

		public static void InitializeSecurity() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static NetworkConnectionError Internal_ConnectToSingleIP(string IP, int remotePort, int localPort, [DefaultValue("\"\"")] string password) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		private static NetworkConnectionError Internal_ConnectToSingleIP(string IP, int remotePort, int localPort)
		{
			string empty = string.Empty;
			return Internal_ConnectToSingleIP(IP, remotePort, localPort, empty);
		}

		private static NetworkConnectionError Internal_ConnectToGuid(string guid, string password) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static NetworkConnectionError Internal_ConnectToIPs(string[] IP, int remotePort, int localPort, [DefaultValue("\"\"")] string password) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		private static NetworkConnectionError Internal_ConnectToIPs(string[] IP, int remotePort, int localPort)
		{
			string empty = string.Empty;
			return Internal_ConnectToIPs(IP, remotePort, localPort, empty);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(string IP, int remotePort)
		{
			string empty = string.Empty;
			return Connect(IP, remotePort, empty);
		}

		public static NetworkConnectionError Connect(string IP, int remotePort, [DefaultValue("\"\"")] string password)
		{
			return Internal_ConnectToSingleIP(IP, remotePort, 0, password);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(string[] IPs, int remotePort)
		{
			string empty = string.Empty;
			return Connect(IPs, remotePort, empty);
		}

		public static NetworkConnectionError Connect(string[] IPs, int remotePort, [DefaultValue("\"\"")] string password)
		{
			return Internal_ConnectToIPs(IPs, remotePort, 0, password);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(string GUID)
		{
			string empty = string.Empty;
			return Connect(GUID, empty);
		}

		public static NetworkConnectionError Connect(string GUID, [DefaultValue("\"\"")] string password)
		{
			return Internal_ConnectToGuid(GUID, password);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(HostData hostData)
		{
			string empty = string.Empty;
			return Connect(hostData, empty);
		}

		public static NetworkConnectionError Connect(HostData hostData, [DefaultValue("\"\"")] string password)
		{
			if (hostData == null)
			{
				throw new NullReferenceException();
			}
			if (hostData.guid.Length > 0 && hostData.useNat)
			{
				return Connect(hostData.guid, password);
			}
			return Connect(hostData.ip, hostData.port, password);
		}

		public static void Disconnect([DefaultValue("200")] int timeout) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void Disconnect()
		{
			int timeout = 200;
			Disconnect(timeout);
		}

		public static void CloseConnection(NetworkPlayer target, bool sendDisconnectionNotification) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetPlayer() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_AllocateViewID(out NetworkViewID viewID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static NetworkViewID AllocateViewID()
		{
			Internal_AllocateViewID(out NetworkViewID viewID);
			return viewID;
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object prefab, Vector3 position, Quaternion rotation, int group)
		{
			return INTERNAL_CALL_Instantiate(prefab, ref position, ref rotation, group);
		}

		private static Object INTERNAL_CALL_Instantiate(Object prefab, ref Vector3 position, ref Quaternion rotation, int group) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Destroy(NetworkViewID viewID)
		{
			INTERNAL_CALL_Destroy(ref viewID);
		}

		private static void INTERNAL_CALL_Destroy(ref NetworkViewID viewID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Destroy(GameObject gameObject)
		{
			if (gameObject != null)
			{
				NetworkView component = gameObject.GetComponent<NetworkView>();
				if (component != null)
				{
					Destroy(component.viewID);
				}
				else
				{
					Debug.LogError("Couldn't destroy game object because no network view is attached to it.", gameObject);
				}
			}
		}

		public static void DestroyPlayerObjects(NetworkPlayer playerID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_RemoveRPCs(NetworkPlayer playerID, NetworkViewID viewID, uint channelMask)
		{
			INTERNAL_CALL_Internal_RemoveRPCs(playerID, ref viewID, channelMask);
		}

		private static void INTERNAL_CALL_Internal_RemoveRPCs(NetworkPlayer playerID, ref NetworkViewID viewID, uint channelMask) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void RemoveRPCs(NetworkPlayer playerID)
		{
			Internal_RemoveRPCs(playerID, NetworkViewID.unassigned, uint.MaxValue);
		}

		public static void RemoveRPCs(NetworkPlayer playerID, int group)
		{
			Internal_RemoveRPCs(playerID, NetworkViewID.unassigned, (uint)(1 << group));
		}

		public static void RemoveRPCs(NetworkViewID viewID)
		{
			Internal_RemoveRPCs(NetworkPlayer.unassigned, viewID, uint.MaxValue);
		}

		public static void RemoveRPCsInGroup(int group)
		{
			Internal_RemoveRPCs(NetworkPlayer.unassigned, NetworkViewID.unassigned, (uint)(1 << group));
		}

		public static void SetLevelPrefix(int prefix) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetLastPing(NetworkPlayer player) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetAveragePing(NetworkPlayer player) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetReceivingEnabled(NetworkPlayer player, int group, bool enabled) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetSendingGlobal(int group, bool enabled) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetSendingSpecific(NetworkPlayer player, int group, bool enabled) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetSendingEnabled(int group, bool enabled)
		{
			Internal_SetSendingGlobal(group, enabled);
		}

		public static void SetSendingEnabled(NetworkPlayer player, int group, bool enabled)
		{
			Internal_SetSendingSpecific(player, group, enabled);
		}

		private static void Internal_GetTime(out double t) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static ConnectionTesterStatus TestConnection([DefaultValue("false")] bool forceTest) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static ConnectionTesterStatus TestConnection()
		{
			bool forceTest = false;
			return TestConnection(forceTest);
		}

		public static ConnectionTesterStatus TestConnectionNAT([DefaultValue("false")] bool forceTest) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static ConnectionTesterStatus TestConnectionNAT()
		{
			bool forceTest = false;
			return TestConnectionNAT(forceTest);
		}

		public static bool HavePublicAddress() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
