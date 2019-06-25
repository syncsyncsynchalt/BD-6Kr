using Common.Enum;
using KCV.Scene.Others;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Interior
{
	public class UserInterfaceInteriorChangeManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			FurnitureKindsSelect,
			FurnitureSelect,
			FurnitureDetail,
			FurniturePreView
		}

		[SerializeField]
		private UIInteriorFurnitureKindSelector mUIInteriorChangeFurnitureSelector;

		[SerializeField]
		private UIInteriorFurnitureDetail mPrefab_UIInteriorFurnitureDetail;

		[SerializeField]
		private UIInteriorFurnitureChangeScrollListNew mPrefab_UIInteriorFurnitureChangeScrollListNew;

		private UIInteriorFurnitureDetail mUIInteriorFurnitureDetail;

		private UIInteriorFurnitureChangeScrollListNew mUIInteriorFurnitureChangeScrollList;

		[SerializeField]
		private UIInteriorFurniturePreviewWaiter mUIInteriorFurniturePreviewWaiter;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		[SerializeField]
		private Transform mTransform_MoveButton;

		private AudioClip mAudioClip_CommonEnter1;

		private AudioClip mAudioClip_CommonCancel1;

		private AudioClip mAudioClip_CommonCursolMove;

		private AudioClip mAudioClip_CommonEnter2;

		private KeyControl mKeyController;

		private UserInterfaceInteriorManager.StateManager<State> mStateManager;

		private int mDeckid;

		private InteriorManager mInteriorManager;

		private Context mContext;

		private Action mOnRequestMoveStore;

		private Camera mCamera_SwipeEventCatch;

		public string StateToString()
		{
			return mStateManager.ToString();
		}

		private void Back()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void Initialize(InteriorManager interiorManager)
		{
			mDeckid = interiorManager.Deck.Id;
			mInteriorManager = interiorManager;
			mUserInterfacePortInteriorManager.InitializeFurnituresForConfirmation(mInteriorManager.Deck, interiorManager.GetRoomInfo());
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void StartState()
		{
			mContext = new Context();
			mStateManager = new UserInterfaceInteriorManager.StateManager<State>(State.NONE);
			mStateManager.OnPop = OnPopState;
			mStateManager.OnPush = OnPushState;
			mStateManager.OnResume = OnResumeState;
			mStateManager.OnSwitch = OnSwitchState;
			mStateManager.PushState(State.FurnitureKindsSelect);
		}

		public void Release()
		{
			mUIInteriorFurnitureChangeScrollList.SetKeyController(null);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_CommonEnter1);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_CommonCancel1);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_CommonCursolMove);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_CommonEnter2);
			mKeyController = null;
			mContext = null;
			mStateManager = null;
			mUIInteriorChangeFurnitureSelector = null;
			mUIInteriorFurnitureDetail = null;
			mUIInteriorFurnitureChangeScrollList = null;
			mUIInteriorFurniturePreviewWaiter = null;
			mUserInterfacePortInteriorManager = null;
		}

		public void Clean()
		{
			mKeyController = null;
			mContext = null;
			mStateManager = null;
			if (mUIInteriorChangeFurnitureSelector != null)
			{
				mUIInteriorChangeFurnitureSelector.SetKeyController(null);
			}
			if (mUIInteriorFurnitureChangeScrollList != null)
			{
				mUIInteriorFurnitureChangeScrollList.SetKeyController(null);
			}
			if (mUIInteriorFurnitureDetail != null)
			{
				mUIInteriorFurnitureDetail.SetKeyController(null);
			}
			if (mUIInteriorFurniturePreviewWaiter != null)
			{
				mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
			}
		}

		private void Start()
		{
			mAudioClip_CommonEnter1 = SoundFile.LoadSE(SEFIleInfos.CommonEnter1);
			mAudioClip_CommonCancel1 = SoundFile.LoadSE(SEFIleInfos.CommonCancel1);
			mAudioClip_CommonCursolMove = SoundFile.LoadSE(SEFIleInfos.CommonCursolMove);
			mAudioClip_CommonEnter2 = SoundFile.LoadSE(SEFIleInfos.CommonEnter2);
			mUIInteriorChangeFurnitureSelector.SetOnSelectFurnitureKindListener(OnSelectFurnitureKindListener);
			mUIInteriorChangeFurnitureSelector.SetOnSelectCancelListener(OnSelectCancelListener);
			mUIInteriorFurniturePreviewWaiter.SetOnBackListener(OnFinishedPreview);
		}

		private void OnBackListener()
		{
			if (mStateManager.CurrentState == State.FurnitureSelect)
			{
				SoundUtils.PlaySE(mAudioClip_CommonCancel1);
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnChanedItemListener(UIInteriorFurnitureChangeScrollListChildNew child)
		{
			if (mStateManager.CurrentState == State.FurnitureSelect)
			{
				SoundUtils.PlaySE(mAudioClip_CommonCursolMove);
				mUIInteriorFurnitureDetail.Initialize(mDeckid, child.GetModel().GetFurnitureModel());
			}
		}

		private void OnSelectedListener(UIInteriorFurnitureChangeScrollListChildNew child)
		{
			if (mStateManager.CurrentState == State.FurnitureSelect)
			{
				mContext.SetSelectedFurniture(child.GetModel().GetFurnitureModel());
				mUIInteriorFurnitureChangeScrollList.LockControl();
				mStateManager.PushState(State.FurnitureDetail);
				SoundUtils.PlaySE(mAudioClip_CommonEnter1);
			}
		}

		private void Update()
		{
			if (mKeyController != null && mKeyController.IsRSRightDown() && mStateManager.CurrentState == State.FurnitureKindsSelect)
			{
				RequestMoveStore();
			}
		}

		private void OnSelectPreviewListener()
		{
			mUIInteriorFurnitureDetail.SetKeyController(null);
			mStateManager.PushState(State.FurniturePreView);
			SoundUtils.PlaySE(mAudioClip_CommonEnter2);
		}

		private void OnSelectChangeListener()
		{
			ChangeFurniture(mContext.CurrentCategory, mContext.SelectedFurniture);
			FurnitureModel furniture = mInteriorManager.GetFurniture(mContext.CurrentCategory, mContext.SelectedFurniture.MstId);
			mContext.SetSelectedFurniture(furniture);
			mUIInteriorFurnitureDetail.Initialize(mDeckid, mContext.SelectedFurniture);
			SoundUtils.PlaySE(mAudioClip_CommonEnter1);
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		private void OnBackDetailListener()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIInteriorFurnitureDetail.QuitState();
			mStateManager.PopState();
			mStateManager.ResumeState();
			SoundUtils.PlaySE(mAudioClip_CommonCancel1);
		}

		private void OnSelectFurnitureKindListener(FurnitureKinds furnitureKind)
		{
			if (mStateManager.CurrentState == State.FurnitureKindsSelect)
			{
				mContext.SetSelectFurnitureKind(furnitureKind);
				mUIInteriorChangeFurnitureSelector.SetKeyController(null);
				mStateManager.PushState(State.FurnitureSelect);
			}
		}

		private void OnSelectCancelListener()
		{
			Back();
		}

		private void OnPopState(State state)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			switch (state)
			{
			case State.FurnitureDetail:
				break;
			case State.FurnitureSelect:
				mUIInteriorFurnitureDetail.Hide();
				mUIInteriorFurnitureChangeScrollList.LockControl();
				mUIInteriorFurnitureChangeScrollList.Hide();
				break;
			case State.FurniturePreView:
				mTransform_MoveButton.SetActive(isActive: true);
				break;
			}
		}

		private void OnPushStateFurnitureSelect()
		{
			StartCoroutine(OnPushStateFurnitureSelectCoroutine());
		}

		private IEnumerator OnPushStateFurnitureSelectCoroutine()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			FurnitureModel[] furnitureModels = mInteriorManager.GetFurnitures(mContext.CurrentCategory);
			if (mUIInteriorFurnitureChangeScrollList == null)
			{
				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Reset();
				stopWatch.Start();
				mUIInteriorFurnitureChangeScrollList = Util.Instantiate(mPrefab_UIInteriorFurnitureChangeScrollListNew.gameObject, base.gameObject).GetComponent<UIInteriorFurnitureChangeScrollListNew>();
				mUIInteriorFurnitureChangeScrollList.SetActive(isActive: true);
				mUIInteriorFurnitureChangeScrollList.SetOnSelectedListener(OnSelectedListener);
				mUIInteriorFurnitureChangeScrollList.SetOnChangedItemListener(OnChanedItemListener);
				mUIInteriorFurnitureChangeScrollList.SetOnBackListener(OnBackListener);
				stopWatch.Stop();
				for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			mUIInteriorFurnitureChangeScrollList.Initialize(mDeckid, mContext.CurrentCategory, furnitureModels, mCamera_SwipeEventCatch);
			StartCoroutine(ShowFurnitureSelecter());
		}

		private void OnPushState(State state)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			switch (state)
			{
			case State.FurnitureKindsSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mUIInteriorChangeFurnitureSelector.Initialize();
				mUIInteriorChangeFurnitureSelector.SetKeyController(mKeyController);
				mUIInteriorChangeFurnitureSelector.StartState();
				break;
			case State.FurnitureSelect:
				OnPushStateFurnitureSelect();
				break;
			case State.FurnitureDetail:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mUIInteriorFurnitureDetail.SetKeyController(mKeyController);
				mUIInteriorFurnitureDetail.StartState();
				break;
			case State.FurniturePreView:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mTransform_MoveButton.SetActive(isActive: false);
				mUIInteriorChangeFurnitureSelector.transform.localScale = Vector3.zero;
				mUIInteriorFurnitureChangeScrollList.transform.localScale = Vector3.zero;
				mUIInteriorFurnitureDetail.transform.localScale = Vector3.zero;
				mKeyController.ClearKeyAll();
				mKeyController.firstUpdate = true;
				mUIInteriorFurniturePreviewWaiter.SetKeyController(mKeyController);
				mUIInteriorFurniturePreviewWaiter.StartWait();
				PreviewFurniture(mContext.CurrentCategory, mContext.SelectedFurniture);
				break;
			}
		}

		private IEnumerator ShowFurnitureSelecter()
		{
			if (mUIInteriorFurnitureDetail == null)
			{
				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Reset();
				stopWatch.Start();
				mUIInteriorFurnitureDetail = Util.Instantiate(mPrefab_UIInteriorFurnitureDetail.gameObject, base.gameObject).GetComponent<UIInteriorFurnitureDetail>();
				mUIInteriorFurnitureDetail.SetOnSelectBackListener(OnBackDetailListener);
				mUIInteriorFurnitureDetail.SetOnSelectChangeListener(OnSelectChangeListener);
				mUIInteriorFurnitureDetail.SetOnSelectPreviewListener(OnSelectPreviewListener);
				stopWatch.Stop();
				for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			yield return new WaitForEndOfFrame();
			mUIInteriorFurnitureDetail.Show();
			mUIInteriorFurnitureChangeScrollList.SetActive(isActive: false);
			mUIInteriorFurnitureChangeScrollList.SetActive(isActive: true);
			mUIInteriorFurnitureChangeScrollList.Show();
			mUIInteriorFurnitureChangeScrollList.SetKeyController(mKeyController);
			mUIInteriorFurnitureChangeScrollList.StartControl();
		}

		private void OnFinishedPreview()
		{
			if (mStateManager.CurrentState == State.FurniturePreView)
			{
				SoundUtils.PlaySE(mAudioClip_CommonCursolMove);
				mUIInteriorChangeFurnitureSelector.transform.localScale = Vector3.one;
				mUIInteriorFurnitureChangeScrollList.transform.localScale = Vector3.one;
				mUIInteriorFurnitureDetail.transform.localScale = Vector3.one;
				mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
				PreviewFurniture(mContext.CurrentCategory, mInteriorManager.GetRoomInfo()[mContext.CurrentCategory]);
				mStateManager.PopState();
				mStateManager.ResumeState();
			}
		}

		private void OnSwitchState(State state)
		{
		}

		private void OnResumeState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.FurnitureKindsSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mUIInteriorChangeFurnitureSelector.SetKeyController(mKeyController);
				mUIInteriorChangeFurnitureSelector.ResumeState();
				break;
			case State.FurnitureSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mUIInteriorFurnitureChangeScrollList.ResumeControl();
				break;
			case State.FurnitureDetail:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mUIInteriorFurnitureDetail.SetKeyController(mKeyController);
				mUIInteriorFurnitureDetail.ResumeState();
				break;
			}
		}

		private void ChangeFurniture(FurnitureKinds furnitureKind, FurnitureModel furnitureModel)
		{
			if (mStateManager.CurrentState == State.FurnitureDetail)
			{
				new Dictionary<FurnitureKinds, int>();
				if (mInteriorManager.ChangeRoom(furnitureKind, furnitureModel.MstId))
				{
					mUserInterfacePortInteriorManager.UpdateFurniture(mInteriorManager.Deck, furnitureKind, furnitureModel);
					mUIInteriorFurnitureChangeScrollList.RefreshViews();
					mUIInteriorFurnitureDetail.QuitState();
				}
			}
		}

		private void PreviewFurniture(FurnitureKinds furnitureKind, FurnitureModel furnitureModel)
		{
			if (mStateManager.CurrentState == State.FurniturePreView)
			{
				mUserInterfacePortInteriorManager.UpdateFurniture(mInteriorManager.Deck, furnitureKind, furnitureModel);
			}
		}

		public void SetOnRequestMoveToFurnitureStoreListener(Action onRequestMoveStore)
		{
			mOnRequestMoveStore = onRequestMoveStore;
		}

		private void RequestMoveStore()
		{
			if (mOnRequestMoveStore != null)
			{
				mOnRequestMoveStore();
			}
		}

		[Obsolete("Inspector上に設定して使用します")]
		public void OnTouchMoveToFurnitureStore()
		{
			if (mStateManager != null && mStateManager.CurrentState == State.FurnitureKindsSelect)
			{
				RequestMoveStore();
			}
		}

		private void OnDestroy()
		{
			mUIInteriorChangeFurnitureSelector = null;
			mUIInteriorFurnitureDetail = null;
			mUIInteriorFurnitureChangeScrollList = null;
			mUIInteriorFurniturePreviewWaiter = null;
			mUserInterfacePortInteriorManager = null;
			mTransform_MoveButton = null;
			mAudioClip_CommonEnter1 = null;
			mAudioClip_CommonCancel1 = null;
			mAudioClip_CommonCursolMove = null;
			mAudioClip_CommonEnter2 = null;
			mKeyController = null;
			mStateManager = null;
			mInteriorManager = null;
			mContext = null;
		}

		internal void SetSwipeEventCamera(Camera swipeEventCatch)
		{
			mCamera_SwipeEventCatch = swipeEventCatch;
		}
	}
}
