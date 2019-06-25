using DG.Tweening;
using KCV.Furniture;
using KCV.Scene.Port;
using System.Collections;
using UnityEngine;

public class UIDynamicDeskFurnitureShootingGallery : UIDynamicFurniture
{
	private const float RELOCATION_INTERVAL_SECONDS = 2f;

	[SerializeField]
	private UISprite mSprite;

	[SerializeField]
	private UISpriteAnimation mSpriteAnimation_Cat;

	private bool mIsWaitingRelocation;

	protected override void OnAwake()
	{
		mSpriteAnimation_Cat.Pause();
		mSpriteAnimation_Cat.enabled = false;
		mSpriteAnimation_Cat.framesPerSecond = 0;
		mSpriteAnimation_Cat.SetOnFinishedAnimationListener(OnFinishedAnimation);
	}

	private void OnFinishedAnimation()
	{
		mSprite.alpha = 1E-05f;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	protected override void OnCalledActionEvent()
	{
		if (!mSpriteAnimation_Cat.isPlaying && !mIsWaitingRelocation)
		{
			mSpriteAnimation_Cat.framesPerSecond = 12;
			mSpriteAnimation_Cat.enabled = true;
			mSpriteAnimation_Cat.ResetToBeginning();
			mSpriteAnimation_Cat.Play();
			IEnumerator routine = RelocationIntervalCoroutine();
			StartCoroutine(routine);
		}
	}

	private IEnumerator RelocationIntervalCoroutine()
	{
		mIsWaitingRelocation = true;
		yield return new WaitForSeconds(2f);
		mSpriteAnimation_Cat.ResetToBeginning();
		mSpriteAnimation_Cat.Pause();
		Tween tween = DOVirtual.Float(mSprite.alpha, 1f, 1f, delegate(float alpha)
		{
			this.mSprite.alpha = alpha;
		});
		yield return tween.WaitForCompletion();
		mIsWaitingRelocation = false;
	}

	protected override void OnDestroyEvent()
	{
		base.OnDestroyEvent();
		UserInterfacePortManager.ReleaseUtils.Release(ref mSprite);
		mSpriteAnimation_Cat = null;
	}
}
