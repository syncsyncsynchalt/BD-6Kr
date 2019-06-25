using DG.Tweening;
using UnityEngine;

public class UIDeckPracticeShip : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_SplayTail;

	[SerializeField]
	private UITexture mTexture_SplayTail_Delay;

	[SerializeField]
	private UITexture mTexture_SplayHead;

	private int mDefaultSplayTailWidth;

	private void Awake()
	{
		mDefaultSplayTailWidth = mTexture_SplayTail.width;
	}

	private void Start()
	{
		GenerateSplayHeadAnimation();
		GenerateLoopTailAnimation();
	}

	private Tween GenerateSplayHeadAnimation()
	{
		return mTexture_SplayHead.transform.DOScaleY(Random.RandomRange(0.5f, 0.8f), 0.25f).SetLoops(int.MaxValue).SetId(this);
	}

	private Tween GenerateLoopTailAnimation()
	{
		Tween result = GenerateSplayAnimation(mTexture_SplayTail).SetLoops(int.MaxValue, LoopType.Restart).SetId(this);
		DOVirtual.DelayedCall(1.5f, delegate
		{
			GenerateSplayAnimation(mTexture_SplayTail_Delay).SetLoops(int.MaxValue, LoopType.Restart);
		}).SetId(this);
		return result;
	}

	private Tween GenerateSplayAnimation(UITexture texture)
	{
		Tween t = DOVirtual.Float(0f, 1f, 3f, delegate(float percentage)
		{
			texture.width = (int)((float)mDefaultSplayTailWidth + 30f * percentage);
		}).SetId(this);
		Sequence t2 = DOTween.Sequence().Append(DOVirtual.Float(0f, 1f, 1.5f, delegate(float percentage)
		{
			texture.alpha = percentage;
		}).SetId(this)).Append(DOVirtual.Float(1f, 0f, 1.5f, delegate(float percentage)
		{
			texture.alpha = percentage;
		}).SetId(this))
			.SetId(this);
		Sequence sequence = DOTween.Sequence();
		sequence.Join(t);
		sequence.Join(t2);
		sequence.OnPlay(delegate
		{
			texture.alpha = 0f;
			texture.width = mDefaultSplayTailWidth;
		});
		sequence.SetId(this);
		return sequence;
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		mTexture_SplayTail = null;
		mTexture_SplayTail_Delay = null;
		mTexture_SplayHead = null;
	}
}
