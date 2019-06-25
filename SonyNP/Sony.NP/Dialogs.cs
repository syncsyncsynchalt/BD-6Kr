using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Dialogs
	{
		public enum EnumNpDlgResult
		{
			NP_DLG_CANCELED,
			NP_DLG_OK
		}

		public enum CommerceDialogMode
		{
			CATEGORY,
			PRODUCT,
			PRODUCE_CODE,
			CHECKOUT,
			DOWNLOADLIST,
			PLUS
		}

		public struct NpDialogReturn
		{
			private IntPtr _npID;

			private int npIDSize;

			public bool plusAllowed;

			public EnumNpDlgResult result;

			public byte[] npID
			{
				get
				{
					byte[] destination = new byte[npIDSize];
					Marshal.Copy(_npID, destination, 0, npIDSize);
					return destination;
				}
			}
		}

		public static bool IsDialogOpen => PrxNpIsDialogOpen();

		public static event Messages.EventHandler OnDlgFriendsListClosed;

		public static event Messages.EventHandler OnDlgSharedPlayHistoryClosed;

		public static event Messages.EventHandler OnDlgProfileClosed;

		public static event Messages.EventHandler OnDlgCommerceClosed;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxNpDialogGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxNpDialogGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxNpIsDialogOpen();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpDialogFriendsList();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpDialogSharedPlayHistory();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpDialogProfile(byte[] npID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpDialogCommerce(int mode, ulong features);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpDialogGetResult(out NpDialogReturn result);

		public static ErrorCode FriendsList()
		{
			return PrxNpDialogFriendsList();
		}

		public static ErrorCode SharedPlayHistory()
		{
			return PrxNpDialogSharedPlayHistory();
		}

		public static ErrorCode Profile(byte[] npID)
		{
			return PrxNpDialogProfile(npID);
		}

		public static ErrorCode Commerce(CommerceDialogMode mode, Requests.PlusFeature features)
		{
			return PrxNpDialogCommerce((int)mode, (ulong)features);
		}

		public static NpDialogReturn GetDialogResult()
		{
			NpDialogReturn result = default(NpDialogReturn);
			PrxNpDialogGetResult(out result);
			return result;
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_DlgFriendsClosed:
				if (Dialogs.OnDlgFriendsListClosed != null)
				{
					Dialogs.OnDlgFriendsListClosed(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_DlgSharedPlayHistoryClosed:
				if (Dialogs.OnDlgSharedPlayHistoryClosed != null)
				{
					Dialogs.OnDlgSharedPlayHistoryClosed(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_DlgCommerceClosed:
				if (Dialogs.OnDlgCommerceClosed != null)
				{
					Dialogs.OnDlgCommerceClosed(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_DlgProfileClosed:
				if (Dialogs.OnDlgProfileClosed != null)
				{
					Dialogs.OnDlgProfileClosed(msg);
				}
				break;
			}
			return false;
		}
	}
}
