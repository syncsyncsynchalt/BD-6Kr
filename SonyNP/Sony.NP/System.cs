using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class System
	{
		public struct Bandwidth
		{
			public float uploadBPS;

			public float downloadBPS;
		}

		public enum EnumConnectionStatus
		{
			NET_CTL_STATE_DISCONNECTED,
			NET_CTL_STATE_CONNECTING,
			NET_CTL_STATE_IPOBTAINING,
			NET_CTL_STATE_IPOBTAINED
		}

		public enum EnumNatStunStatus
		{
			NET_CTL_NATINFO_STUN_UNCHECKED,
			NET_CTL_NATINFO_STUN_FAILED,
			NET_CTL_NATINFO_STUN_OK
		}

		public enum EnumNatType
		{
			NET_CTL_NATINFO_NAT_TYPE_1 = 1,
			NET_CTL_NATINFO_NAT_TYPE_2,
			NET_CTL_NATINFO_NAT_TYPE_3
		}

		public struct NetInfoBasic
		{
			public EnumConnectionStatus connectionStatus;

			private IntPtr _ipAddress;

			public EnumNatType natType;

			public EnumNatStunStatus natStunStatus;

			public int natMappedAddr;

			public string ipAddress => Marshal.PtrToStringAnsi(_ipAddress);
		}

		public enum NetDeviceType
		{
			Wireless,
			Wired,
			Phone
		}

		public static bool connectionUp = true;

		public static bool IsConnected => connectionUp;

		public static event Messages.EventHandler OnSysEvent;

		public static event Messages.EventHandler OnSysResume;

		public static event Messages.EventHandler OnSysNpMessageArrived;

		public static event Messages.EventHandler OnSysStorePurchase;

		public static event Messages.EventHandler OnSysStoreRedemption;

		public static event Messages.EventHandler OnConnectionUp;

		public static event Messages.EventHandler OnConnectionDown;

		public static event Messages.EventHandler OnGotBandwidth;

		public static event Messages.EventHandler OnGotNetInfo;

		public static event Messages.EventHandler OnNetInfoError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxNetInfoIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxNetInfoGetLastError(out ResultCode result);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxNetCtlGetLastConnectionError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxNetInfoGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNetInfoRequestBandwidth();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNetInfoGetBandwidth(out Bandwidth bandwidth);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNetInfoRequestInfo();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNetInfoGetInfo(out NetInfoBasic info);

		[DllImport("UnityNpToolkit")]
		private static extern NetDeviceType PrxNetInfoGetDeviceType();

		public static NetDeviceType GetNetworkDeviceType()
		{
			return PrxNetInfoGetDeviceType();
		}

		[DllImport("UnityNpToolkit")]
		private static extern long PrxGetNetworkTime();

		public static bool RequestBandwidthInfoIsBusy()
		{
			return PrxNetInfoIsBusy();
		}

		public static ErrorCode RequestBandwidthInfo()
		{
			return PrxNetInfoRequestBandwidth();
		}

		public static Bandwidth GetBandwidthInfo()
		{
			Bandwidth bandwidth = default(Bandwidth);
			PrxNetInfoGetBandwidth(out bandwidth);
			return bandwidth;
		}

		public static ErrorCode RequestNetInfo()
		{
			return PrxNetInfoRequestInfo();
		}

		public static NetInfoBasic GetNetInfo()
		{
			NetInfoBasic info = default(NetInfoBasic);
			PrxNetInfoGetInfo(out info);
			return info;
		}

		public static long GetNetworkTime()
		{
			return PrxGetNetworkTime();
		}

		public static bool GetLastConnectionError(out ResultCode result)
		{
			PrxNetCtlGetLastConnectionError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_ConnectionUp:
				connectionUp = true;
				if (System.OnConnectionUp != null)
				{
					System.OnConnectionUp(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_ConnectionDown:
				connectionUp = false;
				if (System.OnConnectionDown != null)
				{
					System.OnConnectionDown(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_SysEvent:
				if (System.OnSysEvent != null)
				{
					System.OnSysEvent(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_SysResume:
				if (System.OnSysResume != null)
				{
					System.OnSysResume(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_SysNpMessageArrived:
				if (System.OnSysNpMessageArrived != null)
				{
					System.OnSysNpMessageArrived(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_SysStoreRedemption:
				if (System.OnSysStoreRedemption != null)
				{
					System.OnSysStoreRedemption(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_SysStorePurchase:
				if (System.OnSysStorePurchase != null)
				{
					System.OnSysStorePurchase(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_NetInfoGotBandwidth:
				if (System.OnGotBandwidth != null)
				{
					System.OnGotBandwidth(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_NetInfoGotBasic:
				if (System.OnGotNetInfo != null)
				{
					System.OnGotNetInfo(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_NetInfoError:
				if (System.OnNetInfoError != null)
				{
					System.OnNetInfoError(msg);
				}
				break;
			}
			return false;
		}
	}
}
