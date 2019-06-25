using System.Runtime.CompilerServices;

namespace UnityEngine.VR
{
	public sealed class VRSettings
	{
		public static bool enabled
		{
			get;
			set;
		}

		public static bool showDeviceView
		{
			get;
			set;
		}

		public static float renderScale
		{
			get;
			set;
		}

		public static VRDeviceType loadedDevice
		{
			get;
			set;
		}
	}
}
