using DG.Tweening;
using System;
using UnityEngine;

public class UINumberCounter
{
	public enum AnimationType
	{
		Random,
		RandomRange,
		Count
	}

	private Action mOnFinishedCallBack;

	private UILabel mLabel_Target;

	private int mFrom;

	private int mTo;

	private float mDurationSeconds;

	private AnimationType mAnimationType = AnimationType.Count;

	public UINumberCounter(UILabel targetLabel)
	{
		SetFrom(0);
		SetTo(0);
		SetOnFinishedCallBack(null);
		SetDuration(0f);
		SetLabel(targetLabel);
	}

	public UINumberCounter SetLabel(UILabel targetLabel)
	{
		mLabel_Target = targetLabel;
		return this;
	}

	public UINumberCounter SetFrom(int from)
	{
		mFrom = from;
		return this;
	}

	public UINumberCounter SetTo(int to)
	{
		mTo = to;
		return this;
	}

	public UINumberCounter SetDuration(float duration)
	{
		mDurationSeconds = duration;
		return this;
	}

	public UINumberCounter SetAnimationType(AnimationType type)
	{
		mAnimationType = type;
		return this;
	}

	public UINumberCounter SetOnFinishedCallBack(Action onFinishedCallBack)
	{
		mOnFinishedCallBack = onFinishedCallBack;
		return this;
	}

	private void OnFinishedCallBack()
	{
		if (mOnFinishedCallBack != null)
		{
			mOnFinishedCallBack();
		}
	}

	public Tween Buld()
	{
		Tween result = null;
		switch (mAnimationType)
		{
		case AnimationType.Count:
			result = PlayCount();
			result.SetId(this);
			return result;
		case AnimationType.Random:
			result = PlayWithRandom();
			result.SetId(this);
			return result;
		case AnimationType.RandomRange:
			result = PlayWithRandomRange();
			result.SetId(this);
			return result;
		default:
			return result;
		}
	}

	public Tween PlayWithRandom()
	{
		int digit = mTo.ToString().Length;
		return DOVirtual.Float(mFrom, mTo, mDurationSeconds, delegate
		{
			string text = string.Empty;
			for (int i = 0; i < digit; i++)
			{
				text += UnityEngine.Random.Range(0, 9).ToString();
			}
			mLabel_Target.text = text;
		}).OnComplete(delegate
		{
			mLabel_Target.text = mTo.ToString();
			OnFinishedCallBack();
		}).SetEase(Ease.Linear);
	}

	public Tween PlayWithRandomRange()
	{
		return DOVirtual.Float(mFrom, mTo, mDurationSeconds, delegate
		{
			mLabel_Target.text = UnityEngine.Random.Range(mFrom, mTo).ToString();
		}).OnComplete(delegate
		{
			mLabel_Target.text = mTo.ToString();
			OnFinishedCallBack();
		}).SetEase(Ease.Linear);
	}

	public Tween PlayCount()
	{
		return DOVirtual.Float(mFrom, mTo, mDurationSeconds, delegate(float currentValue)
		{
			mLabel_Target.text = ((int)currentValue).ToString();
		}).OnComplete(delegate
		{
			OnFinishedCallBack();
		}).SetEase(Ease.Linear);
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
	}
}
