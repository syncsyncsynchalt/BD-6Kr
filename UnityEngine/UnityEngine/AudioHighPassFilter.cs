using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioHighPassFilter : Behaviour
	{
		public float cutoffFrequency
		{
			get;
			set;
		}

		public float highpassResonanceQ
		{
			get;
			set;
		}
	}
}
