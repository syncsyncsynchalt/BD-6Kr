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

		private void PlayInternal(Playable playable, object customData) { throw new NotImplementedException("�Ȃɂ���"); }

		public void Stop() { throw new NotImplementedException("�Ȃɂ���"); }

		public void SetTime(double time) { throw new NotImplementedException("�Ȃɂ���"); }

		public double GetTime() { throw new NotImplementedException("�Ȃɂ���"); }

		public void SetTimeUpdateMode(DirectorUpdateMode mode) { throw new NotImplementedException("�Ȃɂ���"); }

		public DirectorUpdateMode GetTimeUpdateMode() { throw new NotImplementedException("�Ȃɂ���"); }
	}
}
