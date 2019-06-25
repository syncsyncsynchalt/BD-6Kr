using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
	public class ListMatchRequest : Request
	{
		public int pageSize
		{
			get;
			set;
		}

		public int pageNum
		{
			get;
			set;
		}

		public string nameFilter
		{
			get;
			set;
		}

		public bool includePasswordMatches
		{
			get;
			set;
		}

		public int eloScore
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributeFilterLessThan
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributeFilterEqualTo
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributeFilterGreaterThan
		{
			get;
			set;
		}

		public override string ToString()
		{
			return UnityString.Format("[{0}]-pageSize:{1},pageNum:{2},nameFilter:{3},matchAttributeFilterLessThan.Count:{4}, matchAttributeFilterGreaterThan.Count:{5}", base.ToString(), pageSize, pageNum, nameFilter, (matchAttributeFilterLessThan != null) ? matchAttributeFilterLessThan.Count : 0, (matchAttributeFilterGreaterThan != null) ? matchAttributeFilterGreaterThan.Count : 0);
		}

		public override bool IsValid()
		{
			int num = (matchAttributeFilterLessThan != null) ? matchAttributeFilterLessThan.Count : 0;
			num += ((matchAttributeFilterEqualTo != null) ? matchAttributeFilterEqualTo.Count : 0);
			num += ((matchAttributeFilterGreaterThan != null) ? matchAttributeFilterGreaterThan.Count : 0);
			return base.IsValid() && (pageSize >= 1 || pageSize <= 1000) && num <= 10;
		}
	}
}
