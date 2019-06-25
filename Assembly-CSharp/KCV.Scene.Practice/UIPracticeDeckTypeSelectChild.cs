using Common.Enum;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIPracticeDeckTypeSelectChild : MonoBehaviour
	{
		[SerializeField]
		private DeckPracticeType mDeckPracticeType;

		[SerializeField]
		private UIButton mButton;

		private Action<UIPracticeDeckTypeSelectChild> mDeckPracticeTypeSelectedAction;

		public void Hover()
		{
			mButton.SetState(UIButtonColor.State.Hover, immediate: true);
		}

		public void RemoveHover()
		{
			mButton.SetState(UIButtonColor.State.Normal, immediate: true);
		}

		public void Enabled(bool isEnabled)
		{
			mButton.SetEnableCollider2D(isEnabled);
		}

		public DeckPracticeType GetDeckPracticeType()
		{
			return mDeckPracticeType;
		}

		public void SetOnClickListener(Action<UIPracticeDeckTypeSelectChild> deckPracticeTypeView)
		{
			mDeckPracticeTypeSelectedAction = deckPracticeTypeView;
		}

		public void OnClickView()
		{
			if (mDeckPracticeTypeSelectedAction != null)
			{
				mDeckPracticeTypeSelectedAction(this);
			}
		}

		public void ParentHasChanged()
		{
			mButton.GetSprite().ParentHasChanged();
		}

		public void Initialize()
		{
		}
	}
}
