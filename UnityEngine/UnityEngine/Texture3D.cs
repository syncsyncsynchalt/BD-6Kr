using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Texture3D : Texture
	{
		public int depth
		{
			get;
		}

		public TextureFormat format
		{
			get;
		}

		public Texture3D(int width, int height, int depth, TextureFormat format, bool mipmap)
		{
			Internal_Create(this, width, height, depth, format, mipmap);
		}

		public Color[] GetPixels([DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Color[] GetPixels()
		{
			int miplevel = 0;
			return GetPixels(miplevel);
		}

		public Color32[] GetPixels32([DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			int miplevel = 0;
			return GetPixels32(miplevel);
		}

		public void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors)
		{
			int miplevel = 0;
			SetPixels(colors, miplevel);
		}

		public void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors)
		{
			int miplevel = 0;
			SetPixels32(colors, miplevel);
		}

		public void Apply([DefaultValue("true")] bool updateMipmaps) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void Apply()
		{
			bool updateMipmaps = true;
			Apply(updateMipmaps);
		}

		private static void Internal_Create([Writable] Texture3D mono, int width, int height, int depth, TextureFormat format, bool mipmap) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
