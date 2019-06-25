using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace UnityEngine
{
	internal sealed class UnityLogWriter : TextWriter
	{
		public override Encoding Encoding => Encoding.UTF8;

		public static void WriteStringToUnityLog(string s) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void Init()
		{
			Console.SetOut(new UnityLogWriter());
		}

		public override void Write(char value)
		{
			WriteStringToUnityLog(value.ToString());
		}

		public override void Write(string s)
		{
			WriteStringToUnityLog(s);
		}
	}
}
