using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	public class MatchInfo
	{
		public string address
		{
			get;
			private set;
		}

		public int port
		{
			get;
			private set;
		}

		public NetworkID networkId
		{
			get;
			private set;
		}

		public NetworkAccessToken accessToken
		{
			get;
			private set;
		}

		public NodeID nodeId
		{
			get;
			private set;
		}

		public bool usingRelay
		{
			get;
			private set;
		}

		public MatchInfo(CreateMatchResponse matchResponse)
		{
			address = matchResponse.address;
			port = matchResponse.port;
			networkId = matchResponse.networkId;
			accessToken = new NetworkAccessToken(matchResponse.accessTokenString);
			nodeId = matchResponse.nodeId;
			usingRelay = matchResponse.usingRelay;
		}

		public MatchInfo(JoinMatchResponse matchResponse)
		{
			address = matchResponse.address;
			port = matchResponse.port;
			networkId = matchResponse.networkId;
			accessToken = new NetworkAccessToken(matchResponse.accessTokenString);
			nodeId = matchResponse.nodeId;
			usingRelay = matchResponse.usingRelay;
		}

		public override string ToString()
		{
			return UnityString.Format("{0} @ {1}:{2} [{3},{4}]", networkId, address, port, nodeId, usingRelay);
		}
	}
}
