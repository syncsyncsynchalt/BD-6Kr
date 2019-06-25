using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioEchoFilter : Behaviour
	{
		public float delay
		{
			get;
			set;
		}

		public float decayRatio
		{
			get;
			set;
		}

		public float dryMix
		{
			get;
			set;
		}

		public float wetMix
		{
			get;
			set;
		}
	}
}
