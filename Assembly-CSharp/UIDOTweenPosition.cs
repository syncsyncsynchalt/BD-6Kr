using DG.Tweening;
using UnityEngine;

public class UIDOTweenPosition : MonoBehaviour
{
	[SerializeField]
	private Vector3 mVector3_From;

	[SerializeField]
	private Vector3 mVector3_To;

	[SerializeField]
	private LoopType mLoopType_Type;

	[SerializeField]
	[Tooltip("Minus is Infinite")]
	private int mLoop;

	[SerializeField]
	private Ease mEase;

	[SerializeField]
	private float mDuration;

	[SerializeField]
	private float mDelay;

	private Tween mTween;

	private void Start()
	{
		if (mLoop < 0)
		{
			mLoop = int.MinValue;
		}
		mTween = base.transform.DOLocalMove(mVector3_To, mDuration).SetLoops(mLoop, mLoopType_Type).SetDelay(mDelay)
			.SetEase(mEase);
	}
}
