using KCV.Scene.Port;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemYousei : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Yousei;

		[SerializeField]
		private Texture[] mTextures_Frame;

		private IEnumerator mAnimationCoroutine;

		private void Awake()
		{
			mAnimationCoroutine = InitializeAnimationCoroutine();
		}

		private IEnumerator InitializeAnimationCoroutine()
		{
			while (true)
			{
				if (Random.Range(0, 100) < 50)
				{
					if (Random.Range(0, 100) < 50)
					{
						yield return StartCoroutine(DoubleBlink());
					}
					else
					{
						yield return StartCoroutine(Blink());
					}
				}
				yield return new WaitForSeconds(Random.Range(0.5f, 3.5f));
			}
		}

		private void OnEnable()
		{
			if (mAnimationCoroutine != null)
			{
				StartCoroutine(mAnimationCoroutine);
			}
		}

		private void OnDisable()
		{
			if (mAnimationCoroutine != null)
			{
				StopCoroutine(mAnimationCoroutine);
			}
		}

		private IEnumerator Blink()
		{
			mTexture_Yousei.mainTexture = mTextures_Frame[0];
			yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
			mTexture_Yousei.mainTexture = mTextures_Frame[1];
			yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
			mTexture_Yousei.mainTexture = mTextures_Frame[0];
		}

		private IEnumerator DoubleBlink()
		{
			yield return Blink();
			yield return Blink();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Frame);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Yousei);
			mAnimationCoroutine = null;
		}
	}
}
