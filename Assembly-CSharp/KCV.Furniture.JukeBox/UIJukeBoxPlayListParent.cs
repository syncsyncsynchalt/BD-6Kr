using KCV.View.ScrollView;
using local.managers;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	public class UIJukeBoxPlayListParent : UIScrollList<Mst_bgm_jukebox, UIJukeBoxPlayListChild>
	{
		[SerializeField]
		private UILabel mLabel_WalletInCoin;

		private KeyControl mKeyController;

		private ManagerBase mManager;

		private Action mOnRequestBackToRoot;

		private Action mOnBackListener;

		private Action mOnRequestChangeScene;

		private Action<Mst_bgm_jukebox> mOnSelectedMusicListener;

		public void Initialize(ManagerBase manager, Mst_bgm_jukebox[] models, Camera camera)
		{
			mManager = manager;
			base.ChangeImmediateContentPosition(ContentDirection.Hell);
			Initialize(models);
			SetSwipeEventCamera(camera);
		}

		protected override void OnUpdate()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[8].down)
				{
					PrevFocus();
				}
				else if (mKeyController.keyState[12].down)
				{
					NextFocus();
				}
				else if (mKeyController.keyState[14].down)
				{
					PrevPageOrHeadFocus();
				}
				else if (mKeyController.keyState[10].down)
				{
					NextPageOrTailFocus();
				}
				else if (mKeyController.keyState[1].down)
				{
					Select();
				}
				else if (mKeyController.keyState[0].down)
				{
					OnBack();
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

		public void Refresh(ManagerBase manager, Mst_bgm_jukebox[] models, Camera camera)
		{
			Initialize(manager, models, camera);
		}

		protected override bool OnSelectable(UIJukeBoxPlayListChild view)
		{
			return true;
		}

		protected override void OnSelect(UIJukeBoxPlayListChild view)
		{
			mOnSelectedMusicListener(view.GetModel());
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchNext()
		{
			NextPageOrTailFocus();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchPrev()
		{
			PrevPageOrHeadFocus();
		}

		private void OnBack()
		{
			if (mOnBackListener != null)
			{
				mOnBackListener();
			}
		}

		public void SetOnBackListener(Action onBackListener)
		{
			mOnBackListener = onBackListener;
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

		public void SetOnSelectedMusicListener(Action<Mst_bgm_jukebox> onSelectedMusicListener)
		{
			mOnSelectedMusicListener = onSelectedMusicListener;
		}

		public void StartState()
		{
			UpdateWalletInCoin();
			HeadFocus();
			StartControl();
		}

		public void ResumeState()
		{
			UpdateWalletInCoin();
			ResumeFocus();
		}

		public void LockState()
		{
			LockControl();
		}

		public void UpdateWalletInCoin()
		{
			mLabel_WalletInCoin.text = mManager.UserInfo.FCoin.ToString();
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void OnSelect(Mst_bgm_jukebox selectedJukeBoxBGM)
		{
			if (mOnSelectedMusicListener != null)
			{
				mOnSelectedMusicListener(selectedJukeBoxBGM);
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchBack()
		{
			OnBack();
		}

		internal void CloseState()
		{
			OnBack();
		}

		protected override void OnCallDestroy()
		{
			mLabel_WalletInCoin = null;
			mKeyController = null;
			mManager = null;
			mOnRequestBackToRoot = null;
			mOnBackListener = null;
			mOnRequestChangeScene = null;
			mOnSelectedMusicListener = null;
		}
	}
}
