using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioReverbZone : Behaviour
	{
		public float minDistance
		{
			get;
			set;
		}

		public float maxDistance
		{
			get;
			set;
		}

		public AudioReverbPreset reverbPreset
		{
			get;
			set;
		}

		public int room
		{
			get;
			set;
		}

		public int roomHF
		{
			get;
			set;
		}

		public int roomLF
		{
			get;
			set;
		}

		public float decayTime
		{
			get;
			set;
		}

		public float decayHFRatio
		{
			get;
			set;
		}

		public int reflections
		{
			get;
			set;
		}

		public float reflectionsDelay
		{
			get;
			set;
		}

		public int reverb
		{
			get;
			set;
		}

		public float reverbDelay
		{
			get;
			set;
		}

		public float HFReference
		{
			get;
			set;
		}

		public float LFReference
		{
			get;
			set;
		}

		public float roomRolloffFactor
		{
			get;
			set;
		}

		public float diffusion
		{
			get;
			set;
		}

		public float density
		{
			get;
			set;
		}
	}
}
