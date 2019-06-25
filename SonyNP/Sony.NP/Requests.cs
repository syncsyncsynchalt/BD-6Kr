using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Requests
	{
		[Flags]
		public enum PlusFeature
		{
			NONE = 0x0,
			REALTIME_MULTIPLAY = 0x1,
			ASYNC_MULTIPLAY = 0x2
		}

		public static event Messages.EventHandler OnCheckPlusResult;

		public static event Messages.EventHandler OnAccountLanguageResult;

		public static event Messages.EventHandler OnParentalControlResult;

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpRequestCheckPlus(int userid, ulong features);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpRequestCheckPlusDefaultUser(ulong features);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpRequestNotifyPlusFeature(int userid, ulong features);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxNpRequestNotifyPlusFeatureDefaultUser(ulong features);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxNpRequestGetAccountLanguage(string onlineID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxNpRequestGetParentalControlInfo(string onlineID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxNpRequestCheckNpAvailability(string onlineID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxNpRequestSetGamePresenceOnline(string onlineID);

		public static ErrorCode CheckPlus(int userid, PlusFeature features)
		{
			return PrxNpRequestCheckPlus(userid, (ulong)features);
		}

		public static ErrorCode CheckPlus(PlusFeature features)
		{
			return PrxNpRequestCheckPlusDefaultUser((ulong)features);
		}

		public static ErrorCode NotifyPlusFeature(int userid, PlusFeature features)
		{
			return PrxNpRequestNotifyPlusFeature(userid, (ulong)features);
		}

		public static ErrorCode NotifyPlusFeature(PlusFeature features)
		{
			return PrxNpRequestNotifyPlusFeatureDefaultUser((ulong)features);
		}

		public static ErrorCode GetAccountLanguage(string onlineId)
		{
			return PrxNpRequestGetAccountLanguage(onlineId);
		}

		public static ErrorCode GetParentalControlInfo(string onlineId)
		{
			return PrxNpRequestGetParentalControlInfo(onlineId);
		}

		public static ErrorCode CheckNpAvailability(string onlineId)
		{
			if (string.IsNullOrEmpty(onlineId))
			{
				throw new ArgumentException("onlineId is null or empty");
			}
			return PrxNpRequestCheckNpAvailability(onlineId);
		}

		public static ErrorCode SetGamePresenceOnline(string onlineId)
		{
			return PrxNpRequestSetGamePresenceOnline(onlineId);
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_CheckPlusResult:
				if (Requests.OnCheckPlusResult != null)
				{
					Requests.OnCheckPlusResult(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_AccountLanguageResult:
				if (Requests.OnAccountLanguageResult != null)
				{
					Requests.OnAccountLanguageResult(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_ParentalControlResult:
				if (Requests.OnParentalControlResult != null)
				{
					Requests.OnParentalControlResult(msg);
				}
				return true;
			default:
				return false;
			}
		}

		public static void GetCheckPlusResult(Messages.PluginMessage msg, out bool CheckPlusResult, out int userId)
		{
			CheckPlusResult = false;
			int ofs = 64;
			if (Marshal.ReadByte(msg.data, ofs) != 0)
			{
				CheckPlusResult = true;
			}
			int ofs2 = 16;
			userId = Marshal.ReadInt32(msg.data, ofs2);
		}

		public static string GetAccountLanguageResult(Messages.PluginMessage msg)
		{
			int num = 28;
			IntPtr ptr = new IntPtr(msg.data.ToInt64() + num);
			return Marshal.PtrToStringAnsi(ptr);
		}

		public static string GetRequestOnlineId(Messages.PluginMessage msg)
		{
			int num = 8;
			IntPtr ptr = new IntPtr(msg.data.ToInt64() + num);
			return Marshal.PtrToStringAnsi(ptr);
		}

		public static byte[] GetRequestResultData(Messages.PluginMessage msg)
		{
			byte[] array = new byte[msg.dataSize];
			Marshal.Copy(msg.data, array, 0, msg.dataSize);
			return array;
		}

		public static void GetParentalControlInfoResult(Messages.PluginMessage msg, out int Age, out bool chatRestriction, out bool ugcRestriction)
		{
			byte[] array = new byte[msg.dataSize];
			Marshal.Copy(msg.data, array, 0, msg.dataSize);
			Age = array[28];
			chatRestriction = ((array[30] == 1) ? true : false);
			ugcRestriction = ((array[31] == 1) ? true : false);
		}
	}
}
