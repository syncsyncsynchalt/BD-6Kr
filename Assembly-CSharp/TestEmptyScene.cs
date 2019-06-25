using System.Collections;
using UnityEngine;

public class TestEmptyScene : MonoBehaviour
{
	private AsyncOperation async;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.X))
		{
			Resources.UnloadUnusedAssets();
		}
		else if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Z))
		{
			Application.LoadLevel(Generics.Scene.Strategy.ToString());
		}
		else if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.C))
		{
			Application.LoadLevel(1);
		}
		if (async != null)
		{
			Debug.Log(async.progress);
			if (async.progress >= 0.9f && (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.V)))
			{
				async.allowSceneActivation = true;
			}
		}
	}

	private IEnumerator LoadScene(AsyncOperation async)
	{
		async.allowSceneActivation = false;
		while (async.progress < 0.9f)
		{
			yield return null;
		}
		async.allowSceneActivation = true;
	}
}
