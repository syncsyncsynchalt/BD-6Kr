using System;
using System.Diagnostics;
using System.Threading;

namespace live2d
{
	public class UtSystem
	{
		public const long USER_TIME_AUTO = -1L;

		private static Stopwatch sw = null;

		private static long userTimeMSec = -1L;

		public static bool isBigEndian()
		{
			return !BitConverter.IsLittleEndian;
		}

		public static void sleepMSec(long msec)
		{
			TimeSpan timeout = new TimeSpan(0, 0, (int)(msec / 1000));
			Thread.Sleep(timeout);
		}

		public static long getUserTimeMSec()
		{
			if (userTimeMSec != -1)
			{
				return userTimeMSec;
			}
			return getSystemTimeMSec();
		}

		public static void setUserTimeMSec(long userTime)
		{
			userTimeMSec = userTime;
		}

		public static long updateUserTimeMSec()
		{
			return userTimeMSec = getSystemTimeMSec();
		}

		public static long getTimeMSec()
		{
			if (sw == null)
			{
				sw = new Stopwatch();
				sw.Start();
			}
			return sw.ElapsedMilliseconds;
		}

		public static long getSystemTimeMSec()
		{
			return getTimeMSec();
		}

		public static void resetUserTimeMSec()
		{
			userTimeMSec = -1L;
		}

		public static void exit(int code)
		{
			Environment.Exit(code);
		}
	}
}
