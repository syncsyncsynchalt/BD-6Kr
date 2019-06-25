using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	public class UIBattleNavigation : UINavigation<UIBattleNavigation>
	{
		[Serializable]
		private struct Params
		{
			public float animationTime;
		}

		[Header("[Animation Properties]")]
		[SerializeField]
		private Params _strParams;

		public static UIBattleNavigation Instantiate(UIBattleNavigation prefab, Transform parent, SettingModel model)
		{
			UIBattleNavigation uIBattleNavigation = UnityEngine.Object.Instantiate(prefab);
			uIBattleNavigation.transform.parent = parent;
			uIBattleNavigation.transform.localPositionZero();
			uIBattleNavigation.transform.localScaleOne();
			return uIBattleNavigation.VirtualCtor(model);
		}

		protected override UIBattleNavigation VirtualCtor(SettingModel model)
		{
			this.SetAnchor(Anchor.BottomLeft);
			panel.alpha = 0f;
			panel.widgetsAreStatic = true;
			return base.VirtualCtor(model);
		}

		protected override void OnUnInit()
		{
		}

		public override UIBattleNavigation SetAnchor(Anchor iAnchor)
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			return base.SetAnchor(iAnchor);
		}

		public UIBattleNavigation SetNavigationInCommand(BattleCommandMode iCommandMode)
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			switch (iCommandMode)
			{
			case BattleCommandMode.SurfaceBox:
				AddDetail(HowToKey.arrow_LR, "指揮ボックス選択");
				AddDetail(HowToKey.btn_maru, "決定");
				AddDetail(HowToKey.btn_batsu, "外す");
				AddDetail(HowToKey.btn_shikaku, "一括解除");
				break;
			case BattleCommandMode.UnitList:
				AddDetail(HowToKey.arrow_UDLR, "指揮コマンド選択");
				AddDetail(HowToKey.btn_maru, "決定");
				AddDetail(HowToKey.btn_batsu, "戻る");
				break;
			}
			return DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInWithdrawalDecision(ProdWithdrawalDecisionSelection.Mode iMode)
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			switch (iMode)
			{
			case ProdWithdrawalDecisionSelection.Mode.Selection:
				AddDetail(HowToKey.arrow_LR, "選択");
				AddDetail(HowToKey.btn_maru, "決定");
				AddDetail(HowToKey.btn_sankaku, "戦況確認");
				break;
			case ProdWithdrawalDecisionSelection.Mode.TacticalSituation:
				AddDetail(HowToKey.btn_sankaku, "戻る");
				break;
			}
			return DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInResult()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.btn_maru, "次へ");
			return DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInAdvancingWithDrawal()
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

		public UIBattleNavigation SetNavigationInEscortShipEvacuation()
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

		public UIBattleNavigation SetNavigationInFlagshipWreck()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.btn_maru, "決定");
			return DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInMapOpen()
		{
			if (!settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(Anchor.BottomLeft);
			AddDetail(HowToKey.btn_maru, "次へ");
			return DrawRefresh();
		}

		public void Show()
		{
			Show(_strParams.animationTime, null);
		}

		public void Show(Action onFinished)
		{
			Show(_strParams.animationTime, onFinished);
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

		public void Hide()
		{
			Hide(_strParams.animationTime, null);
		}

		public void Hide(Action onFinished)
		{
			Hide(_strParams.animationTime, onFinished);
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
