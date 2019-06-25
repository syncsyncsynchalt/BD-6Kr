using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Audio
{
	public class AudioMixer : Object
	{
		public AudioMixerGroup outputAudioMixerGroup
		{
			get;
			set;
		}

		internal AudioMixer()
		{
		}

		public AudioMixerGroup[] FindMatchingGroups(string subPath) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AudioMixerSnapshot FindSnapshot(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void TransitionToSnapshot(AudioMixerSnapshot snapshot, float timeToReach)
		{
			if (snapshot == null)
			{
				throw new ArgumentException("null Snapshot passed to AudioMixer.TransitionToSnapshot of AudioMixer '" + base.name + "'");
			}
			if (snapshot.audioMixer != this)
			{
				throw new ArgumentException("Snapshot '" + snapshot.name + "' passed to AudioMixer.TransitionToSnapshot is not a snapshot from AudioMixer '" + base.name + "'");
			}
			snapshot.TransitionTo(timeToReach);
		}

		public void TransitionToSnapshots(AudioMixerSnapshot[] snapshots, float[] weights, float timeToReach) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool SetFloat(string name, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool ClearFloat(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool GetFloat(string name, out float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
