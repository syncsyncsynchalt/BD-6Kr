using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class BtlCut_UICircleHPGauge : BaseAnimation
	{
		private UITexture _uiBackground;

		private UITexture _uiForeground;

		private UILabel _uiHPLabel;

		private UIPanel _uiPanel;

		private int _nFromHP;

		private int _nToHP;

		private int _nMaxHP;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		protected override void Awake()
		{
			Util.FindParentToChild(ref _uiBackground, base.transform, "Background");
			Util.FindParentToChild(ref _uiForeground, base.transform, "Foreground");
			Util.FindParentToChild(ref _uiHPLabel, base.transform, "Hp");
			_uiForeground.type = UIBasicSprite.Type.Filled;
			_uiForeground.fillAmount = 0f;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_uiBackground = null;
			_uiForeground = null;
			_uiHPLabel = null;
		}

		public void SetHPGauge(int maxHP, int beforeHP, int afterHP)
		{
			SetHPGauge(maxHP, beforeHP, afterHP, isChangeColor: true);
		}

		public void SetHPGauge(int maxHP, int beforeHP, int afterHP, bool isChangeColor)
		{
			_uiHPLabel.textInt = beforeHP;
			_nMaxHP = maxHP;
			_nFromHP = beforeHP;
			_nToHP = afterHP;
			_uiHPLabel.textInt = beforeHP;
			_uiForeground.fillAmount = Mathe.Rate(0f, _nMaxHP, beforeHP);
			if (isChangeColor)
			{
				_uiForeground.color = Util.HpGaugeColor2(_nMaxHP, beforeHP);
			}
		}

		public override void Play(Action callback)
		{
			_actCallback = callback;
			base.transform.LTValue(_nFromHP, _nToHP, 0.45f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int num = (int)Math.Floor(x);
				_uiHPLabel.textInt = num;
				_uiForeground.fillAmount = Mathe.Rate(0f, _nMaxHP, x);
				_uiForeground.color = Util.HpGaugeColor2(_nMaxHP, num);
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref callback);
				});
		}

		public LTDescr PlayNonColor()
		{
			return base.transform.LTValue(_nFromHP, _nToHP, 1f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int textInt = (int)Math.Floor(x);
				_uiHPLabel.textInt = textInt;
				_uiForeground.fillAmount = Mathe.Rate(0f, _nMaxHP, x);
			});
		}

		public LTDescr Show(float duration)
		{
			return LeanTween.value(base.gameObject, 0f, 1f, duration).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		public LTDescr Hide(float duration)
		{
			return LeanTween.value(base.gameObject, 1f, 0f, duration).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}
	}
}
