using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Sony.Vita.SavedGame
{
	public class Main
	{
		public static bool enableInternalLogging
		{
			get
			{
				return PrxSavedGamesGetLoggingEnabled();
			}
			set
			{
				PrxSavedGamesSetLoggingEnabled(value);
			}
		}

		public static event Messages.EventHandler OnLog;

		public static event Messages.EventHandler OnLogWarning;

		public static event Messages.EventHandler OnLogError;

		[DllImport("SavedGames")]
		private static extern int PrxSavedGamesInitialise();

		[DllImport("SavedGames")]
		private static extern int PrxSavedGamesUpdate();

		[DllImport("SavedGames")]
		private static extern bool PrxSavedGamesGetLoggingEnabled();

		[DllImport("SavedGames")]
		private static extern void PrxSavedGamesSetLoggingEnabled(bool enabled);

		public static void Initialise()
		{
			PrxSavedGamesInitialise();
		}

		public static void Update()
		{
			PrxSavedGamesUpdate();
			PumpMessages();
		}

		private static void PumpMessages()
		{
			while (Messages.HasMessage())
			{
				Messages.PluginMessage msg = default(Messages.PluginMessage);
				Messages.GetFirstMessage(out msg);
				try
				{
					SaveLoad.ProcessMessage(msg);
					switch (msg.type)
					{
					case Messages.MessageType.kSavedGame_Log:
						if (Main.OnLog != null)
						{
							Main.OnLog(msg);
						}
						break;
					case Messages.MessageType.kSavedGame_LogWarning:
						if (Main.OnLogWarning != null)
						{
							Main.OnLogWarning(msg);
						}
						break;
					case Messages.MessageType.kSavedGame_LogError:
						if (Main.OnLogError != null)
						{
							Main.OnLogError(msg);
						}
						break;
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				Messages.RemoveFirstMessage();
			}
		}
	}
}
