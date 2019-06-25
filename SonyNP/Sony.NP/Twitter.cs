using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Twitter
	{
		public struct PostTwitter
		{
			public string userText;

			public string imagePath;

			public bool forbidAttachPhoto;

			public bool disableEditTweetMsg;

			public bool forbidOnlyImageTweet;

			public bool forbidNoImageTweet;

			public bool disableChangeImage;

			public bool limitToScreenShot;
		}

		public static event Messages.EventHandler OnTwitterDialogStarted;

		public static event Messages.EventHandler OnTwitterDialogFinished;

		public static event Messages.EventHandler OnTwitterDialogCanceled;

		public static event Messages.EventHandler OnTwitterMessagePosted;

		public static event Messages.EventHandler OnTwitterMessagePostFailed;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTwitterIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTwitterIsDialogOpen();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxTwitterPostMessage(ref PostTwitter message);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxTwitterCancelDialog();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTwitterGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxTwitterGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		public static bool IsBusy()
		{
			return PrxTwitterIsBusy();
		}

		public static ErrorCode PostMessage(PostTwitter message)
		{
			return PrxTwitterPostMessage(ref message);
		}

		public static ErrorCode CancelDialog()
		{
			return PrxTwitterCancelDialog();
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_TwitterDialogStarted:
				if (Twitter.OnTwitterDialogStarted != null)
				{
					Twitter.OnTwitterDialogStarted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TwitterDialogFinished:
				if (Twitter.OnTwitterDialogFinished != null)
				{
					Twitter.OnTwitterDialogFinished(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TwitterDialogCanceled:
				if (Twitter.OnTwitterDialogCanceled != null)
				{
					Twitter.OnTwitterDialogCanceled(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TwitterMessagePosted:
				if (Twitter.OnTwitterMessagePosted != null)
				{
					Twitter.OnTwitterMessagePosted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TwitterMessagePostFailed:
				if (Twitter.OnTwitterMessagePostFailed != null)
				{
					Twitter.OnTwitterMessagePostFailed(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
