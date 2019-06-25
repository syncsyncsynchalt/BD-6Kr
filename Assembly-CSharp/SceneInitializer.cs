using KCV;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
	public static string strReturnScene;

	public string InitSceneName;

	private void Awake()
	{
		DebugUtils.SLog("SceneInitializer Awake START");
		if (AppInitializeManager.IsInitialize)
		{
			strReturnScene = null;
			Object.Destroy(base.gameObject);
			return;
		}
		strReturnScene = string.Copy(Application.loadedLevelName);
		if (InitSceneName != string.Empty)
		{
			Application.LoadLevel(InitSceneName);
		}
		DebugUtils.SLog("SceneInitializer Awake END");
	}

	private void Start()
	{
	}
}
