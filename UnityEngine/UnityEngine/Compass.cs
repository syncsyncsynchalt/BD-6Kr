using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Compass
	{
		public float magneticHeading
		{
			get;
		}

		public float trueHeading
		{
			get;
		}

		public float headingAccuracy
		{
			get;
		}

		public Vector3 rawVector
		{
			get
			{
				INTERNAL_get_rawVector(out Vector3 value);
				return value;
			}
		}

		public double timestamp
		{
			get;
		}

		public bool enabled
		{
			get;
			set;
		}

		private void INTERNAL_get_rawVector(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
