using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Startup
{
	public class UIStartupNavigation : UINavigation<UIStartupNavigation>
	{
		[SerializeField]
		private struct Params
		{
			public float animationTime;
		}

		[SerializeField]
		[Header("Animarion Parameter")]
		private Params _strParams;

		public void Startup(bool isInherit, SettingModel model)
		{
			VirtualCtor(isInherit, model);
		}

		protected UIStartupNavigation VirtualCtor(bool isInherit, SettingModel model)
		{
			base.VirtualCtor(model);
			SetNavigationInAdmiralInfo(isInherit);
			panel.alpha = ((!settingModel.GuideDisplay) ? 0f : 1f);
			panel.widgetsAreStatic = true;
			return this;
		}

		public override UIStartupNavigation SetAnchor(Anchor iAnchor)
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			return base.SetAnchor(iAnchor);
		}

		public UIStartupNavigation SetNavigationInAdmiralInfo(bool isInherit)
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			if (isInherit)
			{
				AddDetail(HowToKey.btn_start, "決定");
			}
			else
			{
				AddDetail(HowToKey.btn_start, "決定");
				AddDetail(HowToKey.btn_batsu, "タイトルに戻る");
			}
			DrawRefresh();
			return this;
		}

		public UIStartupNavigation SetNavigationInStarterSelect()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.arrow_LR, "選択");
			AddDetail(HowToKey.btn_maru, "決定");
			AddDetail(HowToKey.btn_batsu, "戻る");
			DrawRefresh();
			return this;
		}

		public UIStartupNavigation SetNavigationInPartnerSelect()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.arrow_LR, "選択");
			AddDetail(HowToKey.btn_maru, "決定");
			AddDetail(HowToKey.btn_batsu, "戻る");
			DrawRefresh();
			return this;
		}

		public void Show(Action onFinished)
		{
			PreparaAnimation(isFoward: true, onFinished);
		}

		public void Hide(Action onFinished)
		{
			PreparaAnimation(isFoward: false, onFinished);
		}

		private void PreparaAnimation(bool isFoward, Action onFinished)
		{
			if (settingModel.GuideDisplay)
			{
				float to = (!isFoward) ? 0f : 1f;
				base.transform.LTValue(panel.alpha, to, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					panel.alpha = x;
				})
					.setOnComplete((Action)delegate
					{
						Dlg.Call(ref onFinished);
					});
			}
		}
	}
}
