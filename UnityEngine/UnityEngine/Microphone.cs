using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Microphone
	{
		public static string[] devices
		{
			get;
		}

		public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency) { throw new NotImplementedException("なにこれ"); }

		public static void End(string deviceName) { throw new NotImplementedException("なにこれ"); }

		public static bool IsRecording(string deviceName) { throw new NotImplementedException("なにこれ"); }

		public static int GetPosition(string deviceName) { throw new NotImplementedException("なにこれ"); }

		public static void GetDeviceCaps(string deviceName, out int minFreq, out int maxFreq) { throw new NotImplementedException("なにこれ"); }
	}
}
