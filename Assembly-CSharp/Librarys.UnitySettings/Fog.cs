using UnityEngine;

namespace Librarys.UnitySettings
{
	public sealed class Fog
	{
		public static bool fog
		{
			get
			{
				return RenderSettings.fog;
			}
			set
			{
				RenderSettings.fog = value;
			}
		}

		public static Color fogColor
		{
			get
			{
				return RenderSettings.fogColor;
			}
			set
			{
				RenderSettings.fogColor = value;
			}
		}

		public static float fogDensity
		{
			get
			{
				return RenderSettings.fogDensity;
			}
			set
			{
				RenderSettings.fogDensity = value;
			}
		}

		public static float fogStartDistance
		{
			get
			{
				return RenderSettings.fogStartDistance;
			}
			set
			{
				RenderSettings.fogStartDistance = value;
			}
		}

		public static float fogEndDistance
		{
			get
			{
				return RenderSettings.fogEndDistance;
			}
			set
			{
				RenderSettings.fogEndDistance = value;
			}
		}

		public static FogMode fogMode
		{
			get
			{
				return RenderSettings.fogMode;
			}
			set
			{
				RenderSettings.fogMode = value;
			}
		}
	}
}
