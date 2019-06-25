using System;
using System.Runtime.InteropServices;

namespace Sony.Vita.SavedGame
{
	public class SaveLoad
	{
		[Flags]
		public enum ControlFlags
		{
			NOSPACE_DIALOG_CONTINUABLE = 0x0,
			NOSPACE_DIALOG_NOT_CONTINUABLE = 0x1
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct SavedGameSlotParams
		{
			public string title;

			public string subTitle;

			public string detail;

			public string iconPath;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct SavedGameData
		{
			public int dataSize;

			private IntPtr _data;

			public byte[] data
			{
				get
				{
					if (dataSize > 0)
					{
						byte[] array = new byte[dataSize];
						Marshal.Copy(_data, array, 0, dataSize);
						return array;
					}
					return null;
				}
			}
		}

		public enum SlotStatus
		{
			AVAILABLE,
			BROKEN,
			EMPTY
		}

		private struct SavedGameSlotInfoInternal
		{
			public long _modifiedTime;

			public SlotStatus status;

			public int sizeKiB;

			private IntPtr _title;

			private IntPtr _subTitle;

			private IntPtr _detail;

			private IntPtr _iconPath;

			public DateTime modifiedTime => new DateTime(_modifiedTime);

			public string title => Marshal.PtrToStringAnsi(_title);

			public string subTitle => Marshal.PtrToStringAnsi(_subTitle);

			public string detail => Marshal.PtrToStringAnsi(_detail);

			public string iconPath => Marshal.PtrToStringAnsi(_iconPath);
		}

		public struct SavedGameSlotInfo
		{
			public DateTime modifiedTime;

			public SlotStatus status;

			public int slotNumber;

			public string title;

			public string subTitle;

			public string detail;

			public string iconPath;
		}

		public const int kSaveDataDefaultSlot = 0;

		public static bool IsDialogOpen => PrxIsSavedGamesDialogOpen();

		public static bool IsBusy => PrxIsSavedGamesBusy();

		public static event Messages.EventHandler OnGameSaved;

		public static event Messages.EventHandler OnGameLoaded;

		public static event Messages.EventHandler OnCanceled;

		public static event Messages.EventHandler OnSaveError;

		public static event Messages.EventHandler OnLoadError;

		public static event Messages.EventHandler OnLoadNoData;

		[DllImport("SavedGames")]
		private static extern bool PrxSavedGamesGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxSavedGamesGetLastError(out result);
			return result.lastError == ErrorCode.SG_OK;
		}

		[DllImport("SavedGames")]
		private static extern int PrxSavedGamesInitialise();

		[DllImport("SavedGames")]
		private static extern void PrxSavedGamesUpdate();

		[DllImport("SavedGames")]
		private static extern bool PrxIsSavedGamesDialogOpen();

		[DllImport("SavedGames")]
		private static extern bool PrxIsSavedGamesBusy();

