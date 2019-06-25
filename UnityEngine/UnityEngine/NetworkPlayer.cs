using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct NetworkPlayer
	{
		internal int index;

		public string ipAddress
		{
			get
			{
				if (index == Internal_GetPlayerIndex())
				{
					return Internal_GetLocalIP();
				}
				return Internal_GetIPAddress(index);
			}
		}

		public int port
		{
			get
			{
				if (index == Internal_GetPlayerIndex())
				{
					return Internal_GetLocalPort();
				}
				return Internal_GetPort(index);
			}
		}

		public string guid
		{
			get
			{
				if (index == Internal_GetPlayerIndex())
				{
					return Internal_GetLocalGUID();
				}
				return Internal_GetGUID(index);
			}
		}

		public string externalIP => Internal_GetExternalIP();

		public int externalPort => Internal_GetExternalPort();

		internal static NetworkPlayer unassigned
		{
			get
			{
				NetworkPlayer result = default(NetworkPlayer);
				result.index = -1;
				return result;
			}
		}

		public NetworkPlayer(string ip, int port)
		{
			Debug.LogError("Not yet implemented");
			index = 0;
		}

		private static string Internal_GetIPAddress(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetPort(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static string Internal_GetExternalIP() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetExternalPort() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static string Internal_GetLocalIP() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetLocalPort() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetPlayerIndex() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static string Internal_GetGUID(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static string Internal_GetLocalGUID() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public override bool Equals(object other)
		{
			if (!(other is NetworkPlayer))
			{
				return false;
			}
			NetworkPlayer networkPlayer = (NetworkPlayer)other;
			return networkPlayer.index == index;
		}

		public override string ToString()
		{
			return index.ToString();
		}

		public static bool operator ==(NetworkPlayer lhs, NetworkPlayer rhs)
		{
			return lhs.index == rhs.index;
		}

		public static bool operator !=(NetworkPlayer lhs, NetworkPlayer rhs)
		{
			return lhs.index != rhs.index;
		}
	}
}
