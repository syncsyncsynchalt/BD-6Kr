using System;
using System.Runtime.InteropServices;

namespace Sony.Vita.SavedGame
{
	public class Messages
	{
		public enum MessageType
		{
			kSavedGame_NotSet,
			kSavedGame_Log,
			kSavedGame_LogWarning,
			kSavedGame_LogError,
			kSavedGame_GameSaved,
			kSavedGame_GameLoaded,
			kSavedGame_Canceled,
			kSavedGame_SaveNoSpace,
			kSavedGame_SaveNotMounted,
			kSavedGame_SaveGenericError,
			kSavedGame_LoadCorrupted,
			kSavedGame_LoadNoData
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
					case MessageType.kSavedGame_Log:
					case MessageType.kSavedGame_LogWarning:
					case MessageType.kSavedGame_LogError:
						return Marshal.PtrToStringAnsi(data);
					default:
						return "no text";
					}
				}
			}
		}

		[DllImport("SavedGames")]
		private static extern bool PrxSavedGamesHasMessage();

		[DllImport("SavedGames")]
		private static extern bool PrxSavedGamesGetFirstMessage(out PluginMessage msg);

		[DllImport("SavedGames")]
		private static extern bool PrxSavedGamesRemoveFirstMessage();

		public static bool HasMessage()
		{
			return PrxSavedGamesHasMessage();
		}

		public static void RemoveFirstMessage()
		{
			PrxSavedGamesRemoveFirstMessage();
		}

		public static void GetFirstMessage(out PluginMessage msg)
		{
			PrxSavedGamesGetFirstMessage(out msg);
		}
	}
}
