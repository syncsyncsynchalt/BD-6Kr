using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.Sprites
{
	public sealed class DataUtility
	{
		public static Vector4 GetInnerUV(Sprite sprite) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Vector4 GetOuterUV(Sprite sprite) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Vector4 GetPadding(Sprite sprite) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Vector2 GetMinSize(Sprite sprite)
		{
			Internal_GetMinSize(sprite, out Vector2 output);
			return output;
		}

		private static void Internal_GetMinSize(Sprite sprite, out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
