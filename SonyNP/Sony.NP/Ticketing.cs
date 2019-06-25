using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Ticketing
	{
		public struct Ticket
		{
			private IntPtr _data;

			public int dataSize;

			public byte[] data
			{
				get
				{
					byte[] array = new byte[dataSize];
					Marshal.Copy(_data, array, 0, dataSize);
					return array;
				}
			}
		}

		public struct TicketEntitlement
		{
			private IntPtr _id;

			public int type;

			public int remainingCount;

			public int consumedCount;

			public ulong createdDate;

			public ulong expireDate;

			public string id => Marshal.PtrToStringAnsi(_id);
		}

		public struct TicketEntitlementArray
		{
			private IntPtr _data;

			public int count;

			public TicketEntitlement[] entitlements
			{
				get
				{
					TicketEntitlement[] array = new TicketEntitlement[count];
					int num = Marshal.SizeOf(typeof(TicketEntitlement));
					for (int i = 0; i < count; i++)
					{
						IntPtr ptr = new IntPtr(_data.ToInt64() + num * i);
						array[i] = (TicketEntitlement)Marshal.PtrToStructure(ptr, typeof(TicketEntitlement));
					}
					return array;
				}
			}
		}

		public struct TicketInfo
		{
			public long issuedDate;

			public long expireDate;

			public long subjectAccountID;

			public long statusDuration;

			private int serialIDSize;

			private IntPtr _serialID;

			private IntPtr _subjectOnlineID;

			private IntPtr _countryCode;

			private IntPtr _subjectDomain;

			private IntPtr _serviceID;

			public int issuerID;

			public int languageCode;

			public int subjectAge;

			public int chatDisabled;

			public int contentRating;

			public byte[] serialID
			{
				get
				{
					byte[] array = new byte[serialIDSize];
					Marshal.Copy(_serialID, array, 0, serialIDSize);
					return array;
				}
			}

			public string subjectOnlineID => Marshal.PtrToStringAnsi(_subjectOnlineID);

			public string countryCode => Marshal.PtrToStringAnsi(_countryCode);

			public string subjectDomain => Marshal.PtrToStringAnsi(_subjectDomain);

			public string serviceID => Marshal.PtrToStringAnsi(_serviceID);
		}

		public static event Messages.EventHandler OnGotTicket;

		public static event Messages.EventHandler OnError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTicketingIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTicketingGetLastError(out ResultCode result);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTicketingRequestTicket();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTicketingRequestCachedTicket();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTicketingGetTicket(out Ticket ticket);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTicketingGetTicketInfo(ref Ticket ticket, out TicketInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTicketingGetEntitlementList(ref Ticket ticket, out TicketEntitlementArray result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxTicketingGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		public static bool IsBusy()
		{
			return PrxTicketingIsBusy();
		}

		public static ErrorCode RequestTicket()
		{
			return PrxTicketingRequestTicket();
		}

		public static ErrorCode RequestCachedTicket()
		{
			return PrxTicketingRequestCachedTicket();
		}

		public static Ticket GetTicket()
		{
			Ticket ticket = default(Ticket);
			PrxTicketingGetTicket(out ticket);
			return ticket;
		}

		public static TicketInfo GetTicketInfo(Ticket ticket)
		{
			TicketInfo info = default(TicketInfo);
			PrxTicketingGetTicketInfo(ref ticket, out info);
			return info;
		}

		public static TicketEntitlement[] GetTicketEntitlements(Ticket ticket)
		{
			TicketEntitlementArray result = default(TicketEntitlementArray);
			PrxTicketingGetEntitlementList(ref ticket, out result);
			return result.entitlements;
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_TicketingGotTicket:
				if (Ticketing.OnGotTicket != null)
				{
					Ticketing.OnGotTicket(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TicketingError:
				if (Ticketing.OnError != null)
				{
					Ticketing.OnError(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
