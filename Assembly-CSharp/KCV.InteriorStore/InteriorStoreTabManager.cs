using Common.Enum;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreTabManager : MonoBehaviour
	{
		public enum CategoryName
		{
			Wall,
			Floor,
			Chair,
			Window,
			Decoration,
			Interior
		}

		[SerializeField]
		private OnClickEventSender mOnClickEventSender_Determine;

		private UIButtonManager btnMng;

		private CategoryName nowCategory;

		private Action OnSelectAction;

		private Action OnDesideAction;

		public FurnitureKinds GetCurrentCategory()
		{
			switch (nowCategory)
			{
			case CategoryName.Wall:
				return FurnitureKinds.Wall;
			case CategoryName.Floor:
				return FurnitureKinds.Floor;
			case CategoryName.Chair:
				return FurnitureKinds.Desk;
			case CategoryName.Window:
				return FurnitureKinds.Window;
			case CategoryName.Decoration:
				return FurnitureKinds.Hangings;
			case CategoryName.Interior:
				return FurnitureKinds.Chest;
			default:
				throw new Exception("家具選択が不正");
			}
		}

		public void StartState()
		{
			mOnClickEventSender_Determine.SetClickable(clickable: true);
			InitTab();
			showUnselectTabs();
		}

		public void PopState()
		{
			mOnClickEventSender_Determine.SetClickable(clickable: false);
			hideUnselectTabs();
		}

		private void Awake()
		{
			btnMng = GetComponent<UIButtonManager>();
			btnMng.setFocus(0);
			nowCategory = CategoryName.Wall;
			btnMng.setButtonDelegate(Util.CreateEventDelegate(this, "ChangeTabOnTouch", null));
			mOnClickEventSender_Determine.SetClickable(clickable: false);
		}

		public void Init(Action OnSelect, Action OnDeside)
		{
			OnSelectAction = OnSelect;
			OnDesideAction = OnDeside;
		}

		public void ChangeTabOnTouch()
		{
			if (nowCategory != (CategoryName)btnMng.nowForcusIndex)
			{
				OnSelectAction();
			}
			else
			{
				OnDesideAction();
			}
		}

		public void changeNowCategory()
		{
			nowCategory = (CategoryName)btnMng.nowForcusIndex;
		}

		public void InitTab()
		{
			btnMng.setFocus(0);
		}

		public void NextTab()
		{
			if (btnMng.moveNextButton())
			{
				OnSelectAction();
			}
		}

		public void PrevTab()
		{
			if (btnMng.movePrevButton())
			{
				OnSelectAction();
			}
		}

		public void setAllButtonEnable(bool isEnable)
		{
			btnMng.setAllButtonEnable(isEnable);
		}

		public void hideUnselectTabs()
		{
			mOnClickEventSender_Determine.SetClickable(clickable: false);
			UIButton[] focusableButtons = btnMng.GetFocusableButtons();
			UIButton[] array = focusableButtons;
			foreach (UIButton uIButton in array)
			{
				if (uIButton != btnMng.nowForcusButton)
				{
					TweenAlpha.Begin(uIButton.gameObject, 0.2f, 0.5f);
				}
			}
		}

		public void showUnselectTabs()
		{
			mOnClickEventSender_Determine.SetClickable(clickable: true);
			UIButton[] focusableButtons = btnMng.GetFocusableButtons();
			UIButton[] array = focusableButtons;
			foreach (UIButton uIButton in array)
			{
				if (uIButton != btnMng.nowForcusButton)
				{
					TweenAlpha.Begin(uIButton.gameObject, 0.2f, 1f);
				}
			}
		}

		public FurnitureKinds getFurnitureKinds()
		{
			switch (nowCategory)
			{
			case CategoryName.Wall:
				return FurnitureKinds.Wall;
			case CategoryName.Floor:
				return FurnitureKinds.Floor;
			case CategoryName.Chair:
				return FurnitureKinds.Desk;
			case CategoryName.Decoration:
				return FurnitureKinds.Hangings;
			case CategoryName.Window:
				return FurnitureKinds.Window;
			case CategoryName.Interior:
				return FurnitureKinds.Chest;
			default:
				return FurnitureKinds.Wall;
			}
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchDetermine()
		{
			ChangeTabOnTouch();
		}

		internal void ResumeState()
		{
			mOnClickEventSender_Determine.SetClickable(clickable: true);
			showUnselectTabs();
		}
	}
}
