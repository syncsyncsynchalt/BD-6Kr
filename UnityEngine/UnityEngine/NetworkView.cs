using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class NetworkView : Behaviour
	{
		public Component observed
		{
			get;
			set;
		}

		public NetworkStateSynchronization stateSynchronization
		{
			get;
			set;
		}

		public NetworkViewID viewID
		{
			get
			{
				Internal_GetViewID(out NetworkViewID viewID);
				return viewID;
			}
			set
			{
				Internal_SetViewID(value);
			}
		}

		public int group
		{
			get;
			set;
		}

		public bool isMine => viewID.isMine;

		public NetworkPlayer owner => viewID.owner;

		private static void Internal_RPC(NetworkView view, string name, RPCMode mode, object[] args) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_RPC_Target(NetworkView view, string name, NetworkPlayer target, object[] args) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("NetworkView RPC functions are deprecated. Refer to the new Multiplayer Networking system.")]
		public void RPC(string name, RPCMode mode, params object[] args)
		{
			Internal_RPC(this, name, mode, args);
		}

		[Obsolete("NetworkView RPC functions are deprecated. Refer to the new Multiplayer Networking system.")]
		public void RPC(string name, NetworkPlayer target, params object[] args)
		{
			Internal_RPC_Target(this, name, target, args);
		}

		private void Internal_GetViewID(out NetworkViewID viewID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_SetViewID(NetworkViewID viewID)
		{
			INTERNAL_CALL_Internal_SetViewID(this, ref viewID);
		}

		private static void INTERNAL_CALL_Internal_SetViewID(NetworkView self, ref NetworkViewID viewID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SetScope(NetworkPlayer player, bool relevancy) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static NetworkView Find(NetworkViewID viewID)
		{
			return INTERNAL_CALL_Find(ref viewID);
		}

		private static NetworkView INTERNAL_CALL_Find(ref NetworkViewID viewID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
