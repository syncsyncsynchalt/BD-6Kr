using UnityEngine;

namespace KCV.Generic
{
	public struct KCVColor
	{
		private static Color _cBlueText = new Color(0f, Mathe.Rate(0f, 255f, 80f), 1f, 0.5f);

		public static readonly Color WarVateransGaugeGreen = new Color(ColorRate(89f), ColorRate(233f), ColorRate(50f), 1f);

		public static readonly Color WarVateransGaugeRed = new Color(ColorRate(227f), ColorRate(44f), ColorRate(44f), 1f);

		public static readonly Color WarVateransEXPGaugeGreen = new Color(ColorRate(163f), ColorRate(233f), ColorRate(208f));

		public static readonly Color BattleBlueLineColor = new Color(0f, ColorRate(80f), 1f, 0.5f);

		public static readonly Color BattleCommandSurfaceBlue = new Color(ColorRate(50f), ColorRate(154f), ColorRate(214f), 1f);

		public static Color blueTextBG => _cBlueText;

		public static float ColorRate(float val)
		{
			return Mathe.Rate(0f, 255f, val);
		}

		public static Color ConvertColor(float r, float g, float b, float a)
		{
			return new Color(ColorRate(r), ColorRate(g), ColorRate(b), ColorRate(a));
		}
	}
}
