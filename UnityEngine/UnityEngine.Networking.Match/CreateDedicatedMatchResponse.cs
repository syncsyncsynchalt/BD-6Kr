using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	public class CreateDedicatedMatchResponse : BasicResponse
	{
		public NetworkID networkId
		{
			get;
			set;
		}

		public NodeID nodeId
		{
			get;
			set;
		}

		public string address
		{
			get;
			set;
		}

		public int port
		{
			get;
			set;
		}

		public string accessTokenString
		{
			get;
			set;
		}

		public override void Parse(object obj)
		{
			base.Parse(obj);
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary != null)
			{
				address = ParseJSONString("address", obj, dictionary);
				port = ParseJSONInt32("port", obj, dictionary);
				accessTokenString = ParseJSONString("accessTokenString", obj, dictionary);
				networkId = (NetworkID)ParseJSONUInt64("networkId", obj, dictionary);
				nodeId = (NodeID)ParseJSONUInt16("nodeId", obj, dictionary);
				return;
			}
			throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
		}
	}
}
