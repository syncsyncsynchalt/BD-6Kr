using Common.Enum;
using KCV.View.Scroll;
using local.managers;
using local.models;
using System;
using System.Linq;

namespace KCV.InteriorStore
{
	public class InteriorStoreList : UIScrollListParent<FurnitureModel, InteriorStoreListChild>
	{
		private FurnitureKinds nowCategory;

		private FurnitureStoreManager manager;

		private Action mOnRequestChangeMode;

		protected override void OnAwake()
		{
			EnableBottomUpMode();
		}

		public void ChangeCategory(FurnitureKinds kinds)
		{
			if (nowCategory != kinds)
			{
				nowCategory = kinds;
				FurnitureModel[] storeItem = manager.GetStoreItem(kinds);
				if (Views == null)
				{
					Initialize(storeItem);
				}
				else
				{
					RefreshAndFirstFocus(storeItem);
				}
			}
		}

		public void Refresh()
		{
			FurnitureModel[] storeItem = manager.GetStoreItem(nowCategory);
			Refresh(storeItem);
		}

		public void setManager(FurnitureStoreManager manager)
		{
			this.manager = manager;
		}

		protected override void OnChildAction(ActionType actionType, UIScrollListChild<FurnitureModel> actionChild)
		{
			if (mKeyController == null)
			{
				OnRequestChangeMode();
			}
			if (actionType == ActionType.OnTouch && actionChild.Model != null && actionChild.mIsClickable)
			{
				ChangeFocusView((InteriorStoreListChild)actionChild, isFirstFocus: false);
				OnAction(actionType, this, (InteriorStoreListChild)actionChild);
			}
		}

		public void SetOnRequestChangeMode(Action onRequestChangeMode)
		{
			mOnRequestChangeMode = onRequestChangeMode;
		}

		private void OnRequestChangeMode()
		{
			if (mOnRequestChangeMode != null)
			{
				mOnRequestChangeMode();
			}
		}

		private FurnitureModel[] FurnitureFilter(FurnitureModel[] models, FurnitureKinds kinds)
		{
			int[] array = new int[3]
			{
				164,
				165,
				166
			};
			int[] array2 = new int[4]
			{
				1,
				2,
				4,
				5
			};
			int[] array3 = new int[4]
			{
				72,
				73,
				74,
				77
			};
			int[] array4 = new int[4]
			{
				102,
				103,
				104,
				105
			};
			int[] array5 = new int[4]
			{
				142,
				143,
				141,
				144
			};
			int[] array6 = new int[4]
			{
				38,
				39,
				40,
				41
			};
			int[] checkTarget = new int[0];
			switch (kinds)
			{
			case FurnitureKinds.Chest:
				checkTarget = array5;
				break;
			case FurnitureKinds.Floor:
				checkTarget = array2;
				break;
			case FurnitureKinds.Window:
				checkTarget = array3;
				break;
			case FurnitureKinds.Hangings:
				checkTarget = array4;
				break;
			case FurnitureKinds.Desk:
				checkTarget = array;
				break;
			case FurnitureKinds.Wall:
				checkTarget = array6;
				break;
			}
			return models.Where(delegate(FurnitureModel model)
			{
				int[] array7 = checkTarget;
				foreach (int num in array7)
				{
					if (num == model.MstId)
					{
						return true;
					}
				}
				return false;
			}).ToArray();
		}
	}
}
