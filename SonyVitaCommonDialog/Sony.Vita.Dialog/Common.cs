using System.Runtime.InteropServices;

namespace Sony.Vita.Dialog
{
	public class Common
	{
		public enum EnumSystemMessageType
		{
			MSG_DIALOG_SYSMSG_TYPE_WAIT = 1,
			MSG_DIALOG_SYSMSG_TYPE_NOSPACE = 2,
			MSG_DIALOG_SYSMSG_TYPE_MAGNETIC_CALIBRATION = 3,
			MSG_DIALOG_SYSMSG_TYPE_WAIT_SMALL = 5,
			MSG_DIALOG_SYSMSG_TYPE_WAIT_CANCEL = 6,
			MSG_DIALOG_SYSMSG_TYPE_NOSPACE_CONTINUABLE = 9,
			MSG_DIALOG_SYSMSG_TYPE_LOCATION_DATA_OBTAINING = 10,
			MSG_DIALOG_SYSMSG_TYPE_LOCATION_DATA_FAILURE = 11,
			MSG_DIALOG_SYSMSG_TYPE_LOCATION_DATA_FAILURE_RETRY = 12,
			MSG_DIALOG_SYSMSG_TYPE_PATCH_FOUND = 13,
			MSG_DIALOG_SYSMSG_TYPE_TRC_MIC_DISABLED = 100,
			MSG_DIALOG_SYSMSG_TYPE_TRC_WIFI_REQUIRED_OPERATION = 101,
			MSG_DIALOG_SYSMSG_TYPE_TRC_WIFI_REQUIRED_APPLICATION = 102,
			MSG_DIALOG_SYSMSG_TYPE_TRC_EMPTY_STORE = 103,
			MSG_DIALOG_SYSMSG_TYPE_TRC_PSN_AGE_RESTRICTION = 104,
			MSG_DIALOG_SYSMSG_TYPE_TRC_PSN_CHAT_RESTRICTION = 105,
			MSG_DIALOG_SYSMSG_TYPE_TRC_MIC_DISABLED_CONTINUABLE = 106
		}

		public enum EnumUserMessageType
		{
			MSG_DIALOG_BUTTON_TYPE_OK,
			MSG_DIALOG_BUTTON_TYPE_YESNO,
			MSG_DIALOG_BUTTON_TYPE_NONE,
			MSG_DIALOG_BUTTON_TYPE_OK_CANCEL,
			MSG_DIALOG_BUTTON_TYPE_CANCEL,
			MSG_DIALOG_BUTTON_TYPE_3BUTTONS
		}

		public enum EnumCommonDialogResult
		{
			RESULT_BUTTON_NOT_SET,
			RESULT_BUTTON_OK,
			RESULT_BUTTON_CANCEL,
			RESULT_BUTTON_YES,
			RESULT_BUTTON_NO,
			RESULT_BUTTON_1,
			RESULT_BUTTON_2,
			RESULT_BUTTON_3,
			RESULT_CANCELED,
			RESULT_ABORTED,
			RESULT_CLOSED
		}

		public static bool IsDialogOpen => PrxCommonDialogIsDialogOpen();

		public static event Messages.EventHandler OnGotDialogResult;

		[DllImport("CommonDialog")]
		private static extern int PrxCommonDialogInitialise();

		[DllImport("CommonDialog")]
		private static extern void PrxCommonDialogUpdate();

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogIsDialogOpen();

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogErrorMessage(uint errorCode);

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogSystemMessage(EnumSystemMessageType type, bool infobar, int value);

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogClose();

		[DllImport("CommonDialog", CharSet = CharSet.Ansi)]
		private static extern bool PrxCommonDialogProgressBar(string str);

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogProgressBarSetPercent(int percent);

		[DllImport("CommonDialog", CharSet = CharSet.Ansi)]
		private static extern bool PrxCommonDialogProgressBarSetMessage(string str);

		[DllImport("CommonDialog", CharSet = CharSet.Ansi)]
		private static extern bool PrxCommonDialogUserMessage(EnumUserMessageType type, bool infobar, string str, string button1, string button2, string button3);

		[DllImport("CommonDialog")]
		private static extern EnumCommonDialogResult PrxCommonDialogGetResult();

		public static bool ShowErrorMessage(uint errorCode)
		{
			return PrxCommonDialogErrorMessage(errorCode);
		}

		public static bool ShowSystemMessage(EnumSystemMessageType type, bool infoBar, int value)
		{
			return PrxCommonDialogSystemMessage(type, infoBar, value);
		}

		public static bool ShowProgressBar(string message)
		{
			return PrxCommonDialogProgressBar(message);
		}

		public static bool SetProgressBarPercent(int percent)
		{
			return PrxCommonDialogProgressBarSetPercent(percent);
		}

		public static bool SetProgressBarMessage(string message)
		{
			return PrxCommonDialogProgressBarSetMessage(message);
		}

		public static bool ShowUserMessage(EnumUserMessageType type, bool infoBar, string str)
		{
			return PrxCommonDialogUserMessage(type, infoBar, str, "1", "2", "3");
		}

		public static bool ShowUserMessage3Button(bool infoBar, string str, string button1, string button2, string button3)
		{
			return PrxCommonDialogUserMessage(EnumUserMessageType.MSG_DIALOG_BUTTON_TYPE_3BUTTONS, infoBar, str, button1, button2, button3);
		}

		public static bool Close()
		{
			return PrxCommonDialogClose();
		}

		public static EnumCommonDialogResult GetResult()
		{
			return PrxCommonDialogGetResult();
		}

		public static void ProcessMessage(Messages.PluginMessage msg)
		{
			Messages.MessageType type = msg.type;
			if (type == Messages.MessageType.kDialog_GotDialogResult && Common.OnGotDialogResult != null)
			{
				Common.OnGotDialogResult(msg);
			}
		}

		public static void Initialise()
		{
			PrxCommonDialogInitialise();
		}

		public static void Update()
		{
			PrxCommonDialogUpdate();
		}
	}
}
