using System.Collections.Generic;
using UnityEngine;

namespace live2d
{
	public class UtDebug
	{
		internal class DebugTimer
		{
			internal string key;

			internal double startTimeMs;
		}

		private static Dictionary<string, DebugTimer> timerMap = new Dictionary<string, DebugTimer>();

		public static void start(string key)
		{
			if (!timerMap.ContainsKey(key))
			{
				timerMap.Add(key, new DebugTimer());
			}
			DebugTimer debugTimer = timerMap[key];
			if (debugTimer == null)
			{
				debugTimer = new DebugTimer();
				debugTimer.key = key;
				timerMap.Add(key, debugTimer);
			}
			debugTimer.startTimeMs = UtSystem.getSystemTimeMSec();
		}

		public static void dump(string key)
		{
			DebugTimer debugTimer = timerMap[key];
			if (debugTimer != null)
			{
				double num = UtSystem.getSystemTimeMSec();
				double num2 = num - debugTimer.startTimeMs;
				print("{0} :: {1}ms\n", key, num2);
			}
		}

		public static void error(string format, params object[] args)
		{
			print(format + "\n", args);
		}

		public static void print(string format, params object[] args)
		{
			MonoBehaviour.print(string.Format(format, args));
		}

		public static void println(string format, params object[] args)
		{
			MonoBehaviour.print(string.Format(format, args));
		}

		public static void dumpByte(byte[] data, int length)
		{
			for (int i = 0; i < length; i++)
			{
				if (i % 16 == 0 && i > 0)
				{
					print("\n");
				}
				else if (i % 8 == 0 && i > 0)
				{
					print("  ");
				}
				print("%02X ", data[i] & 0xFF);
			}
			print("\n");
		}

		public static void printVectorUShort(string msg, short[] v, string unit)
		{
			print("%s＼n", msg);
			int num = v.Length;
			for (int i = 0; i < num; i++)
			{
				print("%5d", v[i]);
				print("%s＼n", unit);
				print(",");
			}
			print("\n");
		}
	}
}
