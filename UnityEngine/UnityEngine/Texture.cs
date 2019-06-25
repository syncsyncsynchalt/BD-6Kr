using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class Texture : Object
	{
		public static int masterTextureLimit
		{
			get;
			set;
		}

		public static AnisotropicFiltering anisotropicFiltering
		{
			get;
			set;
		}

		public virtual int width
		{
			get
			{
				return Internal_GetWidth(this);
			}
			set
			{
				throw new Exception("not implemented");
			}
		}

		public virtual int height
		{
			get
			{
				return Internal_GetHeight(this);
			}
			set
			{
				throw new Exception("not implemented");
			}
		}

		public FilterMode filterMode
		{
			get;
			set;
		}

		public int anisoLevel
		{
			get;
			set;
		}

		public TextureWrapMode wrapMode
		{
			get;
			set;
		}

		public float mipMapBias
		{
			get;
			set;
		}

		public Vector2 texelSize
		{
			get
			{
				INTERNAL_get_texelSize(out Vector2 value);
				return value;
			}
		}

		public static void SetGlobalAnisotropicFilteringLimits(int forcedMin, int globalMax) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetWidth(Texture mono) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static int Internal_GetHeight(Texture mono) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_texelSize(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public IntPtr GetNativeTexturePtr() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use GetNativeTexturePtr instead.")]
		public int GetNativeTextureID() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
