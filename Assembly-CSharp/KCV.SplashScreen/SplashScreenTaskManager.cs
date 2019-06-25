using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SplashScreen
{
	public class SplashScreenTaskManager : MonoBehaviour
	{
		[SerializeField]
		private TweenAlpha _taTexture;

		private AsyncOperation _asyncOperation;

		private void Start()
		{
			Application.LoadLevel(Generics.Scene.Title.ToString());
		}

		private void OnDestroy()
		{
			Mem.Del(ref _taTexture);
		}

		private IEnumerator LoadNextTitle(IObserver<bool> observer)
		{
			_asyncOperation = Application.LoadLevelAsync(Generics.Scene.Title.ToString());
			_asyncOperation.allowSceneActivation = false;
			while (_asyncOperation.progress != 0.9f)
			{
				observer.OnNext(value: false);
				yield return new WaitForEndOfFrame();
			}
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		public void OnFinished()
		{
			_asyncOperation.allowSceneActivation = true;
		}
	}
}
