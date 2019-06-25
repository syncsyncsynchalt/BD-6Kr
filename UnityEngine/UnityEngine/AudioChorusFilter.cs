using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AudioChorusFilter : Behaviour
	{
		public float dryMix
		{
			get;
			set;
		}

		public float wetMix1
		{
			get;
			set;
		}

		public float wetMix2
		{
			get;
			set;
		}

		public float wetMix3
		{
			get;
			set;
		}

		public float delay
		{
			get;
			set;
		}

		public float rate
		{
			get;
			set;
		}

		public float depth
		{
			get;
			set;
		}

		[Obsolete("feedback is deprecated, this property does nothing.")]
		public float feedback
		{
			get;
			set;
		}
	}
}
