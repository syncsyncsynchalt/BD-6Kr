using System;

namespace UnityEngine
{
	public enum RuntimePlatform
	{
		OSXEditor = 0,
		OSXPlayer = 1,
		WindowsPlayer = 2,
		OSXWebPlayer = 3,
		OSXDashboardPlayer = 4,
		WindowsWebPlayer = 5,
		WindowsEditor = 7,
		IPhonePlayer = 8,
		XBOX360 = 10,
		PS3 = 9,
		Android = 11,
		[Obsolete("NaCl export is no longer supported in Unity 5.0+.")]
		NaCl = 12,
		[Obsolete("FlashPlayer export is no longer supported in Unity 5.0+.")]
		FlashPlayer = 0xF,
		LinuxPlayer = 13,
		WebGLPlayer = 17,
		[Obsolete("Use WSAPlayerX86 instead")]
		MetroPlayerX86 = 18,
		WSAPlayerX86 = 18,
		[Obsolete("Use WSAPlayerX64 instead")]
		MetroPlayerX64 = 19,
		WSAPlayerX64 = 19,
		[Obsolete("Use WSAPlayerARM instead")]
		MetroPlayerARM = 20,
		WSAPlayerARM = 20,
		WP8Player = 21,
		BlackBerryPlayer = 22,
		TizenPlayer = 23,
		PSP2 = 24,
		PS4 = 25,
		PSM = 26,
		XboxOne = 27,
		SamsungTVPlayer = 28,
		WiiU = 30
	}
}
