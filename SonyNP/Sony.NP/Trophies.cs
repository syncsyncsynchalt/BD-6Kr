using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Sony.NP
{
	public class Trophies
	{
		public struct GameInfo
		{
			public int numGroups;

			public int numTrophies;

			public int numPlatinum;

			public int numGold;

			public int numSilver;

			public int numBronze;

			private IntPtr _title;

			private IntPtr _description;

			public string title => Marshal.PtrToStringAnsi(_title);

			public string description => Marshal.PtrToStringAnsi(_description);
		}

		public struct GroupDetails
		{
			public int groupId;

			public int numTrophies;

			public int numPlatinum;

			public int numGold;

			public int numSilver;

			public int numBronze;

			public int iconWidth;

			public int iconHeight;

			public int iconDataSize;

			private IntPtr _iconData;

			private IntPtr _title;

			private IntPtr _description;

			public string title => Marshal.PtrToStringAnsi(_title);

			public string description => Marshal.PtrToStringAnsi(_description);

			public Texture2D icon
			{
				get
				{
					if (iconDataSize > 0)
					{
						byte[] array = new byte[iconDataSize];
						Marshal.Copy(_iconData, array, 0, iconDataSize);
						Texture2D texture2D = new Texture2D(iconWidth, iconHeight);
						texture2D.LoadImage(array);
						return texture2D;
					}
					return null;
				}
			}

			public bool hasIcon => iconDataSize > 0;
		}

		public struct GroupData
		{
			public int groupId;

			public int unlockedTrophies;

			public int unlockedPlatinum;

			public int unlockedGold;

			public int unlockedSilver;

			public int unlockedBronze;

			public int progressPercentage;

			public int userId;
		}

		public struct TrophyDetails
		{
			public int trophyId;

			public int trophyGrade;

			public int groupId;

			public bool hidden;

			private IntPtr _name;

			private IntPtr _description;

			public string name => Marshal.PtrToStringAnsi(_name);

			public string description => Marshal.PtrToStringAnsi(_description);
		}

		public struct TrophyData
		{
			public int trophyId;

			public bool unlocked;

			public long timestamp;

			public int iconWidth;

			public int iconHeight;

			public int iconDataSize;

			private IntPtr _iconData;

			public int userId;

			public Texture2D icon
			{
				get
				{
					if (iconDataSize > 0)
					{
						byte[] array = new byte[iconDataSize];
						Marshal.Copy(_iconData, array, 0, iconDataSize);
						Texture2D texture2D = new Texture2D(iconWidth, iconHeight);
						texture2D.LoadImage(array);
						return texture2D;
					}
					return null;
				}
			}

			public bool hasIcon => iconDataSize > 0;
		}

		public struct TrophyProgress
		{
			public int unlockedTrophies;

			public int unlockedPlatinum;

			public int unlockedGold;

			public int unlockedSilver;

			public int unlockedBronze;

			public int progressPercentage;

			public int userId;
		}

		private static bool trophiesAreAvailable = false;

		public static bool TrophiesAreAvailable => trophiesAreAvailable;

		public static event Messages.EventHandler OnPackageRegistered;

		public static event Messages.EventHandler OnRegisterPackageFailed;

		public static event Messages.EventHandler OnGotGameInfo;

		public static event Messages.EventHandler OnGotGroupInfo;

		public static event Messages.EventHandler OnGotTrophyInfo;

		public static event Messages.EventHandler OnGotProgress;

		public static event Messages.EventHandler OnUnlockedPlatinum;

		public static event Messages.EventHandler OnAwardedTrophy;

		public static event Messages.EventHandler OnAwardTrophyFailed;

		public static event Messages.EventHandler OnAlreadyAwardedTrophy;

		public static event Messages.EventHandler OnTrophyError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTrophyGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxTrophyGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTrophyRegisterPackIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTrophyRefreshGroupInfoIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTrophyRefreshTrophyInfoIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTrophyRefreshProgressIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyGetGameInfo(out GameInfo info);

		[DllImport("UnityNpToolkit")]
		private static extern void PrxTrophyLockList();

		[DllImport("UnityNpToolkit")]
		private static extern void PrxTrophyUnlockList();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyRefreshGroupInfo();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxTrophyGetGroupDetailsCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyGetGroupDetails(int index, out GroupDetails details);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxTrophyGetGroupDataCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyGetGroupData(int index, out GroupData data);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyRefreshTrophyInfo();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxTrophyGetTrophyDetailsCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyGetTrophyDetails(int index, out TrophyDetails details);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxTrophyGetTrophyDataCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyGetTrophyData(int index, out TrophyData data);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyRefreshProgress();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyGetProgress(out TrophyProgress info);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTrophyAward(int index);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRegisterTrophyPack();

		public static bool RegisterTrophyPackIsBusy()
		{
			return PrxTrophyRegisterPackIsBusy();
		}

		public static GameInfo GetCachedGameInfo()
		{
			GameInfo info = default(GameInfo);
			PrxTrophyGetGameInfo(out info);
			return info;
		}

		public static bool RequestGroupInfoIsBusy()
		{
			return PrxTrophyRefreshGroupInfoIsBusy();
		}

		public static ErrorCode RequestGroupInfo()
		{
			return PrxTrophyRefreshGroupInfo();
		}

		public static GroupData[] GetCachedGroupData()
		{
			PrxTrophyLockList();
			int num = PrxTrophyGetGroupDataCount();
			GroupData[] array = new GroupData[num];
			for (int i = 0; i < num; i++)
			{
				PrxTrophyGetGroupData(i, out array[i]);
			}
			PrxTrophyUnlockList();
			return array;
		}

		public static GroupDetails[] GetCachedGroupDetails()
		{
			PrxTrophyLockList();
			int num = PrxTrophyGetGroupDetailsCount();
			GroupDetails[] array = new GroupDetails[num];
			for (int i = 0; i < num; i++)
			{
				PrxTrophyGetGroupDetails(i, out array[i]);
			}
			PrxTrophyUnlockList();
			return array;
		}

		public static bool RequestTrophyInfoIsBusy()
		{
			return PrxTrophyRefreshTrophyInfoIsBusy();
		}

		public static ErrorCode RequestTrophyInfo()
		{
			return PrxTrophyRefreshTrophyInfo();
		}

		public static TrophyData[] GetCachedTrophyData()
		{
			PrxTrophyLockList();
			int num = PrxTrophyGetTrophyDataCount();
			TrophyData[] array = new TrophyData[num];
			for (int i = 0; i < num; i++)
			{
				PrxTrophyGetTrophyData(i, out array[i]);
			}
			PrxTrophyUnlockList();
			return array;
		}

		public static TrophyDetails[] GetCachedTrophyDetails()
		{
			PrxTrophyLockList();
			int num = PrxTrophyGetTrophyDetailsCount();
			TrophyDetails[] array = new TrophyDetails[num];
			for (int i = 0; i < num; i++)
			{
				PrxTrophyGetTrophyDetails(i, out array[i]);
			}
			PrxTrophyUnlockList();
			return array;
		}

		public static bool RequestTrophyProgressIsBusy()
		{
			return PrxTrophyRefreshProgressIsBusy();
		}

		public static ErrorCode RequestTrophyProgress()
		{
			return PrxTrophyRefreshProgress();
		}

		public static TrophyProgress GetCachedTrophyProgress()
		{
			TrophyProgress info = default(TrophyProgress);
			PrxTrophyGetProgress(out info);
			return info;
		}

		public static ErrorCode AwardTrophy(int index)
		{
			return PrxTrophyAward(index);
		}

		public static ErrorCode RegisterTrophyPack()
		{
			return PrxRegisterTrophyPack();
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_TrophySetSetupSuccess:
				trophiesAreAvailable = true;
				if (Trophies.OnPackageRegistered != null)
				{
					Trophies.OnPackageRegistered(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophySetSetupCancelled:
			case Messages.MessageType.kNPToolKit_TrophySetSetupAborted:
			case Messages.MessageType.kNPToolKit_TrophySetSetupFail:
				if (Trophies.OnRegisterPackageFailed != null)
				{
					Trophies.OnRegisterPackageFailed(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_TrophyGotGameInfo:
				if (Trophies.OnGotGameInfo != null)
				{
					Trophies.OnGotGameInfo(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyGotGroupInfo:
				if (Trophies.OnGotGroupInfo != null)
				{
					Trophies.OnGotGroupInfo(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyGotTrophyInfo:
				if (Trophies.OnGotTrophyInfo != null)
				{
					Trophies.OnGotTrophyInfo(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyGotProgress:
				if (Trophies.OnGotProgress != null)
				{
					Trophies.OnGotProgress(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyUnlocked:
				if (Trophies.OnAwardedTrophy != null)
				{
					Trophies.OnAwardedTrophy(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyUnlockFailed:
				if (Trophies.OnAwardTrophyFailed != null)
				{
					Trophies.OnAwardTrophyFailed(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyUnlockedAlready:
				if (Trophies.OnAlreadyAwardedTrophy != null)
				{
					Trophies.OnAlreadyAwardedTrophy(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyUnlockedPlatinum:
				if (Trophies.OnUnlockedPlatinum != null)
				{
					Trophies.OnUnlockedPlatinum(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TrophyError:
				if (Trophies.OnTrophyError != null)
				{
					Trophies.OnTrophyError(msg);
				}
				return true;
			}
			return false;
		}
	}
}
