using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class WindZone : Component
	{
		public WindZoneMode mode
		{
			get;
			set;
		}

		public float radius
		{
			get;
			set;
		}

		public float windMain
		{
			get;
			set;
		}

		public float windTurbulence
		{
			get;
			set;
		}

		public float windPulseMagnitude
		{
			get;
			set;
		}

		public float windPulseFrequency
		{
			get;
			set;
		}
	}
}
