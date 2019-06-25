using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Messaging
	{
		private struct MessageAttachment
		{
			public int dataSize;

			private IntPtr _data;

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

		private struct InGameDataMessageInternal
		{
			private IntPtr _npID;

			private int npIDSize;

			public int messageID;

			public int dataSize;

			private IntPtr _data;

			public byte[] fromNpID
			{
				get
				{
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_npID, array, 0, npIDSize);
					return array;
				}
			}

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

		public struct InGameDataMessage
		{
			public int messageID;

			public byte[] fromNpID;

			public byte[] data;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MsgRequest
		{
			private IntPtr _body;

			private IntPtr _data;

			private int dataSize;

			public int expireMinutes;

			public int npIDCount;

			public string dataName;

			public string dataDescription;

			public string iconPath;

			public string body
			{
				set
				{
					_body = Marshal.StringToCoTaskMemAnsi(value);
				}
			}

			public byte[] data
			{
				set
				{
					dataSize = value.Length;
					_data = Marshal.AllocCoTaskMem(value.Length);
					Marshal.Copy(value, 0, _data, value.Length);
				}
			}

			~MsgRequest()
			{
				Marshal.FreeCoTaskMem(_body);
				Marshal.FreeCoTaskMem(_data);
			}
		}

		public static event Messages.EventHandler OnMessageSent;

		public static event Messages.EventHandler OnMessageNotSent;

		public static event Messages.EventHandler OnMessageCanceled;

		public static event Messages.EventHandler OnSessionInviteMessageRetrieved;

		public static event Messages.EventHandler OnCustomInviteMessageRetrieved;

		public static event Messages.EventHandler OnCustomDataMessageRetrieved;

		public static event Messages.EventHandler OnInGameDataMessageRetrieved;

		public static event Messages.EventHandler OnMessageNotSentFreqTooHigh;

		public static event Messages.EventHandler OnMessageSessionInviteReceived;

		public static event Messages.EventHandler OnMessageSessionInviteAccepted;

		public static event Messages.EventHandler OnMessageError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingIsBusy();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingSendInGameDataMessage(byte[] npID, byte[] data, int dataSize);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingSendMessage(MsgRequest msgRequest);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingSendGameInvite(MsgRequest msgRequest);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingShowMessageDialog();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingShowInviteDialog();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingGetMessageAttachment(out MessageAttachment attachment);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxMessagingGetGameInviteAttachment(out MessageAttachment attachment);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxHasInGameDataMessage();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxGetFirstInGameDataMessage(out InGameDataMessageInternal message);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxRemoveFirstInGameDataMessage();

		public static bool IsBusy()
		{
			return PrxMessagingIsBusy();
		}

		public static bool SendInGameDataMessage(byte[] npID, byte[] data)
		{
			return PrxMessagingSendInGameDataMessage(npID, data, data.Length);
		}

		public static bool SendMessage(MsgRequest request)
		{
			return PrxMessagingSendMessage(request);
		}

		public static bool SendGameInvite(MsgRequest request)
		{
			return PrxMessagingSendGameInvite(request);
		}

		public static bool ShowRecievedDataMessageDialog()
		{
			return PrxMessagingShowMessageDialog();
		}

		public static bool ShowRecievedInviteDialog()
		{
			return PrxMessagingShowInviteDialog();
		}

		public static byte[] GetGameInviteAttachment()
		{
			MessageAttachment attachment = default(MessageAttachment);
			PrxMessagingGetGameInviteAttachment(out attachment);
			return attachment.data;
		}

		public static byte[] GetMessageAttachment()
		{
			MessageAttachment attachment = default(MessageAttachment);
			PrxMessagingGetMessageAttachment(out attachment);
			return attachment.data;
		}

		public static bool InGameDataMessagesRecieved()
		{
			return PrxHasInGameDataMessage();
		}

		public static InGameDataMessage GetInGameDataMessage()
		{
			InGameDataMessageInternal message = default(InGameDataMessageInternal);
			PrxGetFirstInGameDataMessage(out message);
			InGameDataMessage result = default(InGameDataMessage);
			result.fromNpID = message.fromNpID;
			result.messageID = message.messageID;
			result.data = message.data;
			PrxRemoveFirstInGameDataMessage();
			return result;
		}

		public static int GetErrorFromMessage(Messages.PluginMessage msg)
		{
			if (msg.type != Messages.MessageType.kNPToolKit_MessagingError)
			{
				return -1;
			}
			return Marshal.ReadInt32(msg.data, 0);
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_MessagingSent:
				if (Messaging.OnMessageSent != null)
				{
					Messaging.OnMessageSent(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingNotSent:
				if (Messaging.OnMessageNotSent != null)
				{
					Messaging.OnMessageNotSent(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingNotSentFreqTooHigh:
				if (Messaging.OnMessageNotSentFreqTooHigh != null)
				{
					Messaging.OnMessageNotSentFreqTooHigh(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingCanceled:
				if (Messaging.OnMessageCanceled != null)
				{
					Messaging.OnMessageCanceled(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingSessionInviteRetrieved:
				if (Messaging.OnSessionInviteMessageRetrieved != null)
				{
					Messaging.OnSessionInviteMessageRetrieved(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingCustomInviteRetrieved:
				if (Messaging.OnCustomInviteMessageRetrieved != null)
				{
					Messaging.OnCustomInviteMessageRetrieved(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingDataMessageRetrieved:
				if (Messaging.OnCustomDataMessageRetrieved != null)
				{
					Messaging.OnCustomDataMessageRetrieved(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingInGameDataMessageRetrieved:
				if (Messaging.OnInGameDataMessageRetrieved != null)
				{
					Messaging.OnInGameDataMessageRetrieved(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingSessionInviteReceived:
				if (Messaging.OnMessageSessionInviteReceived != null)
				{
					Messaging.OnMessageSessionInviteReceived(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingSessionInviteAccepted:
				if (Messaging.OnMessageSessionInviteAccepted != null)
				{
					Messaging.OnMessageSessionInviteAccepted(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_MessagingError:
				if (Messaging.OnMessageError != null)
				{
					Messaging.OnMessageError(msg);
				}
				break;
			}
			return false;
		}
	}
}
