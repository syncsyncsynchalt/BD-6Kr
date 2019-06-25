using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.Audio
{
	public class AudioMixerSnapshot : Object
	{
		public AudioMixer audioMixer
		{
			get;
		}

		internal AudioMixerSnapshot()
		{
		}

		public void TransitionTo(float timeToReach) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
