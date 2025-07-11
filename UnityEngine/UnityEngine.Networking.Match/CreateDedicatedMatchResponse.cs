using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match;

public class CreateDedicatedMatchResponse : BasicResponse
{
	public NetworkID networkId { get; set; }

	public NodeID nodeId { get; set; }

	public string address { get; set; }

	public int port { get; set; }

	public string accessTokenString { get; set; }

	public override void Parse(object obj)
	{
		base.Parse(obj);
		if (obj is IDictionary<string, object> dictJsonObj)
		{
			address = ParseJSONString("address", obj, dictJsonObj);
			port = ParseJSONInt32("port", obj, dictJsonObj);
			accessTokenString = ParseJSONString("accessTokenString", obj, dictJsonObj);
			networkId = (NetworkID)ParseJSONUInt64("networkId", obj, dictJsonObj);
			nodeId = (NodeID)ParseJSONUInt16("nodeId", obj, dictJsonObj);
			return;
		}
		throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
	}
}
