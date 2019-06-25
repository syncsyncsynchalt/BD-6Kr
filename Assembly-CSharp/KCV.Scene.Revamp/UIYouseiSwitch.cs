using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIYouseiSwitch : MonoBehaviour
	{
		public enum ActionType
		{
			OFF,
			ON
		}

		public delegate void UIYouseiSwitchAction(ActionType actionType);

		private UIYouseiSwitchAction mYouseiSwitchActionCallBack;

		[SerializeField]
		private UISprite mSprite_Background;

		[SerializeField]
		private UISprite mSprite_Thumb;

		[SerializeField]
		private UISpriteAnimation mSpriteAnimation_Yousei;

		[SerializeField]
		private ParticleSystem mSleepParticle;

		[SerializeField]
		public bool Enabled = true;

		private bool mSwitchFlag;

		private int THUMB_POS_X_OFF;

		private int THUMB_POS_X_ON = 128;

		private Vector3 SWITCH_ON_YOUSEI_HIDE_POS = new Vector3(-50f, -45f, 0f);

		private Vector3 SWITCH_ON_YOUSEI_STAND_POS = new Vector3(-50f, -20f, 0f);

		private Vector3 SWITCH_OFF_YOUSEI_STAND_POS = new Vector3(46f, -59f, 0f);

		private Vector3 SWITCH_OFF_YOUSEI_HIDE_POS = new Vector3(46f, -85f, 0f);

		private IEnumerator mAnimationCoroutine;

		public void SetYouseiSwitchActionCallBack(UIYouseiSwitchAction callBack)
		{
			mYouseiSwitchActionCallBack = callBack;
		}

		private void Start()
		{
			((Component)mSleepParticle).SetActive(isActive: true);
			mSleepParticle.Play();
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.T))
			{
				ClickSwitch();
			}
		}

		public void ClickSwitch()
		{
			if (Enabled)
			{
				OnClickSwitch();
			}
		}

		private void OnClickSwitch()
		{
			if (mAnimationCoroutine == null)
			{
				if (mSwitchFlag)
				{
					((Component)mSleepParticle).SetActive(isActive: true);
					mSleepParticle.Play();
					ClickSwitchOff();
				}
				else
				{
					mSleepParticle.Stop();
					((Component)mSleepParticle).SetActive(isActive: false);
					ClickSwitchOn();
				}
				mSwitchFlag = !mSwitchFlag;
				if (mSwitchFlag)
				{
					OnCallBack(ActionType.ON);
				}
				else
				{
					OnCallBack(ActionType.OFF);
				}
			}
		}

		private void OnCallBack(ActionType actionType)
		{
			if (mYouseiSwitchActionCallBack != null)
			{
				mYouseiSwitchActionCallBack(actionType);
			}
		}

		private void ClickSwitchOn()
		{
			mSpriteAnimation_Yousei.gameObject.transform.localPosition = SWITCH_ON_YOUSEI_HIDE_POS;
			mSpriteAnimation_Yousei.namePrefix = "mini_05_b_0";
			mSpriteAnimation_Yousei.framesPerSecond = 2;
			if (mAnimationCoroutine != null)
			{
				StopCoroutine(mAnimationCoroutine);
			}
			mAnimationCoroutine = ClickSwitchOnCoroutine(delegate
			{
				mAnimationCoroutine = null;
			});
			StartCoroutine(mAnimationCoroutine);
		}

		private void ClickSwitchOff()
		{
			if (mAnimationCoroutine != null)
			{
				StopCoroutine(mAnimationCoroutine);
			}
			mAnimationCoroutine = ClickSwitchOffCoroutine(delegate
			{
				mSpriteAnimation_Yousei.namePrefix = "mini_05_a_0";
				mSpriteAnimation_Yousei.framesPerSecond = 1;
				mAnimationCoroutine = null;
			});
			StartCoroutine(mAnimationCoroutine);
		}

		private IEnumerator ClickSwitchOnCoroutine(Action finished)
		{
			mSpriteAnimation_Yousei.transform.DOKill();
			mSprite_Thumb.transform.DOKill();
			mSpriteAnimation_Yousei.gameObject.transform.localPosition = SWITCH_ON_YOUSEI_HIDE_POS;
			Sequence sequence = DOTween.Sequence().SetId(this);
			sequence.Append(mSpriteAnimation_Yousei.transform.DOLocalMoveY(SWITCH_ON_YOUSEI_STAND_POS.y, 1f).SetEase(Ease.OutElastic));
			sequence.Join(mSprite_Thumb.transform.DOLocalMoveX(THUMB_POS_X_ON, 0.3f).OnComplete(delegate
			{
			}));
			mSprite_Background.spriteName = "switch_kenzo_on";
			mSprite_Thumb.spriteName = "switch_pin_on";
			sequence.OnComplete(delegate
			{
				if (finished != null)
				{
					finished();
				}
			});
			yield return null;
		}

		private IEnumerator ClickSwitchOffCoroutine(Action finished)
		{
			mSpriteAnimation_Yousei.transform.DOKill();
			mSprite_Thumb.transform.DOKill();
			Sequence sequence = DOTween.Sequence().SetId(this);
			mSpriteAnimation_Yousei.gameObject.transform.localPosition = SWITCH_OFF_YOUSEI_HIDE_POS;
			sequence.Append(mSpriteAnimation_Yousei.transform.DOLocalMoveY(SWITCH_OFF_YOUSEI_STAND_POS.y, 0.3f));
			sequence.Join(mSprite_Thumb.transform.DOLocalMoveX(THUMB_POS_X_OFF, 0.3f));
			mSprite_Background.spriteName = "switch_kenzo_off";
			mSprite_Thumb.spriteName = "switch_pin_off";
			sequence.OnComplete(delegate
			{
				if (finished != null && finished != null)
				{
					finished();
				}
			});
			yield return null;
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mYouseiSwitchActionCallBack = null;
			mSprite_Background = null;
			mSprite_Thumb = null;
			mSpriteAnimation_Yousei = null;
			mSleepParticle = null;
		}
	}
}
