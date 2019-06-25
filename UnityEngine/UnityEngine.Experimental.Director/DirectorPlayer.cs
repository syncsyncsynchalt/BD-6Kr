using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
	public class DirectorPlayer : Behaviour
	{
		public void Play(Playable playable, object customData)
		{
			PlayInternal(playable, customData);
		}

		public void Play(Playable playable)
		{
			PlayInternal(playable, null);
		}

		private void PlayInternal(Playable playable, object customData) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Stop() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTime(double time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public double GetTime() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTimeUpdateMode(DirectorUpdateMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public DirectorUpdateMode GetTimeUpdateMode() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
