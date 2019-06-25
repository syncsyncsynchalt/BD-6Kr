using System.Collections;
using UnityEngine;

public class UIFurnitureYousei : MonoBehaviour
{
	public enum YouseiType
	{
		Store,
		Room
	}

	[SerializeField]
	public Texture[] mTextures_RoomYouseiWalkFrames;

	[SerializeField]
	public Texture[] mTextures_StoreYouseiWalkFrames;

	[SerializeField]
	private UITexture mTexture_Yousei;

	private YouseiType mYouseiType;

	private IEnumerator mWalkCoroutine;

	public float alpha
	{
		get
		{
			return mTexture_Yousei.alpha;
		}
		set
		{
			mTexture_Yousei.alpha = value;
		}
	}

	public void Initialize(YouseiType type)
	{
		mYouseiType = type;
		mTexture_Yousei.alpha = 1f;
	}

	public void StartWalk()
	{
		if (mWalkCoroutine != null)
		{
			StopCoroutine(mWalkCoroutine);
			mWalkCoroutine = null;
		}
		mWalkCoroutine = WalkCoroutine();
		StartCoroutine(mWalkCoroutine);
	}

	public void StopWalk()
	{
		if (mWalkCoroutine != null)
		{
			StopCoroutine(mWalkCoroutine);
		}
	}

	private IEnumerator WalkCoroutine()
	{
		int frameLength = 0;
		switch (mYouseiType)
		{
		case YouseiType.Room:
			frameLength = mTextures_RoomYouseiWalkFrames.Length;
			break;
		case YouseiType.Store:
			frameLength = mTextures_StoreYouseiWalkFrames.Length;
			break;
		}
		int frameCounter = 0;
		while (true)
		{
			OnFrameChange(mYouseiType, frameCounter);
			frameCounter++;
			if (frameCounter % frameLength == 0)
			{
				frameCounter = 0;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void OnFrameChange(YouseiType youseiType, int frameCount)
	{
		switch (youseiType)
		{
		case YouseiType.Room:
			mTexture_Yousei.mainTexture = mTextures_RoomYouseiWalkFrames[frameCount];
			mTexture_Yousei.SetDimensions(242, 266);
			break;
		case YouseiType.Store:
			mTexture_Yousei.mainTexture = mTextures_StoreYouseiWalkFrames[frameCount];
			mTexture_Yousei.SetDimensions(246, 262);
			break;
		}
	}

	public void Release()
	{
		if (mWalkCoroutine != null)
		{
			StopCoroutine(mWalkCoroutine);
		}
		for (int i = 0; i < mTextures_RoomYouseiWalkFrames.Length; i++)
		{
			mTextures_RoomYouseiWalkFrames[i] = null;
		}
		for (int j = 0; j < mTextures_StoreYouseiWalkFrames.Length; j++)
		{
			mTextures_StoreYouseiWalkFrames[j] = null;
		}
		mTextures_RoomYouseiWalkFrames = null;
		mTextures_StoreYouseiWalkFrames = null;
		mTexture_Yousei = null;
		mWalkCoroutine = null;
	}
}
