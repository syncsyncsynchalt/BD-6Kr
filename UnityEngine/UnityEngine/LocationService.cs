using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class LocationService
	{
		public bool isEnabledByUser
		{
			get;
		}

		public LocationServiceStatus status
		{
			get;
		}

		public LocationInfo lastData
		{
			get;
		}

		public void Start([DefaultValue("10f")] float desiredAccuracyInMeters, [DefaultValue("10f")] float updateDistanceInMeters) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void Start(float desiredAccuracyInMeters)
		{
			float updateDistanceInMeters = 10f;
			Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		[ExcludeFromDocs]
		public void Start()
		{
			float updateDistanceInMeters = 10f;
			float desiredAccuracyInMeters = 10f;
			Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		public void Stop() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
