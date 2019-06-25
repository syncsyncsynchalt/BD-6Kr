using DG.Tweening;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(UIButtonManager))]
	public class UIJukeBoxMusicPlayingDialog : MonoBehaviour
	{
		private UIButtonManager mButtonManager;

		private UIPanel mPanelThis;

		[SerializeField]
		private Transform mTransform_YouseiOffset;

		[SerializeField]
		private UITexture mTexture_Yousei;

		[SerializeField]
		private Texture mTexture2d_Yousei_Frame_0;

		[SerializeField]
		private Texture mTexture2d_Yousei_Frame_1;

		[SerializeField]
		private Texture mTexture2d_Yousei_Frame_2;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIJukeBoxMusicPlayingRollLabel mUIJukeBoxMusicPlayingRollLabel;

		private Action mOnRequestBackToRoot;

		private Action mOnSelectPositiveListener;

		private Action mOnSelectNegativeListener;

		private UIButton mButtonCurrentFocus;

		private Action mOnRequestChangeScene;

		private Mst_bgm_jukebox mMst_bgm_jukebox;

		private KeyControl mKeyController;

		private UIButton[] mButtonFocasable;

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

		public void Initialize(Mst_bgm_jukebox bgmJukeBoxModel)
		{
			mMst_bgm_jukebox = bgmJukeBoxModel;
			string title = $"「{mMst_bgm_jukebox.Name}」リクエスト中♪";
			mUIJukeBoxMusicPlayingRollLabel.Initialize(title);
			List<UIButton> list = new List<UIButton>();
			list.Add(mButton_Negative);
			if (mMst_bgm_jukebox.Bgm_flag == 1)
			{
				list.Add(mButton_Positive);
				mButton_Positive.SetState(UIButtonColor.State.Normal, immediate: true);
				mButton_Positive.enabled = true;
				mButton_Positive.isEnabled = true;
			}
			else
			{
				mButton_Positive.SetState(UIButtonColor.State.Disabled, immediate: true);
				mButton_Positive.enabled = false;
				mButton_Positive.isEnabled = false;
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
			if (mMst_bgm_jukebox.Bgm_flag == 1)
			{
				mButton_Positive.SetState(UIButtonColor.State.Normal, immediate: true);
				mButton_Positive.enabled = true;
				mButton_Positive.isEnabled = true;
			}
			else
			{
				mButton_Positive.SetState(UIButtonColor.State.Disabled, immediate: true);
				mButton_Positive.enabled = false;
				mButton_Positive.isEnabled = false;
			}
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			DOVirtual.Float(mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			}).SetId(this);
			mUIJukeBoxMusicPlayingRollLabel.StartRoll();
			GenerateTweenYouseiSwing();
			GenerateTweenYouseiMarch();
			GenerateTweenYouseiMove();
		}

		public void CloseState()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mButtonFocasable = null;
			mKeyController = null;
			DOVirtual.Float(mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			}).SetId(this);
		}

		private Tween GenerateTweenYouseiSwing()
		{
			TweenCallback action = delegate
			{
				mTexture_Yousei.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			};
			TweenCallback callback = delegate
			{
				mTexture_Yousei.transform.localRotation = Quaternion.Euler(Vector3.zero);
			};
			TweenCallback callback2 = delegate
			{
				mTexture_Yousei.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 6f));
			};
			Sequence sequence = DOTween.Sequence().SetId(this);
			sequence.OnPlay(action);
			sequence.AppendInterval(1f);
			sequence.AppendCallback(callback2);
			sequence.AppendInterval(1f);
			sequence.AppendCallback(callback);
			sequence.SetLoops(int.MaxValue, LoopType.Restart);
			return sequence;
		}

		private Tween GenerateTweenYouseiMove()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetId(this);
			TweenCallback action = delegate
			{
				mTransform_YouseiOffset.localPositionX(250f);
				mTransform_YouseiOffset.localRotation = Quaternion.Euler(Vector3.zero);
			};
			Tween t = mTransform_YouseiOffset.DOLocalMoveX(-380f, 5f).SetEase(Ease.Linear);
			Tween t2 = mTransform_YouseiOffset.DOLocalRotate(new Vector3(0f, 180f, 0f), 1f);
			Tween t3 = mTransform_YouseiOffset.DOLocalMoveX(250f, 5f).SetEase(Ease.Linear);
			Tween t4 = mTransform_YouseiOffset.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f);
			sequence.OnPlay(action);
			sequence.Append(t);
			sequence.Append(t2);
			sequence.Append(t3);
			sequence.Append(t4);
			sequence.SetLoops(int.MaxValue, LoopType.Restart);
			return sequence;
		}

		private Tween GenerateTweenYouseiMarch()
		{
			TweenCallback action = delegate
			{
				mTexture_Yousei.mainTexture = mTexture2d_Yousei_Frame_0;
			};
			TweenCallback callback = delegate
			{
				mTexture_Yousei.mainTexture = mTexture2d_Yousei_Frame_0;
			};
			TweenCallback callback2 = delegate
			{
				mTexture_Yousei.mainTexture = mTexture2d_Yousei_Frame_1;
			};
			TweenCallback callback3 = delegate
			{
				mTexture_Yousei.mainTexture = mTexture2d_Yousei_Frame_2;
			};
			Sequence sequence = DOTween.Sequence();
			sequence.SetId(this);
			sequence.OnPlay(action);
			sequence.AppendCallback(callback);
			sequence.AppendInterval(0.1f);
			sequence.AppendCallback(callback2);
			sequence.AppendInterval(0.1f);
			sequence.AppendCallback(callback3);
			sequence.AppendInterval(0.1f);
			sequence.SetLoops(int.MaxValue, LoopType.Restart);
			return null;
		}

		public void Release()
		{
			mTexture_Yousei.mainTexture = null;
			mTexture_Yousei = null;
			mTexture2d_Yousei_Frame_0 = null;
			mTexture2d_Yousei_Frame_1 = null;
			mTexture2d_Yousei_Frame_2 = null;
			mButtonManager = null;
			mPanelThis = null;
			mButton_Negative.onClick.Clear();
			mButton_Negative = null;
			mButton_Positive.onClick.Clear();
			mButton_Positive = null;
			mKeyController = null;
			mButtonFocasable = null;
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchYousei()
		{
			if (!DOTween.IsTweening(mTexture_Yousei))
			{
				mTransform_YouseiOffset.DOLocalMoveY(UnityEngine.Random.Range(55, 80), 0.5f).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo)
					.SetId(mTexture_Yousei);
			}
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
			mTransform_YouseiOffset = null;
			mTexture_Yousei = null;
			mTexture2d_Yousei_Frame_0 = null;
			mTexture2d_Yousei_Frame_1 = null;
			mTexture2d_Yousei_Frame_2 = null;
			mButton_Negative = null;
			mButton_Positive = null;
			mUIJukeBoxMusicPlayingRollLabel = null;
			mOnRequestBackToRoot = null;
			mOnSelectPositiveListener = null;
			mOnSelectNegativeListener = null;
			mButtonCurrentFocus = null;
			mOnRequestChangeScene = null;
			mMst_bgm_jukebox = null;
			mKeyController = null;
			mButtonFocasable = null;
		}
	}
}
