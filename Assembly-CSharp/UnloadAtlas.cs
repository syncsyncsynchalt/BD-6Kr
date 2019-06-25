using System.Collections;
using UnityEngine;

public class UnloadAtlas : MonoBehaviour
{
	[SerializeField]
	private UIAtlas[] ManualDestroyAtlases;

	[SerializeField]
	private Texture[] ManualDestroyTextures;

	[Button("UnusedUnload", "UnusedUnload", new object[]
	{

	})]
	public int button;

	[Button("EmptyScene", "EmptyScene", new object[]
	{

	})]
	public int button2;

	public IEnumerator Unload()
	{
		Debug.Log("Unload Start");
		if (ManualDestroyAtlases != null)
		{
			for (int i = 0; i < ManualDestroyAtlases.Length; i++)
			{
				if (ManualDestroyAtlases[i] != null && ManualDestroyAtlases[i].texture != null)
				{
					Resources.UnloadAsset(ManualDestroyAtlases[i].spriteMaterial.mainTexture);
				}
			}
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
	}

	private void UnusedUnload()
	{
		Resources.UnloadUnusedAssets();
	}

	private void EmptyScene()
	{
		Application.LoadLevel("TestEmptyScene");
	}
}
