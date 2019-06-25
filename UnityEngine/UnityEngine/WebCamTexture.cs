using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class WebCamTexture : Texture
	{
		public bool isPlaying
		{
			get;
		}

		public string deviceName
		{
			get;
			set;
		}

		public float requestedFPS
		{
			get;
			set;
		}

		public int requestedWidth
		{
			get;
			set;
		}

		public int requestedHeight
		{
			get;
			set;
		}

		public static WebCamDevice[] devices
		{
			get;
		}

		public int videoRotationAngle
		{
			get;
		}

		public bool videoVerticallyMirrored
		{
			get;
		}

		public bool didUpdateThisFrame
		{
			get;
		}

		public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight, int requestedFPS)
		{
			Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, requestedFPS);
		}

		public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight)
		{
			Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, 0);
		}

		public WebCamTexture(string deviceName)
		{
			Internal_CreateWebCamTexture(this, deviceName, 0, 0, 0);
		}

		public WebCamTexture(int requestedWidth, int requestedHeight, int requestedFPS)
		{
			Internal_CreateWebCamTexture(this, string.Empty, requestedWidth, requestedHeight, requestedFPS);
		}

		public WebCamTexture(int requestedWidth, int requestedHeight)
		{
			Internal_CreateWebCamTexture(this, string.Empty, requestedWidth, requestedHeight, 0);
		}

		public WebCamTexture()
		{
			Internal_CreateWebCamTexture(this, string.Empty, 0, 0, 0);
		}

		private static void Internal_CreateWebCamTexture([Writable] WebCamTexture self, string scriptingDevice, int requestedWidth, int requestedHeight, int maxFramerate) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Play()
		{
			INTERNAL_CALL_Play(this);
		}

		private static void INTERNAL_CALL_Play(WebCamTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Pause()
		{
			INTERNAL_CALL_Pause(this);
		}

		private static void INTERNAL_CALL_Pause(WebCamTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Stop()
		{
			INTERNAL_CALL_Stop(this);
		}

		private static void INTERNAL_CALL_Stop(WebCamTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color GetPixel(int x, int y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color[] GetPixels()
		{
			return GetPixels(0, 0, width, height);
		}

		public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color32[] GetPixels32([DefaultValue("null")] Color32[] colors) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			Color32[] colors = null;
			return GetPixels32(colors);
		}
	}
}
