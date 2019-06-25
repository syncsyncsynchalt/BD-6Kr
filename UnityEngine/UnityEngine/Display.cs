using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Display
	{
		public delegate void DisplaysUpdatedDelegate();

		internal IntPtr nativeDisplay;

		public static Display[] displays = new Display[1]
		{
			new Display()
		};

		private static Display _mainDisplay = displays[0];

		public int renderingWidth
		{
			get
			{
				int w = 0;
				int h = 0;
				GetRenderingExtImpl(nativeDisplay, out w, out h);
				return w;
			}
		}

		public int renderingHeight
		{
			get
			{
				int w = 0;
				int h = 0;
				GetRenderingExtImpl(nativeDisplay, out w, out h);
				return h;
			}
		}

		public int systemWidth
		{
			get
			{
				int w = 0;
				int h = 0;
				GetSystemExtImpl(nativeDisplay, out w, out h);
				return w;
			}
		}

		public int systemHeight
		{
			get
			{
				int w = 0;
				int h = 0;
				GetSystemExtImpl(nativeDisplay, out w, out h);
				return h;
			}
		}

		public RenderBuffer colorBuffer
		{
			get
			{
				GetRenderingBuffersImpl(nativeDisplay, out RenderBuffer color, out RenderBuffer _);
				return color;
			}
		}

		public RenderBuffer depthBuffer
		{
			get
			{
				GetRenderingBuffersImpl(nativeDisplay, out RenderBuffer _, out RenderBuffer depth);
				return depth;
			}
		}

		public static Display main => _mainDisplay;

		public static event DisplaysUpdatedDelegate onDisplaysUpdated;

		internal Display()
		{
			nativeDisplay = new IntPtr(0);
		}

		internal Display(IntPtr nativeDisplay)
		{
			this.nativeDisplay = nativeDisplay;
		}

		static Display()
		{
			Display.onDisplaysUpdated = null;
		}

		public void Activate()
		{
			ActivateDisplayImpl(nativeDisplay, 0, 0, 60);
		}

		public void Activate(int width, int height, int refreshRate)
		{
			ActivateDisplayImpl(nativeDisplay, width, height, refreshRate);
		}

		public void SetParams(int width, int height, int x, int y)
		{
			SetParamsImpl(nativeDisplay, width, height, x, y);
		}

		public void SetRenderingResolution(int w, int h)
		{
			SetRenderingResolutionImpl(nativeDisplay, w, h);
		}

		public static bool MultiDisplayLicense()
		{
			return MultiDisplayLicenseImpl();
		}

		public static Vector3 RelativeMouseAt(Vector3 inputMouseCoordinates)
		{
			int rx = 0;
			int ry = 0;
			int x = (int)inputMouseCoordinates.x;
			int y = (int)inputMouseCoordinates.y;
			Vector3 result = default(Vector3);
			result.z = RelativeMouseAtImpl(x, y, out rx, out ry);
			result.x = rx;
			result.y = ry;
			return result;
		}

		private static void RecreateDisplayList(IntPtr[] nativeDisplay)
		{
			displays = new Display[nativeDisplay.Length];
			for (int i = 0; i < nativeDisplay.Length; i++)
			{
				displays[i] = new Display(nativeDisplay[i]);
			}
			_mainDisplay = displays[0];
		}

		private static void FireDisplaysUpdated()
		{
			if (Display.onDisplaysUpdated != null)
			{
				Display.onDisplaysUpdated();
			}
		}

		private static void GetSystemExtImpl(IntPtr nativeDisplay, out int w, out int h) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void GetRenderingExtImpl(IntPtr nativeDisplay, out int w, out int h) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void GetRenderingBuffersImpl(IntPtr nativeDisplay, out RenderBuffer color, out RenderBuffer depth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void SetRenderingResolutionImpl(IntPtr nativeDisplay, int w, int h) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void ActivateDisplayImpl(IntPtr nativeDisplay, int width, int height, int refreshRate) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void SetParamsImpl(IntPtr nativeDisplay, int width, int height, int x, int y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool MultiDisplayLicenseImpl() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int RelativeMouseAtImpl(int x, int y, out int rx, out int ry) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
