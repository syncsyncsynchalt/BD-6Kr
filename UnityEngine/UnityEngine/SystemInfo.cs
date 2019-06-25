using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class SystemInfo
	{
		public static string operatingSystem
		{
			get;
		}

		public static string processorType
		{
			get;
		}

		public static int processorCount
		{
			get;
		}

		public static int systemMemorySize
		{
			get;
		}

		public static int graphicsMemorySize
		{
			get;
		}

		public static string graphicsDeviceName
		{
			get;
		}

		public static string graphicsDeviceVendor
		{
			get;
		}

		public static int graphicsDeviceID
		{
			get;
		}

		public static int graphicsDeviceVendorID
		{
			get;
		}

		public static GraphicsDeviceType graphicsDeviceType
		{
			get;
		}

		public static string graphicsDeviceVersion
		{
			get;
		}

		public static int graphicsShaderLevel
		{
			get;
		}

		[Obsolete("graphicsPixelFillrate is no longer supported in Unity 5.0+.")]
		public static int graphicsPixelFillrate => -1;

		[Obsolete("Vertex program support is required in Unity 5.0+")]
		public static bool supportsVertexPrograms => true;

		public static bool graphicsMultiThreaded
		{
			get;
		}

		public static bool supportsShadows
		{
			get;
		}

		public static bool supportsRenderTextures
		{
			get;
		}

		public static bool supportsRenderToCubemap
		{
			get;
		}

		public static bool supportsImageEffects
		{
			get;
		}

		public static bool supports3DTextures
		{
			get;
		}

		public static bool supportsComputeShaders
		{
			get;
		}

		public static bool supportsInstancing
		{
			get;
		}

		public static bool supportsSparseTextures
		{
			get;
		}

		public static int supportedRenderTargetCount
		{
			get;
		}

		public static int supportsStencil
		{
			get;
		}

		public static NPOTSupport npotSupport
		{
			get;
		}

		public static string deviceUniqueIdentifier
		{
			get;
		}

		public static string deviceName
		{
			get;
		}

		public static string deviceModel
		{
			get;
		}

		public static bool supportsAccelerometer
		{
			get;
		}

		public static bool supportsGyroscope
		{
			get;
		}

		public static bool supportsLocationService
		{
			get;
		}

		public static bool supportsVibration
		{
			get;
		}

		public static DeviceType deviceType
		{
			get;
		}

		public static int maxTextureSize
		{
			get;
		}

		public static bool SupportsRenderTextureFormat(RenderTextureFormat format) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool SupportsTextureFormat(TextureFormat format) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
