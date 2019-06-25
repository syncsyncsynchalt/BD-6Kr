using Common.Enum;
using KCV.Base;
using KCV.Display;
using KCV.Interface;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Formation
{
	public class UIBattleFormationKindSelector : UICircleMenu<BattleFormationKinds1, UIBattleFormationKind>, IUIKeyControllable
	{
		public enum ActionType
		{
			Select,
			SelectAuto,
			OnNextChanged,
			OnPrevChanged
		}

		public delegate void UIBattleFormationKindSelectorAction(ActionType actionType, UIBattleFormationKind centerObject);

		private const int MIN_FORMATION = 1;

		private UIBattleFormationKindSelectorAction mUIBattleFormationKindSelectorAction;

		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		private KeyControl mKeyController;

		public void Initialize(BattleFormationKinds1[] kinds)
		{
			if (1 < kinds.Length)
			{
				Initialize(kinds, 3);
			}
			else
			{
				CallBackAction(ActionType.SelectAuto, null);
			}
		}

		public void SetOnUIBattleFormationKindSelectorAction(UIBattleFormationKindSelectorAction callBackMethod)
		{
			mUIBattleFormationKindSelectorAction = callBackMethod;
		}

		protected override void InitOriginalDefaultPosition(ref Vector3[] defaultPositions)
		{
			base.InitOriginalDefaultPosition(ref defaultPositions);
			defaultPositions[defaultPositions.Length - 1] += new Vector3(0f, 75f);
			defaultPositions[1] += new Vector3(0f, 75f);
			defaultPositions[defaultPositions.Length - 2] = new Vector3(-700f, 350f);
			defaultPositions[2] += new Vector3(700f, 200f);
		}

		public new void Prev()
		{
			if (!base.mIsAnimationNow)
			{
				base.Prev();
				CallBackAction(ActionType.OnPrevChanged, mCenterView);
			}
		}

		public new void Next()
		{
			if (!base.mIsAnimationNow)
			{
				base.Next();
				CallBackAction(ActionType.OnNextChanged, mCenterView);
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchSelectCenter()
		{
			SelectCenter();
		}

		public void SelectCenter()
		{
			if (!base.mIsAnimationNow)
			{
				mKeyController = null;
				if (mUIDisplaySwipeEventRegion != null)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					mUIDisplaySwipeEventRegion.enabled = false;
					mUIDisplaySwipeEventRegion = null;
					mCenterView.OnSelectAnimation(delegate
					{
						CallBackAction(ActionType.Select, mCenterView);
						base.mIsAnimationNow = false;
					});
				}
			}
		}

		private void CallBackAction(ActionType actionType, UIBattleFormationKind centerObject)
		{
			if (mUIBattleFormationKindSelectorAction != null)
			{
				mUIBattleFormationKindSelectorAction(actionType, centerObject);
			}
		}

		public void SetKeyController(KeyControl keyController, UIDisplaySwipeEventRegion displaySwipeEventRegion)
		{
			mKeyController = keyController;
			mUIDisplaySwipeEventRegion = displaySwipeEventRegion;
		}

		public void OnUpdatedKeyController()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[4].down || mKeyController.keyState[14].down)
				{
					Debug.Log("PRev");
					Prev();
				}
				else if (mKeyController.keyState[5].down || mKeyController.keyState[10].down)
				{
					Next();
				}
				else if (mKeyController.keyState[1].down)
				{
					SelectCenter();
				}
			}
		}

		public void OnReleaseKeyController()
		{
			mKeyController = null;
			mUIDisplaySwipeEventRegion = null;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void OnDestroy()
		{
			mUIBattleFormationKindSelectorAction = null;
			mUIDisplaySwipeEventRegion = null;
			mKeyController = null;
		}
	}
}
