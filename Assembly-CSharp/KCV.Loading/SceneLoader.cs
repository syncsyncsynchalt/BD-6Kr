using System.Collections;
using UnityEngine;

namespace KCV.Loading
{
	public class SceneLoader : MonoBehaviour
	{
		[SerializeField]
		private UILoadingShip loadingShip;

		[SerializeField]
		private Camera MainCamera;

		private AsyncOperation mAsyncOperation;

		private Coroutine cor;

		private void Start()
		{
			Debug.Log("SceneLoader Start");
			if (SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene == Generics.Scene.Scene_BEF)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
				Debug.LogError("エラー：次のシーンが設定されていません");
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType == AppInformation.LoadType.Ship)
			{
				loadingShip.LoadNextScene(SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene);
			}
			else if (SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType == AppInformation.LoadType.Yousei)
			{
				loadingShip.SetActive(isActive: false);
				if (!SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isYouseiExist)
				{
					SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation = true;
					SingletonMonoBehaviour<NowLoadingAnimation>.Instance.StartTextAnimation();
					SingletonMonoBehaviour<NowLoadingAnimation>.Instance.StartAnimation(Random.Range(1, 10));
				}
				LoadNextScene(SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene);
			}
			else
			{
				loadingShip.SetActive(isActive: false);
				MainCamera.backgroundColor = Color.white;
				LoadNextScene(SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene);
			}
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Scene_BEF;
		}

		private void LoadNextScene(Generics.Scene scene)
		{
			mAsyncOperation = Application.LoadLevelAsync(scene.ToString());
			mAsyncOperation.allowSceneActivation = false;
			if (cor != null)
			{
				Debug.LogError("coroutine is not null");
			}
			cor = StartCoroutine(LoadStart());
		}

		private IEnumerator LoadStart()
		{
			Debug.Log("☆☆☆☆☆☆☆☆☆☆LoadStart☆☆☆☆☆☆☆☆☆☆");
			while (mAsyncOperation.progress < 0.9f)
			{
				yield return null;
			}
			mAsyncOperation.allowSceneActivation = true;
			mAsyncOperation = null;
			if (SingletonMonoBehaviour<NowLoadingAnimation>.exist())
			{
				SingletonMonoBehaviour<NowLoadingAnimation>.Instance.EndAnimation();
			}
		}

		private void OnDestroy()
		{
			if (cor != null)
			{
				StopCoroutine(cor);
			}
			cor = null;
			loadingShip = null;
			MainCamera = null;
			mAsyncOperation = null;
		}
	}
}
