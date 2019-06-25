using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Facebook
	{
		public struct PostFacebook
		{
			public long appID;

			public string userText;

			public string photoURL;

			public string photoTitle;

			public string photoCaption;

			public string photoDescription;

			public string actionLinkName;

			public string actionLinkURL;
		}

		public static event Messages.EventHandler OnFacebookDialogStarted;

		public static event Messages.EventHandler OnFacebookDialogFinished;

		public static event Messages.EventHandler OnFacebookMessagePosted;

		public static event Messages.EventHandler OnFacebookMessagePostFailed;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxFacebookIsBusy();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxFacebookPostMessage(ref PostFacebook message);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxFacebookGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxFacebookGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		public static bool IsBusy()
		{
			return PrxFacebookIsBusy();
		}

		public static ErrorCode PostMessage(PostFacebook message)
		{
			return PrxFacebookPostMessage(ref message);
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_FacebookDialogStarted:
				if (Facebook.OnFacebookDialogStarted != null)
				{
					Facebook.OnFacebookDialogStarted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_FacebookDialogFinished:
				if (Facebook.OnFacebookDialogFinished != null)
				{
					Facebook.OnFacebookDialogFinished(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_FacebookMessagePosted:
				if (Facebook.OnFacebookMessagePosted != null)
				{
					Facebook.OnFacebookMessagePosted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_FacebookMessagePostFailed:
				if (Facebook.OnFacebookMessagePostFailed != null)
				{
					Facebook.OnFacebookMessagePostFailed(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
