using DG.Tweening;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeUpParameter : MonoBehaviour
	{
		private int mBefore;

		private int mAfter;

		private bool mIsAleadyMax;

		[SerializeField]
		private UITexture mTexture_Left;

		[SerializeField]
		private UITexture mTexture_Right;

		[SerializeField]
		private UILabel mLabel_Before;

		[SerializeField]
		private UILabel mLabel_After;

		[SerializeField]
		private UITexture mTexture_ParameterUp;

		[SerializeField]
		private Transform mTransform_ParameterMax;

		private Vector3 mVector3_DefaultLocalPosition;

		private void Awake()
		{
			mVector3_DefaultLocalPosition = base.transform.localPosition;
		}

		public void Initialize(int before, int after, bool aleadyMax)
		{
			mBefore = before;
			mAfter = after;
			mIsAleadyMax = aleadyMax;
			TweenAlpha component = mTexture_Left.gameObject.GetComponent<TweenAlpha>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			TweenAlpha component2 = mTexture_Right.gameObject.GetComponent<TweenAlpha>();
			if (component2 != null)
			{
				Object.Destroy(component2);
			}
			mLabel_Before.alpha = 1E-07f;
			mLabel_After.alpha = 1E-07f;
			mTexture_Left.alpha = 1E-07f;
			mTexture_Right.alpha = 1E-07f;
			mLabel_Before.text = mBefore.ToString();
			mLabel_After.text = mAfter.ToString();
			if (mBefore == mAfter)
			{
				mLabel_After.alpha = 1E-06f;
				mTexture_ParameterUp.alpha = 1E-07f;
			}
			mTransform_ParameterMax.SetActive(isActive: false);
		}

		public Tween GenerateParameterUpAnimation(float duration)
		{
			Sequence sequence = DOTween.Sequence();
			bool flag = mAfter != mBefore;
			Tween t = DOVirtual.Float(0f, 1f, duration, delegate(float alpha)
			{
				mLabel_Before.alpha = alpha;
				if (mAfter != mBefore)
				{
					mLabel_After.alpha = alpha;
				}
			});
			sequence.OnPlay(GenerateArrowTween());
			sequence.AppendCallback(GenerateArrowAlphaTweenCallBack(flag, mIsAleadyMax));
			sequence.Append(t);
			if (flag)
			{
				Tween t2 = GenerateTweenParameterUp();
				sequence.Join(t2);
			}
			return sequence;
		}

		private TweenCallback GenerateArrowAlphaTweenCallBack(bool isParamUp, bool aleadyMax)
		{
			if (aleadyMax)
			{
				return delegate
				{
					if (mIsAleadyMax)
					{
						mTransform_ParameterMax.SetActive(isActive: true);
					}
					mTexture_Left.color = Color.clear;
					mTexture_Right.color = Color.clear;
				};
			}
			if (isParamUp)
			{
				return delegate
				{
					if (mIsAleadyMax)
					{
						mTransform_ParameterMax.SetActive(isActive: true);
					}
					mTexture_Left.color = Color.white;
					mTexture_Right.color = Color.white;
				};
			}
			return delegate
			{
				if (mIsAleadyMax)
				{
					mTransform_ParameterMax.SetActive(isActive: true);
				}
				mTexture_Left.color = new Color(0.266f, 0.266f, 0.266f, 0.13f);
				mTexture_Right.color = new Color(0.266f, 0.266f, 0.266f, 0.13f);
			};
		}

		private TweenCallback GenerateArrowTween()
		{
			return delegate
			{
				TweenAlpha component = mTexture_Left.gameObject.GetComponent<TweenAlpha>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				TweenAlpha component2 = mTexture_Right.gameObject.GetComponent<TweenAlpha>();
				if (component2 != null)
				{
					Object.Destroy(component2);
				}
				if (mAfter != mBefore)
				{
					mTexture_Left.color = Color.white;
					mTexture_Right.color = Color.white;
					TweenAlpha tweenAlpha = TweenAlpha.Begin(mTexture_Left.gameObject, 1f, 0f);
					tweenAlpha.method = UITweener.Method.EaseInOut;
					tweenAlpha.style = UITweener.Style.PingPong;
					TweenAlpha tweenAlpha2 = TweenAlpha.Begin(mTexture_Right.gameObject, 1f, 0f);
					tweenAlpha2.method = UITweener.Method.EaseInOut;
					tweenAlpha2.style = UITweener.Style.PingPong;
					tweenAlpha2.delay = 0.5f;
				}
			};
		}

		public Tween GenerateSlotInAnimation()
		{
			return base.transform.DOLocalMoveX(mVector3_DefaultLocalPosition.x - 296f, 0.15f).SetDelay(0.075f).SetEase(Ease.OutCubic);
		}

		public Tween GenerateSlotOutAnimation()
		{
			return base.transform.DOLocalMoveX(mVector3_DefaultLocalPosition.x, 0.15f).SetDelay(0.075f).SetEase(Ease.OutCubic);
		}

		private Tween GenerateTweenParameterUp()
		{
			float y = 0f;
			float endValue = 20f;
			float duration = 1.5f;
			float delay = 0.5f;
			Sequence sequence = DOTween.Sequence().SetId(this);
			mTexture_ParameterUp.alpha = 0f;
			mTexture_ParameterUp.transform.localPositionY(y);
			Tween t = DOVirtual.Float(1f, 0f, duration, delegate(float alpha)
			{
				mTexture_ParameterUp.alpha = alpha;
			}).SetDelay(delay).SetId(this);
			sequence.Append(mTexture_ParameterUp.transform.DOLocalMoveY(endValue, duration).SetId(this));
			sequence.Join(t);
			sequence.OnPlay(delegate
			{
				mTexture_ParameterUp.alpha = 1f;
			});
			return sequence;
		}

		public void Reposition()
		{
			base.transform.localPosition = mVector3_DefaultLocalPosition;
		}

		private void OnDestroy()
		{
			mTexture_Left = null;
			mTexture_Right = null;
			mLabel_Before = null;
			mLabel_After = null;
			mTexture_ParameterUp = null;
		}
	}
}
