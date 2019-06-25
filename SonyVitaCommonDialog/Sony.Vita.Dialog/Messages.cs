using System;
using System.Runtime.InteropServices;

namespace Sony.Vita.Dialog
{
	public class Messages
	{
		public enum MessageType
		{
			kDialog_NotSet,
			kDialog_Log,
			kDialog_LogWarning,
			kDialog_LogError,
			kDialog_GotDialogResult,
			kDialog_GotIMEDialogResult
		}

		public delegate void EventHandler(PluginMessage msg);

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PluginMessage
		{
			public MessageType type;

			public int dataSize;

			public IntPtr data;

			public string Text
			{
				get
				{
					switch (type)
					{
					case MessageType.kDialog_Log:
					case MessageType.kDialog_LogWarning:
					case MessageType.kDialog_LogError:
						return Marshal.PtrToStringAnsi(data);
					default:
						return "no text";
					}
				}
			}

			public int Int => 0;
		}

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogHasMessage();

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogGetFirstMessage(out PluginMessage msg);

		[DllImport("CommonDialog")]
		private static extern bool PrxCommonDialogRemoveFirstMessage();

		public static bool HasMessage()
		{
			return PrxCommonDialogHasMessage();
		}

		public static void RemoveFirstMessage()
		{
			PrxCommonDialogRemoveFirstMessage();
		}

		public static void GetFirstMessage(out PluginMessage msg)
		{
			PrxCommonDialogGetFirstMessage(out msg);
		}
	}
}
