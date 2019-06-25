using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
	public class CreateMatchRequest : Request
	{
		public string name
		{
			get;
			set;
		}

		public uint size
		{
			get;
			set;
		}

		public string publicAddress
		{
			get;
			set;
		}

		public string privateAddress
		{
			get;
			set;
		}

		public int eloScore
		{
			get;
			set;
		}

		public bool advertise
		{
			get;
			set;
		}

		public string password
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributes
		{
			get;
			set;
		}

		public override string ToString()
		{
			return UnityString.Format("[{0}]-name:{1},size:{2},advertise:{3},HasPassword:{4},matchAttributes.Count:{5}", base.ToString(), name, size, advertise, (!(password == string.Empty)) ? "YES" : "NO", (matchAttributes != null) ? matchAttributes.Count : 0);
		}

		public override bool IsValid()
		{
			return (base.IsValid() && size >= 2 && matchAttributes == null) || matchAttributes.Count <= 10;
		}
	}
}
