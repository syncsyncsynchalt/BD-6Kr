using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class MasterServer
	{
		public static string ipAddress
		{
			get;
			set;
		}

		public static int port
		{
			get;
			set;
		}

		public static int updateRate
		{
			get;
			set;
		}

		public static bool dedicatedServer
		{
			get;
			set;
		}

		public static void RequestHostList(string gameTypeName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static HostData[] PollHostList() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void RegisterHost(string gameTypeName, string gameName, [DefaultValue("\"\"")] string comment) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void RegisterHost(string gameTypeName, string gameName)
		{
			string empty = string.Empty;
			RegisterHost(gameTypeName, gameName, empty);
		}

		public static void UnregisterHost() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void ClearHostList() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
