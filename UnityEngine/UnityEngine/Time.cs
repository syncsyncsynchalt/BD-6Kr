using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Time
	{
		public static float time
		{
			get;
		}

		public static float timeSinceLevelLoad
		{
			get;
		}

		public static float deltaTime
		{
			get;
		}

		public static float fixedTime
		{
			get;
		}

		public static float unscaledTime
		{
			get;
		}

		public static float unscaledDeltaTime
		{
			get;
		}

		public static float fixedDeltaTime
		{
			get;
			set;
		}

		public static float maximumDeltaTime
		{
			get;
			set;
		}

		public static float smoothDeltaTime
		{
			get;
		}

		public static float timeScale
		{
			get;
			set;
		}

		public static int frameCount
		{
			get;
		}

		public static int renderedFrameCount
		{
			get;
		}

		public static float realtimeSinceStartup
		{
			get;
		}

		public static int captureFramerate
		{
			get;
			set;
		}
	}
}
