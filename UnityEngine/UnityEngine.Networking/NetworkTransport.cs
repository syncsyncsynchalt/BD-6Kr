using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking;

public sealed class NetworkTransport
{
	public static extern bool IsStarted
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	private NetworkTransport()
	{
	}

	public static int ConnectEndPoint(int hostId, EndPoint xboxOneEndPoint, int exceptionConnectionId, out byte error)
	{
		error = 0;
		byte[] array = new byte[4] { 95, 36, 19, 246 };
		if (xboxOneEndPoint == null)
		{
			throw new NullReferenceException("Null EndPoint provided");
		}
		if (xboxOneEndPoint.GetType().FullName != "UnityEngine.XboxOne.XboxOneEndPoint")
		{
			throw new ArgumentException("Endpoint of type XboxOneEndPoint required");
		}
		if (xboxOneEndPoint.AddressFamily != AddressFamily.InterNetworkV6)
		{
			throw new ArgumentException("XboxOneEndPoint has an invalid family");
		}
		SocketAddress socketAddress = xboxOneEndPoint.Serialize();
		if (socketAddress.Size != 14)
		{
			throw new ArgumentException("XboxOneEndPoint has an invalid size");
		}
		if (socketAddress[0] != 0 || socketAddress[1] != 0)
		{
			throw new ArgumentException("XboxOneEndPoint has an invalid family signature");
		}
		if (socketAddress[2] != array[0] || socketAddress[3] != array[1] || socketAddress[4] != array[2] || socketAddress[5] != array[3])
		{
			throw new ArgumentException("XboxOneEndPoint has an invalid signature");
		}
		byte[] array2 = new byte[8];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = socketAddress[6 + i];
		}
		IntPtr intPtr = new IntPtr(BitConverter.ToInt64(array2, 0));
		if (intPtr == IntPtr.Zero)
		{
			throw new ArgumentException("XboxOneEndPoint has an invalid SOCKET_STORAGE pointer");
		}
		byte[] array3 = new byte[2];
		Marshal.Copy(intPtr, array3, 0, array3.Length);
		AddressFamily addressFamily = (AddressFamily)((array3[1] << 8) + array3[0]);
		if (addressFamily != AddressFamily.InterNetworkV6)
		{
			throw new ArgumentException("XboxOneEndPoint has corrupt or invalid SOCKET_STORAGE pointer");
		}
		return Internal_ConnectEndPoint(hostId, intPtr, 128, exceptionConnectionId, out error);
	}

	public static void Init()
	{
		InitWithNoParameters();
	}

	public static void Init(GlobalConfig config)
	{
		InitWithParameters(new GlobalConfigInternal(config));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void InitWithNoParameters();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void InitWithParameters(GlobalConfigInternal config);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void Shutdown();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern string GetAssetId(GameObject go);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void AddSceneId(int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetNextSceneId();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void ConnectAsNetworkHost(int hostId, string address, int port, NetworkID network, SourceID source, NodeID node, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void DisconnectNetworkHost(int hostId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern NetworkEventType ReceiveRelayEventFromHost(int hostId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, int bytesPerSec, float bucketSizeFactor, out byte error);

	public static int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, out byte error)
	{
		return ConnectToNetworkPeer(hostId, address, port, exceptionConnectionId, relaySlotId, network, source, node, 0, 0f, out error);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetCurrentIncomingMessageAmount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetCurrentOutgoingMessageAmount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetCurrentRtt(int hostId, int connectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetNetworkLostPacketNum(int hostId, int connectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetPacketSentRate(int hostId, int connectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetPacketReceivedRate(int hostId, int connectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("GetRemotePacketReceivedRate has been made obsolete. Please do not use this function.")]
	[WrapperlessIcall]
	public static extern int GetRemotePacketReceivedRate(int hostId, int connectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetNetIOTimeuS();

	public static void GetConnectionInfo(int hostId, int connectionId, out string address, out int port, out NetworkID network, out NodeID dstNode, out byte error)
	{
		address = GetConnectionInfo(hostId, connectionId, out port, out var network2, out var dstNode2, out error);
		network = (NetworkID)network2;
		dstNode = (NodeID)dstNode2;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern string GetConnectionInfo(int hostId, int connectionId, out int port, out ulong network, out ushort dstNode, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetNetworkTimestamp();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetRemoteDelayTimeMS(int hostId, int connectionId, int remoteTime, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool StartSendMulticast(int hostId, int channelId, byte[] buffer, int size, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool SendMulticast(int hostId, int connectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool FinishSendMulticast(int hostId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int GetMaxPacketSize();

	private static void CheckTopology(HostTopology topology)
	{
		int maxPacketSize = GetMaxPacketSize();
		if (topology.DefaultConfig.PacketSize > maxPacketSize)
		{
			throw new ArgumentOutOfRangeException("Default config: packet size should be less than packet size defined in global config: " + maxPacketSize);
		}
		for (int i = 0; i < topology.SpecialConnectionConfigs.Count; i++)
		{
			if (topology.SpecialConnectionConfigs[i].PacketSize > maxPacketSize)
			{
				throw new ArgumentOutOfRangeException("Special config " + i + ": packet size should be less than packet size defined in global config: " + maxPacketSize);
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int AddWsHostWrapper(HostTopologyInternal topologyInt, string ip, int port);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int AddWsHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port);

	[ExcludeFromDocs]
	public static int AddWebsocketHost(HostTopology topology, int port)
	{
		string ip = null;
		return AddWebsocketHost(topology, port, ip);
	}

	public static int AddWebsocketHost(HostTopology topology, int port, [DefaultValue("null")] string ip)
	{
		if (topology == null)
		{
			throw new NullReferenceException("topology is not defined");
		}
		if (ip == null)
		{
			return AddWsHostWrapperWithoutIp(new HostTopologyInternal(topology), port);
		}
		return AddWsHostWrapper(new HostTopologyInternal(topology), ip, port);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int AddHostWrapper(HostTopologyInternal topologyInt, string ip, int port, int minTimeout, int maxTimeout);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int AddHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port, int minTimeout, int maxTimeout);

	[ExcludeFromDocs]
	public static int AddHost(HostTopology topology, int port)
	{
		string ip = null;
		return AddHost(topology, port, ip);
	}

	[ExcludeFromDocs]
	public static int AddHost(HostTopology topology)
	{
		string ip = null;
		int port = 0;
		return AddHost(topology, port, ip);
	}

	public static int AddHost(HostTopology topology, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
	{
		if (topology == null)
		{
			throw new NullReferenceException("topology is not defined");
		}
		if (ip == null)
		{
			return AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, 0, 0);
		}
		return AddHostWrapper(new HostTopologyInternal(topology), ip, port, 0, 0);
	}

	[ExcludeFromDocs]
	public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, int port)
	{
		string ip = null;
		return AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
	}

	[ExcludeFromDocs]
	public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout)
	{
		string ip = null;
		int port = 0;
		return AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
	}

	public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
	{
		if (topology == null)
		{
			throw new NullReferenceException("topology is not defined");
		}
		if (ip == null)
		{
			return AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, minTimeout, maxTimeout);
		}
		return AddHostWrapper(new HostTopologyInternal(topology), ip, port, minTimeout, maxTimeout);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool RemoveHost(int hostId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int Connect(int hostId, string address, int port, int exeptionConnectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int Internal_ConnectEndPoint(int hostId, IntPtr sockAddrStorage, int sockAddrStorageLen, int exceptionConnectionId, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int ConnectWithSimulator(int hostId, string address, int port, int exeptionConnectionId, out byte error, ConnectionSimulatorConfig conf);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool Disconnect(int hostId, int connectionId, out byte error);

	public static bool Send(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error)
	{
		if (buffer == null)
		{
			throw new NullReferenceException("send buffer is not initialized");
		}
		return SendWrapper(hostId, connectionId, channelId, buffer, size, out error);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern bool SendWrapper(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern NetworkEventType Receive(out int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern NetworkEventType ReceiveFromHost(int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetPacketStat(int direction, int packetStatId, int numMsgs, int numBytes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool StartBroadcastDiscovery(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void StopBroadcastDiscovery();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool IsBroadcastDiscoveryRunning();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetBroadcastCredentials(int hostId, int key, int version, int subversion, out byte error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern string GetBroadcastConnectionInfo(int hostId, out int port, out byte error);

	public static void GetBroadcastConnectionInfo(int hostId, out string address, out int port, out byte error)
	{
		address = GetBroadcastConnectionInfo(hostId, out port, out error);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void GetBroadcastConnectionMessage(int hostId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);
}
