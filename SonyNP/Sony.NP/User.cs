using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Sony.NP
{
	public class User
	{
		public struct UserProfile
		{
			public int language;

			public int age;

			public int chatRestricted;

			public int contentRestricted;

			private IntPtr _onlineID;

			private IntPtr _npID;

			private int npIDSize;

			private IntPtr _avatarURL;

			private IntPtr _countryCode;

			private IntPtr _firstName;

			private IntPtr _middleName;

			private IntPtr _lastName;

			private IntPtr _profilePictureUrl;

			public ulong npAccountId;

			public string firstName => Marshal.PtrToStringAnsi(_firstName);

			public string middleName => Marshal.PtrToStringAnsi(_middleName);

			public string lastName => Marshal.PtrToStringAnsi(_lastName);

			public string profilePictureUrl => Marshal.PtrToStringAnsi(_profilePictureUrl);

			public string onlineID => Marshal.PtrToStringAnsi(_onlineID);

			public byte[] npID
			{
				get
				{
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_npID, array, 0, npIDSize);
					return array;
				}
			}

			public string avatarURL => Marshal.PtrToStringAnsi(_avatarURL);

			public string countryCode => Marshal.PtrToStringAnsi(_countryCode);
		}

		public struct NpID
		{
			private IntPtr _npID;

			private int npIDSize;

			public byte[] npID
			{
				get
				{
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_npID, array, 0, npIDSize);
					return array;
				}
			}
		}

		public struct RemoteUserProfile
		{
			public int language;

			private IntPtr _onlineID;

			private IntPtr _npID;

			private int npIDSize;

			private IntPtr _avatarURL;

			private IntPtr _countryCode;

			private IntPtr _firstName;

			private IntPtr _middleName;

			private IntPtr _lastName;

			private IntPtr _profilePictureUrl;

			public string onlineID => Marshal.PtrToStringAnsi(_onlineID);

			public string firstName => Marshal.PtrToStringAnsi(_firstName);

			public string middleName => Marshal.PtrToStringAnsi(_middleName);

			public string lastName => Marshal.PtrToStringAnsi(_lastName);

			public string profilePictureUrl => Marshal.PtrToStringAnsi(_profilePictureUrl);

			public byte[] npID
			{
				get
				{
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_npID, array, 0, npIDSize);
					return array;
				}
			}

			public string avatarURL => Marshal.PtrToStringAnsi(_avatarURL);

			public string countryCode => Marshal.PtrToStringAnsi(_countryCode);
		}

		public static bool signedIn = false;

		public static bool IsSignedInPSN
		{
			get
			{
				if (Application.platform == RuntimePlatform.PS4)
				{
					byte[] npID;
					int userSigninStatus = GetUserSigninStatus(GetCurrentUserId(), out npID);
					return userSigninStatus == 2;
				}
				return IsSignedIn;
			}
		}

		public static bool IsSignedIn => signedIn;

		public static bool IsSigninBusy => PrxSigninIsBusy();

		public static bool IsUserProfileBusy => PrxUserProfileIsBusy();

		public static event Messages.EventHandler OnSignedIn;

		public static event Messages.EventHandler OnSignedOut;

		public static event Messages.EventHandler OnPresenceSet;

		public static event Messages.EventHandler OnGotUserProfile;

		public static event Messages.EventHandler OnGotRemoteUserNpID;

		public static event Messages.EventHandler OnGotRemoteUserProfile;

		public static event Messages.EventHandler OnUserProfileError;

		public static event Messages.EventHandler OnPresenceError;

		public static event Messages.EventHandler OnSignInError;

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxSignin();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxSigninIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxIsSignedIn();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxSigninGetLastError(out ResultCode result);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxSetCurrentUserId(int UserId);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetCurrentUserId();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxLogOutUser(int UserId);

		public static int SetCurrentUserId(int UserId)
		{
			return PrxSetCurrentUserId(UserId);
		}

		public static int GetCurrentUserId()
		{
			return PrxGetCurrentUserId();
		}

		public static int LogOutUser(int UserId)
		{
			return PrxLogOutUser(UserId);
		}

		public static bool GetLastSignInError(out ResultCode result)
		{
			PrxSigninGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxUserProfileGetLastError(out ResultCode result);

		public static bool GetLastUserProfileError(out ResultCode result)
		{
			PrxUserProfileGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxPresenceGetLastError(out ResultCode result);

		public static bool GetLastPresenceError(out ResultCode result)
		{
			PrxPresenceGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxSetPresence(string text);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxSetPresenceIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRequestUserProfile();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxUserProfileIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxGetUserProfile(out UserProfile profile);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxGetUserSigninStatus(int userID, ref byte[] npID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRequestRemoteUserNpID(string onlineID);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxGetRemoteUserNpID(out NpID npID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxRequestRemoteUserProfileForOnlineID(string onlineID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxRequestRemoteUserProfileForNpID(byte[] npID);

		[DllImport("UnityNpToolkit")]
		private static extern bool PrxGetRemoteUserProfile(out RemoteUserProfile profile);

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_SignedIn:
				signedIn = true;
				System.connectionUp = true;
				if (User.OnSignedIn != null)
				{
					User.OnSignedIn(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_SignedOut:
				signedIn = false;
				if (User.OnSignedOut != null)
				{
					User.OnSignedOut(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_PresenceSet:
				if (User.OnPresenceSet != null)
				{
					User.OnPresenceSet(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_GotUserProfile:
				if (User.OnGotUserProfile != null)
				{
					User.OnGotUserProfile(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_GotRemoteUserProfile:
				if (User.OnGotRemoteUserProfile != null)
				{
					User.OnGotRemoteUserProfile(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_GotRemoteUserNpID:
				if (User.OnGotRemoteUserNpID != null)
				{
					User.OnGotRemoteUserNpID(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_UserProfileError:
				if (User.OnUserProfileError != null)
				{
					User.OnUserProfileError(msg);
				}
				if (Application.platform == RuntimePlatform.PS4)
				{
					ResultCode result = default(ResultCode);
					GetLastUserProfileError(out result);
					uint lastErrorSCE = (uint)result.lastErrorSCE;
					if (lastErrorSCE == 2183135618u || lastErrorSCE == 2183135876u)
					{
						Debug.LogError($"bad missing NP title ID/ NP Title secret ... check your publishing settings ({result.lastErrorSCE:X})");
					}
				}
				return true;
			case Messages.MessageType.kNPToolKit_PresenceError:
				if (User.OnPresenceError != null)
				{
					User.OnPresenceError(msg);
				}
				break;
			case Messages.MessageType.kNPToolKit_SignInError:
				if (User.OnSignInError != null)
				{
					User.OnSignInError(msg);
				}
				break;
			}
			return false;
		}

		public static ErrorCode SignIn()
		{
			return PrxSignin();
		}

		public static ErrorCode SetOnlinePresence(string text)
		{
			return PrxSetPresence(text);
		}

		public static bool OnlinePresenceIsBusy()
		{
			return PrxSetPresenceIsBusy();
		}

		public static ErrorCode RequestUserProfile()
		{
			return PrxRequestUserProfile();
		}

		public static UserProfile GetCachedUserProfile()
		{
			UserProfile profile = default(UserProfile);
			PrxGetUserProfile(out profile);
			return profile;
		}

		public static ErrorCode RequestRemoteUserNpID(string onlineID)
		{
			return PrxRequestRemoteUserNpID(onlineID);
		}

		public static int GetUserSigninStatus(int userID, out byte[] npID)
		{
			npID = new byte[36];
			npID[0] = 170;
			npID[1] = 187;
			return PrxGetUserSigninStatus(userID, ref npID);
		}

		public static byte[] GetCachedRemoteUserNpID()
		{
			NpID npID = default(NpID);
			PrxGetRemoteUserNpID(out npID);
			return npID.npID;
		}

		public static ErrorCode RequestRemoteUserProfileForOnlineID(string onlineID)
		{
			return PrxRequestRemoteUserProfileForOnlineID(onlineID);
		}

		public static ErrorCode RequestRemoteUserProfileForNpID(byte[] npID)
		{
			return PrxRequestRemoteUserProfileForNpID(npID);
		}

		public static RemoteUserProfile GetCachedRemoteUserProfile()
		{
			RemoteUserProfile profile = default(RemoteUserProfile);
			PrxGetRemoteUserProfile(out profile);
			return profile;
		}

		public static bool UserProfileIsBusy()
		{
			return PrxUserProfileIsBusy();
		}
	}
}
