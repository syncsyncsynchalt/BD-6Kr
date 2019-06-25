using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class RenderTexture : Texture
	{
		public override int width
		{
			get
			{
				return Internal_GetWidth(this);
			}
			set
			{
				Internal_SetWidth(this, value);
			}
		}

		public override int height
		{
			get
			{
				return Internal_GetHeight(this);
			}
			set
			{
				Internal_SetHeight(this, value);
			}
		}

		public int depth
		{
			get;
			set;
		}

		public bool isPowerOfTwo
		{
			get;
			set;
		}

		public bool sRGB
		{
			get;
		}

		public RenderTextureFormat format
		{
			get;
			set;
		}

		public bool useMipMap
		{
			get;
			set;
		}

		public bool generateMips
		{
			get;
			set;
		}

		public bool isCubemap
		{
			get;
			set;
		}

		public bool isVolume
		{
			get;
			set;
		}

		public int volumeDepth
		{
			get;
			set;
		}

		public int antiAliasing
		{
			get;
			set;
		}

		public bool enableRandomWrite
		{
			get;
			set;
		}

		public RenderBuffer colorBuffer
		{
			get
			{
				GetColorBuffer(out RenderBuffer res);
				return res;
			}
		}

		public RenderBuffer depthBuffer
		{
			get
			{
				GetDepthBuffer(out RenderBuffer res);
				return res;
			}
		}

		public static RenderTexture active
		{
			get;
			set;
		}

		[Obsolete("Use SystemInfo.supportsRenderTextures instead.")]
		public static bool enabled
		{
			get;
			set;
		}

		public RenderTexture(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			Internal_CreateRenderTexture(this);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.format = format;
			bool sRGB = readWrite == RenderTextureReadWrite.sRGB;
			if (readWrite == RenderTextureReadWrite.Default)
			{
				sRGB = (QualitySettings.activeColorSpace == ColorSpace.Linear);
			}
			Internal_SetSRGBReadWrite(this, sRGB);
		}

		public RenderTexture(int width, int height, int depth, RenderTextureFormat format)
		{
			Internal_CreateRenderTexture(this);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.format = format;
			Internal_SetSRGBReadWrite(this, QualitySettings.activeColorSpace == ColorSpace.Linear);
		}

		public RenderTexture(int width, int height, int depth)
		{
			Internal_CreateRenderTexture(this);
			this.width = width;
			this.height = height;
			this.depth = depth;
			format = RenderTextureFormat.Default;
			Internal_SetSRGBReadWrite(this, QualitySettings.activeColorSpace == ColorSpace.Linear);
		}

		private static void Internal_CreateRenderTexture([Writable] RenderTexture rt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static RenderTexture GetTemporary(int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			int antiAliasing = 1;
			return GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			return GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			return GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			int depthBuffer = 0;
			return GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		public static void ReleaseTemporary(RenderTexture temp) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetWidth(RenderTexture mono) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetWidth(RenderTexture mono, int width) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetHeight(RenderTexture mono) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetHeight(RenderTexture mono, int width) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_SetSRGBReadWrite(RenderTexture mono, bool sRGB) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Create()
		{
			return INTERNAL_CALL_Create(this);
		}

		private static bool INTERNAL_CALL_Create(RenderTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Release()
		{
			INTERNAL_CALL_Release(this);
		}

		private static void INTERNAL_CALL_Release(RenderTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsCreated()
		{
			return INTERNAL_CALL_IsCreated(this);
		}

		private static bool INTERNAL_CALL_IsCreated(RenderTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void DiscardContents()
		{
			INTERNAL_CALL_DiscardContents(this);
		}

		private static void INTERNAL_CALL_DiscardContents(RenderTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void DiscardContents(bool discardColor, bool discardDepth) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void MarkRestoreExpected()
		{
			INTERNAL_CALL_MarkRestoreExpected(this);
		}

		private static void INTERNAL_CALL_MarkRestoreExpected(RenderTexture self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetColorBuffer(out RenderBuffer res) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetDepthBuffer(out RenderBuffer res) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetGlobalShaderProperty(string propertyName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_GetTexelOffset(RenderTexture tex, out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector2 GetTexelOffset()
		{
			Internal_GetTexelOffset(this, out Vector2 output);
			return output;
		}

		public static bool SupportsStencil(RenderTexture rt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("SetBorderColor is no longer supported.", true)]
		public void SetBorderColor(Color color)
		{
		}
	}
}
