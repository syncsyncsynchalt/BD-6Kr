using System;
using System.Collections;
using UnityEngine;

namespace KCV.Loading
{
	public class UserInterfaceLoadingManager : SingletonMonoBehaviour<UserInterfaceLoadingManager>
	{
		[SerializeField]
		private UISprite mSpriteLoadingShip;

		[SerializeField]
		private UIRipples mRipples;

		[SerializeField]
		private Camera mCamera;

		private bool mSceneChanged;

		private AsyncOperation mAsyncOperation;

		private Action mParallelAction;

		private void Start()
		{
			StopLoadingAnimation();
			base.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (!mSceneChanged && mAsyncOperation != null && mAsyncOperation.progress >= 0.9f && mParallelAction == null)
			{
				mAsyncOperation.allowSceneActivation = true;
				StopLoadingAnimation();
				mSceneChanged = true;
				mCamera.depth = 0f;
				base.gameObject.SetActive(false);
			}
			else
			{
				Debug.Log("Wait Tasks");
			}
		}

		public void LoadNextScene(Generics.Scene nextScene, Action parallelAction)
		{
			mCamera.depth = 100f;
			base.gameObject.SetActive(true);
			Debug.Log(string.Empty);
			StartLoadingAnimation();
			mParallelAction = parallelAction;
			StartCoroutine(StartParallelAction());
			if (mAsyncOperation != null)
			{
				mAsyncOperation = null;
			}
			mAsyncOperation = Application.LoadLevelAsync(nextScene.ToString());
			mAsyncOperation.allowSceneActivation = false;
		}

		private void StartLoadingAnimation()
		{
			mSpriteLoadingShip.gameObject.SetActive(true);
			mRipples.gameObject.SetActive(true);
			mSpriteLoadingShip.GetComponent<Animation>().Play("Anim_LoadingShip");
			mRipples.PlayRipple();
		}

		private void StopLoadingAnimation()
		{
			mSpriteLoadingShip.gameObject.SetActive(false);
			mRipples.gameObject.SetActive(false);
		}

		private IEnumerator StartParallelAction()
		{
			if (mParallelAction != null)
			{
				yield return null;
				mParallelAction();
				mParallelAction = null;
				Debug.Log("Call:StartParallelAction");
			}
			yield return null;
		}
	}
}
