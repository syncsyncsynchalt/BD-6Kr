using KCV.Furniture;
using UnityEngine;

public class UIDynamicDeskFurnitureColdWaterBath : UIDynamicFurniture
{
	[SerializeField]
	private UISpriteAnimation mSpriteAnimation_Kouhyoteki;

	protected override void OnAwake()
	{
		mSpriteAnimation_Kouhyoteki.Pause();
		mSpriteAnimation_Kouhyoteki.enabled = false;
		mSpriteAnimation_Kouhyoteki.framesPerSecond = 0;
	}

	protected override void OnCalledActionEvent()
	{
		if (!mSpriteAnimation_Kouhyoteki.isPlaying)
		{
			mSpriteAnimation_Kouhyoteki.framesPerSecond = 6;
			mSpriteAnimation_Kouhyoteki.enabled = true;
			mSpriteAnimation_Kouhyoteki.ResetToBeginning();
			mSpriteAnimation_Kouhyoteki.Play();
		}
	}

	protected override void OnDestroyEvent()
	{
		base.OnDestroyEvent();
		mSpriteAnimation_Kouhyoteki = null;
	}
}
