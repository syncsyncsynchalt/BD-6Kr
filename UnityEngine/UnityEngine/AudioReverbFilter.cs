using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioReverbFilter : Behaviour
	{
		public AudioReverbPreset reverbPreset
		{
			get;
			set;
		}

		public float dryLevel
		{
			get;
			set;
		}

		public float room
		{
			get;
			set;
		}

		public float roomHF
		{
			get;
			set;
		}

		public float roomRolloff
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

		public float reflectionsLevel
		{
			get;
			set;
		}

		public float reflectionsDelay
		{
			get;
			set;
		}

		public float reverbLevel
		{
			get;
			set;
		}

		public float reverbDelay
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

		public float hfReference
		{
			get;
			set;
		}

		public float roomLF
		{
			get;
			set;
		}

		public float lfReference
		{
			get;
			set;
		}
	}
}
