using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Texture2D : Texture
	{
		public int mipmapCount
		{
			get;
		}

		public TextureFormat format
		{
			get;
		}

		public static Texture2D whiteTexture
		{
			get;
		}

		public static Texture2D blackTexture
		{
			get;
		}

		public Texture2D(int width, int height)
		{
			Internal_Create(this, width, height, TextureFormat.ARGB32, mipmap: true, linear: false, IntPtr.Zero);
		}

		public Texture2D(int width, int height, TextureFormat format, bool mipmap)
		{
			Internal_Create(this, width, height, format, mipmap, linear: false, IntPtr.Zero);
		}

		public Texture2D(int width, int height, TextureFormat format, bool mipmap, bool linear)
		{
			Internal_Create(this, width, height, format, mipmap, linear, IntPtr.Zero);
		}

		private static void Internal_Create([Writable] Texture2D mono, int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetPixel(int x, int y, Color color)
		{
			INTERNAL_CALL_SetPixel(this, x, y, ref color);
		}

		private static void INTERNAL_CALL_SetPixel(Texture2D self, int x, int y, ref Color color) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color GetPixel(int x, int y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color GetPixelBilinear(float u, float v) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors)
		{
			int miplevel = 0;
			SetPixels(colors, miplevel);
		}

		public void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel)
		{
			int num = width >> miplevel;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = height >> miplevel;
			if (num2 < 1)
			{
				num2 = 1;
			}
			SetPixels(0, 0, num, num2, colors, miplevel);
		}

		public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, [DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors)
		{
			int miplevel = 0;
			SetPixels(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		private void SetAllPixels32(Color32[] colors, int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetBlockOfPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors)
		{
			int miplevel = 0;
			SetPixels32(colors, miplevel);
		}

		public void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel)
		{
			SetAllPixels32(colors, miplevel);
		}

		[ExcludeFromDocs]
		public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors)
		{
			int miplevel = 0;
			SetPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, [DefaultValue("0")] int miplevel)
		{
			SetBlockOfPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		public bool LoadImage(byte[] data) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void LoadRawTextureData(byte[] data) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public byte[] GetRawTextureData() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Color[] GetPixels()
		{
			int miplevel = 0;
			return GetPixels(miplevel);
		}

		public Color[] GetPixels([DefaultValue("0")] int miplevel)
		{
			int num = width >> miplevel;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = height >> miplevel;
			if (num2 < 1)
			{
				num2 = 1;
			}
			return GetPixels(0, 0, num, num2, miplevel);
		}

		public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, [DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight)
		{
			int miplevel = 0;
			return GetPixels(x, y, blockWidth, blockHeight, miplevel);
		}

		public Color32[] GetPixels32([DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			int miplevel = 0;
			return GetPixels32(miplevel);
		}

		public void Apply([DefaultValue("true")] bool updateMipmaps, [DefaultValue("false")] bool makeNoLongerReadable) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void Apply(bool updateMipmaps)
		{
			bool makeNoLongerReadable = false;
			Apply(updateMipmaps, makeNoLongerReadable);
		}

		[ExcludeFromDocs]
		public void Apply()
		{
			bool makeNoLongerReadable = false;
			bool updateMipmaps = true;
			Apply(updateMipmaps, makeNoLongerReadable);
		}

		public bool Resize(int width, int height, TextureFormat format, bool hasMipMap) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool Resize(int width, int height)
		{
			return Internal_ResizeWH(width, height);
		}

		private bool Internal_ResizeWH(int width, int height) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Compress(bool highQuality)
		{
			INTERNAL_CALL_Compress(this, highQuality);
		}

		private static void INTERNAL_CALL_Compress(Texture2D self, bool highQuality) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Rect[] PackTextures(Texture2D[] textures, int padding, [DefaultValue("2048")] int maximumAtlasSize, [DefaultValue("false")] bool makeNoLongerReadable) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize)
		{
			bool makeNoLongerReadable = false;
			return PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
		}

		[ExcludeFromDocs]
		public Rect[] PackTextures(Texture2D[] textures, int padding)
		{
			bool makeNoLongerReadable = false;
			int maximumAtlasSize = 2048;
			return PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
		}

		public void ReadPixels(Rect source, int destX, int destY, [DefaultValue("true")] bool recalculateMipMaps)
		{
			INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
		}

		[ExcludeFromDocs]
		public void ReadPixels(Rect source, int destX, int destY)
		{
			bool recalculateMipMaps = true;
			INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
		}

		private static void INTERNAL_CALL_ReadPixels(Texture2D self, ref Rect source, int destX, int destY, bool recalculateMipMaps) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public byte[] EncodeToPNG() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public byte[] EncodeToJPG(int quality) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public byte[] EncodeToJPG()
		{
			return EncodeToJPG(75);
		}
	}
}
