using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Shader : Object
	{
		public bool isSupported
		{
			get;
		}

		public int maximumLOD
		{
			get;
			set;
		}

		public static int globalMaximumLOD
		{
			get;
			set;
		}

		public int renderQueue
		{
			get;
		}

		internal DisableBatchingType disableBatching
		{
			get;
		}

		public static Shader Find(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static Shader FindBuiltin(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void EnableKeyword(string keyword) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void DisableKeyword(string keyword) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool IsKeywordEnabled(string keyword) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetGlobalColor(string propertyName, Color color)
		{
			SetGlobalColor(PropertyToID(propertyName), color);
		}

		public static void SetGlobalColor(int nameID, Color color)
		{
			INTERNAL_CALL_SetGlobalColor(nameID, ref color);
		}

		private static void INTERNAL_CALL_SetGlobalColor(int nameID, ref Color color) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetGlobalVector(string propertyName, Vector4 vec)
		{
			SetGlobalColor(propertyName, vec);
		}

		public static void SetGlobalVector(int nameID, Vector4 vec)
		{
			SetGlobalColor(nameID, vec);
		}

		public static void SetGlobalFloat(string propertyName, float value)
		{
			SetGlobalFloat(PropertyToID(propertyName), value);
		}

		public static void SetGlobalFloat(int nameID, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetGlobalInt(string propertyName, int value)
		{
			SetGlobalFloat(propertyName, value);
		}

		public static void SetGlobalInt(int nameID, int value)
		{
			SetGlobalFloat(nameID, value);
		}

		public static void SetGlobalTexture(string propertyName, Texture tex)
		{
			SetGlobalTexture(PropertyToID(propertyName), tex);
		}

		public static void SetGlobalTexture(int nameID, Texture tex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetGlobalMatrix(string propertyName, Matrix4x4 mat)
		{
			SetGlobalMatrix(PropertyToID(propertyName), mat);
		}

		public static void SetGlobalMatrix(int nameID, Matrix4x4 mat)
		{
			INTERNAL_CALL_SetGlobalMatrix(nameID, ref mat);
		}

		private static void INTERNAL_CALL_SetGlobalMatrix(int nameID, ref Matrix4x4 mat) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("SetGlobalTexGenMode is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
		public static void SetGlobalTexGenMode(string propertyName, TexGenMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("SetGlobalTextureMatrixName is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
		public static void SetGlobalTextureMatrixName(string propertyName, string matrixName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetGlobalBuffer(string propertyName, ComputeBuffer buffer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int PropertyToID(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void WarmupAllShaders() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
