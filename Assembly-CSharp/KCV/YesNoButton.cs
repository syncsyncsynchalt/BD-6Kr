using KCV.Utils;
using System;
using UnityEngine;

namespace KCV
{
	public class YesNoButton : MonoBehaviour
	{
		[SerializeField]
		private UIButtonManager mUIButtonManager;

		private KeyControl mKeyController;

		private Action mOnSelectNegativeListener;

		private Action mOnSelectPositiveListener;

		private void Update()
		{
			if (mKeyController != null)
			{
				mKeyController.Update();
				if (mKeyController.keyState[14].down)
				{
					mUIButtonManager.movePrevButton();
				}
				else if (mKeyController.keyState[10].down)
				{
					mUIButtonManager.moveNextButton();
				}
				else if (mKeyController.keyState[1].down)
				{
					mUIButtonManager.Decide();
				}
				else if (mKeyController.keyState[0].down)
				{
					OnSelectNegative();
				}
			}
		}

		[Obsolete("Inspector上でボタンに設定する為に使用します")]
		public void OnTouchPositive()
		{
			OnSelectPositive();
		}

		[Obsolete("Inspector上でボタンに設定する為に使用します")]
		public void OnTouchNegative()
		{
			OnSelectNegative();
		}

		public void SetKeyController(KeyControl keyController, bool isFocusLeft = true)
		{
			mKeyController = keyController;
			App.OnlyController = mKeyController;
			int focus = (!isFocusLeft) ? 1 : 0;
			mUIButtonManager.setFocus(focus);
			mUIButtonManager.isPlaySE = true;
		}

		public void SetOnSelectNegativeListener(Action action)
		{
			mOnSelectNegativeListener = action;
		}

		public void SetOnSelectPositiveListener(Action action)
		{
			mOnSelectPositiveListener = action;
		}

		private void OnSelectNegative()
		{
			if (mOnSelectNegativeListener != null && mKeyController != null)
			{
				mKeyController = null;
				App.OnlyController = null;
				mOnSelectNegativeListener();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		private void OnSelectPositive()
		{
			if (mOnSelectPositiveListener != null && mKeyController != null)
			{
				mKeyController = null;
				App.OnlyController = null;
				mOnSelectPositiveListener();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		private void OnDestroy()
		{
			mKeyController = null;
			mUIButtonManager = null;
		}
	}
}
