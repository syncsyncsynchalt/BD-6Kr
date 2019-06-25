using local.models;
using LT.Tweening;
using System;

namespace KCV.BattleCut
{
	public class UIBattleCutNavigation : UINavigation<UIBattleCutNavigation>
	{
		public UIBattleCutNavigation Startup(SettingModel model)
		{
			this.SetAnchor(Anchor.BottomLeft);
			panel.alpha = 0f;
			panel.widgetsAreStatic = true;
			return base.VirtualCtor(model);
		}

		public override UIBattleCutNavigation SetAnchor(Anchor iAnchor)
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			return base.SetAnchor(iAnchor);
		}

		public UIBattleCutNavigation SetNavigationInFormation()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.arrow_upDown, "選択");
			AddDetail(HowToKey.btn_maru, "決定");
			return DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInCommand(CtrlBCCommandSelect.CtrlMode iMode)
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			switch (iMode)
			{
			case CtrlBCCommandSelect.CtrlMode.Command:
				AddDetail(HowToKey.arrow_upDown, "選択");
				AddDetail(HowToKey.btn_maru, "決定");
				AddDetail(HowToKey.btn_batsu, "戻る");
				break;
			case CtrlBCCommandSelect.CtrlMode.Surface:
				AddDetail(HowToKey.arrow_upDown, "選択");
				AddDetail(HowToKey.btn_maru, "決定");
				AddDetail(HowToKey.btn_batsu, "外す");
				AddDetail(HowToKey.btn_shikaku, "一括解除");
				break;
			}
			return DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInWithdrawalDecision()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.arrow_LR, "選択");
			AddDetail(HowToKey.btn_maru, "決定");
			AddDetail(HowToKey.btn_batsu, "戻る");
			return DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInResult()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.btn_maru, "次へ");
			return DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInAdvancingWithdrawal()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.arrow_LR, "選択");
			AddDetail(HowToKey.btn_maru, "決定");
			return DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInFlagshipWreck()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.btn_maru, "決定");
			return DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInEscortShipEvacuation()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.arrow_LR, "選択");
			AddDetail(HowToKey.btn_maru, "決定");
			return DrawRefresh();
		}

		public void Show(float fTime, Action onFinished)
		{
			if (settingModel.GuideDisplay)
			{
				panel.widgetsAreStatic = false;
				base.transform.LTCancel();
				base.transform.LTValue(panel.alpha, 1f, fTime).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
				{
					panel.alpha = x;
				})
					.setOnComplete((Action)delegate
					{
						Dlg.Call(ref onFinished);
					});
			}
		}

		public void Hide(float fTime, Action onFinished)
		{
			if (settingModel.GuideDisplay)
			{
				base.transform.LTCancel();
				base.transform.LTValue(panel.alpha, 0f, fTime).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
				{
					panel.alpha = x;
				})
					.setOnComplete((Action)delegate
					{
						Dlg.Call(ref onFinished);
						panel.widgetsAreStatic = true;
					});
			}
		}
	}
}
