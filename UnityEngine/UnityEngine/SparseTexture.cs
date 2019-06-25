using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class SparseTexture : Texture
	{
		public int tileWidth
		{
			get;
		}

		public int tileHeight
		{
			get;
		}

		public bool isCreated
		{
			get;
		}

		public SparseTexture(int width, int height, TextureFormat format, int mipCount)
		{
			Internal_Create(this, width, height, format, mipCount, linear: false);
		}

		public SparseTexture(int width, int height, TextureFormat format, int mipCount, bool linear)
		{
			Internal_Create(this, width, height, format, mipCount, linear);
		}

		private static void Internal_Create([Writable] SparseTexture mono, int width, int height, TextureFormat format, int mipCount, bool linear) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void UpdateTile(int tileX, int tileY, int miplevel, Color32[] data) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void UpdateTileRaw(int tileX, int tileY, int miplevel, byte[] data) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void UnloadTile(int tileX, int tileY, int miplevel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
