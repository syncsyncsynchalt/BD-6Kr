using AnimationOrTween;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : MonoBehaviour
{
	public static ActiveAnimation current;

	public List<EventDelegate> onFinished = new List<EventDelegate>();

	[HideInInspector]
	public GameObject eventReceiver;

	[HideInInspector]
	public string callWhenFinished;

	private Animation mAnim;

	private Direction mLastDirection;

	private Direction mDisableDirection;

	private bool mNotify;

	private Animator mAnimator;

	private string mClip = string.Empty;

	private float playbackTime
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			AnimatorStateInfo currentAnimatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
			return Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime);
		}
	}

	public bool isPlaying
	{
		get
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			if ((Object)mAnim == null)
			{
				if ((Object)mAnimator != null)
				{
					if (mLastDirection == Direction.Reverse)
					{
						if (playbackTime == 0f)
						{
							return false;
						}
					}
					else if (playbackTime == 1f)
					{
						return false;
					}
					return true;
				}
				return false;
			}
			foreach (AnimationState item in mAnim)
			{
				AnimationState val = item;
				if (mAnim.IsPlaying(val.name))
				{
					if (mLastDirection == Direction.Forward)
					{
						if (val.time < val.length)
						{
							return true;
						}
					}
					else
					{
						if (mLastDirection != Direction.Reverse)
						{
							return true;
						}
						if (val.time > 0f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	public void Finish()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if ((Object)mAnim != null)
		{
			foreach (AnimationState item in mAnim)
			{
				AnimationState val = item;
				if (mLastDirection == Direction.Forward)
				{
					val.time = val.length;
				}
				else if (mLastDirection == Direction.Reverse)
				{
					val.time = 0f;
				}
			}
			mAnim.Sample();
		}
		else if ((Object)mAnimator != null)
		{
			mAnimator.Play(mClip, 0, (mLastDirection != Direction.Forward) ? 0f : 1f);
		}
	}

	public void Reset()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if ((Object)mAnim != null)
		{
			foreach (AnimationState item in mAnim)
			{
				AnimationState val = item;
				if (mLastDirection == Direction.Reverse)
				{
					val.time = val.length;
				}
				else if (mLastDirection == Direction.Forward)
				{
					val.time = 0f;
				}
			}
		}
		else if ((Object)mAnimator != null)
		{
			mAnimator.Play(mClip, 0, (mLastDirection != Direction.Reverse) ? 0f : 1f);
		}
	}

	private void Start()
	{
		if (eventReceiver != null && EventDelegate.IsValid(onFinished))
		{
			eventReceiver = null;
			callWhenFinished = null;
		}
	}

	private void Update()
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		float deltaTime = RealTime.deltaTime;
		if (deltaTime == 0f)
		{
			return;
		}
		if ((Object)mAnimator != null)
		{
			mAnimator.Update((mLastDirection != Direction.Reverse) ? deltaTime : (0f - deltaTime));
			if (isPlaying)
			{
				return;
			}
			((Behaviour)mAnimator).enabled = false;
			base.enabled = false;
		}
		else
		{
			if (!((Object)mAnim != null))
			{
				base.enabled = false;
				return;
			}
			bool flag = false;
			foreach (AnimationState item in mAnim)
			{
				AnimationState val = item;
				if (mAnim.IsPlaying(val.name))
				{
					float num = val.speed * deltaTime;
					AnimationState obj = val;
					obj.time = obj.time + num;
					if (num < 0f)
					{
						if (val.time > 0f)
						{
							flag = true;
						}
						else
						{
							val.time = 0f;
						}
					}
					else if (val.time < val.length)
					{
						flag = true;
					}
					else
					{
						val.time = val.length;
					}
				}
			}
			mAnim.Sample();
			if (flag)
			{
				return;
			}
			base.enabled = false;
		}
		if (!mNotify)
		{
			return;
		}
		mNotify = false;
		if (current == null)
		{
			current = this;
			EventDelegate.Execute(onFinished);
			if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
			{
				eventReceiver.SendMessage(callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			current = null;
		}
		if (mDisableDirection != 0 && mLastDirection == mDisableDirection)
		{
			NGUITools.SetActive(base.gameObject, state: false);
		}
	}

	private void Play(string clipName, Direction playDirection)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		if (playDirection == Direction.Toggle)
		{
			playDirection = ((mLastDirection != Direction.Forward) ? Direction.Forward : Direction.Reverse);
		}
		if ((Object)mAnim != null)
		{
			base.enabled = true;
			((Behaviour)mAnim).enabled = false;
			if (string.IsNullOrEmpty(clipName))
			{
				if (!mAnim.isPlaying)
				{
					mAnim.Play();
				}
			}
			else if (!mAnim.IsPlaying(clipName))
			{
				mAnim.Play(clipName);
			}
			foreach (AnimationState item in mAnim)
			{
				AnimationState val = item;
				if (string.IsNullOrEmpty(clipName) || val.name == clipName)
				{
					float num = Mathf.Abs(val.speed);
					val.speed = num * (float) playDirection;
					if (playDirection == Direction.Reverse && val.time == 0f)
					{
						val.time = val.length;
					}
					else if (playDirection == Direction.Forward && val.time == val.length)
					{
						val.time = 0f;
					}
				}
			}
			mLastDirection = playDirection;
			mNotify = true;
			mAnim.Sample();
		}
		else if ((Object)mAnimator != null)
		{
			if (base.enabled && isPlaying && mClip == clipName)
			{
				mLastDirection = playDirection;
				return;
			}
			base.enabled = true;
			mNotify = true;
			mLastDirection = playDirection;
			mClip = clipName;
			mAnimator.Play(mClip, 0, (playDirection != Direction.Forward) ? 1f : 0f);
		}
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!NGUITools.GetActive(((Component)anim).gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(((Component)anim).gameObject, state: true);
			UIPanel[] componentsInChildren = ((Component)anim).gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				componentsInChildren[i].Refresh();
			}
		}
		ActiveAnimation activeAnimation = ((Component)anim).GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = ((Component)anim).gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnim = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		if ((Object)activeAnimation.mAnim != null)
		{
			activeAnimation.mAnim.Sample();
		}
		else if ((Object)activeAnimation.mAnimator != null)
		{
			activeAnimation.mAnimator.Update(0f);
		}
		return activeAnimation;
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animator anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (enableBeforePlay != EnableCondition.IgnoreDisabledState && !NGUITools.GetActive(((Component)anim).gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(((Component)anim).gameObject, state: true);
			UIPanel[] componentsInChildren = ((Component)anim).gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				componentsInChildren[i].Refresh();
			}
		}
		ActiveAnimation activeAnimation = ((Component)anim).GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = ((Component)anim).gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnimator = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		if ((Object)activeAnimation.mAnim != null)
		{
			activeAnimation.mAnim.Sample();
		}
		else if ((Object)activeAnimation.mAnimator != null)
		{
			activeAnimation.mAnimator.Update(0f);
		}
		return activeAnimation;
	}
}
