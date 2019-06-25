using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioLowPassFilter : Behaviour
	{
		public float cutoffFrequency
		{
			get;
			set;
		}

		public AnimationCurve customCutoffCurve
		{
			get;
			set;
		}

		public float lowpassResonanceQ
		{
			get;
			set;
		}
	}
}
