using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Main
	{
		public static int kNpToolkitCreate_CacheTrophyIcons = 1;

		public static int kNpToolkitCreate_NoRanking = 2;

		public static bool enableInternalLogging
		{
			get
			{
				return PrxGetLoggingEnabled();
			}
			set
			{
				PrxSetLoggingEnabled(value);
			}
		}

		public static event Messages.EventHandler OnLog;

		public static event Messages.EventHandler OnLogWarning;

		public static event Messages.EventHandler OnLogError;

		public static event Messages.EventHandler OnNPInitialized;

		[DllImport("UnityNpToolkit")]
		private static extern int PrxInitialize(int creationFlags);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxInitializeWithNpAgeRating(int creationFlags, int npAgeRating);

		[DllImport("UnityNpToolkit")]
		private static extern bool PrxGetLoggingEnabled();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxSetLoggingEnabled(bool enabled);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern void PrxSetSessionImage(string imageFilePath);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxShutDown();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxUpdate();

		public static void Initialize(int creationFlags)
		{
			PrxInitialize(creationFlags);
		}

		public static void SetSessionImage(string imageFilePath)
		{
			PrxSetSessionImage(imageFilePath);
		}

		public static void InitializeWithNpAgeRating(int creationFlags, int npAgeRating)
		{
			PrxInitializeWithNpAgeRating(creationFlags, npAgeRating);
		}

		public static void Update()
		{
			PrxUpdate();
			PumpMessages();
		}

		public static void ShutDown()
		{
			PrxShutDown();
		}

		private static void PumpMessages()
		{
			while (Messages.HasMessage())
			{
				Messages.PluginMessage msg = default(Messages.PluginMessage);
				Messages.GetFirstMessage(out msg);
				System.ProcessMessage(msg);
				User.ProcessMessage(msg);
				Friends.ProcessMessage(msg);
				Trophies.ProcessMessage(msg);
				Ranking.ProcessMessage(msg);
				Matching.ProcessMessage(msg);
				Messaging.ProcessMessage(msg);
				WordFilter.ProcessMessage(msg);
				Commerce.ProcessMessage(msg);
				Ticketing.ProcessMessage(msg);
				TusTss.ProcessMessage(msg);
				Dialogs.ProcessMessage(msg);
				Facebook.ProcessMessage(msg);
				Twitter.ProcessMessage(msg);
				Requests.ProcessMessage(msg);
				switch (msg.type)
				{
				case Messages.MessageType.kNPToolKit_Log:
					if (Main.OnLog != null)
					{
						Main.OnLog(msg);
					}
					break;
				case Messages.MessageType.kNPToolKit_LogWarning:
					if (Main.OnLogWarning != null)
					{
						Main.OnLogWarning(msg);
					}
					break;
				case Messages.MessageType.kNPToolKit_LogError:
					if (Main.OnLogError != null)
					{
						Main.OnLogError(msg);
					}
					break;
				case Messages.MessageType.kNPToolKit_NPInitialized:
					if (Main.OnNPInitialized != null)
					{
						Main.OnNPInitialized(msg);
					}
					break;
				}
				Messages.RemoveFirstMessage();
			}
		}
	}
}
