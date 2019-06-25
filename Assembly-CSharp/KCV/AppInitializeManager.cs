using UnityEngine;

namespace KCV
{
	public class AppInitializeManager : MonoBehaviour
	{
		public static bool IsInitialize;

		public static void Awake()
		{
			Debug.Log("AppInitializeManager Start");
			if (!IsInitialize)
			{
				App.InitLoadMasterDataManager();
				int num = 0;
				while (!App.isMasterInit)
				{
					num++;
				}
				App.Initialize();
				IsInitialize = true;
				Debug.Log("AppInitializeManager Start END");
				if (SceneInitializer.strReturnScene != null)
				{
					Application.LoadLevel(SceneInitializer.strReturnScene);
				}
			}
		}
	}
}
