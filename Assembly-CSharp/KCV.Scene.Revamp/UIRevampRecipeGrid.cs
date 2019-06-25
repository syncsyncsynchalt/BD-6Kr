using KCV.View;
using local.models;
using System;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeGrid : BaseUISummaryGrid<UIRevampRecipeSummary, RevampRecipeModel>
	{
		public enum ActionType
		{
			Back,
			Select
		}

		public delegate bool UIRevampRecipeGridCheckDelegate(UIRevampRecipeSummary summary);

		public delegate void UIRevampRecipeGridAction(ActionType actionType, UIRevampRecipeGrid calledObject, UIRevampRecipeSummary summary);

		private UIRevampRecipeGridCheckDelegate mRevampSummarySelectableCheckDelegate;

		private UIRevampRecipeGridAction mRevampSummaryActionCallBack;

		private KeyControl mKeyController;

		private UIRevampRecipeSummary mFocusSummary;

		[Obsolete]
		public override void Initialize(RevampRecipeModel[] models)
		{
		}

		public void Initialize(RevampRecipeModel[] models, UIRevampRecipeGridCheckDelegate summarySelectableCheckDelegate)
		{
			mRevampSummarySelectableCheckDelegate = summarySelectableCheckDelegate;
			base.Initialize(models);
		}

		public void SetOnRevampGridActionDelegate(UIRevampRecipeGridAction gridActionDelegate)
		{
			mRevampSummaryActionCallBack = gridActionDelegate;
		}

		public override void OnFinishedCreateViews()
		{
			UIRevampRecipeSummary[] summaryViews = GetSummaryViews();
			foreach (UIRevampRecipeSummary uIRevampRecipeSummary in summaryViews)
			{
				if (!mRevampSummarySelectableCheckDelegate(uIRevampRecipeSummary))
				{
					uIRevampRecipeSummary.OnDisabled();
				}
				iTween.MoveTo(uIRevampRecipeSummary.gameObject, iTween.Hash("x", 20f, "isLocal", true, "time", 0.3f, "delay", (float)uIRevampRecipeSummary.GetIndex() * 0.1f));
				TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(uIRevampRecipeSummary.gameObject, 0.3f);
				tweenAlpha.from = 0.1f;
				tweenAlpha.to = 1f;
				tweenAlpha.delay = (float)uIRevampRecipeSummary.GetIndex() * 0.1f;
				tweenAlpha.PlayForward();
			}
		}

		public KeyControl GetKeyController(int firstFocusIndex)
		{
			mKeyController = new KeyControl();
			ChangeFocusSummary(GetSummaryView(firstFocusIndex));
			return mKeyController;
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[8].down)
			{
				if (0 <= mFocusSummary.GetIndex() - 1)
				{
					UIRevampRecipeSummary summaryView = GetSummaryView(mFocusSummary.GetIndex() - 1);
					ChangeFocusSummary(summaryView);
				}
			}
			else if (mKeyController.keyState[12].down)
			{
				if (mFocusSummary.GetIndex() + 1 < GetCurrentViewCount())
				{
					UIRevampRecipeSummary summaryView2 = GetSummaryView(mFocusSummary.GetIndex() + 1);
					ChangeFocusSummary(summaryView2);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				OnCallBack(ActionType.Select, mFocusSummary);
			}
			else if (mKeyController.keyState[0].down)
			{
				OnCallBack(ActionType.Back, mFocusSummary);
			}
		}

		private void RecipeSelectAnimation(UIRevampRecipeSummary selectedSummary)
		{
			switch (selectedSummary.GetIndex())
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			}
		}

		private void OnCallBack(ActionType actionType, UIRevampRecipeSummary summary)
		{
			if (mRevampSummaryActionCallBack != null)
			{
				mRevampSummaryActionCallBack(actionType, this, summary);
			}
		}

		private void ChangeFocusSummary(UIRevampRecipeSummary summary)
		{
			if (mFocusSummary != null)
			{
				mFocusSummary.RemoveHover();
			}
			mFocusSummary = summary;
			if (mFocusSummary != null)
			{
				mFocusSummary.Hover();
			}
		}
	}
}
