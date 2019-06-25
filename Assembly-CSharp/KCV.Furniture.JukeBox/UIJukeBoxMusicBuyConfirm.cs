using DG.Tweening;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIButtonManager))]
	[RequireComponent(typeof(UIPanel))]
	public class UIJukeBoxMusicBuyConfirm : MonoBehaviour
	{
		private UIButtonManager mButtonManager;

		private UIPanel mPanelThis;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Coin_From;

		[SerializeField]
		private UILabel mLabel_Coin_To;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private Transform mTransform_Configuable;

		private Action mOnRequestBackToRoot;

		private Action mOnSelectPositiveListener;

		private Action mOnSelectNegativeListener;

		private UIButton mButtonCurrentFocus;

		private Action mOnRequestChangeScene;

		private KeyControl mKeyController;

		private bool mIsValidBuy;

		private UIButton[] mButtonFocasable;

		private Mst_bgm_jukebox mMst_bgm_jukebox;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0f;
			mButtonManager = GetComponent<UIButtonManager>();
			mButtonManager.IndexChangeAct = delegate
			{
				int num = Array.IndexOf(mButtonFocasable, mButtonManager.nowForcusButton);
				if (0 <= num)
				{
					ChangeFocus(mButtonManager.nowForcusButton);
				}
			};
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				ChangeFocus(mButton_Negative);
			}
			else if (mKeyController.keyState[10].down)
			{
				int num = Array.IndexOf(mButtonFocasable, mButton_Positive);
				if (0 <= num)
				{
					ChangeFocus(mButton_Positive);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				if (mButton_Negative.Equals(mButtonCurrentFocus))
				{
					OnClickNegative();
				}
				else if (mButton_Positive.Equals(mButtonCurrentFocus))
				{
					OnClickPositive();
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				OnClickNegative();
			}
			else if (mKeyController.IsRDown())
			{
				OnRequestChangeScene();
			}
			else if (mKeyController.IsLDown())
			{
				OnRequestBackToRoot();
			}
		}

		public void SetOnRequestBackToRoot(Action onRequestBackToRoot)
		{
			mOnRequestBackToRoot = onRequestBackToRoot;
		}

		private void OnRequestBackToRoot()
		{
			if (mOnRequestBackToRoot != null)
			{
				mOnRequestBackToRoot();
			}
		}

		public void Initialize(Mst_bgm_jukebox jukeBoxBGM, int walletInFurnitureCoin, bool isValidBuy)
		{
			mMst_bgm_jukebox = jukeBoxBGM;
			List<UIButton> list = new List<UIButton>();
			list.Add(mButton_Negative);
			mIsValidBuy = isValidBuy;
			mLabel_Name.text = jukeBoxBGM.Name;
			mLabel_Coin_From.text = walletInFurnitureCoin.ToString();
			mLabel_Coin_To.text = (walletInFurnitureCoin - jukeBoxBGM.R_coins).ToString();
			mLabel_Price.text = jukeBoxBGM.R_coins.ToString();
			if (mIsValidBuy)
			{
				list.Add(mButton_Positive);
				mButton_Positive.enabled = true;
				mButton_Positive.isEnabled = true;
			}
			else
			{
				mButton_Positive.enabled = false;
				mButton_Positive.isEnabled = false;
			}
			if (mMst_bgm_jukebox.Bgm_flag == 1)
			{
				mTransform_Configuable.gameObject.SetActive(true);
			}
			else
			{
				mTransform_Configuable.gameObject.SetActive(false);
			}
			mButtonFocasable = list.ToArray();
			mButtonManager.UpdateButtons(mButtonFocasable);
			ChangeFocus(mButton_Negative);
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void OnClickPositive()
		{
			SelectPositive();
		}

		private void OnClickNegative()
		{
			SelectNegative();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchPositive()
		{
			SelectPositive();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchNegative()
		{
			SelectNegative();
		}

		public void SetOnSelectPositiveListener(Action listener)
		{
			mOnSelectPositiveListener = listener;
		}

		public void SetOnSelectNegativeListener(Action listener)
		{
			mOnSelectNegativeListener = listener;
		}

		private void SelectPositive()
		{
			if (mOnSelectPositiveListener != null)
			{
				mOnSelectPositiveListener();
			}
		}

		private void SelectNegative()
		{
			if (mOnSelectNegativeListener != null)
			{
				mOnSelectNegativeListener();
			}
		}

		private void ChangeFocus(UIButton changeTarget)
		{
			if (mButtonCurrentFocus != null)
			{
				if (mButtonCurrentFocus.isEnabled)
				{
					mButtonCurrentFocus.SetState(UIButtonColor.State.Normal, immediate: true);
				}
				else
				{
					mButtonCurrentFocus.SetState(UIButtonColor.State.Disabled, immediate: true);
				}
			}
			mButtonCurrentFocus = changeTarget;
			if (mButtonCurrentFocus != null)
			{
				mButtonCurrentFocus.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		public void StartState()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			DOVirtual.Float(mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			}).SetId(this);
		}

		public void CloseState()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mButtonFocasable = null;
			mKeyController = null;
			mIsValidBuy = false;
			DOVirtual.Float(mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			}).SetId(this);
		}

		public void Release()
		{
			mButtonManager = null;
			mPanelThis = null;
			mButton_Negative.onClick.Clear();
			mButton_Negative = null;
			mButton_Positive.onClick.Clear();
			mButton_Positive = null;
			mKeyController = null;
			mIsValidBuy = false;
		}

		public void SetOnRequestChangeScene(Action onRequestChangeScene)
		{
			mOnRequestChangeScene = onRequestChangeScene;
		}

		private void OnRequestChangeScene()
		{
			if (mOnRequestChangeScene != null)
			{
				mOnRequestChangeScene();
			}
		}

		private void OnDestroy()
		{
			mButtonManager = null;
			mPanelThis = null;
			mButton_Negative = null;
			mButton_Positive = null;
			mLabel_Name = null;
			mLabel_Coin_From = null;
			mLabel_Coin_To = null;
			mLabel_Price = null;
			mTransform_Configuable = null;
			mOnRequestBackToRoot = null;
			mOnSelectPositiveListener = null;
			mOnSelectNegativeListener = null;
			mButtonCurrentFocus = null;
			mOnRequestChangeScene = null;
			mKeyController = null;
			mButtonFocasable = null;
			mMst_bgm_jukebox = null;
		}
	}
}
