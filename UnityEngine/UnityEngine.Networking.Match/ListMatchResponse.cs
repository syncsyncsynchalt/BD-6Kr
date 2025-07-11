using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match;

public class ListMatchResponse : BasicResponse
{
	public List<MatchDesc> matches { get; set; }

	public ListMatchResponse()
	{
	}

	public ListMatchResponse(List<MatchDesc> otherMatches)
	{
		matches = otherMatches;
	}

	public override string ToString()
	{
		return UnityString.Format("[{0}]-matches.Count:{1}", base.ToString(), matches.Count);
	}

	public override void Parse(object obj)
	{
		base.Parse(obj);
		if (obj is IDictionary<string, object> dictJsonObj)
		{
			matches = ParseJSONList<MatchDesc>("matches", obj, dictJsonObj);
			return;
		}
		throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
	}
}
