using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct NetworkViewID
	{
		private int a;

		private int b;

		private int c;

		public static NetworkViewID unassigned
		{
			get
			{
				INTERNAL_get_unassigned(out NetworkViewID value);
				return value;
			}
		}

		public bool isMine => Internal_IsMine(this);

		public NetworkPlayer owner
		{
			get
			{
				Internal_GetOwner(this, out NetworkPlayer player);
				return player;
			}
		}

		private static void INTERNAL_get_unassigned(out NetworkViewID value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static bool Internal_IsMine(NetworkViewID value)
		{
			return INTERNAL_CALL_Internal_IsMine(ref value);
		}

		private static bool INTERNAL_CALL_Internal_IsMine(ref NetworkViewID value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static void Internal_GetOwner(NetworkViewID value, out NetworkPlayer player)
		{
			INTERNAL_CALL_Internal_GetOwner(ref value, out player);
		}

		private static void INTERNAL_CALL_Internal_GetOwner(ref NetworkViewID value, out NetworkPlayer player) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static string Internal_GetString(NetworkViewID value)
		{
			return INTERNAL_CALL_Internal_GetString(ref value);
		}

		private static string INTERNAL_CALL_Internal_GetString(ref NetworkViewID value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static bool Internal_Compare(NetworkViewID lhs, NetworkViewID rhs)
		{
			return INTERNAL_CALL_Internal_Compare(ref lhs, ref rhs);
		}

		private static bool INTERNAL_CALL_Internal_Compare(ref NetworkViewID lhs, ref NetworkViewID rhs) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public override int GetHashCode()
		{
			return a ^ b ^ c;
		}

		public override bool Equals(object other)
		{
			if (!(other is NetworkViewID))
			{
				return false;
			}
			NetworkViewID rhs = (NetworkViewID)other;
			return Internal_Compare(this, rhs);
		}

		public override string ToString()
		{
			return Internal_GetString(this);
		}

		public static bool operator ==(NetworkViewID lhs, NetworkViewID rhs)
		{
			return Internal_Compare(lhs, rhs);
		}

		public static bool operator !=(NetworkViewID lhs, NetworkViewID rhs)
		{
			return !Internal_Compare(lhs, rhs);
		}
	}
}
