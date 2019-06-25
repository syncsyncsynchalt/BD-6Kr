using UnityEngine;

namespace KCV
{
	public static class TextureFile
	{
		public static Texture2D LoadRareBG(int nRare)
		{
			return Resources.Load($"Textures/Common/RareBG/s_rare_{nRare}") as Texture2D;
		}

		public static Texture2D LoadCardRareBG(int nRare)
		{
			return Resources.Load<Texture2D>($"Textures/Common/RareBG/c_rare_{nRare}");
		}

		public static Texture2D LoadOverlay()
		{
			return Resources.Load("Textures/Common/Overlay") as Texture2D;
		}
	}
}
