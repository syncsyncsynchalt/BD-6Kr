using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ColorUtility
	{
		internal static bool DoTryParseHtmlColor(string htmlString, out Color32 color) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool TryParseHtmlString(string htmlString, out Color color)
		{
			Color32 color2;
			bool result = DoTryParseHtmlColor(htmlString, out color2);
			color = color2;
			return result;
		}

		public static string ToHtmlStringRGB(Color color)
		{
			Color32 color2 = color;
			return $"{color2.r:X2}{color2.g:X2}{color2.b:X2}";
		}

		public static string ToHtmlStringRGBA(Color color)
		{
			Color32 color2 = color;
			return $"{color2.r:X2}{color2.g:X2}{color2.b:X2}{color2.a:X2}";
		}
	}
}
