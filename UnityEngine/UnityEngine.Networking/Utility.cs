using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking;

public class Utility
{
	private static System.Random s_randomGenerator = new System.Random(Environment.TickCount);

	private static bool s_useRandomSourceID = false;

	private static int s_randomSourceComponent = 0;

	private static AppID s_programAppID = AppID.Invalid;

	private static Dictionary<NetworkID, NetworkAccessToken> s_dictTokens = new Dictionary<NetworkID, NetworkAccessToken>();

	public static bool useRandomSourceID
	{
		get
		{
			return s_useRandomSourceID;
		}
		set
		{
			SetUseRandomSourceID(value);
		}
	}

	private Utility()
	{
	}

	public static SourceID GetSourceID()
	{
		return (SourceID)(SystemInfo.deviceUniqueIdentifier + s_randomSourceComponent).GetHashCode();
	}

	private static void SetUseRandomSourceID(bool useRandomSourceID)
	{
		if (useRandomSourceID && !s_useRandomSourceID)
		{
			s_randomSourceComponent = s_randomGenerator.Next(int.MaxValue);
		}
		else if (!useRandomSourceID && s_useRandomSourceID)
		{
			s_randomSourceComponent = 0;
		}
		s_useRandomSourceID = useRandomSourceID;
	}

	public static void SetAppID(AppID newAppID)
	{
		s_programAppID = newAppID;
	}

	public static AppID GetAppID()
	{
		return s_programAppID;
	}

	public static void SetAccessTokenForNetwork(NetworkID netId, NetworkAccessToken accessToken)
	{
		s_dictTokens.Add(netId, accessToken);
	}

	public static NetworkAccessToken GetAccessTokenForNetwork(NetworkID netId)
	{
		if (!s_dictTokens.TryGetValue(netId, out var value))
		{
			return new NetworkAccessToken();
		}
		return value;
	}
}
