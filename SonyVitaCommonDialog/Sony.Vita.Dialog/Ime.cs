using System;
using System.Runtime.InteropServices;

namespace Sony.Vita.Dialog
{
	public class Ime
	{
		public enum EnumImeDialogEnterLabel
		{
			ENTER_LABEL_DEFAULT,
			ENTER_LABEL_SEND,
			ENTER_LABEL_SEARCH,
			ENTER_LABEL_GO
		}

		public enum EnumImeDialogType
		{
			TYPE_DEFAULT,
			TYPE_BASIC_LATIN,
			TYPE_NUMBER,
			TYPE_EXTENDED_NUMBER,
			TYPE_URL,
			TYPE_MAIL
		}

		[Flags]
		public enum FlagsTextBoxMode
		{
			TEXTBOX_MODE_DEFAULT = 0x0,
			TEXTBOX_MODE_PASSWORD = 0x1,
			TEXTBOX_MODE_WITH_CLEAR = 0x2
		}

		[Flags]
		public enum FlagsSupportedLanguages
		{
			LANGUAGE_DANISH = 0x1,
			LANGUAGE_GERMAN = 0x2,
			LANGUAGE_ENGLISH_US = 0x4,
			LANGUAGE_SPANISH = 0x8,
			LANGUAGE_FRENCH = 0x10,
			LANGUAGE_ITALIAN = 0x20,
			LANGUAGE_DUTCH = 0x40,
			LANGUAGE_NORWEGIAN = 0x80,
			LANGUAGE_POLISH = 0x100,
			LANGUAGE_PORTUGUESE_PT = 0x200,
			LANGUAGE_RUSSIAN = 0x400,
			LANGUAGE_FINNISH = 0x800,
			LANGUAGE_SWEDISH = 0x1000,
			LANGUAGE_JAPANESE = 0x2000,
			LANGUAGE_KOREAN = 0x4000,
			LANGUAGE_SIMPLIFIED_CHINESE = 0x8000,
			LANGUAGE_TRADITIONAL_CHINESE = 0x10000,
			LANGUAGE_PORTUGUESE_BR = 0x20000,
			LANGUAGE_ENGLISH_GB = 0x40000,
			LANGUAGE_TURKISH = 0x80000
		}

		[Flags]
		public enum FlagsTextBoxOption
		{
			OPTION_DEFAULT = 0x0,
			OPTION_MULTILINE = 0x1,
			OPTION_NO_AUTO_CAPITALIZATION = 0x2,
			OPTION_NO_ASSISTANCE = 0x4
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class ImeDialogParams
		{
			public FlagsSupportedLanguages supportedLanguages;

			public bool languagesForced;

			public EnumImeDialogType type;

			public FlagsTextBoxOption option;

			public bool canCancel;

			public FlagsTextBoxMode textBoxMode;

			public EnumImeDialogEnterLabel enterLabel;

			public int maxTextLength;

			private IntPtr _title;

			private IntPtr _initialText;

			public string title
			{
				get
				{
					return Marshal.PtrToStringUni(_title);
				}
				set
				{
					_title = Marshal.StringToCoTaskMemUni(value);
				}
			}

			public string initialText
			{
				get
				{
					return Marshal.PtrToStringUni(_initialText);
				}
				set
				{
					_initialText = Marshal.StringToCoTaskMemUni(value);
				}
			}

			~ImeDialogParams()
			{
				Marshal.FreeCoTaskMem(_title);
				Marshal.FreeCoTaskMem(_initialText);
			}
		}

		public enum EnumImeDialogResult
		{
			RESULT_OK,
			RESULT_USER_CANCELED,
			RESULT_ABORTED
		}

		public enum EnumImeDialogResultButton
		{
			BUTTON_NONE,
			BUTTON_CLOSE,
			BUTTON_ENTER
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ImeDialogResult
		{
			public EnumImeDialogResult result;

			public EnumImeDialogResultButton button;

			private IntPtr _text;

			public string text => Marshal.PtrToStringAnsi(_text);
		}

		public static bool IsDialogOpen => PrxImeDialogIsDialogOpen();

		public static event Messages.EventHandler OnGotIMEDialogResult;

		[DllImport("CommonDialog")]
		private static extern int PrxImeDialogInitialise();

		[DllImport("CommonDialog")]
		private static extern void PrxImeDialogUpdate();

		[DllImport("CommonDialog")]
		private static extern bool PrxImeDialogIsDialogOpen();

		[DllImport("CommonDialog")]
		private static extern bool PrxImeDialogOpen(ImeDialogParams parameters);

		[DllImport("CommonDialog")]
		private static extern bool PrxImeDialogGetResult(out ImeDialogResult result);

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogHasMessage();

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogGetFirstMessage(out Messages.PluginMessage msg);

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogRemoveFirstMessage();

		public static bool Open(ImeDialogParams info)
		{
			return PrxImeDialogOpen(info);
		}

		public static ImeDialogResult GetResult()
		{
			ImeDialogResult result = default(ImeDialogResult);
			PrxImeDialogGetResult(out result);
			return result;
		}

		public static void ProcessMessage(Messages.PluginMessage msg)
		{
			Messages.MessageType type = msg.type;
			if (type == Messages.MessageType.kDialog_GotIMEDialogResult && Ime.OnGotIMEDialogResult != null)
			{
				Ime.OnGotIMEDialogResult(msg);
			}
		}

		public static void Initialise()
		{
			PrxImeDialogInitialise();
		}

		public static void Update()
		{
			PrxImeDialogUpdate();
		}
	}
}
