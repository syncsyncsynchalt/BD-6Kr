using UnityEngine;

public class AppDataPath
{
	public static string ResourcesFilePath => string.Format("{0}/{1}", Application.dataPath, "Resources");

	public static string PrefabFilePath => string.Format("{0}", "Prefabs");

	public static string ParticleFilePath => string.Format("{0}", "Particles");

	public static string TextureFilePath => string.Format("{0}", "Textures");

	public static string ShipTexturePath => string.Format("{0}/{1}", TextureFilePath, "Ships");

	public static string ShaderFilePath => string.Format("{0}", "Shader");

	public static string AnimationFilePath => string.Format("{0}", "Animations");

	public static string SoundFilePath => string.Format("{0}", "Sounds");

	public static string BGMFilePath => string.Format("{0}/{1}", SoundFilePath, "BGM");

	public static string SEFilePath => string.Format("{0}/{1}", SoundFilePath, "SE");

	public static string ShipVoicePath => string.Format("{0}/{1}", SoundFilePath, "Voice");
}
