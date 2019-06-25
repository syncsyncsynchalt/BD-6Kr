using UnityEngine;

namespace KCV.Loading
{
	public class UILoadingShip : MonoBehaviour
	{
		[SerializeField]
		private UISprite mSpriteLoadingShip;

		[SerializeField]
		private UIRipples mRipples;

		[SerializeField]
		private Transform mLoadingText;

		private AsyncOperation mAsyncOperation;

		private bool b;

		private void Start()
		{
			StopLoadingAnimation();
		}

		private void Update()
		{
			if (mAsyncOperation != null)
			{
				Debug.Log("%" + mAsyncOperation.progress);
				if (mAsyncOperation.progress >= 0.9f && !b)
				{
					mAsyncOperation.allowSceneActivation = true;
					mAsyncOperation = null;
				}
			}
		}

		private void OnDestroy()
		{
			DebugUtils.SLog(":(");
		}

		public void LoadNextScene(Generics.Scene nextScene)
		{
			DebugUtils.SLog("LoadNextScene");
			StartLoadingAnimation();
			if (mAsyncOperation != null)
			{
				mAsyncOperation = null;
			}
			string levelName = nextScene.ToString();
			mAsyncOperation = Application.LoadLevelAsync(levelName);
			mAsyncOperation.allowSceneActivation = false;
			DebugUtils.SLog("LoadNextScene END");
		}

		private void StartLoadingAnimation()
		{
			DebugUtils.SLog("StartLoadingAnimation");
			mSpriteLoadingShip.gameObject.SetActive(true);
			mRipples.gameObject.SetActive(true);
			mLoadingText.SetActive(isActive: true);
			mSpriteLoadingShip.GetComponent<Animation>().Play("Anim_LoadingShip");
			mRipples.PlayRipple();
			DebugUtils.SLog("StartLoadingAnimation\u3000END");
		}

		private void StopLoadingAnimation()
		{
			DebugUtils.SLog("StopLoadingAnimation");
			mSpriteLoadingShip.gameObject.SetActive(false);
			mRipples.gameObject.SetActive(false);
			mLoadingText.SetActive(isActive: false);
			DebugUtils.SLog("StopLoadingAnimation\u3000END");
		}
	}
}
