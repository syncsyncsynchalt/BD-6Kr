using Common.Enum;
using DG.Tweening;
using KCV.Base;
using System;
using UnityEngine;

namespace KCV.Battle.Formation
{
	public class UIBattleFormationKind : UICircleCategory<BattleFormationKinds1>
	{
		[SerializeField]
		private UISprite mSprite_Formation;

		[SerializeField]
		private UITexture mTexture_Background_Circle;

		private Vector3 mOnCenterScale = new Vector3(1.1f, 1.1f, 1f);

		private Vector3 mOnOtherThanCenterScale = new Vector3(0.7f, 0.7f, 1f);

		private Vector3 mOnOutDisplayScale = new Vector3(0.7f, 0.7f, 1f);

		public override void Initialize(int index, BattleFormationKinds1 category)
		{
			base.Initialize(index, category);
			mSprite_Formation.spriteName = GetSpriteNameFormation(category);
			mSprite_Formation.transform.localScale = Vector3.zero;
			mSprite_Formation.alpha = 0.7f;
			mTexture_Background_Circle.alpha = 0f;
		}

		private string GetSpriteNameFormation(BattleFormationKinds1 kind)
		{
			string result = string.Empty;
			switch (kind)
			{
			case BattleFormationKinds1.FukuJuu:
				result = "jin_fukujyu";
				break;
			case BattleFormationKinds1.Rinkei:
				result = "jin_rinkei";
				break;
			case BattleFormationKinds1.TanJuu:
				result = "jin_tanjyu";
				break;
			case BattleFormationKinds1.TanOu:
				result = "jin_tanou";
				break;
			case BattleFormationKinds1.Teikei:
				result = "jin_teikei";
				break;
			}
			return result;
		}

		public override void OnCenter(float animationTime, Ease animationEase)
		{
			base.OnCenter(animationTime, animationEase);
			mSprite_Formation.transform.DOScale(mOnCenterScale, animationTime).SetEase(animationEase);
		}

		public override void OnOtherThanCenter(float animationTime, Ease easeType)
		{
			base.OnOtherThanCenter(animationTime, easeType);
			mSprite_Formation.transform.DOScale(mOnOtherThanCenterScale, animationTime).SetEase(easeType);
		}

		public override void OnOutDisplay(float animationTime, Ease easeType, Action onFinished)
		{
			base.OnOutDisplay(animationTime, easeType, onFinished);
			mSprite_Formation.transform.DOScale(mOnOutDisplayScale, animationTime).SetEase(easeType).OnComplete(delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
				mSprite_Formation.alpha = 0.5f;
			});
		}

		public override void OnInDisplay(float animationTime, Ease animationEase, Action onFinished)
		{
			base.OnInDisplay(animationTime, animationEase, onFinished);
			mSprite_Formation.alpha = 0.7f;
			mSprite_Formation.transform.DOScale(mOnOtherThanCenterScale, animationTime).SetEase(animationEase).OnComplete(delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
			});
		}

		public override void OnFirstDisplay(float animationTime, bool isCenter, Ease easeType)
		{
			base.OnFirstDisplay(animationTime, isCenter, easeType);
			if (isCenter)
			{
				mSprite_Formation.transform.DOScale(mOnCenterScale, animationTime).SetEase(easeType);
			}
			else
			{
				mSprite_Formation.transform.DOScale(mOnOtherThanCenterScale, animationTime).SetEase(easeType);
			}
		}

		public override void OnSelectAnimation(Action onAnimationFinished)
		{
			Sequence s = DOTween.Sequence();
			UISprite spriteFormation = Util.Instantiate(mSprite_Formation.gameObject, base.transform.gameObject).GetComponent<UISprite>();
			spriteFormation.transform.localScale = mSprite_Formation.transform.localScale;
			spriteFormation.transform.localPosition = mSprite_Formation.transform.localPosition;
			DOVirtual.Float(spriteFormation.alpha, 0f, 1f, delegate(float alpha)
			{
				spriteFormation.alpha = alpha;
			});
			spriteFormation.transform.DOScale(new Vector3(1.5f, 1.5f), 1f);
			mTexture_Background_Circle.transform.localScale(new Vector3(0.3f, 0.3f));
			mTexture_Background_Circle.transform.DOScale(Vector3.one, 0.3f);
			Tween t = DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
			{
				mTexture_Background_Circle.alpha = alpha;
			});
			Tween t2 = mTexture_Background_Circle.transform.DOScale(Vector3.one, 0.3f);
			t2.SetEase(Ease.OutBack);
			TweenCallback action = delegate
			{
				if (onAnimationFinished != null)
				{
					onAnimationFinished();
				}
			};
			s.Append(t).Join(t2).AppendInterval(1f)
				.OnComplete(action);
		}
	}
}
