using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Screen
	{
		public static Resolution[] resolutions
		{
			get;
		}

		[Obsolete("Property lockCursor has been deprecated. Use Cursor.lockState and Cursor.visible instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool lockCursor
		{
			get
			{
				return CursorLockMode.None == Cursor.lockState;
			}
			set
			{
				if (value)
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
				else
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
		}

		public static Resolution currentResolution
		{
			get;
		}

		public static int width
		{
			get;
		}

		public static int height
		{
			get;
		}

		public static float dpi
		{
			get;
		}

		public static bool fullScreen
		{
			get;
			set;
		}

		public static bool autorotateToPortrait
		{
			get;
			set;
		}

		public static bool autorotateToPortraitUpsideDown
		{
			get;
			set;
		}

		public static bool autorotateToLandscapeLeft
		{
			get;
			set;
		}

		public static bool autorotateToLandscapeRight
		{
			get;
			set;
		}

		public static ScreenOrientation orientation
		{
			get;
			set;
		}

		public static int sleepTimeout
		{
			get;
			set;
		}

		public static void SetResolution(int width, int height, bool fullscreen, [UnityEngine.Internal.DefaultValue("0")] int preferredRefreshRate) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static void SetResolution(int width, int height, bool fullscreen)
		{
			int preferredRefreshRate = 0;
			SetResolution(width, height, fullscreen, preferredRefreshRate);
		}
	}
}
