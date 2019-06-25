using KCV.View;
using local.models;
using System;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemGrid : BaseUISummaryGrid<UIRevampSlotItemSummary, SlotitemModel>
	{
		public enum ActionType
		{
			Back,
			Select
		}

		public delegate void UIRevampSlotItemGridAction(ActionType actionType, UIRevampSlotItemGrid calledObject, UIRevampSlotItemSummary summary);

		private KeyControl mKeyController;

		private UIRevampSlotItemSummary mFocusSummary;

		private UIRevampSlotItemGridAction mRevampSlotItemGridActionCallBack;

		private RevampRecipeModel mRevampRecipeModel;

		[Obsolete]
		public override void Initialize(SlotitemModel[] models)
		{
		}

		public void Initialize(SlotitemModel[] models, RevampRecipeModel revampRecipe)
		{
			base.Initialize(models);
			mRevampRecipeModel = revampRecipe;
		}

		public RevampRecipeModel GetRevampRecipe()
		{
			return mRevampRecipeModel;
		}

		public void SetOnRevampSlotItemGridActionCallBack(UIRevampSlotItemGridAction callBack)
		{
			mRevampSlotItemGridActionCallBack = callBack;
		}

		private void ChangeFocusSummary(UIRevampSlotItemSummary summary)
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

		public KeyControl GetKeyController()
		{
			mKeyController = new KeyControl();
			ChangeFocusSummary(GetSummaryView(0));
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
				int num = mFocusSummary.GetIndex() - 1;
				if (0 <= num)
				{
					ChangeFocusSummary(GetSummaryView(num));
				}
			}
			else if (mKeyController.keyState[12].down)
			{
				int num2 = mFocusSummary.GetIndex() + 1;
				if (num2 < GetCurrentViewCount())
				{
					ChangeFocusSummary(GetSummaryView(num2));
				}
			}
			else if (mKeyController.keyState[14].down)
			{
				int num3 = GetCurrentPageIndex() - 1;
				if (0 <= num3)
				{
					GoToPage(num3);
					ChangeFocusSummary(GetSummaryView(0));
				}
			}
			else if (mKeyController.keyState[10].down)
			{
				int num4 = GetCurrentPageIndex() + 1;
				if (num4 < GetPageSize())
				{
					GoToPage(num4);
					ChangeFocusSummary(GetSummaryView(0));
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

		public void OnCallBack(ActionType actionType, UIRevampSlotItemSummary summary)
		{
			if (mRevampSlotItemGridActionCallBack != null)
			{
				mRevampSlotItemGridActionCallBack(actionType, this, summary);
			}
		}
	}
}
