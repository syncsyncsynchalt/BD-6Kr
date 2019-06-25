using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Cubemap : Texture
	{
		public int mipmapCount
		{
			get;
		}

		public TextureFormat format
		{
			get;
		}

		public Cubemap(int size, TextureFormat format, bool mipmap)
		{
			Internal_Create(this, size, format, mipmap);
		}

		public void SetPixel(CubemapFace face, int x, int y, Color color)
		{
			INTERNAL_CALL_SetPixel(this, face, x, y, ref color);
		}

		private static void INTERNAL_CALL_SetPixel(Cubemap self, CubemapFace face, int x, int y, ref Color color) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color GetPixel(CubemapFace face, int x, int y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Color[] GetPixels(CubemapFace face, [DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Color[] GetPixels(CubemapFace face)
		{
			int miplevel = 0;
			return GetPixels(face, miplevel);
		}

		public void SetPixels(Color[] colors, CubemapFace face, [DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors, CubemapFace face)
		{
			int miplevel = 0;
			SetPixels(colors, face, miplevel);
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

		private static void Internal_Create([Writable] Cubemap mono, int size, TextureFormat format, bool mipmap) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SmoothEdges([DefaultValue("1")] int smoothRegionWidthInPixels) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SmoothEdges()
		{
			int smoothRegionWidthInPixels = 1;
			SmoothEdges(smoothRegionWidthInPixels);
		}
	}
}