		[DllImport("SavedGames", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxSetEmptySlotIconPath(string iconPath);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameSetSlotCount(int slotCount);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameSave(byte[] data, int dataSize, int slotNumber, ref SavedGameSlotParams slotParams, ControlFlags controlFlags);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameLoad(int slotNumber);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameAutoSave(byte[] data, int dataSize, int slotNumber, ref SavedGameSlotParams slotParams, ControlFlags controlFlags);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameAutoLoad(int slotNumber);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameGetGameData(out SavedGameData data);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameListSave(byte[] data, int dataSize, ref SavedGameSlotParams slotParams, ControlFlags controlFlags);

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameListLoad();

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameListDelete();

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameDeleteSlot(int slotNumber);

		[DllImport("SavedGames")]
		private static extern int PrxSavedGameGetQuota();

		[DllImport("SavedGames")]
		private static extern int PrxSavedGameGetUsedSize();

		[DllImport("SavedGames")]
		private static extern ErrorCode PrxSavedGameGetSlotInfo(int slotNumber, out SavedGameSlotInfoInternal slotInfo);

		public static int GetQuota()
		{
			return PrxSavedGameGetQuota();
		}

		public static int GetUsedSize()
		{
			return PrxSavedGameGetUsedSize();
		}

		public static ErrorCode DeleteSlot(int slotNumber)
		{
			return PrxSavedGameDeleteSlot(slotNumber);
		}

		public static ErrorCode GetSlotInfo(int slotNumber, out SavedGameSlotInfo slotInfo)
		{
			SavedGameSlotInfoInternal slotInfo2 = default(SavedGameSlotInfoInternal);
			ErrorCode errorCode = PrxSavedGameGetSlotInfo(slotNumber, out slotInfo2);
			if (errorCode == ErrorCode.SG_OK)
			{
				slotInfo.status = slotInfo2.status;
				slotInfo.modifiedTime = slotInfo2.modifiedTime;
				slotInfo.title = slotInfo2.title;
				slotInfo.subTitle = slotInfo2.subTitle;
				slotInfo.detail = slotInfo2.detail;
				slotInfo.iconPath = slotInfo2.iconPath;
			}
			else
			{
				slotInfo.status = SlotStatus.EMPTY;
				slotInfo.modifiedTime = DateTime.MinValue;
				slotInfo.title = "";
				slotInfo.subTitle = "";
				slotInfo.detail = "";
				slotInfo.iconPath = "";
				if (errorCode == ErrorCode.SG_ERR_SLOT_NOT_FOUND)
				{
					errorCode = ErrorCode.SG_OK;
				}
			}
			slotInfo.slotNumber = slotNumber;
			return errorCode;
		}

		public static ErrorCode SetEmptySlotIconPath(string iconPath)
		{
			return PrxSetEmptySlotIconPath(iconPath);
		}

		public static ErrorCode SetSlotCount(int slotCount)
		{
			return PrxSavedGameSetSlotCount(slotCount);
		}

		public static ErrorCode SaveGame(byte[] data, SavedGameSlotParams slotParams, ControlFlags controlFlags)
		{
			return PrxSavedGameSave(data, data.Length, 0, ref slotParams, controlFlags);
		}

		public static ErrorCode SaveGame(byte[] data, int slotNumber, SavedGameSlotParams slotParams, ControlFlags controlFlags)
		{
			return PrxSavedGameSave(data, data.Length, slotNumber, ref slotParams, controlFlags);
		}

		public static ErrorCode LoadGame()
		{
			return PrxSavedGameLoad(0);
		}

		public static ErrorCode LoadGame(int slotNumber)
		{
			return PrxSavedGameLoad(slotNumber);
		}

		public static ErrorCode AutoSaveGame(byte[] data, int slotNumber, SavedGameSlotParams slotParams, ControlFlags controlFlags)
		{
			return PrxSavedGameAutoSave(data, data.Length, slotNumber, ref slotParams, controlFlags);
		}

		public static ErrorCode AutoLoadGame(int slotNumber)
		{
			return PrxSavedGameAutoLoad(slotNumber);
		}

		public static byte[] GetLoadedGame()
		{
			SavedGameData data = default(SavedGameData);
			if (PrxSavedGameGetGameData(out data) == ErrorCode.SG_OK)
			{
				return data.data;
			}
			return null;
		}

		public static ErrorCode SaveGameList(byte[] data, SavedGameSlotParams slotParams, ControlFlags controlFlags)
		{
			return PrxSavedGameListSave(data, data.Length, ref slotParams, controlFlags);
		}

		public static ErrorCode LoadGameList()
		{
			return PrxSavedGameListLoad();
		}

		public static ErrorCode DeleteGameList()
		{
			return PrxSavedGameListDelete();
		}

		public static void ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kSavedGame_GameSaved:
				if (SaveLoad.OnGameSaved != null)
				{
					SaveLoad.OnGameSaved(msg);
				}
				break;
			case Messages.MessageType.kSavedGame_GameLoaded:
				if (SaveLoad.OnGameLoaded != null)
				{
					SaveLoad.OnGameLoaded(msg);
				}
				break;
			case Messages.MessageType.kSavedGame_Canceled:
				if (SaveLoad.OnCanceled != null)
				{
					SaveLoad.OnCanceled(msg);
				}
				break;
			case Messages.MessageType.kSavedGame_SaveNoSpace:
			case Messages.MessageType.kSavedGame_SaveNotMounted:
			case Messages.MessageType.kSavedGame_SaveGenericError:
				if (SaveLoad.OnSaveError != null)
				{
					SaveLoad.OnSaveError(msg);
				}
				break;
			case Messages.MessageType.kSavedGame_LoadCorrupted:
				if (SaveLoad.OnLoadError != null)
				{
					SaveLoad.OnLoadError(msg);
				}
				break;
			case Messages.MessageType.kSavedGame_LoadNoData:
				if (SaveLoad.OnLoadNoData != null)
				{
					SaveLoad.OnLoadNoData(msg);
				}
				break;
			}
		}

		public static void Initialise()
		{
			PrxSavedGamesInitialise();
		}

		public static void Update()
		{
			PrxSavedGamesUpdate();
		}
	}
}
